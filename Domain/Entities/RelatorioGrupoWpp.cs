namespace Domain.Entities
{
    public class RelatorioGrupoWpp
    {
        public string Id { get; set; }
        public string TopMensagens { get; set; }
        public string TopCaracteres { get; set; }
        public string TopDuranteSemana { get; set; }
        public string TopFds { get; set; }
        public string TopHorarioComercial { get; set; }
        public string TopUltimos30Dias { get; set; }
        public string TopMadrugada { get; set; }
        public string TopTestemunhaGeova { get; set; }
        public string TopRacista { get; set; }
        public string TopGordofobico { get; set; }
        public string TopHomofobico { get; set; }
        public string MenosMensagens { get; set; }

        public List<string> RankingMensagens { get; set; }
        public List<string> RankingCaracteres { get; set; }
        public List<string> RankingDuranteSemana { get; set; }
        public List<string> RankingFds { get; set; }
        public List<string> RankingHorarioComercial { get; set; }
        public List<string> RankingUltimos30Dias { get; set; }
        public List<string> RankingMadrugada { get; set; }
        public List<string> RankingTestemunhaGeova { get; set; }
        public List<string> RankingRacista { get; set; }
        public List<string> RankingGordofobico { get; set; }
        public List<string> RankingHomofobico { get; set; }
        public List<DadosWpp> Dados { get; set; }
    }
}
