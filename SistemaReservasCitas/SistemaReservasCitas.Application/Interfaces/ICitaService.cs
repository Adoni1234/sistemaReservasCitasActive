using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaReservasCitas.Domain.DTOs;
using SistemaReservasCitas.Domain.Entities;

namespace SistemaReservasCitas.Application.Interfaces
{

    public interface ICitaService
    {
        Task<Cita> ReservarCitaAsync(int usuarioId, int slotid);
        Task<IEnumerable<CitasDto>> ObtenerCitasPorUsuarioAsync(int usuarioId);
        Task CancelarCitaAsync(int slotid, int  usuarioId);
    }

}