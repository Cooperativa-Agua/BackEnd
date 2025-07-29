using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
   
     
        public class UpdateBombaEstadoDto
        {
            [Required]
            public bool EstaEncendida { get; set; }

            [Required]
            public bool RelayActivo { get; set; }

            [Required]
            public bool SalvaMotorActivo { get; set; }

            [Required]
            public bool FlujometroActivo { get; set; }
        }
    
   
}
