using System.ComponentModel.DataAnnotations;

namespace ListaDeTarefas.Enums
{
    public enum PrioridadeEnum
    {
        [Display(Name = "Baixa")]
        Baixa = 0,
        [Display(Name = "Média")]
        Media = 1,
        [Display(Name = "Alta")]
        Alta = 2
    }
}
