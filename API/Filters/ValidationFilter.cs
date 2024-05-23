using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Service.SharedDTOs;

namespace API.Filters;

public class ValidationFilter : ActionFilterAttribute
{

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            var responseModel = ResponseModelDto<NoContent>.Fail(errors);

            context.Result = new BadRequestObjectResult(responseModel);
        }
    }

}
