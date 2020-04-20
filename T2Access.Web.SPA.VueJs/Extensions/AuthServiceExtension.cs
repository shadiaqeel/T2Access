using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

using T2Access.Security.Tokenization.Services;

namespace T2Access.Web.SPA.VueJs.Extensions
{
    public static class AuthServiceExtension
    {



        public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfigurationSection AuthOptionsSection)
        {


            if (AuthOptionsSection == null)
            {
                throw new ArgumentNullException(nameof(AuthOptionsSection));
            }


            if (bool.Parse(AuthOptionsSection["JwtBearer:IsEnabled"]))
            {


                services.TryAddScoped<IAuthService>(x => new JWTService(AuthOptionsSection["JwtBearer:SecretKey"], AuthOptionsSection["JwtBearer:Issuer"], AuthOptionsSection["JwtBearer:Audience"]));




                services.AddAuthentication(options => {
                    options.DefaultAuthenticateScheme = "JwtBearer";
                    options.DefaultChallengeScheme = "JwtBearer";
                }).AddJwtBearer("JwtBearer", options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.Audience = AuthOptionsSection["JwtBearer:Audience"];

                    options.TokenValidationParameters = new TokenValidationParameters
                    {

                        // The signing key must match!
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthOptionsSection["JwtBearer:SecretKey"])),

                        // Validate the JWT Issuer (iss) claim
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptionsSection["JwtBearer:Issuer"],

                        // Validate the JWT Audience (aud) claim
                        ValidateAudience = true,
                        ValidAudience = AuthOptionsSection["JwtBearer:Audience"],

                        // Validate the token expiry
                        ValidateLifetime = true,

                        // If you want to allow a certain amount of clock drift, set that here
                        ClockSkew = TimeSpan.Zero
                    };


                });
            }




            return services;
        }





    }
}
