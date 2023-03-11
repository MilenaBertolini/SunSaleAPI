using Main = Domain.Entities.AcaoUsuario;
using IService = Application.Interface.Services.IAcaoUsuarioService;
using IRepository = Application.Interface.Repositories.IAcaoUsuarioRepository;

namespace Application.Implementation.Services
{
    public class AcaoUsuarioService : IService
    {
        private readonly IRepository _repository;
        public AcaoUsuarioService(IRepository repository)
        {
            _repository = repository;
        }

        public Task<Main> Add(Main entity)
        {
            return _repository.Add(entity);
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

        public void Dispose()
        {
            this._repository.Dispose();
        }
    }
}
