using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class BombaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool EstaEncendida { get; set; }
        public bool RelayActivo { get; set; }
        public bool SalvaMotorActivo { get; set; }
        public bool FlujometroActivo { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string TipoFalla { get; set; } = string.Empty;
        public bool EsBombaReserva { get; set; }
        public int Prioridad { get; set; }
        public DateTime? UltimaFalla { get; set; }
        public int HorasOperacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime UltimaActualizacion { get; set; }
        public DateTime? UltimoMantenimiento { get; set; }
        public bool EstaOperativa { get; set; }
        public bool PuedeEncenderse { get; set; }
    }
}
