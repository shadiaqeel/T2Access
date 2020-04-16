using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using T2Access.BLL.Services;
using T2Access.DAL;
using T2Access.DAL.Options;
using T2Access.Security.Tokenization.Services;

namespace T2Access.API.Extensions
{
    public static class WebServiceExtension
    {



        public static IServiceCollection AddWebServices(
         this IServiceCollection services,
         IConfigurationSection AuthOptionsSection,
         //IConfigurationSection BLLOptionsSection,
         IConfigurationSection DALOptionSection)
        {
            //if (BLLOptionsSection == null)
            //{
            //    throw new ArgumentNullException(nameof(BLLOptionsSection));
            //}

            if (DALOptionSection == null)
            {
                throw new ArgumentNullException(nameof(DALOptionSection));
            }       
            if (AuthOptionsSection == null)
            {
                throw new ArgumentNullException(nameof(AuthOptionsSection));
            }




            services.Configure<DALOptions>(opt => DALOptionSection.Bind(opt));


            // DAL Services
            services.TryAddSingleton<IUserManager, UserManager>();
            services.TryAddSingleton<IGateManager, GateManager>();
            services.TryAddSingleton<ITransactionManager, TransactionManager>();
            services.TryAddSingleton<IUserGateManager, UserGateManager>();

            // BLL Services
            services.TryAddScoped<IUserService, UserService>();
            services.TryAddScoped<IGateService, GateService>();
            services.TryAddScoped<ITransactionService, TransactionService>();


            services.TryAddScoped<IAuthService>(x => new JWTService(
                AuthOptionsSection.GetValue<string>("SecretKey"),
                AuthOptionsSection.GetValue<string>("Issuer"),
                AuthOptionsSection.GetValue<string>("AudienceId")));




            return services;
        }



        //public static IApplicationBuilder UseWebServices(this IApplicationBuilder app)
        //{

        //    return app;
        //}

    }
}
