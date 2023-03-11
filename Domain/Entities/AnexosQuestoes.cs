using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities
{
    public class AnexosQuestoes

    {
        [Key]
        public int Codigo { get; set; }
        [NotNull]
        public int CodigoQuestao { get; set; }

        public DateTime DataRegistro { get; set; }

        public byte[] Anexo { get; set; }
    }
}
