using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using WebAPI.Data;
using WebAPI.Data.Repo;
using WebAPI.Extenstions;
using WebAPI.Helpers;
using WebAPI.Interfaces;
using WebAPI.Middlewares;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var builder = new SqlConnectionStringBuilder(
                Configuration.GetConnectionString("Default"));

            var connectionstring = builder.ConnectionString;

            services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionstring));
            services.AddControllers();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            var secretkey = Configuration.GetSection("AppSettings:key").Value;

            var key = new SymmetricSecurityKey(Encoding.UTF8
               .GetBytes(secretkey));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt => {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = false,
                        IssuerSigningKey = key

                    };
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.ConfigureExceptionHandler(env);


           // app.UseMiddleware<ExceptionMiddleware>();
            app.UseHsts();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();    
            app.UseAuthorization();

            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
