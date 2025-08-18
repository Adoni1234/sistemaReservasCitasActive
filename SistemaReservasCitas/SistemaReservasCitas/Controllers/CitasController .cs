using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaReservasCitas.Application.Interfaces;
using SistemaReservasCitas.Domain.DTOs;
using SistemaReservasCitasApi.Application.Interfaces;

namespace SistemaReservasCitasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitaController : ControllerBase
    {
        private readonly ICitaService _citaService;

        public CitaController(ICitaService citaService)
        {
            _citaService = citaService;
        }

        [HttpPost("reservar/{usuarioId:int}/{turnoId:int}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> ReservarCita(int usuarioId, int turnoId)
        {
            try
            {
                var cita = await _citaService.ReservarCitaAsync(usuarioId, turnoId);
                return Ok(cita);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("usuario/{usuarioId}")]
        [AllowAnonymous]
        //[Authorize(Roles = "user")]
        public async Task<IActionResult> ObtenerCitasPorUsuario(int usuarioId)
        {
            var citas = await _citaService.ObtenerCitasPorUsuarioAsync(usuarioId);
            return Ok(citas);
        }

        [HttpDelete("{turnoId}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> CancelarCita(int turnoId)
        {
            int userId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var success = await _citaService.CancelarCitaAsync(turnoId);
            return success ? Ok() : NotFound();
        }
        
        [HttpDelete("porUsuario/{turnoId}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> CancelarCitaPorUsuarioYTurno(int turnoId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("No se pudo obtener el usuario del token");
            }

            int userId = int.Parse(userIdClaim);

            var success = await _citaService.CancelarCitaPorIdAsync(turnoId, userId);

            return success != null
                ? Ok($"Cita {success.Id} eliminada correctamente")
                : NotFound("No se encontró la cita para este usuario y turno");
        }
        
    }
}