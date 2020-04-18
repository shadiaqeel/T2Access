using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using T2Access.Security.Tokenization.Services;
using T2Access.Services.HttpClientService;
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

                options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders = new[]{ new T2Access.Web.Helper.RouteDataRequestCultureProvider{
                    IndexOfCulture=1,
                    IndexofUICulture=1
                }};
            });

            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("lang", typeof(LanguageRouteConstraint));
            }); 
            #endregion


            services.AddControllersWithViews()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
               .AddDataAnnotationsLocalization();

            services.AddSession();


            #region AuthTokenization
            var key = Configuration.GetSection("AppSettings").GetSection("AuthTokenization").GetValue<string>("SecretKey");
            services.TryAddScoped<IAuthService>(x => new JWTService(
                     key,
                     Configuration.GetSection("AppSettings").GetSection("AuthTokenization").GetValue<string>("Issuer"),
                     Configuration.GetSection("AppSettings").GetSection("AuthTokenization").GetValue<string>("AudienceId")));

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie(cfg => { cfg.SlidingExpiration = true; })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false, //Configuration.GetSection("AppSettings").GetSection("AuthTokenization").GetValue<string>("AudienceId")
                    ValidateIssuer = false, //Configuration.GetSection("AppSettings").GetSection("AuthTokenization").GetValue<string>("Issuer")
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key))
                };
            });
            #endregion





            services.TryAddTransient<IHttpClientService>(x => new HttpClientService (new Uri (Configuration.GetSection("AppSettings").GetValue<string>("ServerBaseAddress"))));

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


            //Add Token Authentication service
            app.UseAuthentication();
            app.UseAuthorization();


            //#region Localization
            //IList<CultureInfo> supportedCultures = new List<CultureInfo>
            //  {
            //      new CultureInfo("en"),
            //      new CultureInfo("ar"),
            //  };
            //var localizationOptions = new RequestLocalizationOptions
            //{
            //    DefaultRequestCulture = new RequestCulture("en"),
            //    SupportedCultures = supportedCultures,
            //    SupportedUICultures = supportedCultures
            //};
            //localizationOptions.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider()
            //{
            //    RouteDataStringKey = "lang",
            //    Options = localizationOptions
            //});
            //#endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{lang=ar}/{controller=account}/{action=login}/{id?}");
            });
        }
    }
}
