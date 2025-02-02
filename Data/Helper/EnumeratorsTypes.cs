﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Helper
{
    public static class EnumeratorsTypes
    {
        public enum TipoArquivo 
        { 
            PROVA = 0,
            GABARITO = 1
        }

        public enum TipoQuestoes 
        { 
            GENERIC = 0,
            ENEM = 1,
            IFTM = 2
        }

        public enum TipoLog
        {
            INFO = 0,
            WARNING = 1,
            ERROR = 2,
        }

        public enum TipoSistema
        {
            QuestoesAqui = 0,
            CrudForms = 1
        }

        public enum TipoPostagem
        {
            Featured_Projects = 0,
            Articles = 1,
            Psico = 2
        }
    }
}
