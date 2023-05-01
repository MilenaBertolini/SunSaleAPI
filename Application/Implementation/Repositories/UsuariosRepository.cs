using Data.Context;
using Main = Domain.Entities.Usuarios;
using IRepository = Application.Interface.Repositories.IUsuariosRepository;
using Microsoft.EntityFrameworkCore;
using Application.Model;

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
            entity.Created = DateTime.Now;
            entity.Updated = DateTime.Now;

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

            if(!string.IsNullOrEmpty(entity.Login))
                model.Login = entity.Login;

            if(!string.IsNullOrEmpty(entity.Pass))
                model.Pass = entity.Pass;

            if (!string.IsNullOrEmpty(entity.Email))
                model.Email = entity.Email;

            model.Nome = entity.Nome;
            model.Admin = entity.Admin;
            model.DataNascimento = entity.DataNascimento;
            model.Updated = DateTime.Now;

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

        public async Task<Main> VerifyLogin(string user, string pass)
        {
            var query = GetQueryable().Where(p => (p.Login.Equals(user) || p.Email.Equals(user)) && p.Pass.Equals(pass));
            return await query.SingleOrDefaultAsync();
        }

        public async Task<Main> GetByEmail(string email)
        {
            var query = GetQueryable().Where(p => p.Email.Equals(email));
            return await query?.SingleOrDefaultAsync();
        }

        public async Task<Main> GetByLogin(string login)
        {
            var query = GetQueryable().Where(p => p.Login.Equals(login));
            return await query?.SingleOrDefaultAsync();
        }

        public void Dispose()
        {
            this.Dispose(true);
        }
    }
}
