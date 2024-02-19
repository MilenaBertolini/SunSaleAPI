using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities
{
    public class Avaliacao
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        [NotNull]
        public string Nome { get; set; }
        public string? Orientacao { get; set; }
        public string IsActive { get; set; }
        public string IsPublic { get; set; }
        public string? Key { get; set; }

        [ForeignKey("IdAvaliacao")]
        public virtual ICollection<QuestoesAvaliacao> QuestoesAvaliacao { get; set; }

        [ForeignKey("CreatedBy")]
        public Usuarios Usuario { get; set; }

    }
}
