using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaReservasCitas.Domain.Entities;

namespace SistemaReservasCitas.Application.Interfaces
{

    public interface ICitaService
    {
        Task<Cita> ReservarCitaAsync(int usuarioId, int turnoId);
        Task<IEnumerable<Cita>> ObtenerCitasPorUsuarioAsync(int usuarioId);
        Task<bool> CancelarCitaAsync(int citaId);
    }

}