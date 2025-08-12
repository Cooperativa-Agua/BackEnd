using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Tanque
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public double NivelAgua { get; set; } // Nivel del 1 al 100
        public double CapacidadMaxima { get; set; } // En litros
        public bool EstaActivo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime UltimaActualizacion { get; set; }

        public Tanque()
        {
            FechaCreacion = DateTime.UtcNow;
            UltimaActualizacion = DateTime.UtcNow;
            EstaActivo = true;
        }

        public void ActualizarNivel(double nuevoNivel)
        {
            if (nuevoNivel < 0 || nuevoNivel > 100)
                throw new ArgumentException("El nivel de agua debe estar entre 0 y 100");

            NivelAgua = nuevoNivel;
            UltimaActualizacion = DateTime.UtcNow;
        }

        public double GetLitrosActuales()
        {
            return (NivelAgua / 100.0) * CapacidadMaxima;
        }

        public string GetEstadoNivel()
        {
            return NivelAgua switch
            {
                >= 80 => "Alto",
                >= 50 => "Medio",
                >= 20 => "Bajo",
                _ => "Crítico"
            };
        }
    }
}