using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Domain.ViewModel
{
    public class UsuariosViewModel
    {
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        public string Login { get; set; }

        public string Pass { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public DateTime? DataNascimento { get; set; }

        public string? IsVerified { get; set; }

        public string? Admin { get; set; }
        public string? Instituicao { get; set; }

        public TipoPerfilViewModel? TipoPerfil { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Updated { get; set; }
    }
}
