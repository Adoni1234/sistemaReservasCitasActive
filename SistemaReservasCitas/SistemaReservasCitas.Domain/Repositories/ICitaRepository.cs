using SistemaReservasCitas.Domain.Entities;

namespace SistemaReservasCitas.Domain.Repositories;

public interface ICitaRepository :IRepository<Cita>
{
	Task<Cita> DeleteshiftByIdAsync(int turnoId, int userId);
}