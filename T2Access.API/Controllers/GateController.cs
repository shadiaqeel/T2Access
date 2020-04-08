using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using T2Access.Models.Dtos;
using T2Access.Security.Tokenization.Models;

namespace T2Access.API.Controllers
{

    //  [Route("api/gate/{action}")]
    public class GateController : BaseController
    {
        private readonly IGateService gateService = new GateService();


        [HttpPost]
        [ResponseType(typeof(string))]
        public async Task<HttpResponseMessage> Login(LoginModel gate)
        {

            var response = await gateService.LoginAsync(gate);

            if (response.Success)
            {
                var _gate = response.Data;

                List<Claim> cliamList = new List<Claim>
                {
                    new Claim("GateId", _gate.Id.ToString()),
                    new Claim("UserName", _gate.UserName),
                    new Claim("NameAr", _gate.NameAr),
                    new Claim("NameEn", _gate.NameEn),
                    new Claim("Role", "Gate")
                };

                string Token;

                try
                {
                    Token = AuthorizationFactory.GetAuthorization().GenerateToken(new JWTContainerModel()
                    {

                        ExpireMinutes = DateTime.Now.AddMinutes(15).Minute,
                        Claims = cliamList.ToArray()

                    });
                }
                catch (Exception e)
                {
                    Trace.WriteLine($" {e.GetType()}   :    {e.Message }  ");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, Resource.NotAuthorized);

                }

                return Request.CreateResponse(HttpStatusCode.OK, Token);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, Resource.UserNotExist);
            }
        }



        //========================================================================= Admin Region


        [HttpPost]
        [ResponseType(typeof(string))]
        public async Task<HttpResponseMessage> SignUp(SignUpGateModel gate)
        {


            var response = await gateService.CreateAsync(gate);
            return response.Success ?
                Request.CreateResponse(HttpStatusCode.OK, response.Data) :
                Request.CreateResponse(HttpStatusCode.NotFound, response.ErrorMessage);



        }


        [HttpGet]
        [CustomAuthorize(Roles = "Admin")]
        [ResponseType(typeof(GateListResponse))]
        public async Task<HttpResponseMessage> GetListWithFilter([FromUri]FilterGateModel filter)
        {
            if (filter == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Resource.FilterMiss);

            }

            var response = await gateService.GetListWithFilterAsync(filter);

            return response.Success
                ? Request.CreateResponse(HttpStatusCode.OK, response.Data)
                : Request.CreateResponse(HttpStatusCode.NotFound, response.ErrorMessage);
        }


        [HttpGet]
        [CustomAuthorize(Roles = "Admin")]
        [ResponseType(typeof(ServiceResponse<ListResponse<CheckedGateDto>>))]
        public async Task<HttpResponseMessage> GetCheckedListByUserId([FromUri]FilterUserModel filter)
        {
            if (filter == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Resource.FilterMiss);

            }


            var response = await gateService.GetCheckedListByUserIdAsync(filter);
            return response.Success ?
                Request.CreateResponse(HttpStatusCode.OK, response.Data) :
                Request.CreateResponse(HttpStatusCode.NotFound, response.ErrorMessage);

        }


        [HttpDelete()]
        [CustomAuthorize(Roles = "Admin")]
        [ResponseType(typeof(string))]
        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            var response = await gateService.DeleteAsync(id);
            return (response.Success) ?
                Request.CreateResponse(HttpStatusCode.OK, response.Data) :
                Request.CreateResponse(HttpStatusCode.BadRequest, response.ErrorMessage);

        }


        [HttpPut]
        [CustomAuthorize(Roles = "Admin")]
        [ResponseType(typeof(string))]
        public async Task<HttpResponseMessage> Edit(Guid id, [FromBody] GateModel model)
        {
            model.Id = id;
            var response = await gateService.EditAsync(model);
            return (response.Success) ?
                Request.CreateResponse(HttpStatusCode.OK, response.Data) :
                Request.CreateResponse(HttpStatusCode.BadRequest, response.ErrorMessage);

        }


        [HttpPut]
        [CustomAuthorize(Roles = "Admin")]
        [ResponseType(typeof(string))]
        public async Task<HttpResponseMessage> ResetPassword(Guid id, [FromBody] ResetPasswordModel model)
        {
            model.Id = id;
            var response = await gateService.ResetPasswordAsync(model);

            return (response.Success) ?
                Request.CreateResponse(HttpStatusCode.OK, response.Data) :
                Request.CreateResponse(HttpStatusCode.BadRequest, response.ErrorMessage);

        }


    }
}
