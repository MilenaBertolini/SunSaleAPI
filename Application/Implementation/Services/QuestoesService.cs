using Main = Domain.Entities.Questoes;
using IService = Application.Interface.Services.IQuestoesService;
using IRepository = Application.Interface.Repositories.IQuestoesRepository;
using IRepositoryRespostas = Application.Interface.Repositories.IRespostasQuestoesRepository;
using IRepositoryCodes = Application.Interface.Repositories.ICodigosTableRepository;
using Application.Model;
using Domain.Entities;

namespace Application.Implementation.Services
{
    public class QuestoesService : IService
    {
        private readonly IRepository _repository;
        private readonly IRepositoryRespostas _repositoryRespostas;
        private readonly IRepositoryCodes _repositoryCodes;

        public QuestoesService(IRepository repository, IRepositoryCodes repositoryCodes, IRepositoryRespostas repositoryRespostas)
        {
            _repository = repository;
            _repositoryCodes = repositoryCodes;
            _repositoryRespostas = repositoryRespostas;
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

        public async Task<IEnumerable<Main>> GetAllPagged(int page, int quantity, int? codigoProva, string? subject)
        {
            return await _repository.GetAllPagged(page, quantity, codigoProva, subject);
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

        public Task<IEnumerable<string>> GetMaterias(int? prova)
        {
            return _repository.GetMaterias(prova == null ? -1 : prova.Value);
        }

        public Task<IEnumerable<Test>> GetTests(int? id)
        {
            return _repository.GetTests(id == null ? -1 : id.Value);   
        }

        public async Task<IEnumerable<Main>> GetQuestoesByProva(int prova)
        {
            var response = await _repository.GetByProva(prova);
            return response;
        }

        public async Task<IEnumerable<Main>> GetQuestoesByMateria(string materia)
        {
            var response = await _repository.GetByMateria(materia);
            return response;
        }
    }
}
