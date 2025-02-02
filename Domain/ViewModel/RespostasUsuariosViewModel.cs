﻿using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Domain.ViewModel
{
    public class RespostasUsuariosViewModel
    {
        [Key]
        [JsonPropertyName("Id")]
        public int Codigo { get; set; }

        [NotNull]
        public int CodigoUsuario { get; set; }

        [NotNull]
        public int CodigoResposta { get; set; }
        [NotNull]
        public int CodigoQuestao { get; set; }
        public DateTime DataResposta { get; set; }

        public UsuariosResumedViewModel Usuario { get; set;}

        public RespostasQuestoesViewModel Resposta { get; set;}
        public QuestoesViewModel Questao { get; set;}
    }
}
