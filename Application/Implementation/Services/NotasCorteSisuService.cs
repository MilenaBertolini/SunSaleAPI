using Main = Domain.Entities.NotasCorteSisu;
using IService = Application.Interface.Services.INotasCorteSisuService;
using IRepository = Application.Interface.Repositories.INotasCorteSisuRepository;
using IRepositoryCodes = Application.Interface.Repositories.ICodigosTableRepository;
using Domain.Entities;

namespace Application.Implementation.Services
{
    public class NotasCorteSisuService : IService
    {
        private readonly IRepository _repository;
        private readonly IRepositoryCodes _repositoryCodes;
        public NotasCorteSisuService(IRepository repository, IRepositoryCodes repositoryCodes)
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

        public IEnumerable<OpcaoCodigoNome> GetInstituicoes()
        {
            return _repository.GetInstituicoes();
        }

        public IEnumerable<OpcaoCodigoNome> GetCursosFromInstituicao(int instituicao)
        {
            return _repository.GetCursosFromInstituicao(instituicao);
        }

        public async Task<List<Main>> GetByCursoAndInstituicao(long curso, int instituicao)
        {
            return await _repository.GetByCursoAndInstituicao(curso, instituicao);
        }

        public void Dispose()
        {
            this._repository.Dispose();
        }
    }
}
