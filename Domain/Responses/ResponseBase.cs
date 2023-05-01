using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Responses
{
    public class ResponseBase<T>
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public int? Quantity { get; set; }
        public int? Total { get; set; }
        public T Object { get; set; }

    }
}
