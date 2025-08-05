using SistemaReservasCitas.Application.Interfaces;
using SistemaReservasCitas.Application.ValidacionesServices;
using SistemaReservasCitas.Domain.Entities;
using SistemaReservasCitas.Domain.Repositories;

using SistemaReservasCitasApi.Application.Interfaces;

namespace SistemaReservasCitasApi.Application.Services
{
    public class CitaService : ICitaService
    {

        private readonly IRepository<Cita> _citaRepository;
        private readonly IRepository<Turno> _turnoRepository;
        private readonly IEmailService _emailService;
        private readonly ReservaValidations _validations;
        public CitaService(IRepository<Cita> citaRepository, IRepository<Turno> turnoRepository, IEmailService emailService, ReservaValidations validations)
        {
            _validations = validations;
            _citaRepository = citaRepository;
            _turnoRepository = turnoRepository;
            _emailService = emailService;
        }

        public async Task<Cita> ReservarCitaAsync(int usuarioId, int turnoId)
        {
            await _validations.ValidarTurnoActivoAsync(turnoId);

            var turno = await _turnoRepository.GetByIdAsync(turnoId);

            await _validations.ValidarFechaHabilitadaAsync(turno.Fecha);
            await _validations.ValidarTurnoNoLlenoAsync(turnoId);
            await _validations.ValidarUsuarioSinCitaEnFechaAsync(usuarioId, turno.Fecha);

            var cita = new Cita { IdUsuario = usuarioId, TurnoId = turnoId };
            await _citaRepository.AddAsync(cita);
            await _emailService.SendEmailAsync("user@GMAIL.com", "Cita Reservada", "Su cita ha sido reservada.");
            return cita;
        }

        public async Task<IEnumerable<Cita>> ObtenerCitasPorUsuarioAsync(int usuarioId)
        {
            var citas = await _citaRepository.GetAllAsync();
            return citas.Where(c => c.IdUsuario == usuarioId);
        }

        public async Task<bool> CancelarCitaAsync(int citaId)
        {
            var cita = await _citaRepository.GetByIdAsync(citaId);
            if (cita == null) return false;
            await _citaRepository.DeleteAsync(citaId);
            return true;
        }
    }
}
