using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class CartaoCreditoDevTools
    {
        [Key]
        public string NumeroCartao { get; set; }

        public string DataValidade { get; set; }

        public string CodigoSeguranca { get; set; }

        public string Created { get; set; }
    }
}
