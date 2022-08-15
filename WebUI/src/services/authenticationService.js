export async function registerUser(data) {
    const response = await fetch(`https://localhost:5001/api/auth/register`, {
        method: 'POST',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify(data)
    })

    return response;
}

export async function loginUser(data) {
    const response = await fetch(`https://localhost:5001/api/auth/login`, {
        method: 'POST',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify(data)
    })

    return response;
}

export async function getUser(token) {
    const response = await fetch(`https://localhost:5001/api/user`, {
        method: 'GET',
        headers: {'Content-Type': 'application/json', 'Authorization': 'Bearer '+token}
    })

    return response;
}