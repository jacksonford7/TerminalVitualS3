function soloLetras(e, caracteres, espacios) {
    key = e.keyCode || e.which;
    tecla = String.fromCharCode(key).toLowerCase();
    if (caracteres) {
        letras = caracteres;
    }
    else {
        letras = " áéíóúabcdefghijklmnñopqrstuvwxyz";
    }
    if (espacios == undefined || espacios == null) {
        especiales = [8, 13, 32,  9, 16, 20];
    }
    else {
           especiales = [8, 13,  9, 16, 20];
    }
    tecla_especial = false
    for (var i in especiales) {
        if (key == especiales[i]) {
            tecla_especial = true;
            break;
        }
    }
    if (letras.indexOf(tecla) == -1 && !tecla_especial) {
        return false;
    }
}

/*
onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_-$')"
*/

//Validador de cedula ecuatoriana
function cedValidate(control, validador) {
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
            validator.innerHTML = '<span class="obligado">Mínimo 10 caracteres!</span>';
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
        var coeficientes = [2, 1, 2, 1, 2, 1, 2, 1, 2];
        var cedarr = numced.split("");
        var sumador = 0;
        for (i = 0; i <= coeficientes.length - 1; i++) {
            var sumando = coeficientes[i] * parseInt(cedarr[i]);
            if (sumando >= 10) {
                sumando = sumando - 9;
            }
            sumador = sumador + sumando;
        }
        var decena = ((parseInt(sumador / 10) + 1) * 10) - sumador;
        if (verficador != decena) {
            validator.innerHTML = '<span class="obligado"> No. CI no válido</span>';
            return false;
        }
        validator.innerHTML = '';
        return true;

    } catch (e) {
        alert(e.Message);
        return false;
    }
}


//iso validador del contenedor
function isoValidate(control, validador, opcional) {
    var numcont = control.value.trim().toUpperCase();
    var verficador = numcont.substring(numcont.length - 1);
    var validator = document.getElementById(validador);

    //Opcional no vino, opñcional es nulo opcional es falso
    if (opcional == undefined || opcional == null || opcional == false) {
        if (numcont.trim().length <= 0) {
            validator.innerHTML = '* opcional';
            return true;
        }
    }
    if (numcont.length <= 0) {
        validator.innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
        return false;
    }
    //valida el largo de la cadena
    if (numcont.length > 0 && numcont.length != 11) {
        validator.innerHTML = '<span class="obligado">El número debe tener 11 caracteres!</span>';
        return false;
    }
    //cadena básica
    //\w{4}\d{7}
    expr = /[a-zA-Z]{4}\d{7}/;
    if (!expr.test(numcont)) {
        validator.innerHTML = '<span class="obligado">El número de contenedor no es estándar!</span>';
        return false;
    }


    //valida el verficador
    if (isNaN(verficador)) {
        validator.innerHTML = '<span class="obligado">El verificador [' + verficador + '] incorrecto!</span>';
        return false;
    }
    try {
        verficador = parseInt(numcont.substring(numcont.length - 1));
        var leters = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"];
        var values = [10, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 34, 35, 36, 37, 38];
        validador.innerHTML = '';
        var potencia = 0;
        var xchar = numcont.split("");
        if (!isNaN(xchar[0]) || !isNaN(xchar[1]) || !isNaN(xchar[2])) {
            validator.innerHTML = '<span class="obligado">Propietario inválido</span>';
            return;
        }
        if (xchar[3].toLowerCase() != 'u' && xchar[3].toLowerCase() != 'z' && xchar[3].toLowerCase() != 'j') {
            validator.innerHTML = '<span class="obligado">Tipo de equipo inválido</span>';
            return;
        }
        var sumador = 0;
        for (i = 0; i <= xchar.length - 2; i++) {
            if (isNaN(xchar[i])) {
                sumador = sumador + (parseInt(values[leters.indexOf(xchar[i])]) * Math.pow(2, potencia));
            }
            else {
                sumador = sumador + (parseInt(xchar[i]) * Math.pow(2, potencia))
            }
            potencia++;
        }
        var nuevo = parseInt(sumador / 11) * 11;
        var original = parseInt(sumador)
        var result = verficador - (original - nuevo);
    }
    catch (e) {
        return alert('Se produjo un error: ' + e.Message);
        return false;
    }
    if (result != 0) {
        validator.innerHTML = '<span class="obligado">Siglas no válidas [ISO] !</span>';
        return false;
    }
    else {
        validator.innerHTML = '';
        return true;
    }
    return false;
}

//uso del foco multinavegador
function foco(elemento) {
    window.setTimeout(function () { document.getElementById(elemento).focus(); }, 0);
}

