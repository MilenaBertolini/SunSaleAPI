using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Responses
{
    public class PerfilUsuario
    {
        public int CodigoUsuario { get; set; }
        public string Admin { get; set; }
        public int QuantidadeQuestoesResolvidas { get; set; }
        public int QuantidadeQuestoesAcertadas { get; set; }

        public UsuariosViewModel Usuario { get; set; }
    }
}
