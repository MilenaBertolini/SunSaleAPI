using Data.Context;
using Main = Domain.Entities.RespostasAvaliacoes;
using IRepository = Application.Interface.Repositories.IRespostasAvaliacoesRepository;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Application.Implementation.Repositories
{
    public class RespostasAvaliacoesRepository : RepositoryBase<Main>, IRepository
    {
        private static readonly string includes = "Questao;Questao.Prova;Avaliacao;Resposta";

        public RespostasAvaliacoesRepository(DataContext dataContext) : base(dataContext)
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

            return await base.GetAllPagedAsync(query, page, quantity, orderBy: "CreatedOn:Desc");
        }

        public async Task<IEnumerable<Main>> GetByAvaliacao(int avaliacao, int user)
        {
            var query = base.GetQueryable().Where(r => r.IdAvaliacao.Equals(avaliacao));

            if(user != -1)
            {
                query = query.Where(r => r.CreatedBy.Equals(user));
            }

            GetIncludes(includes).ToList().ForEach(p => query = query.Include(p));

            query = query.OrderBy(r => r.CreatedBy);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Usuarios>> GetUsuariosFizeramAvaliacao(int avaliacao)
        {
            var query = (from r in _dataContext.RespostasAvaliacoes
                         join u in _dataContext.Usuarios on r.CreatedBy equals u.Id
                         where r.IdAvaliacao == avaliacao

                         select u).OrderBy(u => u.Nome);

            return await query.Distinct().ToListAsync();
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

    }
}