//validador de largo de cadena
function cadenareqerida(caja, min, max, validator) {
    var xcaja = document.getElementById(caja.id);
    var mens = document.getElementById(validator);
    try {
        mens.innerHTML = '';
        if (xcaja == undefined) {
            alert('El control:[ ' + caja.id + '] no existe');
            return false;
        }
        var n = xcaja.value.trim().length
        if (n == 0 && min == 0) {
            mens.innerHTML = ' * opcional';
            return;
        }

        if (n == 0) {
            mens.innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
            return false;
        }
        if (n >= min && n <= max) {
            mens.innerHTML = '';
            return true;
        }
        else {
            mens.innerHTML = '<span class="obligado">Mínimo ' + min + ' carateres!</span>';
            return false;
        }
        return false;

    } catch (e) {
        alert(e.Message);
        return false;
    }

}

//pide caja origen, desde, hasta, span del mensaje, return false cancela envio del frm
function valrange(caja, desde, hasta, validator, requerido) {
    var xcaja = document.getElementById(caja.id);
    var mens = document.getElementById(validator);
    try {
        var xdesde = 0;var xhasta = 0;
        mens.innerHTML = '';
        if (xcaja == undefined) {
            alert('El control:[ ' + caja.id + '] no existe');
            return false;
        }
        if (requerido == null || requerido == undefined || requerido == false) {
            if (xcaja.value.trim().length <= 0) {
                mens.innerHTML = '* opcional';
                return true;
            }
        }
        if (xcaja.value.trim().length <= 0) {
            mens.innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
            return false;
        }
        if (xcaja != undefined && xcaja.value.length > 0) {
            var valor = xcaja.value.trim();
            //valida q sea un número
            if (isNaN(valor)) {
                mens.innerHTML = '<span class="obligado">Número inválido!</span>';
                return false;
            }
            //paso un valor directo[0-9]
            if (isNaN(desde)) {
                //revisar si es un control.
                if (desde.value != undefined) {
                    desde = desde.value;
                }
                if (desde.textContent != undefined) {
                    desde = desde.textContent;
                }
                if (isNaN(desde)) {
                    desde = 0;
                }
         
            }
            if (isNaN(hasta)) {
                //revisar si es un control.
                if (hasta.value != undefined) {
                    hasta = hasta.value;
                }
                if (hasta.textContent != undefined) {
                    hasta = hasta.textContent;
                }
                if (isNaN(hasta)) {
                    hasta = 0;
                }
            }
            valor = parseFloat(valor);
            if (valor >= parseFloat(desde) && valor <= parseFloat(hasta)) {
                return true;
            }
            else {
                if (parseFloat(desde) == parseFloat(hasta)) {
                    return true;
                }
                mens.innerHTML = '<span class="obligado">Valores de: ' + desde + ' a ' + hasta + '</span>';
                return false;
            }
        }
        return false;
    } catch (e) {
        alert(e.Message);
        return false;
    }
}

//Validar mail->Mover
function validarEmail(email) {
    expr = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!expr.test(email)) {
        return false;
    }
    return true;
}

//Validar varios mail->Mover
function validarVariosEmail(email) {
    expr = /^([a-zA-Z0-9@;_\.\-])+\@(([a-zA-Z0-9@;\-])+\.)+([a-zA-Z0-9@;]{2,4})+$/; ;
    if (!expr.test(email)) {
        return false;
    }
    return true;
}

//validar una fecha
function valDate(caja, requerido, validator) {
    try {
        var xcaja = document.getElementById(caja.id);
        if (xcaja == undefined) {
            alert('El control:[ ' + xcaja + '] no existe');
            return false;
        }
        if (requerido) {
            if (xcaja.value.trim().length <= 0) {
                validator.innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
                return false;
            }
        }
        var datetime = xcaja.value.split(' ');
        if (datetime.length <= 0) {
            validator.innerHTML = '<span class="obligado">Obligatorio!</span>';
            return false;
        }
        expr = /^((((0[1-9])|([1-2][0-9])|(3[0-1]))|([1-9]))\x2F(((0[1-9])|(1[0-2]))|([1-9]))\x2F(([0-9]{2})|(((19)|([2]([0]{1})))([0-9]{2}))))$/;

        if (xcaja.value.trim().length && !expr.test(datetime[0])) {
             validator.innerHTML = '<span class="obligado">Fecha incorrecta!(dia/mes/año)</span>';
             return false;
         }
         if (datetime.length > 1) {
             expr2 = /^([01]?[0-9]|2[0-3]):[0-5][0-9]$/;
             if (!expr2.test(datetime[1])) {
                 validator.innerHTML = '<span class="obligado">Hora incorrecta!(hh:mm)</span>';
                 return false;
             }
         }
        validator.innerHTML = '';
        return true;
    } catch (e) {
        alert(e.Message);
        return false;
    }
}

