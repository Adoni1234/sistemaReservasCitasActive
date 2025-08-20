﻿using System.Security.Claims;
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

        [HttpPost("reservar/{usuarioId:int}/{slotId:int}")]
        //[Authorize(Roles = "user")]
        public async Task<IActionResult> ReservarCita(int usuarioId, int slotId)
        {
            try
            {
                var cita = await _citaService.ReservarCitaAsync(usuarioId, slotId);
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

        [HttpDelete("porUsuario/{slotId}/{usuarioId}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> CancelarCitaPorUsuarioYTurno(int slotId, int usuarioId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("No se pudo obtener el usuario del token");
            }

            int userId = int.Parse(userIdClaim);
            await _citaService.CancelarCitaAsync(slotId, userId);
            return Ok();
        }
        
    }
}