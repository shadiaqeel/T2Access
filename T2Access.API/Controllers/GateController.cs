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
using T2Access.Models.Dtos;
using T2Access.Security.Tokenization.Models;

namespace T2Access.API.Controllers
{

    [Route("api/gate/{action}")]
    public class GateController : BaseController
    {
        IGateService gateService = new GateService();


        [HttpPost]
        [ResponseType(typeof(ServiceResponse<string>))]
        public HttpResponseMessage Login(LoginModel gate)
        {
            //if (!ModelState.IsValid)
            //    return Request.CreateResponse(HttpStatusCode.NotFound, ModelState);


            var response = gateService.Login(gate);

            if (response.Success)
            {
                var _gate = response.Data;

                List<Claim> cliamList = new List<Claim>();
                cliamList.Add(new Claim("GateId", _gate.Id.ToString()));
                cliamList.Add(new Claim("UserName", _gate.UserName));
                cliamList.Add(new Claim("NameAr", _gate.NameAr));
                cliamList.Add(new Claim("NameEn", _gate.NameEn));
                cliamList.Add(new Claim("Role", "Gate"));



                var Token = AuthorizationFactory.GetAuthrization().GenerateToken(new JWTContainerModel()
                {

                    ExpireMinutes = DateTime.Now.AddMinutes(15).Minute,
                    Claims = cliamList.ToArray()

                });

                return Request.CreateResponse(HttpStatusCode.OK, new ServiceResponse<string>() { Data = Token });
            }
            else
            
                return Request.CreateResponse(HttpStatusCode.NotFound, new ServiceResponse<string>() { Success = false, Message = Resource.UserNotExist });

            


        }


        [HttpPost]
        [ResponseType(typeof(ServiceResponse<string>))]
        public HttpResponseMessage SignUp(SignUpGateModel gate)
        {

            //if (!ModelState.IsValid)
            //    return Request.CreateResponse(HttpStatusCode.NotFound, ModelState);


            var response = gateService.Create(gate);

                if (response.Success)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, response);

                }



        }


        [HttpGet]
        [CustomAuthorize(Roles = "Admin")]
        [ResponseType(typeof(ServiceResponse<GateListResponse>))]
        public HttpResponseMessage GetListWithFilter([FromUri]FilterGateModel filter)
        {
            if (filter == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ServiceResponse<string>(){ Success=false , Message = Resource.FilterMiss } );

            }
            return Request.CreateResponse(HttpStatusCode.OK, gateService.GetListWithFilter(filter));

        }


        [HttpGet]
        [CustomAuthorize(Roles = "Admin")]
        [ResponseType(typeof(ServiceResponse<List<GateDto>>))]
        public HttpResponseMessage GetCheckedListByUserId(Guid userId)
        {
            if (userId == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ServiceResponse<string>() { Success = false, Message = Resource.FilterMiss });

            }
            return Request.CreateResponse(HttpStatusCode.OK, gateService.GetCheckedListByUserId(userId));

        }


        [HttpDelete()]
        [CustomAuthorize(Roles = "Admin")]
        [ResponseType(typeof(ServiceResponse<string>))]
        public HttpResponseMessage Delete(Guid id)
        {
            var response = gateService.Delete(id);
            if (response.Success)
            {
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, response );

        }


        [HttpPut]
        [CustomAuthorize(Roles = "Admin")]
        [ResponseType(typeof(ServiceResponse<string>))]
        public HttpResponseMessage Edit(Guid id, [FromBody] GateModel model)
        {
            model.Id = id;
            var response = gateService.Edit(model);
            if (response.Success)
            {
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, response);

        }


        [HttpPut]
        [CustomAuthorize(Roles = "Admin")]
        [ResponseType(typeof(ServiceResponse<string>))]
        public HttpResponseMessage ResetPassword(Guid id, [FromBody] ResetPasswordModel model)
        {
            model.Id = id;
            var response = gateService.ResetPassword(model);
                if (response.Success)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

            return Request.CreateResponse(HttpStatusCode.BadRequest, response);

        }


    }
}
