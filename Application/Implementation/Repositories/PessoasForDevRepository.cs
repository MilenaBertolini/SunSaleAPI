using Data.Context;
using Main = Domain.Entities.PessoasForDev;
using IRepository = Application.Interface.Repositories.IPessoasForDevRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Application.Implementation.Repositories
{
    public class PessoasForDevRepository : RepositoryBase<Main>, IRepository
    {
        private static readonly string includes = "";

        public PessoasForDevRepository(DataContext dataContext) : base(dataContext)
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

        public async Task<Main> GetByCpf(string cpf)
        {
            var query = GetQueryable().Where(p => p.CPF == cpf);
            GetIncludes(includes).ToList().ForEach(p => query = query.Include(p));

            return await query.SingleOrDefaultAsync();
        }

        public async Task<Main> Update(Main entity)
        {
            var model = await GetByCpf(entity.CPF);
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

        public async Task<IEnumerable<Main>> GetRandom(int qt)
        {
            int count = _dataContext.PessoasForDev.Count();

            List<Main> list = new List<Main>();

            int limite = 1000;
            int i = 0;

            while (list.Count < qt && i != limite)
            {
                i++;
                int index = new Random().Next(count);
                var temp = _dataContext.PessoasForDev.Skip(index).FirstOrDefault();

                if (temp == null) continue;

                if (!list.Contains(temp)) list.Add(temp);
            }

            return list;
        }

        public Task<Main> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            this.Dispose(true);
        }
    }
}
