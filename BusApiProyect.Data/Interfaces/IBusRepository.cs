using BusApiProyect.Data.Models;

namespace BusApiProyect.Data.Interfaces
{
    public interface IBusRepository
    {
        Task<Bus> CreateBusAsync(Bus bus);
        Task DeleteBusAsync(Bus bus);
        Task<IEnumerable<Bus>> GetAllBusesAsyc();
        Task<Bus> GetBusByIdAsyc(int id);
        Task UpdateBusAsync(Bus bus);
        Task<bool> IsBusNumberDuplicateAsync(int busNumber);
        Task<bool> IsBusNumberDuplicateForOtherBusAsync(int busNumber, int busId);
    }
}