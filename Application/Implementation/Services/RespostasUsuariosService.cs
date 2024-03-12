using Main = Domain.Entities.RespostasUsuarios;
using IService = Application.Interface.Services.IRespostasUsuariosService;
using IRepository = Application.Interface.Repositories.IRespostasUsuariosRepository;
using IRepositoryCodes = Application.Interface.Repositories.ICodigosTableRepository;
using Domain.Responses;

namespace Application.Implementation.Services
{
    public class RespostasUsuariosService : IService
    {
        private readonly IRepository _repository;
        private readonly IRepositoryCodes _repositoryCodes;
        public RespostasUsuariosService(IRepository repository, IRepositoryCodes repositoryCodes)
        {
            _repository = repository;
            _repositoryCodes = repositoryCodes;
        }

        public async Task<Main> Add(Main entity)
        {
            entity.Codigo = await _repositoryCodes.GetNextCodigo(typeof(Main).Name);
            entity.DataResposta = DateTime.Now;

            if (entity.Codigo == -1) throw new Exception("Impossible to create a new Id");

            return await _repository.Add(entity);
        }

        public Task<bool> DeleteById(int id)
        {
            return _repository.Delete(id);
        }

        public async Task<IEnumerable<Main>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<IEnumerable<Main>> GetAllPagged(int page, int quantity, int user)
        {
            return await _repository.GetAllPagged(page, quantity, user);
        }

        public async Task<Main> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public Task<Main> Update(Main entity)
        {
            return _repository.Update(entity);
        }

        public Task<IEnumerable<Main>> GetByUser(int user)
        {
            return _repository.GetByUser(user);
        }

        public Task<IEnumerable<Main>> GetByQuestao(int questao)
        {
            return _repository.GetByQuestao(questao);
        }

        public Task<IEnumerable<Main>> GetByUserQuestao(int user, int questao = -1)
        {
            return _repository.GetByUserQuestao(user, questao);
        }

        public async Task<Tuple<IEnumerable<HistoricoUsuario>, int, int>> GetHistory(int user, int page, int quantity)
        {
            return await _repository.GetHistory(user, page, quantity);
        }

        public async Task<int> GetQuantidadeQuestoesCertas(int user)
        {
            return await _repository.GetQuantidadeQuestoesCertas(user);
        }

        public async Task<int> GetQuantidadeQuestoesTentadas(int user)
        {
            return await _repository.GetQuantidadeQuestoesTentadas(user);
        }

        public async Task<IEnumerable<Ranking>> GetRanking()
        {
            return await _repository.GetRanking();
        }

        public async Task<int> QuantidadeTotal()
        {
            return await _repository.QuantidadeTotal();
        }
        public void Dispose()
        {
            this._repository.Dispose();
        }
    }
}
