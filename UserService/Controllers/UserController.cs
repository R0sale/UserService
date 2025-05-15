using Contracts;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOObjects;
using Microsoft.AspNetCore.JsonPatch;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly IServiceManager _service;
        public UserController(IServiceManager service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _service.UserService.GetAllUsersAsync(false);

            return Ok(users);
        }

        [HttpGet("{id:guid}", Name = "UserById")]
        public async Task<IActionResult> GetAllUsers(Guid id)
        {
            var user = await _service.UserService.GetUserAsync(id ,false);

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserForCreationDTO userForCreation)
        {
            var user = await _service.UserService.CreateUser(userForCreation);

            return CreatedAtRoute("UserById", new {id = user.Id}, user);
        }

        [HttpDelete("id:guid")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _service.UserService.DeleteUser(id, false);

            return NoContent();
        }


        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserForUpdateDTO userForUpdate)
        {
            await _service.UserService.UpdateUser(id, userForUpdate, true);

            return NoContent();
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> PartiallyUpdateProduct(Guid id, [FromBody] JsonPatchDocument<UserForUpdateDTO> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            var result = await _service.UserService.GetUserForPatialUpdate(id, true);
            patchDoc.ApplyTo(result.userForUpd);

            await _service.UserService.SaveChangesForPatrialUpdate(result.userForUpd, result.userEntity);
            return NoContent();
        }
    }
}
