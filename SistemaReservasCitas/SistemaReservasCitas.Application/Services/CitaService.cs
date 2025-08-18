using SistemaReservasCitas.Application.Interfaces;
using SistemaReservasCitas.Application.ValidacionesServices;
using SistemaReservasCitas.Domain.DTOs;
using SistemaReservasCitas.Domain.Entities;
using SistemaReservasCitas.Domain.Repositories;
using SistemaReservasCitasApi.Application.Interfaces;

namespace SistemaReservasCitas.Application.Services
{
    public class CitaService : ICitaService
    {

        private readonly ICitaRepository _citaRepository;
        private readonly IRepository<Turno> _turnoRepository;
        private readonly IRepository<Usuario> _userRepository;
        private readonly IEmailService _emailService;
        private readonly ReservaValidations _validations;
        public CitaService(
            ICitaRepository citaRepository, 
            IRepository<Turno> turnoRepository, 
            IEmailService emailService, 
            ReservaValidations validations, 
            IRepository<Usuario> userRepository)
        {
            _validations = validations;
            _citaRepository = citaRepository;
            _turnoRepository = turnoRepository;
            _emailService = emailService;
            _userRepository = userRepository;
        }

        public async Task<Cita> ReservarCitaAsync(int usuarioId, int turnoId)
        {
                await _validations.ValidarTurnoActivoAsync(turnoId);

                var turno = await _turnoRepository.GetByIdAsync(turnoId);
                var user = await _userRepository.GetByIdAsync(usuarioId);
                var email = user.Email;

                await _validations.ValidarFechaHabilitadaAsync(turno.Fecha);
                await _validations.ValidarTurnoNoLlenoAsync(turnoId);
                await _validations.ValidarUsuarioSinCitaEnFechaAsync(usuarioId, turno.Fecha);

                var cita = new Cita { IdUsuario = usuarioId, TurnoId = turnoId };
                await _citaRepository.AddAsync(cita);
                await _emailService.SendEmailAsync(email, "Cita Reservada", $"Estimado/a {user.UsuarioNombre} su cita fue reservada correctamente, favor de asistir en la fecha programada {turno.Fecha}");
                return cita;
            
        }

        public async Task<IEnumerable<CitasDto>> ObtenerCitasPorUsuarioAsync(int usuarioId)
        {
            var citas = await _citaRepository.GetAllAsync();
            List<CitasDto> citasDto = new List<CitasDto>();
            foreach (var cita in citas)
            {
                citasDto.Add(new CitasDto()
                {
                    Id = cita.Id,
                    UsuarioId = usuarioId,
                    TurnoId = cita.TurnoId
                    
                });
            }
            return citasDto.Where(c => c.UsuarioId == usuarioId).ToList();
        }

        public async Task<bool> CancelarCitaAsync(int citaId)
        {
            var cita = await _citaRepository.GetByIdAsync(citaId);
            if (cita == null) return false;
            await _citaRepository.DeleteAsync(citaId);
            return true;
        }
        
        public async Task<Cita> CancelarCitaPorIdAsync(int turnoId, int usuarioId)
        {
           return await _citaRepository.DeleteshiftByIdAsync(turnoId, usuarioId);
            
        }
    }
}
