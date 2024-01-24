using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Pesos
    {
        [Key]
        public int Id { get; set; }
        public string Faculdade { get; set; }
        public string Curso { get; set; }
        public string Turno { get; set; }
        public string Materia { get; set; }
        public decimal Peso { get; set; }
        public int CodigoCurso { get; set; }

    }
}
