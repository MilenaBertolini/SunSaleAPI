using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel
{
    public class CartaoCreditoDevToolsViewModel
    {
        [Key]
        public string NumeroCartao { get; set; }

        public string DataValidade { get; set; }

        public string CodigoSeguranca { get; set; }

        public string Created { get; set; }
    }
}
