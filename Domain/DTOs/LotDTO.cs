using Domain.Models;
using Domain.Models.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class LotDTO
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public string? CreateDate { get; set; } = "";

        public string? UpdatedAt { get; set; }
        public virtual List<DocumentDTO> Documents { get; set; }
        public DeviceDTO? Device { get; set; }
    }

    public class DocumentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Reference { get; set; }
        public string? Path { get; set; }
        public JMXDTO? Jmx { get; set; }
        public int? LotId { get; set; }

    }

    public class JMXDTO
    {

        public int? Id { get; set; }
        public string? Reference { get; set; }
        public List<VehicleDataDTO>? VehicleDatas { get; set; }
        public DeploymentSummaryDTO? DeploymentSummary { get; set; }

    }

    public class LotListDTO
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public string CreatedDate { get; set; }
        public int NbFiles { get; set; }

    }

    public class AddDocumentDTO
    {
        public List<IFormFile> Images { get; set; }
        public DocumentDTO Dto { get; set; }
    }

}
