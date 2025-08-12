using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class EstadoRedundanciaDto
    {
        public int TotalBombas { get; set; }
        public int BombasRequeridas { get; set; }
        public int BombasActivas { get; set; }
        public int BombasOperativas { get; set; }
        public int BombasConFalla { get; set; }
        public int BombasEnMantenimiento { get; set; }
        public string EstadoSistema { get; set; } = string.Empty; // Normal, Advertencia, Crítico
        public DateTime UltimaVerificacion { get; set; }
        public List<BombaEstadoDto> Bombas { get; set; } = new();
    }
}
