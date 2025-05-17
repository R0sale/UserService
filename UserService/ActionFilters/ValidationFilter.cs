using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace UserService.ActionFilters
{
    public class ValidationFilter : IActionFilter
    {
        public ValidationFilter()
        { }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var action = context.RouteData.Values["action"];
            var controller = context.RouteData.Values["controller"];

            var param = context.ActionArguments.SingleOrDefault(a => a.Value.ToString().Contains("DTO")).Value;

            if (param is null)
            {
                context.Result = new BadRequestObjectResult($"Object is null. Controller: {controller}, action: {action}");
                return;
            }

            if (!context.ModelState.IsValid)
            {
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        { }
    }
}
