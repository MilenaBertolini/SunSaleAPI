using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Usuarios
    {
        [Key]
        public int Id { get; set; }

        [NotNull]
        public string Login { get; set; }

        [NotNull]
        public string Pass { get; set; }

        [NotNull]
        public string Nome { get; set; }

        [NotNull]
        public string Email { get; set; }
    }
}
