using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaReservasCitas.Domain.DTOs;
using SistemaReservasCitas.Domain.Entities;

namespace SistemaReservasCitas.Application.Interfaces
{
    public interface IUsuarioService
    {
            Task<Usuario> CrearUsuarioAsync(CrearUsuarioDto usuarioDto);
            Task<Usuario> ObtenerUsuarioAsync(int id);
            Task<Usuario> ActualizarUsuarioAsync(int id, CrearUsuarioDto usuario);
            Task<bool> EliminarUsuarioAsync(int id);
            Task<IEnumerable<Usuario>> ObtenerUsuarioAllAsync();


    }
}
