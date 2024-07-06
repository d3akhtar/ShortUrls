const handleGoogleAuth = () => {
    console.log("handleGoogleAuth");
    const callbackUrl = `${window.location.href}`;
    const google_clientId = "774475233264-6lrmnafeqe99eejal4cki9msv1qkcjoq.apps.googleusercontent.com";
    const targetUrl = `https://accounts.google.com/o/oauth2/auth?redirect_uri=${encodeURIComponent(
        callbackUrl
      )}&response_type=token&client_id=${google_clientId}&scope=openid%20email%20profile`;
    window.location.href = targetUrl;
}

export default handleGoogleAuth;