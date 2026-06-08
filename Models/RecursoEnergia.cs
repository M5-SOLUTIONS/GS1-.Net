using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace M5Storage.Models
{
    [Table("T_RECURSO_ENERGIA")]
    public class RecursoEnergia
    {
        [Key]
        [Column("ID")]
        [ForeignKey("Recurso")]
        public int Id { get; set; }

        [Column("TIPO_ENERGIA")]
        [MaxLength(50)]
        [Display(Name = "Tipo de Energia")]
        public string? TipoEnergia { get; set; }

        public Recurso? Recurso { get; set; }
    }
}
