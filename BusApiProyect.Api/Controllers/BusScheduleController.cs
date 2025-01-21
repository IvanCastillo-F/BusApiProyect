using BusApiProyect.Data.DTO;
using BusApiProyect.Data.Interfaces;
using BusApiProyect.Data.Models;
using BusApiProyect.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusApiProyect.Api.Controllers
{
    [Route("api/schedule")]
    [ApiController]
    public class BusScheduleController : ControllerBase
    {
        private readonly IBusScheduleRepository _busScheduleRepository;
        private readonly ILogger<BusScheduleController> _logger;

        public BusScheduleController(IBusScheduleRepository busScheduleRepository, ILogger<BusScheduleController> logger)
        {
            _busScheduleRepository = busScheduleRepository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddBusSchedule(BusScheduleDTO scheduleDto)
        {
            try
            {
                // Map the DTO to the entity
                var schedule = new BusSchedule
                {
                    Id = scheduleDto.Id,
                    BusForScheduleId = scheduleDto.BusForScheduleId,
                    RouteScheduledId = scheduleDto.RouteScheduledId,
                    DepartureTime = scheduleDto.DepartureTime,
                    Arrival_Time = scheduleDto.ArrivalTime // Ensure this matches the property in your model
                };

                // Validate schedule constraints
                bool isConflict = await _busScheduleRepository.IsScheduleConflictAsync(schedule);
                if (isConflict)
                {
                    return BadRequest("The schedule conflicts with an existing schedule for this bus.");
                }

                // Create the new schedule if no conflicts are found
                var createdSchedule = await _busScheduleRepository.CreateBusScheduleAsync(schedule);

                // Map the created entity back to a DTO for the response
                var createdScheduleDto = new BusScheduleDTO
                {
                    Id = createdSchedule.Id,
                    BusForScheduleId = createdSchedule.BusForScheduleId,
                    RouteScheduledId = createdSchedule.RouteScheduledId,
                    DepartureTime = createdSchedule.DepartureTime,
                    ArrivalTime = createdSchedule.Arrival_Time
                };

                return CreatedAtAction(nameof(AddBusSchedule), new { id = createdScheduleDto.Id }, createdScheduleDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPut]
        public async Task<IActionResult> UpdateBusSchedule(BusScheduleDTO scheduleDto)
        {
            try
            {
                // Buscar el registro existente por ID
                var existingSchedule = await _busScheduleRepository.GetBusScheduleByIdAsyc(scheduleDto.Id);
                if (existingSchedule == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = "Record Not Found"
                    });
                }

                // Crear un nuevo objeto con los valores actualizados
                var updatedSchedule = new BusSchedule
                {
                    Id = scheduleDto.Id,
                    BusForScheduleId = scheduleDto.BusForScheduleId,
                    RouteScheduledId = scheduleDto.RouteScheduledId,
                    DepartureTime = scheduleDto.DepartureTime,
                    Arrival_Time = scheduleDto.ArrivalTime
                };

                // Validar si los nuevos horarios entran en conflicto
                bool isConflict = await _busScheduleRepository.IsScheduleConflictAsync(updatedSchedule);
                if (isConflict)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        message = "The schedule conflicts with an existing schedule for this bus."
                    });
                }

                // Actualizar el registro con los nuevos valores
                existingSchedule.BusForScheduleId = scheduleDto.BusForScheduleId;
                existingSchedule.RouteScheduledId = scheduleDto.RouteScheduledId;
                existingSchedule.DepartureTime = scheduleDto.DepartureTime;
                existingSchedule.Arrival_Time = scheduleDto.ArrivalTime;

                await _busScheduleRepository.UpdateBusScheduleAsync(existingSchedule);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusSchedule(int id)
        {
            try
            {
                var existingSchedule = await _busScheduleRepository.GetBusScheduleByIdAsyc(id);
                if (existingSchedule == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = "Record Not Found"
                    });
                }
                await _busScheduleRepository.DeleteBusScheduleAsync(existingSchedule);
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
        public async Task<IActionResult> GetBusSchedules(int id)
        {
            try
            {
                var existingSchedule = await _busScheduleRepository.GetBusScheduleByIdAsyc(id);
                if (existingSchedule == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = "Record Not Found"
                    });
                }
                return Ok(existingSchedule);
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
        public async Task<IActionResult> GetBusSchedules()
        {
            try
            {
                var buses = await _busScheduleRepository.GetAllBusSchedulesAsyc();
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
