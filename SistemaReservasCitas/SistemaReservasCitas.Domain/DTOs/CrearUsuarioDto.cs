using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaReservasCitas.Domain.Enums;

namespace SistemaReservasCitas.Domain.DTOs
{
    public class CrearUsuarioDto
    {
        public string UsuarioNombre { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Rol Rol { get; set; }
    }
}