using SistemaReservasCitas.Domain.Entities;

namespace SistemaReservasCitas.Application.Interfaces;

public interface IJwt
{
	string GenerarToken(Usuario usuario);
}