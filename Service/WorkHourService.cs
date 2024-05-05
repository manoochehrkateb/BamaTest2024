using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;
using Data;
using Microsoft.EntityFrameworkCore;
using Service.Dto;

namespace Service
{
    public class WorkHourService : IWorkHourService
    {
        private readonly BamaTestDbContext dbContext;
        public WorkHourService(BamaTestDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<ServiceResultDto<bool>> ChangeCapacity(ChangeCapacityDto dto)
        {
            try
            {
                var dbModel = await dbContext.CustomeWorkHours.Where(x => (dto.startDate >= x.StartDate && dto.startDate < x.EndDate) || (dto.endDate >= x.StartDate && dto.endDate < x.EndDate) ||
                (dto.startDate <= x.StartDate && dto.endDate >= x.EndDate)).ToListAsync();
                if (dbModel == null || dbModel.Count == 0)
                {
                    dbContext.CustomeWorkHours.Add(new CustomeWorkHour()
                    {
                        Capacity = dto.capacity,
                        EndDate = dto.endDate,
                        EndHour = dto.endHour,
                        StartDate = dto.startDate,
                        StartHour = dto.startHour
                    });
                }
                else if (dbModel.Count > 1)
                {
                    return ServiceResultDto<bool>.NotOk("it has duplicate row");
                }
                else
                {
                    SplitCustomeWorkHour(dbModel.First(), dto);
                }

                await dbContext.SaveChangesAsync();

                return ServiceResultDto<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return ServiceResultDto<bool>.NotOk(ex.Message);
            }
        }

        public async Task<ServiceResultDto<List<AvailableInPeriodResultDto>>> GetAvailableInPeriod(DateOnly startDate, DateOnly endDate)
        {
            var customeWorkHours = await dbContext.CustomeWorkHours.AsNoTracking().Where(x => (startDate >= x.StartDate && startDate < x.EndDate) || (endDate >= x.StartDate && endDate < x.EndDate)||
            (startDate <= x.StartDate && endDate >= x.EndDate)).ToListAsync();
            var reservInPeriod = await dbContext.ReservHours.AsNoTracking().Where(x => startDate >= x.ReservedDate || endDate <= x.ReservedDate).ToListAsync();

            var result = new List<AvailableInPeriodResultDto>();
            for (var i = 0; i <= endDate.DayNumber - startDate.DayNumber; i++)
            {
                var addItem = new AvailableInPeriodResultDto() { Date = startDate.AddDays(i) };
                for (var j = 9; j < 18; j++)
                {
                    var customWorkHour = HasCustomWorkHour(customeWorkHours, addItem.Date, j);
                    var totalCapacity = customWorkHour?.Capacity ?? DefaultCapacity(addItem.Date.DayOfWeek, j).data;
                    var remainCapacity = totalCapacity - (HasReserved(reservInPeriod, addItem.Date, j) ?? 0);
                    addItem.CapacityPerHours.Add(new CapacityPerHour()
                    {
                        Capacity = remainCapacity,
                        Hour = j
                    });
                }
                result.Add(addItem);
            }

            return ServiceResultDto<List<AvailableInPeriodResultDto>>.Ok(result);
        }

        public ServiceResultDto<int> DefaultCapacity(DayOfWeek dayName, int hour)
        {
            int defaultCapacity = 2;
            switch (dayName)
            {
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                    if (hour >= 9 && hour <= 17)
                        return ServiceResultDto<int>.Ok(defaultCapacity);
                    else
                        return ServiceResultDto<int>.Ok(0);
                case DayOfWeek.Thursday:
                    if (hour >= 9 && hour <= 13)
                        return ServiceResultDto<int>.Ok(defaultCapacity);
                    else
                        return ServiceResultDto<int>.Ok(0);
                case DayOfWeek.Friday:
                    break;
                default:
                    break;
            }

            return ServiceResultDto<int>.NotOk("Invalid input");
        }


        #region Helpers
        private CustomeWorkHour? HasCustomWorkHour(List<CustomeWorkHour> customeWorkHours, DateOnly currentDate, int hour)
        {
            if (customeWorkHours == null || !customeWorkHours.Any())
                return null;
            return customeWorkHours.FirstOrDefault(x => currentDate >= x.StartDate && currentDate <= x.EndDate && hour >= x.StartHour && hour <= x.EndHour);
        }

        private int? HasReserved(List<ReservHour> reservHours, DateOnly currentDate, int hour)
        {
            if (reservHours == null || !reservHours.Any())
                return null;
            return reservHours.Count(x => currentDate == x.ReservedDate && hour == x.ReservedHour);
        }

        private void SplitCustomeWorkHour(CustomeWorkHour dbModel, ChangeCapacityDto dto)
        {
            if (dto.startDate > dbModel.StartDate && dto.endDate < dbModel.EndDate)
            {
                var newDbModel = new CustomeWorkHour();
                newDbModel.StartDate = dto.startDate;
                newDbModel.EndDate = dto.endDate;
                newDbModel.Capacity = dto.capacity;
                newDbModel.StartHour = dto.startHour;
                newDbModel.EndHour = dto.endHour;

                dbContext.CustomeWorkHours.Add(newDbModel);

                var newDbModel2 = new CustomeWorkHour();
                newDbModel2.StartDate = dto.endDate.AddDays(1);
                newDbModel2.EndDate = dbModel.EndDate;
                newDbModel2.Capacity = dbModel.Capacity;
                newDbModel2.StartHour = dbModel.StartHour;
                newDbModel2.EndHour = dbModel.EndHour;

                dbContext.CustomeWorkHours.Add(newDbModel2);

                dbModel.EndDate = dto.startDate.AddDays(-1);
            }
            else if (dto.startDate <= dbModel.StartDate && dto.endDate < dbModel.EndDate)
            {
                dbModel.StartDate = dto.endDate.AddDays(1);
                dbModel.Capacity = dto.capacity;
            }
            else if (dto.endDate >= dbModel.EndDate && dto.startDate > dbModel.StartDate)
            {
                dbModel.Capacity = dto.capacity;
                dbModel.EndDate = dto.startDate.AddDays(-1);
            }
            else if (dto.endDate >= dbModel.EndDate && dto.startDate == dbModel.StartDate)
            {
                dbModel.EndDate = dto.endDate;
                dbModel.Capacity = dto.capacity;
            }
        }
        #endregion
    }
}
