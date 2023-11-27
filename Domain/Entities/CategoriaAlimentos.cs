using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class CategoriaAlimentos
    {
        [Key]
        public int Codigo { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string Descricao { get; set; }
    }
}
