using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class ComentariosQuestoes
    {
        [Key]
        public int Codigo { get; set; }
        public string Comentario { get; set; }
        public int CodigoUsuario { get; set; }
        public int CodigoQuestao { get; set; }
        public string IsActive { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
