﻿using Data.Context;
using Main = Domain.Entities.EmpresaForDev;
using IRepository = Application.Interface.Repositories.IEmpresaForDevRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Application.Implementation.Repositories
{
    public class EmpresaForDevRepository : RepositoryBase<Main>, IRepository
    {
        private static readonly string includes = "";

        public EmpresaForDevRepository(DataContext dataContext) : base(dataContext)
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

        public async Task<Main> GetByCnpj(string cnpj)
        {
            var query = GetQueryable().Where(p => p.CNPJ == cnpj);
            GetIncludes(includes).ToList().ForEach(p => query = query.Include(p));

            return await query.SingleOrDefaultAsync();
        }

        public async Task<Main> Update(Main entity)
        {
            var model = await GetByCnpj(entity.CNPJ);
            if (model == null)
                return null;

            model.Bairro = entity.Bairro;
            model.Celular = entity.Celular;
            model.CEP = entity.CEP;
            model.Cidade= entity.Cidade;
            model.DataAbertura = entity.DataAbertura;
            model.Email = entity.Email;
            model.Endereco = entity.Email;
            model.Estado = entity.Estado;
            model.IE = entity.IE;
            model.Nome = entity.Nome;
            model.Numero = entity.Numero;
            model.Site = entity.Site;

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

        public Task<Main> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Main>> GetRandom(int qt)
        {
            int count = _dataContext.EmpresaForDev.Count();

            List<Main> list = new List<Main>();

            int limite = 1000;
            int i = 0;

            while (list.Count < qt && i != limite)
            {
                i++;
                int index = new Random().Next(count);
                var temp = _dataContext.EmpresaForDev.Skip(index).FirstOrDefault();

                if (temp == null) continue;

                if (!list.Contains(temp)) list.Add(temp);
            }

            return list;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }
    }
}
