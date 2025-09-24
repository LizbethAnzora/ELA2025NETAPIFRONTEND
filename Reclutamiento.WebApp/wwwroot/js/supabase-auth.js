// wwwroot/js/supabase-auth.js
const SupabaseAuth = (function () {
    let client = null;

    function init(url, anonKey) {
        if (!url || !anonKey) {
            console.warn('Supabase no configurado (url/anonKey faltan).');
            return;
        }

        if (!window.supabase) {
            const s = document.createElement('script');
            s.src = 'https://cdn.jsdelivr.net/npm/@supabase/supabase-js/dist/umd/supabase.min.js';
            s.onload = () => {
                client = window.supabase.createClient(url, anonKey);
                window.supabaseClient = client;
            };
            document.head.appendChild(s);
        } else {
            client = window.supabase.createClient(url, anonKey);
            window.supabaseClient = client;
        }
    }

    async function signInWithGithub() {
        if (!client) {
            alert('Espere un momento mientras se inicializa la autenticación.');
            return;
        }
        try {
            const redirectTo = window.location.origin + '/Auth/SupabaseCallback';
            // signInWithOAuth will redirect to provider
            await client.auth.signInWithOAuth({ provider: 'github', options: { redirectTo } });
        } catch (err) {
            console.error('Error al iniciar OAuth:', err);
            alert('No se pudo iniciar sesión con GitHub.');
        }
    }

    async function handleCallbackAndSendToServer(apiEndpoint) {
        if (!client) {
            console.error('Supabase client no inicializado.');
            return;
        }

        try {
            const { data, error } = await client.auth.getSessionFromUrl();
            if (error) {
                console.error('Error al obtener sesión desde URL:', error);
                return;
            }

            const session = data?.session;
            const user = session?.user;
            const githubId = user?.id;
            if (!githubId) {
                console.error('No se obtuvo el id de GitHub del usuario.');
                return;
            }

            const res = await fetch(apiEndpoint, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ githubId })
            });

            if (res.ok) {
                // El backend guarda la sesión del frontend en server-side (Session)
                // Recargamos para que la UI se actualice según sesión del servidor
                window.location.href = '/';
            } else {
                console.error('Error al enviar githubId al servidor:', res.statusText);
                alert('Ocurrió un error al conectar con el servidor.');
            }

        } catch (err) {
            console.error('Excepción en handler de callback:', err);
        }
    }

    return { init, signInWithGithub, handleCallbackAndSendToServer };
})();
