using Domain.Entities;

namespace Application.Interface.Services
{
    public interface IWppData
    {
        List<DadosWpp> GetDadosWppsAsync(MemoryStream file);
    }
}
