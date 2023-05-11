using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel
{
    public class RecuperaSenhaViewModel
    {
        [Key]
        public int Code { get; set; }
        public string Guid { get; set; }
        public string EmailUser { get; set; }
        public string Validated { get; set; }
    }
}
