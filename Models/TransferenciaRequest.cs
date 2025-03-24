namespace TransferenciasBancarias.Models
{
    public class TransferenciaRequest
    {
        public int CuentaOrigenId { get; set; }
        public int CuentaDestinoId { get; set; }
        public decimal Monto { get; set; }
    }
}