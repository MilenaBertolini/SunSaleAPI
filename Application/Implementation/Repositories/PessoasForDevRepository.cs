using Data.Context;
using Main = Domain.Entities.PessoasForDev;
using IRepository = Application.Interface.Repositories.IPessoasForDevRepository;
using Microsoft.EntityFrameworkCore;

namespace Application.Implementation.Repositories
{
    public class PessoasForDevRepository : RepositoryBase<Main>, IRepository
    {
        public PessoasForDevRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public async Task<Main> Add(Main entity)
        {
            base.Add(entity);
            await base.CommitAsync();
            return entity;
        }

        public async Task<bool> Delete(int id)
        {
            var model = await base.GetByIdAsync(id);
            if(model == null)
                return false;

            base.Remove(model);
            await base.CommitAsync();
            return true;
        }

        public async Task<IEnumerable<Main>> GetAll()
        {
            return await GetAllAsync();
        }

        public async Task<Main> GetByCpf(string cpf)
        {
            var query = GetQueryable().Where(p => p.CPF == cpf);
            return await query.SingleOrDefaultAsync();
        }

        public async Task<Main> Update(Main entity)
        {
            var model = await GetByCpf(entity.CPF);
            if (model == null)
                return null;

            model.Altura = entity.Altura;
            model.Bairro = entity.Bairro;
            model.Celular = entity.Celular;
            model.Cep = entity.Cep;
            model.Cidade= entity.Cidade;
            model.CorFavorita = entity.CorFavorita;
            model.DataNascimento = entity.DataNascimento;
            model.Email = entity.Email;
            model.Endereco = entity.Email;
            model.Estado = entity.Estado;
            model.Idade = entity.Idade;
            model.Mae = entity.Mae;
            model.Nome = entity.Nome;
            model.Numero = entity.Numero;

            base.Update(model);
            await base.CommitAsync();
            return model;
        }
        
        public async Task<IEnumerable<Main>> GetAllPagged(int page, int quantity)
        {
            return await base.GetAllPagedAsync(base.GetQueryable(), page, quantity);
        }

        public async Task<IEnumerable<Main>> GetRandom(int qt)
        {
            int count = _dataContext.PessoasForDev.Count();
            int index = new Random().Next(count);
            return _dataContext.PessoasForDev.Skip(index).Take(qt).ToList();
        }

        public Task<Main> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            this.Dispose(true);
        }
    }
}
