using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto
{
    public record ChangeCapacityDto
    {
        public DateOnly startDate { get; set; }
        public DateOnly endDate { get; set; }
        public int startHour { get; set; }
        public int endHour { get; set; }
        public int capacity { get; set; }
    }
}