//formateo de fechas
function formattedDate(date,dmy) {
    var d = new Date(date || Date.now()),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();
    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;
    var h = d.getHours().toString();
    if (h.length < 2) h = '0' + h;
    var m = d.getMinutes().toString();
    if (m.length < 2) m = '0' + m;
    if (dmy != undefined && dmy != null && dmy == true) {
        return [day, month, year].join('/') + ' ' + h + ':' + m;
    }
  return [year, month, day].join('/') +' '+ h+':'+ m;
}

//validar dropdown
function rulevalidate(control, validador) {
    if (control.value != 0) {
        validador.innerHTML = '';
        return;
    }
    validador.innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
    return;
}

//valida un control dropdow cuando cambia
function valdpme(control, valor, validador) {
    if (control.value != valor) {
        validador.innerHTML = '';
        return;
    }
    validador.innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
    return;
}
//unicamente le pone si/no a un ckbox
function cksi(control, label) {
    if (control.checked) {
        label.textContent = '[SI]';
        return;
    }
    label.textContent = '[NO]';
    return;
}

//cuando cambie el año o el mes inactivar días del mes->para combos
function inactiveDays(anual, mensual, dia) {
    var anio = anual.value;
    var mes = mensual.value;
    for (i = 0; i <= 30; i++) {
        dia.children[i].disabled = false;
        dia.children[i].style.backgroundColor = 'inherit';
    }
    //selecciona el primer item..
    dia.children[0].selected = true;
    //obtiene el mes siguiente.
    var fecha = new Date(anio, mes, 1, 0, 0);
    //el ultimo día del mes actual.
    fecha.setDate(fecha.getDate() - 1);
    //tengo el día límite
    var dialimite = fecha.getDate();
    //apaga los que estan fuera de rango
    for (i = dialimite; i <= 30; i++) {
        dia.children[i].disabled = true;
        dia.children[i].style.backgroundColor = '#CCC';
    }
}

function conver(control, convierte) {
    if (control.value.trim().length <= 0) {
        convierte.textContent = '...';
        return;
    }
    if (!isNaN(control.value.trim())) {
        convierte.textContent = (parseFloat(control.value) * 0.001).toFixed(2) + ' Toneladas';
        return;
    }
    return;
}
//para mensajes mientras escribe
function msgfinder(control, expresa, prevmsg) {
    if (control.value.trim().length <= 0) {
        document.getElementById(expresa).textContent = prevmsg;
        return;
    }
    document.getElementById(expresa).textContent = 'Se buscará [' + control.value.toUpperCase() + '], presione el botón';
}

//calcular horas
function calcular(v1, v2) {
    horas1 = v1.split(":"); /*Mediante la función split separamos el string por ":" y lo convertimos en array. */
    horas2 = v2.split(":");
    horatotale = new Array();
    for (a = 0; a < 3; a++) /*bucle para tratar la hora, los minutos y los segundos*/
    {
        horas1[a] = (isNaN(parseInt(horas1[a]))) ? 0 : parseInt(horas1[a]) /*si horas1[a] es NaN lo convertimos a 0, sino convertimos el valor en entero*/
        horas2[a] = (isNaN(parseInt(horas2[a]))) ? 0 : parseInt(horas2[a])
        horatotale[a] = (horas1[a] - horas2[a]); /* insertamos la resta dentro del array horatotale[a].*/
    }
    horatotal = new Date()  /*Instanciamos horatotal con la clase Date de javascript para manipular las horas*/
    horatotal.setHours(horatotale[0]); /* En horatotal insertamos las horas, minutos y segundos calculados en el bucle*/
    horatotal.setMinutes(horatotale[1]);
    horatotal.setSeconds(horatotale[2]);
    return horatotal.getHours() + ":" + horatotal.getMinutes() + ":" +
horatotal.getSeconds();
    /*Devolvemos el valor calculado en el formato hh:mm:ss*/
}

