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

    [Route("api/user/{action}")]
    public class UserController : BaseController
    {
        IUserService userService = new UserService();



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

                    List<Claim> cliamList = new List<Claim>();
                    cliamList.Add(new Claim("UserId", user.Id.ToString()));
                    cliamList.Add(new Claim("Username", user.UserName));
                    cliamList.Add(new Claim("FirstName", user.FirstName));
                    cliamList.Add(new Claim("LastName", user.LastName));



                    if (Enum.IsDefined(typeof(UserStatus), user.Status))
                        cliamList.Add(new Claim("Role", $"{(UserStatus)user.Status},User"));
                    else
                        cliamList.Add(new Claim("Role", $"{(UserStatus)0},User"));


                    var token = AuthorizationFactory.GetAuthrization().GenerateToken(new JWTContainerModel()
                    {

                        ExpireMinutes = DateTime.Now.AddMinutes(15).Minute,
                        Claims = cliamList.ToArray()

                    });

                    return Request.CreateResponse(HttpStatusCode.OK, new ServiceResponse<string>() { Data=token });


            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, response);

        }








        #region Admin Operations

        [HttpPost]
        [ResponseType(typeof(ServiceResponse<string>))]
        public HttpResponseMessage SignUp(SignUpUserModel user)
        {

            var response = userService.Create(user);
                if (response.Success)
                {
                    return Request.CreateResponse(HttpStatusCode.OK,response);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, response);
                }
            

        }




        [HttpGet]
        [CustomAuthorize(Roles = "Admin,User")]
        [ResponseType(typeof(ServiceResponse<UserListResponse>))]
        public HttpResponseMessage GetListWithFilter([FromUri]FilterUserModel filter)
        {
            if (filter == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,new ServiceResponse<UserListResponse>() { Success = false, Message = Resource.FilterMiss });
            }

 
            return Request.CreateResponse(HttpStatusCode.OK, userService.GetList(filter));

        }




        [HttpGet]
        [CustomAuthorize(Roles = "Admin,User")]
        [ResponseType(typeof(ServiceResponse<UserDto>))]
        public HttpResponseMessage GetById(Guid id)
        {
            if (id == null)
              return Request.CreateResponse(HttpStatusCode.BadRequest, new ServiceResponse<UserDto>() { Success = false, Message = Resource.FilterMiss });
            

            var  response = userService.GetById(id);

            if(response.Success)
                 return Request.CreateResponse(HttpStatusCode.OK, response);
 
            else
                return Request.CreateResponse(HttpStatusCode.NotFound, response);


        }



        [HttpDelete]
        [CustomAuthorize(Roles = "Admin,User")]
        [ResponseType(typeof(string))]
        public HttpResponseMessage Delete(Guid id)
        {
            var response = userService.Delete(id);
            if (response.Success)
            {
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, response);

        }



        [HttpPut]
        [CustomAuthorize(Roles = "Admin,User")]
        [ResponseType(typeof(ServiceResponse<string>))]
        public HttpResponseMessage Edit(Guid id, [FromBody] UpdateUserModel model)
        {
            model.Id = id;
            var response = userService.Edit(model);
            if (response.Success)
            {
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, response);

        }



        [HttpPut]
        [CustomAuthorize(Roles = "Admin,User")]
        [ResponseType(typeof(List<string>))]
        public HttpResponseMessage ResetPassword(Guid id, [FromBody] ResetPasswordModel model)
        {
            model.Id = id;

            var response = userService.ResetPassword(model);
            if (response.Success)
            {
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, response);

        }









        [HttpPost]
        [CustomAuthorize(Roles = "Admin")]
        [ResponseType(typeof(string))]
        public HttpResponseMessage AssignToGate(UserGateModel userGate)
        {


             var response = userService.Assign(userGate);
            if (response.Success)
            {
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, response);
        }




        [HttpPost]
        [CustomAuthorize(Roles = "Admin")]
        [ResponseType(typeof(string))]
        public HttpResponseMessage UnassignToGate(UserGateModel userGate)
        {


             var response = userService.Unassign(userGate);
            if (response.Success)
            {
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, response);
        }





        #endregion




    }


}





