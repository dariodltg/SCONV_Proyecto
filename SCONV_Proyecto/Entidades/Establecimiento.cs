using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCONV_Proyecto.Entidades
{
    public class Establecimiento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        public List<Plato> Platos { get; set; }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
