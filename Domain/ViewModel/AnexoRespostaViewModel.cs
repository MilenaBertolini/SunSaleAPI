using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Domain.ViewModel
{
    public class AnexoRespostaViewModel
    {
        [Key]
        [JsonPropertyName("id")]
        public int Codigo { get; set; }

        [NotNull]
        public int CodigoQuestao { get; set; }

        [NotNull]
        public DateTime DataRegistro { get; set; }

        public byte[] Anexo { get; set; }
    }
}
