using BusApiProyect.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusApiProyect.Data.DTO
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public int UserForBookingId { get; set; }

        public int ScheduleForBookingId { get; set; }

        public int SeatsBooked { get; set; }
    }
}
