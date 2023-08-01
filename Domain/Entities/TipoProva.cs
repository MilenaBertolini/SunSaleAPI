using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities
{
    public class TipoProva
    {
        [Key]
        public int Codigo { get; set; }

        [NotNull]
        public string Descricao { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }

        [NotNull]
        public string IsActive { get; set; }

        [InverseProperty("TipoProva")]
        public virtual ICollection<TipoProvaAssociado> TipoProvaAssociados { get; set; }
    }
}
