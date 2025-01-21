using BusApiProyect.Data.Models;

namespace BusApiProyect.Data.Interfaces
{
    public interface IBusScheduleRepository
    {
        Task<BusSchedule> CreateBusScheduleAsync(BusSchedule busSched);
        Task DeleteBusScheduleAsync(BusSchedule busSched);
        Task<IEnumerable<BusSchedule>> GetAllBusSchedulesAsyc();
        Task<BusSchedule> GetBusScheduleByIdAsyc(int id);
        Task UpdateBusScheduleAsync(BusSchedule busSched);
        Task<bool> IsScheduleConflictAsync(BusSchedule schedule);
    }
}