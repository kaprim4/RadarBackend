using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Domain.Models.Data
{
    [Table("DeploymentSummary")]
    public class DeploymentSummary : BaseEntity
    {

        public string? LocationCode { get; set; }

        public string? Location { get; set; }

        public string? DeploymentId { get; set; }

        public long StartSequence { get; set; }

        public long EndSequence { get; set; }

        public int SpeedLimit { get; set; }

        public int CaptureSpeed { get; set; }

        public string? MeasurementUnit { get; set; }

        public string? OperatorId { get; set; }

        public string? OperatorName { get; set; }

        public string? StartDtm { get; set; }

        public string? EndDtm { get; set; }
        public string? CameraName { get; set; }


        [ForeignKey(nameof(JMX))]
        public int JMX_Id { get; set; }
        public virtual JMX JMX { get; set; }



    }
}
