using Data.Context;
using Data.Helper;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using Domain.Entities;

namespace Application.Implementation.Repositories
{
    public class RepositoryBase<TEntity> : IDisposable where TEntity : class
    {
        protected readonly DataContext _dataContext;

        public RepositoryBase(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void DetachAll()
        {
            foreach (EntityEntry dbEntityEntry in _dataContext.ChangeTracker.Entries())
            {
                if (dbEntityEntry.Entity != null)
                {
                    dbEntityEntry.State = EntityState.Detached;
                }
            }
        }

        public void DetachAll(string[] entities)
        {
            _dataContext.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e =>
            {
                Type entityType = e.Entity.GetType();

                if (entityType.BaseType != null && entityType.Namespace == "System.Data.Entity.DynamicProxies")
                    entityType = entityType.BaseType;

                string entityTypeName = entityType.Name;

                if (entities.Contains(entityTypeName))
                {
                    e.State = EntityState.Detached;
                }
            });
        }

        public void DetachAll(string entity)
        {
            DetachAll(new string[] { entity });
        }

        public Type FindType(string qualifiedTypeName)
        {
            Type t = Type.GetType(qualifiedTypeName);

            if (t != null)
            {
                return t;
            }
            else
            {
                foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    t = asm.GetType(qualifiedTypeName);
                    if (t != null)
                        return t;
                }
                return null;
            }
        }

        public IQueryable<TEntity> GetQueryable()
        {
            return _dataContext.Set<TEntity>().AsQueryable();
        }

        public async Task<TEntity> GetByIdAsync(int codigo)
        {
            return await _dataContext.Set<TEntity>().FindAsync(codigo);
        }

        public async Task<TEntity> GetByIdAsync(string code)
        {
            return await _dataContext.Set<TEntity>().FindAsync(code);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null, string[] includes = null, string orderBy = null)
        {
            var query = _dataContext.Set<TEntity>().AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            return await GetAllAsync(query, includes, orderBy);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(IQueryable<TEntity> query, string[] includes = null, string orderBy = null)
        {
            includes?.ToList().ForEach(item => query = query.Include(item));

            if (!string.IsNullOrEmpty(orderBy))
            {
                var arr = orderBy.Split(':');
                query = query.OrderBy(arr[0], arr[1]);
            }

            return await query.ToListAsync();

        }

        public async Task<int> GetAllPagedTotalAsync(IQueryable<TEntity> query, string[] includes = null)
        {
            includes?.ToList().ForEach(item => query = query.Include(item));
            return await query.CountAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllPagedAsync(IQueryable<TEntity> query, int page, int quantity, string[] includes = null, string orderBy = null)
        {
            includes?.ToList().ForEach(item => query = query.Include(item));

            orderBy = orderBy ?? "Created:Asc";

            var fullName = query.GetType().GenericTypeArguments.Select(c => c.FullName).FirstOrDefault();

            string fieldOrderBy = null;

            if (!string.IsNullOrEmpty(orderBy))
            {
                var arrCheck = orderBy.Split(':');
                fieldOrderBy = arrCheck[0];
            }
            else if (!string.IsNullOrEmpty(fullName))
            {
                Type objectType = FindType(fullName);

                if (objectType != null)
                {
                    var property = objectType.GetProperty(fieldOrderBy);

                    if (property == null)
                    {
                        var firstProperty = objectType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance).Where(p => p.PropertyType.IsPublic).FirstOrDefault();

                        if (firstProperty != null)
                        {
                            orderBy = $"{firstProperty.Name}:Asc";
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                var arr = orderBy.Split(':');
                query = query.OrderBy(arr[0], arr[1]);
            }

            return await query.ToListPagedAsync(page, quantity);
        }

        public void Add(TEntity model)
        {
            _dataContext.Set<TEntity>().Add(model);
        }

        public void Update(TEntity model)
        {
            _dataContext.Entry(model).State = EntityState.Modified;

            //TestUpdate(model);
        }

        /*
        private void TestUpdate(object item)
        {
            var props = item.GetType().GetProperties();
            foreach (var prop in props)
            {
                object value = prop.GetValue(item);
                if (prop.PropertyType.IsInterface && value != null)
                {
                    foreach (var iItem in (System.Collections.IEnumerable)value)
                    {
                        TestUpdate(iItem);
                    }
                }
            }

            _dataContext.Entry(item).State = System.Data.Entity.EntityState.Modified;

            /*
            int codigo = (int)item.GetType().GetProperty("Code").GetValue(item);
            if (codigo == 0)
            {
                _dataContext.Entry(item).State = System.Data.Entity.EntityState.Added;
            }
            else
            {
                _dataContext.Entry(item).State = System.Data.Entity.EntityState.Modified;
            }

        }
        */

        public void Merge(TEntity persisted, TEntity current)
        {
            try
            {
                _dataContext.Entry(persisted).CurrentValues.SetValues(current);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /*
        public void Merge(TEntity persisted, TEntity current)
        {
            var props = persisted.GetType().GetProperties();

            foreach (var prop in props)
            {
                object value = prop.GetValue(persisted);
                if (prop.PropertyType.IsInterface && value != null)
                {
                    foreach (var iItem in (System.Collections.IEnumerable)value)
                    {
                        Merge(iItem, value);
                    }
                }
            }

            _dataContext.Entry(persisted).CurrentValues.SetValues(current);
        }
        */

        public void Merge<T>(T persisted, T current) where T : class
        {
            _dataContext.Entry(persisted).CurrentValues.SetValues(current);
        }


        /*
        public void Merge<T>(T persisted, T current) where T : class
        {
            var props = persisted.GetType().GetProperties();

            foreach (var prop in props)
            {
                object value = prop.GetValue(persisted);
                if (prop.PropertyType.IsInterface && value != null)
                {
                    foreach (var iItem in (System.Collections.IEnumerable)value)
                    {
                        Merge(iItem, value);
                    }
                }
            }

            _dataContext.Entry(persisted).CurrentValues.SetValues(current);
        }
        */

        public void Remove(TEntity model)
        {
            _dataContext.Entry(model).State = EntityState.Deleted;
            _dataContext.Set<TEntity>().Remove(model);
        }

        public void RemoveRange(List<TEntity> models)
        {
            foreach (var item in models)
            {
                _dataContext.Entry(item).State = EntityState.Deleted;
            }
            _dataContext.Set<TEntity>().RemoveRange(models);
        }

        public void Commit()
        {
            _dataContext.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _dataContext.SaveChangesAsync();
        }

        private bool _disposed = false;

        public string[] GetIncludes(string include)
        {
            List<string> includes = new List<string>();

            if(!string.IsNullOrEmpty(include))
            {
                includes.AddRange(include.Split(';'));
            }

            return includes.ToArray<string>();
        }

        public async Task<IEnumerable<StringPlusInt>> BuscaConsultaDescricaoValor(string query)
        {
            var response = await _dataContext.Database.SqlQueryRaw<StringPlusInt>(query).ToListAsync();

            return response;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _dataContext.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
