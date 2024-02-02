using Main = Domain.Entities.ResultadosTabuadaDivertida;
using IService = Application.Interface.Services.IResultadosTabuadaDivertidaService;
using IRepository = Application.Interface.Repositories.IResultadosTabuadaDivertidaRepository;
using IRepositoryCodes = Application.Interface.Repositories.ICodigosTableRepository;
using Domain.Responses;

namespace Application.Implementation.Services
{
    public class ResultadosTabuadaDivertidaService : IService
    {
        private readonly IRepository _repository;
        private readonly IRepositoryCodes _repositoryCodes;
        public ResultadosTabuadaDivertidaService(IRepository repository, IRepositoryCodes repositoryCodes)
        {
            _repository = repository;
            _repositoryCodes = repositoryCodes;
        }

        public async Task<Main> Add(Main entity)
        {
            entity.Codigo = await _repositoryCodes.GetNextCodigo(typeof(Main).Name);
            entity.Created = DateTime.Now;

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

        public async Task<List<RankingTabuadaDivertida>> GetRankingTabuada()
        {
            return await _repository.GetRankingTabuada();
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
