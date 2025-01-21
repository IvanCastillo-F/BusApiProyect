using BusApiProyect.Data.Interfaces;
using BusApiProyect.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BusApiProyect.Data.Repositories
{
    public class BusScheduleRepository : IBusScheduleRepository
    {
        private readonly DBContext _context;
        public BusScheduleRepository(DBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BusSchedule>> GetAllBusSchedulesAsyc()
        {
            var busSched = await _context.Schedules.ToArrayAsync();
            return busSched;
        }

        public async Task<BusSchedule> GetBusScheduleByIdAsyc(int id)
        {
            return await _context.Schedules.FindAsync(id);
        }

        public async Task<BusSchedule> CreateBusScheduleAsync(BusSchedule busSched)
        {
            _context.Schedules.Add(busSched);
            await _context.SaveChangesAsync();
            return busSched;
        }

        public async Task UpdateBusScheduleAsync(BusSchedule busSched)
        {
            _context.Schedules.Update(busSched);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBusScheduleAsync(BusSchedule busSched)
        {
            _context.Schedules.Remove(busSched);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsScheduleConflictAsync(BusSchedule schedule)
        {
            // Check if there is a time overlap with existing schedules for the same bus
            return await _context.Schedules.AnyAsync(existing =>
                    // Overlap conditions
                    (schedule.DepartureTime >= existing.DepartureTime && schedule.DepartureTime < existing.Arrival_Time) || // Starts during another schedule
                    (schedule.Arrival_Time > existing.DepartureTime && schedule.Arrival_Time <= existing.Arrival_Time) ||  // Ends during another schedule
                    (schedule.DepartureTime <= existing.DepartureTime && schedule.Arrival_Time >= existing.Arrival_Time)   // Fully overlaps another schedule
                );
        }
    }
}
