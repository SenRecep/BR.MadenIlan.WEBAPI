using BR.MadenIlan.Shared.ExtensionMethods;

using Microsoft.AspNetCore.Mvc.Filters;

namespace BR.MadenIlan.Shared.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
                context.Result = context.GetBadRequestResultErrorDtoForModelState();
        }
    }
}
