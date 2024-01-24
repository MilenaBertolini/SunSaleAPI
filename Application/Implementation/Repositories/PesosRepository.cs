using Data.Context;
using Main = Domain.Entities.Pesos;
using IRepository = Application.Interface.Repositories.IPesosRepository;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Application.Implementation.Repositories
{
    public class PesosRepository : RepositoryBase<Main>, IRepository
    {
        private static readonly string includes = "";

        public PesosRepository(DataContext dataContext) : base(dataContext)
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

        public async Task<IEnumerable<Main>> GetByCurso(int curso)
        {
            var query = base.GetQueryable().Where(p => p.CodigoCurso == curso);
            GetIncludes(includes).ToList().ForEach(p => query = query.Include(p));

            return await query.ToListAsync();
        }

        public List<OpcaoCodigoNome> GetPesos()
        {
            var itens = this._dataContext.Pesos.Select(p => new OpcaoCodigoNome() 
            { 
                Codigo = p.CodigoCurso,
                Nome = $"{p.Curso} - {p.Turno}"
            }).Distinct().ToList();

            return itens;
        }
        public void Dispose()
        {
            this.Dispose(true);
        }

    }
}
