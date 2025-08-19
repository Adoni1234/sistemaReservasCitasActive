using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaReservasCitas.Domain.Entities
{
    public class Turno
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int IdHorario { get; set; }
        public int EstacionesCantidad { get; set; }
        public int TiempoCita { get; set; }
        public string Estado { get; set; } = string.Empty;
        public Horario? Horario { get; set; }
        
        public List<Slot>? Slots { get; set; }
    }
}

