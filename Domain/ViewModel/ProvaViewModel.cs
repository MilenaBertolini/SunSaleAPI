using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Domain.ViewModel
{
    public class ProvaViewModel
    {
        [Key]
        public int Codigo { get; set; }

        [NotNull]
        public string NomeProva { get; set; }

        public string Local { get; set; }

        public string TipoProva { get; set; }

        public string DataAplicacao { get; set; }

        [NotNull]
        public byte[] PROVA { get; set; }

        [NotNull]
        public byte[] GABARITO { get; set; }

        public string ObservacaoProva { get; set; }

        public string ObservacaoGabarito { get; set; }

        [NotNull]
        public DateTime DataRegistro { get; set; }

        [NotNull]
        public string Banca { get; set; }
    }
}
