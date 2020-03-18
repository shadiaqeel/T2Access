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


            var user = userService.Login(loginModel);


            if (user != null)
            {
                List<Claim> cliamList = new List<Claim>();
                cliamList.Add(new Claim("UserId", user.Id.ToString()));
                cliamList.Add(new Claim("Username", user.UserName));
                cliamList.Add(new Claim("FirstName", user.FirstName));
                cliamList.Add(new Claim("LastName", user.LastName));
                cliamList.Add(new Claim("Role", "User"));



                var token = AuthrizationFactory.GetAuthrization().GenerateToken(new JWTContainerModel()
                {

                    ExpireMinutes = DateTime.Now.AddMinutes(15).Minute,
                    Claims = cliamList.ToArray()

                });


                return Request.CreateResponse(HttpStatusCode.OK, token);

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, Resource.UserNotExist);

            }


        }








        #region Admin Operations

        [HttpPost]
        [ResponseType(typeof(string))]
        public HttpResponseMessage SignUp(UserSignUpModel user)
        {

            //if (!ModelState.IsValid)
            //    return Request.CreateResponse(HttpStatusCode.NotFound, ModelState);




            if (userService.CheckUserName(user.UserName))
            {
                if (userService.Create(user))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, Resource.SignupSuccess);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, Resource.SignupFailed);

                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, Resource.UserExist);


            }



        }




        [HttpGet]
        [CustomAuthorize(Roles = "User")]
        [ResponseType(typeof(List<UserModel>))]
        public HttpResponseMessage GetListWithFilter([FromUri]UserFilterModel filter)
        {
            if (filter == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Resource.FilterMiss);
            }

            return Request.CreateResponse(HttpStatusCode.OK, userService.GetList(filter));

        }



        [HttpDelete()]
        [CustomAuthorize(Roles = "User")]
        [ResponseType(typeof(List<UserModel>))]
        public HttpResponseMessage Delete(Guid id )
        {
            if (userService.Delete(id))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Resource.DeleteFailed);
            }

            return Request.CreateResponse(HttpStatusCode.OK, Resource.DeleteSuccess);

        }












        [HttpPost]
        [CustomAuthorize(Roles = "User")]
        [ResponseType(typeof(string))]
        public HttpResponseMessage AssignToGate(UserGateModel userGate)
        {


            if (userService.Assign(userGate))
            {
                return Request.CreateResponse(HttpStatusCode.OK, Resource.AssignSuccess);
            }
            else
                return Request.CreateResponse(HttpStatusCode.NotFound, Resource.AssignFailed);
        }




        [HttpPost]
        [CustomAuthorize(Roles = "User")]
        [ResponseType(typeof(string))]
        public HttpResponseMessage UnassignToGate(UserGateModel userGate)
        {


            if (userService.Unassign(userGate))
            {
                return Request.CreateResponse(HttpStatusCode.OK, Resource.UnassignSuccess);
            }
            else
                return Request.CreateResponse(HttpStatusCode.NotFound, Resource.UnassignFailed);
        }





        #endregion




    }


}





