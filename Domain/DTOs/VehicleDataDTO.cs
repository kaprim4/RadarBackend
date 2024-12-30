using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Domain.DTOs
{
    public class VehicleDataDTO 
    {
        public int? Id { get; set; }
        public long Sequence { get; set; }

        public int VehicleSpeed { get; set; }

        public string? Timestamp { get; set; }

        public string? Image { get; set; }
        //public IFormFile ImageFile { get; set; }

        public string? Video { get; set; }

        public string? Jmx { get; set; }

        public string? Txt { get; set; }

        public int CrosshairX { get; set; }

        public int CrosshairY { get; set; }
        public string? Image_Name { get; set; }


    }
}
