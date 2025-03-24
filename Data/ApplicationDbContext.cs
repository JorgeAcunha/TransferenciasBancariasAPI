using Microsoft.EntityFrameworkCore;
using TransferenciasBancarias.Models;

namespace TransferenciasBancarias.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<CuentaBancaria> CuentasBancarias { get; set; }
        public DbSet<Transferencia> Transferencias { get; set; }
    }
}
