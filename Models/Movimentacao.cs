using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace M5Storage.Models
{
    [Table("T_MOVIMENTACOES")]
    public class Movimentacao
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Required]
        [Column("USUARIO_ID")]
        [Display(Name = "Usuário")]
        public int UsuarioId { get; set; }

        [Required]
        [Column("RECURSO_ID")]
        [Display(Name = "Recurso")]
        public int RecursoId { get; set; }

        [Required(ErrorMessage = "Tipo de movimentação é obrigatório")]
        [Column("TIPO_MOVIMENTACAO")]
        [MaxLength(30)]
        [Display(Name = "Tipo")]
        public string TipoMovimentacao { get; set; } = "CONSUMO";

        [Required(ErrorMessage = "Quantidade é obrigatória")]
        [Column("QUANTIDADE")]
        [Display(Name = "Quantidade")]
        public decimal Quantidade { get; set; }

        [Column("DESCRICAO")]
        [MaxLength(255)]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Column("DATA_MOVIMENTACAO")]
        [Display(Name = "Data")]
        public DateTime? DataMovimentacao { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }

        [ForeignKey("RecursoId")]
        public Recurso? Recurso { get; set; }
    }
}
