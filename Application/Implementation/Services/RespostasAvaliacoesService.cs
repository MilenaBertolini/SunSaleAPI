using Main = Domain.Entities.RespostasAvaliacoes;
using IService = Application.Interface.Services.IRespostasAvaliacoesService;
using IAvaliacaoService = Application.Interface.Services.IAvaliacaoService;
using IRepository = Application.Interface.Repositories.IRespostasAvaliacoesRepository;
using IRepositoryCodes = Application.Interface.Repositories.ICodigosTableRepository;
using Domain.Entities;

namespace Application.Implementation.Services
{
    public class RespostasAvaliacoesService : IService
    {
        private readonly IRepository _repository;
        private readonly IRepositoryCodes _repositoryCodes;
        private readonly IAvaliacaoService _avaliacaoService;
        public RespostasAvaliacoesService(IRepository repository, IRepositoryCodes repositoryCodes, IAvaliacaoService avaliacaoService)
        {
            _repository = repository;
            _repositoryCodes = repositoryCodes;
            _avaliacaoService = avaliacaoService;
        }

        public async Task<Main> Add(Main entity)
        {
            entity.Id = await _repositoryCodes.GetNextCodigo(typeof(Main).Name);
            entity.CreatedOn = DateTime.Now;
            entity.UpdatedOn = DateTime.Now;

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

        public async Task<IEnumerable<Main>> GetByAvaliacao(int avaliacao, int user)
        {
            var retorno =  await _repository.GetByAvaliacao(avaliacao, user);

            var avaliacaoObj = await _avaliacaoService.GetById(avaliacao);

            while(avaliacaoObj.QuestoesAvaliacao.Count() < retorno.Count())
            {
                retorno = retorno.SkipLast(1);
            }

            return retorno;
        }

        public async Task<Main> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public Task<Main> Update(Main entity)
        {
            return _repository.Update(entity);
        }

        public async Task<IEnumerable<Usuarios>> GetUsuariosFizeramAvaliacao(int avaliacao)
        {
            return await _repository.GetUsuariosFizeramAvaliacao(avaliacao);
        }

        public void Dispose()
        {
            this._repository.Dispose();
        }
    }
}
