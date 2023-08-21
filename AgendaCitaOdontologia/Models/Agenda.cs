using System.ComponentModel.DataAnnotations;

namespace AgendaCitaOdontologia.Models
{
    public class Agenda
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El {0} es obligatorio")]
        [StringLength(maximumLength: 50, MinimumLength = 3,
            ErrorMessage = "La cantidad de letras que debe ser entre 3 y 50")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Range(minimum: 18, maximum: 120, ErrorMessage = "El valor de {0} debe estar en {1} y {2}")]
        public int Edad { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Range(10000000, 99999999, ErrorMessage = "El {0} debe contener 8 dígitos")]
        public int Telefono { get; set; }

        [Display(Name = "Correo Electronico")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [EmailAddress(ErrorMessage = "El campo {0} debe ser un correo valido")]
        public string Email { get; set; }

        [Display(Name = "Tipo de cita")]
        [Required(ErrorMessage = "Elija una opcion de {0}")]
        public string TipoCita { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Inserte la fecha de la cita.")]
        [DataType(DataType.Date, ErrorMessage = "Inserte la fecha de la cita.")]
        public DateTime FechaHota { get; set; }
        //[Required(ErrorMessage = "Ingrese una {0} sobre la cita")]
        public string Observacion { get; set; }
        
        public int UsuarioId { get; set; }
    }
}
