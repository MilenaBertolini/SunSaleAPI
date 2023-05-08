﻿using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Domain.ViewModel
{
    public class ResultadosTabuadaDivertidaViewModel
    {
        [Key]
        [JsonPropertyName("id")]
        public int Codigo { get; set; }

        [NotNull]
        public string Nome { get; set; }

        public int Tempo { get; set; }

        public int NumerAcertos { get; set; }

        public DateTime DataInsercao { get; set; }

        public string Tipo { get; set; }

        public int NumeroQuestoes { get; set; }
    }
}
