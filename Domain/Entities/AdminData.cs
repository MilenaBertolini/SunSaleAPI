namespace Domain.Entities
{
    public class AdminData
    {
        public int QuantidadeVerificados { get; set; }
        public int QuantidadeNaoVerificados { get; set; }
        public int QuantidadeTotal { get; set; }
        public int QuantidadeRespostas { get; set; }
        public int QuantidadeRespostasCertas { get; set; }
        public int QuantidadeRespostasUltimas24Horas { get; set; }
        public int QuantidadeQuestoesAtivas { get; set; }
        public int QuantidadeQuestoesSolicitadasRevisao { get; set; }
        public int QuantidadeProvasAtivas { get; set; }
        public int QuantidadeProvasDesativasAtivas { get; set; }

        public IEnumerable<AdminUsuariosDate> UsuariosDates { get; set; }
    }
}
