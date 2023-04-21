using Data.Context;
using Main = Domain.Entities.Usuarios;
using IRepository = Application.Interface.Repositories.IUsuariosRepository;
using Microsoft.EntityFrameworkCore;

namespace Application.Implementation.Repositories
{
    public class UsuariosRepository : RepositoryBase<Main>, IRepository
    {
        private static readonly string includes = "";

        public UsuariosRepository(DataContext dataContext) : base(dataContext)
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
            return await GetAllAsync(includes: GetIncludes(includes).Length == 0 ? null : GetIncludes(includes));
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

            model.Login = entity.Login;
            model.Nome = entity.Nome;
            model.Pass = entity.Pass;

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

        public async Task<Main> GetByLogin(string user, string pass)
        {
            var query = GetQueryable().Where(p => p.Login.Equals(user) && p.Pass.Equals(pass));
            return await query.SingleOrDefaultAsync();
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

    }
}
