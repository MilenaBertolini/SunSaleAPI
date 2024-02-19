using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities
{
    public class Usuarios
    {
        [Key]
        public int Id { get; set; }

        public string Login { get; set; }

        public string Pass { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public DateTime? DataNascimento { get; set; }

        public string Admin { get; set; }

        [ForeignKey(nameof(Admin))]
        public TipoPerfil? TipoPerfil { get; set; }

        public string IsVerified { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }
        public string? Instituicao { get; set; }
    }
}
