using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel
{
    public class VeiculosForDevViewModel
    {
        public string Marca { get; set; }

        public string Modelo { get; set; }

        public string Ano { get; set; }

        [Key]
        public string Renavam { get; set; }

        public string PlacaVeiculo { get; set; }

        public string Cor { get; set; }

    }
}