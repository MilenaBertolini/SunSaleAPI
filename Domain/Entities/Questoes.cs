using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Questoes
    {
        [Key]
        public int Codigo { get; set; }

        public DateTime DataRegistro { get; set; }

        public string CampoQuestao { get; set; }

        public string ObservacaoQuestao { get; set; }

        public string Materia { get; set; }

        public int CodigoProva { get; set; }

        public string NumeroQuestao { get; set; }

        public string Ativo { get; set; }
    }
}
