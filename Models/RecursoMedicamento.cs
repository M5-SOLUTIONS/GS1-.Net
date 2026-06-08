using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace M5Storage.Models
{
    [Table("T_RECURSO_MEDICAMENTO")]
    public class RecursoMedicamento
    {
        [Key]
        [Column("ID")]
        [ForeignKey("Recurso")]
        public int Id { get; set; }

        [Column("VALIDADE")]
        [Display(Name = "Validade")]
        public DateTime? Validade { get; set; }

        public Recurso? Recurso { get; set; }
    }
}
