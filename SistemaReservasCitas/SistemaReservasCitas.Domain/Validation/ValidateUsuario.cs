using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaReservasCitas.Domain.Entities;
using SistemaReservasCitas.Domain.Enums;

namespace SistemaReservasCitas.Domain.Validation
{
    public static class UsuarioValidator
    {
        public static void ValidateUsuario(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario), "El usuario no puede ser nulo.");

            if (string.IsNullOrEmpty(usuario.UsuarioNombre))
                throw new ArgumentException("El nombre de usuario es requerido.", nameof(usuario.UsuarioNombre));

            if (string.IsNullOrEmpty(usuario.Password))
                throw new ArgumentException("La contraseña es requerida.", nameof(usuario.Password));

            if (!Enum.IsDefined(typeof(Rol), usuario.Rol))
                throw new ArgumentException("Rol inválido.", nameof(usuario.Rol));
        }
    }
}