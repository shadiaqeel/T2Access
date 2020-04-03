﻿using System;
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

    //  [Route("api/gate/{action}")]
    public class GateController : BaseController
    {
        private readonly IGateService gateService = new GateService();


        [HttpPost]
        [ResponseType(typeof(string))]
        public HttpResponseMessage Login(LoginModel gate)
        {
            //if (!ModelState.IsValid)
            //    return Request.CreateResponse(HttpStatusCode.NotFound, ModelState);


            var response = gateService.Login(gate);

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



                var Token = AuthorizationFactory.GetAuthrization().GenerateToken(new JWTContainerModel()
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


        [HttpPost]
        [ResponseType(typeof(string))]
        public HttpResponseMessage SignUp(SignUpGateModel gate)
        {

            //if (!ModelState.IsValid)
            //    return Request.CreateResponse(HttpStatusCode.NotFound, ModelState);


            var response = gateService.Create(gate);
            return (response.Success) ?
                Request.CreateResponse(HttpStatusCode.OK, response.Data) :
                Request.CreateResponse(HttpStatusCode.NotFound, response.ErrorMessage);



        }


        [HttpGet]
        [CustomAuthorize(Roles = "Admin")]
        [ResponseType(typeof(GateListResponse))]
        public HttpResponseMessage GetListWithFilter([FromUri]FilterGateModel filter)
        {
            if (filter == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Resource.FilterMiss);

            }

            var response = gateService.GetListWithFilter(filter);

            if (response.Success)
            {
                return Request.CreateResponse(HttpStatusCode.OK, response.Data);
            }

            return Request.CreateResponse(HttpStatusCode.NotFound, response.ErrorMessage);

        }


        [HttpGet]
        [CustomAuthorize(Roles = "Admin")]
        [ResponseType(typeof(ServiceResponse<List<GateDto>>))]
        public HttpResponseMessage GetCheckedListByUserId(Guid userId)
        {
            if (userId == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, Resource.FilterMiss);

            }


            var response = gateService.GetCheckedListByUserId(userId);
            return response.Success ?
                Request.CreateResponse(HttpStatusCode.OK, response.Data) :
                Request.CreateResponse(HttpStatusCode.NotFound, response.ErrorMessage);

        }


        [HttpDelete()]
        [CustomAuthorize(Roles = "Admin")]
        [ResponseType(typeof(string))]
        public HttpResponseMessage Delete(Guid id)
        {
            var response = gateService.Delete(id);
            return (response.Success) ?
                Request.CreateResponse(HttpStatusCode.OK, response.Data) :
                Request.CreateResponse(HttpStatusCode.BadRequest, response.ErrorMessage);

        }


        [HttpPut]
        [CustomAuthorize(Roles = "Admin")]
        [ResponseType(typeof(string))]
        public HttpResponseMessage Edit(Guid id, [FromBody] GateModel model)
        {
            model.Id = id;
            var response = gateService.Edit(model);
            return (response.Success) ?
                Request.CreateResponse(HttpStatusCode.OK, response.Data) :
                Request.CreateResponse(HttpStatusCode.BadRequest, response.ErrorMessage);

        }


        [HttpPut]
        [CustomAuthorize(Roles = "Admin")]
        [ResponseType(typeof(string))]
        public HttpResponseMessage ResetPassword(Guid id, [FromBody] ResetPasswordModel model)
        {
            model.Id = id;
            var response = gateService.ResetPassword(model);

            return (response.Success) ?
                Request.CreateResponse(HttpStatusCode.OK, response.Data) :
                Request.CreateResponse(HttpStatusCode.BadRequest, response.ErrorMessage);

        }


    }
}
