using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class ActualizarPrioridadDto
    {
        [Required]
        [Range(1, 10)]
        public int NuevaPrioridad { get; set; }
    }
}