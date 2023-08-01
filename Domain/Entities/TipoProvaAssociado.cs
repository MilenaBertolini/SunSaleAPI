using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class TipoProvaAssociado
    {
        [Key]
        public int Codigo { get; set; }

        public int CodigoTipo { get; set; }

        public int CodigoProva { get; set; }

        [ForeignKey("CodigoTipo")]
        public virtual TipoProva TipoProva { get; set; }
    }
}
