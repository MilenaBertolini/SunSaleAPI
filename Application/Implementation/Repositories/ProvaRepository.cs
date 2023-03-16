﻿using Data.Context;
using Main = Domain.Entities.Prova;
using IRepository = Application.Interface.Repositories.IProvaRepository;
using Microsoft.EntityFrameworkCore;

namespace Application.Implementation.Repositories
{
    public class ProvaRepository : RepositoryBase<Main>, IRepository
    {
        public ProvaRepository(DataContext dataContext) : base(dataContext)
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
            return await GetAllAsync();
        }

        public async Task<Main> GetById(int id)
        {
            var query = GetQueryable().Where(p => p.Codigo == id);
            return await query.SingleOrDefaultAsync();
        }

        public async Task<Main> Update(Main entity)
        {
            var model = await GetByIdAsync(entity.Codigo);
            if (model == null)
                return null;

            model.DataRegistro = entity.DataRegistro;
            model.LinkProva = entity.LinkProva;
            model.LinkGabarito = entity.LinkGabarito;
            model.ObservacaoProva = entity.ObservacaoProva;
            model.ObservacaoGabarito = entity.ObservacaoGabarito;
            model.Banca = entity.Banca;
            model.Local = entity.Local;
            model.DataAplicacao = entity.DataAplicacao;

            base.Update(model);
            await base.CommitAsync();
            return model;
        }
        
        public async Task<IEnumerable<Main>> GetAllPagged(int page, int quantity)
        {
            return await base.GetAllPagedAsync(base.GetQueryable(), page, quantity);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

    }
}
