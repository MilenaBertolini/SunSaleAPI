using Data.Context;
using Main = Domain.Entities.Avaliacao;
using IRepository = Application.Interface.Repositories.IAvaliacaoRepository;
using Microsoft.EntityFrameworkCore;

namespace Application.Implementation.Repositories
{
    public class AvaliacaoRepository : RepositoryBase<Main>, IRepository
    {
        private static readonly string includes = "QuestoesAvaliacao;QuestoesAvaliacao.Questao;Usuario";

        public AvaliacaoRepository(DataContext dataContext) : base(dataContext)
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

            entity.CreatedOn = model.CreatedOn;
            entity.CreatedBy = model.CreatedBy;
            base.Merge(model, entity);

            base.Update(model);
            await base.CommitAsync();
            return model;
        }
        
        public async Task<IEnumerable<Main>> GetAllPagged(int page, int quantity, string chave, int user)
        {
            var query = base.GetQueryable().Where(a => a.IsPublic.Equals("1") && a.IsActive.Equals("1"));

            if(user != -1)
            {
                query = query.Where(a => a.CreatedBy == user);
            }

            if (string.IsNullOrEmpty(chave))
            {
                query = query.Where(a => a.Key == chave);
            }

            GetIncludes(includes).ToList().ForEach(p => query = query.Include(p));

            return await base.GetAllPagedAsync(query, page, quantity, orderBy: "CreatedOn:Desc");
        }

        public async Task<IEnumerable<Main>> GetByUserId(int id)
        {
            var query = GetQueryable().Where(p => p.CreatedBy == id);

            GetIncludes(includes).ToList().ForEach(p => query = query.Include(p));

            return await query.ToListAsync();
        }

        public async Task<int> QuantidadeTotal()
        {
            var response = await _dataContext.Database.SqlQueryRaw<int>("select count(1) as value from Avaliacao where IsPublic = '1'").FirstOrDefaultAsync();

            return response;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

    }
}
