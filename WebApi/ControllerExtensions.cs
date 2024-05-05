using Microsoft.AspNetCore.Mvc;
using Service.Dto;

namespace WebApi
{
    public static class ControllerExtensions
    {
        public static IActionResult CookIt<T>(this ControllerBase controller, ServiceResultDto<T> responseDto)
        {
            if (responseDto.isSuccess)
                return controller.Ok(responseDto);
            else
                return controller.BadRequest(responseDto);

            //switch (responseDto.Code)
            //{
            //    case Constants.ResponseCodeEnum.Ok:
            //        return controller.Ok(responseDto);
            //    case Constants.ResponseCodeEnum.BadRequest:
            //        return controller.BadRequest(responseDto);
            //    case Constants.ResponseCodeEnum.Conflict:
            //        return controller.Conflict(responseDto);
            //    case Constants.ResponseCodeEnum.NoContent:
            //        return controller.NoContent();
            //    case Constants.ResponseCodeEnum.NotFound:
            //        return controller.NotFound(responseDto);
            //    default:
            //        return controller.StatusCode(500, responseDto);
            //}
        }

    }
}
