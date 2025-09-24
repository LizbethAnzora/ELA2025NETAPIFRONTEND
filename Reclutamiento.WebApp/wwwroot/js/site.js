// wwwroot/js/site.js
window.Site = (function () {
    function showToast(message, title = "", type = "info", duration = 4000) {
        // type: info | success | warning | danger
        const colors = {
            info: "#0d6efd",
            success: "#198754",
            warning: "#ffc107",
            danger: "#dc3545"
        };
        const toast = document.createElement('div');
        toast.className = 'toast p-3';
        toast.style.background = '#ffffff';
        toast.style.borderLeft = `6px solid ${colors[type] || colors.info}`;
        toast.style.boxShadow = '0 6px 18px rgba(0,0,0,0.08)';
        toast.style.marginBottom = '8px';
        toast.innerHTML = `<strong style="display:block;margin-bottom:6px">${title}</strong><div>${message}</div>`;
        const containerId = 'site-toast-container';
        let container = document.getElementById(containerId);
        if (!container) {
            container = document.createElement('div');
            container.id = containerId;
            container.className = 'toast-custom';
            document.body.appendChild(container);
        }
        container.appendChild(toast);
        setTimeout(() => {
            toast.style.opacity = 0;
            setTimeout(() => toast.remove(), 400);
        }, duration);
    }

    function confirmAction(message, onConfirm) {
        if (confirm(message)) {
            onConfirm();
        }
    }

    return { showToast, confirmAction };
})();
