using Application.Models;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IBombaRedundanciaService
    {
        Task<RedundanciaResultDto> VerificarYMantenerRedundanciaAsync();
        Task<RedundanciaResultDto> ReportarFallaBombaAsync(int bombaId, TipoFalla tipoFalla);
        Task<RedundanciaResultDto> RepararBombaAsync(int bombaId);
        Task<EstadoRedundanciaDto> GetEstadoRedundanciaAsync();
        Task<RedundanciaResultDto> ForzarCambioTurnoAsync();
    }
}
