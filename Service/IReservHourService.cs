using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IReservHourService
    {
        Task<ServiceResultDto<bool>> Reserve(ReserveDto dto);
    }
}
