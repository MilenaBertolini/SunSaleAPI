using Application.Interface.Services;
using Domain.Entities;
using Newtonsoft.Json;
using System.Globalization;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;

namespace Application.Implementation.Services
{
    public class WppData : IWppData
    {
        private readonly string format = "dd/MM/yyyy HH:mm";
        private readonly List<string> palavrasRacistas = new List<string>() { "A coisa tá preta", "Cabelo ruim", "cabelo bombril", "cabelo duro", "Cor de pele", "Crioulo ou crioula", "Da cor do pecado", "Denegrir", "Dia de branco", "Disputar a negra", "Esclarecer", "Escravo e escrava", "Estampa étnica", "Humor negro", "Inhaca", "Inveja branca", "Lista negra", "Magia negra", "Mercado negro", "Mulata ou mulato", "Mulata tipo exportação", "Não sou tuas negas!", "Nasceu com um pé na cozinha", "Nega maluca", "Negra com traços finos", "Negra de beleza exótica", "Negro de alma branca | Preto de alma branca", "Ovelha negra", "Quando não está preso está armado", "Samba do crioulo doido", "Serviço de preto", "Teta de nega", "Nego", "Negro", "Macaco", "Mamaco", "Pixe", "Preto", "suco de pneu", "memory card", "avatar defumado", "sombra 3D", "charuto de macumba", "capa de biblia", "combustivel de churrasqueira", "ze gotinha da Petrobras", "mico Leão queimado", "sabonete de mecânico", "testiculo de africano", "mumia de fita isolante", "metade de zebra", "oreo sem recheio", "enderman do minecraft", "inimigo da luz", "amigo da escuridão", "black out", "meianoite", "eclipse", "pedaço de coco de vaca", "tapioca de luto", "pretoshina e negrozaki", "parachoque da rotam", "personagem não desbloqueado", "adaptação da netflix", "ausente no arcoíris", "prêmio de PM", "guardanapo de mecânico", "lanterna queimada", "cotonete de escape", "papai noel da angola", "o preto de barro e os 7 carvões", "picolé de asfalto", "lata de macumba", "materia escura", "50 tons de preto", "corretivo de petróleo", "cirilo", "pelé", "unha de mendigo", "olaf de barro", "judeu cremado", "batizado na fogueira de são joão", "escondidinho de graxa", "raio x sem osso", "sofá de couro", "super choque", "madrugada ambulante", "gênio do pote de café" };
        private readonly List<string> palavrasHomofobicas = new List<string>() { "viado", "gay", "dar a bund", "viadinho", "baitola", "sapatão", "caminhoneira", "bixa", "viada" };
        private readonly List<string> palavrasGordofobicas = new List<string>() { "Gordo", "Balei", "Taís Carla", "Bola", "Redondo", "Dominic Torresmo", "Duque de Coxinhas", "Zumbi dos Jantares", "Estátua da Obesidade", "Bem Frito de Paula", "Amanteigado Batista", "Back Estria Boys", "Capitão Enchimento", "Dom Peso I", "Garfo Galático", "Fafá de Acém", "Mc Ronald Golias", "Balão da Pisadinha", "Lasanha Manoela", "Monteiro Carboidrato", "Elisa Lanches", "Chitãozinho Mocotó", "Poderoso Pratão", "Fábio Jumbo", "Leitão Jobim", "Padre Infarto de Melo", "Grande Bujão Branco", "Martin Burger King"};
        private readonly ISavedResultsWppService _savedResultsWppService;

        public WppData(ISavedResultsWppService savedResultsWppService)
        {
            _savedResultsWppService = savedResultsWppService;
        }

