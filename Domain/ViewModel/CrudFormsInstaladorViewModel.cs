using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Domain.ViewModel
{
    public class CrudFormsInstaladorViewModel
    {
        [Key]
        public int Codigo { get; set; }
        [NotNull]
        public string Versao { get; set; }
        [NotNull]
        public DateTime Created { get; set; }
        [NotNull]
        public string Diretorio { get; set; }
        [NotNull]
        public string Ativo { get; set; }
    }
}
