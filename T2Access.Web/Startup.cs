using System;
using System.Globalization;
using System.Net;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

using T2Access.Services.HttpClientService;
using T2Access.Web.Extensions;
using T2Access.Web.Helper;

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



            #region Localization
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ar"),
                };

                options.DefaultRequestCulture = new RequestCulture(culture: "en", uiCulture: "en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders = new[]{ new RouteDataRequestCultureProvider{
                        RouteDataStringKey = "lang",
                        UIRouteDataStringKey = "lang"
                }};
            });

            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("lang", typeof(LanguageRouteConstraint));
            });
            #endregion


            services.AddControllersWithViews(options =>
            {
                // global authorize filter
                options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole("Admin").Build()));
            })
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
               .AddDataAnnotationsLocalization()
               .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);


            services.AddSession();

            services.AddAuthServices(Configuration.GetSection("Authentication"));


            services.TryAddTransient<IHttpClientService>(x => new HttpClientService(new Uri(Configuration.GetSection("AppSettings").GetValue<string>("ServerBaseAddress"))));

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


            app.UseRequestLocalization();

            //Add User session
            app.UseSession();


            //Add Token to all incoming HTTP Request Header
            app.Use(async (context, next) =>
            {
                var token = context.Session.GetString("Token");
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


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{lang=en}/{controller=user}/{action=index}/{id?}");
            });
        }



    }
}
