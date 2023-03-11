using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities
{
    public class AcaoUsuario
    {
        [Key]
        public int Codigo { get; set; }

        [NotNull]
        public int CodigoUsuario { get; set; }

        [NotNull]
        public string Acao { get; set; }

        public DateTime DataRegistro { get; set; }
    }
}
