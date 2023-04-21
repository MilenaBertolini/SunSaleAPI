using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities
{
    public class RespostasQuestoes
    {
        public RespostasQuestoes() 
        {
            AnexoResposta = new HashSet<AnexoResposta>();
        }

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

        [ForeignKey("CodigoQuestao")]
        public virtual ICollection<AnexoResposta> AnexoResposta { get; set; }

    }
}
