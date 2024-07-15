using System.Diagnostics.CodeAnalysis;

namespace Domain.ViewModel
{
    public class PostagemViewModel
    {
        public int? Id { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
        public string? IsActive { get; set; }

        public string Titulo { get; set; }
        public string TituloEn { get; set; }

        public string Intro { get; set; }
        public string IntroEn { get; set; }

        public string Descricao { get; set; }
        public string DescricaoEn { get; set; }
        public int TipoPostagem { get; set; }
        public string Capa { get; set; }
        public string? Link { get; set; }
        public int Curtidas { get; set; }
    }
}
