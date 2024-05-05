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
    public class WorkHourConfigs : IEntityTypeConfiguration<CustomeWorkHour>
    {
        public void Configure(EntityTypeBuilder<CustomeWorkHour> builder)
        {
            builder.HasKey(h => h.Id);
        }
    }
}
