using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class TanqueResumenDto
    {
        public int TotalTanques { get; set; }
        public int TanquesActivos { get; set; }
        public int TanquesInactivos { get; set; }
        public double NivelPromedioGeneral { get; set; }
        public double CapacidadTotalSistema { get; set; }
        public double LitrosTotalesActuales { get; set; }
        public int TanquesNivelCritico { get; set; } // Nivel < 20%
        public int TanquesNivelBajo { get; set; }    // Nivel 20-49%
        public int TanquesNivelMedio { get; set; }   // Nivel 50-79%
        public int TanquesNivelAlto { get; set; }    // Nivel >= 80%
        public DateTime UltimaActualizacion { get; set; }
    }
}
