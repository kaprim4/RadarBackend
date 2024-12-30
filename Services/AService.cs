using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public abstract class AService
    {
        public AppDbContext _context { get; set; }

        public AService(AppDbContext context)
        {
            _context = context;
        }

    }
}
