using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Dto;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkHourController : ControllerBase
    {
        private readonly IWorkHourService _workHourService;
        public WorkHourController(IWorkHourService workHourService)
        {
            _workHourService = workHourService;
        }

        [HttpPost]
        public async Task<IActionResult> ChangeCapacity([FromBody] ChangeCapacityDto dto)
        {
            var result = await _workHourService.ChangeCapacity(dto);
            return this.CookIt(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableInPeriod([FromQuery] DateOnly startDate , DateOnly endDate)
        {
            var result = await _workHourService.GetAvailableInPeriod(startDate, endDate);
            return this.CookIt(result);
        }
    }
}
