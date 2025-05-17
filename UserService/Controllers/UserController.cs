using Contracts;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOObjects;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using UserService.ActionFilters;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IServiceManager _service;
        public UserController(IServiceManager service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _service.UserService.GetAllUsersAsync();

            return Ok(users);
        }


        [HttpGet("{id:guid}", Name = "UserById")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _service.UserService.GetUserAsync(id);

            return Ok(user);
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _service.UserService.DeleteUser(id);

            return NoContent();
        }


        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserForUpdateDTO userForUpdate)
        {
            await _service.UserService.UpdateUser(id, userForUpdate);

            return NoContent();
        }


        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> PartiallyUpdateProduct(Guid id, [FromBody] JsonPatchDocument<UserForUpdateDTO> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest("The patchDoc is null.");

            var result = await _service.UserService.GetUserForPatialUpdate(id);
            patchDoc.ApplyTo(result.userForUpd);

            await _service.UserService.PartiallyUpdateUser(result.userEntity, result.userForUpd);

            return NoContent();
        }
    }
}
