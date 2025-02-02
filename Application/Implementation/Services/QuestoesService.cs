﻿using Main = Domain.Entities.Questoes;
using IService = Application.Interface.Services.IQuestoesService;
using IRepository = Application.Interface.Repositories.IQuestoesRepository;
using IRepositoryRespostas = Application.Interface.Repositories.IRespostasQuestoesRepository;
using IRepositoryAnexos = Application.Interface.Repositories.IAnexosQuestoesRepository;
using IRepositoryRespostasAnexos = Application.Interface.Repositories.IAnexoRespostaRepository;
using IRepositoryCodes = Application.Interface.Repositories.ICodigosTableRepository;
using IServiceAcao = Application.Interface.Services.IAcaoUsuarioService;
using Application.Model;
using Domain.Entities;
using static Data.Helper.EnumeratorsTypes;

namespace Application.Implementation.Services
{
    public class QuestoesService : IService
    {
        private readonly IRepository _repository;
        private readonly IRepositoryRespostas _repositoryRespostas;
        private readonly IRepositoryCodes _repositoryCodes;
        private readonly IRepositoryAnexos _repositoryAnexos;
        private readonly IRepositoryRespostasAnexos _repositoryRespostasAnexos;
        private readonly IServiceAcao _serviceAcao;

        public QuestoesService(IRepository repository, IRepositoryCodes repositoryCodes, IRepositoryRespostas repositoryRespostas, IRepositoryAnexos repositoryAnexos, IRepositoryRespostasAnexos repositoryRespostasAnexos, IServiceAcao serviceAcao)
        {
            _repository = repository;
            _repositoryCodes = repositoryCodes;
            _repositoryRespostas = repositoryRespostas;
            _repositoryAnexos = repositoryAnexos;
            _repositoryRespostasAnexos = repositoryRespostasAnexos;
            _serviceAcao = serviceAcao;
        }

        public async Task<Main> Add(Main entity, int user)
        {
            entity.Codigo = await _repositoryCodes.GetNextCodigo(typeof(Main).Name);
            entity.DataRegistro = DateTime.Now;
            entity.CreatedBy = user;
            entity.UpdatedOn = DateTime.Now;
            entity.UpdatedBy = user;

            List<RespostasQuestoes> respostasQuestoes = new List<RespostasQuestoes>();
            foreach (var r in entity.RespostasQuestoes)
            {
                r.Codigo = await _repositoryCodes.GetNextCodigo(typeof(RespostasQuestoes).Name);
                r.DataRegistro = DateTime.Now;
                r.CodigoQuestao = entity.Codigo;
                r.ObservacaoResposta = string.Empty;

                if (string.IsNullOrEmpty(r.Certa))
                    r.Certa = "0";

                List<AnexoResposta> anexosRespostas = new List<AnexoResposta>();
                bool haAnexo = false;
                foreach(var a in r.AnexoResposta)
                {
                    if(a.Anexo.Length > 0)
                    {
                        a.Codigo = await _repositoryCodes.GetNextCodigo(typeof(AnexoResposta).Name);
                        a.DataRegistro = DateTime.Now;
                        a.CodigoQuestao = r.Codigo;

                        anexosRespostas.Add(a);
                        haAnexo = true;
                    }
                }

                r.AnexoResposta.Clear();
                anexosRespostas.ForEach(a => r.AnexoResposta.Add(a));
                
                if(!string.IsNullOrEmpty(r.TextoResposta) || haAnexo)
                {
                    respostasQuestoes.Add(r);
                }
            }

            entity.RespostasQuestoes.Clear();
            respostasQuestoes.ForEach(r => entity.RespostasQuestoes.Add(r));

            List<AnexosQuestoes> anexos = new List<AnexosQuestoes>();
            foreach(var r in entity.AnexosQuestoes)
            { 
                if(r.Anexo.Length > 0)
                {
                    r.Codigo = await _repositoryCodes.GetNextCodigo(typeof(AnexosQuestoes).Name);
                    r.CodigoQuestao = entity.Codigo;
                    r.DataRegistro = DateTime.Now;

                    anexos.Add(r);
                }
            }

            entity.AnexosQuestoes.Clear();
            anexos.ForEach(a => entity.AnexosQuestoes.Add(a));

            try
            {
                var result = await _repository.Add(entity);

                return result;
            }
            catch(Exception ex) 
            {
                string temp = ex.Message;
            }

            return null;
        }

        public Task<bool> DeleteById(int id)
        {
            return _repository.Delete(id);
        }

        public async Task<IEnumerable<Main>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<Tuple<IEnumerable<Main>, int>> GetAllPagged(int page, int quantity, int user, bool includeAnexos, string subject, string bancas, string provas, string materias, string tipos, int? codigoProva, TipoQuestoes? tipo, bool? randon, int? id, int? avaliacao)
        {
            if (id.HasValue)
            {
                List<Main> response = new List<Main>() { await GetById(id.Value) };
                return Tuple.Create((IEnumerable<Main>)response, response.Count);
            }
            else if (avaliacao.HasValue)
            {
                List<Main> response = new List<Main>() { await _repository.GetQuestoesByAvaliacao(avaliacao.Value, page) };
                return Tuple.Create((IEnumerable<Main>)response, response.Count);
            }
            else
            {
                return await _repository.GetAllPagged(page, quantity, user, includeAnexos, subject, bancas, provas, materias, tipos, codigoProva, tipo, randon);
            }
        }

