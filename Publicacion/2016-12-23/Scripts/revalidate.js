function cedula_validar(control, validador) {
    try {

        var numced = control.value.trim().toUpperCase();
        var verficador = numced.substring(numced.length - 1);
        var validator = document.getElementById(validador);
        if (numced.length <= 0) {
            validator.innerHTML = '<span class="obligado"> OBLIGATORIO!</span>';
            return false;
        }
        //valida el largo de la cadena
        if (numced.length > 0 && numced.length != 10) {
            validator.innerHTML = '<span class="obligado">El valor no corresponde a un No. de CI</span>';
            return false;
        }
        //valida el verficador
        if (isNaN(verficador)) {
            validator.innerHTML = '<span class="obligado"> El verificador [' + verficador + '] es incorrecto!</span>';
            return false;
        }
        verficador = parseInt(numced.substring(numced.length - 1));
        if (parseInt(numced.substring(0, 2)) > 24) {
            validator.innerHTML = '<span class="obligado">No. CI no válido!</span>';
            return false;
        }
        if (parseInt(numced.substring(2, 3)) > 6) {
            validator.innerHTML = '<span class="obligado">No. CI no válido!</span>';
            return false;
        }

        if (numced.length == 10) {

            var array = numced.split("");
            var num = array.length;
            var total = 0;
            var digito = (array[9] * 1);
            for (i = 0; i < (num - 1); i++) {
                var mult = 0;
                if ((i % 2) != 0) {
                    total = total + (array[i] * 1);
                }
                else {
                    mult = array[i] * 2;
                    if (mult > 9)
                        total = total + (mult - 9);
                    else
                        total = total + mult;
                }
            }
            var decena = total / 10;
            decena = Math.floor(decena);
            decena = (decena + 1) * 10;
            var final = (decena - total);

            if (digito != 0) {
                if (final != digito) {
                    validator.innerHTML = '<span class="obligado">No. CI no válido!</span>';
                    return false;
                }
            }
            else {
                if (final != 10) {
                    validator.innerHTML = '<span class="obligado">No. CI no válido!</span>';
                    return false;
                }
            }
        }
        else {
            validator.innerHTML = '<span class="obligado">El valor no corresponde a un No. de CI</span>';
            return false;
        }

        validator.innerHTML = '';
        return true;

    } catch (e) {
        alert(e.Message);
        return false;
    }
}