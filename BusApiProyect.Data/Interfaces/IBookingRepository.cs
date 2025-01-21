using BusApiProyect.Data.Models;

namespace BusApiProyect.Data.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking> CreateBookingAsync(Booking booking);
        Task DeleteBookingAsync(Booking booking);
        Task<IEnumerable<Booking>> GetAllBookingsAsyc();
        Task<Booking> GetBookingByIdAsyc(int id);
        Task UpdateBookingAsync(Booking booking);
        Task<Booking> GetBookingByCriteriaAsync(int userId, int scheduleId, DateTime bookingDate, int? excludeId = null);
    }
}