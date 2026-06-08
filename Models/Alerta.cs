using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace M5Storage.Models
{
    [Table("T_ALERTAS")]
    public class Alerta
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Required]
        [Column("RECURSO_ID")]
        [Display(Name = "Recurso")]
        public int RecursoId { get; set; }

        [Column("MENSAGEM")]
        [MaxLength(255)]
        [Display(Name = "Mensagem")]
        public string? Mensagem { get; set; }

        [Column("NIVEL")]
        [MaxLength(30)]
        [Display(Name = "Nível")]
        public string? Nivel { get; set; }

        [Column("RESOLVIDO")]
        [Display(Name = "Resolvido")]
        public int Resolvido { get; set; } = 0;

        [Column("DATA_ALERTA")]
        [Display(Name = "Data do Alerta")]
        public DateTime? DataAlerta { get; set; }

        [ForeignKey("RecursoId")]
        public Recurso? Recurso { get; set; }
    }
}
