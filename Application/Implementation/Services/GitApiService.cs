using Application.Interface.Repositories;
using Application.Interface.Services;
using Domain.Entities;

namespace Application.Implementation.Services
{
    public class GitApiService : IGitApiService
    {
        private readonly IGitApiRepository _gitApi;

        public GitApiService(IGitApiRepository gitApi)
        {
            _gitApi = gitApi;
        }

        public async Task<Tuple<List<Postagem>, int>> BuscaInformacoesPessoais(int page, int quantity)
        {
            return await _gitApi.BuscaInformacoesPessoais(page, quantity);
        }
    }
}
