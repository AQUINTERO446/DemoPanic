namespace DemoPanic.Domain
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ClientType
    {
        [Key]
        public int ClientTypeId { get; set; }

        [Required(ErrorMessage = "El campo {0}es requerido.")]
        [MaxLength(50, ErrorMessage = "El campo {0} solo puede tener un maximo de {1} caracteres.")]
        [Index("ClientType_Name_Index", IsUnique = true)]
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
