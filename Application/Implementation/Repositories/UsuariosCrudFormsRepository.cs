﻿using Data.Context;
using Main = Domain.Entities.UsuariosCrudForms;
using IRepository = Application.Interface.Repositories.IUsuariosCrudFormsRepository;
using Microsoft.EntityFrameworkCore;
using Application.Model;
using Domain.Responses;

namespace Application.Implementation.Repositories
{
    public class UsuariosCrudFormsRepository : RepositoryBase<Main>, IRepository
    {
        private static readonly string includes = "";

        public UsuariosCrudFormsRepository(DataContext dataContext) : base(dataContext)
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

        public async Task<IEnumerable<Main>> GetByUser(int id)
        {
            var query = GetQueryable().Where(p => p.UsuarioPai == id);

            GetIncludes(includes).ToList().ForEach(p => query = query.Include(p));

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
            entity.Created = model.Created;
            entity.Updated = DateTime.Now;
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

        public async Task<Main> VerifyLogin(string user, string pass)
        {
            var query = GetQueryable().Where(p => (p.Login.Equals(user) || p.Email.Equals(user)) && p.Senha.Equals(pass));
            return await query.SingleOrDefaultAsync();
        }

        public async Task<Main> GetByEmail(string email)
        {
            var query = GetQueryable().Where(p => p.Email.Equals(email) || p.Login.Equals(email));
            return await query?.SingleOrDefaultAsync();
        }

        public async Task<Main> GetByLogin(string login)
        {
            var query = GetQueryable().Where(p => p.Login.Equals(login));
            return await query?.SingleOrDefaultAsync();
        }

        public async Task<PerfilUsuario> GetPerfil(int user)
        {
            var query = (from u in _dataContext.Usuarios
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
                                 Pass = u.Pass
                             }
                         });

            var response = await query.FirstOrDefaultAsync();
            return response;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }
    }
}
