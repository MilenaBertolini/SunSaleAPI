using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities
{
    public class RespostasQuestoes
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

        public string ObservacaoResposta { get; set; }
    }
}
