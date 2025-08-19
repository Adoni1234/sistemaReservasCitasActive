using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ProyectoFinal.Models;

namespace SistemaReservasCitas.Domain.Entities
{
    public class Cita
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int idSlots { get; set; }
        public Usuario? Usuario { get; set; }
        [JsonIgnore]
        public Slot? Slots { get; set; }
    }
}
