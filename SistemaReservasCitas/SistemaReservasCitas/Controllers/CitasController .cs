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

        [HttpDelete("{citaId}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> CancelarCita(int citaId)
        {
            var success = await _citaService.CancelarCitaAsync(citaId);
            return success ? Ok() : NotFound();
        }
    }
}