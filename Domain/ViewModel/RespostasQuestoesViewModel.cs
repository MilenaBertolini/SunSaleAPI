using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.ViewModel
{
    public class RespostasQuestoesViewModel
    {
        [Key]
        public int Codigo { get; set; }

        [NotNull]
        public int CodigoQuestao { get; set; }

        [NotNull]
        public DateTime DataRegistro { get; set; }

        [NotNull]
        public string TextoResposta { get; set; }

        [NotNull]
        public string Certa { get; set; }
    }
}
