using Main = Domain.Entities.RecuperaSenha;
using IService = Application.Interface.Services.IRecuperaSenhaService;
using IRepository = Application.Interface.Repositories.IRecuperaSenhaRepository;
using IRepositoryCodes = Application.Interface.Repositories.ICodigosTableRepository;

namespace Application.Implementation.Services
{
    public class RecuperaSenhaService : IService
    {
        private readonly IRepository _repository;
        private readonly IRepositoryCodes _repositoryCodes;
        public RecuperaSenhaService(IRepository repository, IRepositoryCodes repositoryCodes)
        {
            _repository = repository;
            _repositoryCodes = repositoryCodes;
        }

        public async Task<Main> Add(Main entity)
        {
            entity.Code = await _repositoryCodes.GetNextCodigo(typeof(Main).Name);
            entity.Guid = Guid.NewGuid().ToString();
            entity.Created = DateTime.Now;
            entity.Updated = DateTime.Now;

            if (entity.Code == -1) throw new Exception("Impossible to create a new Id");

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
            entity.Updated = DateTime.Now;
            return _repository.Update(entity);
        }

        public async Task<Main> GetByGuid(string guid)
        {
            return await _repository.GetByGuid(guid);
        }

        public void Dispose()
        {
            this._repository.Dispose();
        }
    }
}
