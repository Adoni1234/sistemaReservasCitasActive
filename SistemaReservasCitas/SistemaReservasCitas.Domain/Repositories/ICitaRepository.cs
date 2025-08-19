using SistemaReservasCitas.Domain.Entities;

namespace SistemaReservasCitas.Domain.Repositories;

public interface ICitaRepository :IRepository<Cita>
{
	Task<Cita> DeleteBySlotIdAndUserId(int slotid, int userId);
}