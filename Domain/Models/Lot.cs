using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Lot : BaseEntity
    {
        public string Reference { get; set; }

        [ForeignKey(nameof(Device))]
        public int Device_Id { get; set; }
        public virtual Device Device { get; set; }
        public virtual List<Document> Documents { get; set; }
    }
}
