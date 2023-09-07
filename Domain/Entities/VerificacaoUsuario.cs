using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class VerificacaoUsuario
    {
        [Key]
        public int Codigo { get; set; }

        public string GuidText { get; set; }

        public int CodigoUsuario { get; set; }

        public string IsActive { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }
    }
}
