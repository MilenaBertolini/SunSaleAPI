using Main = Domain.Entities.Avaliacao;
using IService = Application.Interface.Services.IAvaliacaoService;
using IQuestoesAvaliacaoService = Application.Interface.Services.IQuestoesAvaliacaoService;
using IRepository = Application.Interface.Repositories.IAvaliacaoRepository;
using IRepositoryCodes = Application.Interface.Repositories.ICodigosTableRepository;
using Domain.Entities;

namespace Application.Implementation.Services
{
    public class AvaliacaoService : IService
    {
        private readonly IRepository _repository;
        private readonly IQuestoesAvaliacaoService _questoesAvaliacaoService;
        private readonly IRepositoryCodes _repositoryCodes;
        public AvaliacaoService(IRepository repository, IRepositoryCodes repositoryCodes, IQuestoesAvaliacaoService questoesAvaliacaoService)
        {
            _repository = repository;
            _repositoryCodes = repositoryCodes;
            _questoesAvaliacaoService = questoesAvaliacaoService;
        }

        public async Task<Main> Add(Main entity, int user)
        {
            entity.Id = await _repositoryCodes.GetNextCodigo(typeof(Main).Name);
            entity.CreatedBy = user;
            entity.UpdatedBy = user;
            entity.CreatedOn = DateTime.Now;
            entity.UpdatedOn = DateTime.Now;
            entity.IsActive = "1";

            if (entity.Id == -1) throw new Exception("Impossible to create a new Id");

            foreach (var item in entity.QuestoesAvaliacao)
            {
                item.IdAvaliacao = entity.Id;
                await _questoesAvaliacaoService.Add(item, user);
            }

            if(entity.IsPublic != "1")
            {
                entity.Key = Guid.NewGuid().ToString();
            }

            var retorno = await _repository.Add(entity);

            return retorno;
        }

        public async Task<bool> DeleteById(int id, int user)
        {
            var entity = await GetById(id);
            entity.UpdatedBy = user;
            entity.UpdatedOn = DateTime.Now;
            entity.IsActive = "1";

            return await _repository.Update(entity) != null;
        }

        public async Task<IEnumerable<Main>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<IEnumerable<Main>> GetAllPagged(int page, int quantity, string chave, int user, string subject, string bancas, string provas, string materias, string professores)
        {
            var list = await _repository.GetAllPagged(page, quantity, chave, user, subject, bancas, provas, materias, professores);

            return list;
        }

        public async Task<Main> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task<Main> Update(Main entity, int user)
        {
            entity.UpdatedBy = user;
            entity.UpdatedOn = DateTime.Now;
            entity.IsActive = "1";

            foreach (var item in await _questoesAvaliacaoService.GetAllByAvaliacao(entity.Id))
            {
                await _questoesAvaliacaoService.DeleteById(item.Id);
            }
            
            foreach(var item in entity.QuestoesAvaliacao)
            {
                item.IdAvaliacao = entity.Id;
                await _questoesAvaliacaoService.Add(item, user);
            }

            return await _repository.Update(entity);
        }

        public async Task<IEnumerable<Main>> GetByUserId(int id)
        {
            return await _repository.GetByUserId(id);
        }

        public async Task<int> QuantidadeTotal()
        {
            return await _repository.QuantidadeTotal();
        }

        public async Task<IEnumerable<string>> GetAllProfessores()
        {
            return await _repository.GetAllProfessores();
        }

        public void Dispose()
        {
            this._repository.Dispose();
        }
    }
}
