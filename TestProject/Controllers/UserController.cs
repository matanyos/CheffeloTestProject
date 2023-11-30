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
        private readonly ILogger<UserController> logger;

        public UserController(IRepository<User, Guid> userRepository, ILogger<UserController> logger)
        {
            this.userRepository = userRepository;
            this.logger = logger;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get(Guid id)
        {
            try
            {
                var user = userRepository.Read(id);

                logger.LogInformation($"Returned user with ID {id}");
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogWarning(ex, $"User with ID {id} is not found");
                return NotFound($"User with ID {id} is not found");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Internal server error: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

                logger.LogInformation($"Created user with ID {user.UserId}");
                return CreatedAtAction(nameof(Get), new { id = user.UserId }, user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

                logger.LogInformation($"Updated user entity");
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogWarning(ex, $"User with ID {userModelDto.Id} is not found");
                return NotFound($"User with ID {userModelDto.Id} is not found");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Internal server error: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var user = userRepository.Read(id);

                userRepository.Remove(user);

                logger.LogInformation($"Deleted user with ID {id}");
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogWarning(ex, $"User with ID {id} is not found");
                return NotFound($"User with ID {id} is not found");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Internal server error: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
