using Domain.Entities;
using Main = Domain.Entities.RespostasQuestoes;

namespace Application.Interface.Services
{
    public interface IRespostasQuestoesService : IDisposable
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity);
        Task<Main> GetById(int id);
        Task<Main> Add(Main entity);
        Task<Main> Update(Main entity);
        Task<bool> DeleteById(int id);
        Task<Main> GetRespostaCorreta(int questao);
        string CriaDocumentoDetalhado(IEnumerable<Questoes> questoes, Usuarios user, List<Prova> provas, List<RespostasUsuarios> respostasUsuarios);

    }
}
