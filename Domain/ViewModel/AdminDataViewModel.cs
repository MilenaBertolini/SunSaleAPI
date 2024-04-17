namespace Domain.ViewModel
{
    public class AdminDataViewModel
    {
        public int QuantidadeVerificados { get; set; }
        public int QuantidadeNaoVerificados { get; set; }
        public int QuantidadeUsuarios30Dias { get; set; }
        public int QuantidadeTotal { get; set; }
        public int QuantidadeRespostas { get; set; }
        public int QuantidadeRespostasCertas { get; set; }
        public int QuantidadeRespostasUltimas30Dias { get; set; }
        public int QuantidadeQuestoesAtivas { get; set; }
        public int QuantidadeQuestoesSolicitadasRevisao { get; set; }
        public int QuantidadeProvasAtivas { get; set; }
        public int QuantidadeProvasDesativasAtivas { get; set; }
        public int QuantidadeRespostasTabuadaDivertida { get; set; }
        public int QuantidadeRespostasTabuadaDivertidaUltimas30Dias { get; set; }
        public IEnumerable<AdminUsuariosDateViewModel> UsuariosDates { get; set; }
        public IEnumerable<RespostasPorProvaViewModel> RespostasPorProvas { get; set; }
        public IEnumerable<RespostasPorProvaViewModel> RespostasPorMateria { get; set; }
        public IEnumerable<RespostasPorProvaViewModel> RespostasPorBanca { get; set; }
        public IEnumerable<RespostasPorProvaViewModel> RespostasPorTipo { get; set; }
        public IEnumerable<RespostasPorProvaViewModel> RespostasPorAvaliacao { get; set; }
    }
}
