using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaReservasCitasApi.Application.Interfaces;

namespace SistemaReservasCitasApi.Application.Services
{
    public class AnalizadorFactory
    {
        public static IAnalizador CrearAnalizador(string tipo)
        {
            switch (tipo.ToLower())
            {
                case "estadistico":
                    return new AnalizadorEstadistico();
                case "texto":
                    return new AnalizadorTexto();
                default:
                    throw new ArgumentException("Tipo de analizador no soportado.");
            }
        }
    }

    public class AnalizadorEstadistico : IAnalizador
    {
        public string AnalizarDatos(string data) => $"Análisis estadístico de: {data}";
    }

    public class AnalizadorTexto : IAnalizador
    {
        public string AnalizarDatos(string data) => $"Análisis de texto de: {data}";
    }
}
