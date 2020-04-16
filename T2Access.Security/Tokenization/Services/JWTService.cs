using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

using T2Access.Security.Tokenization.Models;

namespace T2Access.Security.Tokenization.Services
{
    public class JWTService : IAuthService
    {
        public string SecretKey { get; set; }

        private readonly string _issuer, _audienceId;

        public JWTService(string secretKey = null , string issuer = null , string audienceId = null)
        {
            SecretKey = secretKey ?? Constants.SECRET_KEY;
            _issuer = issuer ?? "T2";
            _audienceId = audienceId ?? "T2";
        }




        private TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters()
            {

                RequireExpirationTime = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = GetSymmetricSecurityKey()
            };
        }


        private SecurityKey GetSymmetricSecurityKey()
        {
            byte[] symmetricKey = Encoding.ASCII.GetBytes(SecretKey);
            return new SymmetricSecurityKey(symmetricKey);
        }








        public string GenerateToken(IAuthContainerModel model)
        {

            if (model == null || model.Claims == null || model.Claims.Length == 0)
            {
                throw new ArgumentException("Arguments to  create  token are not valid");
            }

            try
            {

                return new JwtSecurityTokenHandler().WriteToken(
                         new JwtSecurityToken(
                             header: new JwtHeader(
                                 new SigningCredentials(
                                     GetSymmetricSecurityKey(), model.SecurityAlgorithm)),
                             payload: new JwtPayload(_issuer,
                                            _audienceId,
                                            model.Claims,
                                            DateTime.Now,
                                            DateTime.Now.AddMinutes(model.ExpireMinutes),
                                            DateTime.Now)
                            ));
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public IEnumerable<Claim> GetTokenClaims(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Given token is null or empty   ");
            }

            return GetPrincipal(token).Claims;
        }


        public string GetTokenClaimValue(string token, string claimName)
        {


            IEnumerator<Claim> Claim = GetPrincipal(token).Claims.GetEnumerator();

            while (Claim.MoveNext() && Claim.Current.Type != claimName)
            {
                ;
            }

            return Claim.Current.Value;

        }

        public bool IsTokenValid(string token)
        {

            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Given token is null or empty");
            }

            try
            {

                ClaimsPrincipal principal = GetPrincipal(token);
                return true;

            }
            catch (Exception)
            {
                return false;
            }



        }



        public ClaimsPrincipal GetPrincipal(string token)
        {


            var tokenHandler = new JwtSecurityTokenHandler();


            if (!(tokenHandler.ReadToken(token) is JwtSecurityToken jwtToken))
            {
                return null;
            }

            return tokenHandler.ValidateToken(token, GetTokenValidationParameters(), out SecurityToken validatedToken);





        }





    }
}
