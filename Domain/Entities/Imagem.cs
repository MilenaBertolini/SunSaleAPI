namespace Domain.Entities
{
    public class Imagem
    {
        public string Arquivo { get; set; }
        public string tipo { get; set; }
        public int? width { get; set; }
        public int? height { get; set; }
        public bool? turnTransparent { get; set; }
    }
}
