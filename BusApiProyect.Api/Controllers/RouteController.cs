using BusApiProyect.Data.Interfaces;
using BusApiProyect.Data.Models;
using BusApiProyect.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusApiProyect.Api.Controllers
{
    [Route("api/route")]
    [ApiController]
    public class RouteController : ControllerBase
    {

        private readonly IRouteRepository _routeRepository;
        private readonly ILogger<RouteController> _logger;

        public RouteController(IRouteRepository routeRepository, ILogger<RouteController> logger)
        {
            _routeRepository = routeRepository;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddRoute(Data.Models.Route route)
        {
            try
            {
                // Check if the route already exists
                var isDuplicate = await _routeRepository.IsRouteDuplicateAsync(route.Origin, route.Destination);
                if (isDuplicate)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        message = "A route with the same origin and destination already exists."
                    });
                }

                double distance = await _routeRepository.GetDistance(route);
                route.Distance = distance;

                // Create the route if no duplicate is found
                var createdRoute = await _routeRepository.CreateRouteAsync(route);
                return CreatedAtAction(nameof(AddRoute), createdRoute);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateRoute(Data.Models.Route routeToUpdate)
        {
            try
            {
                // Check if the route exists
                var existingRoute = await _routeRepository.GetRouteByIdAsyc(routeToUpdate.Id);
                if (existingRoute == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = "Record Not Found"
                    });
                }

                // Check if the new route details conflict with an existing route
                var isDuplicate = await _routeRepository.IsRouteDuplicateForOtherRouteAsync(
                    routeToUpdate.Origin, routeToUpdate.Destination, routeToUpdate.Id);
                if (isDuplicate)
                {
                    return BadRequest(new
                    {
                        StatusCode = 400,
                        message = "A route with the same origin and destination already exists."
                    });
                }

                // Update the route details
                existingRoute.Origin = routeToUpdate.Origin;
                existingRoute.Origin_Latitude = routeToUpdate.Origin_Latitude;
                existingRoute.Origin_Longitude = routeToUpdate.Origin_Longitude;
                existingRoute.Destination = routeToUpdate.Destination;
                existingRoute.Destination_Latitude = routeToUpdate.Destination_Latitude;
                existingRoute.Destination_Longitude = routeToUpdate.Destination_Longitude;
                existingRoute.Distance = routeToUpdate.Distance;
                await _routeRepository.UpdateRouteAsync(existingRoute);
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


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoute(int id)
        {
            try
            {
                var existingRoute = await _routeRepository.GetRouteByIdAsyc(id);
                if (existingRoute == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = "Record Not Found"
                    });
                }
                await _routeRepository.DeleteRouteAsync(existingRoute);
                return NoContent();
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoutes(int id)
        {
            try
            {
                var existingRoute = await _routeRepository.GetRouteByIdAsyc(id);
                if (existingRoute == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = "Record Not Found"
                    });
                }
                return Ok(existingRoute);
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

        [HttpGet]
        public async Task<IActionResult> GetRoutes()
        {
            try
            {
                var routes = await _routeRepository.GetAllRoutesAsyc();
                return Ok(routes);
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

        [HttpGet("ByOrigin/{originName}")]
        public async Task<IActionResult> GetRoutesByOrigin(string originName)
        {
            try
            {
                // Fetch routes by origin name
                var routes = await _routeRepository.GetRoutesByOriginNameAsync(originName);

                if (routes == null || !routes.Any())
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = "No routes found for the specified origin."
                    });
                }

                // Sort routes by distance (ascending)
                var sortedRoutes = routes.OrderBy(r => r.Distance).ToList();

                return Ok(sortedRoutes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = 500,
                    message = "An error occurred while retrieving the routes."
                });
            }
        }

    }
}
