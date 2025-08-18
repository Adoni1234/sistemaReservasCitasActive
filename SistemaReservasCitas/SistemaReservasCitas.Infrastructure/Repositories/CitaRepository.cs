using Microsoft.EntityFrameworkCore;
using SistemaReservasCitas.Domain.Entities;
using SistemaReservasCitas.Domain.Repositories;
using SistemaReservasCitas.Infrastructure.Data;

namespace SistemaReservasCitasApi.Infrastructure.Repositories;

public class CitaRepository : SqlRepository<Cita>, ICitaRepository
{
	public CitaRepository(SistemaReservasCitasContext context) : base(context)
	{
	}

	public async Task<Cita> DeleteshiftByIdAsync(int turnoId, int userId)
	{ 
		var cita = await _context.Cita
			.FirstOrDefaultAsync(c => c.TurnoId == turnoId && c.IdUsuario == userId);

		_context.Cita.Remove(cita);
		await _context.SaveChangesAsync();
		return cita;
	}
}