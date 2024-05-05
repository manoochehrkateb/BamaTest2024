using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class ReservHour : BaseEntity
    {
        public string UserName { get; set; }
        public DateOnly ReservedDate { get; set; }
        public int ReservedHour { get; set; }
    }
}
