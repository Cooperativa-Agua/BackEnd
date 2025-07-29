using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IBombaService
{
    Task<IEnumerable<BombaDto>> GetAllBombasAsync();
    Task<BombaDto?> GetBombaByIdAsync(int id);
    Task<BombaDto> CreateBombaAsync(CreateBombaDto createBombaDto);
    Task<BombaDto> UpdateBombaEstadoAsync(int id, UpdateBombaEstadoDto updateDto);
    Task<BombaDto> EncenderBombaAsync(int id);
    Task<BombaDto> ApagarBombaAsync(int id);
    Task DeleteBombaAsync(int id);
    Task<BombaEstadoResumenDto> GetEstadoResumenAsync();
    Task<IEnumerable<BombaDto>> GetBombasEncendidasAsync();
    Task<IEnumerable<BombaDto>> GetBombasApagadasAsync();
    Task<IEnumerable<BombaDto>> EncenderTodasLasBombasAsync();
    Task<IEnumerable<BombaDto>> ApagarTodasLasBombasAsync();
}
