﻿using Main = Domain.Entities.AnexoResposta;
using IService = Application.Interface.Services.IAnexoRespostaService;
using IRepository = Application.Interface.Repositories.IAnexoRespostaRepository;
using IRepositoryCodes = Application.Interface.Repositories.ICodigosTableRepository;

namespace Application.Implementation.Services
{
    public class AnexoRespostaService : IService
    {
        private readonly IRepository _repository;
        private readonly IRepositoryCodes _repositoryCodes;

        public AnexoRespostaService(IRepository repository, IRepositoryCodes repositoryCodes)
        {
            _repository = repository;
            _repositoryCodes = repositoryCodes;
        }

        public async Task<Main> Add(Main entity)
        {
            entity.Codigo = await _repositoryCodes.GetNextCodigo(typeof(Main).Name);

            if (entity.Codigo == -1) throw new Exception("Impossible to create a new Id");
            var result = await _repository.Add(entity);

            return result;
        }

        public Task<bool> DeleteById(int id)
        {
            return _repository.Delete(id);
        }

        public async Task<IEnumerable<Main>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<IEnumerable<Main>> GetAllPagged(int page, int quantity)
        {
            return await _repository.GetAllPagged(page, quantity);
        }

        public async Task<Main> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public Task<Main> Update(Main entity)
        {
            return _repository.Update(entity);
        }

        public void Dispose()
        {
            this._repository.Dispose();
        }
    }
}
