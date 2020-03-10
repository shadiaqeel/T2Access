using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;
using Ole5.Tokenization.Models;
using T2Access.API.Attributes;
using T2Access.API.Helper;
using T2Access.BLL.Services;
using T2Access.Models;


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
            if (!ModelState.IsValid)
                return Request.CreateResponse(HttpStatusCode.NotFound, ModelState);


            var user = userService.Login(loginModel);


            if (user != null)
            {
                List<Claim> cliamList = new List<Claim>();
                cliamList.Add(new Claim("UserId", user.Id.ToString()));
                cliamList.Add(new Claim("UserName", user.UserName));
                cliamList.Add(new Claim("FirstName", user.FirstName));
                cliamList.Add(new Claim("LastName", user.LastName));
                cliamList.Add(new Claim("Role", "User"));



                var Token = AuthrizationFactory.GetAuthrization().GenerateToken(new JWTContainerModel()
                {

                    ExpireMinutes = DateTime.Now.AddMinutes(15).Minute,
                    Claims = cliamList.ToArray()

                });

                return Request.CreateResponse(HttpStatusCode.OK, Token);

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Username isn't exist");

            }


        }



        [HttpPost]
        [ResponseType(typeof(UserSignUpModel))]
        public HttpResponseMessage SignUp(UserSignUpModel user)
        {

            if (!ModelState.IsValid)
                return Request.CreateResponse(HttpStatusCode.NotFound, ModelState);




            if (userService.CheckUserName(user.UserName))
            {
                if (userService.Create(user))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, user);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Signup process failed");

                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Signup process failed , Username is Used ");


            }



        }




        [HttpPost]
        [CustomAuthorize(Roles = "User")]
        [ResponseType(typeof(UserGateModel))]
        public HttpResponseMessage AssignToGate(UserGateModel userGate)
        {


            if (userService.Assign(userGate))
            {
                return Request.CreateResponse(HttpStatusCode.OK, userGate);
            }
            else
                return Request.CreateResponse(HttpStatusCode.NotFound, "Assignment process Failed");
        }


        [HttpPost]
        [CustomAuthorize(Roles = "User")]
        [ResponseType(typeof(UserGateModel))]
        public HttpResponseMessage UnassignToGate(UserGateModel userGate)
        {


            if (userService.Unassign(userGate))
            {
                return Request.CreateResponse(HttpStatusCode.OK, userGate);
            }
            else
                return Request.CreateResponse(HttpStatusCode.NotFound, "Unassignment process Failed");
        }

    }


}





