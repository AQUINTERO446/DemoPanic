namespace DemoPanic.Domain
{
    using System.ComponentModel.DataAnnotations;

    public class Parameters
    {
        [Key]
        public int ParameterId { get; set; }

        [Display(Name = "Descripcion")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(50, ErrorMessage = "El campo {0} solo puede tener un maximo de {1} caracteres.")]
        public string Descripcion { get; set; }

        [Display(Name = "Parametro")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(50, ErrorMessage = "El campo {0} solo puede tener un maximo de {1} caracteres.")]
        public string Parameter { get; set; }
    }
}
