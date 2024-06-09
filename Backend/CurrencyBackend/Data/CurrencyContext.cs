using Microsoft.EntityFrameworkCore;
using CurrencyBackend.Models;

namespace CurrencyBackend.Data
{
	public class CurrencyContext(DbContextOptions<CurrencyContext> options) : DbContext(options)
	{
		public DbSet<CurrencyRate> CurrencyRates { get; set; }
		public DbSet<CurrencyCode>? CurrencyCodes { get; set; }

        public class CurrencyCode
        {
            public string? Code { get; internal set; }
        }
    }
}
