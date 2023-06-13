using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.ViewModel
{
    public class SimuladosViewModel
    {
        [Key]
        public int? Codigo { get; set; }
        public string Respostas { get; set; }
        public DateTime Created { get; set; }
        public int? CodigoUsuario { get; set; }
        [ForeignKey("Prova")]
        public int CodigoProva { get; set; }
        public int? QuantidadeQuestoes { get; set; }
        public int? QuantidadeCertas { get; set; }
        public int Tempo { get; set; }

        public ProvaViewModel? Prova { get; set; }
    }
}
