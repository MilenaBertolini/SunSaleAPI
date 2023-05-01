using System.ComponentModel.DataAnnotations;
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

        public DateTime DataResposta { get; set; }
    }
}
