using System.ComponentModel.DataAnnotations;

namespace ListaDeTarefas.Enums
{
    public enum SituacaoEnum
    {
        [Display(Name = "Pendente")]
        Pendente = 1,
        [Display(Name = "Em Andamento")]
        EmAndamento = 2,
        [Display(Name = "Concluida")]
        Concluida = 3,
        [Display(Name = "Cancelada")]
        Cancelada = 4,
        [Display(Name = "Impedida")]
        Impedida = 5,

    }
}
