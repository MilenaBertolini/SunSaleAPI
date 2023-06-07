using Main = Domain.Entities.Imagem;

namespace Application.Interface.Services
{
    public interface IImageMagicService : IDisposable
    {
        Task<Main> TreatAsync(Main input);
    }
}
