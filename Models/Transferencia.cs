using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransferenciasBancarias.Models
{
    public class Transferencia
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CuentaOrigenId { get; set; }

        [Required]
        public int CuentaDestinoId { get; set; }

        [Required]
        public decimal Monto { get; set; }

        [Required]
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        [ForeignKey("CuentaOrigenId")]
        public CuentaBancaria CuentaOrigen { get; set; } = null!;

        [ForeignKey("CuentaDestinoId")]
        public CuentaBancaria CuentaDestino { get; set; } = null!;
    }
}
