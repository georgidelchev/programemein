using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Programemein.Helpers;
using System.Threading.Tasks;

namespace Programemein.Infrastructure.Filters
{
    public class RequestSizeLimitChecker : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var requestSize = context.HttpContext.Request.ContentLength;

            if (requestSize.HasValue)
            {
                if (requestSize.Value > FileSizeCalculator.MegaBytes(100))
                {
                    var result = new ViewResult();
                    result.StatusCode = 413;
                    result.ViewName = "~/Views/Images/Failed.cshtml";

                    context.Result = result;
                    return;
                }
            }

            await next();
        }
    }
}
