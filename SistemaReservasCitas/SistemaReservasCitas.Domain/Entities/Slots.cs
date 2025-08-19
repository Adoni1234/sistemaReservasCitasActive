using SistemaReservasCitas.Domain.Entities;

namespace ProyectoFinal.Models;

public class Slot
{
    public int id { get; set; }
    public TimeOnly inicioFecha { get; set; }
    public TimeOnly finFecha { get; set; }
    public int idSlot { get; set; }

    public bool tonamado { get; set; }

    public Cita Cita { get; set; }
    public Turno Turno { get; set; }

}