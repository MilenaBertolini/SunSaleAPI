using Data.Context;
using Main = Domain.Entities.Questoes;
using IRepository = Application.Interface.Repositories.IQuestoesRepository;
using Microsoft.EntityFrameworkCore;
using Application.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Domain.Entities;

namespace Application.Implementation.Repositories
{
    public class QuestoesRepository : RepositoryBase<Main>, IRepository
    {
        private static readonly string includes = "RespostasQuestoes;AnexosQuestoes";

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
            return await GetAllAsync(includes: includes.Split(';'));
        }

        public async Task<Main> GetById(int id)
        {
            var query = GetQueryable().Where(p => p.Codigo == id);
            
            includes.Split(';').ToList().ForEach(p => query = query.Include(p));

            return await query.SingleOrDefaultAsync();
        }

        public async Task<Main> Update(Main entity)
        {
            var model = await GetByIdAsync(entity.Codigo);
            if (model == null)
                return null;

            model.DataRegistro = entity.DataRegistro;
            model.ObservacaoQuestao = entity.ObservacaoQuestao;
            model.CampoQuestao = entity.CampoQuestao;
            model.NumeroQuestao = entity.NumeroQuestao;
            model.CodigoProva = entity.CodigoProva;
            model.Ativo = entity.Ativo;
            model.DataRegistro = entity.DataRegistro;
            model.Materia = entity.Materia;

            base.Update(model);
            await base.CommitAsync();
            return model;
        }
        
        public async Task<IEnumerable<Main>> GetAllPagged(int page, int quantity)
        {
            return await base.GetAllPagedAsync(base.GetQueryable().Include(includes), page, quantity);
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

        public async Task<IEnumerable<Main>> GetByProva(int prova)
        {
            var query = (from q in _dataContext.Questoes
                        where q.CodigoProva == prova
                        select q);

            includes.Split(';').ToList().ForEach(p => query = query.Include(p));

            var response = query.AsEnumerable();
            return response;
        }

        public async Task<IEnumerable<Main>> GetByMateria(string materia)
        {
            var query = (from q in _dataContext.Questoes
                        where q.Materia == materia
                        select q);

            includes.Split(';').ToList().ForEach(p => query = query.Include(p));

            var response = query.AsEnumerable();

            return response;
        }
    }
}
