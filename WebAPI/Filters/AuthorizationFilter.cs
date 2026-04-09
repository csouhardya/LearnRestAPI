using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Filters
{
    public class AuthorizationFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
