using SistemaReservasCitas.Domain.DTOs;
using SistemaReservasCitas.Domain.Entities;

namespace SistemaReservasCitas.Application.Interfaces;

public interface IInicioSesion
{
	Task<string?> InicioSesionAsync(InicioSesionDto inicioSesionDto);
}