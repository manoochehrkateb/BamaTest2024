using Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IWorkHourService
    {
        Task<ServiceResultDto<bool>> ChangeCapacity(ChangeCapacityDto dto);
        Task<ServiceResultDto<List<AvailableInPeriodResultDto>>> GetAvailableInPeriod(DateOnly startDate, DateOnly endDate);
        ServiceResultDto<int> DefaultCapacity(DayOfWeek dayName, int hour);
    }
}
