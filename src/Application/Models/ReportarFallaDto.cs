using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class ReportarFallaDto
    {
        [Required]
        public TipoFalla TipoFalla { get; set; }

        public string Descripcion { get; set; } = string.Empty;
    }
}
