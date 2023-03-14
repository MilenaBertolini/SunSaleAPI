using Data.Context;
using Main = Domain.Entities.CartaoCreditoDevTools;
using IRepository = Application.Interface.Repositories.ICartaoCreditoDevToolsRepository;
using Microsoft.EntityFrameworkCore;

namespace Application.Implementation.Repositories
{
    public class CartaoCreditoDevtoolsRepository : RepositoryBase<Main>, IRepository
    {
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
            return await GetAllAsync();
        }

        public async Task<Main> GetByCartao(string cartao)
        {
            var query = GetQueryable().Where(p => p.NumeroCartao == cartao);
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
            return await base.GetAllPagedAsync(base.GetQueryable(), page, quantity);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public async Task<IEnumerable<Main>> GetRandom(int qt)
        {
            int count = _dataContext.CartaoCreditoDevTools.Count();
            int index = new Random().Next(count);
            return _dataContext.CartaoCreditoDevTools.Skip(index).Take(qt).ToList();
        }

        public Task<Main> GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
