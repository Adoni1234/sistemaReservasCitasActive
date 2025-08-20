﻿using SistemaReservasCitas.Domain.Entities;
using System.Threading.Tasks;

using SistemaReservasCitas.Domain.DTOs;
using System.Threading.Tasks;

namespace SistemaReservasCitas.Application.Interfaces
{
    public interface IConfiguracionService
    {
        // fechas
        Task<FechaHabilitada> CrearFechaHabilitadaAsync(CrearFechaHabilitadaDto fechaDto);
        Task<FechaHabilitada> ObtenerFechaHabilitadaAsync(int id);
        Task<IEnumerable<FechaHabilitada>> ObtenerFechaHabilitadaAllAsync();
        
        //horarios
        Task<Horario> ObtenerHorarioAsync(int id);
        Task<IEnumerable<Horario>> ObtenerHorarioAllAsync();
        Task<Horario> CrearHorarioAsync(CrearHorarioDto horarioDto);
        // Turnos
        Task<IEnumerable<Turno>> ObtenerTodosLosTurnosAsync(int userFiltro);
        Task<IEnumerable<Turno>> ObtenerTodosLosTurnosDeEsteUsuarioAsync(int userId);
        Task<IEnumerable<Turno>> ObtenerTodosLosTurnosDisponibles();
        Task<Turno> ObtenerTurnoAsync(int id);
        Task<Turno> CrearTurnoAsync(CrearTurnoDto turnoDto);
    }
}
