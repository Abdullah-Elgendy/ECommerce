using ECommerce.Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;

namespace ECommerce.API.Attributes
{
    public class RedisCacheAttribute : ActionFilterAttribute
    {
        private readonly int _durationInSeconds;

        public RedisCacheAttribute(int durationInSeconds = 60)
        {
            _durationInSeconds = durationInSeconds;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Get Cache Service From Container
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            //Create Cache Key From Request Path
            var cacheKey = CreateCacheKey(context.HttpContext.Request);

            //Get Data Using Service
            var data = await cacheService.GetDataAsync(cacheKey);

            //If Data Exists In Cache -> Get Data From Cache

            if (!string.IsNullOrEmpty(data))
            {
                context.Result = new ContentResult()
                {
                    Content = data,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }

            //If Data Doesn't Exists In Cache -> Execute Endpoint Then Store Result In Cache
            var executedContext = await next.Invoke();

            //IF Result is 200Ok AND Has Data.
            if (executedContext.Result is OkObjectResult { Value: not null } ok)
            {
                await cacheService.SetDataAsync(cacheKey, ok.Value, TimeSpan.FromSeconds(_durationInSeconds)); 
            }
   
        }

        private static string CreateCacheKey(HttpRequest request)
        {
            var key = new StringBuilder();

            key.Append(request.Path);

            if (request.Query.Any())
            {
                key.Append('?');
                foreach (var queryItem in request.Query.OrderBy(x => x.Key))
                { 
                    key.Append($"{queryItem.Key}={queryItem.Value}&");
                }
            }

            return key.ToString();
        }
    }

}
