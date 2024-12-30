using Domain.DTOs;
using Domain.Helper;
using Domain.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DataService
{
    public interface IDataService
    {
        Task<ResponseModel<LotListDTO>> List(PagableDTO<LotListDTO> pagable);
        Task<LotDTO> GetById(int id);
        Task<ResponseModel<LotDTO>> Add(LotDTO device);
        Task<ResponseModel<LotDTO>> Update(LotDTO updatedDevice);
        Task<ResponseModel<LotDTO>> Delete(int id);

        Task<List<FileCheckDTO>> CheckFiles(List<string> files);
        Task<ResponseModel<DocumentDTO>> AddDocument(DocumentDTO dto);
        Task<ResponseModel<LotDTO>> DeleteDocument(int id);
    }
}
