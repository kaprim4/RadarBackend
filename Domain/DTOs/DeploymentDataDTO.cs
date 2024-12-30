using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class DeploymentDataDTO
    {
        public int? Id { get; set; }

        public List<VehicleDataDTO>? VehicleDatas { get; set; }
        public DeploymentSummaryDTO? DeploymentSummary { get; set; }

    }
}
