namespace Domain.ViewModel
{
    public class WhosThatPokemonResultViewModel
    {
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
        public string Nome { get; set; }
        public int Tempo { get; set; }
        public int Acertos { get; set; }
        public int Erros { get; set; }
        public byte Kanto { get; set; }
        public byte Johto { get; set; }
        public byte Hoenn { get; set; }
        public byte Sinnoh { get; set; }
        public byte Unova { get; set; }
        public byte Kalos { get; set; }
        public byte Alola { get; set; }
        public byte Paldea { get; set; }
    }
}
