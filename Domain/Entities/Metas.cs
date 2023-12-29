using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities
{
    public class Metas
    {
        [Key]
        public int Id { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        [NotNull]
        public string IsActive { get; set; }
        [NotNull]
        public string Sent { get; set; }

        [NotNull]
        public string Meta { get; set; }
        [NotNull]
        public string Email { get; set; }
        public DateTime DataObjetivo { get; set; }

    }
}
