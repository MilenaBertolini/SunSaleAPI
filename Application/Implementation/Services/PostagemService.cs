using Main = Domain.Entities.Postagem;
using IService = Application.Interface.Services.IPostagemService;
using IRepository = Application.Interface.Repositories.IPostagemRepository;
using IRepositoryCodes = Application.Interface.Repositories.ICodigosTableRepository;
using static Data.Helper.EnumeratorsTypes;

namespace Application.Implementation.Services
{
    public class PostagemService : IService
    {
        private readonly IRepository _repository;
        private readonly IRepositoryCodes _repositoryCodes;
        public PostagemService(IRepository repository, IRepositoryCodes repositoryCodes)
        {
            _repository = repository;
            _repositoryCodes = repositoryCodes;
        }

        public async Task<Main> Add(Main entity)
        {
            entity.Created = DateTime.Now;
            entity.Updated = DateTime.Now;
            entity.IsActive = "1";

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

        public async Task<Tuple<IEnumerable<Main>, int>> GetAllPagged(int page, int quantity, TipoPostagem tipoPostagem)
        {
            return await _repository.GetAllPagged(page, quantity, tipoPostagem);
        }

        public async Task<Main> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task<Main> Curtir(bool positivo, int postagemId)
        {
            var postagem = await _repository.GetById(postagemId);

            if (positivo)
            {
                postagem.Curtidas++;
            }
            else
            {
                postagem.Curtidas--;
            }

            return await Update(postagem);
        }

        public Task<Main> Update(Main entity)
        {
            entity.Updated = DateTime.Now;

            return _repository.Update(entity);
        }

        public void Dispose()
        {
            this._repository.Dispose();
        }
    }
}
