using Data.Context;
using Main = Domain.Entities.NotasCorteSisu;
using IRepository = Application.Interface.Repositories.INotasCorteSisuRepository;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Application.Implementation.Repositories
{
    public class NotasCorteSisuRepository : RepositoryBase<Main>, IRepository
    {
        private static readonly string includes = "";

        public NotasCorteSisuRepository(DataContext dataContext) : base(dataContext)
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

        public async Task<List<Main>> GetByCursoAndInstituicao(long curso, int instituicao)
        {
            var query = GetQueryable().Where(p => p.CodigoCurso == curso && p.CodigoInstituicaoEnsino == instituicao);

            GetIncludes(includes).ToList().ForEach(p => query = query.Include(p));

            return await query.ToListAsync();
        }

        public IEnumerable<OpcaoCodigoNome> GetInstituicoes()
        {
            var distinctData = _dataContext.NotasCorteSisu.Select(n => new OpcaoCodigoNome(){ Codigo = n.CodigoInstituicaoEnsino, Nome = n.NomeInstituicao }).Distinct().ToList();

            return distinctData;
        }

        public IEnumerable<OpcaoCodigoNome> GetCursosFromInstituicao(int instituicao)
        {
            var distinctData = _dataContext.NotasCorteSisu.Where(n => n.CodigoInstituicaoEnsino.Equals(instituicao)).Select(n => new OpcaoCodigoNome() { Codigo = n.CodigoCurso, Nome = n.NomeCurso + " - Campus " + n.NomeCampus}).Distinct().ToList();

            return distinctData;
        }

        public async Task<Main> Update(Main entity)
        {
            var model = await GetByIdAsync((int)entity.Id);
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

        public void Dispose()
        {
            this.Dispose(true);
        }

    }
}
