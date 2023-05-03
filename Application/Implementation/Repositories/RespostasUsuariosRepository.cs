using Data.Context;
using Main = Domain.Entities.RespostasUsuarios;
using IRepository = Application.Interface.Repositories.IRespostasUsuariosRepository;
using Microsoft.EntityFrameworkCore;
using Application.Model;
using Domain.Responses;

namespace Application.Implementation.Repositories
{
    public class RespostasUsuariosRepository : RepositoryBase<Main>, IRepository
    {
        private static readonly string includes = "RespostasQuestoes";

        public RespostasUsuariosRepository(DataContext dataContext) : base(dataContext)
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
        
        public async Task<IEnumerable<Main>> GetAllPagged(int page, int quantity, int user)
        {
            var query = base.GetQueryable().Where(u => u.CodigoUsuario.Equals(user)).Distinct();
            GetIncludes(includes).ToList().ForEach(p => query = query.Include(p));

            return await base.GetAllPagedAsync(query, page, quantity);
        }

        public async Task<IEnumerable<Main>> GetByUser(int user)
        {
            var query = base.GetQueryable().Where(r => r.CodigoUsuario.Equals(user));

            return await base.GetAllAsync(query);
        }

        public async Task<IEnumerable<Main>> GetByQuestao(int questao)
        {
            var query = (from q in _dataContext.Questoes
                         join r in _dataContext.RespostasQuestoes on q.Codigo equals r.CodigoQuestao
                         join u in _dataContext.RespostasUsuarios on r.Codigo equals u.CodigoResposta

                         where q.Codigo.Equals(questao)
                         select u);

            var response = query.AsEnumerable();

            return response;
        }

        public async Task<IEnumerable<Main>> GetByUserQuestao(int user, int questao = -1)
        {
            var query = (from q in _dataContext.Questoes
                         join r in _dataContext.RespostasQuestoes on q.Codigo equals r.CodigoQuestao
                         join u in _dataContext.RespostasUsuarios on r.Codigo equals u.CodigoResposta

                         where (q.Codigo.Equals(questao) || questao == -1) && u.CodigoUsuario.Equals(user)
                         select u);

            var response = query.AsEnumerable();

            return response;
        }

        public async Task<IEnumerable<HistoricoUsuario>> GetHistory(int user)
        {
            var query = (from r in _dataContext.RespostasUsuarios
                         join re in _dataContext.RespostasQuestoes on r.CodigoResposta equals re.Codigo
                         join q in _dataContext.Questoes on re.CodigoQuestao equals q.Codigo
                         join p in _dataContext.Prova on q.CodigoProva equals p.Codigo
                         where q.Ativo.Equals("1")
                         && r.CodigoUsuario.Equals(user)

                         select new HistoricoUsuario
                         {
                             Codigo = r.Codigo,
                             RespostaCorreta = re.Certa,
                             NumeroQuestao = q.NumeroQuestao,
                             NomeProva = p.NomeProva,
                             DataResposta = r.DataResposta,
                             CodigoQuestao = q.Codigo
                         });

            var response = query.AsEnumerable();

            return response;
        }

        public async Task<int> GetQuantidadeQuestoesCertas(int user)
        {
            var query = (from r in _dataContext.RespostasUsuarios
                         join re in _dataContext.RespostasQuestoes on r.CodigoResposta equals re.Codigo
                         where r.CodigoUsuario.Equals(user) && re.Certa.Equals("1")

                         select r.CodigoResposta).Distinct();

            return query.Count();
        }

        public async Task<int> GetQuantidadeQuestoesTentadas(int user)
        {
            var query = (from r in _dataContext.RespostasUsuarios
                         where r.CodigoUsuario.Equals(user)

                         select r.CodigoResposta).Distinct();

            return query.Count();
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

    }
}
