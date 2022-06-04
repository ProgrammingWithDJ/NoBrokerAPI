using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebAPI.Errors;

namespace WebAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly IHostEnvironment env;
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            this.env = env;
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch(Exception ex)
            {
                APIError response;
                HttpStatusCode statusCode=HttpStatusCode.InternalServerError;
                String message;
                var exceptionType = ex.GetType();

                if(exceptionType==typeof(UnauthorizedAccessException))
                {
                    statusCode = HttpStatusCode.Forbidden;
                    message = "You are not Authorized";
                }
                else
                {
                    statusCode = HttpStatusCode.InternalServerError;
                    message = "Some error occured";
                }

                if(env.IsDevelopment())
                {
                    response = new APIError((int)statusCode,ex.Message,ex.StackTrace.ToString());
                }
                else
                {
                    response = new APIError((int)statusCode, message);
                }

                logger.LogError(ex, ex.Message);
                context.Response.StatusCode = (int)statusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(response.ToString());
            }
        }


    }
}
