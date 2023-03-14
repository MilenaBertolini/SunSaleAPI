using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class PessoasForDevViewModel
    {
        public string Nome { get; set; }

        public int Idade { get; set; }

        [Key]
        public string CPF { get; set; }

        public string RG { get; set; }

        public string DataNascimento { get; set; }

        public string Sexo { get; set; }

        public string Signo { get; set; }

        public string Mae { get; set; }

        public string Pai { get; set; }

        public string Email { get; set; }

        public string Senha { get; set; }

        public string Cep { get; set; }

        public string Endereco { get; set; }

        public int Numero { get; set; }

        public string Bairro { get; set; }

        public string Cidade { get; set; }

        public string Estado { get; set; }

        public string TelefoneFixo { get; set; }

        public string Celular { get; set; }

        public string Altura { get; set; }

        public int Peso { get; set; }

        public string TipoSanguineo { get; set; }

        public string CorFavorita { get; set; }
    }
}
