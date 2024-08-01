using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Domain.ViewModel
{
    public class ResultadosTabuadaDivertidaViewModel
    {
        [Key]
        [JsonPropertyName("Id")]
        public int Codigo { get; set; }

        [NotNull]
        public string Nome { get; set; }

        public int Tempo { get; set; }

        public int NumeroAcertos { get; set; }

        public DateTime Created { get; set; }

        public string Tipo { get; set; }

        public int NumeroQuestoes { get; set; }
    }
}
