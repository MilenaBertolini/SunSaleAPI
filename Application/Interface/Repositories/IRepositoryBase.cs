namespace Application.Interface.Repositories
{
    public interface IRepositoryBase<Entity> : IDisposable where Entity : class
    {
        Task<Entity> GetById(int id);

        Task<Entity> Add(Entity entity);

        Task<Entity> Update(Entity entity);

        Task<bool> Delete(int id);

    }
}
