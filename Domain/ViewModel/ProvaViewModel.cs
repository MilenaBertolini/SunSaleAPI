using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Domain.ViewModel
{
    public class ProvaViewModel
    {
        [Key]
        [JsonPropertyName("id")]
        public int Codigo { get; set; }

        [NotNull]
        public string NomeProva { get; set; }

        public string Local { get; set; }

        public string TipoProva { get; set; }

        public string DataAplicacao { get; set; }

        public string LinkProva { get; set; }

        public string LinkGabarito { get; set; }

        public string ObservacaoProva { get; set; }

        public string ObservacaoGabarito { get; set; }

        [NotNull]
        public DateTime DataRegistro { get; set; }

        [NotNull]
        public string Banca { get; set; }
    }
}
