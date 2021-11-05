using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Programemein.Web.Infrastructure.Helpers;

namespace Programemein.Web.Infrastructure.Filters
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
