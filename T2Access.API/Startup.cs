using System.Collections.Generic;
using System.Globalization;
using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

using T2Access.API.Extensions;

namespace T2Access.API
{

    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {


            services.AddLocalization(o =>
                    {
                        // We will put our translations in a folder called Resources
                        o.ResourcesPath = "Resources";
                    });


            services.AddControllers();

            services.AddWebServices(
                     //BLLOptionsSection: Configuration.GetSection("AppSettings"),
                     AuthOptionsSection: Configuration.GetSection("AppSettings").GetSection("AuthTokenization"),
                     DALOptionSection: Configuration.GetSection("DALSettings")
                     );


            #region AuthTokenization
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings").GetSection("AuthTokenization").GetValue<string>("SecretKey"));
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
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            }); 
            #endregion
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();



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
                endpoints.MapControllers();
            });
        }
    }
}
