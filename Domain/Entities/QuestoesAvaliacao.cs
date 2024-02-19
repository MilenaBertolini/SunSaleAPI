using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class QuestoesAvaliacao
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int IdAvaliacao { get; set; }
        public int IdQuestao { get; set; }
        public decimal NotaQuestao { get; set; }

        [ForeignKey(nameof(IdQuestao))]
        public Questoes? Questao { get; set; }
    }
}
