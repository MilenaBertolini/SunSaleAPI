namespace Domain.ViewModel
{
    public class AvaliacaoViewModel
    {
        public int? Id { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string Nome { get; set; }
        public string? Orientacao { get; set; }
        public decimal? NotaTotal { get; set; }
        public string IsPublic { get; set; }
        public string? Key { get; set; }
        public virtual ICollection<QuestoesAvaliacaoViewModel> QuestoesAvaliacao { get; set; }
        public UsuariosResumedViewModel? Usuario { get; set; }

    }
}
