using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Domain.ViewModel
{
    public class AcaoUsuarioViewModel
    {
        [Key]
        [JsonPropertyName("id")]
        public int Codigo { get; set; }

        [NotNull]
        public int CodigoUsuario { get; set; }

        [NotNull]
        public string Acao { get; set; }

        public DateTime DataRegistro { get; set; }
    }
}
