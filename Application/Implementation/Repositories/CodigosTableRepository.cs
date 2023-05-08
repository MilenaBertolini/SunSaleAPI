using Data.Context;
using Main = Domain.Entities.Codigos_Table;
using IRepository = Application.Interface.Repositories.ICodigosTableRepository;
using Microsoft.EntityFrameworkCore;

namespace Application.Implementation.Repositories
{
    public class CodigosTableRepository : RepositoryBase<Main>, IRepository
    {
        public CodigosTableRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public async Task<Main> Add(Main entity)
        {
            base.Add(entity);
            await base.CommitAsync();
            return entity;
        }

        public async Task<Main> GetByTable(string table)
        {
            var query = GetQueryable().Where(p => p.Tabela.ToUpper().Equals(table.ToUpper()));
            return await query.SingleOrDefaultAsync();
        }

        public async Task<Main> Update(Main entity)
        {
            var model = await GetByIdAsync(entity.Tabela);
            if (model == null)
                return null;

            base.Merge(model, entity);

            base.Update(model);
            await base.CommitAsync();
            return model;
        }
        

        public async Task<int> GetNextCodigo(string table)
        {
            var model = await GetByTable(table);
            if (model == null)
            {
                await Add(new Main { Tabela = table, Codigo = 1000 });
                model = await GetByTable(table);
            }

            model.Codigo++;
            await this.Update(model);

            return model.Codigo;
        }
        public void Dispose()
        {
            this.Dispose(true);
        }

        public Task<Main> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
