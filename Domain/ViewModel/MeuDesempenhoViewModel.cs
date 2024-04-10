namespace Domain.ViewModel
{
    public class MeuDesempenhoViewModel
    {
        public int QuantidadeQuestoesTentadas { get; set; }
        public int QuantidadeQuestoesResolvidasCorretas { get; set; }
        public int QuantidadeQuestoesIncorretas { get; set; }
        public IEnumerable<RespostasPorProvaViewModel> RespostasPorProvas { get; set; }
        public IEnumerable<RespostasPorProvaViewModel> RespostasPorMateria { get; set; }
        public IEnumerable<RespostasPorProvaViewModel> RespostasPorBanca { get; set; }
        public IEnumerable<RespostasPorProvaViewModel> RespostasPorTipo { get; set; }
        public IEnumerable<RespostasPorProvaViewModel> RespostasPorAvaliacao { get; set; }
    }
}
