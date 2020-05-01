using System;
using System.Globalization;
using System.Net;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

using T2Access.Services.HttpClientService;
using T2Access.Web.SPA.VueJs.Extensions;
using T2Access.Web.SPA.VueJs.Providers;


[assembly: ApiConventionType(typeof(DefaultApiConventions))]

namespace T2Access.Web.SPA.VueJs
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddSession();

            services.AddAuthServices(Configuration.GetSection("Authentication"));


            services.TryAddTransient<IHttpClientService>(x => new HttpClientService(new Uri(Configuration["AppSettings:ServerBaseAddress"])));


            services.AddCors(options =>
            {
                /*
                 Specifying AllowAnyOrigin and AllowCredentials is an insecure configuration and can result in cross-site request forgery. 
                 The CORS service returns an invalid CORS response when an app is configured with both methods.
                 */
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    //.AllowCredentials()
                    );
            });


            //services.Configure<GzipCompressionProviderOptions>(options =>
            //    options.Level = CompressionLevel.Optimal);

            //            services.AddResponseCompression(options =>
            //            {
            //#if (!NoHttps)
            //                options.EnableForHttps = true;
            //#endif
            //            });


            //services.AddSpaStaticFiles(config => { config.RootPath = "wwwroot/"; });
            services.AddSpaStaticFiles(options => options.RootPath = "ClientApp/dist");



            // Example with dependency injection for a data provider.
            services.AddSingleton<IWeatherProvider, WeatherProviderFake>();




        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
#if (!NoHttps)
                app.UseHsts();
            }

            app.UseHttpsRedirection();
#else
            }
#endif

            //app.UseResponseCompression();




            //app.UseDefaultFiles();
            app.UseStaticFiles();


            app.UseRouting();
            app.UseCors("CorsPolicy");



            app.UseRequestLocalization();

            //Add User session
            app.UseSession();


            //Add Token to all incoming HTTP Request Header
            app.Use(async (context, next) =>
            {
                string token = context.Session.GetString("Token");
                if (!string.IsNullOrEmpty(token))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + token);
                }
                await next();
            });



            app.UseStatusCodePages(async context =>
            {
                var response = context.HttpContext.Response;

                if (response.StatusCode == (int)HttpStatusCode.Unauthorized ||
                    response.StatusCode == (int)HttpStatusCode.Forbidden)
                {
                    response.Redirect($"{context.HttpContext.Request.PathBase}/{CultureInfo.CurrentCulture.TwoLetterISOLanguageName}/account/login");/*?returnUrl={context.HttpContext.Request.Path}*/
                    context.HttpContext.Session.SetString("returnUrl", context.HttpContext.Request.Path);
                }



            });



            //Add Token Authentication service
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseSpaStaticFiles();



            //pattern: "{lang=en}/{area}/{controller=Home}/{action=Index}/{id?}");


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "spa-admin-api",
                    pattern: "{lang=en}/admin/{controller=Home}/{action=index}",
                    defaults: new { area = "admin" });

                endpoints.MapControllerRoute(
                  name: "spa-admin-fallback",
                  pattern: "{lang = en}/admin/{*anything}",
                  defaults: new { area = "admin", controller = "Home", action = "Index" });

                endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{lang=en}/{controller=account}/{action=login}");
                  //defaults: new { controller = "Account", action = "Login" });
            


            });



            app.UseSpa(spa =>
                                {
                                    spa.Options.SourcePath = "ClientApp";

                                    if (env.IsDevelopment())
                                    {

                                        // Launch development server for Vue.js
                                        //spa.UseVueDevelopmentServer(npmScript: "build");


                                    }
                                });









        }
    }
}
