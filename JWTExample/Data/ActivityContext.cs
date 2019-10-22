using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JWTExample.Models
{
    public class ActivityContext : IdentityDbContext
    {
        public ActivityContext (DbContextOptions<ActivityContext> options)
            : base(options)
        {
        }


        public DbSet<ActivityLog> ActivityLog { get; set; }
    }
}
