using Data.Context;
using Main = Domain.Entities.ResultadosTabuadaDivertida;
using IRepository = Application.Interface.Repositories.IResultadosTabuadaDivertidaRepository;
using Microsoft.EntityFrameworkCore;
using Domain.Responses;
using System.Net.Http.Headers;
using System.Security.Cryptography;

namespace Application.Implementation.Repositories
{
    public class ResultadosTabuadaDivertidaRepository : RepositoryBase<Main>, IRepository
    {
        private static readonly string includes = "";

        public ResultadosTabuadaDivertidaRepository(DataContext dataContext) : base(dataContext)
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

        public async Task<List<RankingTabuadaDivertida>> GetRankingTabuada()
        {
            var resultado = from r in _dataContext.ResultadosTabuadaDivertida
                            where r.NumerAcertos == r.NumeroQuestoes
                            group r by new { r.Nome, r.Tipo, r.NumeroQuestoes } into g
                            orderby g.Max(x => Convert.ToInt32(x.NumerAcertos)) descending, g.Min(x => Convert.ToInt32(x.Tempo)) ascending
                            select new
                            {
                                Nome = g.Key.Nome,
                                Tipo = g.Key.Tipo,
                                NumeroAcertos = g.Key.NumeroQuestoes,
                                Tempo = g.Min(x => Convert.ToInt32(x.Tempo))
                            };

            var ranking = resultado.Select(x => new RankingTabuadaDivertida()
            {
                Nome = x.Nome,
                Tempo = x.Tempo,
                Quantidade = x.NumeroAcertos,
                Tipo = x.Tipo
            }).ToList();

            return ranking;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

    }
}
