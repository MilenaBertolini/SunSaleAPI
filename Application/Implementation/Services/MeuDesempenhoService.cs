using Main = Domain.Entities.MeuDesempenho;
using IService = Application.Interface.Services.IMeuDesempenhoService;
using IRepository = Application.Interface.Repositories.IMeuDesempenhoRepository;
using IRepositoryAdmin = Application.Interface.Repositories.IAdminRepository;
using Domain.Entities;

namespace Application.Implementation.Services
{
    public class MeuDesempenhoService : IService
    {
        private readonly IRepository _repository;
        private readonly IRepositoryAdmin _repositoryAdmin;
        public MeuDesempenhoService(IRepository repository, IRepositoryAdmin repositoryAdmin)
        {
            _repositoryAdmin = repositoryAdmin;
            _repository = repository;
        }


        public async Task<Main> GetAllDados(int user)
        {
            MeuDesempenho desempenho = new Main();

            desempenho.RespostasPorProvas = await _repositoryAdmin.BuscaRespostasPorProva(user);
            desempenho.RespostasPorMateria = await _repositoryAdmin.BuscaRespostasPorMateria(user);
            desempenho.RespostasPorBanca = await _repositoryAdmin.BuscaRespostasPorBanca(user);
            desempenho.RespostasPorTipo = await _repositoryAdmin.BuscaRespostasPorTipo(user);
            desempenho.RespostasPorAvaliacao = await _repositoryAdmin.BuscaRespostasPorAvaliacao(user);
            desempenho.QuantidadeQuestoesIncorretas = await _repository.BuscaQuestoesIncorretas(user);
            desempenho.QuantidadeQuestoesResolvidasCorretas = await _repository.BuscaQuestoesCorretas(user);
            desempenho.QuantidadeQuestoesTentadas = await _repository.BuscaQuestoesTentadas(user);

            return desempenho;
        }

        public void Dispose()
        {
            _repositoryAdmin.Dispose();
        }
    }
}
