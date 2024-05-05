using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto
{
    public record AvailableInPeriodResultDto
    {
        public DateOnly Date { get; set; }
        public IList<CapacityPerHour> CapacityPerHours { get; set; } = new List<CapacityPerHour>();
    }

    public record CapacityPerHour
    {
        public int Hour { get; set; }
        public int Capacity { get; set; }
    }
}
