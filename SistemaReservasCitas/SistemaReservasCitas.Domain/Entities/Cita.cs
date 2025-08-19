using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SistemaReservasCitas.Domain.Entities
{
    public class Cita
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdSlot { get; set; }
        public Usuario? Usuario { get; set; }
        [JsonIgnore] public Slot? Slot { get; set; }
    }
}