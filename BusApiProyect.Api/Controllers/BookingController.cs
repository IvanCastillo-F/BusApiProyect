using BusApiProyect.Data.DTO;
using BusApiProyect.Data.Interfaces;
using BusApiProyect.Data.Models;
using BusApiProyect.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusApiProyect.Api.Controllers
{
    [Route("api/booking")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IBusScheduleRepository _busScheduleRepository;
        private readonly ILogger<BookingController> _logger;

        public BookingController(IBookingRepository bookingRepository, IBusScheduleRepository busScheduleRepository, ILogger<BookingController> logger)
        {
            _bookingRepository = bookingRepository;
            _busScheduleRepository = busScheduleRepository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddBooking(BookingDTO bookingDto)
        {
            try
            {
                // Fetch the schedule from the BusSchedule model using the ScheduleForBookingId
                var busSchedule = await _busScheduleRepository.GetBusScheduleByIdAsyc(bookingDto.ScheduleForBookingId);
                if (busSchedule == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = "Schedule not found."
                    });
                }

                // Extract the date and set it to midnight
                var scheduleDateAtMidnight = busSchedule.DepartureTime.Date;

                // Check for duplicate booking
                var duplicateBooking = await _bookingRepository.GetBookingByCriteriaAsync(
                    bookingDto.UserForBookingId,
                    bookingDto.ScheduleForBookingId,
                    scheduleDateAtMidnight);

                if (duplicateBooking != null)
                {
                    return Conflict(new
                    {
                        StatusCode = 409,
                        message = "Duplicate booking detected."
                    });
                }

                // Map the DTO to the entity
                var booking = new Booking
                {
                    Id = bookingDto.Id,
                    UserForBookingId = bookingDto.UserForBookingId,
                    ScheduleForBookingId = bookingDto.ScheduleForBookingId,
                    SeatsBooked = bookingDto.SeatsBooked,
                    BookingDate = scheduleDateAtMidnight
                };

                // Create the new booking
                var createdBooking = await _bookingRepository.CreateBookingAsync(booking);

                // Map the created entity back to a DTO for the response
                var createdBookingDto = new BookingDTO
                {
                    Id = createdBooking.Id,
                    UserForBookingId = createdBooking.UserForBookingId,
                    ScheduleForBookingId = createdBooking.ScheduleForBookingId,
                    SeatsBooked = createdBooking.SeatsBooked,
                };

                return CreatedAtAction(nameof(AddBooking), new { id = createdBookingDto.Id }, createdBookingDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [HttpPut]
        public async Task<IActionResult> UpdateBooking(BookingDTO bookingDto)
        {
            try
            {
                // Fetch the schedule from the BusSchedule model using the ScheduleForBookingId
                var busSchedule = await _busScheduleRepository.GetBusScheduleByIdAsyc(bookingDto.ScheduleForBookingId);
                if (busSchedule == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = "Schedule not found."
                    });
                }

                // Extract the date and set it to midnight
                var scheduleDateAtMidnight = busSchedule.DepartureTime.Date;

                // Check for duplicate booking (excluding the current booking ID)
                var duplicateBooking = await _bookingRepository.GetBookingByCriteriaAsync(
                    bookingDto.UserForBookingId,
                    bookingDto.ScheduleForBookingId,
                    scheduleDateAtMidnight,
                    excludeId: bookingDto.Id);

                if (duplicateBooking != null)
                {
                    return Conflict(new
                    {
                        StatusCode = 409,
                        message = "Duplicate booking detected."
                    });
                }

                // Fetch the existing booking
                var existingBooking = await _bookingRepository.GetBookingByIdAsyc(bookingDto.Id);
                if (existingBooking == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = "Record Not Found"
                    });
                }

                // Update the existing booking
                existingBooking.UserForBookingId = bookingDto.UserForBookingId;
                existingBooking.ScheduleForBookingId = bookingDto.ScheduleForBookingId;
                existingBooking.SeatsBooked = bookingDto.SeatsBooked;
                existingBooking.BookingDate = scheduleDateAtMidnight;

                await _bookingRepository.UpdateBookingAsync(existingBooking);
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
        public async Task<IActionResult> DeleteBooking(int id)
        {
            try
            {
                var existingBooking = await _bookingRepository.GetBookingByIdAsyc(id);
                if (existingBooking == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = "Record Not Found"
                    });
                }
                await _bookingRepository.DeleteBookingAsync(existingBooking);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = 500,
                    message = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooking(int id)
        {
            try
            {
                var existingBooking = await _bookingRepository.GetBookingByIdAsyc(id);
                if (existingBooking == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = "Record Not Found"
                    });
                }
                return Ok(existingBooking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = 500,
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBooking()
        {
            try
            {
                var bookings = await _bookingRepository.GetAllBookingsAsyc();
                return Ok(bookings);
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
