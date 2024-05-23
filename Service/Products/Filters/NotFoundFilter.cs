using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Repository.Products.AsyncMethods;
using Service.Logs.SyncMethod;
using Service.Products.DTOs;
using Service.SharedDTOs;

namespace Service.Products
{
    public class NotFoundFilter(IProductRepository2 productRepository2, ILoggerService logger) : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var actionName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName;

            var productIdFormAction = context.ActionArguments.Values.First();
            var productId = 0;

            if (actionName == "UpdateProductName" && productIdFormAction is ProductNameUpdateRequestDto productNameUpdateRequestDto)
            {
                productId = productNameUpdateRequestDto.Id;
            }

            if (productId == 0 && !int.TryParse(productIdFormAction.ToString(), out productId))
            {
                return;
            }

            var hasProduct = productRepository2.HasExist(productId).Result;

            if (!hasProduct)
            {
                logger.LogInfo($"There is no product with id: {productId}");
                var errorMessage = $"There is no product with id: {productId}";

                var responseModel = ResponseModelDto<NoContent>.Fail(errorMessage);
                context.Result = new NotFoundObjectResult(responseModel);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            throw new NotImplementedException();
        }
    }
}