using Main = Domain.Entities.SavedResultsWpp;
using IService = Application.Interface.Services.ISavedResultsWppService;
using IRepository = Application.Interface.Repositories.ISavedResultsWppRepository;

namespace Application.Implementation.Services
{
    public class SavedResultsWppService : IService
    {
        private readonly IRepository _repository;
        public SavedResultsWppService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<Main> Add(Main entity)
        {
            entity.Created = DateTime.Now;
            entity.Updated = DateTime.Now;
            entity.IsActive = "1";
            entity.Token = Guid.NewGuid().ToString();

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

        public async Task<Main> GetByToken(string token)
        {
            return await _repository.GetByToken(token);
        }

        public Task<Main> Update(Main entity)
        {
            return _repository.Update(entity);
        }

        public void Dispose()
        {
            this._repository.Dispose();
        }
    }
}
