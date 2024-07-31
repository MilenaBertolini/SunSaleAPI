using Application.Interface.Services;
using Domain.Entities;
using System.Text.RegularExpressions;
using System.Text;
using System.IO.Compression;
using System.IO;
using System.Globalization;

namespace Application.Implementation.Services
{
    public class WppData : IWppData
    {
        private readonly string format = "dd/MM/yyyy HH:mm";
        
        public List<DadosWpp> GetDadosWppsAsync(MemoryStream file)
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
                var qtDiasDaSemana = item.Value.Where(j => days.Contains(DateTime.ParseExact(j.Split('-')[0].Trim(), format, CultureInfo.InvariantCulture).DayOfWeek)).Count();
                var qtFds = item.Value.Where(j => !days.Contains(DateTime.ParseExact(j.Split('-')[0].Trim(), format, CultureInfo.InvariantCulture).DayOfWeek)).Count();
                var qtMsgMadrugada = item.Value.Where(j => DateTime.ParseExact(j.Split('-')[0].Trim(), format, CultureInfo.InvariantCulture).Hour >= 0 && DateTime.ParseExact(j.Split('-')[0].Trim(), format, CultureInfo.InvariantCulture).Hour <= 8).Count();
                var qtMsgUltimos30Dias = item.Value.Where(j => DateTime.ParseExact(j.Split('-')[0].Trim(), format, CultureInfo.InvariantCulture) >= DateTime.Now.AddMonths(-1)).Count();
                int qtCaracteres = item.Value.Sum(i => i.Length);
                var qtMensagens = item.Value.Count();

                List<MensagemWpp> mensagens = new List<MensagemWpp>();
                foreach (var texto in item.Value)
                {
                    var data = texto.Split('-')[0].Trim();
                    var temp = texto.Replace(data, "").Split(":");
                    var mensagem = temp.Length > 1 ? temp[1].Trim() : texto.Replace(data, "");
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
                    QtMensagensMadrugada = qtMsgMadrugada
                });
            }

            return dados;
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
