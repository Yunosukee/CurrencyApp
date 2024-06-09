import { useEffect, useState } from 'react';

interface Currency {
  code: string;
}

interface ExchangeRateResponse {
  exchangeRate: number;
}

const CurrencyRates: React.FC = () => {
  const [currencies, setCurrencies] = useState<Currency[]>([]);
  const [selectedCurrency, setSelectedCurrency] = useState<Currency | null>(null);
  const [date, setDate] = useState<string | null>(null);
  const [exchangeRate, setExchangeRate] = useState<number | null>(null);

  useEffect(() => {
    async function fetchCurrencyCodes() {
      try {
        const response = await fetch('http://localhost:21370/api/tags');
        if (!response.ok) {
          throw new Error(`HTTP error! status: ${response.status}`);
        }
        const data = await response.json();
        const currencyCodes = data.codes.map((code: string) => ({ code }));
        setCurrencies(currencyCodes);
      } catch (error) {
        console.error('Error fetching currencies', error);
      }
    }
    fetchCurrencyCodes();
  }, []);

  const fetchRate = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const response = await fetch(
        `http://localhost:21370/api/currency?currency=${selectedCurrency?.code}&date=${date}`
      );
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      const data: ExchangeRateResponse = await response.json();
      setExchangeRate(data.exchangeRate);
    } catch (error) {
      console.error('Error fetching exchange rate', error);
    }
  };

  return (
    <div>
      <form onSubmit={fetchRate}>
        <select
          value={selectedCurrency?.code || ''}
          onChange={(e) => setSelectedCurrency(currencies.find(c => c.code === e.target.value) || null)}
        >
          <option value="">Select currency</option>
          {currencies.map((currency) => (
            <option key={currency.code} value={currency.code}>
              {currency.code}
            </option>
          ))}
        </select>
        <br></br>
        <input
          type="text"
          value={date || ''}
          onChange={(e) => setDate(e.target.value)}
          placeholder="Date (YYYY-MM-DD)"
        />
        <br></br>
        <button type="submit">Fetch Rate</button>
        <br></br>
      </form>
      {exchangeRate !== null && (
        <ul>
          <li>Currency: {selectedCurrency?.code}</li>
          <li>Date: {date}</li>
          <li>Exchange Rate: {exchangeRate}</li>
        </ul>
      )}
    </div>
  );
};

export default CurrencyRates;