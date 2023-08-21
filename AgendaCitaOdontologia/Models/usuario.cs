using System.ComponentModel.DataAnnotations;

namespace AgendaCitaOdontologia.Models
{
    public class usuario
    {
        public int Id { get; set; }
        [Display(Name = "Correo Electronico")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [EmailAddress(ErrorMessage = "El campo {0} debe ser un correo valido")]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
