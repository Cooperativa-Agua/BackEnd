using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum EstadoBomba
    {
        Apagada = 0,
        Encendida = 1,
        Falla = 2,
        Mantenimiento = 3,
        EnEspera = 4
    }

    public enum TipoFalla
    {
        SinFalla = 0,
        FallaSalvaMotor = 1,
        FallaRelay = 2,
        FallaFlujometro = 3,
        FallaGeneral = 4,
        SinRespuesta = 5
    }
}