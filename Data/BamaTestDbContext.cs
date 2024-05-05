using Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class BamaTestDbContext : DbContext
    {
        public BamaTestDbContext(DbContextOptions<BamaTestDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }


        public virtual DbSet<CustomeWorkHour> CustomeWorkHours { get; set; }
        public virtual DbSet<ReservHour> ReservHours { get; set; }
    }
}
