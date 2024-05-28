using Application.Interface.Services;
using Domain.Entities;
using Domain.ViewModel;

namespace Application.Implementation.Services
{
    public class RandomRaxaService : IRandomRaxaService
    {
        public void Dispose()
        {
        }

        public List<Team> GetTeams(List<Players> players, int numeroJogadores = 4)
        {
            if (players == null || players.Count == 0)
            {
                return new List<Team>();
            }

            Random random = new Random();

            var groupedPlayers = players.OrderByDescending(g => g.Nota)
                                            .GroupBy(j => j.Nota)
                                            .ToList();

            var randomList = new List<List<Players>>();

            foreach (var group in groupedPlayers)
            {
                randomList.Add(group.OrderBy(j => random.Next()).ToList());
            }

            List<Players> jogadoresOrdenados = new List<Players>();
            foreach (var group in randomList)
            {
                jogadoresOrdenados.AddRange(group.Select(i => i).ToList());
            }

            int numJogadores = players.Count;

            int numTimes = numJogadores / numeroJogadores;

            List<Team> teams = new List<Team>();
            for (int i = 0; i < numTimes; i++)
            {
                teams.Add(new Team()
                {
                    Players = new List<Players>()
                });
            }

            for (int i = 0; i < jogadoresOrdenados.Count; i++)
            {
                teams[i % numTimes].Players.Add(jogadoresOrdenados[i]);
            }

            return teams;
        }
    }
}
