using Domain.Entities;

namespace Application.Interface.Services
{
    public interface IWppData
    {
        Task<RelatorioGrupoWpp> GetDadosWppsAsync(MemoryStream file);
        Task<RelatorioGrupoWpp> GetRelatorioByToken(string id);
    }
}
