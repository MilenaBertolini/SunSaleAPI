using Main = Domain.Entities.Prova;
using IService = Application.Interface.Services.IProvaService;
using IRepository = Application.Interface.Repositories.IProvaRepository;
using IServiceAcao = Application.Interface.Services.IAcaoUsuarioService;
using IRepositoryCodes = Application.Interface.Repositories.ICodigosTableRepository;
using AutoMapper.Internal.Mappers;

namespace Application.Implementation.Services
{
    public class ProvaService : IService
    {
        private readonly IRepository _repository;
        private readonly IServiceAcao _serviceAcao;
        private readonly IRepositoryCodes _repositoryCodes;

        public ProvaService(IRepository repository, IRepositoryCodes repositoryCodes, IServiceAcao serviceAcao)
        {
            _repository = repository;
            _repositoryCodes = repositoryCodes;
            _serviceAcao = serviceAcao;
        }

        public async Task<Main> Add(Main entity, int codigoUsuario)
        {
            entity.Codigo = await _repositoryCodes.GetNextCodigo(typeof(Main).Name);
            if (entity.Codigo == -1) throw new Exception("Impossible to create a new Id");

            entity.DataRegistro = DateTime.Now;
            entity.CreatedBy = codigoUsuario;
            entity.UpdatedBy = codigoUsuario;
            entity.UpdatedOn = DateTime.Now;

            var result = await _repository.Add(entity);

            await _serviceAcao.Add(new Domain.Entities.AcaoUsuario()
            {
                Acao = $"Inserir Prova {result.Codigo} - {result.NomeProva}",
                CodigoUsuario = codigoUsuario
            });

            return result;
        }

        public Task<bool> DeleteById(int id)
        {
            return _repository.Delete(id);
        }

        public async Task<IEnumerable<Main>> GetSimulados()
        {
            return await _repository.GetSimulados();
        }

        public async Task<Tuple<IEnumerable<Main>, int>> GetAllPagged(int page, int quantity, string prova, bool admin)
        {
            return await _repository.GetAllPagged(page, quantity, prova, admin);
        }

        public async Task<IEnumerable<Main>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<Main> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public Task<Main> Update(Main entity, int user)
        {
            entity.UpdatedBy = user;
            entity.UpdatedOn = DateTime.Now;

            return _repository.Update(entity);
        }

        public void Dispose()
        {
            this._repository.Dispose();
        }
    }
}
