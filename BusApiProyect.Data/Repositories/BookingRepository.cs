using BusApiProyect.Data.Interfaces;
using BusApiProyect.Data.Models;
using Microsoft.EntityFrameworkCore;


namespace BusApiProyect.Data.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly DBContext _context;
        public BookingRepository(DBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsyc()
        {
            var bookings = await _context.Bookings.ToArrayAsync();
            return bookings;
        }

        public async Task<Booking> GetBookingByIdAsyc(int id)
        {
            return await _context.Bookings.FindAsync(id);
        }

        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task UpdateBookingAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBookingAsync(Booking booking)
        {
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<Booking> GetBookingByCriteriaAsync(int userId, int scheduleId, DateTime bookingDate, int? excludeId = null)
        {
            var query = _context.Bookings.Where(b =>
                b.UserForBookingId == userId &&
                b.ScheduleForBookingId == scheduleId &&
                b.BookingDate == bookingDate);

            if (excludeId.HasValue)
            {
                query = query.Where(b => b.Id != excludeId.Value);
            }

            return await query.FirstOrDefaultAsync();
        }

    }
}
