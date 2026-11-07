using CentroUnas.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CentroUnas.Infrastructure.Context
{
    public class CentroUnasContext : DbContext
    {
        public CentroUnasContext(DbContextOptions<CentroUnasContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Tecnica> Tecnicas { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<Pago> Pagos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("Clientes");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Apellido).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(150);
                entity.Property(e => e.Telefono).IsRequired().HasMaxLength(20);
                entity.Property(e => e.FechaRegistro).HasDefaultValueSql("GETDATE()");

                entity.HasIndex(e => e.Email).IsUnique();
            });

            modelBuilder.Entity<Tecnica>(entity =>
            {
                entity.ToTable("Tecnicas");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Apellido).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(150);
                entity.Property(e => e.Telefono).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Especialidad).IsRequired().HasMaxLength(100);
                entity.Property(e => e.FechaRegistro).HasDefaultValueSql("GETDATE()");

                entity.HasIndex(e => e.Email).IsUnique();
            });

            modelBuilder.Entity<Empleado>(entity =>
            {
                entity.ToTable("Empleados");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Apellido).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(150);
                entity.Property(e => e.Telefono).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Cargo).IsRequired().HasMaxLength(100);
                entity.Property(e => e.FechaRegistro).HasDefaultValueSql("GETDATE()");

                entity.HasIndex(e => e.Email).IsUnique();
            });

            modelBuilder.Entity<Servicio>(entity =>
            {
                entity.ToTable("Servicios");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Descripcion).HasMaxLength(500);
                entity.Property(e => e.Precio).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.FechaRegistro).HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<Horario>(entity =>
            {
                entity.ToTable("Horarios");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.DiaSemana).HasConversion<string>().HasMaxLength(20);
                entity.Property(e => e.FechaRegistro).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Tecnica)
                    .WithMany(t => t.Horarios)
                    .HasForeignKey(e => e.TecnicaId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.TecnicaId, e.DiaSemana });
            });

            modelBuilder.Entity<Turno>(entity =>
            {
                entity.ToTable("Turnos");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Precio).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.Estado).HasConversion<string>().HasMaxLength(20);
                entity.Property(e => e.Observacion).HasMaxLength(500);
                entity.Property(e => e.FechaRegistro).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Cliente)
                    .WithMany(c => c.Turnos)
                    .HasForeignKey(e => e.ClienteId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Tecnica)
                    .WithMany(t => t.Turnos)
                    .HasForeignKey(e => e.TecnicaId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Servicio)
                    .WithMany(s => s.Turnos)
                    .HasForeignKey(e => e.ServicioId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Empleado)
                    .WithMany(em => em.Turnos)
                    .HasForeignKey(e => e.EmpleadoId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => new { e.TecnicaId, e.FechaHoraInicio });
                entity.HasIndex(e => new { e.ClienteId, e.FechaHoraInicio });
            });

            modelBuilder.Entity<Pago>(entity =>
            {
                entity.ToTable("Pagos");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Monto).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.MetodoPago).HasConversion<string>().HasMaxLength(20);
                entity.Property(e => e.Referencia).HasMaxLength(100);
                entity.Property(e => e.FechaPago).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.FechaRegistro).HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Turno)
                    .WithMany(t => t.Pagos)
                    .HasForeignKey(e => e.TurnoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
