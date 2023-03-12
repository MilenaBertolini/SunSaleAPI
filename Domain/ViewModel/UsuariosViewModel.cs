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

        [NotNull]
        public string Login { get; set; }

        [NotNull]
        public string Pass { get; set; }

        [NotNull]
        public string Nome { get; set; }

        [NotNull]
        public string Email { get; set; }
    }
}
