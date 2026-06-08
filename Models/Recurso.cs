using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace M5Storage.Models
{
    [Table("T_RECURSOS")]
    public class Recurso
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [Column("NOME")]
        [MaxLength(100)]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Categoria é obrigatória")]
        [Column("CATEGORIA")]
        [MaxLength(50)]
        [Display(Name = "Categoria")]
        public string Categoria { get; set; } = string.Empty;

        [Required(ErrorMessage = "Quantidade é obrigatória")]
        [Column("QUANTIDADE")]
        [Display(Name = "Quantidade")]
        public decimal Quantidade { get; set; }

        [Required(ErrorMessage = "Mínimo é obrigatório")]
        [Column("MINIMO")]
        [Display(Name = "Mínimo Seguro")]
        public decimal Minimo { get; set; }

        [Column("CRITICO")]
        [Display(Name = "Crítico")]
        public int Critico { get; set; } = 0;

        [Column("STATUS")]
        [MaxLength(30)]
        [Display(Name = "Status")]
        public string? Status { get; set; }

        [Column("NIVEL")]
        [MaxLength(30)]
        [Display(Name = "Nível")]
        public string? Nivel { get; set; }

        [Column("ULTIMA_ATUALIZACAO")]
        [Display(Name = "Última Atualização")]
        public DateTime? UltimaAtualizacao { get; set; }

        public ICollection<Movimentacao> Movimentacoes { get; set; } = new List<Movimentacao>();
        public ICollection<Alerta> Alertas { get; set; } = new List<Alerta>();

        public RecursoEnergia? RecursoEnergia { get; set; }
        public RecursoMedicamento? RecursoMedicamento { get; set; }
    }
}
