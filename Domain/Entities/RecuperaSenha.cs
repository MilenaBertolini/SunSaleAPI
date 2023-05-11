using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class RecuperaSenha
    {
        [Key]
        public int Code { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string Guid { get; set; }
        public string EmailUser { get; set; }
        public string Validated { get; set; }
    }
}
