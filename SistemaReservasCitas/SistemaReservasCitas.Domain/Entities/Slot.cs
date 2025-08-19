namespace SistemaReservasCitas.Domain.Entities;

public class Slot
{
    public int Id { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }
    
    public int IdTurno { get; set; }
    public bool EstaTomando { get; set; }

    public Cita Cita { get; set; }
    public Turno Turno { get; set; }
}