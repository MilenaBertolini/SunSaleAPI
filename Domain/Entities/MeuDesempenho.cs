namespace Domain.Entities
{
    public class MeuDesempenho
    {
        public int QuantidadeQuestoesTentadas { get; set; }
        public int QuantidadeQuestoesResolvidasCorretas { get; set; }
        public int QuantidadeQuestoesIncorretas { get; set; }
        public IEnumerable<RespostasPorProva> RespostasPorProvas { get; set; }
        public IEnumerable<RespostasPorProva> RespostasPorMateria { get; set; }
        public IEnumerable<RespostasPorProva> RespostasPorBanca { get; set; }
        public IEnumerable<RespostasPorProva> RespostasPorTipo { get; set; }
        public IEnumerable<RespostasPorProva> RespostasPorAvaliacao { get; set; }
    }
}
