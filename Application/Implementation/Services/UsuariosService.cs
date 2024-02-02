using Main = Domain.Entities.Usuarios;
using IService = Application.Interface.Services.IUsuariosService;
using IRepository = Application.Interface.Repositories.IUsuariosRepository;
using IRepositoryCodes = Application.Interface.Repositories.ICodigosTableRepository;
using Domain.Responses;

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
            var userExists = await GetByEmail(entity.Email);

            if(userExists != null)
            {
                userExists.Updated = DateTime.Now;
                userExists.IsVerified = "0";
                return await _repository.Update(userExists);
            }
            else
            {
                entity.Id = await _repositoryCodes.GetNextCodigo(typeof(Main).Name);
                entity.IsVerified = "0";

                if (entity.Id == -1) throw new Exception("Impossible to create a new Id");

                return await _repository.Add(entity);
            }
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

        public async Task<bool> ExistsEmail(string email, bool isVerified = false)
        {
            var temp = await _repository.GetByEmail(email, isVerified);

            return temp != null;
        }

        public async Task<bool> ExistsLogin(string login, bool isVerified = false)
        {
            var temp = await _repository.GetByLogin(login, isVerified);

            return temp != null;
        }

        public async Task<PerfilUsuario> GetPerfil(int user)
        {
            return await _repository.GetPerfil(user);
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
