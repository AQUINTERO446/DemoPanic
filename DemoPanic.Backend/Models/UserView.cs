namespace DemoPanic.Backend.Models
{
    using Domain;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [NotMapped]
    public class UserView : User
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(20, ErrorMessage = "El tamaño del campo {0} debe estar entre {1} y {2} caracteres", MinimumLength = 6)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Compare("Password", ErrorMessage = "No coincide con la contraseña")]
        [Display(Name = "Password confirm")]
        public string PasswordConfirm { get; set; }
    }
}