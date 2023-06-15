using Main = Domain.Entities.UsuariosCrudForms;
using IService = Application.Interface.Services.IUsuariosCrudFormsService;
using IRepository = Application.Interface.Repositories.IUsuariosCrudFormsRepository;
using IRepositoryCodes = Application.Interface.Repositories.ICodigosTableRepository;
using Domain.Responses;

namespace Application.Implementation.Services
{
    public class UsuariosCrudFormsService : IService
    {
        private readonly IRepository _repository;
        private readonly IRepositoryCodes _repositoryCodes;
        public UsuariosCrudFormsService(IRepository repository, IRepositoryCodes repositoryCodes)
        {
            _repository = repository;
            _repositoryCodes = repositoryCodes;
        }

        public async Task<Main> Add(Main entity)
        {
            entity.Codigo = await _repositoryCodes.GetNextCodigo(typeof(Main).Name);

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

        public async Task<IEnumerable<Main>> GetByUser(int id)
        {
            return await _repository.GetByUser(id);
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

        public async Task<PerfilUsuario> GetPerfil(int user)
        {
            return await _repository.GetPerfil(user);
        }


        public void Dispose()
        {
            this._repository.Dispose();
        }
    }
}
