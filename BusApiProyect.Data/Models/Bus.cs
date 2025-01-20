using System.ComponentModel.DataAnnotations;


namespace BusApiProyect.Data.Models
{
    public class Bus
    {
        [Key]
        public int _id { get; set; }

        public int _bus_number { get; set; }

        public int _capacity { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [EnumDataType(typeof(BusStatus), ErrorMessage = "Invalid status.")]
        public BusStatus _current_status { get; set; }
    }

    public enum BusStatus
    {
        Available,
        OnTrip,
        Maintenance,
    }
}
