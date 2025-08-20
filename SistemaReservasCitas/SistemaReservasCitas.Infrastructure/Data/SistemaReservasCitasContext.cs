using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using SistemaReservasCitas.Domain.Entities;

namespace SistemaReservasCitas.Infrastructure.Data
{
    public class SistemaReservasCitasContext : DbContext
    {
        public DbSet<Cita> Cita { get; set; }
        public DbSet<Turno> Turno { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Horario> Horario { get; set; }
        public DbSet<FechaHabilitada> FechasHabilitadas { get; set; }
        public DbSet<Slot> Slots { get; set; }

        public SistemaReservasCitasContext(DbContextOptions<SistemaReservasCitasContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Especifica los nombres exactos de las tablas
            modelBuilder.Entity<Cita>().ToTable("Cita");
            modelBuilder.Entity<Turno>().ToTable("Turno");
            modelBuilder.Entity<Usuario>().ToTable("Usuario");
            modelBuilder.Entity<Horario>().ToTable("Horario");
            modelBuilder.Entity<FechaHabilitada>().ToTable("FechasHabilitadas");
            modelBuilder.Entity<Slot>().ToTable("SLOTS");
            modelBuilder.Entity<Cita>();


            modelBuilder.Entity<Cita>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                // Mapeo columnas con nombres reales de la BD
                entity.Property(e => e.IdUsuario)
                    .HasColumnName("id_usuario");

                entity.Property(e => e.IdSlot)
                    .HasColumnName("idSlots");

                // Relaciones
                entity.HasOne(c => c.Usuario)
                    .WithMany(u => u.Citas)
                    .HasForeignKey(c => c.IdUsuario);

                entity.HasOne(c => c.Slot)
                    .WithOne(s => s.Cita)
                    .HasForeignKey<Cita>(c => c.IdSlot);
            });



            // Mapeo de columnas para Usuario
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Id)
                .HasColumnName("id");

            modelBuilder.Entity<Usuario>()
                .Property(u => u.UsuarioNombre)
                .HasColumnName("usuario");

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Password)
                .HasColumnName("psswd");
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Email)
                .HasColumnName("email");

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Rol)
                .HasColumnName("rol")
                .HasConversion<int>();


            //Turno   
            modelBuilder.Entity<Turno>()
                .HasOne(t => t.Horario)
                .WithMany(h => h.Turnos)
                .HasForeignKey(t => t.IdHorario);

            modelBuilder.Entity<Turno>()
                .Property(u => u.Id)
                .HasColumnName("id");

            modelBuilder.Entity<Turno>()
                .Property(u => u.Fecha)
                .HasColumnName("fecha");

            modelBuilder.Entity<Turno>()
                .Property(u => u.IdHorario)
                .HasColumnName("id_horario");

            modelBuilder.Entity<Turno>()
                .Property(u => u.EstacionesCantidad)
                .HasColumnName("estaciones_cantidad");

            modelBuilder.Entity<Turno>()
                .Property(u => u.TiempoCita)
                .HasColumnName("tiempo_cita")
                .HasColumnType("int");

            modelBuilder.Entity<Turno>()
                .Property(u => u.Estado)
                .HasColumnName("estado")
                .HasDefaultValue(1);

            modelBuilder.Entity<Turno>()
                .Property(u => u.IdHorario)
                .HasColumnName("id_horario");


            modelBuilder.Entity<Horario>(entity =>
            {
                entity.ToTable("Horario");

                entity.HasKey(h => h.Id);

                entity.Property(h => h.Inicio)
                    .HasColumnName("inicio")
                    .HasDefaultValue(TimeSpan.Zero);

                entity.Property(h => h.Fin)
                    .HasColumnName("fin")
                    .HasDefaultValue(TimeSpan.Zero);

                entity.Property(h => h.NombreTurno)
                    .HasColumnName("nombre_turno")
                    .HasMaxLength(50)
                    .HasDefaultValue("Sin nombre");
            });


            modelBuilder.Entity<Slot>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("Slot_Id_PK");
                
                entity.HasOne(s => s.Turno)
                    .WithMany(t => t.Slots)
                    .HasForeignKey(s => s.IdTurno)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.Property(s => s.HoraInicio)
                    .IsRequired()
                    .HasColumnType("time(0)")
                    .HasColumnName("fechaInicio");

                entity.Property(s => s.HoraFin)
                    .IsRequired()
                    .HasColumnType("time(0)")
                    .HasColumnName("fechaFin");

                entity.Property(s => s.EstaTomando)
                    .HasColumnName("tomado")
                    .HasDefaultValue(false);
            });
        }
    }
}