using Contracts;
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

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDTO userForAuth)
        {
            if (!await _service.AuthenticationService.ValidateUser(userForAuth))
                return Unauthorized();

            return Ok(new
            {
                Token = await _service.AuthenticationService.CreateToken()
            });
        }

    }
}
