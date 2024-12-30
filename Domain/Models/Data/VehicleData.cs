using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Domain.Models.Data
{
    [Table("VehicleData")]
    public class VehicleData : BaseEntity
    {
        public long Sequence { get; set; }

        public int VehicleSpeed { get; set; }

        public string? Timestamp { get; set; }
        public string? Image { get; set; }


        public string? Jmx { get; set; }

        public string? Txt { get; set; }

        public int CrosshairX { get; set; }

        public int CrosshairY { get; set; }

        [ForeignKey(nameof(JMX))]
        public int JMX_Id { get; set; }
        public virtual JMX JMX { get; set; }


    }
}