//limpiar el formulario
function resetForm(forma, all) {
    if (all) {
        //reset al text;
        forma.reset();
        //reset object
        this.jAisv = {};
        //reset al cajas
        $('span.caja').text('...');
        $('span.validacion').text('* obligatorio');
        $('span.opcional').text('* opcional');
        //clean all hiidenfileds
        $("input[type='hidden']").val('');
    }
}
//file
function ValidateFile(control, validador) {
    try {

        control = document.getElementById(control);
        var b = validador == null || validador == undefined;
        if (control == undefined || control == null) {
            if (b) {
                alert('El input no está definido');
                return false;
            }
            validador.innerHTML = '<span class="obligado">Control sin definir!!</span>';
            return false;
        }

        var atributo = control.getAttribute('accept');
        if (atributo == undefined || atributo == null || atributo.trim().length <= 0) {
            atributo = 'txt';
        }
        var fileName = control.value;
        if (fileName.length <= 0) {
            if (b) {
                alert('Por favor seleccione el archivo a procesar!');
                return false;
            }
            validador.innerHTML = '<span class="obligado">Seleccione el archivo a procesar</span>';
            return false;
        }

        if (!(fileName.lastIndexOf(atributo) == fileName.length - 3)) {
            if (b) {
                alert('El archivo debe ser de tipo '+atributo);
                return false;
            }
            validador.innerHTML = '<span class="obligado">El archivo debe ser de tipo ['+atributo+']</span>';
            return false
        }
        if (!b) {
            validador.textContent = '';
            validador.innerHTML = '';
        }

        return true;
    }
    catch (e) {
        alert(e.Message);
        return false;
    }
}
//elecciona tabla
function SelectContent(el) {
    var elemToSelect = document.getElementById(el);
    if (window.getSelection) {  // all browsers, except IE before version 9
        var selection = window.getSelection();
        var rangeToSelect = document.createRange();
        rangeToSelect.selectNodeContents(elemToSelect);
        selection.removeAllRanges();
        selection.addRange(rangeToSelect);
    }
    else       // Internet Explorer before version 9
        if (document.body.createTextRange) {    // Internet Explorer
            var rangeToSelect = document.body.createTextRange();
            rangeToSelect.moveToElementText(elemToSelect);
            rangeToSelect.select();
        }
        else if (document.createRange && window.getSelection) {
            range = document.createRange();
            range.selectNodeContents(el);
            sel = window.getSelection();
            sel.removeAllRanges();
            sel.addRange(range);
        }
    }



    /*fx cambia la plantilla de los texbox para la dae*/
    function adudoc(control,d1,d2,d3,ms) {
        try {
            var dae1 = document.getElementById(d1);
            var dae2 = document.getElementById(d2);
            var dae3 = document.getElementById(d3);
            var hoy = new Date();
            //dpdoc
            var txt = document.getElementById(control.id).value;
            switch (txt) {
                case 'DAE':
                    //dae
                    dae1.disabled = false;
                    dae2.disabled = false;
                    dae1.value = '024';
                    dae2.value = hoy.getFullYear();
                    dae3.value = '40';
                    dae3.setAttribute('maxlength', 17);
                    document.getElementById(ms).textContent = 'D.A.E: Declaración Aduanera de Exportación';
                    break;
                case 'DAS':
                    //das
                    dae1.disabled = false;
                    dae2.disabled = false;
                    dae1.value = '024';
                    dae2.value = hoy.getFullYear();
                    dae3.value = '83';
                    document.getElementById(ms).textContent = 'D.A.S: Declaración Aduanera Simplificada';
                    dae3.setAttribute('maxlength', 20);
                    break;
                case 'DJT':
                    dae1.disabled = false;
                    dae2.disabled = false;
                    dae1.value = '073';
                    dae2.value = hoy.getFullYear();
                    dae3.value = '00';
                    document.getElementById(ms).textContent = 'D.J.T: Declaración Juramentada de Turista';
                    dae3.setAttribute('maxlength', 15);
                    break;
                case 'TRS':
                    //djt
                    dae1.disabled = true;
                    dae2.disabled = true;
                    dae1.value = '';
                    dae2.value = '';
                    dae3.value = '050';
                    document.getElementById(ms).textContent = 'T.R.S: Solicitud de Traslado';
                    dae3.setAttribute('maxlength', 20);
                    break;
                default:
                    //valor default
                    dae1.disabled = false;
                    dae2.disabled = false;
                    dae1.value = '024';
                    dae2.value = hoy.getFullYear();
                    dae3.value = '40';
                    document.getElementById(ms).textContent = 'Seleccione documento';
                    dae3.setAttribute('maxlength', 17);
                    break;
            }
        } catch (e) {
            alert(e.Message);
        }

    }
    function validateDatesRange(desde, hasta,ctrl) {
        var desde = document.getElementById(desde);
        var hasta = document.getElementById(hasta);
        if (desde == undefined || desde == null || hasta == undefined || hasta == null) {
            alert('Por favor agregue o revise las fechas desde/hasta');
            return false;
        }

        var datePart = desde.value.split('/');
        var date = new Date(datePart[1] + '/' + datePart[0] + '/' + datePart[2]);
        if (!date instanceof Date || isNaN(date.valueOf())) {
            alert('Por favor revise/escriba la fecha desde');
            return false;
        }
        var datePart2 = hasta.value.split('/');
        var date2 = new Date(datePart2[1] + '/' + datePart2[0] + '/' + datePart2[2]);
        if (!date2 instanceof Date || isNaN(date2.valueOf())) {
            alert('Por favor revise/escriba la fecha hasta');
            return false;
        }
        if (date > date2) {
            alert('El rango de fechas está incorrecto, por favor revise');
            return false;

        }
        if ((date.getFullYear() - date2.getFullYear()) != 0) {
            alert('El rango máximo de registros es de 1 año');
            return false;
        }

        var mini = parseInt(datePart[1]);  var mfin = parseInt(datePart2[1]);
        if (mini != mfin) {
            alert('Solo puede solicitar documentos en rango de 1 mes, gracias por entender');
            return false;
        }

        if (ctrl != null && ctrl != undefined && ctrl.trim().length > 0) {
            document.getElementById(ctrl).innerHTML = '<img alt="" src="../shared/imgs/loader.gif">'
        }
        return true;
    }

    //valida que tenga seleccionado linea y exportador antes del booking
    function linkbokin(clinea, cexport, tipo) {
        try {
            var linea = document.getElementById(clinea).value;
            var exportador = document.getElementById(cexport).value;
            if (exportador == null || exportador == undefined || exportador == '') {
                alert(' * Por favor escoja el exportador *');
                return;
            }
            if (linea == null || linea == undefined || linea == '') {
                alert(' * Por favor escoja la línea *');
                return;
            }
            var w = window.open('../catalogo/booking.aspx?linea=' + linea + '&tipo=' + tipo, 'Bookings', 'width=850,height=880');
            w.focus();

        } catch (e) {
            alert(e.Message);
        }
    }

    //Validar mail->Mover
    function validarPlaca(caja, validador) {
        var xcaja = document.getElementById(caja.id);
        var mens = document.getElementById(validador);
        mens.innerHTML = '';
        if (xcaja == undefined) {
            alert('El control:[ ' + caja.id + '] no existe');
            return false;
        }
        var n = xcaja.value.trim().length
        if (n == 0) {
            mens.innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
            return false;
        }
        expr = /[a-zA-Z]{3}\d{4}/;
        if (!expr.test(xcaja.value.trim())) {
            mens.innerHTML = '<span class="obligado">El número de placa no es estándar</span>';
            return false;
        }
        return true;
    }
    function getfile(file) {
        var iframe = document.createElement("iframe");
        iframe.src = "../handler/exporter.ashx?file=" + file;
        iframe.style.display = "none";
        document.body.appendChild(iframe);

    }
    function getTable(file) {
        var iframe = document.createElement("iframe");
        iframe.src = "../handler/filer.ashx?file=" + file;
        iframe.style.display = "none";
        document.body.appendChild(iframe);
    }
    function maildata(control, destino) {
        if (control.value.trim().length > 0) {
            if (!validarEmail(control.value)) {
                document.getElementById(destino).innerHTML = '<span class="obligado">Mail no parece correcto</span>';
                return false;
            }
        }
        document.getElementById(destino).innerHTML = '';
        return true;
    }
    function maildatavarios(control, destino) {
        if (control.value.trim().length > 0) {
            if (!validarVariosEmail(control.value)) {
                document.getElementById(destino).innerHTML = '<span class="obligado">Mail no parece correcto</span>';
                return false;
            }
        }
        document.getElementById(destino).innerHTML = '';
        return true;
    }
    function ckcupos(control) {
        var txt = control.checked;
        if (txt) {
            document.getElementById('dpins').disabled = false;
            document.getElementById('dprule').disabled = false;
            $('#rules').removeClass('disable');
            document.getElementById('mrule').innerHTML = '';
        }
        else {
            $('#dpins option:last').attr('selected', 'selected')
            $('#dprule option:last').attr('selected', 'selected')
            document.getElementById('dpins').disabled = true;
            document.getElementById('dprule').disabled = true;
            document.getElementById('dprule').innerHTML = '';
            var option = document.createElement("option");
            option.text = 'Regla';
            option.value = '0';
            document.getElementById('dprule').add(option);
            $('#rules').addClass('disable');
            document.getElementById('mrule').innerHTML = '';
        }
    }
    //Cargar las reglas de modo dinámico
    function loadrule(result) {
        //quito los options que pudiera tener previamente el combo
        $("#dprule").html("");
        //recorro cada item que devuelve el servicio web y lo añado como un opcion
        $.each(result.d, function () {
            $("#dprule").append($("<option></option>").attr("value", this.clave).text(this.valor))
        });
    }
    //Cargar los cantones de modo dinámico
    function loadcanton(result) {
        //quito los options que pudiera tener previamente el combo
        $("#dcanton").html("");
        //recorro cada item que devuelve el servicio web y lo añado como un opcion
        $.each(result.d, function () {
            $("#dcanton").append($("<option></option>").attr("value", this.clave).text(this.valor))
        });
    }


    //bloquea y libera los campos de exceso.
    function radio(control, control2) {
        var txt = control.checked;
        if (control2 != null && control2 != undefined) {
            control2.checked = txt;
        }
        if (txt) {
            $('.xalter').html('<span class="obligado">OBLIGATORIO!</span>');
            $(".xact").prop('disabled', false);
        }
        else {
            $('.xalter').text('');
            $(".xact").prop('disabled', true);
            $(".xact").val('0');
        }
    }

    //llamada al servicecatalog
    function invoke(filtro, catalogo, funcion) {
        var params = {};
        params.catalogID = catalogo;
        params.filtro = document.getElementById(filtro.id).value;
        params = JSON.stringify(params);
        $.ajax({
            type: "POST",
            url: "../services/catalogo.asmx/GetDPCatalog",
            data: params,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: funcion,
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus + ": " + XMLHttpRequest.responseText);
            }
        });
    }

    //esta función setea los valores antes del postback
    function setmails() {
        try {
            document.getElementById('valmail').innerHTML = '';
            var div = $('#TextBoxesGroup input[type="text"]');
            /*Si existe el div contenedor*/
            if (div.length) {
                for (i = 1; i <= 5; i++) {
                    var txt = this.document.getElementById('textbox' + i);
                    /*Si el elemento existe y su texto no es cadena vacía*/
                    if (txt != null && txt != undefined) {
                        if (i == 1 && txt.value.trim().length <= 0) {
                            document.getElementById('valmail').innerHTML = '<span class="obligado">REQUERIDO!</span>';
                            return false;
                        }
                        if (txt.value.trim().length > 0) {
                            //validar mail->return
                            if (!validarEmail(txt.value.trim())) {
                                txt.className = 'mailerror';
                                document.getElementById('valmail').innerHTML = '<span class="obligado">Existe un mail incorrecto, verifique</span>';
                                return false;
                            }
                            else {
                                txt.className = '';
                            }
                            if (i == 1) { this.jAisv.mail1 = txt.value.trim(); }
                            else if (i == 2) { this.jAisv.mail2 = txt.value.trim(); }
                            else if (i == 3) { this.jAisv.mail3 = txt.value.trim(); }
                            else if (i == 4) { this.jAisv.mail4 = txt.value.trim(); }
                            else if (i == 5) { this.jAisv.mail5 = txt.value.trim(); }
                        }
                    }
                }
                return true;
            }
        } catch (e) {
            alert(e.Message);
            return false;
        }
    }

    function processOk(response) {
        var jmsg = JSON.parse(response.d);
        if (!jmsg.resultado) {
            alert(jmsg.mensaje);
            if (jmsg.fluir) {
                eval(jmsg.data);
            }
            document.getElementById("loader").className = 'nover';
            document.getElementById('btsalvar').disabled = false;
            return;
        }
        else {
            alert(jmsg.mensaje);
            alert(' * Se va a imprimir el AISV No. [' + jmsg.data + '] * ');
            window.open("printaisv.aspx?sid=" + jmsg.mensaje + "", "Imprimir", "width=850,height=700, scrollbars=yes");
        }
        document.getElementById("loader").className = 'nover';
        document.getElementById('btsalvar').disabled = false;
    }

    //se encarga de setear la zona reefer
    function setRefer(estado) {
        estado = !estado;
        document.getElementById('valtemp').innerHTML = '';
        document.getElementById('valrefri').innerHTML = '';
         document.getElementById('txttemp').innerHTML = '';
        document.getElementById('dprefrigera').disabled = estado;
        document.getElementById('txttemp').disabled = estado;
        document.getElementById('dphumedad').disabled = estado;
        document.getElementById('dpventila').disabled = estado;
        if (!estado) {
              document.getElementById('valtemp').innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
            document.getElementById('valrefri').innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
            document.getElementById('dprefrigera').children[document.getElementById('dprefrigera').length - 1].disabled = true;
            document.getElementById('dprefrigera').children[document.getElementById('dprefrigera').length - 1].style.backgroundColor = '#FFE13C';
            document.getElementById('dprefrigera').selectedIndex = "1";
            document.getElementById('tseal2').disabled = estado;
            return;
        }
        $('#dprefrigera option:last').attr('selected', 'selected')
        $('#dphumedad option:last').attr('selected', 'selected')
        $('#dpventila option:last').attr('selected', 'selected')
    }

    function fullreset(form, all) {
        var nomant = document.getElementById('nomexpo').textContent;
        var numant = document.getElementById('numexport').value;
        resetForm(form, all);
        setRefer(false);
        radio(document.getElementById('ckdimen'));
        ckcupos(document.getElementById('ckcupo'));
        document.getElementById('nomexpo').textContent = nomant;
        document.getElementById('numexport').value = numant
        document.getElementById('numexpo').textContent = numant;
    }

    function getPrint(jAisv,url) {
        if (jAisv.bnumber == undefined || jAisv.bnumber.trim().length <= 0) {
            alert('Por favor seleccione el Booking');
            window.scrollTo(0, 0);
            document.getElementById("loader").className = 'nover';
            return;
        }
        if (setmails()) {
            document.getElementById("loader").className = '';
            document.getElementById('btsalvar').disabled = true;
            prepareObject();
            invokeJsonTransport(jAisv,url);
        }
        else {
            alert('Por favor revise los emails');
        }
    }
    function ShowProgress() {
        setTimeout(function () {
            var modal = $('<div />');
            modal.addClass("dmodal");
            $('body').append(modal);
            var loading = $(".loading");
            loading.show();
            var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
            var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
            loading.css({ top: top, left: left });
        }, 200);
    }
    function invokeJsonTransport(objeto,url) {
        $.ajax({
            type: "POST",
            url: url,
            data: "{'objeto':" + JSON.stringify(objeto) + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (response) { alert(response); },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus + ": " + XMLHttpRequest.responseText);
                document.getElementById("loader").className = 'nover';
                document.getElementById('btsalvar').disabled = false;
            }
        });
    }
    //esta funcion crea una ventana popup dinamicamente
    function CreateWindow(HTMLstring, alto, ancho) {
        var dimensiones = "width=" + ancho + ", height=" + alto;
        newwindow = window.open("", "", dimensiones);
        newdocument = newwindow.document;
        newdocument.write(HTMLstring);
    }

    //valida un control dropdow cuando cambia
    function valdpmeref(control, valor, validador, mensajero, obligar) {
        switch (control.value) {
            case '01':
                mensajero.textContent = 'Unidad Reefer Normal de Carga Perecible (30°C a -9.9°C)  y/o Congelada (-10°C a -35°C)';
                validador.textContent = '';
                break;
            case '02':
                mensajero.textContent = 'Unidad de enfriamiento mediante agua, que se aplica durante el viaje abordo';
                validador.textContent = '';
            break;
        case '03':
            mensajero.textContent = 'Unidad que llega a una temperatura de -60°C, usa dos compresores';
            validador.textContent = '';
            break;
        case '04':
            mensajero.textContent = 'Unidad que permite manejar los niveles de O2 y CO2 para que no madure durante el viaje';
            validador.textContent = '';
            break;
        default:
            mensajero.textContent = 'No requiere refrigeración';
            validador.textContent = 'OBLIGATORIO!';
            break;
    }
    if (obligar != undefined && obligar != null && obligar == 1) {
        if (control.value != valor) {
            validador.innerHTML = '';
            return;
        }
        validador.innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
        return;
    }
}

