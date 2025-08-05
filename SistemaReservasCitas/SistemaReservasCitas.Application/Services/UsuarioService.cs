using SistemaReservasCitas.Application.Interfaces;
using SistemaReservasCitas.Domain.DTOs;
using SistemaReservasCitas.Domain.Entities;
using SistemaReservasCitas.Domain.Repositories;
using SistemaReservasCitas.Domain.Validation;
using System;
using System.Threading.Tasks;

namespace SistemaReservasCitas.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IRepository<Usuario> _usuarioRepository;

        public UsuarioService(IRepository<Usuario> usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Usuario> CrearUsuarioAsync(CrearUsuarioDto usuarioDto)
        {
            if (usuarioDto == null || string.IsNullOrEmpty(usuarioDto.UsuarioNombre) || string.IsNullOrEmpty(usuarioDto.Password))
                throw new ArgumentException("Datos inválidos.");

            var usuario = new Usuario
            {
                UsuarioNombre = usuarioDto.UsuarioNombre,
                Password = usuarioDto.Password,
                Rol = usuarioDto.Rol
            };

            UsuarioValidator.ValidateUsuario(usuario);
            await _usuarioRepository.AddAsync(usuario);
            return usuario;
        }

        public async Task<Usuario> ObtenerUsuarioAsync(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null)
                throw new KeyNotFoundException("Usuario no encontrado.");
            return usuario;
        }

        public async Task<Usuario> ActualizarUsuarioAsync(int id, CrearUsuarioDto usuarioDto)
        {
            if (usuarioDto == null)
                throw new ArgumentException("Datos inválidos.");

            var existingUser = await _usuarioRepository.GetByIdAsync(id);
            if (existingUser == null)
                throw new KeyNotFoundException("Usuario no encontrado.");

            // Mapea solo las propiedades del DTO
            existingUser.UsuarioNombre = usuarioDto.UsuarioNombre;
            existingUser.Password = usuarioDto.Password; // En producción, re-hash
            if (usuarioDto.Rol != 0) // Evita sobrescribir si no se proporciona
                existingUser.Rol = usuarioDto.Rol;

            UsuarioValidator.ValidateUsuario(existingUser);
            await _usuarioRepository.UpdateAsync(existingUser);
            return existingUser;
        }

        public async Task<bool> EliminarUsuarioAsync(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null)
                throw new KeyNotFoundException("Usuario no encontrado.");

            await _usuarioRepository.DeleteAsync(id);
            return true;
        }
    }
}