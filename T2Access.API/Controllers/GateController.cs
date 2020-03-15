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

    [RoutePrefix("api/gate")]
    public class GateController : BaseController
    {
        IGateService gateService = new GateService();


        [Route("login")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public HttpResponseMessage Login(LoginModel gate)
        {
            //if (!ModelState.IsValid)
            //    return Request.CreateResponse(HttpStatusCode.NotFound, ModelState);


            var _gate = gateService.Login(gate);


            if (_gate != null)
            {
                List<Claim> cliamList = new List<Claim>();
                cliamList.Add(new Claim("GateId", _gate.Id.ToString()));
                cliamList.Add(new Claim("UserName", _gate.UserName));
                cliamList.Add(new Claim("NameAr", _gate.NameAr));
                cliamList.Add(new Claim("NameEn", _gate.NameEn));
                cliamList.Add(new Claim("Role", "Gate"));



                var Token = AuthrizationFactory.GetAuthrization().GenerateToken(new JWTContainerModel()
                {

                    ExpireMinutes = DateTime.Now.AddMinutes(15).Minute,
                    Claims = cliamList.ToArray()

                });

                return Request.CreateResponse(HttpStatusCode.OK, Token);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, Resource.UserNotExist);

            }


        }


        [Route("signup")]
        [HttpPost]
        [ResponseType(typeof(string))]
        public HttpResponseMessage SignUp(GateSignUpModel gate)
        {

            //if (!ModelState.IsValid)
            //    return Request.CreateResponse(HttpStatusCode.NotFound, ModelState);



            if (gateService.CheckUserName(gate.UserName))
            {
                if (gateService.Create(gate))
                {
                    return Request.CreateResponse(HttpStatusCode.OK,  Resource.SignupSuccess);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, Resource.SignupFailed);

                }
            }
            else {

                return Request.CreateResponse(HttpStatusCode.NotFound,Resource.UserExist);


            }


        }


        [HttpGet]
        [CustomAuthorize(Roles = "User")]
        [ResponseType(typeof(List<UserModel>))]
        public HttpResponseMessage GetListWithFilter([FromUri]GateFilterModel filter)
        {
            if (filter == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Resource.FilterMiss);

            }
            return Request.CreateResponse(HttpStatusCode.OK, gateService.GetListWithFilter(filter));

        }







    }
}
