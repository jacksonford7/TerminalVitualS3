function overlay(control, control2) {
    alert('Estimado Cliente:\nAl cancelar este servicio ud no contará con:\n-Copias de sus documentos\n');
    /*
        if (control2 != null) {
            if (!control2.checked) {
                control.style.visibility = (control.style.visibility == "visible") ? "hidden" : "visible";
            }
        }
        else {
            control.style.visibility = (control.style.visibility == "visible") ? "hidden" : "visible";
        }
*/
}


function comprobarFecha(fechaini, fechafin) {
    //formato dd/MM/yyyy HH:mm
    var parHora = fechaini.split(' ');
    var parXhora = parHora.length == 2 ? parHora[1].split(':') : null;
    var hora = parXhora != null ? parXhora[0] : 0;
    var minuto = parXhora != null ? parXhora[1] : 0;

    var parDate = parHora[0].split('/');
    var dia = parDate[0];
    var mes = parDate[1] - 1;
    var anio = parDate[2];
    var fechaInicial = new Date(anio, mes, dia, hora, minuto, 0, 0);

    parHora = fechafin.split(' ');
    parXhora = parHora.length == 2 ? parHora[1].split(':') : null;
    hora = parXhora != null ? parXhora[0] : 0;
    minuto = parXhora != null ? parXhora[1] : 0;

    parDate = parHora[0].split('/');
    dia = parDate[0];
    mes = parDate[1] - 1;
    anio = parDate[2];
    var fechaFinal = new Date(anio, mes, dia, hora, minuto, 0, 0);

    var rango = fechaInicial <= fechaFinal;
    if (rango) {
        return true;
    }
    return false;
}