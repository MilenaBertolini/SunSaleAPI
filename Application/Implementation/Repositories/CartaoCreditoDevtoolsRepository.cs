using Data.Context;
using Main = Domain.Entities.CartaoCreditoDevTools;
using IRepository = Application.Interface.Repositories.ICartaoCreditoDevToolsRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Application.Implementation.Repositories
{
    public class CartaoCreditoDevtoolsRepository : RepositoryBase<Main>, IRepository
    {
        private static readonly string includes = "";

        public CartaoCreditoDevtoolsRepository(DataContext dataContext) : base(dataContext)
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

        public async Task<Main> GetByCartao(string cartao)
        {
            var query = GetQueryable().Where(p => p.NumeroCartao == cartao);
            GetIncludes(includes).ToList().ForEach(p => query = query.Include(p));

            return await query.SingleOrDefaultAsync();
        }

        public async Task<Main> Update(Main entity)
        {
            var model = await GetByCartao(entity.NumeroCartao);
            if (model == null)
                return null;

            model.CodigoSeguranca = entity.CodigoSeguranca;
            model.Created = entity.Created;
            model.DataValidade = entity.DataValidade;

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

        public void Dispose()
        {
            this.Dispose(true);
        }

        public async Task<IEnumerable<Main>> GetRandom(int qt)
        {
            int count = _dataContext.CartaoCreditoDevTools.Count();

            List<Main> list = new List<Main>();

            int limite = 1000;
            int i = 0;

            while (list.Count < qt && i != limite)
            {
                i++;
                int index = new Random().Next(count);
                var temp = _dataContext.CartaoCreditoDevTools.Skip(index).FirstOrDefault();

                if (temp == null) continue;

                if (!list.Contains(temp)) list.Add(temp);
            }

            return list;
        }

        public Task<Main> GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
