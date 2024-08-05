using Application.Interface.Repositories;
using Domain.Entities;
using Domain.Entities.Git;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Application.Implementation.Repositories
{
    public class GitApiRepository : IGitApiRepository
    {
        private readonly GitSettings _settings;

        public GitApiRepository(IOptions<GitSettings> options) 
        { 
            _settings = options.Value;
        }

        public async Task<Tuple<List<Postagem>, int>> BuscaInformacoesPessoais(int page, int quantity, int id = 0)
        {
            int total = 0;
            List < Postagem > posts = new List<Postagem>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.Token);

                var reposUrl = $"{_settings.Api}/user/repos";
                var reposResponse = await client.GetStringAsync(reposUrl);

                List<GitInfo> lista = JsonConvert.DeserializeObject<List<GitInfo>>(reposResponse);
                if (lista == null || lista.Count == 0)
                    return Tuple.Create(posts, 0);

                if(id > 0)
                    lista = lista.Where(i => i.id.Equals(id)).ToList();
                lista = lista.OrderByDescending(i => i.created_at).ToList();
                lista = lista.Where(i => !string.IsNullOrEmpty(i.description)).ToList();
                total = lista.Count()-6;
                int i = 0;
                foreach (var repo in lista)
                {
                    i++;
                    if(posts.Count == quantity || i < (page-1)*quantity)
                    {
                        continue;
                    }

                    var repoName = repo.name.ToString();
                    var readmeUrl = $"https://api.github.com/repos/{_settings.Username}/{repoName}/readme";

                    try
                    {
                        var readmeResponse = await client.GetStringAsync(readmeUrl);
                        var readmeJson = JObject.Parse(readmeResponse);
                        var readmeContent = readmeJson["content"].ToString();

                        // Decode Base64 content
                        var readmeDecoded = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(readmeContent));
                        posts.Add(new Postagem()
                        {
                            Id = repo.id,
                            Curtidas = repo.watchers,
                            Created = repo.created_at,
                            Updated = repo.updated_at,
                            Descricao = readmeDecoded,
                            Titulo = repo.name,
                            Intro = repo.description,
                            Link = repo.html_url,
                            TipoPostagem = 3
                        });
                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine($"Failed to retrieve README for {repoName}: {ex.Message}");
                    }
                }
            }

            return Tuple.Create(posts, total);
        }
    }
}
