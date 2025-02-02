﻿using Main = Domain.Entities.Avaliacao;

namespace Application.Interface.Services
{
    public interface IAvaliacaoService : IDisposable
    {
        Task<IEnumerable<Main>> GetAll();
        Task<IEnumerable<Main>> GetAllPagged(int page, int quantity, string chave, int user, string subject, string bancas, string provas, string materias, string professores);
        Task<Main> GetById(int id);
        Task<Main> Add(Main entity, int user);
        Task<Main> Update(Main entity, int user);
        Task<bool> DeleteById(int id, int user);
        Task<IEnumerable<Main>> GetByUserId(int id);
        Task<int> QuantidadeTotal();
        Task<IEnumerable<string>> GetAllProfessores();

        Task<string> CriaDocumentoAvaliacao(int codigo);

        Task<string> CriaDocumentoGabaritoAvaliacao(int codigo);
    }
}
