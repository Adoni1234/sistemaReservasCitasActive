using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SistemaReservasCitas.Application.Interfaces;
using SistemaReservasCitas.Domain.DTOs;

namespace SistemaReservasCitas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize(Roles = "Admin")]
    public class ConfiguracionController : ControllerBase
    {
        private readonly IConfiguracionService _configuracionService;
        private readonly ILogger<ConfiguracionController> _logger;

        public ConfiguracionController(IConfiguracionService configuracionService, ILogger<ConfiguracionController> logger)
        {
            _configuracionService = configuracionService;
            _logger = logger;
        }

        [HttpPost("fechas-habilitadas")]
        public async Task<IActionResult> CrearFechaHabilitada([FromBody] CrearFechaHabilitadaDto fechaDto)
        {
            try
            {
                _logger.LogInformation("Creando nueva fecha habilitada.");
                var createdFecha = await _configuracionService.CrearFechaHabilitadaAsync(fechaDto);
                return CreatedAtAction(nameof(ObtenerFechaHabilitada), new { id = createdFecha.Id }, createdFecha);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Error de validación al crear fecha habilitada: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error interno al crear fecha habilitada: {Message}", ex.Message);
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpPost("horarios")]
        public async Task<IActionResult> CrearHorario([FromBody] CrearHorarioDto horarioDto)
        {
            try
            {
                _logger.LogInformation("Creando nuevo horario.");
                var createdHorario = await _configuracionService.CrearHorarioAsync(horarioDto);
                return CreatedAtAction(nameof(ObtenerHorario), new { id = createdHorario.Id }, createdHorario);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Error de validación al crear horario: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error interno al crear horario: {Message}", ex.Message);
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpPost("turnos")]
        public async Task<IActionResult> CrearTurno([FromBody] CrearTurnoDto turnoDto)
        {
            try
            {
                _logger.LogInformation("Creando nuevo turno.");
                var createdTurno = await _configuracionService.CrearTurnoAsync(turnoDto);
                return CreatedAtAction(nameof(ObtenerTurno), new { id = createdTurno.Id }, createdTurno);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Error de validación al crear turno: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error interno al crear turno: {Message}", ex.Message);
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpGet("fechas-habilitadas/{id}")]
        public async Task<IActionResult> ObtenerFechaHabilitada(int id)
        {
            try
            {
                _logger.LogInformation("Consultando fecha habilitada con ID {id}", id);
                var fecha = await _configuracionService.ObtenerFechaHabilitadaAsync(id);
                return Ok(fecha);
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("No se encontró la fecha habilitada con ID {id}", id);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error interno al consultar fecha habilitada con ID {id}: {Message}", id, ex.Message);
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpGet("horarios/{id}")]
        public async Task<IActionResult> ObtenerHorario(int id)
        {
            try
            {
                _logger.LogInformation("Consultando horario con ID {id}", id);
                var horario = await _configuracionService.ObtenerHorarioAsync(id);
                return Ok(horario);
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("No se encontró el horario con ID {id}", id);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error interno al consultar horario con ID {id}: {Message}", id, ex.Message);
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpGet("turnos/{id}")]
        public async Task<IActionResult> ObtenerTurno(int id)
        {
            try
            {
                _logger.LogInformation("Consultando turno con ID {id}", id);
                var turno = await _configuracionService.ObtenerTurnoAsync(id);
                return Ok(turno);
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("No se encontró el turno con ID {id}", id);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error interno al consultar turno con ID {id}: {Message}", id, ex.Message);
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}
