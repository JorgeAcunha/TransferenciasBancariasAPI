using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TransferenciasBancarias.Services;
using System.Threading.Tasks;
using TransferenciasBancarias.Models;


namespace TransferenciasBancarias.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TransferenciasController : ControllerBase
    {
        private readonly TransferenciaService _service;
        private readonly ILogger<TransferenciasController> _logger;

        public TransferenciasController(TransferenciaService service, ILogger<TransferenciasController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("realizar")]
        public async Task<IActionResult> RealizarTransferencia([FromBody] TransferenciaRequest request)
        {
            _logger.LogInformation($"Intentando realizar transferencia: CuentaOrigen={request.CuentaOrigenId}, CuentaDestino={request.CuentaDestinoId}, Monto={request.Monto}");

            try
            {
                await _service.RealizarTransferenciaAsync(request.CuentaOrigenId, request.CuentaDestinoId, request.Monto);
                _logger.LogInformation("Transferencia realizada con éxito.");
                return Ok(new { mensaje = "Transferencia realizada con éxito." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al realizar la transferencia.");
                return StatusCode(500, new { error = "Error al realizar la transferencia." });
            }
        }

        [HttpGet("cuentas/{id}")]
        public async Task<IActionResult> ObtenerCuenta(int id)
        {
            _logger.LogInformation($"Accediendo al endpoint ObtenerCuenta con id: {id}");

            var cuenta = await _service.ObtenerCuentaAsync(id);
            if (cuenta == null)
            {
                _logger.LogWarning($"No se encontró la cuenta con id: {id}");
                return NotFound();
            }

            _logger.LogInformation($"Cuenta encontrada: Id={cuenta.Id}, Saldo={cuenta.Saldo}");
            return Ok(cuenta);
        }

        [HttpGet("historial/{cuentaId}")]
        public async Task<IActionResult> ObtenerHistorial(int cuentaId)
        {
            _logger.LogInformation($"Obteniendo historial de transferencias para cuentaId: {cuentaId}");

            var historial = await _service.ObtenerHistorialTransferenciasAsync(cuentaId);
            return Ok(historial);
        }
    }
}
