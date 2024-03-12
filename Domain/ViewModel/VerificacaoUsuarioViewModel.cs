namespace Domain.ViewModel
{
    public class VerificacaoUsuarioViewModel
    {
        public int Codigo { get; set; }

        public string? GuidText { get; set; }

        public int CodigoUsuario { get; set; }

        public string IsActive { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }
        public int QuantidadeTentativas { get; set; }
    }
}
