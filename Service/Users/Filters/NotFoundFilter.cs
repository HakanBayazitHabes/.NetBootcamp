using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Repository.Users;
using Service.Logs.SyncMethod;
using Service.SharedDTOs;
using Service.Users.DTOs;

namespace Service.Users.Filters;

public class NotFoundFilter(IUserRepositoryAsync userRepositoryAsync, ILoggerService logger) : Attribute, IActionFilter
{

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var actionName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName;

        var userIdFormAction = context.ActionArguments.Values.First();
        var userId = 0;

        if (actionName == "UpdateUserName" && userIdFormAction is UserNameUpdateRequestDto userNameUpdateRequestDto)
        {
            userId = userNameUpdateRequestDto.Id;
        }

        if (userId == 0 && !int.TryParse(userIdFormAction.ToString(), out userId))
        {
            return;
        }

        var hasUser = userRepositoryAsync.HasExist(userId).Result;

        if (!hasUser)
        {
            logger.LogInfo($"There is no user with id: {userId}");
            var errorMessage = $"There is no user with id: {userId}";

            var responseModel = ResponseModelDto<NoContent>.Fail(errorMessage);
            context.Result = new NotFoundObjectResult(responseModel);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        throw new NotImplementedException();
    }
}
