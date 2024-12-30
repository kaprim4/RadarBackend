using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    [Table("Device")]
    public class Device : BaseEntity
    {
        public string SerialNumber { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public virtual List<Lot> Lots { get; set;} 
    }
}
