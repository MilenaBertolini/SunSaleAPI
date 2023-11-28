using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Alimentos
    {
        [Key]
        public int Codigo { get; set; }
        public int NumeroAlimento { get; set; }
        public int CategoriaCodigo { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string Descricao { get; set; }
        public decimal? Umidade { get; set; }
        public decimal? EnergiaKcal { get; set; }
        public decimal? EnergiaKg { get; set; }
        public decimal? Proteina { get; set; }
        public string Lipidios { get; set; }
        public string Colesterol { get; set; }
        public decimal? Carboidrato { get; set; }
        public decimal? FibraAlimentar { get; set; }
        public decimal? Cinzas { get; set; }
        public decimal? Calcio { get; set; }
        public string Magnesio { get; set; }
        public string Manganes { get; set; }
        public string Fosforo { get; set; }
        public string Ferro { get; set; }
        public string Sodio { get; set; }
        public string Potassio { get; set; }
        public string Cobre { get; set; }
        public string Zinco { get; set; }
        public string Retinol { get; set; }
        public string RE { get; set; }
        public string RAE { get; set; }
        public string Tiamina { get; set; }
        public string Piridozina { get; set; }
        public string Riboflavina { get; set; }
        public string Niacina { get; set; }
        public string VitaminaC { get; set; }

        [ForeignKey("CategoriaCodigo")]
        public CategoriaAlimentos CategoriaAlimentos { get; set; }
    }
}
