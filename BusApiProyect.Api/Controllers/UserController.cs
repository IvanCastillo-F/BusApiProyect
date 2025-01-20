﻿using BusApiProyect.Data.Interfaces;
using BusApiProyect.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusApiProyect.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            try
            {
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
                var existingUser = await _userRepository.GetUsersByIdAsyc(userToUpdate.Id);
                if (existingUser == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = "Record Not Found"
                    });
                }
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
                    message = "Record Not Found"
                });
            }
        }

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
    }
}
