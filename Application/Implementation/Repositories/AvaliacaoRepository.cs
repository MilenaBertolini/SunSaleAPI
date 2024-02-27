using Data.Context;
using Main = Domain.Entities.Avaliacao;
using IRepository = Application.Interface.Repositories.IAvaliacaoRepository;
using Microsoft.EntityFrameworkCore;

namespace Application.Implementation.Repositories
{
    public class AvaliacaoRepository : RepositoryBase<Main>, IRepository
    {
        private static readonly string includes = "QuestoesAvaliacao;QuestoesAvaliacao.Questao;QuestoesAvaliacao.Questao.Prova;Usuario";

        public AvaliacaoRepository(DataContext dataContext) : base(dataContext)
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

            entity.CreatedOn = model.CreatedOn;
            entity.CreatedBy = model.CreatedBy;
            base.Merge(model, entity);

            base.Update(model);
            await base.CommitAsync();
            return model;
        }
        
        public async Task<IEnumerable<Main>> GetAllPagged(int page, int quantity, string chave, int user, string subject, string bancas, string provas, string materias, string professores)
        {
            var query = base.GetQueryable().Where(a => a.IsPublic.Equals("1") && a.IsActive.Equals("1"));
            GetIncludes(includes).ToList().ForEach(p => query = query.Include(p));

            if (user != -1)
            {
                query = query.Where(a => a.CreatedBy == user);
            }

            if (string.IsNullOrEmpty(chave))
            {
                query = query.Where(a => a.Key == chave);
            }

            if (!string.IsNullOrEmpty(bancas))
            {
                var bancasList = bancas.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                query = query.Where(q => q.QuestoesAvaliacao.Any(q1 => bancasList.Contains(q1.Questao.Prova.Banca)));
            }

            if (!string.IsNullOrEmpty(provas))
            {
                var provasList = provas.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                query = query.Where(q => q.QuestoesAvaliacao.Any(q1 => provasList.Contains(q1.Questao.Prova.NomeProva)));
            }

            if (!string.IsNullOrEmpty(materias))
            {
                var materiasList = materias.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                query = query.Where(q => q.QuestoesAvaliacao.Any(q1 => materiasList.Contains(q1.Questao.Materia)));
            }

            if (!string.IsNullOrEmpty(subject))
            {
                var assuntosList = subject.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                query = query.Where(q => q.QuestoesAvaliacao.Any(q1 => assuntosList.Contains(q1.Questao.Assunto)));
            }

            if (!string.IsNullOrEmpty(professores))
            {
                var professoresList = professores.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                query = query.Where(q => professoresList.Contains(q.Usuario.Nome));
            }

            return await base.GetAllPagedAsync(query, page, quantity, orderBy: "CreatedOn:Desc");
        }

        public async Task<IEnumerable<Main>> GetByUserId(int id)
        {
            var query = GetQueryable().Where(p => p.CreatedBy == id);

            GetIncludes(includes).ToList().ForEach(p => query = query.Include(p));

            return await query.ToListAsync();
        }

        public async Task<int> QuantidadeTotal()
        {
            var response = await _dataContext.Database.SqlQueryRaw<int>("select count(1) as value from Avaliacao where IsPublic = '1'").FirstOrDefaultAsync();

            return response;
        }

        public async Task<IEnumerable<string>> GetAllProfessores()
        {
            var query = GetQueryable();

            return await query.Select(q => q.Usuario.Nome).Distinct().ToListAsync();
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

    }
}
