using Microsoft.EntityFrameworkCore;
using SistemaReservasCitas.Domain.Entities;
using SistemaReservasCitas.Domain.Repositories;
using SistemaReservasCitas.Infrastructure.Data;

namespace SistemaReservasCitasApi.Infrastructure.Repositories;

public class TurnoRepository : SqlRepository<Turno>, ITurnoRepository
{
	public TurnoRepository(SistemaReservasCitasContext context) : base(context)
	{
	}
	
	public override async Task<IEnumerable<Turno>> GetAllAsync()
	{
		return await _context.Set<Turno>()
			.Include(t => t.Citas)
			.Where(t => t.Citas.Count < t.EstacionesCantidad).ToListAsync();

	}
	
	public async Task<List<Turno>> GetAllTheShiftsOfTheUserAsync(int userId)
	{
		return await this._context.Turno.Where(t => t.Citas.Any(c => c.IdUsuario == userId)).ToListAsync();
		
	}
}