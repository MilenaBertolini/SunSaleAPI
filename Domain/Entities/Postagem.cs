using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities
{
    public class Postagem
    {
        [Key]
        public int Id { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        [NotNull]
        public string IsActive { get; set; }
        [NotNull]
        public string Titulo { get; set; }
        [NotNull]
        public string TituloEn { get; set; }

        [NotNull]
        public string Intro { get; set; }
        [NotNull]
        public string IntroEn { get; set; }
        [NotNull]
        public string Descricao { get; set; }
        [NotNull]
        public string DescricaoEn { get; set; }
        [NotNull]
        public int TipoPostagem { get; set; }
        [NotNull]
        public string Capa { get; set; }
        public string? Link { get; set; }
    }
}
