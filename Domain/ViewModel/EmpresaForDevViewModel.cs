using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel
{
    public class EmpresaForDevViewModel
    {
        public string Nome { get; set; }

        [Key]
        public string CNPJ { get; set; }

        public string IE { get; set; }

        public string DataAbertura { get; set; }

        public string Site { get; set; }

        public string Email { get; set; }

        public string CEP { get; set; }

        public string Endereco { get; set; }

        public string Numero { get; set; }

        public string Bairro { get; set; }

        public string Cidade { get; set; }

        public string Estado { get; set; }

        public string TelefoneFixo { get; set; }

        public string Celular { get; set; }
    }
}
