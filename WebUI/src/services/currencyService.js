export async function getAllLatestCurrencyData(token, currency) {
    return fetch(`https://localhost:5001/api/currency/ohlc/` + currency, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
        }
    }).then(response => response.json());
}

export async function getLatestCurrencyData(token, currency) {
    return fetch(`https://localhost:5001/api/currency/ohlc/latest/` + currency, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
        }
    }).then(response => response.json());
}