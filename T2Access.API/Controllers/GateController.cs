using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using T2Access.API.Resources;
using T2Access.BLL.Services;
using T2Access.Models;
using T2Access.Models.Dtos;
using T2Access.Security.Tokenization.Models;
using T2Access.Security.Tokenization.Services;

namespace T2Access.API.Controllers
{

    //  [Route("api/gate/{action}")]
    public class GateController : ApiBaseController
    {
        private readonly IGateService _gateService;
        private readonly IAuthService _authService;


        public GateController(IGateService gateService, IAuthService authService)
        {
            _gateService = gateService ?? new GateService();
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));

        }

        [HttpPost]
        [Produces(typeof(string))]
        public async Task<IActionResult> Login(LoginModel gate)
        {

            var response = await _gateService.LoginAsync(gate);

            if (response.Success)
            {
                var _gate = response.Data;

                List<Claim> cliamList = new List<Claim>
                {
                    new Claim("gateId", _gate.Id.ToString()),
                    new Claim("userName", _gate.UserName),
                    new Claim("nameAr", _gate.NameAr),
                    new Claim("nameEn", _gate.NameEn),
                    new Claim("roles", "Gate")
                };

                string Token;

                try
                {
                    Token = _authService.GenerateToken(new JWTContainerModel()
                    {

                        ExpireMinutes = DateTime.Now.AddMinutes(15).Minute,
                        Claims = cliamList.ToArray()

                    });
                }
                catch (Exception e)
                {
                    Trace.WriteLine($" {e.GetType()}   :    {e.Message }  ");
                    return BadRequest(Resource.NotAuthorized);

                }

                return Ok(Token);
            }
            else
            {
                return NotFound(Resource.UserNotExist);
            }
        }



        //========================================================================= Admin Region


        [HttpPost]
        [Produces(typeof(string))]
        public async Task<IActionResult> SignUp(SignUpGateModel gate)
        {


            var response = await _gateService.CreateAsync(gate);

            return response.Success ?
                Ok(response.Data) :
                (IActionResult)NotFound(response.ErrorMessage);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Produces(typeof(GateListResponse))]
        public async Task<IActionResult> GetListWithFilter([FromQuery]FilterGateModel filter)
        {
            if (filter == null)
            {
                return BadRequest(Resource.FilterMiss);

            }

            var response = await _gateService.GetListWithFilterAsync(filter);

            return response.Success
                ? Ok(response.Data)
                : (IActionResult)NotFound(response.ErrorMessage);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Produces(typeof(ServiceResponse<ListResponse<CheckedGateDto>>))]
        public async Task<IActionResult> GetCheckedListByUserId([FromQuery]FilterUserModel filter)
        {
            if (filter == null)
            {
                return BadRequest(Resource.FilterMiss);

            }


            var response = await _gateService.GetCheckedListByUserIdAsync(filter);
            return response.Success ?
                Ok(response.Data) :
                (IActionResult)NotFound(response.ErrorMessage);

        }


        [HttpDelete()]
        [Authorize(Roles = "Admin")]
        [Produces(typeof(string))]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _gateService.DeleteAsync(id);
            return (response.Success) ?
                Ok(response.Data) :
               (IActionResult)BadRequest(response.ErrorMessage);

        }


        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Produces(typeof(string))]
        public async Task<IActionResult> Edit(Guid id, [FromBody] GateModel model)
        {
            model.Id = id;
            var response = await _gateService.EditAsync(model);
            return response.Success ?
                Ok(response.Data) :
               (IActionResult)BadRequest(response.ErrorMessage);

        }


        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Produces(typeof(string))]
        public async Task<IActionResult> ResetPassword(Guid id, [FromBody] ResetPasswordModel model)
        {
            model.Id = id;
            var response = await _gateService.ResetPasswordAsync(model);

            return response.Success ?
                Ok(response.Data) :
                (IActionResult)BadRequest(response.ErrorMessage);

        }


    }
}
