using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class BombaAccionDto
    {
        public int BombaId { get; set; }
        public string NombreBomba { get; set; } = string.Empty;
        public string Accion { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}