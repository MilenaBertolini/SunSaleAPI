using Main = Domain.Entities.Logger;
using IService = Application.Interface.Services.ILoggerService;
using IRepository = Application.Interface.Repositories.ILoggerRepository;
using IRepositoryCodes = Application.Interface.Repositories.ICodigosTableRepository;
using static Data.Helper.EnumeratorsTypes;

namespace Application.Implementation.Services
{
    public class LoggerService : IService
    {
        private readonly IRepository _repository;
        private readonly IRepositoryCodes _repositoryCodes;
        public LoggerService(IRepository repository, IRepositoryCodes repositoryCodes)
        {
            _repository = repository;
            _repositoryCodes = repositoryCodes;
        }

        public async Task<Main> Add(Main entity)
        {
            entity.Id = await _repositoryCodes.GetNextCodigo(typeof(Main).Name);
            entity.Created = DateTime.Now;
            entity.Updated = DateTime.Now;

            if (entity.Id == -1) throw new Exception("Impossible to create a new Id");

            return await _repository.Add(entity);
        }

        public async Task<Main> AddException(Exception exception)
        {
            Main entity = new Main();

            entity.Id = await _repositoryCodes.GetNextCodigo(typeof(Main).Name);
            entity.Created = DateTime.Now;
            entity.Updated = DateTime.Now;
            entity.Descricao = $"Erro na classe {exception?.TargetSite?.DeclaringType?.Name}{exception?.TargetSite?.Name}. Erro: {exception?.Message}";
            entity.StackTrace = exception?.StackTrace;
            entity.Tipo = (int)TipoLog.ERROR;

            if (entity.Id == -1) throw new Exception("Impossible to create a new Id");

            return await _repository.Add(entity);
        }

        public async Task<Main> AddInfo(string info)
        {
            Main entity = new Main();

            entity.Id = await _repositoryCodes.GetNextCodigo(typeof(Main).Name);
            entity.Created = DateTime.Now;
            entity.Updated = DateTime.Now;
            entity.Descricao = info;
            entity.StackTrace = string.Empty;
            entity.Tipo = (int)TipoLog.INFO;

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

        public void Dispose()
        {
            this._repository.Dispose();
        }
    }
}
