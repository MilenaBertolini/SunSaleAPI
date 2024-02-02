using Data.Context;
using Main = Domain.Entities.AdminData;
using IRepository = Application.Interface.Repositories.IAdminRepository;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.ViewModel;
using Data.Helper;

namespace Application.Implementation.Repositories
{
    public class AdminRepository : RepositoryBase<Main>, IRepository
    {
        private static readonly string includes = "";

        public AdminRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public async Task<Main> GetAllDados()
        {
            var main = new Main();
            main.QuantidadeVerificados = await _dataContext.Database.SqlQueryRaw<int>("select count(1) as value from USUARIOS where IsVerified = '1'").FirstOrDefaultAsync();
            main.QuantidadeNaoVerificados = await _dataContext.Database.SqlQueryRaw<int>("select count(1) as value from USUARIOS where IsVerified = '0'").FirstOrDefaultAsync();
            main.QuantidadeTotal = await _dataContext.Database.SqlQueryRaw<int>("select count(1) as value from USUARIOS").FirstOrDefaultAsync();
            main.QuantidadeRespostas = await _dataContext.Database.SqlQueryRaw<int>("select count(1) as value from RESPOSTASUSUARIOS").FirstOrDefaultAsync();
            main.QuantidadeRespostasCertas = await _dataContext.Database.SqlQueryRaw<int>("select count(1) as value from RESPOSTASUSUARIOS where DATARESPOSTA >= GETDATE()-1").FirstOrDefaultAsync();
            main.QuantidadeRespostasUltimas24Horas = await _dataContext.Database.SqlQueryRaw<int>("select count(1) as value from PROVA where IsActive = '0'").FirstOrDefaultAsync();
            main.QuantidadeQuestoesAtivas = await _dataContext.Database.SqlQueryRaw<int>("select count(1) as value from QUESTOES where ativo = '1'").FirstOrDefaultAsync();
            main.QuantidadeQuestoesSolicitadasRevisao = await _dataContext.Database.SqlQueryRaw<int>("select count(1) as value from QUESTOES where ativo = '0'").FirstOrDefaultAsync();
            main.QuantidadeProvasAtivas = await _dataContext.Database.SqlQueryRaw<int>("select count(1) as value from PROVA where IsActive = '1'").FirstOrDefaultAsync();
            main.QuantidadeProvasDesativasAtivas = await _dataContext.Database.SqlQueryRaw<int>("select count(1) as value from PROVA where IsActive = '0'").FirstOrDefaultAsync();

            main.UsuariosDates = await _dataContext.Database.SqlQueryRaw<AdminUsuariosDate>("SELECT  CONVERT(date, Created) AS Date, COUNT(*) AS Count FROM USUARIOS WHERE Created >= DATEADD(day, -30, GETDATE()) GROUP BY CONVERT(date, Created) ORDER BY CONVERT(date, Created) DESC").ToListAsync();

            return main;
        }

        public async Task<IEnumerable<Questoes>> BuscaQuestoesSolicitadasRevisao(int page, int quantity)
        {
            var query = (from q in _dataContext.Questoes
                         where q.Ativo != "1" orderby q.DataRegistro
                         select q);
            var response = await query.ToListPagedAsync(page, quantity);

            return response;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

    }
}
