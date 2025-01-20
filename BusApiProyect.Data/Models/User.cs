using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace BusApiProyect.Data.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public int _id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string _name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string _email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string _password { get; set; }

        [DefaultValue(false)]
        public Boolean _is_admin { get; set; }

    }
}
