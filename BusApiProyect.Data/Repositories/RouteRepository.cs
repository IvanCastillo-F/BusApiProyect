using BusApiProyect.Data.Interfaces;
using BusApiProyect.Data.Models;
using Microsoft.EntityFrameworkCore;


namespace BusApiProyect.Data.Repositories
{
    public class RouteRepository : IRouteRepository
    {
        private readonly DBContext _context;
        public RouteRepository(DBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Route>> GetAllRoutesAsyc()
        {
            var routes = await _context.Routes.ToArrayAsync();
            return routes;
        }

        public async Task<Route> GetRouteByIdAsyc(int id)
        {
            return await _context.Routes.FindAsync(id);
        }

        public async Task<Route> CreateRouteAsync(Route route)
        {
            _context.Routes.Add(route);
            await _context.SaveChangesAsync();
            return route;
        }

        public async Task UpdateRouteAsync(Route route)
        {
            _context.Routes.Update(route);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRouteAsync(Route route)
        {
            _context.Routes.Remove(route);
            await _context.SaveChangesAsync();
        }
    }
}
