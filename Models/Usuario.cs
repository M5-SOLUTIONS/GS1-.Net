using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace M5Storage.Models
{
    [Table("T_USUARIOS")]
    public class Usuario
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [Column("NOME")]
        [MaxLength(100)]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório")]
        [Column("EMAIL")]
        [MaxLength(150)]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é obrigatória")]
        [Column("SENHA")]
        [MaxLength(100)]
        [Display(Name = "Senha")]
        public string Senha { get; set; } = string.Empty;

        public ICollection<Movimentacao> Movimentacoes { get; set; } = new List<Movimentacao>();
    }
}
