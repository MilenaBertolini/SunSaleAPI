using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Domain.ViewModel
{
    public class AnexosQuestoesViewModel
    {
        [Key]
        [JsonPropertyName("Id")]
        public int Codigo { get; set; }
        [NotNull]
        public int CodigoQuestao { get; set; }

        public DateTime DataRegistro { get; set; }

        public string Anexo { get; set; }
    }
}
