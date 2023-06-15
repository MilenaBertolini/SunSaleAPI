using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModel
{
    public class UsuariosCrudFormsViewModel
    {
        [Key]
        public int? Codigo { get; set; }

        public string Login { get; set; }

        public string Senha { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Administrador { get; set; }

        public string Desenvolvedor { get; set; }
        public int? UsuarioPai { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Updated { get; set; }
    }
}
