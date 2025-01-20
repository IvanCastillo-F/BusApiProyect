
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BusApiProyect.Data.Models
{
    public class UserContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionstring;
        public UserContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionstring = _configuration.GetConnectionString("default");
        }
        public DbSet<User> Users { get; set; }

        public DbSet<Bus> Buses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(_connectionstring);
        }
    }
}
