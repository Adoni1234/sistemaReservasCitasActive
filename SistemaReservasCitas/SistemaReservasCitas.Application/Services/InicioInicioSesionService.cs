using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SistemaReservasCitas.Application.Interfaces;
using SistemaReservasCitas.Domain.DTOs;
using SistemaReservasCitas.Domain.Entities;
using SistemaReservasCitas.Domain.Repositories;

namespace SistemaReservasCitas.Application.Services;

public class InicioInicioSesionService : IInicioSesion
{

	private readonly IUsuarioRepository _usuarioRepository;
	private readonly IJwt _jwt;

	public InicioInicioSesionService(IUsuarioRepository usuarioRepository, IJwt jwt)
	{

		_usuarioRepository = usuarioRepository;
		_jwt = jwt;
	}

	public async Task<string?> InicioSesionAsync(InicioSesionDto inicioSesionDto)
	{
		var usuario = await _usuarioRepository.ObtenerPorCredencialesAsync(inicioSesionDto.UsuarioNombre, inicioSesionDto.Password); // comparar con el hash en produccion.

		if (usuario == null)
			return null;

		return _jwt.GenerarToken(usuario);
	}
}
