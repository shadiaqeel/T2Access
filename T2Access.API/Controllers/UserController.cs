using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using T2Access.API.Resources;
using T2Access.BLL.Services;
using T2Access.Models;
using T2Access.Security.Tokenization.Models;
using T2Access.Security.Tokenization.Services;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace T2Access.API.Controllers
{

    // [Route("api/user/{action}")]

    public class UserController : ApiBaseController
    {
       // private readonly IUserService userService = new UserService();
        private readonly IUserService userService ;
        private readonly IAuthService authService;

        //! ===========================================================================
        public UserController(IUserService userService =null , IAuthService authService = null)
        {
            this.userService = userService ?? new UserService() ;
            this.authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        //! ===========================================================================

        [HttpPost]
        [AllowAnonymous]
        [Produces(typeof(string))]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {

            var response = await userService.LoginAsync(loginModel);
            var user = response.Data;

            if (!response.Success)
            {
                return BadRequest(response.ErrorMessage);
            }



            var cliamList = new List<Claim>
                { new Claim("userId", user.Id.ToString()),
                    new Claim("username", user.UserName),
                    new Claim("firstName", user.FirstName),
                    new Claim("lastName", user.LastName)
                };



            if (Enum.IsDefined(typeof(UserStatus), user.Status))
            {

                cliamList.Add(new Claim("roles","User"));
                cliamList.Add(new Claim("roles", ((UserStatus)user.Status).ToString() ));
                //cliamList.Add(new Claim("Role", $"{(UserStatus)user.Status},User"));
            }
            else
            {
                cliamList.Add(new Claim("Role", $"{(UserStatus)0},User"));
            }

            var token = authService.GenerateToken(new JWTContainerModel()
            {

                ExpireMinutes = DateTime.Now.AddMinutes(15).Minute,
                Claims = cliamList.ToArray()

            });

            return Ok(token);

        }



        #region Admin Operations

        [HttpPost]
        [Produces(typeof(string))]
        public async Task<IActionResult> SignUp(SignUpUserModel user)
        {

            var response = await userService.CreateAsync(user);
            return response.Success ? 
                Ok(response.Data) :
                (IActionResult)BadRequest(response.ErrorMessage);
        }




        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Produces(typeof(UserListResponse))]
        public async Task<IActionResult> GetListWithFilter([FromQuery]FilterUserModel filter)
        {
            if (filter == null)
            {
                return BadRequest(Resource.FilterMiss);
            }

            var response = await userService.GetListAsync(filter);

            return response.Success ?
                Ok(response.Data) :
                (IActionResult)NotFound(response.ErrorMessage);

        }




        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Produces(typeof(UserDto))]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (id == null)
            {
                return BadRequest (Resource.FilterMiss);
            }

            var response = await userService.GetByIdAsync(id);
            return response.Success ?
                Ok(response.Data) :
               (IActionResult)NotFound (response);


        }



        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Produces(typeof(string))]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await userService.DeleteAsync(id);
            return response.Success ?
                Ok(response.Data) :
                (IActionResult)BadRequest(response.ErrorMessage);

        }



        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Produces(typeof(string))]
        public async Task<IActionResult> Edit(Guid id, [FromBody] UpdateUserModel model)
        {
            model.Id = id;
            var response = await userService.EditAsync(model);
            return (response.Success) ?
                Ok(response.Data) :
               (IActionResult)BadRequest(response.ErrorMessage);

        }



        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Produces(typeof(List<string>))]
        public async Task<IActionResult> ResetPassword(Guid id, [FromBody] ResetPasswordModel model)
        {
            model.Id = id;

            var response = await userService.ResetPasswordAsync(model);

            return response.Success ?
                Ok(response.Data) :
                (IActionResult)BadRequest(response.ErrorMessage);

        }




        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Produces(typeof(string))]
        public async Task<IActionResult> AssignToGate(UserGateModel userGate)
        {


            var response = await userService.AssignAsync(userGate);

            return response.Success ?
                Ok(response.Data) :
                (IActionResult)BadRequest(response.ErrorMessage);
        }




        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Produces(typeof(string))]
        public async Task<IActionResult> UnassignToGate(UserGateModel userGate)
        {


            var response = await userService.UnassignAsync(userGate);
            return (response.Success) ?
                Ok(response.Data) :
                (IActionResult)BadRequest(response.ErrorMessage);
        }





        #endregion




    }


}





