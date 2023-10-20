using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Responses
{
    public class HistoricoUsuario
    {
        public int Codigo { get; set; }
        public string RespostaCorreta { get; set; }
        public int NumeroQuestao { get; set; }
        public string NomeProva { get; set; }
        public DateTime DataResposta { get; set; }
        public int CodigoQuestao { get; set; }
    }
}
