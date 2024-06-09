using CurrencyBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace CurrencyBackend.Services
{
    [ApiController]
	public class NBPService
	{
		private readonly HttpClient _httpClient;
		public NBPService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		[HttpGet]
		[Route("api/currency")]
		public async Task<IActionResult> GetExchangeRateAsync([FromQuery] string currency, [FromQuery] string date)
		{
			try
			{
				string apiUrl = $"https://api.nbp.pl/api/exchangerates/rates/a/{currency}/{date}?format=json";
				HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

				if (response.IsSuccessStatusCode)
				{
					string jsonResponse = await response.Content.ReadAsStringAsync();
					JObject jsonObject = JObject.Parse(jsonResponse);
					decimal exchangeRate = (decimal)jsonObject["rates"][0]["mid"];
					return Ok(new { exchangeRate });
				}
				else
				{
					return StatusCode((int)response.StatusCode, response.ReasonPhrase ?? "");
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"An error occurred: {ex.Message}");
			}
		}

		[HttpGet]
		[Route("api/tags")]
		public async Task<IActionResult> GetCurrencyTagsAsync()
		{
			try
			{
				string apiUrl = "http://api.nbp.pl/api/exchangerates/tables/A?format=json";
				HttpResponseMessage responseFromNBP = await _httpClient.GetAsync(apiUrl);

				if (responseFromNBP.IsSuccessStatusCode)
				{
					var responseContent = await _httpClient.GetStringAsync("http://api.nbp.pl/api/exchangerates/tables/A?format=json");
					var data = JArray.Parse(responseContent);
					var codes = data[0]["rates"].Select(r => r["code"].Value<string>()).ToList();

					return Ok(new { codes });
				}
				else
				{
					return StatusCode((int)responseFromNBP.StatusCode, responseFromNBP.ReasonPhrase ?? "");
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"An error occurred: {ex.Message}");
			}
		}
		private IActionResult Ok(object value)
		{
			return new OkObjectResult(value);
		}

		private IActionResult StatusCode(int v1, string v2)
		{
			throw new NotImplementedException();
		}

    }
}