namespace Domain.ViewModel
{
    public class NotasCorteSisuViewModel
    {
        public long Id { get; set; }
        public int Year { get; set; }
        public string NumeroEdicao { get; set; }
        public int CodigoInstituicaoEnsino { get; set; }
        public string NomeInstituicao { get; set; }
        public string SiglaInstituicao { get; set; }
        public string OrganizacaoAcademica { get; set; }
        public string CategoriaOrganizacao { get; set; }
        public string NomeCampus { get; set; }
        public string NomeMunicipioCampus { get; set; }
        public string UFCampus { get; set; }
        public string RegiaoCampus { get; set; }
        public long CodigoCurso { get; set; }
        public string NomeCurso { get; set; }
        public string DescricaoGrau { get; set; }
        public string Turno { get; set; }
        public string ModoConcorrencia { get; set; }
        public string DesricaoModoConcorrencia { get; set; }
        public decimal BonusAcaoAfirmativa { get; set; }
        public int QuantidadeVagas { get; set; }
        public decimal NotaCorte { get; set; }
        public int QuantidadeInscricoes { get; set; }
    }
}
