namespace Application.Interface.Repositories
{
    public interface IMeuDesempenhoRepository
    {
        void Dispose();
        Task<int> BuscaQuestoesIncorretas(int user);
        Task<int> BuscaQuestoesCorretas(int user);
        Task<int> BuscaQuestoesTentadas(int user);
    }
}
