function sreqerida(caja, min, max, validator) {
    var xcaja = document.getElementById(caja.id);
    var mens = document.getElementById(validator);
    try {
        if (mens == null) {return true;}
        mens.innerHTML = '';
        if (xcaja == undefined) {
            alert('Error!:[ ' + caja.id + '] dont exists');
            return false;
        }
        var n = xcaja.value.trim().length
        if (n == 0 && min == 0) {
            mens.innerHTML = 'Ok';
            return;
        }

        if (n == 0) {
            mens.innerHTML = '<span class="obligado">*</span>';
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

function sumarHorasFecha() {
    var choras = this.document.getElementById('thoras').value;
    var fechae = this.document.getElementById('tetb').value;
    if (choras == '' || isNaN(choras) || fechae == '') {
        return;
    }
    var parHora = fechae.split(' ');
    var parXhora = parHora[1].split(':');
    var hora = parXhora[0];
    var minuto = parXhora[1];
    var parDate = parHora[0].split('/');
    var dia = parDate[0];
    var mes = parDate[1] - 1;
    var anio = parDate[2];
    var dae = new Date(anio, mes, dia, hora, minuto, 0, 0);
    dae.setHours(dae.getHours() + parseInt(choras));
    var month = (dae.getMonth() + 1) > 9 ? (dae.getMonth() + 1) : "0" + (dae.getMonth() + 1);
    var day = (dae.getDate()) > 9 ? (dae.getDate()) : "0" + (dae.getDate());
    var hours = (dae.getHours()) > 9 ? (dae.getHours()) : "0" + (dae.getHours());
    var minutes = (dae.getMinutes()) > 9 ? (dae.getMinutes()) : "0" + (dae.getMinutes());
    var dateString = day + "/" + month + "/" + dae.getFullYear() + " " + hours + ":" + minutes
    this.document.getElementById('tets').value = dateString;
}

//llamada al catalogo de lineas



function verificar(num) {
    var c1 = 'tIn' + num;
    var c2 = 'tOut' + num;
    if (num == null || num == undefined) {
        return;
    }
    var t1 = document.getElementById(c1);
    var t2 = document.getElementById(c2);
    c1 = 'xVal' + num;
    var xc = document.getElementById(c1);
    if (t1 == null || t1 == undefined || t2 == null || t2 == undefined || xc == null || xc == undefined) {
        return;
    }
    if (t1.value != '' && t2.value != '') {
        xc.textContent = 'Listo'
        xc.className = 'opcional'
    }
    else {
        xc.textContent = 'Pendiente'
        xc.className = 'validacion'
    }
}


//añadir fila a tabla
function add_line_row(tabla_id, arreglo, objeto) {
    var otabla = document.getElementById(tabla_id);
    if (otabla == null || otabla == undefined || objeto == null || objeto == undefined || arreglo == null || arreglo == undefined) {
        alert('Un objeto no fue encontrado');
        return;
    }
    //comprobar linea.
    for (var i = 0; i < arreglo.length; i++) {
        if (arreglo[i].linea == objeto.codigo) {
            alert('La linea ' + objeto.valor + ' ya esta registrada');
            return;
        }
    }
    var itemdoc = {
        id: item,
        linea: objeto.codigo,
        vIn: '',
        vOut: ''
    };
    arreglo.push(itemdoc);

    var item = arreglo.length + 1;

    //esto para insert row debe tener : 1 hidden, 2 text y validaciones
    var row = otabla.insertRow(-1);
    row.className = "point";
    var cell0 = row.insertCell(0); //LINEA
    var cell1 = row.insertCell(1); //VIAJE IN
    var cell2 = row.insertCell(2); // VIAJE OUT

    var cell3 = row.insertCell(3); // RELLENO
    cell3.setAttribute("colspan", "2");

    var sval = document.createElement('span');
    sval.id = 'xVal' + item;
    sval.className = 'validacion';
    sval.textContent = 'Pendiente'
    //validador
    cell3.appendChild(sval);


    //determinar numero de item, que debe ser el ultimo de la lista

    //celda 0
    var linea = document.createElement('p');
    linea.className = 'line-con';
    linea.textContent = objeto.valor;
    cell0.appendChild(linea);
    var input = document.createElement("input");
    input.setAttribute("type", "hidden");
    var nn = 'hid' + item;
    input.setAttribute("name", nn);
    input.setAttribute("id", nn);
    input.setAttribute("value", objeto.codigo);
    cell0.appendChild(input);
    //crear caja 1
    var t1 = document.createElement('input');
    t1.id = 'tIn' + item;
    t1.name = 'tIn' + item;
    t1.setAttribute('type', 'text');
    t1.setAttribute('maxlength', 20);
    t1.setAttribute('onpaste', 'return false;');
    t1.className = 'txc';
    t1.setAttribute("onblur", "sreqerida(this,1,15,'sIn" + item + "');verificar(" + item + ");");
    t1.setAttribute('onkeypress', "return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890/_-.')");
    //caja
    cell1.appendChild(t1);
    var sIn = document.createElement('span');
    sIn.id = 'sIn' + item;
    sIn.className = 'validacion';
    //validador
    cell1.appendChild(sIn);
    //crear caja 2
    var t2 = document.createElement('input');
    t2.id = 'tOut' + item;
    t2.name = 'tOut' + item;
    t2.setAttribute('type', 'text');
    t2.setAttribute('maxlength', 20);
    t2.setAttribute('onpaste', 'return false;');
    t2.className = 'txc';
    t2.setAttribute("onblur", "sreqerida(this,1,15,'sOut" + item + "');verificar(" + item + ");");
    t2.setAttribute('onkeypress', "return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890/_-.')");
    //caja
    cell2.appendChild(t2);
    var sOut = document.createElement('span');
    sOut.id = 'sOut' + item;
    sOut.className = 'validacion';
    //validador
    cell2.appendChild(sOut);
    return;
}

function rem_line_row() {
    var table = this.document.getElementById('tblineas');
    if (table.rows.length <= 1) {
        return;
    }
    table.deleteRow(-1);
    var rem = lineasAsociadas.pop();
}

//consulta de catalogo
function GetPoolLine(filtro, funcion) {
    var params = {};
    params.servicio = filtro;
    params = JSON.stringify(params);
    $.ajax({
        type: "POST",
        url: "../services/atraque.asmx/get_lines",
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

function ClearLines(x) {
    //limpia lineas asociadas
    //limpiar tabla
    var table = document.getElementById('tblineas');
    //borrar filas}
    for (var j = 1; j <= lineasAsociadas.length; j++) {
        table.deleteRow(-1);
    }
   lineasAsociadas = [];
   lineasAsociadas.length = 0;
    for (var p in x.d) {
        add_line_row('tblineas', lineasAsociadas, x.d[p]);
    }
}


function setPort(valor) {
    sw = valor;
    window.open('../catalogo/puerto', 'name', 'width=850,height=480');
}


function populateLines(x) {
    GetPoolLine(x.value, ClearLines)
}


function validaciones(objeto) {
    //valida el servicio
    if (objeto.service == null || objeto.service == undefined || objeto.service == '') {
        alert('Por favor seleccione el servicio de la nave (2)');
        return false;
    }

    //valida la selección de la nave
    if (objeto.imo == null || objeto.imo == undefined || objeto.imo == '') {
        alert('Por favor seleccione la nave (3)');
        return false;
    }
    //Viajes de la Nave
    if (objeto.vIn == null || objeto.vIn == undefined || objeto.vIn == '' || objeto.vOut == null || objeto.vOut == undefined || objeto.vOut == '') {
        alert('Por favor complete la información de viajes de la nave (11, 12)');
        return false;
    }
    //Información PBIP
    if (objeto.pnum == null || objeto.pnum == undefined || objeto.pnum == '' || objeto.phasta == null || objeto.phasta == undefined || objeto.phasta == '' || objeto.pseg == null || objeto.pseg == undefined || objeto.pseg == '0') {
        alert('Por favor complete la información PBIP de la nave (13, 14, 15, 16)');
        return false;
    }
    //Informacion de puertos
    if (objeto.lport == null || objeto.lport == undefined || objeto.lport == '' || objeto.nport == null || objeto.nport == undefined || objeto.nport == '') {
        alert('Por favor complete la información sobre los puertos de la nave (17, 18)');
        return false;
    }
    //Informacion de aduana
    if (objeto.imrn == null || objeto.imrn == undefined || objeto.imrn == '' || objeto.emrn == null || objeto.emrn == undefined || objeto.emrn == '' ) {
        alert('Por favor complete la información de aduana para la nave (19, 20)');
        return false;
    }

    //validacion opcional y posible
    if (objeto.emrn == objeto.imrn) {
        var re = confirm('El número de manifiesto de importación esta igual al de exportación (19, 20), está seguro de continuar?');
        if (re == false) {
            return false;
        }
    }
    //opcionales
    if (objeto.imrn.length < 15) {
        var xc = confirm('El número de manifiesto de importación [' + objeto.imrn + '] parece estar incorrecto, está seguro de continuar?');
        if (xc == false) {
            return false;
        }
    }

    //Informacion de APG
    if (objeto.anio == null || objeto.anio == undefined || objeto.anio == '' || objeto.regis == null || objeto.regis == undefined || objeto.regis == '') {
        alert('Por favor complete la información de Autoridad Portuaria para esta nave (21, 22)');
        return false;
    }
    //Informacion de fechas estimadas
    if (objeto.eta == null || objeto.eta == undefined || objeto.eta == '' || objeto.etb == null || objeto.etb == undefined || objeto.etb == '' || objeto.uso == null || objeto.uso == undefined || objeto.uso == '') {
        alert('Por favor complete la información sobre fechas estimadas (23, 24, 25, 26)');
        return false;
    }
    //aca validar las lineas del objeto
    if (objeto.lines == null || objeto.lines == undefined || objeto.lines.length <= 0) {
        alert('Por seleccione las líneas asociadas al servicio (sección 2) ');
        return false;
    }
    //preguntar por validación de igualdad
    for (c = 0; c <= objeto.lines.length - 1; c++) {
        var item = objeto.lines[c];

        if (item.viajeIn == '' || item.viajeOut == '') {
            alert('La línea ' + item.line + ' tiene un valor vacío en ViajeIn|ViajeOut');
            return false;
        }
        //Esta validación puede ser opcional
//        if (item.viajeIn === item.viajeOut) {
//            alert('La línea ' + item.line + ' presenta un valor repetido en In:[' + item.viajeIn + '] = Out:[' + item.viajeOut + ']');
//            return false;
//        }
    }

    if (!setmails(objeto)) {
        return false;
    }

    return true;
}

function objecLines(objetoT, arregloT, tablaT) {
    var tt = document.getElementById(tablaT);
    var artt = [];
    objetoT.lines = artt;
    if (tt == undefined || tt == null) {
        alert('No se encontró la tabla de líneas asociadas');
        return;
    }
    for (var i = 0; i <= tt.rows.length - 1; i++) {
        var item = { id: i, line: '', viajeIn: '', viajeOut: '' , lnombre: ''};
        var fila = tt.rows[i];
        if (fila != null && fila != undefined) {
            var inputOnFila = fila.getElementsByTagName('input');
            if (inputOnFila != null && inputOnFila != undefined && inputOnFila.length == 3) {
                item.lnombre = fila.cells[0].textContent;
                item.line = inputOnFila[0].value;
                item.viajeIn = inputOnFila[1].value;
                item.viajeOut = inputOnFila[2].value;
                artt.push(item);
            }
        }
    }
    objetoT.lines = artt;
    return;
}

function segVal(control) {
    if (control != null) {
        var ve = control.value;
        if (ve != '0') {
            document.getElementById('va_nivel').textContent = '';
        }
        else {
            document.getElementById('va_nivel').textContent = '* obligatorio';
        }
    }

}

function invokeJsonTransport(objeto, url) {
    $.ajax({
        type: "POST",
        url: url,
        data: "{'objeto':" + JSON.stringify(objeto) + "}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: processOk,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(textStatus + ": " + XMLHttpRequest.responseText);
            document.getElementById("loader").className = 'nover';
            document.getElementById('btsalvar').disabled = false;
        }
    });
}


//esta función setea los valores antes del postback
function setmails(objeto) {
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
                        if (i == 1) { objeto.mail1 = txt.value.trim(); }
                        else if (i == 2) { objeto.mail2 = txt.value.trim(); }
                        else if (i == 3) { objeto.mail3 = txt.value.trim(); }
                        else if (i == 4) { objeto.mail4 = txt.value.trim(); }
                        else if (i == 5) { objeto.mail5 = txt.value.trim(); }
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
        window.open("../atraque/printer.aspx?sid=" + jmsg.mensaje + "", "Imprimir", "width=850,height=700, scrollbars=yes");
    }
    document.getElementById("loader").className = 'nover';
    document.getElementById('btsalvar').disabled = false;
}

//Validar mail->Mover
function validarEmail(email) {
    expr = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!expr.test(email)) {
        return false;
    }
    return true;
}