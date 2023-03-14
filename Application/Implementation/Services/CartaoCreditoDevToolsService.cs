using Main = Domain.Entities.CartaoCreditoDevTools;
using IService = Application.Interface.Services.ICartaoCreditoDevToolsService;
using IRepository = Application.Interface.Repositories.ICartaoCreditoDevToolsRepository;
using IRepositoryCodes = Application.Interface.Repositories.ICodigosTableRepository;

namespace Application.Implementation.Services
{
    public class CartaoCreditoDevToolsService : IService
    {
        private readonly IRepository _repository;
        private readonly IRepositoryCodes _repositoryCodes;
        public CartaoCreditoDevToolsService(IRepository repository, IRepositoryCodes repositoryCodes)
        {
            _repository = repository;
            _repositoryCodes = repositoryCodes;
        }

        public async Task<Main> Add(Main entity)
        {
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

        public async Task<Main> GetByCartao(string cartao)
        {
            return await _repository.GetByCartao(cartao);
        }

        public Task<Main> Update(Main entity)
        {
            return _repository.Update(entity);
        }

        public async Task<IEnumerable<Main>> GetRandom(int? qt)
        {
            return await _repository.GetRandom(qt == null ? 1 : qt.Value);
        }

        public void Dispose()
        {
            this._repository.Dispose();
        }
    }
}