function addrow() {
    var ttara = document.getElementById('tara').textContent;
    if (ttara.trim().length <= 0 || parseFloat(ttara) <=0 ) {
        alert('Por favor seleccione el Booking');return;
    }

    if (xrowcounter > 30) {
        alert('El máximo de documentos que se pueden consolidar es 30 por contenedor, ha llegado al límite');
        return;
    }
    var dae1 = document.getElementById('txtdae1');
    var dae2 = document.getElementById('txtdae2');
    var dae3 = document.getElementById('txtdae3');
    //si no hay daes
    if (dae1 == null || dae2 == null || dae3 == null || dae1 == undefined || dae2 == undefined || dae3 == undefined) {
        alert('Escriba el número de documento');
        return;
    }
    var documento = dae1.value.trim() + dae2.value.trim() + dae3.value.trim();
    for (var i = 0; i < lista.length; i++) {
        if (lista[i].adudoc == documento) {
            alert('El número de documento ya está registrado');
            return;
        }
    }
    //bultos
    dae1 = document.getElementById('txtbultos');
    if (dae1.value.trim().length <= 0 || isNaN(dae1.value) || parseInt(dae1.value) <= 0) {
        alert('Por favor escriba la cantidad de items');
        return;
    }
    //pesoc
    dae2 = document.getElementById('pesoc');
    if (dae2.value.trim().length <= 0 || isNaN(dae2.value) || parseFloat(dae2.value) <= 0) {
        alert('Por favor agregue un valor de peso válido');
        return;
    }
    //package
    dae3 = document.getElementById('dpembala');
    if (dae3.value.trim().length <= 0 || isNaN(dae3.value) || parseInt(dae3.value) <= 0) {
        alert('Pr favor escoja el tipo de embalaje de la lista');
        return;
    }
    var tpd = document.getElementById('dpdoc').value;
    var dco = document.getElementById('dpdoc');
    var table = this.document.getElementById('daes');
    var row = table.insertRow(-1);
    row.className = "point";
    var cell0 = row.insertCell(0); //NUMERO
    var cell1 = row.insertCell(1); //DOCUMENTO
    var cell2 = row.insertCell(2); // DESCRIPCION
    var cell3 = row.insertCell(3); // PESO
    cell0.textContent = ++xrowcounter;
    cell1.textContent = documento + ' (' + dco.options[dco.selectedIndex].innerHTML + ')';
    cell2.textContent = dae1.value + ' Items, ' + dae3.options[dae3.selectedIndex].innerHTML;
    cell3.textContent = dae2.value;
    document.getElementById('conteo').textContent = 'Total de documentos de exportación ingresados:' + xrowcounter;
    var xnum = xrowcounter + 1;
    var itemdoc = {
        numero: xnum,
        peso: dae2.value,
        bultos: dae1.value,
        embalaje: dae3.value,
        adudoc: documento,
        tipodoc: tpd
    };
    lista.push(itemdoc);
    var pesito = 0;
    for (var i = 0; i < lista.length; i++) {
        pesito = parseFloat( lista[i].peso) + pesito;
    }
    pesito = (pesito / 1000);
    document.getElementById('txtpeso').textContent = pesito.toFixed(2);

}

