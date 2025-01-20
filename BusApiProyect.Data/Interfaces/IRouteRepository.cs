using BusApiProyect.Data.Models;

namespace BusApiProyect.Data.Interfaces
{
    public interface IRouteRepository
    {
        Task<Route> CreateRouteAsync(Route route);
        Task DeleteRouteAsync(Route route);
        Task<IEnumerable<Route>> GetAllRoutesAsyc();
        Task<Route> GetRouteByIdAsyc(int id);
        Task UpdateRouteAsync(Route route);
    }
}