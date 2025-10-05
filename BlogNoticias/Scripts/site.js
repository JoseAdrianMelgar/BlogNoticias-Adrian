// Scripts globales para la aplicación BlogNoticias.
// Incluye utilidades mínimas para la navegación responsive y la validación de formularios.
(function () {
    'use strict';

    function toggleMenu() {
        var nav = document.getElementById('main-nav');
        if (!nav) return;
        if (nav.classList.contains('open')) {
            nav.classList.remove('open');
        } else {
            nav.classList.add('open');
        }
    }

    var toggleButton = document.getElementById('nav-toggle');
    if (toggleButton) {
        toggleButton.addEventListener('click', function (e) {
            e.preventDefault();
            toggleMenu();
        });
    }

    var forms = document.querySelectorAll('form.needs-validation');
    Array.prototype.forEach.call(forms, function (form) {
        form.addEventListener('submit', function (event) {
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            }
            form.classList.add('was-validated');
        }, false);
    });
})();
