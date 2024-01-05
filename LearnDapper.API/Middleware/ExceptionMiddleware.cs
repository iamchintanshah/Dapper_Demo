
using System.Net;
using System.Text.Json;

namespace LearnDapper.API.Middleware
{
    //public class RequestLoggerMiddleware : IMiddleware
    public class ExceptionMiddleware
    {
        #region "Bhavin code for Request path"
            //public Task InvokeAsync(HttpContext context, RequestDelegate next)
            //{
            //    if(context.Request.Path.Value!.StartsWith("/api"))
            //    {
            //        return next.Invoke(context);
            //    }
            //    return Task.CompletedTask;
            //}
        #endregion

        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var response = new { error = ex.Message, Title = "Error During running the code", Status = "Internal server error" };
            var payload = JsonSerializer.Serialize(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(payload);
        }
    }
}
