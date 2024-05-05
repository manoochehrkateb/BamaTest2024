using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class CustomeWorkHour : BaseEntity
    {
        public DateOnly StartDate {  get; set; }
        public DateOnly EndDate {  get; set; }
        public int StartHour { get; set; }
        public int EndHour { get; set; }
        public int Capacity { get; set; }
    }
}
