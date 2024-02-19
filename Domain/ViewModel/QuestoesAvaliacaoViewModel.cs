namespace Domain.ViewModel
{
    public class QuestoesAvaliacaoViewModel
    {
        public int? Id { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? IdAvaliacao { get; set; }
        public int IdQuestao { get; set; }
        public decimal NotaQuestao { get; set; }
        public QuestoesViewModel? Questao { get; set; }
    }
}
