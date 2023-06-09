using Main = Domain.Entities.DadosEstagiario;

namespace Application.Interface.Services
{
    public interface IEstagioService : IDisposable
    {
        string CriaDocumento(Main input);
    }
}
