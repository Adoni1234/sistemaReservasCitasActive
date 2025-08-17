using SistemaReservasCitas.Domain.Entities;

namespace SistemaReservasCitas.Domain.Repositories;

public interface ITurnoRepository : IRepository<Turno>
{
	Task<List<Turno>> GetAllTheShiftsOfTheUserAsync(int userId);
}