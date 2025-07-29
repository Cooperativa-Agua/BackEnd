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
    public DateTime FechaCreacion { get; set; }
    public DateTime UltimaActualizacion { get; set; }

    public Bomba()
    {
        FechaCreacion = DateTime.UtcNow;
        UltimaActualizacion = DateTime.UtcNow;
    }

    public void Encender()
    {
        EstaEncendida = true;
        UltimaActualizacion = DateTime.UtcNow;
    }

    public void Apagar()
    {
        EstaEncendida = false;
        UltimaActualizacion = DateTime.UtcNow;
    }

    public void ActualizarEstado(bool relay, bool salvaMotor, bool flujometro)
    {
        RelayActivo = relay;
        SalvaMotorActivo = salvaMotor;
        FlujometroActivo = flujometro;
        UltimaActualizacion = DateTime.UtcNow;
    }
}