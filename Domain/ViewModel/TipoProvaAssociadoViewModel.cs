namespace Domain.ViewModel
{
    public class TipoProvaAssociadoViewModel
    {
        public int Codigo { get; set; }
        public int CodigoTipo { get; set; }
        public int CodigoProva { get; set; }
        public TipoProvaViewModel? TipoProva { get; set; }
    }
}
