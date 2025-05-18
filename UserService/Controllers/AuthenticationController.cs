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

            return Content("To end registration check your email and follow the link.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDTO userForAuth)
        {
            if (!await _service.AuthenticationService.ValidateUser(userForAuth))
                return Unauthorized();

            return Ok(new TokenResponse
            {
                Token = await _service.AuthenticationService.CreateToken()
            });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
                return BadRequest();

            if (!Guid.TryParse(userId, out var id))
                return BadRequest("Id isn't in appropriate format");

            var user = await _service.AuthenticationService.GetUserToConfirmEmail(id);

            if (user == null)
                return NotFound("User not found.");

            var result = await _service.AuthenticationService.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
                return Ok("Email confirmed.");
            else
                return Content("Couldn't confirm email.");
        }
    }
}
