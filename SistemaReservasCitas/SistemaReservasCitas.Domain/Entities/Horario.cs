using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaReservasCitas.Domain.Entities
{
    
        public class Horario
        {
            public int Id { get; set; }
            public TimeSpan Inicio { get; set; }
            public TimeSpan Fin { get; set; }
            public string NombreTurno { get; set; } = string.Empty;
            public List<Turno>? Turnos { get; set; }
        }
}
