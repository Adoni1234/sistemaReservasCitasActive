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


	public async Task AgendarCita(int usuarioId, int slotId)
	{
		using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
		// Retorna el turno que contenga ese Slot
		var turno = await _context.Turno.Include(t => t.Slots)
			.FirstOrDefaultAsync(s => s.Slots.Any( slot => slot.Id == slotId));
        
		if (turno.Slots.Count(s => s.EstaTomando) < turno.EstacionesCantidad)
		{
			await _context.Cita.AddAsync(new Cita { IdUsuario = usuarioId, IdSlot = slotId });
			await _context.SaveChangesAsync();
			await _context.Slots.Where(s => s.Id == slotId )
				.ExecuteUpdateAsync(setters => setters.SetProperty(s => s.EstaTomando, true));
			await transaction.CommitAsync();
		}
		await transaction.RollbackAsync();
	}
	
	public async Task<Cita> DeleteBySlotIdAndUserId(int slotId, int userId)
	{ 
		var cita = await _context.Cita
			.FirstOrDefaultAsync(c => c.Slot.Id == slotId && c.IdUsuario == userId);

		_context.Cita.Remove(cita);
		await _context.SaveChangesAsync();
		return cita;
	}
}