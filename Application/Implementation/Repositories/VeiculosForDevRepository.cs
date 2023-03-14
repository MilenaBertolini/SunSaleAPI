﻿using Data.Context;
using Main = Domain.Entities.VeiculosForDev;
using IRepository = Application.Interface.Repositories.IVeiculosForDevRepository;
using Microsoft.EntityFrameworkCore;

namespace Application.Implementation.Repositories
{
    public class VeiculosForDevRepository : RepositoryBase<Main>, IRepository
    {
        public VeiculosForDevRepository(DataContext dataContext) : base(dataContext)
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

        public async Task<Main> Update(Main entity)
        {
            var model = await GetByRenavam(entity.Renavam);
            if (model == null)
                return null;

            model.Ano = entity.Ano;
            model.Cor = entity.Cor;
            model.Marca = entity.Marca;
            model.Modelo = entity.Modelo;
            model.PlacaVeiculo = entity.PlacaVeiculo;

            base.Update(model);
            await base.CommitAsync();
            return model;
        }
        
        public async Task<IEnumerable<Main>> GetAllPagged(int page, int quantity)
        {
            return await base.GetAllPagedAsync(base.GetQueryable(), page, quantity);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public async Task<Main> GetByRenavam(string renavam)
        {
            var query = GetQueryable().Where(p => p.Renavam == renavam);
            return await query.SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Main>> GetRandom(int qt)
        {
            int count = _dataContext.VeiculosForDev.Count();
            int index = new Random().Next(count);
            return _dataContext.VeiculosForDev.Skip(index).Take(qt).ToList();
        }

        public Task<Main> GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
