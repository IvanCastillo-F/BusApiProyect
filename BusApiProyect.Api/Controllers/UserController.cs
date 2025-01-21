using BusApiProyect.Data.Interfaces;
using BusApiProyect.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace BusApiProyect.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;
        private readonly string _secretKey;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger, IConfiguration config)
        {
            _userRepository = userRepository;
            _logger = logger;
            _secretKey = config.GetSection("settings").GetSection("secretkey").ToString();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            try
            {
                // Check if the email already exists
                var isDuplicate = await _userRepository.IsEmailDuplicateAsync(user.Email);
                if (isDuplicate)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        message = "A user with this email already exists."
                    });
                }

                // Create the user if no duplicate is found
                var createdUser = await _userRepository.CreateUserAsync(user);
                return CreatedAtAction(nameof(AddUser), createdUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(User userToUpdate)
        {
            try
            {
                // Check if the user exists
                var existingUser = await _userRepository.GetUsersByIdAsyc(userToUpdate.Id);
                if (existingUser == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = "Record Not Found"
                    });
                }

                // Check if the new email belongs to another user
                var isDuplicate = await _userRepository.IsEmailDuplicateForOtherUserAsync(userToUpdate.Email, userToUpdate.Id);
                if (isDuplicate)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        message = "A user with this email already exists."
                    });
                }

                // Update the user's details
                existingUser.Name = userToUpdate.Name;
                existingUser.Email = userToUpdate.Email;
                existingUser.Password = userToUpdate.Password;
                await _userRepository.UpdateUserAsync(existingUser);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = 500,
                    message = "An unexpected error occurred."
                });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var existingUser = await _userRepository.GetUsersByIdAsyc(id);
                if (existingUser == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = "Record Not Found"
                    });
                }
                await _userRepository.DeleteUserAsync(existingUser);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = 500,
                    message = "Record Not Found"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsers(int id)
        {
            try
            {
                var existingUser = await _userRepository.GetUsersByIdAsyc(id);
                if (existingUser == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = "Record Not Found"
                    });
                }
                return Ok(existingUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = 500,
                    message = "Record Not Found"
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _userRepository.GetAllUsersAsyc();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = 500,
                    message = "Record Not Found"
                });
            }
        }

        [HttpPost]
        [Route("Validate/{email}/{password}")]
        public async Task<IActionResult> ValidateUser(string email, string password)
        {
            try
            {
                var existingUser = await _userRepository.GetUsersByCredentialsAsyc(email,password);
                if (existingUser == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = "Record Not Found"
                    });
                }
                if (existingUser.IsAdmin == true)
                {
                    var keyBytes = Encoding.ASCII.GetBytes(_secretKey);
                    var claims = new ClaimsIdentity();

                    claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, existingUser.Email));

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = claims,
                        Expires = DateTime.UtcNow.AddMinutes(5),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

                    string createdToken = tokenHandler.WriteToken(tokenConfig);

                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        StatusCode = 200,
                        token = createdToken
                    }); ;
                }
                else
                {
                    return Unauthorized("User is Not Admin");
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = 500,
                    message = "Record Not Found"
                });
            }
            
        }
    }
}
