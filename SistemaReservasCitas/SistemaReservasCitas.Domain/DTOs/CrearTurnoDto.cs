using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaReservasCitas.Domain.DTOs
{
    public class CrearTurnoDto
    {
        public DateTime Fecha { get; set; }
        public int IdHorario { get; set; }
        public int EstacionesCantidad { get; set; }
        public int TiempoCita { get; set; }
    }
}