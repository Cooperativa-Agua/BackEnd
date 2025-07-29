using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Bomba> Bombas { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de la entidad Bomba
        modelBuilder.Entity<Bomba>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500);
            entity.Property(e => e.EstaEncendida)
                .IsRequired();
            entity.Property(e => e.RelayActivo)
                .IsRequired();
            entity.Property(e => e.SalvaMotorActivo)
                .IsRequired();
            entity.Property(e => e.FlujometroActivo)
                .IsRequired();
            entity.Property(e => e.FechaCreacion)
                .IsRequired();
            entity.Property(e => e.UltimaActualizacion)
                .IsRequired();
        });

        // Datos iniciales (Seed data)
        modelBuilder.Entity<Bomba>().HasData(
            new Bomba
            {
                Id = 1,
                Nombre = "Bomba 1",
                Descripcion = "Bomba principal del sector A",
                EstaEncendida = false,
                RelayActivo = false,
                SalvaMotorActivo = false,
                FlujometroActivo = false,
                FechaCreacion = DateTime.UtcNow,
                UltimaActualizacion = DateTime.UtcNow
            },
            new Bomba
            {
                Id = 2,
                Nombre = "Bomba 2",
                Descripcion = "Bomba secundaria del sector A",
                EstaEncendida = false,
                RelayActivo = false,
                SalvaMotorActivo = false,
                FlujometroActivo = false,
                FechaCreacion = DateTime.UtcNow,
                UltimaActualizacion = DateTime.UtcNow
            },
            new Bomba
            {
                Id = 3,
                Nombre = "Bomba 3",
                Descripcion = "Bomba de respaldo del sector B",
                EstaEncendida = false,
                RelayActivo = false,
                SalvaMotorActivo = false,
                FlujometroActivo = false,
                FechaCreacion = DateTime.UtcNow,
                UltimaActualizacion = DateTime.UtcNow
            }
        );
    }
}