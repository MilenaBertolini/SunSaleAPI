using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities
{
    public class Prova
    {
        [Key]
        public int Codigo { get; set; }

        [NotNull]
        public string NomeProva { get; set; }

        public string Local { get; set; }

        public string TipoProva { get; set; }

        public string DataAplicacao { get; set; }

        public string LinkProva { get; set; }

        public string LinkGabarito { get; set; }

        public string ObservacaoProva { get; set; }

        public string ObservacaoGabarito { get; set; }

        [NotNull]
        public DateTime DataRegistro { get; set; }

        [NotNull]
        public string Banca { get; set; }

        public DateTime UpdatedOn { get; set; }

        public int UpdatedBy { get; set; }

        public int CreatedBy { get; set; }

        [ForeignKey("CodigoProva")]
        public virtual ICollection<TipoProvaAssociado> TipoProvaAssociado { get; set; }
    }
}
