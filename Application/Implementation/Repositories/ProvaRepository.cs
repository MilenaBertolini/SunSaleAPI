using Data.Context;
using Main = Domain.Entities.Prova;
using IRepository = Application.Interface.Repositories.IProvaRepository;
using Microsoft.EntityFrameworkCore;
using ImageMagick;

namespace Application.Implementation.Repositories
{
    public class ProvaRepository : RepositoryBase<Main>, IRepository
    {
        private static readonly string includes = "TipoProvaAssociado;TipoProvaAssociado.TipoProva";

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
                         where q.Ativo.Equals("1") && p.IsActive.Equals("1")

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
        
        public async Task<Tuple<IEnumerable<Main>, int>> GetAllPagged(int page, int quantity, string tipo, string prova, bool admin)
        {
            var query = !admin ? (from p in _dataContext.Prova
                         join q in _dataContext.Questoes on p.Codigo equals q.CodigoProva
                         where q.Ativo.Equals("1") && p.IsActive.Equals("1")

                         select p).Distinct()
                         :
                         (from p in _dataContext.Prova

                          select p).Distinct();

            GetIncludes(includes).ToList().ForEach(p => query = query.Include(p));

            if (!string.IsNullOrEmpty(prova))
            {
                query = query.Where(q => q.NomeProva.ToUpper().Contains(prova.ToUpper()));
            }

            if (!string.IsNullOrEmpty(tipo))
            {
                query = query.Where(q => q.TipoProvaAssociado.Where(t => t.TipoProva.Descricao.Equals(tipo)).Count() > 0);
            }

            var qt = await base.GetAllPagedTotalAsync(query);
            var response = await base.GetAllPagedAsync(query, page, quantity, orderBy: "NomeProva:Desc");

            return Tuple.Create(response, qt);
        }

        public async Task<IEnumerable<Main>> GetAll()
        {
            return await GetAllAsync(GetQueryable(), orderBy: "DataRegistro:desc");
        }

        public async Task<bool> UpdateStatus(int id, bool active)
        {
            var main = await GetById(id);

            if(main != null)
            {
                main.IsActive = active ? "1" : "0";
                await Update(main);
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<string>> GetBancas(string provas, string materias, string assuntos)
        {
            var provasList = provas.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var materiasList = materias.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var assuntosList = assuntos.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            var query = (from p in _dataContext.Prova
                         join q in _dataContext.Questoes on p.Codigo equals q.CodigoProva
                         where q.Ativo.Equals("1") && p.IsActive.Equals("1") 
                         && (provasList.Contains(p.NomeProva) || provas == "")
                         && (materiasList.Contains(q.Materia) || materias == "")
                         && (assuntos.Contains(q.Assunto) || assuntos == "")
                         select p.Banca).Distinct();

            return await query.ToListAsync().OrderBy(q => q); ;
        }

        public async Task<IEnumerable<string>> GetProvas(string bancas, string materias, string assuntos)
        {
            var bancasList = bancas.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var materiasList = materias.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var assuntosList = assuntos.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            var query = (from p in _dataContext.Prova
                         join q in _dataContext.Questoes on p.Codigo equals q.CodigoProva
                         where q.Ativo.Equals("1") && p.IsActive.Equals("1")
                         && (bancasList.Contains(p.Banca) || bancas == "")
                         && (materiasList.Contains(q.Materia) || materias == "")
                         && (assuntos.Contains(q.Assunto) || assuntos == "")
                         select p.NomeProva).Distinct().OrderBy(q => q); ;

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<string>> GetMaterias(string bancas, string provas, string assuntos)
        {
            var bancasList = bancas.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var provasList = provas.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var assuntosList = assuntos.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            var query = (from p in _dataContext.Prova
                         join q in _dataContext.Questoes on p.Codigo equals q.CodigoProva
                         where q.Ativo.Equals("1") && p.IsActive.Equals("1")
                         && (bancasList.Contains(p.Banca) || bancas == "")
                         && (provasList.Contains(p.NomeProva) || provas == "")
                         && (assuntos.Contains(q.Assunto) || assuntos == "")
                         select q.Materia).Distinct().OrderBy(q => q); ;

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<string>> GetAssuntos(string bancas, string provas, string materias)
        {
            var bancasList = bancas.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var provasList = provas.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var materiasList = materias.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            var query = (from p in _dataContext.Prova
                         join q in _dataContext.Questoes on p.Codigo equals q.CodigoProva
                         where q.Ativo.Equals("1") && p.IsActive.Equals("1")
                         && (bancasList.Contains(p.Banca) || bancas == "")
                         && (provasList.Contains(p.NomeProva) || provas == "")
                         && (materiasList.Contains(q.Materia) || materias == "")
                         select q.Assunto).Distinct().OrderBy(q => q);

            return await query.ToListAsync();
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

    }
}
