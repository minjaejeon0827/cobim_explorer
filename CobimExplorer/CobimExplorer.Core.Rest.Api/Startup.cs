using CobimExplorer.Core.Rest.Api.CobimBase.User;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CobimExplorer.Core.Rest.Api
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
            services.AddControllersWithViews();

            services.AddSingleton<UserRestServer>();  // ÇØ´ç ¼­ºñ½º ½Ì±ÛÅæ ¼³Á¤ 

            services.AddHttpClient(LoginHelper.httpClient, options =>
            {
                options.BaseAddress = new Uri(LoginHelper.auth_server_url);
            });

            // Example 1: Basic use
            //    services.AddHttpClient();

            // Example 2:
            //    services.AddHttpClient("people", options => 
            //    {
            //        options.BaseAddress = new Uri("https://localhost:44301/api/people");
            //    });

            //    services.AddHttpClient("User", options =>
            //    {
            //        options.BaseAddress = new Uri(LoginHelper.auth_server_url);
            //    });


            //    services.AddHttpClient("weatherForecast", options =>
            //    {
            //        options.BaseAddress = new Uri("https://localhost:44301/api/weatherForecast");
            //        options.DefaultRequestHeaders.Add("amountofElements", "25");
            //    });

            // Example 3:
            //    services.AddHttpClient<IUserRestServer, UserRestServer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
