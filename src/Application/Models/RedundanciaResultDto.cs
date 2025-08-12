using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class RedundanciaResultDto
    {
        public DateTime FechaVerificacion { get; set; }
        public int BombasRequeridas { get; set; }
        public int BombasActivas { get; set; }
        public int BombasDisponibles { get; set; }
        public bool AccionRequerida { get; set; }
        public bool EstadoCritico { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public List<BombaAccionDto> BombasActivadas { get; set; } = new();
        public List<BombaAccionDto> BombasDesactivadas { get; set; } = new();
        public List<BombaAccionDto> BombasConFalla { get; set; } = new();
    }
}
