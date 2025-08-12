using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class BombaMantenimientoDto
    {
        [Required]
        public int BombaId { get; set; }

        [Required]
        public bool IniciarMantenimiento { get; set; }

        public string Descripcion { get; set; } = string.Empty;
    }
}
