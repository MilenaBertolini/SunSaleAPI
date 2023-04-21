using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.ViewModel
{
    public class QuestoesViewModel
    {
        [Key]
        [JsonPropertyName("id")]
        public int Codigo { get; set; }

        public DateTime DataRegistro { get; set; }

        public string CampoQuestao { get; set; }

        public string ObservacaoQuestao { get; set; }

        public string Materia { get; set; }

        public int CodigoProva { get; set; }

        public string NumeroQuestao { get; set; }

        public string Ativo { get; set; }

        public virtual ICollection<RespostasQuestoesViewModel> RespostasQuestoes { get; set; }

        public virtual ICollection<AnexosQuestoesViewModel> AnexosQuestoes { get; set; }
    }
}
