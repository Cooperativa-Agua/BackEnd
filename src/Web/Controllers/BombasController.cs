using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BombasController : ControllerBase
{
    private readonly IBombaService _bombaService;
    private readonly ILogger<BombasController> _logger;

    public BombasController(IBombaService bombaService, ILogger<BombasController> logger)
    {
        _bombaService = bombaService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todas las bombas
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BombaDto>>> GetAllBombas()
    {
        try
        {
            var bombas = await _bombaService.GetAllBombasAsync();
            return Ok(bombas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todas las bombas");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene una bomba por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<BombaDto>> GetBombaById(int id)
    {
        try
        {
            var bomba = await _bombaService.GetBombaByIdAsync(id);
            if (bomba == null)
                return NotFound($"Bomba with ID {id} not found");

            return Ok(bomba);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la bomba con ID {BombaId}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Crea una nueva bomba
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<BombaDto>> CreateBomba([FromBody] CreateBombaDto createBombaDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bomba = await _bombaService.CreateBombaAsync(createBombaDto);
            return CreatedAtAction(nameof(GetBombaById), new { id = bomba.Id }, bomba);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear la bomba");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Actualiza el estado completo de una bomba
    /// </summary>
    [HttpPut("{id}/estado")]
    public async Task<ActionResult<BombaDto>> UpdateBombaEstado(int id, [FromBody] UpdateBombaEstadoDto updateDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bomba = await _bombaService.UpdateBombaEstadoAsync(id, updateDto);
            return Ok(bomba);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Bomba with ID {id} not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el estado de la bomba con ID {BombaId}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Enciende una bomba específica
    /// </summary>
    [HttpPost("{id}/encender")]
    public async Task<ActionResult<BombaDto>> EncenderBomba(int id)
    {
        try
        {
            var bomba = await _bombaService.EncenderBombaAsync(id);
            _logger.LogInformation("Bomba {BombaId} encendida exitosamente", id);
            return Ok(bomba);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Bomba with ID {id} not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al encender la bomba con ID {BombaId}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Apaga una bomba específica
    /// </summary>
    [HttpPost("{id}/apagar")]
    public async Task<ActionResult<BombaDto>> ApagarBomba(int id)
    {
        try
        {
            var bomba = await _bombaService.ApagarBombaAsync(id);
            _logger.LogInformation("Bomba {BombaId} apagada exitosamente", id);
            return Ok(bomba);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Bomba with ID {id} not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al apagar la bomba con ID {BombaId}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Elimina una bomba
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBomba(int id)
    {
        try
        {
            await _bombaService.DeleteBombaAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Bomba with ID {id} not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar la bomba con ID {BombaId}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene un resumen del estado de todas las bombas
    /// </summary>
    [HttpGet("resumen")]
    public async Task<ActionResult<ApiResponse<BombaEstadoResumenDto>>> GetEstadoResumen()
    {
        try
        {
            var resumen = await _bombaService.GetEstadoResumenAsync();
            var response = ApiResponse<BombaEstadoResumenDto>.SuccessResponse(resumen, "Resumen obtenido exitosamente");
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el resumen de estado");
            var response = ApiResponse<BombaEstadoResumenDto>.ErrorResponse("Error interno del servidor");
            return StatusCode(500, response);
        }
    }

    /// <summary>
    /// Obtiene solo las bombas que están encendidas
    /// </summary>
    [HttpGet("encendidas")]
    public async Task<ActionResult<ApiResponse<IEnumerable<BombaDto>>>> GetBombasEncendidas()
    {
        try
        {
            var bombas = await _bombaService.GetBombasEncendidasAsync();
            var response = ApiResponse<IEnumerable<BombaDto>>.SuccessResponse(bombas, "Bombas encendidas obtenidas exitosamente");
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener las bombas encendidas");
            var response = ApiResponse<IEnumerable<BombaDto>>.ErrorResponse("Error interno del servidor");
            return StatusCode(500, response);
        }
    }

    /// <summary>
    /// Obtiene solo las bombas que están apagadas
    /// </summary>
    [HttpGet("apagadas")]
    public async Task<ActionResult<ApiResponse<IEnumerable<BombaDto>>>> GetBombasApagadas()
    {
        try
        {
            var bombas = await _bombaService.GetBombasApagadasAsync();
            var response = ApiResponse<IEnumerable<BombaDto>>.SuccessResponse(bombas, "Bombas apagadas obtenidas exitosamente");
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener las bombas apagadas");
            var response = ApiResponse<IEnumerable<BombaDto>>.ErrorResponse("Error interno del servidor");
            return StatusCode(500, response);
        }
    }

    /// <summary>
    /// Enciende todas las bombas
    /// </summary>
    [HttpPost("encender-todas")]
    public async Task<ActionResult<ApiResponse<IEnumerable<BombaDto>>>> EncenderTodasLasBombas()
    {
        try
        {
            var bombasActualizadas = await _bombaService.EncenderTodasLasBombasAsync();

            _logger.LogInformation("Se encendieron {Count} bombas", bombasActualizadas.Count());
            var response = ApiResponse<IEnumerable<BombaDto>>.SuccessResponse(bombasActualizadas, $"Se encendieron {bombasActualizadas.Count()} bombas");
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al encender todas las bombas");
            var response = ApiResponse<IEnumerable<BombaDto>>.ErrorResponse("Error interno del servidor");
            return StatusCode(500, response);
        }
    }

    /// <summary>
    /// Apaga todas las bombas
    /// </summary>
    [HttpPost("apagar-todas")]
    public async Task<ActionResult<ApiResponse<IEnumerable<BombaDto>>>> ApagarTodasLasBombas()
    {
        try
        {
            var bombasActualizadas = await _bombaService.ApagarTodasLasBombasAsync();

            _logger.LogInformation("Se apagaron {Count} bombas", bombasActualizadas.Count());
            var response = ApiResponse<IEnumerable<BombaDto>>.SuccessResponse(bombasActualizadas, $"Se apagaron {bombasActualizadas.Count()} bombas");
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al apagar todas las bombas");
            var response = ApiResponse<IEnumerable<BombaDto>>.ErrorResponse("Error interno del servidor");
            return StatusCode(500, response);
        }
    }
}