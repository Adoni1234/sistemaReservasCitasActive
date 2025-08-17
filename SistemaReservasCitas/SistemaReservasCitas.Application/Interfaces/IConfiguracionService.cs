﻿using SistemaReservasCitas.Domain.Entities;
using System.Threading.Tasks;

using SistemaReservasCitas.Domain.DTOs;
using System.Threading.Tasks;

namespace SistemaReservasCitas.Application.Interfaces
{
    public interface IConfiguracionService
    {
        Task<FechaHabilitada> CrearFechaHabilitadaAsync(CrearFechaHabilitadaDto fechaDto);
        Task<Horario> CrearHorarioAsync(CrearHorarioDto horarioDto);
        Task<Turno> CrearTurnoAsync(CrearTurnoDto turnoDto);
        Task<FechaHabilitada> ObtenerFechaHabilitadaAsync(int id);
        Task<Horario> ObtenerHorarioAsync(int id);
        Task<Turno> ObtenerTurnoAsync(int id);
        //Task GenerarHorariosAsync();
        Task<IEnumerable<Turno>> ObtenerTurnoAllAsync(int userId);
        Task<IEnumerable<Horario>> ObtenerHorarioAllAsync();
        Task<IEnumerable<FechaHabilitada>> ObtenerFechaHabilitadaAllAsync();
        
        Task<IEnumerable<Turno>> ObtenerTodosLosTurnosDeEsteUsuarioAsync(int userId);
    }
}
