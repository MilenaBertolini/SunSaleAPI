using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities
{
    public class Codigos_Table
    {
        [Key]
        public string Tabela { get; set; }

        [NotNull]
        public int Codigo { get; set; }

    }
}
