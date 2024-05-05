using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto
{
    public record ReserveDto
    {
        public string UserName { get; set; }
        public DateOnly ReservedDate { get; set; }
        public int Hour {  get; set; }
    }
}
