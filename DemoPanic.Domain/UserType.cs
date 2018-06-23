using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoPanic.Domain
{
    public class UserType
    {
        [Key]
        public int UserTypeId { get; set; }

        [Required(ErrorMessage = "El campo {0}es requerido.")]
        [MaxLength(50, ErrorMessage = "El campo {0} solo puede tener un maximo de {1} caracteres.")]
        [Index("UserType_Name_Index", IsUnique = true)]
        [DataType(DataType.EmailAddress)]
        public string  Name { get; set; }
    }
}
