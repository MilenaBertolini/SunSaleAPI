using Main = Domain.Entities.Pesos;
using IService = Application.Interface.Services.IPesosService;
using IRepository = Application.Interface.Repositories.IPesosRepository;
using IRepositoryCodes = Application.Interface.Repositories.ICodigosTableRepository;
using Domain.Entities;

namespace Application.Implementation.Services
{
    public class PesosService : IService
    {
        private readonly IRepository _repository;
        private readonly IRepositoryCodes _repositoryCodes;
        public PesosService(IRepository repository, IRepositoryCodes repositoryCodes)
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

        public async Task<IEnumerable<Main>> GetByCurso(int curso)
        {
            return await _repository.GetByCurso(curso);
        }

        public async Task<Main> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public Task<Main> Update(Main entity)
        {
            return _repository.Update(entity);
        }

        public List<OpcaoCodigoNome> GetPesos()
        {
            return _repository.GetPesos();
        }

        public void Dispose()
        {
            this._repository.Dispose();
        }
    }
}
