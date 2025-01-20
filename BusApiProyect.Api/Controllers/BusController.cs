using BusApiProyect.Data.Interfaces;
using BusApiProyect.Data.Models;
using BusApiProyect.Data.Repositories;
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


        [HttpPost]
        public async Task<IActionResult> AddBus(Bus bus)
        {
            try
            {
                var createdBus = await _busRepository.CreateBusAsync(bus);
                return CreatedAtAction(nameof(AddBus), createdBus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPut]
        public async Task<IActionResult> UpdateBus(Bus busToUpdate)
        {
            try
            {
                var existingBus = await _busRepository.GetBusByIdAsyc(busToUpdate.Id);
                if (existingBus == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = "Record Not Found"
                    });
                }
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
                    message = "Record Not Found"
                });
            }
        }


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
        public async Task<IActionResult> GetUsers()
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
