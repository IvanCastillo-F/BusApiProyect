using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BusApiProyect.Data.Models
{
    [Table("BusSchedule")]
    public class BusSchedule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BusForScheduleId { get; set; }
        public Bus BusForSchedule { get; set; }


        [Required]
        public int RouteScheduledId { get; set; }
        public Route RouteScheduled { get; set; }


        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public DateTime Arrival_Time { get; set; }

    }
}
