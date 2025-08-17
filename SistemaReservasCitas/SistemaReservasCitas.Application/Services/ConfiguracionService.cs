
﻿using SistemaReservasCitas.Application.Interfaces;
using SistemaReservasCitas.Domain.DTOs;
using SistemaReservasCitas.Domain.Entities;
using SistemaReservasCitas.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace SistemaReservasCitas.Application.Services
{
    public class ConfiguracionService : IConfiguracionService
    {
        private readonly IRepository<FechaHabilitada> _fechasRepository;
        private readonly IRepository<Horario> _horarioRepository;
        private readonly ITurnoRepository _turnoRepository;

        public ConfiguracionService(
            IRepository<FechaHabilitada> fechasRepository,
            IRepository<Horario> horarioRepository,
            ITurnoRepository turnoRepository)
        {
            _fechasRepository = fechasRepository;
            _horarioRepository = horarioRepository;
            _turnoRepository = turnoRepository;
        }

        public async Task<FechaHabilitada> CrearFechaHabilitadaAsync(CrearFechaHabilitadaDto fechaDto)
        {
            if (fechaDto == null || fechaDto.Fecha == default)
                throw new ArgumentException("Fecha inválida.");

            var existente = await _fechasRepository.FindAsync(f => f.Fecha == fechaDto.Fecha.Date);

            if (existente != null)
                throw new InvalidOperationException("La fecha ya está registrada.");

            var fecha = new FechaHabilitada { Fecha = fechaDto.Fecha };
            await _fechasRepository.AddAsync(fecha);
            return fecha;
        }


        public async Task<Horario> CrearHorarioAsync(CrearHorarioDto horarioDto)
        {
            if (string.IsNullOrEmpty(horarioDto.Inicio) || string.IsNullOrEmpty(horarioDto.Fin))
                throw new ArgumentException("Inicio y fin del horario son requeridos.");

            // Parsear strings a TimeSpan
            if (!TimeSpan.TryParse(horarioDto.Inicio, out TimeSpan inicio) ||
                !TimeSpan.TryParse(horarioDto.Fin, out TimeSpan fin))
                throw new ArgumentException("Formato de hora inválido. Use HH:mm (ejemplo: 08:00).");

            var horario = new Horario
            {
                Inicio = inicio,
                Fin = fin,
                NombreTurno = string.IsNullOrEmpty(horarioDto.NombreTurno)
                             ? "Sin nombre"
                             : horarioDto.NombreTurno
            };


            try
            {
                await _horarioRepository.AddAsync(horario);
                return horario;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al guardar Horario: " + ex.InnerException?.Message);
                throw;
            }
        }


        public async Task<Turno> CrearTurnoAsync(CrearTurnoDto turnoDto)
        {
            if (turnoDto.Fecha == default || turnoDto.IdHorario <= 0 || turnoDto.EstacionesCantidad <= 0 || turnoDto.TiempoCita <= 0)
                throw new ArgumentException("Datos del turno inválidos.");

            var turno = new Turno
            {
                Fecha = turnoDto.Fecha,
                IdHorario = turnoDto.IdHorario,
                EstacionesCantidad = turnoDto.EstacionesCantidad,
                TiempoCita = turnoDto.TiempoCita,
                Estado = "activo"
            };
            await _turnoRepository.AddAsync(turno);
            return turno;
        }

        public async Task<FechaHabilitada> ObtenerFechaHabilitadaAsync(int id)
        {
            var fecha = await _fechasRepository.GetByIdAsync(id);
            if (fecha == null)
                throw new KeyNotFoundException("Fecha habilitada no encontrada.");
            return fecha;
        }

        public async Task<Horario> ObtenerHorarioAsync(int id)
        {
            var horario = await _horarioRepository.GetByIdAsync(id);
            if (horario == null)
                throw new KeyNotFoundException("Horario no encontrado.");
            return horario;
        }

        public async Task<Turno> ObtenerTurnoAsync(int id)
        {
            var turno = await _turnoRepository.GetByIdAsync(id);
            if (turno == null)
                throw new KeyNotFoundException("Turno no encontrado.");
            return turno;
        }
     
        public async Task<IEnumerable<FechaHabilitada>> ObtenerFechaHabilitadaAllAsync()
        {
            var fecha = await _fechasRepository.GetAllAsync();
            return fecha;
        }
        public async Task<IEnumerable<Turno>> ObtenerTodosLosTurnosDeEsteUsuarioAsync(int userId)
        {
            
            return await _turnoRepository.GetAllTheShiftsOfTheUserAsync(userId);
        }

        public async Task<IEnumerable<Horario>> ObtenerHorarioAllAsync()
        {
            var horario = await _horarioRepository.GetAllAsync();
            return horario;
        }

        //REMINDER: Hacer un metodo aparte para este filtro, ya que esa logica nos afecta el controlador general.
        //TODO: COMPAI LLAMAR A ADONI PA QUE NO NOS LLEVE EL DIABLO.
        public async Task<IEnumerable<Turno>> ObtenerTurnoAllAsync(int userId)
        {
            var turnos = await _turnoRepository.GetAllAsync();
            List<Turno> turnosFiltrados = turnos.ToList();
            //turnosFiltrados.RemoveAll(t => t.Citas.Any(c => c.IdUsuario == userId));
         
            return turnosFiltrados;
        }
        
    }
}
