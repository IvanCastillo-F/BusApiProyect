using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BusApiProyect.Data.Models
{
    [Table("Route")]
    public class Route
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Name Origin is Required")]
        public string Origin { get; set; }

        public double Origin_Latitude { get; set; }
        public double Origin_Longitude { get; set; }

        [Required(ErrorMessage ="Name Destination is Required")]
        public string Destination { get; set; }

        public double Destination_Latitude { get; set; }
        public double Destination_Longitude { get; set; }

        public double Distance { get; set; }


    }
}
