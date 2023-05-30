namespace Domain.ViewModel
{
    public class LoggerViewModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string StackTrace { get; set; }
        public int Tipo { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
