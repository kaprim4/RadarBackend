using AutoMapper;
using Domain.DTOs;
using Domain.Helper;
using Domain.Models;
using Domain.Models.Data;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Services.ImageService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Services.DataService
{
    public class DataService : AService,IDataService
    {
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        public DataService(AppDbContext context, IMapper mapper, IImageService imageService) : base(context)
        {
            _mapper= mapper;
            _imageService= imageService;
        }

        public async Task<ResponseModel<LotListDTO>> List(PagableDTO<LotListDTO> pagable)
        {
            var output = new ResponseModel<LotListDTO>();
            try
            {
                // Include related entities (e.g., Device and Documents)
                var query = _context.Lots
                    .Include(x => x.Device)
                    .Include(x => x.Documents)
                    .AsQueryable();

                // Apply filtering and search
                if (!string.IsNullOrEmpty(pagable.SearchTerm))
                {
                    query = query.Where(lot =>
                        lot.Reference.Contains(pagable.SearchTerm) ||
                        (lot.Device != null && lot.Device.SerialNumber.Contains(pagable.SearchTerm)) 
                    );
                }

                // Total count for pagination
                output.Pagable.TotalContent = await query.CountAsync();

                // Apply pagination and map to DTO
                var filteredQuery = query
                    .Skip((pagable.Page - 1) * pagable.Length)
                    .Take(pagable.Length);

                output.Pagable.Content = await filteredQuery
                    .Select(x => new LotListDTO
                    {
                        Id = x.Id,
                        CreatedDate = x.CreatedAt.ToString("dd-MM-yyyy"),
                        NbFiles = x.Documents.Count,
                        Reference = x.Reference
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                output.AddErrorMessage(ex.ToString());
            }

            return output;
        }



        public async Task<LotDTO?> GetById(int id)
        {
            var treatment = await _context.Lots
                .Select(x=> new LotDTO()
                {
                    Id = x.Id,
                    Documents = x.Documents.Select(document=> new DocumentDTO()
                    {
                        Id = document.Id,
                        Name = document.Name,
                        Path = document.Path,
                        Reference = document.Reference,
                        Jmx = new JMXDTO()
                        {
                            Id = document.Jmx.Id,
                            Reference = document.Jmx.Reference,
                            DeploymentSummary = new DeploymentSummaryDTO() {
                                Id = document.Jmx.DeploymentSummary.Id,
                                CameraName = document.Jmx.DeploymentSummary.CameraName,
                                LocationCode = document.Jmx.DeploymentSummary.LocationCode,
                                Location = document.Jmx.DeploymentSummary.Location,
                                DeploymentId = document.Jmx.DeploymentSummary.DeploymentId,
                                StartSequence = document.Jmx.DeploymentSummary.StartSequence,
                                EndSequence = document.Jmx.DeploymentSummary.EndSequence,
                                SpeedLimit = document.Jmx.DeploymentSummary.SpeedLimit,
                                CaptureSpeed = document.Jmx.DeploymentSummary.CaptureSpeed,
                                MeasurementUnit = document.Jmx.DeploymentSummary.MeasurementUnit,
                                OperatorId = document.Jmx.DeploymentSummary.OperatorId,
                                OperatorName = document.Jmx.DeploymentSummary.OperatorName,
                                StartDtm = document.Jmx.DeploymentSummary.StartDtm,
                                EndDtm = document.Jmx.DeploymentSummary.EndDtm
                            },
                            VehicleDatas = document.Jmx.VehicleDatas.Select(v=> new VehicleDataDTO()
                            {
                                Id = v.Id,
                                CrosshairX = v.CrosshairX,
                                CrosshairY = v.CrosshairY,
                                Image = v.Image,
                                Sequence = v.Sequence,
                                Timestamp = v.Timestamp,
                                Jmx = v.Jmx,
                                Txt = v.Txt,
                                VehicleSpeed = v.VehicleSpeed
                            }).ToList()
                        }
                    }).ToList(),
                    Reference = x.Reference,
                    CreateDate = x.CreatedAt.ToString("dd/MM/yyyy"),
                    UpdatedAt = x.LastUpdatedAt.HasValue ? x.LastUpdatedAt.Value.ToString("dd/MM/yyyy") : null,
                    Device = new DeviceDTO()
                    {
                        Id = x.Device_Id,
                        IsActive = x.Device.IsActive,
                        Name = x.Device.Name,
                        SerialNumber = x.Device.SerialNumber,
                        CreatedDate = x.Device.CreatedAt.ToString("dd/MM/yyyy")
                    }
                })
            .FirstOrDefaultAsync(t => t.Id == id);

            return treatment == null ? null : _mapper.Map<LotDTO>(treatment);
        }

        public async Task<ResponseModel<LotDTO>> Add(LotDTO treatmentDto)
        {
            ResponseModel<LotDTO> output = new(true);
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                //foreach (var deployment in treatmentDto.Deployments)
                //{
                //    if (deployment.VehicleDatas != null && deployment.VehicleDatas.Any())
                //    {
                //        foreach (var vehicle in deployment.VehicleDatas)
                //        {
                //            // Construct paths for image and video
                //            string? imagePath = vehicle.Image != null ? Path.Combine(ImageHelper.GetResourcePathStatic(false), vehicle.Image) : null;
                //            string? videoPath = vehicle.Video != null ? Path.Combine(ImageHelper.GetResourcePathStatic(true), vehicle.Video) : null;

                //            // Add image/video and get the image ID
                //            string imageId = await _imageService.AddImage(imagePath, videoPath);
                //            if (imageId == null)
                //            {
                //                output.AddErrorMessage("Error Adding Image");
                //                return output;
                //            }

                //            vehicle.Image_Name = imageId;

                //        }

                //    }
                //}

                var LotEntity = _mapper.Map<Lot>(treatmentDto);

                var serialNumbersList = LotEntity.Documents.Select(x => x.Jmx.DeploymentSummary.CameraName).Distinct().ToList();
                foreach (var serialNumber in serialNumbersList)
                {
                    var mainDevice = new Device();
                    var device = _context.Devices.FirstOrDefault(x => x.SerialNumber == serialNumber);
                    if (device == null)
                    {
                        mainDevice = new Device()
                        {
                            SerialNumber = serialNumber,
                            Name = GenerateRadarRefrence()
                        };
                        _context.Devices.Add(mainDevice);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        mainDevice = device;
                    }
                        

                    var lot = new Lot
                    {
                        Reference = GenerateLotRefrence(),
                        Device_Id = mainDevice.Id
                    };
                    _context.Lots.Add(lot);
                    await _context.SaveChangesAsync();

                    foreach (var item in LotEntity.Documents.Where(x => x.Jmx.DeploymentSummary.CameraName == serialNumber).ToList())
                    {
                        var document = new Document
                        {
                            Reference = GenerateDocumentRefrence(),
                            Name = item.Name,
                            Path = "",
                            Lot_Id = lot.Id,
                        };
                        _context.Documents.Add(document);
                        await _context.SaveChangesAsync();

                        var jmx = new JMX
                        {
                            Reference = GenerateJMXRefrence(),
                            DeploymentSummary = item.Jmx.DeploymentSummary,
                            VehicleDatas = item.Jmx.VehicleDatas,
                            Document_Id = document.Id
                        };
                        _context.Jmxes.Add(jmx);
                        await _context.SaveChangesAsync();

                        document.Jmx_Id = jmx.Id;
                        await _context.SaveChangesAsync();
                    }
                }

                
               
                await transaction.CommitAsync();
                //_context.Lots.Add(LotEntity);
                //await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                output.AddErrorMessage(ex.ToString());
                await transaction.RollbackAsync();
            }

            return output;
        }


        public async Task<ResponseModel<LotDTO>> Update(LotDTO treatmentDto)
        {
            ResponseModel<LotDTO> output = new(true);
            var treatement = await _context.Jmxes.FirstOrDefaultAsync(x=>x.Id == treatmentDto.Id);
            if (treatement != null)
            {
                try
                {
                    var treatmentEntity = _mapper.Map<JMX>(treatmentDto);

                    _context.Jmxes.Update(treatmentEntity);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    output.AddErrorMessage(ex.ToString());
                }
            }
            else
            {
                output.AddErrorMessage("Traintement introuvable");
            }


            return output;
        }

        public async Task<ResponseModel<LotDTO>> Delete(int id)
        {
            ResponseModel<LotDTO> output = new(true);
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Retrieve the Lot entity along with related data
                var lot = await _context.Lots
                        .Include(l => l.Documents)
                            .ThenInclude(d => d.Jmx)
                                .ThenInclude(j => j.VehicleDatas)
                        .Include(l => l.Documents)
                            .ThenInclude(d => d.Jmx)
                                .ThenInclude(j => j.DeploymentSummary)
                        .FirstOrDefaultAsync(l => l.Id == id);

                if (lot == null)
                {
                    output.AddErrorMessage("Lot not found.");
                    return output;
                }

                // Delete related JMX entities
                foreach (var document in lot.Documents)
                {
                    if (document.Jmx != null)
                    {
                        _context.DeploymentSummaries.Remove(document.Jmx.DeploymentSummary);
                        _context.VehicleDatas.RemoveRange(document.Jmx.VehicleDatas);
                        _context.Jmxes.Remove(document.Jmx);
                    }
                }

                // Delete related Document entities
                _context.Documents.RemoveRange(lot.Documents);

                // Delete the Lot entity
                _context.Lots.Remove(lot);

                // Save changes and commit the transaction
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();


            }
            catch (Exception ex)
            {
                output.AddErrorMessage(ex.ToString());
                await transaction.RollbackAsync();
            }

            return output;
        }
        
        
        public async Task<ResponseModel<LotDTO>> DeleteDocument(int id)
        {
            ResponseModel<LotDTO> output = new(true);
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Retrieve the Lot entity along with related data
                var document = await _context.Documents
                    .Include(x=>x.Jmx)
                    .Include(x=>x.Jmx.DeploymentSummary)
                    .Include(x=>x.Jmx.VehicleDatas)
                    .FirstOrDefaultAsync(l => l.Id == id);

                if (document == null)
                {
                    output.AddErrorMessage("Document not found.");
                    return output;
                }

                _context.DeploymentSummaries.Remove(document.Jmx.DeploymentSummary);
                _context.VehicleDatas.RemoveRange(document.Jmx.VehicleDatas);
                _context.Jmxes.Remove(document.Jmx);

                
                _context.Documents.RemoveRange(document);


                await _context.SaveChangesAsync();
                await transaction.CommitAsync();


            }
            catch (Exception ex)
            {
                output.AddErrorMessage(ex.ToString());
                await transaction.RollbackAsync();
            }

            return output;
        }

        public string GenerateLotRefrence()
        {
            var lastLotId = _context.Lots.Max(x => (int?)x.Id) ?? 0;
            lastLotId++;
            return $"L-{lastLotId.ToString().PadLeft(6, '0')}-{DateTime.Now:ddMMyy}";
        }
        public string GenerateDocumentRefrence()
        {
            var lastLotId = _context.Documents.Max(x => (int?)x.Id) ?? 0;
            lastLotId++;
            return $"D-{lastLotId.ToString().PadLeft(6, '0')}-{DateTime.Now:ddMMyy}";
        }
        public string GenerateJMXRefrence()
        {
            var lastLotId = _context.Jmxes.Max(x => (int?)x.Id) ?? 0;
            lastLotId++;
            return $"J-{lastLotId.ToString().PadLeft(6, '0')}-{DateTime.Now:ddMMyy}";
        }
        
        public string GenerateRadarRefrence()
        {
            var lastLotId = _context.Devices.Max(x => (int?)x.Id) ?? 0;
            lastLotId++;
            return $"R-{lastLotId.ToString().PadLeft(6, '0')}-{DateTime.Now:ddMMyy}";
        }
        

        public async Task<List<FileCheckDTO>> CheckFiles(List<string> files)
        {
            var output = new List<FileCheckDTO>();

            try
            {
                var ValidFiles = _context.Documents.Where(x => files.Contains(x.Name)).Select(x=> x.Name).Distinct().Select(x => new FileCheckDTO()
                {
                    CanTreat = false,
                    Name = x
                }).ToList();

                foreach (var file in files) {
                    if (!ValidFiles.Any(x=>x.Name == file))
                        ValidFiles.Add(new FileCheckDTO()
                        {
                            CanTreat = true,
                            Name = file
                        });
                }

                return ValidFiles;
            }
            catch (Exception ex)
            {
                //output.AddErrorMessage($"{ex}");
            }

            return output;
        }

        public async Task<ResponseModel<DocumentDTO>> AddDocument(DocumentDTO dto)
        {
            var output = new ResponseModel<DocumentDTO>();
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var lot = _context.Lots.FirstOrDefault(x => x.Id == dto.LotId);
                if (lot != null)
                {
                    var document = new Document
                    {
                        Reference = GenerateDocumentRefrence(),
                        Name = dto.Name,
                        Path = "",
                        Lot_Id = lot.Id,
                    };
                    _context.Documents.Add(document);
                    await _context.SaveChangesAsync();

                    var jmx = new JMX
                    {
                        Reference = GenerateJMXRefrence(),
                        DeploymentSummary = _mapper.Map<DeploymentSummary>(dto.Jmx.DeploymentSummary),
                        VehicleDatas = _mapper.Map<List<VehicleData>>(dto.Jmx.VehicleDatas),
                        Document_Id = document.Id
                    };
                    _context.Jmxes.Add(jmx);
                    await _context.SaveChangesAsync();

                    document.Jmx_Id = jmx.Id;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    output.AddErrorMessage($"Lot introuvable !");
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                output.AddErrorMessage($"{ex}");
            }

            return output;
        }
    }
}
