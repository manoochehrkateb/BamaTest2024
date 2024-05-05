using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Service.Dto;

namespace Service
{
    public class ReservHourService : IReservHourService
    {
        private readonly BamaTestDbContext dbContext;
        private IWorkHourService _workHourService;
        public ReservHourService(BamaTestDbContext dbContext, IWorkHourService workHourService)
        {
            this.dbContext = dbContext;
            _workHourService = workHourService;
        }

        public async Task<ServiceResultDto<bool>> Reserve(ReserveDto dto)
        {
            try
            {
                //validation
                if (dto.ReservedDate == DateOnly.FromDateTime(DateTime.Now) && dto.Hour > TimeOnly.FromDateTime(DateTime.Now).Hour + 2)
                    return ServiceResultDto<bool>.NotOk("You must pick time 2 hours Before reservation !");

                var avaiableCapacity = await _workHourService.GetAvailableInPeriod(dto.ReservedDate, dto.ReservedDate);
                if(!avaiableCapacity.isSuccess)
                    return ServiceResultDto<bool>.NotOk(avaiableCapacity.error.First());

                if(avaiableCapacity.data.First().CapacityPerHours.First(f=> f.Hour == dto.Hour).Capacity < 1)
                    return ServiceResultDto<bool>.NotOk("not insert , full capacity !");

                var dbModel = new ReservHour()
                {
                    ReservedDate = dto.ReservedDate,
                    UserName = dto.UserName,
                    ReservedHour = dto.Hour
                };

                dbContext.ReservHours.Add(dbModel);
                await dbContext.SaveChangesAsync();

                return ServiceResultDto<bool>.Ok(true);

            }
            catch (Exception ex)
            {
                return ServiceResultDto<bool>.NotOk(ex.Message);
            }
        }
    }
}
