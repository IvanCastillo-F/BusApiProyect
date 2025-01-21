using BusApiProyect.Data.Interfaces;
using BusApiProyect.Data.Models;
using BusApiProyect.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusApiProyect.Api.Controllers
{
    [Route("api/bus")]
    [ApiController]
    public class BusController : ControllerBase
    {
        private readonly IBusRepository _busRepository;
        private readonly ILogger<BusController> _logger;

        public BusController(IBusRepository busRepository, ILogger<BusController> logger)
        {
            _busRepository = busRepository;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddBus(Bus bus)
        {
            try
            {
                // Check if the bus number already exists
                var isDuplicate = await _busRepository.IsBusNumberDuplicateAsync(bus.BusNumber);
                if (isDuplicate)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        message = "A bus with this number already exists."
                    });
                }

                // Create the bus if no duplicate is found
                var createdBus = await _busRepository.CreateBusAsync(bus);
                return CreatedAtAction(nameof(AddBus), createdBus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateBus(Bus busToUpdate)
        {
            try
            {
                // Check if the bus exists
                var existingBus = await _busRepository.GetBusByIdAsyc(busToUpdate.Id);
                if (existingBus == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = "Record Not Found"
                    });
                }

                // Check if the new bus number belongs to another bus
                var isDuplicate = await _busRepository.IsBusNumberDuplicateForOtherBusAsync(busToUpdate.BusNumber, busToUpdate.Id);
                if (isDuplicate)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        message = "A bus with this number already exists."
                    });
                }

                // Update the bus details
                existingBus.BusNumber = busToUpdate.BusNumber;
                existingBus.Capacity = busToUpdate.Capacity;
                existingBus.CurrentStatus = busToUpdate.CurrentStatus;
                await _busRepository.UpdateBusAsync(existingBus);
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
        public async Task<IActionResult> DeleteBus(int id)
        {
            try
            {
                var existingBus = await _busRepository.GetBusByIdAsyc(id);
                if (existingBus == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = "Record Not Found"
                    });
                }
                await _busRepository.DeleteBusAsync(existingBus);
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
        public async Task<IActionResult> GetBuses(int id)
        {
            try
            {
                var existingBus = await _busRepository.GetBusByIdAsyc(id);
                if (existingBus == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = "Record Not Found"
                    });
                }
                return Ok(existingBus);
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
        public async Task<IActionResult> GetBuses()
        {
            try
            {
                var buses = await _busRepository.GetAllBusesAsyc();
                return Ok(buses);
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
