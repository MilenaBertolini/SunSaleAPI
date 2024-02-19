using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class RespostasAvaliacoes
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public int IdAvaliacao { get; set; }
        public int IdQuestao { get; set; }
        public int IdResposta { get; set; }

        [ForeignKey("IdQuestao")]
        public Questoes Questao { get; set; }
        
        [ForeignKey("IdAvaliacao")]
        public Avaliacao Avaliacao { get; set; }

        [ForeignKey("IdResposta")]
        public RespostasQuestoes Resposta { get; set; }

    }
}
