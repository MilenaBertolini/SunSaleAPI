using Data.Context;
using Main = Domain.Entities.Prova;
using IRepository = Application.Interface.Repositories.IProvaRepository;
using Microsoft.EntityFrameworkCore;
using ImageMagick;

namespace Application.Implementation.Repositories
{
    public class ProvaRepository : RepositoryBase<Main>, IRepository
    {
        private static readonly string includes = "";

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

        public async Task<IEnumerable<Main>> GetSimulados()
        {
            var query = (from p in _dataContext.Prova
                         join q in _dataContext.Questoes on p.Codigo equals q.CodigoProva
                         where q.Ativo.Equals("1")

                         select p).Distinct();

            GetIncludes(includes).ToList().ForEach(p => query = query.Include(p));

            return await GetAllAsync(query, orderBy: "codigo:desc");
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
            entity.DataRegistro = model.DataRegistro;
            if (model == null)
                return null;

            base.Merge(model, entity);
            base.Update(model);
            await base.CommitAsync();

            return model;
        }
        
        public async Task<Tuple<IEnumerable<Main>, int>> GetAllPagged(int page, int quantity, string prova, bool admin)
        {
            var query = !admin ? (from p in _dataContext.Prova
                         join q in _dataContext.Questoes on p.Codigo equals q.CodigoProva
                         where q.Ativo.Equals("1")

                         select p).Distinct()
                         :
                         (from p in _dataContext.Prova

                          select p).Distinct();

            GetIncludes(includes).ToList().ForEach(p => query = query.Include(p));

            if (!string.IsNullOrEmpty(prova))
            {
                query = query.Where(q => q.NomeProva.ToUpper().Contains(prova.ToUpper()));
            }

            var response = await base.GetAllPagedAsync(query, page, quantity);
            var qt = await base.GetAllPagedTotalAsync(query);

            return Tuple.Create(response, qt);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

    }
}
