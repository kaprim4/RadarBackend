using Domain.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Document : BaseEntity
    {
        public string Reference { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        [ForeignKey(nameof(Jmx))]
        public int? Jmx_Id { get; set; }
        public virtual JMX? Jmx { get; set; }

        [ForeignKey(nameof(Lot))]
        public int Lot_Id { get; set; }

        public virtual Lot  Lot { get; set; }
        
    }
}
