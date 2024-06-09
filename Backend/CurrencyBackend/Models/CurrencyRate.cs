using System;

namespace CurrencyBackend.Models
{
	public class CurrencyRate
	{
		public int Id { get; set; }
		public string? Currency { get; set; }
		public DateTime Date { get; set; }
		public decimal Rate { get; set; }
	}
}
