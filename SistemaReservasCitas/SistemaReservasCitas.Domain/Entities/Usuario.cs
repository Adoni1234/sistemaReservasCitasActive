using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaReservasCitas.Domain.Enums;

namespace SistemaReservasCitas.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string UsuarioNombre { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;// En producción, usa hash
        public Rol Rol { get; set; }
        public string Email { get; set; } = string.Empty;
        public List<Cita>? Citas { get; set; }
    }
}

