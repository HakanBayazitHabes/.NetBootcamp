using API.LogModel;
using Microsoft.AspNetCore.Mvc.Filters;
using Service.Logs.SyncMethod;

namespace API.Filters
{
    public class LogFilterAttribute(ILoggerService logger) : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInfo(Log("OnActionExecuting", context.RouteData));
        }

        private string Log(string modelName, RouteData routeData)
        {
            var logDetails = new LogDetails()
            {
                ModelName = modelName,
                Controller = routeData.Values["controller"],
                Action = routeData.Values["action"],
            };

            if (routeData.Values.Count >= 3)
                logDetails.Id = routeData.Values["Id"];

            return logDetails.ToString();
        }
    }
}