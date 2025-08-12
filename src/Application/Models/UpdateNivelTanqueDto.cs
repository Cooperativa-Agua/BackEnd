using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class UpdateNivelTanqueDto
    {
        [Required]
        [Range(0, 100, ErrorMessage = "El nivel de agua debe estar entre 0 y 100")]
        public double NivelAgua { get; set; }
    }
}
