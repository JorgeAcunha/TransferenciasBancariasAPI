using Microsoft.EntityFrameworkCore;
using TransferenciasBancarias.Models;
using TransferenciasBancarias.Data;

namespace TransferenciasBancarias.Services
{
    public class TransferenciaService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TransferenciaService> _logger;

        public TransferenciaService(ApplicationDbContext context, ILogger<TransferenciaService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Realizar una transferencia
        public async Task RealizarTransferenciaAsync(int cuentaOrigenId, int cuentaDestinoId, decimal monto)
        {
            // Buscar la cuenta origen
            var cuentaOrigen = await _context.CuentasBancarias
                .FirstOrDefaultAsync(c => c.Id == cuentaOrigenId);

            if (cuentaOrigen == null)
                throw new Exception("Cuenta origen no encontrada.");

            // Buscar la cuenta destino
            var cuentaDestino = await _context.CuentasBancarias
                .FirstOrDefaultAsync(c => c.Id == cuentaDestinoId);

            if (cuentaDestino == null)
                throw new Exception("Cuenta destino no encontrada.");

            // Validar si existe saldo suficiente
            if (cuentaOrigen.Saldo < monto)
                throw new Exception("Saldo insuficiente.");

            // Actualizar los saldos
            cuentaOrigen.Saldo -= monto;
            cuentaDestino.Saldo += monto;

            // Registrar la transferencia
            var transferencia = new Transferencia
            {
                CuentaOrigenId = cuentaOrigenId,
                CuentaDestinoId = cuentaDestinoId,
                Monto = monto,
                Fecha = DateTime.UtcNow
            };

            _context.Transferencias.Add(transferencia);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Transferencia realizada: {monto} desde cuenta {cuentaOrigenId} a cuenta {cuentaDestinoId}");
        }

        // Consultar estado de cuenta
        public async Task<CuentaBancaria> ObtenerCuentaAsync(int cuentaId)
        {
            var cuenta = await _context.CuentasBancarias.FindAsync(cuentaId);

            if (cuenta == null)
                throw new Exception("Cuenta no encontrada.");

            return cuenta;
        }

        // Consultar el historial de transferencias
        public async Task<List<Transferencia>> ObtenerHistorialTransferenciasAsync(int cuentaId)
{
    return await _context.Transferencias
        .Include(t => t.CuentaOrigen)
        .Include(t => t.CuentaDestino)
        .Where(t => t.CuentaOrigenId == cuentaId || t.CuentaDestinoId == cuentaId)
        .OrderByDescending(t => t.Fecha)
        .ToListAsync();
}
    }
}
