namespace Domain.Entities
{
    public class EmailSettings
    {
        public string Remetente { get; set; }
        public string Smtp { get; set; }
        public int Porta { get; set; }
        public string EmailCredential { get; set; }
        public string Senha { get; set; }
    }
}
