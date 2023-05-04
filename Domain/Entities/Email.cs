using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities
{
    public class Email
    {
        [Key]
        public int Codigo { get; set; }
        [NotNull]
        public string Destinatario { get; set; }
        [NotNull]
        public string Assunto { get; set; }
        [NotNull]
        public byte[] Texto { get; set; }
        public DateTime DataEnvio { get; set; }
        public string Status { get; set; }
        public string Observacao { get; set; }
    }
}