        public async Task<RelatorioGrupoWpp> GetDadosWppsAsync(MemoryStream file)
        {
            List<DadosWpp> dados = new List<DadosWpp>();
            var lines = ExtractTxtFilesFromZip(file)[0].Split('\n');
            Dictionary<string, List<string>> map = new Dictionary<string, List<string>>();
            int i = 0;
            var finalLines = new List<string>();
            foreach (var line in lines)
            {
                string pattern = @"^\d{2}/\d{2}/\d{4} \d{2}:\d{2} - [A-Za-z\s]+:";
                if (!Regex.IsMatch(line, pattern))
                {
                    finalLines[i - 1] += line;
                }
                else
                {
                    finalLines.Add(line);
                    i++;
                }
            }
            foreach (var line in finalLines)
            {
                if (string.IsNullOrEmpty(line))
                    continue;
                string nome = GetName(line);
                if (string.IsNullOrEmpty(nome))
                    continue;
                if (map.ContainsKey(nome))
                {
                    map[nome].Add(line);
                }
                else
                {
                    map.Add(nome, new List<string> { line });
                }
            }

            StringBuilder comercial = new StringBuilder();
            comercial.AppendLine("Nome;Quantdade 8-18;Quantidade dias de semana 8-18; Quantidade fds;Quantidade 0-8;Quantidade últimos 30 dias");
            foreach (var item in map)
            {
                var days = new List<DayOfWeek>() { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
                var qtHorarioComercial = item.Value.Where(j => DateTime.ParseExact(j.Split('-')[0].Trim(), format, CultureInfo.InvariantCulture).Hour >= 8 && DateTime.ParseExact(j.Split('-')[0].Trim(), format, CultureInfo.InvariantCulture).Hour <= 18).Count();
                var qtTestemunhaGeova = item.Value.Where(j => DateTime.ParseExact(j.Split('-')[0].Trim(), format, CultureInfo.InvariantCulture).Hour >= 5 && DateTime.ParseExact(j.Split('-')[0].Trim(), format, CultureInfo.InvariantCulture).Hour <= 8).Count();
                var qtDiasDaSemana = item.Value.Where(j => days.Contains(DateTime.ParseExact(j.Split('-')[0].Trim(), format, CultureInfo.InvariantCulture).DayOfWeek)).Count();
                var qtFds = item.Value.Where(j => !days.Contains(DateTime.ParseExact(j.Split('-')[0].Trim(), format, CultureInfo.InvariantCulture).DayOfWeek)).Count();
                var qtMsgMadrugada = item.Value.Where(j => DateTime.ParseExact(j.Split('-')[0].Trim(), format, CultureInfo.InvariantCulture).Hour >= 0 && DateTime.ParseExact(j.Split('-')[0].Trim(), format, CultureInfo.InvariantCulture).Hour <= 8).Count();
                var qtMsgUltimos30Dias = item.Value.Where(j => DateTime.ParseExact(j.Split('-')[0].Trim(), format, CultureInfo.InvariantCulture) >= DateTime.Now.AddMonths(-1)).Count();
                int qtCaracteres = item.Value.Sum(i => i.Length);
                var qtMensagens = item.Value.Count();

                List<MensagemWpp> mensagens = new List<MensagemWpp>();
                int qtHomofobico = 0;
                int qtRacista = 0;
                int qtGordofobico = 0;
                foreach (var texto in item.Value)
                {
                    var data = texto.Split('-')[0].Trim();
                    var temp = texto.Replace(data, "").Split(":");
                    var mensagem = temp.Length > 1 ? temp[1].Trim() : texto.Replace(data, "");

                    palavrasRacistas.ForEach(p => {
                        if (mensagem.ToUpper().Contains(p.ToUpper()))
                        {
                            qtRacista++;
                        }
                    });

                    palavrasHomofobicas.ForEach(p => {
                        if (mensagem.ToUpper().Contains(p.ToUpper()))
                        {
                            qtHomofobico++;
                        }
                    });

                    palavrasGordofobicas.ForEach(p => {
                        if (mensagem.ToUpper().Contains(p.ToUpper()))
                        {
                            qtGordofobico++;
                        }
                    });

                    mensagens.Add(new MensagemWpp()
                    {
                        Date = DateTime.ParseExact(data, format, CultureInfo.InvariantCulture),
                        Mensagem = mensagem,
                    });
                }

                dados.Add(new DadosWpp()
                {
                    //Mensagens = mensagens,
                    Nome = item.Key,
                    QtCaracteres = qtCaracteres,
                    QtMensagens = qtMensagens,
                    QtMensagensDuranteSemana = qtDiasDaSemana,
                    QtMensagensFds = qtFds,
                    QtMensagensHorarioComercial = qtHorarioComercial,
                    QtUltimos30Dias = qtMsgUltimos30Dias,
                    QtMensagensMadrugada = qtMsgMadrugada,
                    QtMsgTestemunhaGeova = qtTestemunhaGeova,
                    QtMsgGordofobicas = qtGordofobico,
                    QtMsgHomofobica = qtHomofobico,
                    QtMsgRacistas = qtRacista
                });
            }

            RelatorioGrupoWpp relatorio = new RelatorioGrupoWpp();
            relatorio.Dados = dados.OrderByDescending(d => d.QtMensagens).ToList();
            relatorio.RankingMensagens = dados.OrderByDescending(d => d.QtMensagens).Select(t => t.Nome).ToList();
            relatorio.RankingCaracteres = dados.OrderByDescending(d => d.QtCaracteres).Select(t => t.Nome).ToList();
            relatorio.RankingMadrugada = dados.OrderByDescending(d => d.QtMensagensMadrugada).Select(t => t.Nome).ToList();
            relatorio.RankingFds = dados.OrderByDescending(d => d.QtMensagensFds).Select(t => t.Nome).ToList();
            relatorio.RankingDuranteSemana = dados.OrderByDescending(d => d.QtMensagensDuranteSemana).Select(t => t.Nome).ToList();
            relatorio.RankingHorarioComercial = dados.OrderByDescending(d => d.QtMensagensHorarioComercial).Select(t => t.Nome).ToList();
            relatorio.RankingUltimos30Dias = dados.OrderByDescending(d => d.QtUltimos30Dias).Select(t => t.Nome).ToList();
            relatorio.RankingTestemunhaGeova = dados.OrderByDescending(d => d.QtMsgTestemunhaGeova).Select(t => t.Nome).ToList();
            relatorio.RankingRacista = dados.Where(d => d.QtMsgRacistas > 0).OrderByDescending(d => d.QtMsgRacistas).Select(t => t.Nome).ToList();
            relatorio.RankingHomofobico = dados.Where(d => d.QtMsgHomofobica > 0).OrderByDescending(d => d.QtMsgHomofobica).Select(t => t.Nome).ToList();
            relatorio.RankingGordofobico = dados.Where(d => d.QtMsgGordofobicas > 0).OrderByDescending(d => d.QtMsgGordofobicas).Select(t => t.Nome).ToList();

            relatorio.TopMensagens = relatorio.RankingMensagens.First();
            relatorio.TopCaracteres = relatorio.RankingCaracteres.First();
            relatorio.TopMadrugada = relatorio.RankingMadrugada.First();
            relatorio.TopFds = relatorio.RankingFds.First();
            relatorio.TopDuranteSemana = relatorio.RankingDuranteSemana.First();
            relatorio.TopHorarioComercial = relatorio.RankingHorarioComercial.First();
            relatorio.TopUltimos30Dias = relatorio.RankingUltimos30Dias.First();
            relatorio.TopTestemunhaGeova = relatorio.RankingTestemunhaGeova.First();
            relatorio.TopRacista = relatorio.RankingRacista.First();
            relatorio.TopHomofobico = relatorio.RankingHomofobico.First();
            relatorio.TopGordofobico = relatorio.RankingGordofobico.First();
            relatorio.MenosMensagens = relatorio.RankingMensagens.Last();

            SavedResultsWpp savedResult = new SavedResultsWpp()
            {
                Json = JsonConvert.SerializeObject(relatorio)
            };
            var t = await _savedResultsWppService.Add(savedResult);
            relatorio.Id = t.Id;

            return relatorio;
        }

        public async Task<RelatorioGrupoWpp> GetRelatorioById(int id)
        {
            var item = await _savedResultsWppService.GetById(id);

            return JsonConvert.DeserializeObject<RelatorioGrupoWpp>(item.Json);
        }

        private string GetName(string linha)
        {
            string nome = linha.Split('-')[1].Trim();
            nome = nome.Split(":")[0].Trim().Split(' ')[0].Trim();

            if (string.IsNullOrEmpty(nome))
            {
                return string.Empty;
            }

            if (nome.ToString().ToUpper()[0] < 'A' || nome.ToString().ToUpper()[0] > 'Z') nome = nome.Substring(1);

            if (nome.Equals("Você:")) return string.Empty;
            return nome;
        }

        private List<string> ExtractTxtFilesFromZip(MemoryStream memoryStream)
        {
            List<string> textFileContents = new List<string>();

            // Open the zip file for reading
            using (ZipArchive archive = new ZipArchive(memoryStream))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    // Check if the entry is a .txt file
                    if (Path.GetExtension(entry.FullName).Equals(".txt", StringComparison.OrdinalIgnoreCase))
                    {
                        // Read the .txt file into a string variable
                        using (StreamReader reader = new StreamReader(entry.Open()))
                        {
                            string content = reader.ReadToEnd();
                            textFileContents.Add(content);
                        }
                    }
                }
            }

            return textFileContents;
        }
    }
}
