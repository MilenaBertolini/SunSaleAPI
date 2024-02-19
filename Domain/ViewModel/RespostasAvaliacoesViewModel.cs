namespace Domain.ViewModel
{
    public class RespostasAvaliacoesViewModel
    {
        public int? Id { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int IdAvaliacao { get; set; }
        public int IdQuestao { get; set; }
        public int IdResposta { get; set; }
        public QuestoesViewModel? Questao { get; set; }

        public AvaliacaoViewModel? Avaliacao { get; set; }

        public RespostasQuestoesViewModel? Resposta { get; set; }
    }
}
