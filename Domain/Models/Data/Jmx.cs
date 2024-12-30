using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Data
{
    [Table("JMX")]
    public class JMX : BaseEntity
    {
        public string Reference { get; set; }
        public virtual List<VehicleData>? VehicleDatas { get; set; }
        public virtual DeploymentSummary? DeploymentSummary { get; set; }


        [ForeignKey(nameof(Document))]
        public int Document_Id { get; set; }
        public virtual Document Document { get; set; }
    }
}
