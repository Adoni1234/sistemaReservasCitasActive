using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaReservasCitas.Domain.Entities;
using SistemaReservasCitas.Domain.Repositories;

namespace SistemaReservasCitas.Application.ValidacionesServices
{
    public class ReservaValidations
    {
        private readonly IRepository<Cita> _citaRepository;
        private readonly IRepository<Turno> _turnoRepository;
        private readonly IRepository<FechaHabilitada> _fechaHabilitadaRepository;

        public ReservaValidations(
            IRepository<Cita> citaRepository,
            IRepository<Turno> turnoRepository,
            IRepository<FechaHabilitada> fechaHabilitadaRepository)
        {
            _citaRepository = citaRepository;
            _turnoRepository = turnoRepository;
            _fechaHabilitadaRepository = fechaHabilitadaRepository;
        }

        public async Task ValidarTurnoActivoAsync(int turnoId)
        {
            var turno = await _turnoRepository.GetByIdAsync(turnoId);
            if (turno == null || turno.Estado != "activo")
                throw new Exception("Turno no disponible.");
        }

        public async Task ValidarFechaHabilitadaAsync(DateTime fecha)
        {
            var habilitada = await _fechaHabilitadaRepository
                .GetAllAsync()
                .ContinueWith(t => t.Result.Any(f => f.Fecha == fecha && f.Fecha >= DateTime.Today));

            if (!habilitada)
                throw new Exception("La cita no puede hacerse para una fecha pasada o no habilitada.");
        }

        public async Task ValidarTurnoNoLlenoAsync(int turnoId)
        {
            var turno = await _turnoRepository.GetByIdAsync(turnoId);
            var cantidadCitas = await _citaRepository.GetAllAsync()
                .ContinueWith(t => t.Result.Count(c => c.TurnoId == turnoId));

            if (cantidadCitas >= turno.EstacionesCantidad)
                throw new Exception("Turno lleno.");
        }

        public async Task ValidarUsuarioSinCitaEnFechaAsync(int usuarioId, DateTime fecha)
        {
            var citas = await _citaRepository.GetAllAsync();

            bool tieneCita = citas.Any(c => c.IdUsuario == usuarioId && c.Turno != null && c.Turno.Fecha == fecha);

            if (tieneCita)
                throw new Exception("Un usuario no puede tener más de una cita por día.");
        }

        
    }
}
