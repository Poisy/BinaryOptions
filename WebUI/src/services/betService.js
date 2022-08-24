export async function getBetPreview(token, currency) {
    return fetch(`https://localhost:5001/api/bet/` + currency, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
        }
    }).then(response => response.json());
}

export async function createBet(token, bet) {
    return fetch(`https://localhost:5001/api/bet`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
        },
        body: JSON.stringify(bet)
    }).then(response => response.json());
}