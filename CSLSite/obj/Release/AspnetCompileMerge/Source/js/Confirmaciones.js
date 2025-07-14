
// Función para mostrar un mensaje de confirmación
function mostrarConfirmacion(mensaje, callback) {
    Swal.fire({
        title: "Confirmación",
        text: mensaje,
        icon: "question",
        iconColor: "#E23B1B",
        showCancelButton: true,
        confirmButtonText: "Sí",
        confirmButtonColor: "#E23B1B",
        cancelButtonText: "No",
        cancelButtonColor: "#E23B1B"
    }).then((result) => {
        if (result.isConfirmed) {
            callback();
        }
    });
}

// Función para mostrar un mensaje de advertencia
function mostrarAdvertencia(mensaje) {
    Swal.fire({
        title: "Advertencia",
        text: mensaje,
        icon: "warning",
        iconColor: "#E23B1B",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#E23B1B"
    });
}

// Función para mostrar un mensaje de error
function mostrarError(mensaje) {
    Swal.fire({
        title: "Error",
        text: mensaje,
        icon: "error",
        iconColor: "#E23B1B",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#E23B1B"
    });
}

// Función para mostrar un mensaje de éxito
function mostrarExito(mensaje, callback) {
    Swal.fire({
        title: "Éxito",
        text: mensaje,
        icon: "success",
        iconColor: "#E23B1B",
        confirmButtonText: "Aceptar",
        confirmButtonColor: "#E23B1B"
    }).then((result) => {
        if (result.isConfirmed) {
            callback();
        }
    });
}
