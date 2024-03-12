using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities
{
    public class RespostasUsuarios
    {
        [Key]
        public int Codigo { get; set; }

        [NotNull]
        public int CodigoUsuario { get; set; }

        [NotNull]
        public int CodigoResposta { get; set; }
        [NotNull]
        public int CodigoQuestao { get; set; }

        public DateTime DataResposta { get; set; }

        [ForeignKey("CodigoUsuario")]
        public Usuarios Usuario { get; set; }

        [ForeignKey("CodigoResposta")]
        public RespostasQuestoes Resposta { get; set; }

        [ForeignKey("CodigoQuestao")]
        public Questoes Questao { get; set; }
    }
}
