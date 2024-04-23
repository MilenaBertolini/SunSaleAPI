using Main = Domain.Entities.WhosThatPokemonResult;
using IService = Application.Interface.Services.IWhosThatPokemonResultService;
using IRepository = Application.Interface.Repositories.IWhosThatPokemonResultRepository;
using IRepositoryCodes = Application.Interface.Repositories.ICodigosTableRepository;

namespace Application.Implementation.Services
{
    public class WhosThatPokemonResultService : IService
    {
        private readonly IRepository _repository;
        private readonly IRepositoryCodes _repositoryCodes;
        public WhosThatPokemonResultService(IRepository repository, IRepositoryCodes repositoryCodes)
        {
            _repository = repository;
            _repositoryCodes = repositoryCodes;
        }

        public async Task<Main> Add(Main entity)
        {
            entity.Created = DateTime.Now;
            entity.Updated = DateTime.Now;

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
        public async Task<IEnumerable<Main>> GetRanking(bool custom)
        {
            return await _repository.GetRanking(custom);
        }

        public void Dispose()
        {
            this._repository.Dispose();
        }
    }
}
