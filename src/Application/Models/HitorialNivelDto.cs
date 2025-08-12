using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class HistorialNivelDto
    {
        public int TanqueId { get; set; }
        public string NombreTanque { get; set; } = string.Empty;
        public double NivelAnterior { get; set; }
        public double NivelActual { get; set; }
        public double DiferenciaNivel { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string TipoMovimiento { get; set; } = string.Empty; // "Subida", "Bajada", "Sin cambio"
    }
}

