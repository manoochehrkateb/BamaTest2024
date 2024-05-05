using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Config
{
    public class ReservHourConfigs : IEntityTypeConfiguration<ReservHour>
    {
        public void Configure(EntityTypeBuilder<ReservHour> builder)
        {
            builder.HasKey(h => h.Id);
            builder.Property(bc => bc.UserName).HasMaxLength(256).IsRequired();
        }
    }
}
