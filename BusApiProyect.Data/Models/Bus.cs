using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BusApiProyect.Data.Models
{
    [Table("Bus")]
    public class Bus
    {
        [Key]
        public int Id { get; set; }

        public int BusNumber { get; set; }

        public int Capacity { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [EnumDataType(typeof(BusStatus), ErrorMessage = "Invalid status.")]
        public BusStatus CurrentStatus { get; set; }
    }

    public enum BusStatus
    {
        Available,
        OnTrip,
        Maintenance,
    }
}
