window.loginFetch = async (username, password) => {
    const response = await fetch('/auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, password })
    });
    return response.ok;
};


window.logoutFetch = async () => {
    const response = await fetch('/auth/logout', {
        method: 'POST'
    });
    return response.ok;
};

window.getBrowserTimeZone = () => Intl.DateTimeFormat().resolvedOptions().timeZone ?? null;