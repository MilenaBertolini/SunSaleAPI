using Data.Context;
using Main = Domain.Entities.Usuarios;
using IRepository = Application.Interface.Repositories.IUsuariosRepository;
using Microsoft.EntityFrameworkCore;
using Application.Model;
using Domain.Responses;

namespace Application.Implementation.Repositories
{
    public class UsuariosRepository : RepositoryBase<Main>, IRepository
    {
        private static readonly string includes = "TipoPerfil";

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
            entity.Created = model.Created;
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

            return await base.GetAllPagedAsync(query, page, quantity, orderBy: "Created:desc");
        }

        public async Task<Main> VerifyLogin(string user, string pass)
        {
            var query = GetQueryable().Where(p => (p.Login.Equals(user) || p.Email.Equals(user)) && p.Pass.Equals(pass));
            return await query.SingleOrDefaultAsync();
        }

        public async Task<Main> GetByEmail(string email, bool isVerified = false)
        {
            var query = GetQueryable().Where(p => p.Email.Equals(email) || p.Login.Equals(email));

            if (isVerified)
            {
                query = query.Where(p => p.IsVerified == "1");
            }

            return await query?.SingleOrDefaultAsync();
        }

        public async Task<Main> GetByLogin(string login, bool isVerified = false)
        {
            var query = GetQueryable().Where(p => p.Login.Equals(login));
            if (isVerified)
            {
                query = query.Where(p => p.IsVerified == "1");
            }

            return await query?.SingleOrDefaultAsync();
        }

        public async Task<PerfilUsuario> GetPerfil(int user)
        {
            var query = (from u in _dataContext.Usuarios
                         join t in _dataContext.TipoPerfil on u.Admin equals t.Id
                         where u.Id.Equals(user)

                         select new PerfilUsuario
                         {
                             Admin = u.Admin,
                             CodigoUsuario = u.Id,
                             Usuario = new Domain.ViewModel.UsuariosViewModel() 
                             {
                                 Admin = u.Admin,
                                 DataNascimento = u.DataNascimento,
                                 Email = u.Email,
                                 Id = u.Id,
                                 Login = u.Login,
                                 Nome = u.Nome,
                                 Pass = u.Pass,
                                 Instituicao = u.Instituicao,
                                 TipoPerfil = new Domain.ViewModel.TipoPerfilViewModel()
                                 {
                                     Id = t.Id,
                                     Descricao = t.Descricao
                                 }
                             }
                         });

            var response = await query.FirstOrDefaultAsync();
            return response;
        }

        public async Task<int> QuantidadeTotal()
        {
            var response = await _dataContext.Database.SqlQueryRaw<int>("select count(1) as value from Usuarios").FirstOrDefaultAsync();

            return response;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }
    }
}
