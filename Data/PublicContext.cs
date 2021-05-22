using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Public.Models;

namespace Public.Data
{
    public class PublicContext : DbContext
    {
        public PublicContext (DbContextOptions<PublicContext> options)
            : base(options)
        {
        }

        public DbSet<Public.Models.Project> Projects { get; set; }

        public DbSet<Public.Models.SitePage> SitePage { get; set; }
    }
}
