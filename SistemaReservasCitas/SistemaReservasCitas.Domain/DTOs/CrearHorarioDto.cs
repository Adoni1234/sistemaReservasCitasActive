using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaReservasCitas.Domain.DTOs
{
    public class CrearHorarioDto
    {
        public string Inicio { get; set; } = string.Empty;
        public string Fin { get; set; } = string.Empty;
        public string NombreTurno { get; set; } = string.Empty;
    }
}