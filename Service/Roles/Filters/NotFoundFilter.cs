using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Repository.Roles;
using Service.Logs.SyncMethod;
using Service.Roles.DTOs;
using Service.SharedDTOs;

namespace Service.Roles.Filters;

public class NotFoundFilter(IRoleRepositoryAsync roleRepositoryAsync, ILoggerService logger) : Attribute, IActionFilter
{

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var actionName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName;

        var roleIdFormAction = context.ActionArguments.Values.First();
        var roleId = 0;

        if (actionName == "UpdateRoleName" && roleIdFormAction is RoleNameUpdateRequestDto roleNameUpdateRequestDto)
        {
            roleId = roleNameUpdateRequestDto.Id;
        }

        if (roleId == 0 && !int.TryParse(roleIdFormAction.ToString(), out roleId))
        {
            return;
        }

        var hasRole = roleRepositoryAsync.HasExist(roleId).Result;

        if (!hasRole)
        {   
            logger.LogInfo($"There is no role with id: {roleId}");
            var errorMessage = $"There is no role with id: {roleId}";

            var responseModel = ResponseModelDto<NoContent>.Fail(errorMessage);
            context.Result = new NotFoundObjectResult(responseModel);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        throw new NotImplementedException();
    }


}
