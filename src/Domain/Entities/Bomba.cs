using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Bomba
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool EstaEncendida { get; set; }
    public bool RelayActivo { get; set; }
    public bool SalvaMotorActivo { get; set; }
    public bool FlujometroActivo { get; set; }
    public EstadoBomba Estado { get; set; }
    public TipoFalla TipoFalla { get; set; }
    public bool EsBombaReserva { get; set; }
    public int Prioridad { get; set; } // 1 = Mayor prioridad
    public DateTime? UltimaFalla { get; set; }
    public int HorasOperacion { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime UltimaActualizacion { get; set; }
    public DateTime? UltimoMantenimiento { get; set; }

    public Bomba()
    {
        FechaCreacion = DateTime.UtcNow;
        UltimaActualizacion = DateTime.UtcNow;
        Estado = EstadoBomba.Apagada;
        TipoFalla = TipoFalla.SinFalla;
        EsBombaReserva = false;
        Prioridad = 1;
    }

    public void Encender()
    {
        if (Estado == EstadoBomba.Falla || Estado == EstadoBomba.Mantenimiento)
        {
            throw new InvalidOperationException($"No se puede encender la bomba {Nombre}. Estado actual: {Estado}");
        }

        EstaEncendida = true;
        Estado = EstadoBomba.Encendida;
        UltimaActualizacion = DateTime.UtcNow;
    }

    public void Apagar()
    {
        EstaEncendida = false;
        Estado = EstadoBomba.Apagada;
        UltimaActualizacion = DateTime.UtcNow;
    }

    public void MarcarFalla(TipoFalla tipoFalla)
    {
        EstaEncendida = false;
        Estado = EstadoBomba.Falla;
        TipoFalla = tipoFalla;
        UltimaFalla = DateTime.UtcNow;
        UltimaActualizacion = DateTime.UtcNow;
    }

    public void RepararFalla()
    {
        Estado = EstadoBomba.Apagada;
        TipoFalla = TipoFalla.SinFalla;
        UltimaActualizacion = DateTime.UtcNow;
    }

    public void PonerEnMantenimiento()
    {
        EstaEncendida = false;
        Estado = EstadoBomba.Mantenimiento;
        UltimoMantenimiento = DateTime.UtcNow;
        UltimaActualizacion = DateTime.UtcNow;
    }

    public void FinalizarMantenimiento()
    {
        Estado = EstadoBomba.Apagada;
        UltimaActualizacion = DateTime.UtcNow;
    }

    public void PonerEnEspera()
    {
        EstaEncendida = false;
        Estado = EstadoBomba.EnEspera;
        UltimaActualizacion = DateTime.UtcNow;
    }

    public void ActualizarEstado(bool relay, bool salvaMotor, bool flujometro)
    {
        RelayActivo = relay;
        SalvaMotorActivo = salvaMotor;
        FlujometroActivo = flujometro;
        UltimaActualizacion = DateTime.UtcNow;

        // Detectar fallas automáticamente
        if (EstaEncendida)
        {
            if (!salvaMotor)
            {
                MarcarFalla(TipoFalla.FallaSalvaMotor);
            }
            else if (!relay)
            {
                MarcarFalla(TipoFalla.FallaRelay);
            }
            else if (!flujometro)
            {
                MarcarFalla(TipoFalla.FallaFlujometro);
            }
        }
    }

    public bool EstaOperativa()
    {
        return Estado != EstadoBomba.Falla && Estado != EstadoBomba.Mantenimiento;
    }

    public bool PuedeEncenderse()
    {
        return Estado == EstadoBomba.Apagada || Estado == EstadoBomba.EnEspera;
    }
}