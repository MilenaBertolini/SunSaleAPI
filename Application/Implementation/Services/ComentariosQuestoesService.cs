﻿using Main = Domain.Entities.ComentariosQuestoes;
using IService = Application.Interface.Services.IComentariosQuestoesService;
using IRepository = Application.Interface.Repositories.IComentariosQuestoesRepository;
using IRepositoryCodes = Application.Interface.Repositories.ICodigosTableRepository;
using Application.Model;

namespace Application.Implementation.Services
{
    public class ComentariosQuestoesService : IService
    {
        private readonly IRepository _repository;
        private readonly IRepositoryCodes _repositoryCodes;
        public ComentariosQuestoesService(IRepository repository, IRepositoryCodes repositoryCodes)
        {
            _repository = repository;
            _repositoryCodes = repositoryCodes;
        }

        public async Task<Main> Add(Main entity)
        {
            entity.Codigo = await _repositoryCodes.GetNextCodigo(typeof(Main).Name);
            entity.Comentario = entity.Comentario.Replace(Environment.NewLine, "<br/>");
            entity.Created = DateTime.Now;
            entity.Updated = DateTime.Now;

            if (entity.Codigo == -1) throw new Exception("Impossible to create a new Id");

            return await _repository.Add(entity);
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

        public async Task<IEnumerable<ComentariosViewModel>> GetByQuestao(int questao, int user)
        {
            return await _repository.GetByQuestao(questao, user);
        }

        public async Task<Main> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public Task<Main> Update(Main entity)
        {
            entity.Updated = DateTime.Now;
            return _repository.Update(entity);
        }

        public void Dispose()
        {
            this._repository.Dispose();
        }
    }
}
