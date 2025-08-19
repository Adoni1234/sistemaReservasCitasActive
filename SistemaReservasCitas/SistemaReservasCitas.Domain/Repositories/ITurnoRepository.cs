using SistemaReservasCitas.Domain.Entities;

namespace SistemaReservasCitas.Domain.Repositories;

public interface ITurnoRepository : IRepository<Turno>
{
	
	Task AddTheSlots(List<Slot> slots);
	
	Task<List<Turno>> GetAllTheShiftsOfTheUserAsync(int userId);
	Task<List<Turno>> GetAllAvailableShiftsAsync();
	
	Task<bool> ThatSlotIsTaken(int slotId);
	Task<Slot?> GetSlotInfo(int slotId);
}