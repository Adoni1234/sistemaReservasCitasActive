using SistemaReservasCitas.Domain.Entities;

namespace SistemaReservasCitas.Domain.Repositories;

public interface IUsuarioRepository
{
	Task<Usuario?> ObtenerPorCredencialesAsync(string usuarioNombre, string password);
}