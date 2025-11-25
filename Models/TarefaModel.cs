using ListaDeTarefas.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ListaDeTarefas.Models
{
    public class TarefaModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(100, ErrorMessage = "O título deve ter no máximo {1} caracteres.")]
        [Display(Name = "Título")]
        public required string Titulo { get; set; }

        [StringLength(1000, ErrorMessage = "A descrição deve ter no máximo {1} caracteres.")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Display(Name = "Criado em")]
        public DateTime DataDeCriacao { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data limite")]
        public DateTime DataLimite { get; set; }

        [Display(Name = "Prioridade")]
        public PrioridadeEnum Prioridade { get; set; }

        [Display(Name = "Situação")]
        public SituacaoEnum Situacao { get; set; }

        public int CategoriaId { get; set; }


        [Display(Name = "Categoria")]
        public required CategoriaModel Categoria { get; set; }
    }
}
