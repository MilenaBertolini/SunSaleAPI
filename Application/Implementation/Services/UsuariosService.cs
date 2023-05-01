using Main = Domain.Entities.Usuarios;
using IService = Application.Interface.Services.IUsuariosService;
using IRepository = Application.Interface.Repositories.IUsuariosRepository;
using IRepositoryCodes = Application.Interface.Repositories.ICodigosTableRepository;

namespace Application.Implementation.Services
{
    public class UsuariosService : IService
    {
        private readonly IRepository _repository;
        private readonly IRepositoryCodes _repositoryCodes;
        public UsuariosService(IRepository repository, IRepositoryCodes repositoryCodes)
        {
            _repository = repository;
            _repositoryCodes = repositoryCodes;
        }

        public async Task<Main> Add(Main entity)
        {
            entity.Id = await _repositoryCodes.GetNextCodigo(typeof(Main).Name);

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

        public async Task<Main> GetByLogin(string user, string pass)
        {
            return await _repository.VerifyLogin(user, pass);
        }

        public async Task<Main> GetByEmail(string email)
        {
            return await _repository.GetByEmail(email);
        }

        public async Task<bool> ExistsEmail(string email)
        {
            var temp = await _repository.GetByEmail(email);

            return temp != null;
        }

        public async Task<bool> ExistsLogin(string login)
        {
            var temp = await _repository.GetByLogin(login);

            return temp != null;
        }

        public void Dispose()
        {
            this._repository.Dispose();
        }
    }
}
