using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using T2Access.Services.HttpClientService;

namespace T2Access.Web
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
            services.AddLocalization(o =>
            {
                // We will put our translations in a folder called Resources
                o.ResourcesPath = "Resources";
            });


            services.AddControllersWithViews();
            services.AddSession();

            services.AddTransient<IHttpClientService>(x => new HttpClientService (new Uri (Configuration.GetSection("AppSettings").GetValue<string>("ServerBaseAddress"))));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
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

            app.UseSession();

            #region Localization
            IList<CultureInfo> supportedCultures = new List<CultureInfo>
              {
                  new CultureInfo("en"),
                  new CultureInfo("ar"),
              };
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };
            localizationOptions.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider()
            {
                RouteDataStringKey = "lang",
                Options = localizationOptions
            });
            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{lang=en}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
