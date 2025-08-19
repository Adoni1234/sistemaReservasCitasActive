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


	public async Task AddTheSlots(List<Slot> slots)
	{
		await _context.Slots.AddRangeAsync(slots);
		await _context.SaveChangesAsync();
	}

	public async Task<List<Turno>> GetAllTheShiftsOfTheUserAsync(int userId)
	{
		//return await this._context.Turno.Where(t => t.Citas.Any(c => c.IdUsuario == userId)).ToListAsync();
		
		// No este metodo trae todos los registros, no filtra de hoy a dias posteriores
		return await _context.Turno
			.Include(t => t.Horario)
			.Where(t => t.Slots.Any(s => s.Cita != null && s.Cita.IdUsuario == userId))
			.Select(t => new Turno
			{
				Id = t.Id,
				Fecha = t.Fecha,
				EstacionesCantidad = t.EstacionesCantidad,
				Horario = t.Horario,
				TiempoCita = t.TiempoCita,
				Slots = t.Slots.Where(slot => slot.Cita.IdUsuario == userId).ToList()
			})
			.ToListAsync();
	}

	public async Task<List<Turno>> GetAllAvailableShiftsAsync()
	{
		return await _context.Turno
			.Where(t => t.Slots.Any(s => !s.EstaTomando))
			.Select(t => new Turno
			{
				Id = t.Id,
				Fecha = t.Fecha,
				EstacionesCantidad = t.EstacionesCantidad,
				Horario = t.Horario,
				TiempoCita = t.TiempoCita,
				Slots = t.Slots.Where(slot => !slot.EstaTomando).ToList()
			})
			.ToListAsync();
	}

	public async Task<bool> ThatSlotIsTaken(int slotId)
	{
		var slot =  await this._context.Slots.AsNoTracking().FirstOrDefaultAsync(s => s.Id == slotId);
		return slot.EstaTomando;
	}

	public async Task<Slot?> GetSlotInfo(int slotId)
	{
		return  await this._context.Slots
			.Include(s => s.Turno)
			.AsNoTracking().FirstOrDefaultAsync(s => s.Id == slotId);
	}
}