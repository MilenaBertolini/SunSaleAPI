using Domain.ViewModel;

namespace Domain.Responses
{
    public class RankingWhosThatPokemon
    {
        public List<WhosThatPokemonResultViewModel> ListaClassic { get; set; }
        public List<WhosThatPokemonResultViewModel> ListaCustom { get; set; }
    }
}