        public async Task<Main> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task<Main> Update(Main entity, int user)
        {
            entity.UpdatedOn = DateTime.Now;
            entity.UpdatedBy = user;

            if(entity.AnexosQuestoes.Count > 0)
            {
                var listAnexos = await _repositoryAnexos.GetByQuestaoId(entity.Codigo);

                foreach (var item in listAnexos)
                {
                    await _repositoryAnexos.Delete(item.Codigo);
                }
            }

            var respostas = await _repositoryRespostas.GetByCodigoQuestao(entity.Codigo);

            foreach (var r in respostas)
            {
                await _repositoryRespostas.Delete(r.Codigo);
            }

            List<RespostasQuestoes> respostasQuestoes = new List<RespostasQuestoes>();
            foreach (var r in entity.RespostasQuestoes)
            {
                if (r.AnexoResposta.Count > 0)
                {
                    var listAnexos = await _repositoryRespostasAnexos.GetByQuestaoId(r.Codigo);

                    foreach (var item in listAnexos)
                    {
                        await _repositoryAnexos.Delete(item.Codigo);
                    }
                }

                r.Codigo = await _repositoryCodes.GetNextCodigo(typeof(RespostasQuestoes).Name);
                r.DataRegistro = DateTime.Now;
                r.CodigoQuestao = entity.Codigo;
                r.ObservacaoResposta = string.Empty;

                List<AnexoResposta> anexosRespostas = new List<AnexoResposta>();
                bool haAnexo = false;
                foreach (var a in r.AnexoResposta)
                {
                    if (a.Anexo.Length > 0)
                    {
                        a.Codigo = await _repositoryCodes.GetNextCodigo(typeof(AnexoResposta).Name);
                        a.DataRegistro = DateTime.Now;
                        a.CodigoQuestao = r.Codigo;

                        anexosRespostas.Add(a);
                        await _repositoryRespostasAnexos.Add(a);
                        haAnexo = true;
                    }
                }

                r.AnexoResposta.Clear();
                anexosRespostas.ForEach(a => r.AnexoResposta.Add(a));

                if (!string.IsNullOrEmpty(r.TextoResposta) || haAnexo)
                {
                    respostasQuestoes.Add(r);
                    await _repositoryRespostas.Add(r);
                }
            }

            entity.RespostasQuestoes.Clear();
            respostasQuestoes.ForEach(r => entity.RespostasQuestoes.Add(r));

            List<AnexosQuestoes> anexos = new List<AnexosQuestoes>();
            foreach (var r in entity.AnexosQuestoes)
            {
                if (r.Anexo.Length > 0)
                {
                    r.Codigo = await _repositoryCodes.GetNextCodigo(typeof(AnexosQuestoes).Name);
                    r.CodigoQuestao = entity.Codigo;
                    r.DataRegistro = DateTime.Now;

                    await _repositoryAnexos.Add(r);
                    anexos.Add(r);
                }
            }

            entity.AnexosQuestoes.Clear();
            anexos.ForEach(a => entity.AnexosQuestoes.Add(a));

            var result = await _repository.Update(entity);

            await _serviceAcao.Add(new AcaoUsuario()
            {
                Acao = $"Atualizou a questão {entity.Codigo}",
                CodigoUsuario = user
            });


            return result;
        }

        public async Task<Main> UpdateAtivo(int id, bool ativo, int user)
        {
            return await _repository.UpdateAtivo(id, ativo, user);
        }

        public Task<IEnumerable<string>> GetMaterias(int? prova)
        {
            return _repository.GetMaterias(prova == null ? -1 : prova.Value);
        }

        public Task<IEnumerable<Test>> GetTests(int? id)
        {
            return _repository.GetTests(id == null ? -1 : id.Value);   
        }

        public async Task<Main> GetQuestoesByProva(int prova, int numero)
        {
            var response = await _repository.GetByProva(prova, numero);
            return response;
        }

        public async Task<Main> GetLastByProva(int prova)
        {
            var response = await _repository.GetLastByProva(prova);
            return response;
        }

        public async Task<IEnumerable<Main>> GetQuestoesByProva(int prova)
        {
            var response = await _repository.GetByProva(prova);
            return response;
        }

        public async Task<IEnumerable<Main>> GetQuestoesByMateria(string materia)
        {
            var response = await _repository.GetByMateria(materia);
            return response;
        }

        public async Task<int> QuantidadeQuestoes(int prova, int user = -1)
        {
            return await _repository.QuantidadeQuestoes(prova, user);
        }

        public async Task<IEnumerable<string>> GetAllMateris()
        {
            return await _repository.GetAllMateris();
        }

        public async Task<Main> GetQuestoesAleatoria(TipoQuestoes tipo, string? subject, string? banca)
        {
            return await _repository.GetQuestoesAleatoria(tipo, subject, banca);
        }

        public async Task<IEnumerable<Main>> GetQuestoesRespondidas(int usuario)
        {
            return await _repository.GetQuestoesRespondidas(usuario);
        }

        public async Task<Main> GetQuestoesByAvaliacao(int codigoAvaliacao, int? numeroQuestao)
        {
            return await _repository.GetQuestoesByAvaliacao(codigoAvaliacao, numeroQuestao.HasValue ? numeroQuestao.Value : 0);
        }

        public async Task<Main> UpdateAssunto(int id, string assunto, int user)
        {
            return await _repository.UpdateAssunto(id, assunto, user);
        }

        public void Dispose()
        {
            this._repository.Dispose();
        }

    }
}
