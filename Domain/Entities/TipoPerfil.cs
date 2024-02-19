using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class TipoPerfil
    {
        [Key]
        public string Id { get; set; }
        public string Descricao { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
