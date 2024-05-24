using Domain.Entities;

namespace Application.Interface.Services
{
    public interface IRandomRaxaService : IDisposable
    {
        public List<Team> GetTeams(List<Players> players, int numeroJogadores = 4);
    }
}
