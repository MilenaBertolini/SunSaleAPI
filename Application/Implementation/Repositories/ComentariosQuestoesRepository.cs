using Data.Context;
using Main = Domain.Entities.ComentariosQuestoes;
using IRepository = Application.Interface.Repositories.IComentariosQuestoesRepository;
using Microsoft.EntityFrameworkCore;
using Application.Model;

namespace Application.Implementation.Repositories
{
    public class ComentariosQuestoesRepository : RepositoryBase<Main>, IRepository
    {
        private static readonly string includes = "";

        public ComentariosQuestoesRepository(DataContext dataContext) : base(dataContext)
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
            var query = GetQueryable().Where(p => p.Codigo == id);

            GetIncludes(includes).ToList().ForEach(p => query = query.Include(p));

            return await query.SingleOrDefaultAsync();
        }

        public async Task<Main> Update(Main entity)
        {
            var model = await GetByIdAsync(entity.Codigo);
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

        public async Task<IEnumerable<ComentariosViewModel>> GetByQuestao(int questao, int user)
        {
            var query = (from c in _dataContext.ComentariosQuestoes
                         join u in _dataContext.Usuarios on c.CodigoUsuario equals u.Id
                         where c.CodigoQuestao.Equals(questao)

                         select new ComentariosViewModel()
                         {
                             Codigo = c.Codigo,
                             CodigoQuestao = c.CodigoQuestao,
                             CodigoUsuario = c.CodigoUsuario,
                             Comentario = c.Comentario,
                             Created = c.Created,
                             NomeUsuario = u.Nome,
                             CanEdit = u.Id.Equals(user)
                         }).OrderByDescending(c => c.Created);

            return await query.ToListAsync();
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

    }
}
