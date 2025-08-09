using Microsoft.EntityFrameworkCore;
using SistemaReservasCitas.Domain.Entities;
using SistemaReservasCitas.Domain.Repositories;
using SistemaReservasCitas.Infrastructure.Data;

namespace SistemaReservasCitasApi.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
	private readonly SistemaReservasCitasContext _context;

	public UsuarioRepository(SistemaReservasCitasContext context)
	{
		_context = context;
	}

	public async Task<Usuario?> ObtenerPorCredencialesAsync(string usuarioNombre, string password)
	{
		return await _context.Usuario
			.FirstOrDefaultAsync(u =>
				u.UsuarioNombre == usuarioNombre &&
				u.Password == password); // En producción, comparar hash
	}
}