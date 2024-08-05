using Domain.Entities;

namespace Application.Interface.Services
{
    public interface IGitApiService
    {
        Task<Tuple<List<Postagem>, int>> BuscaInformacoesPessoais(int page, int quantity);
    }
}