function deleterow() {
   var ttara = document.getElementById('tara').textContent;
    if (ttara.trim().length <= 0 || parseFloat(ttara) <= 0) {
        alert('Por favor seleccione el Booking'); 
        return;
    }
    var table = this.document.getElementById('daes');
    if (table.rows.length <= 1) {
        return;
    }
    table.deleteRow(-1);
    var rem = lista.pop();
    xrowcounter--;

    var pesito = 0;
    for (var i = 0; i < lista.length; i++) {
        pesito = parseFloat(lista[i].peso) + pesito;
    }
    pesito =  (pesito/1000)
    document.getElementById('txtpeso').textContent = pesito.toFixed(2);
    document.getElementById('conteo').textContent = 'Total de documentos de exportación ingresados:' + xrowcounter;
}
function setBulk(item) {
    var params = {};
    params.item = item;
    params = JSON.stringify(params);
    $.ajax({
        type: "POST",
        url: "../services/catalogo.asmx/getBulkData",
        data: params,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (response) {
            document.getElementById('bulcount').textContent = response.d;
        }
    });
}
function checkDC(control, validador, opcional) {
    try {
        control.style.cssText = "background-color:Yellow;color:Red;width:200px;";
        var codigo;
        codigo = control.value.trim().toUpperCase();
        //Opcional no vino, opñcional es nulo opcional es falso
        if (opcional == undefined || opcional == null || opcional == false) {
            if (codigo.length <= 0) {
                document.getElementById(validador).innerHTML = '<span >* opcional</span>';
                return true;
            }
        }
        if (codigo.length <= 0) {
            document.getElementById(validador).innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
            return false;
        }
        if (codigo.length != 11) {
            document.getElementById(validador).innerHTML = '<span class="obligado">No. contenedor INCOMPLETO</span>';
            return false;
        }
        for (i = 0; i <= 3; i++) {
            if (!IsValidChar(codigo.charAt(i))) {
                document.getElementById(validador).innerHTML = '<span class="obligado">Agregue 4 LETRAS</span>';
                return false;
            }
        }
        for (i = 4; i <= codigo.length - 1; i++) {
            if (!IsValidNum(codigo.charAt(i))) {
                document.getElementById(validador).innerHTML = '<span class="obligado">Agregue 7 NUMEROS</span>';
                return false; 
            }
        }
        //OBTENER EL ULTIMO
        var ultimo = codigo.substring(0, codigo.length - 1);
        var verificador = codigo.substring(codigo.length - 1, codigo.length);
        var newverfic = fCalDig(ultimo);
        if (newverfic != verificador) {
            document.getElementById(validador).innerHTML = '<span class="obligado">Verficador INCORRECTO:[' + verificador + '] -> [' + newverfic + '] </span>';
            return;
        }
        control.style.cssText = "background-color:none;color:none;width:200px;"
        document.getElementById(validador).innerHTML = '';
        return true;
    } catch (e) {
        alert(e.Message);
        return false;
    }
}
function fCalDig(codigo) {
    var refc = "0123456789A_BCDEFGHIJK_LMNOPQRSTU_VWXYZ";
    var nValor, nTotal = 0, nPow2 = 1;
    if (codigo.length != 10) return '';
    for (var n = 0; n < 10; n++) {
        nValor = refc.indexOf(codigo.substr(n, 1));
        if (nValor < 0) return '';
        nTotal += nValor * nPow2;
        nPow2 *= 2;
    }
    nTotal = nTotal % 11;
    if (nTotal >= 10) nTotal = 0;
    return nTotal + '';
}
function IsValidChar(str) {
    return str.length === 1 && str.match(/[A-Z]/i);
}
function IsValidNum(str) {
    return str.length === 1 && str.match(/[0-9]/i);
}

function setWarningContainer(container) {
    if (container == null || container == undefined || container.length < 10) {
        return true;
    }

    try {
        container = container.trim().toUpperCase();
        var sinultimo = container.substring(0, container.length - 1);
        var verificador = container.substring(container.length - 1, container.length);
        var newverfic = fCalDig(sinultimo);
        if (newverfic != verificador) {
            return confirm('Contenedor con digito verificador incorrecto [' + sinultimo + '-' + verificador + '],\nEsta seguro que desea continuar?');
        }
        return true;
    } catch (e) {
    alert(e.Message);
    return false;
    }

}


