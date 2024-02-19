using Main = Domain.Entities.QuestoesAvaliacao;
using IService = Application.Interface.Services.IQuestoesAvaliacaoService;
using IRepository = Application.Interface.Repositories.IQuestoesAvaliacaoRepository;
using IRepositoryCodes = Application.Interface.Repositories.ICodigosTableRepository;

namespace Application.Implementation.Services
{
    public class QuestoesAvaliacaoService : IService
    {
        private readonly IRepository _repository;
        private readonly IRepositoryCodes _repositoryCodes;
        public QuestoesAvaliacaoService(IRepository repository, IRepositoryCodes repositoryCodes)
        {
            _repository = repository;
            _repositoryCodes = repositoryCodes;
        }

        public async Task<Main> Add(Main entity, int user)
        {
            entity.Id = await _repositoryCodes.GetNextCodigo(typeof(Main).Name);
            entity.CreatedOn = DateTime.Now;

            if (entity.Id == -1) throw new Exception("Impossible to create a new Id");

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

        public async Task<IEnumerable<Main>> GetAllPagged(int page, int quantity)
        {
            return await _repository.GetAllPagged(page, quantity);
        }

        public async Task<Main> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public Task<Main> Update(Main entity)
        {
            return _repository.Update(entity);
        }

        public async Task<IEnumerable<Main>> GetAllByAvaliacao(int avaliacao)
        {
            return await _repository.GetAllByAvaliacao(avaliacao);
        }

        public void Dispose()
        {
            this._repository.Dispose();
        }
    }
}
