﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Bomba", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<bool>("EstaEncendida")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("FlujometroActivo")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("RelayActivo")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("SalvaMotorActivo")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("UltimaActualizacion")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Bombas");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Descripcion = "Bomba principal del sector A",
                            EstaEncendida = false,
                            FechaCreacion = new DateTime(2025, 7, 29, 19, 52, 51, 48, DateTimeKind.Utc).AddTicks(4684),
                            FlujometroActivo = false,
                            Nombre = "Bomba 1",
                            RelayActivo = false,
                            SalvaMotorActivo = false,
                            UltimaActualizacion = new DateTime(2025, 7, 29, 19, 52, 51, 48, DateTimeKind.Utc).AddTicks(4685)
                        },
                        new
                        {
                            Id = 2,
                            Descripcion = "Bomba secundaria del sector A",
                            EstaEncendida = false,
                            FechaCreacion = new DateTime(2025, 7, 29, 19, 52, 51, 48, DateTimeKind.Utc).AddTicks(4687),
                            FlujometroActivo = false,
                            Nombre = "Bomba 2",
                            RelayActivo = false,
                            SalvaMotorActivo = false,
                            UltimaActualizacion = new DateTime(2025, 7, 29, 19, 52, 51, 48, DateTimeKind.Utc).AddTicks(4687)
                        },
                        new
                        {
                            Id = 3,
                            Descripcion = "Bomba de respaldo del sector B",
                            EstaEncendida = false,
                            FechaCreacion = new DateTime(2025, 7, 29, 19, 52, 51, 48, DateTimeKind.Utc).AddTicks(4689),
                            FlujometroActivo = false,
                            Nombre = "Bomba 3",
                            RelayActivo = false,
                            SalvaMotorActivo = false,
                            UltimaActualizacion = new DateTime(2025, 7, 29, 19, 52, 51, 48, DateTimeKind.Utc).AddTicks(4689)
                        });
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
