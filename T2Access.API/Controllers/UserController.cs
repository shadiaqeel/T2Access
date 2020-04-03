using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
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
        public HttpResponseMessage Login(LoginModel loginModel)
        {
            //if (!ModelState.IsValid)
            //    return Request.CreateResponse(HttpStatusCode.NotFound, ModelState);



            var response = userService.Login(loginModel);
            var user = response.Data;

            if (response.Success)
            {

                List<Claim> cliamList = new List<Claim>
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

                var token = AuthorizationFactory.GetAuthrization().GenerateToken(new JWTContainerModel()
                {

                    ExpireMinutes = DateTime.Now.AddMinutes(15).Minute,
                    Claims = cliamList.ToArray()

                });

                return Request.CreateResponse(HttpStatusCode.OK, token);


            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, response.ErrorMessage);

        }








        #region Admin Operations

        [HttpPost]
        [ResponseType(typeof(string))]
        public HttpResponseMessage SignUp(SignUpUserModel user)
        {

            var response = userService.Create(user);
            return (response.Success) ?
                Request.CreateResponse(HttpStatusCode.OK, response.Data) :
                Request.CreateResponse(HttpStatusCode.BadRequest, response.ErrorMessage);


        }




        [HttpGet]
        [CustomAuthorize(Roles = "Admin,User")]
        [ResponseType(typeof(UserListResponse))]
        public HttpResponseMessage GetListWithFilter([FromUri]FilterUserModel filter)
        {
            if (filter == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Resource.FilterMiss);
            }

            var response = userService.GetList(filter);
            return (response.Success) ?
                Request.CreateResponse(HttpStatusCode.OK, response.Data) :
                Request.CreateResponse(HttpStatusCode.NotFound, response.ErrorMessage);

        }




        [HttpGet]
        [CustomAuthorize(Roles = "Admin,User")]
        [ResponseType(typeof(UserDto))]
        public HttpResponseMessage GetById(Guid id)
        {
            if (id == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Resource.FilterMiss);
            }

            var response = userService.GetById(id);
            return (response.Success) ?
                Request.CreateResponse(HttpStatusCode.OK, response.Data) :
                Request.CreateResponse(HttpStatusCode.NotFound, response);


        }



        [HttpDelete]
        [CustomAuthorize(Roles = "Admin,User")]
        [ResponseType(typeof(string))]
        public HttpResponseMessage Delete(Guid id)
        {
            var response = userService.Delete(id);
            return (response.Success) ?
                Request.CreateResponse(HttpStatusCode.OK, response.Data) :
                Request.CreateResponse(HttpStatusCode.BadRequest, response.ErrorMessage);

        }



        [HttpPut]
        [CustomAuthorize(Roles = "Admin,User")]
        [ResponseType(typeof(string))]
        public HttpResponseMessage Edit(Guid id, [FromBody] UpdateUserModel model)
        {
            model.Id = id;
            var response = userService.Edit(model);
            return (response.Success) ?
                Request.CreateResponse(HttpStatusCode.OK, response.Data) :
                Request.CreateResponse(HttpStatusCode.BadRequest, response.ErrorMessage);

        }



        [HttpPut]
        [CustomAuthorize(Roles = "Admin,User")]
        [ResponseType(typeof(List<string>))]
        public HttpResponseMessage ResetPassword(Guid id, [FromBody] ResetPasswordModel model)
        {
            model.Id = id;

            var response = userService.ResetPassword(model);
            return (response.Success) ?
                Request.CreateResponse(HttpStatusCode.OK, response.Data) :
                Request.CreateResponse(HttpStatusCode.BadRequest, response.ErrorMessage);

        }









        [HttpPost]
        [CustomAuthorize(Roles = "Admin")]
        [ResponseType(typeof(string))]
        public HttpResponseMessage AssignToGate(UserGateModel userGate)
        {


            var response = userService.Assign(userGate);
            return (response.Success) ?
                Request.CreateResponse(HttpStatusCode.OK, response.Data) :
                Request.CreateResponse(HttpStatusCode.BadRequest, response.ErrorMessage);
        }




        [HttpPost]
        [CustomAuthorize(Roles = "Admin")]
        [ResponseType(typeof(string))]
        public HttpResponseMessage UnassignToGate(UserGateModel userGate)
        {


            var response = userService.Unassign(userGate);
            return (response.Success) ?
                Request.CreateResponse(HttpStatusCode.OK, response.Data) :
                Request.CreateResponse(HttpStatusCode.BadRequest, response.ErrorMessage);
        }





        #endregion




    }


}





