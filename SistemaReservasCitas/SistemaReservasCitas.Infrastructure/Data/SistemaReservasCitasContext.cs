using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
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

        public SistemaReservasCitasContext(DbContextOptions<SistemaReservasCitasContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Especifica los nombres exactos de las tablas
            modelBuilder.Entity<Cita>().ToTable("Cita");
            modelBuilder.Entity<Turno>().ToTable("Turno");
            modelBuilder.Entity<Usuario>().ToTable("Usuario");
            modelBuilder.Entity<Horario>().ToTable("Horario");
            modelBuilder.Entity<FechaHabilitada>().ToTable("FechasHabilitadas");
            modelBuilder.Entity<Cita>();



            modelBuilder.Entity<Cita>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd(); // esto es suficiente// 💡 evita relectura después del insert

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
                entity.Property(e => e.TurnoId).HasColumnName("turno_id");

                entity.HasOne<Turno>()
                    .WithMany(t => t.Citas)
                    .HasForeignKey(c => c.TurnoId);

                entity.HasOne<Usuario>()
                    .WithMany()
                    .HasForeignKey(c => c.IdUsuario);
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
                .HasColumnName("Email");

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Rol)
                .HasColumnName("rol")
                .HasConversion<int>();

            // Relaciones
            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Usuario)
                .WithMany(u => u.Citas)
                .HasForeignKey(c => c.IdUsuario);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Turno)
                .WithMany(t => t.Citas)
                .HasForeignKey(c => c.TurnoId);

            modelBuilder.Entity<Turno>()
                .HasOne(t => t.Horario)
                .WithMany(h => h.Turnos)
                .HasForeignKey(t => t.IdHorario);



            //Turno
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



            // Configuraciones adicionales
            //modelBuilder.Entity<Turno>()
            //    .Property(t => t.Estado)


            modelBuilder.Entity<Horario>()
                .Property(h => h.NombreTurno)
                .HasDefaultValue("Sin nombre");
        }
    }
}