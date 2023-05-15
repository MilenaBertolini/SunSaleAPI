using Data.Context;
using Main = Domain.Entities.Questoes;
using IRepository = Application.Interface.Repositories.IQuestoesRepository;
using Microsoft.EntityFrameworkCore;
using Application.Model;
using static Data.Helper.EnumeratorsTypes;

namespace Application.Implementation.Repositories
{
    public class QuestoesRepository : RepositoryBase<Main>, IRepository
    {
        private static readonly string includes = "RespostasQuestoes;RespostasQuestoes.AnexoResposta;AnexosQuestoes";

        public QuestoesRepository(DataContext dataContext) : base(dataContext)
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
            entity.DataRegistro = model.DataRegistro;
            if (model == null)
                return null;

            base.Merge(model, entity);

            base.Update(model);
            await base.CommitAsync();
            return model;
        }
        
        public async Task<Tuple<IEnumerable<Main>, int>> GetAllPagged(int page, int quantity, int user, bool includeAnexos, int? codigoProva, string? subject)
        {
            var query = base.GetQueryable().Where(q => q.Ativo.Equals("1"));
            if (codigoProva.HasValue)
            {
                query = query.Where(q => q.CodigoProva.Equals(codigoProva));
            }

            if (!string.IsNullOrEmpty(subject))
            {
                query = query.Where(q => q.Materia.ToUpper().Contains(subject.ToUpper()));
            }

            string include = includes;
            if (!includeAnexos)
                include = include.Replace(";RespostasQuestoes.AnexoResposta;AnexosQuestoes", "");

            var list = GetIncludes(include).ToList();
            list.ForEach(p => query = query.Include(p));

            query = query.OrderBy(q => int.Parse(q.NumeroQuestao.StartsWith('0') ? q.NumeroQuestao.Substring(1) : q.NumeroQuestao));

            var response = await base.GetAllPagedAsync(query, page, quantity);

            var qt = await base.GetAllPagedTotalAsync(query);

            return Tuple.Create(response, qt);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public async Task<IEnumerable<string>> GetMaterias(int prova = -1)
        {
            var query = (from q in _dataContext.Questoes
                         where 
                         q.CodigoProva == prova
                         ||
                         prova == -1
                        select q.Materia).Distinct();

            var response = query.AsEnumerable();

            return response;
        }

        public async Task<IEnumerable<Test>> GetTests(int id = -1)
        {
            var query = _dataContext.Prova.Select(i => new Test
            {
                Banca = i.Banca,
                Codigo = i.Codigo,
                DataAplicacao = i.DataAplicacao,
                Local = i.Local,
                NomeProva = i.NomeProva,
                ObservacaoGabarito = i.ObservacaoGabarito,
                ObservacaoProva = i.ObservacaoProva,
                TipoProva = i.TipoProva,
                LinkGabarito = i.LinkGabarito,
                LinkProva = i.LinkProva
            }).Where(t => (t.Codigo.Equals(id) || id == -1) && t.Codigo != 9999);

            var response = query.AsEnumerable();

            return response;
        }

        public async Task<Main> GetByProva(int prova, int numero)
        {
            var query = (from q in _dataContext.Questoes
                        where q.CodigoProva == prova && q.NumeroQuestao.Equals(numero.ToString())
                        select q);

            GetIncludes(includes).ToList().ForEach(p => query = query.Include(p));

            var response = await query.FirstOrDefaultAsync();
            return response;
        }

        public async Task<IEnumerable<Main>> GetByProva(int prova)
        {
            var query = (from q in _dataContext.Questoes
                         where q.CodigoProva == prova
                         select q);

            GetIncludes(includes).ToList().ForEach(p => query = query.Include(p));

            var response = query.AsEnumerable();
            return response;
        }

        public async Task<IEnumerable<Main>> GetByMateria(string materia)
        {
            var query = (from q in _dataContext.Questoes
                        where q.Materia == materia
                        select q);

            GetIncludes(includes).ToList().ForEach(p => query = query.Include(p));

            var response = query.AsEnumerable();

            return response;
        }

        public async Task<int> QuantidadeQuestoes(int prova, int user = -1)
        {
            var query = user == -1 ?
                (from q in _dataContext.Questoes
                 where q.CodigoProva.Equals(prova) && q.Ativo.Equals("1")

                 select q)
                :
                (from q in _dataContext.Questoes
                         join r in _dataContext.RespostasQuestoes on q.Codigo equals r.CodigoQuestao
                         join u in _dataContext.RespostasUsuarios on r.Codigo equals u.CodigoResposta
                         where q.CodigoProva.Equals(prova) && q.Ativo.Equals("1")
                         && u.CodigoUsuario.Equals(user) && r.Certa.Equals("1")

                         select q).Distinct();

            var response = await query.CountAsync();

            return response;
        }

        public async Task<IEnumerable<string>> GetAllMateris()
        {
            var query = (from q in _dataContext.Questoes
                         where q.Ativo.Equals("1")
                         select q.Materia).Distinct();

            return query.AsEnumerable();
        }

        public async Task<Main> GetQuestoesAleatoria(TipoQuestoes tipo, string? subject, string? banca)
        {
            var query = GetQueryable();
            if(tipo == TipoQuestoes.ENEM)
            {
                query = (from q in _dataContext.Questoes
                         join p in _dataContext.Prova on q.CodigoProva equals p.Codigo
                         where p.NomeProva.ToUpper().Contains("ENEM")

                         select q);

            }

            if (!string.IsNullOrEmpty(subject))
            {
                string[] array = subject.Split(';');

                int random = new Random().Next(array.Length);
                query = query.Where(q => q.Materia.Equals(array[random]));
            }

            if(!string.IsNullOrEmpty(banca))
            {
                string[] array = banca.Split(';');

                int random = new Random().Next(array.Length);

                var queryProva = (from p in _dataContext.Prova where p.Banca.ToUpper().Contains(array[random]) select p);
                var prova = queryProva.FirstOrDefault();

                if(prova != null)
                    query = query.Where(q => q.CodigoProva == prova.Codigo);
            }

            GetIncludes(includes).ToList().ForEach(p => query = query.Include(p));

            int index = new Random().Next(query.Count());

            return await query.Skip(index).FirstOrDefaultAsync();
        }
    }
}
