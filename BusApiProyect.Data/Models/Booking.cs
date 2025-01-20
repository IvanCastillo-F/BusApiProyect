using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusApiProyect.Data.Models
{

    [Table("Booking")]
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserForBookingId { get; set; }
        public User UserForBooking { get; set; }

        [Required]
        public int ScheduleForBookingId  { get; set;}
        public Bus_Schedule ScheduleForBooking { get; set; }

        [Required]
        public int SeatsBooked { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }
    }
}
