using Data.Context;
using Main = Domain.Entities.Logger;
using IRepository = Application.Interface.Repositories.ILoggerRepository;
using Microsoft.EntityFrameworkCore;

namespace Application.Implementation.Repositories
{
    public class LoggerRepository : RepositoryBase<Main>, IRepository
    {
        private static readonly string includes = "";

        public LoggerRepository(DataContext dataContext) : base(dataContext)
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
        
        public async Task<Tuple<IEnumerable<Main>, int>> GetAllPagged(int page, int quantity, string message)
        {
            var query = base.GetQueryable();

            if(!string.IsNullOrEmpty(message))
            {
                query = query.Where(q => q.Descricao.Contains(message));
            }

            GetIncludes(includes).ToList().ForEach(p => query = query.Include(p));

            var response = await base.GetAllPagedAsync(query, page, quantity, orderBy: "Id:Desc");
            var qt = await query.CountAsync();

            return Tuple.Create(response, qt);
        }

        public async Task<int> QuantidadeTotal()
        {
            var response = await _dataContext.Database.SqlQueryRaw<int>("select count(1) as value from Logger").FirstOrDefaultAsync();

            return response;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

    }
}
