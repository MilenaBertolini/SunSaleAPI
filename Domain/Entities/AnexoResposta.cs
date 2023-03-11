using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AnexoResposta

    {
        [Key]
        public int Codigo { get; set; }

        [NotNull]
        public int CodigoQuestao { get; set; }

        [NotNull]
        public DateTime DataRegistro { get; set; }

        public byte[] Anexo { get; set; }
    }
}
