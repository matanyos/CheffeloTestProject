using Microsoft.AspNetCore.Mvc;
using Work.ApiModels;
using Work.Database;
using Work.Interfaces;

namespace Work.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IRepository<User, Guid> userRepository;

        public UserController(IRepository<User, Guid> userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var user = userRepository.Read(id);
                return Ok(user);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"User with ID {id} is not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] UserModelDto userModelDto)
        {
            try
            {
                var user = new User
                {
                    UserId = Guid.NewGuid(),
                    UserName = userModelDto.Name,
                    Birthday = userModelDto.Birthdate
                };

                userRepository.Create(user);

                return CreatedAtAction(nameof(Get), new { id = user.UserId }, user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] UserModelDto userModelDto)
        {
            try
            {
                var user = new User
                {
                    UserId = userModelDto.Id,
                    UserName = userModelDto.Name,
                    Birthday = userModelDto.Birthdate
                };

                userRepository.Update(user);

                return Ok(user);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"User with ID {userModelDto.Id} is not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var user = userRepository.Read(id);

                userRepository.Remove(user);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"User with ID {id} is not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
