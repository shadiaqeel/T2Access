using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

using T2Access.API.Attributes;
using T2Access.API.Helper;
using T2Access.API.Resources;
using T2Access.BLL.Services;
using T2Access.Models;
using T2Access.Security.Tokenization.Models;

namespace T2Access.API.Controllers
{

    // [Route("api/user/{action}")]

    public class UserController : BaseController
    {
        private readonly IUserService userService = new UserService();



        [HttpPost]
        [ResponseType(typeof(string))]
        public async Task<HttpResponseMessage> Login(LoginModel loginModel)
        {

            var response = await userService.LoginAsync(loginModel);
            var user = response.Data;

            if (!response.Success)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, response.ErrorMessage);
            }



            var cliamList = new List<Claim>
                { new Claim("UserId", user.Id.ToString()),
                    new Claim("Username", user.UserName),
                    new Claim("FirstName", user.FirstName),
                    new Claim("LastName", user.LastName)
                };



            if (Enum.IsDefined(typeof(UserStatus), user.Status))
            {
                cliamList.Add(new Claim("Role", $"{(UserStatus)user.Status},User"));
            }
            else
            {
                cliamList.Add(new Claim("Role", $"{(UserStatus)0},User"));
            }

            var token = AuthorizationFactory.GetAuthorization().GenerateToken(new JWTContainerModel()
            {

                ExpireMinutes = DateTime.Now.AddMinutes(15).Minute,
                Claims = cliamList.ToArray()

            });

            return Request.CreateResponse(HttpStatusCode.OK, token);

        }








        #region Admin Operations

        [HttpPost]
        [ResponseType(typeof(string))]
        public async Task<HttpResponseMessage> SignUp(SignUpUserModel user)
        {

            var response = await userService.CreateAsync(user);
            return (response.Success) ?
                Request.CreateResponse(HttpStatusCode.OK, response.Data) :
                Request.CreateResponse(HttpStatusCode.BadRequest, response.ErrorMessage);


        }




        [HttpGet]
        [CustomAuthorize(Roles = "Admin,User")]
        [ResponseType(typeof(UserListResponse))]
        public async Task<HttpResponseMessage> GetListWithFilter([FromUri]FilterUserModel filter)
        {
            if (filter == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Resource.FilterMiss);
            }

            var response = await userService.GetListAsync(filter);
            return (response.Success) ?
                Request.CreateResponse(HttpStatusCode.OK, response.Data) :
                Request.CreateResponse(HttpStatusCode.NotFound, response.ErrorMessage);

        }




        [HttpGet]
        [CustomAuthorize(Roles = "Admin,User")]
        [ResponseType(typeof(UserDto))]
        public async Task<HttpResponseMessage> GetById(Guid id)
        {
            if (id == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Resource.FilterMiss);
            }

            var response = await userService.GetByIdAsync(id);
            return (response.Success) ?
                Request.CreateResponse(HttpStatusCode.OK, response.Data) :
                Request.CreateResponse(HttpStatusCode.NotFound, response);


        }



        [HttpDelete]
        [CustomAuthorize(Roles = "Admin,User")]
        [ResponseType(typeof(string))]
        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            var response = await userService.DeleteAsync(id);
            return (response.Success) ?
                Request.CreateResponse(HttpStatusCode.OK, response.Data) :
                Request.CreateResponse(HttpStatusCode.BadRequest, response.ErrorMessage);

        }



        [HttpPut]
        [CustomAuthorize(Roles = "Admin,User")]
        [ResponseType(typeof(string))]
        public async Task<HttpResponseMessage> Edit(Guid id, [FromBody] UpdateUserModel model)
        {
            model.Id = id;
            var response = await userService.EditAsync(model);
            return (response.Success) ?
                Request.CreateResponse(HttpStatusCode.OK, response.Data) :
                Request.CreateResponse(HttpStatusCode.BadRequest, response.ErrorMessage);

        }



        [HttpPut]
        [CustomAuthorize(Roles = "Admin,User")]
        [ResponseType(typeof(List<string>))]
        public async Task<HttpResponseMessage> ResetPassword(Guid id, [FromBody] ResetPasswordModel model)
        {
            model.Id = id;

            var response = await userService.ResetPasswordAsync(model);
            return (response.Success) ?
                Request.CreateResponse(HttpStatusCode.OK, response.Data) :
                Request.CreateResponse(HttpStatusCode.BadRequest, response.ErrorMessage);

        }




        [HttpPost]
        [CustomAuthorize(Roles = "Admin")]
        [ResponseType(typeof(string))]
        public async Task<HttpResponseMessage> AssignToGate(UserGateModel userGate)
        {


            var response = await userService.AssignAsync(userGate);
            return (response.Success) ?
                Request.CreateResponse(HttpStatusCode.OK, response.Data) :
                Request.CreateResponse(HttpStatusCode.BadRequest, response.ErrorMessage);
        }




        [HttpPost]
        [CustomAuthorize(Roles = "Admin")]
        [ResponseType(typeof(string))]
        public async Task<HttpResponseMessage> UnassignToGate(UserGateModel userGate)
        {


            var response = await userService.UnassignAsync(userGate);
            return (response.Success) ?
                Request.CreateResponse(HttpStatusCode.OK, response.Data) :
                Request.CreateResponse(HttpStatusCode.BadRequest, response.ErrorMessage);
        }





        #endregion




    }


}





