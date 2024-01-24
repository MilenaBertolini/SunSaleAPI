using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class PesosViewModel
    {
        public int Id { get; set; }
        public string Faculdade { get; set; }
        public string Curso { get; set; }
        public string Turno { get; set; }
        public string Materia { get; set; }
        public decimal Peso { get; set; }
        public int CodigoCurso { get; set; }
    }
}
