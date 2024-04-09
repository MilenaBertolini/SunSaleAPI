using Main = Domain.Entities.MeuDesempenho;

namespace Application.Interface.Services
{
    public interface IMeuDesempenhoService : IDisposable
    {
        Task<Main> GetAllDados(int user);
    }
}
