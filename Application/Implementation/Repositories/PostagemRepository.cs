using Data.Context;
using Main = Domain.Entities.Postagem;
using IRepository = Application.Interface.Repositories.IPostagemRepository;
using Microsoft.EntityFrameworkCore;
using static Data.Helper.EnumeratorsTypes;

namespace Application.Implementation.Repositories
{
    public class PostagemRepository : RepositoryBase<Main>, IRepository
    {
        private static readonly string includes = "";

        public PostagemRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public async Task<Main> Add(Main entity)
        {
            base.Add(entity);
            await base.CommitAsync();
            return entity;
        }

        public async Task<bool> Delete(int id)
        {
            var model = await base.GetByIdAsync(id);
            if(model == null)
                return false;

            base.Remove(model);
            await base.CommitAsync();
            return true;
        }

        public async Task<IEnumerable<Main>> GetAll()
        {
            return await GetAllAsync(includes: GetIncludes(includes));
        }

        public async Task<Main> GetById(int id)
        {
            var query = GetQueryable().Where(p => p.Id == id);

            GetIncludes(includes).ToList().ForEach(p => query = query.Include(p));

            return await query.SingleOrDefaultAsync();
        }

        public async Task<Main> Update(Main entity)
        {
            var model = await GetByIdAsync(entity.Id);
            if (model == null)
                return null;

            base.Merge(model, entity);

            base.Update(model);
            await base.CommitAsync();
            return model;
        }
        
        public async Task<Tuple<IEnumerable<Main>, int>> GetAllPagged(int page, int quantity, TipoPostagem tipoPostagem)
        {
            var query = base.GetQueryable().Where(p => p.TipoPostagem == (int)tipoPostagem);
            GetIncludes(includes).ToList().ForEach(p => query = query.Include(p));

            var qt = await base.GetAllPagedTotalAsync(query);
            var response = await base.GetAllPagedAsync(query, page, quantity, orderBy: tipoPostagem == TipoPostagem.Articles ? "Curtidas:Desc" : "Id:Asc", random: tipoPostagem == TipoPostagem.Psico);

            return Tuple.Create(response, qt);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

    }
}
