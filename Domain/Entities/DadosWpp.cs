namespace Domain.Entities
{
    public class DadosWpp
    {
        public string Nome { get; set; }
        public int QtMensagens { get; set; }
        public int QtCaracteres { get; set; }
        public int QtMensagensHorarioComercial { get; set; }
        public int QtMensagensFds { get; set; }
        public int QtMensagensDuranteSemana { get; set; }
        public int QtMensagensMadrugada { get; set; }
        public int QtUltimos30Dias { get; set; }
        public List<MensagemWpp> Mensagens { get; set; }
    }
}
