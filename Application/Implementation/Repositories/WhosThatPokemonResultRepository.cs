﻿using Data.Context;
using Main = Domain.Entities.WhosThatPokemonResult;
using IRepository = Application.Interface.Repositories.IWhosThatPokemonResultRepository;
using Microsoft.EntityFrameworkCore;

namespace Application.Implementation.Repositories
{
    public class WhosThatPokemonResultRepository : RepositoryBase<Main>, IRepository
    {
        private static readonly string includes = "";

        public WhosThatPokemonResultRepository(DataContext dataContext) : base(dataContext)
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
        
        public async Task<IEnumerable<Main>> GetAllPagged(int page, int quantity)
        {
            var query = base.GetQueryable();
            GetIncludes(includes).ToList().ForEach(p => query = query.Include(p));

            return await base.GetAllPagedAsync(query, page, quantity);
        }

        public async Task<IEnumerable<Main>> GetRanking(bool custom)
        {
            var query = (from r in _dataContext.WhosThatPokemonResult
                        where (r.Tempo == 60 && !custom) || (custom && r.Tempo == 0)
                        orderby r.Acertos descending
                        
                        select r).Take(10);

            return await query.ToListAsync();
        }


        public void Dispose()
        {
            this.Dispose(true);
        }

    }
}
