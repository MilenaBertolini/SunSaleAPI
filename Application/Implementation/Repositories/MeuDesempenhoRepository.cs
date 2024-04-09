using Data.Context;
using Main = Domain.Entities.MeuDesempenho;
using IRepository = Application.Interface.Repositories.IMeuDesempenhoRepository;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Data.Helper;

namespace Application.Implementation.Repositories
{
    public class MeuDesempenhoRepository : RepositoryBase<Main>, IRepository
    {
        private static readonly string includes = "";

        public MeuDesempenhoRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public async Task<int> BuscaQuestoesIncorretas(int user)
        {
            int retorno = await _dataContext.Database.SqlQueryRaw<int>($"select count(distinct q.CODIGO) as value from QUESTOES q where q.CODIGO in (select ru.CodigoQuestao from RESPOSTASUSUARIOS ru, RESPOSTASQUESTOES r where r.CODIGO = ru.CODIGORESPOSTA and r.CERTA <> '1' and ru.CODIGOUSUARIO = {user}) and q.CODIGO not in (select ru.CodigoQuestao from RESPOSTASUSUARIOS ru, RESPOSTASQUESTOES r where r.CODIGO = ru.CODIGORESPOSTA and r.CERTA = '1' and ru.CODIGOUSUARIO = {user})").FirstOrDefaultAsync();

            return retorno;
        }

        public async Task<int> BuscaQuestoesCorretas(int user)
        {
            int retorno = await _dataContext.Database.SqlQueryRaw<int>($"select count(distinct q.CODIGO) as value from QUESTOES q where q.CODIGO in (select ru.CodigoQuestao from RESPOSTASUSUARIOS ru, RESPOSTASQUESTOES r where r.CODIGO = ru.CODIGORESPOSTA and r.CERTA = '1' and ru.CODIGOUSUARIO = {user}) and q.CODIGO not in (select ru.CodigoQuestao from RESPOSTASUSUARIOS ru, RESPOSTASQUESTOES r where r.CODIGO = ru.CODIGORESPOSTA and r.CERTA <> '1' and ru.CODIGOUSUARIO = {user})").FirstOrDefaultAsync();

            return retorno;
        }

        public async Task<int> BuscaQuestoesTentadas(int user)
        {
            int retorno = await _dataContext.Database.SqlQueryRaw<int>($"select count(distinct q.CODIGO) as value from QUESTOES q where q.CODIGO in (select ru.CodigoQuestao from RESPOSTASUSUARIOS ru, RESPOSTASQUESTOES r where r.CODIGO = ru.CODIGORESPOSTA and ru.CODIGOUSUARIO = {user})").FirstOrDefaultAsync();

            return retorno;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

    }
}
