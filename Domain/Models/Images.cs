using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    [Table("Images")]
    public class Images : BaseEntity
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string VideoPath { get; set; }

    }
    public enum Types
    {
        Image,
        Video,
    }
}
