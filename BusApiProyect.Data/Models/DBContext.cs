
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BusApiProyect.Data.Models
{
    public class DBContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionstring;
        public DBContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionstring = _configuration.GetConnectionString("default");
        }
        public DbSet<User> Users { get; set; }

        public DbSet<Bus> Buses { get; set; }

        public DbSet<Route> Routes { get; set; }

        public DbSet<Bus_Schedule> Schedules { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(_connectionstring);
        }
    }
}
