using Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOObjects;
using UserService.ActionFilters;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServiceManager _service;

        public AuthenticationController(IServiceManager service) => _service = service;


        [HttpPost]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDTO userForRegistration)
        {
            var result = await _service.AuthenticationService.RegisterUser(userForRegistration);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest();
            }

            var user = await _service.UserService.GetUserByEmailAsync(userForRegistration.Email);

            return CreatedAtRoute(routeName: "UserById",
                                  routeValues: new { id = user.Id },
                                  value: new { message = "To end registration check your email and follow the link." });
        }

        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDTO userForAuth)
        {
            await _service.AuthenticationService.ValidateUser(userForAuth);

            return Ok(new TokenResponse
            {
                Token = await _service.AuthenticationService.CreateToken()
            });
        }

        [HttpGet("confirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            var result = await _service.AuthenticationService.ConfirmEmailAsync(userId, code);

            if (result.Succeeded)
                return Ok("Email confirmed.");
            else
                return Content("Couldn't confirm email.");
        }

        [HttpPost("restorePassword")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<IActionResult> RestorePassword([FromBody] RestorePasswordUserDTO restoreUser)
        {
            await _service.AuthenticationService.RestorePassword(restoreUser);

            return Content("To change your password check the email and follow the link.");
        }

        [HttpGet("changePassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword(string userId, string code, string newPassword)
        {
            var result = await _service.AuthenticationService.ConfirmPasswordAsync(userId, code, newPassword);

            if (result.Succeeded)
                return Ok("Password changed.");
            else
                return Content("Couldn't change your password.");
        }
    }
}
