using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Dto;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservHourController : ControllerBase
    {
        private readonly IReservHourService reservedService;
        public ReservHourController(IReservHourService reservedService)
        {
            this.reservedService = reservedService;
        }

        [HttpPost]
        public async Task<IActionResult> Reserve([FromBody] ReserveDto dto)
        {
            var result = await reservedService.Reserve(dto);
            return this.CookIt(result);
        }
    }
}
