namespace Application.Model
{
    public class ComentariosViewModel
    {
        public int Codigo { get; set; }
        public int CodigoUsuario { get; set; }
        public int CodigoQuestao { get; set; }

        public string Comentario { get; set; }
        public string NomeUsuario { get; set; }
        public DateTime Created { get; set; }
    }
}
