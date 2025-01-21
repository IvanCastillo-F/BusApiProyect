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

        public async Task<bool> IsRouteDuplicateAsync(string origin, string destination)
        {
            return await _context.Routes.AnyAsync(r => r.Origin == origin && r.Destination == destination);
        }

        public async Task<bool> IsRouteDuplicateForOtherRouteAsync(string origin, string destination, int routeId)
        {
            return await _context.Routes.AnyAsync(r => r.Origin == origin && r.Destination == destination && r.Id != routeId);
        }

        public async Task<double> GetDistance(Route route)
        {
            if (route.Origin_Latitude < -90 || route.Origin_Latitude > 90 ||
               route.Destination_Latitude < -90 || route.Destination_Latitude > 90)
            {
                throw new ArgumentException("Latitude must be between -90 and 90 degrees.");
            }

            if (route.Origin_Longitude < -180 || route.Origin_Longitude > 180 ||
                route.Destination_Longitude < -180 || route.Destination_Longitude > 180)
            {
                throw new ArgumentException("Longitude must be between -180 and 180 degrees.");
            }
            var d1 = route.Origin_Latitude * (Math.PI / 180.0);
            var num1 = route.Origin_Longitude * (Math.PI / 180.0);
            var d2 = route.Destination_Latitude * (Math.PI / 180.0);
            var num2 = route.Destination_Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }

        public async Task<IEnumerable<Route>> GetRoutesByOriginNameAsync(string originName)
        {
            return await _context.Routes
                .Where(r => r.Origin.Equals(originName, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }

    }
}
