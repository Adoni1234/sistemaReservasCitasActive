using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaReservasCitas.Application.Interfaces;
using SistemaReservasCitas.Domain.DTOs;

namespace SistemaReservasCitas.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InicioSesionController : Controller
{
	private readonly IInicioSesion _inicioSesion;

	public InicioSesionController(IInicioSesion inicioSesion)
	{
		_inicioSesion = inicioSesion;
	}

	[HttpPost("login")]
	[AllowAnonymous]
	public async Task<IActionResult> InicioSesion([FromBody] InicioSesionDto dto)
	{
		try
		{
			var token = await _inicioSesion.InicioSesionAsync(dto);

			if (token == null)
				return Unauthorized(new { 
					success = false,
					message = "Usuario o contraseña incorrectos." 
				});

			return Ok(new { 
				success = true,
				data = new {
					token = token,
					tokenType = "Bearer"
				},
				message = "Inicio de sesion exitoso"
			});
		}
		catch (Exception ex)
		{
			// TODO: Log the exception with a logger
			return StatusCode(500, new { 
				success = false,
				message = "Error interno del servidor." 
			});
		}
	}
}