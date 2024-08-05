using Domain.Entities;

namespace Application.Interface.Repositories
{
    public interface IGitApiRepository
    {
        Task<Tuple<List<Postagem>, int>> BuscaInformacoesPessoais(int page, int quantity, int id = 0);
    }
}
