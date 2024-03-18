using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Questoes
    {
        public Questoes() 
        { 
            RespostasQuestoes = new HashSet<RespostasQuestoes>();
            AnexosQuestoes = new HashSet<AnexosQuestoes>();
        }

        [Key]
        public int Codigo { get; set; }

        public DateTime DataRegistro { get; set; }

        public string CampoQuestao { get; set; }

        public string ObservacaoQuestao { get; set; }

        public string Materia { get; set; }

        public int CodigoProva { get; set; }

        [ForeignKey("CodigoProva")]
        public Prova Prova { get; set; }

        public int NumeroQuestao { get; set; }

        public string Ativo { get; set; }

        public DateTime UpdatedOn { get; set; }

        public int UpdatedBy { get; set; }

        public int CreatedBy { get; set; }

        public string? Assunto { get; set; }

        [ForeignKey("CodigoQuestao")]
        public virtual ICollection<RespostasQuestoes>? RespostasQuestoes { get; set; }

        [ForeignKey("CodigoQuestao")]
        public virtual ICollection<AnexosQuestoes>? AnexosQuestoes { get; set; }

    }
}
