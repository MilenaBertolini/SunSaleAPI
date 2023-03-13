﻿using Data.Context;
using Main = Domain.Entities.RespostasQuestoes;
using IRepository = Application.Interface.Repositories.IRespostasQuestoesRepository;
using Microsoft.EntityFrameworkCore;

namespace Application.Implementation.Repositories
{
    public class RespostasQuestoesRepository : RepositoryBase<Main>, IRepository
    {
        public RespostasQuestoesRepository(DataContext dataContext) : base(dataContext)
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
            model.Certa = entity.Certa;
            model.CodigoQuestao = entity.CodigoQuestao;
            model.ObservacaoResposta = entity.ObservacaoResposta;
            model.TextoResposta = entity.TextoResposta;

            base.Update(model);
            await base.CommitAsync();
            return model;
        }
        
        public async Task<IEnumerable<Main>> GetAllPagged(int page, int quantity)
        {
            return await base.GetAllPagedAsync(base.GetQueryable(), page, quantity);
        }

        public async Task<IEnumerable<Main>> GetByCodigoQuestao(int codigo)
        {
            var query = from r in base.GetQueryable()
                        where r.CodigoQuestao == codigo
                        select r;

            return await query.ToListAsync();
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

    }
}
