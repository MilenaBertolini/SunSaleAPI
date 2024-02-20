using Data.Context;
using Main = Domain.Entities.TipoProva;
using IRepository = Application.Interface.Repositories.ITipoProvaRepository;
using Microsoft.EntityFrameworkCore;

namespace Application.Implementation.Repositories
{
    public class TipoProvaRepository : RepositoryBase<Main>, IRepository
    {
        private static readonly string includes = "";

        public TipoProvaRepository(DataContext dataContext) : base(dataContext)
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
            var query = (from t in _dataContext.TipoProva
                         join tp in _dataContext.TipoProvaAssociado on t.Codigo equals tp.CodigoTipo
                         join p in _dataContext.Prova on tp.CodigoProva equals p.Codigo
                         where p.IsActive.Equals("1")

                         select t).Distinct();

            return await query.ToListAsync();
        }

        public async Task<Main> GetById(int id)
        {
            var query = GetQueryable().Where(p => p.Codigo == id);

            GetIncludes(includes).ToList().ForEach(p => query = query.Include(p));

            return await query.SingleOrDefaultAsync();
        }

        public async Task<Main> Update(Main entity)
        {
            var model = await GetByIdAsync(entity.Codigo);
            if (model == null)
                return null;

            base.Merge(model, entity);

            base.Update(model);
            await base.CommitAsync();
            return model;
        }
        
        public async Task<IEnumerable<Main>> GetAllPagged(int page, int quantity)
        {
            var query = base.GetQueryable();
            GetIncludes(includes).ToList().ForEach(p => query = query.Include(p));

            return await base.GetAllPagedAsync(query, page, quantity);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

    }
}
