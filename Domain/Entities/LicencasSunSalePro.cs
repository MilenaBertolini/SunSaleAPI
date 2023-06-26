using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class LicencasSunSalePro
    {
        [Key]
        public int Codigo { get; set; }
        public string Licenca { get; set; }
        public string IsActive { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
