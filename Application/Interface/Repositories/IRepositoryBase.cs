using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
