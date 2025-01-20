using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BusApiProyect.Data.Models
{
    [Table("Bus_Schedule")]
    public class Bus_Schedule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BusScheduleId { get; set; }
        public Bus BusSchedule { get; set; }


        [Required]
        public int RouteScheduledId { get; set; }
        public Route RouteScheduled { get; set; }


        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public DateTime Arrival_Time { get; set; }

    }
}
