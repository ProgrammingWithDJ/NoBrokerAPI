using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace WebAPI.Extenstions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(

                    options =>
                     options.Run(
                         async context =>
                         {
                             context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                             var ex = context.Features.Get<IExceptionHandler>();

                             if (ex != null)
                             {
                                 await context.Response.WriteAsync(ex.ToString());
                             }
                         }
                         ));
            }

        }
    }
}
