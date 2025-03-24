using System.ComponentModel.DataAnnotations;

namespace TransferenciasBancarias.Models
{
    public class CuentaBancaria
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string NumeroCuenta { get; set; } = string.Empty;

        [Required]
        public string Titular { get; set; } = string.Empty;

        [Required]
        public decimal Saldo { get; set; }
    }
}
