using Main = Domain.Entities.Prova;
using IService = Application.Interface.Services.IProvaService;
using IRepository = Application.Interface.Repositories.IProvaRepository;
using IServiceAcao = Application.Interface.Services.IAcaoUsuarioService;
using IServiceTipoProvaAssociado = Application.Interface.Services.ITipoProvaAssociadoService;
using IRepositoryCodes = Application.Interface.Repositories.ICodigosTableRepository;
using Domain.Entities;

namespace Application.Implementation.Services
{
    public class ProvaService : IService
    {
        private readonly IRepository _repository;
        private readonly IServiceAcao _serviceAcao;
        private readonly IRepositoryCodes _repositoryCodes;
        private readonly IServiceTipoProvaAssociado _serviceTipoProvaAssociado;

        public ProvaService(IRepository repository, IRepositoryCodes repositoryCodes, IServiceAcao serviceAcao, IServiceTipoProvaAssociado serviceTipoProvaAssociado)
        {
            _repository = repository;
            _repositoryCodes = repositoryCodes;
            _serviceAcao = serviceAcao;
            _serviceTipoProvaAssociado = serviceTipoProvaAssociado;
        }

        public async Task<Main> Add(Main entity, int codigoUsuario)
        {
            entity.Codigo = await _repositoryCodes.GetNextCodigo(typeof(Main).Name);
            if (entity.Codigo == -1) throw new Exception("Impossible to create a new Id");

            entity.DataRegistro = DateTime.Now;
            entity.CreatedBy = codigoUsuario;
            entity.UpdatedBy = codigoUsuario;
            entity.UpdatedOn = DateTime.Now;

            var tipos = new List<TipoProvaAssociado>();
            foreach(var t in entity.TipoProvaAssociado.ToList())
            {
                t.CodigoProva = entity.Codigo;
                t.Codigo = await _repositoryCodes.GetNextCodigo(typeof(TipoProvaAssociado).Name);
                tipos.Add(t);
            };

            entity.TipoProvaAssociado.Clear();
            tipos.ForEach(t => entity.TipoProvaAssociado.Add(t));


            var result = await _repository.Add(entity);
            
            await _serviceAcao.Add(new AcaoUsuario()
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

        public async Task<Tuple<IEnumerable<Main>, int>> GetAllPagged(int page, int quantity, string tipo, string prova, bool admin)
        {
            return await _repository.GetAllPagged(page, quantity, tipo, prova, admin);
        }

        public async Task<IEnumerable<Main>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<Main> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task<Main> Update(Main entity, int user)
        {
            entity.UpdatedBy = user;
            entity.UpdatedOn = DateTime.Now;

            var retorno = await _repository.Update(entity);

            var tipos = await _serviceTipoProvaAssociado.GetAllByProva(entity.Codigo);

            foreach(var t in tipos)
            {
                await _serviceTipoProvaAssociado.DeleteById(t.Codigo);
            }

            foreach(var t in entity.TipoProvaAssociado)
            {
                await _serviceTipoProvaAssociado.Add(t);
            }

            return retorno;
        }

        public void Dispose()
        {
            this._repository.Dispose();
        }
    }
}
