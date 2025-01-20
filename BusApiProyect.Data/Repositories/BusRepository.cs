using BusApiProyect.Data.Interfaces;
using BusApiProyect.Data.Models;
using Microsoft.EntityFrameworkCore;


namespace BusApiProyect.Data.Repositories
{
    public class BusRepository : IBusRepository
    {
        private readonly DBContext _context;
        public BusRepository(DBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Bus>> GetAllBusesAsyc()
        {
            var buses = await _context.Buses.ToArrayAsync();
            return buses;
        }

        public async Task<Bus> GetBusByIdAsyc(int id)
        {
            return await _context.Buses.FindAsync(id);
        }

        public async Task<Bus> CreateBusAsync(Bus bus)
        {
            _context.Buses.Add(bus);
            await _context.SaveChangesAsync();
            return bus;
        }

        public async Task UpdateBusAsync(Bus bus)
        {
            _context.Buses.Update(bus);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBusAsync(Bus bus)
        {
            _context.Buses.Remove(bus);
            await _context.SaveChangesAsync();
        }

    }
}
