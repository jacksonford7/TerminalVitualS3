function myfunction1(oSrc, args) {
//    alert('entro');
//    var clasetipo = document.getElementById('<%=txtrazonsocial.ClientID %>');
//    alert(clasetipo.value);
//    if (clasetipo.value.trim().length <= 0) {
//        alert('Por favor digite la Clase/Tipo del Vehiculo.');
//        clasetipo.focus();
//        clasetipo.style.cssText = "background-color:#ffffc6;color:Red;width:200;";
//        return true;
//    }
    args.IsValid = (args.Value.length >= 8);
}
function mailopcional(control, destino) {
    if (control.value.trim().length > 0) {
        if (!validarVariosEmail(control.value)) {
            document.getElementById(destino).innerHTML = '<span class="obligado">Mail no parece correcto</span>';
            return false;
        }
    }
    document.getElementById(destino).innerHTML = ' * opcional';
    return true;
}
function mailone(control, destino) {
    if (control.value.trim().length <= 0) {
            document.getElementById(destino).innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
            return false;
    }
    if (control.value.trim().length > 0) {
        if (!validarVariosEmail(control.value)) {
            document.getElementById(destino).innerHTML = '<span class="obligado">Mail no parece correcto</span>';
            return false;
        }
    }
    control.value = control.value.trim().toLowerCase();
    document.getElementById(destino).innerHTML = '';
    return true;
}
function validalacedula(control, validador, opcional) {
    try {
        var codigo;
        codigo = control.value.trim().toUpperCase();
        if (codigo.length = 0) {
            document.getElementById(validador).innerHTML = '<span class="obligado">No. CI INCOMPLETO!</span>';
            return false;
        }
        if (codigo.length < 10) {
            document.getElementById(validador).innerHTML = '<span class="obligado">No. CI INCOMPLETO!</span>';
            return false;
        }
        if (!/^([0-9])*$/.test(codigo)) {
            document.getElementById(validador).innerHTML = '<span class="obligado">No. CI No es un Numero!</span>';
            return false;
        }
        var array = codigo.split("");
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
                document.getElementById(validador).innerHTML = '<span class="obligado">No. CI no válido!</span>';
                return false;
            }
        }
        else {
            if (final != 10) {
                document.getElementById(validador).innerHTML = '<span class="obligado">No. CI no válido!</span>';
                return false;
            }
        }

        control.style.cssText = "background-color:none;color:none;width:200px;"
        document.getElementById(validador).innerHTML = '';
        return true;
    } catch (e) {
        alert(e.Message);
        return false;
    }
}
function checkrucci(control, validador, opcional) {
    try {
        control.style.cssText = "background-color:White;color:Red;width:200px;";
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
        if (codigo.length < 10 ) {
            document.getElementById(validador).innerHTML = '<span class="obligado">No. C.I. INCOMPLETO</span>';
            return false;
        }
        if (codigo.length > 10) {
                    if (codigo.length < 13) {
                        document.getElementById(validador).innerHTML = '<span class="obligado">No. RUC. INCOMPLETO</span>';
                        return false;
                    }
        }
        control.style.cssText = "background-color:none;color:none;width:200px;"
        document.getElementById(validador).innerHTML = '';
        return true;
    } catch (e) {
        alert(e.Message);
        return false;
    }
}
function valplaca(control, validador, opcional) {
    try {
        control.style.cssText = "background-color:White;color:Red;width:200px;";
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
        if (codigo.length < 7) {
            document.getElementById(validador).innerHTML = '<span class="obligado">PLACA INCOMPLETA!</span>';
            return false;
        }
        control.style.cssText = "background-color:none;color:none;width:200px;"
        document.getElementById(validador).innerHTML = '';
        return true;
    } catch (e) {
        alert(e.Message);
        return false;
    }
}
function checkcaja(control, validador, opcional) {
    try {
        control.style.cssText = "background-color:White;color:Red;width:200px;";
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
        control.style.cssText = "background-color:none;color:none;width:200px;"
        document.getElementById(validador).innerHTML = '';
        control.value = control.value.trim().toUpperCase();
        return true;
    } catch (e) {
        alert(e.Message);
        return false;
    }
}
function checkcajalarge(control, validador, opcional) {
    try {
        control.style.cssText = "background-color:White;color:Red;width:400px;";
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
        control.style.cssText = "background-color:none;color:none;width:400px;"
        document.getElementById(validador).innerHTML = '';
        control.value = control.value.trim().toUpperCase();
        return true;
    } catch (e) {
        alert(e.Message);
        return false;
    }
}
function checkcajagps(control, validador, opcional) {
    try {
        control.style.cssText = "background-color:White;color:Red;width:570px;";
        var codigo;
        codigo = control.value.trim().toUpperCase();
        //Opcional no vino, opñcional es nulo opcional es falso
        if (opcional == undefined || opcional == null || opcional == false) {
            if (codigo.length <= 0) {
                document.getElementById(validador).innerHTML = '<span >* opcional</span>';
                return true;
            }
        }
//        if (codigo.length <= 0) {
//            document.getElementById(validador).innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
//            return false;
//        }
        control.style.cssText = "background-color:none;color:none;width:570px;"
        document.getElementById(validador).innerHTML = '';
        return true;
    } catch (e) {
        alert(e.Message);
        return false;
    }
}
function telconvencionalcelular(control, validador, opcional) {
    try {
        control.style.cssText = "background-color:White;color:Red;width:200px;";
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
        if (codigo.length < 9) {
            document.getElementById(validador).innerHTML = '<span class="obligado">Telf. Convencional o Celular. INCOMPLETO</span>';
            return false;
        }
        control.style.cssText = "background-color:none;color:none;width:200px;"
        document.getElementById(validador).innerHTML = '';
        return true;
    } catch (e) {
        alert(e.Message);
        return false;
    }
}
function telconvencionalcelularnulo(control, validador, opcional) {
    try {
        control.style.cssText = "background-color:White;color:Red;width:100px;";
        var codigo;
        codigo = control.value.trim().toUpperCase();
        //Opcional no vino, opñcional es nulo opcional es falso
        if (opcional == undefined || opcional == null || opcional == false) {
            if (codigo.length <= 0) {
                document.getElementById(validador).innerHTML = '<span >* opcional</span>';
                return true;
            }
        }
        if (codigo.length > 0) {
            if (codigo.length < 9) {
                document.getElementById(validador).innerHTML = '<span class="obligado">Telf. Convencional o Celular. INCOMPLETO</span>';
                return false;
            }
        }
        control.style.cssText = "background-color:none;color:none;width:100px;"
        document.getElementById(validador).innerHTML = '';
        return true;
    } catch (e) {
        alert(e.Message);
        return false;
    }
}
function telconvencional(control, validador, opcional) {
    try {
        control.style.cssText = "background-color:White;color:Red;width:200px;";
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
        if (codigo.length < 9) {
            document.getElementById(validador).innerHTML = '<span class="obligado">Telf. Convencional. INCOMPLETO</span>';
            return false;
        }
        control.style.cssText = "background-color:none;color:none;width:200px;"
        document.getElementById(validador).innerHTML = '';
        return true;
    } catch (e) {
        alert(e.Message);
        return false;
    }
}
function telplanta(control, validador, opcional) {
    try {
        control.style.cssText = "background-color:White;color:Red;width:200px;";
        var codigo;
        codigo = control.value.trim().toUpperCase();
        //Opcional no vino, opñcional es nulo opcional es falso
        if (opcional == undefined || opcional == null || opcional == false) {
            if (codigo.length <= 0) {
                document.getElementById(validador).innerHTML = '<span >* opcional</span>';
                return true;
            }
        }
        if (codigo.length > 0)
        {
        if (codigo.length < 9) {
            document.getElementById(validador).innerHTML = '<span class="obligado">Telf. Planta. INCOMPLETO</span>';
            return false;
        }
        }
        control.style.cssText = "background-color:none;color:none;width:200px;"
        document.getElementById(validador).innerHTML = '';
        return true;
    } catch (e) {
        alert(e.Message);
        return false;
    }
}
function telcelular(control, validador, opcional) {
    try {
        control.style.cssText = "background-color:White;color:Red;width:200px;";
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
        if (codigo.length < 10) {
            document.getElementById(validador).innerHTML = '<span class="obligado">Telf. Celular. INCOMPLETO</span>';
            return false;
        }
        control.style.cssText = "background-color:none;color:none;width:200px;"
        document.getElementById(validador).innerHTML = '';
        return true;
    } catch (e) {
        alert(e.Message);
        return false;
    }
}
//Validar Pagina Web
function validarpaginaweb(control, validador, opcional) {
    try {
        control.style.cssText = "background-color:White;color:Red;width:400px;";
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
        // Validamos Relleno Campos
        //patron = /^www.\w+.\w+$/gi;
        patron = /^HTTP|HTTP|http(s)?:\/\/(www\.)?[A-Za-z0-9]+([\-\.]{1}[A-Za-z0-9]+)*\.[A-Za-z]{2,40}(:[0-9]{1,40})?(\/.*)?$/;
        if (!patron.test(codigo)) {
            document.getElementById(validador).innerHTML = '<span class="obligado">La Web debe tener el formato http https://www.dominio.com .ec .es .org etc.</span>';
            return false;
        }

        control.style.cssText = "background-color:none;color:none;width:400px;"
        document.getElementById(validador).innerHTML = '';
        return true;
    } catch (e) {
        alert(e.Message);
        return false;
    }
}
function validamail(control, destino) {
    if (control.value.trim().length > 0) {
        if (!validarEmail(control.value)) {
            document.getElementById(destino).innerHTML = '<span class="obligado">Mail no parece correcto</span>';
            return false;
        }
    }
    document.getElementById(destino).innerHTML = '';
    return true;
}
function validarEmail(email) {
    expr = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!expr.test(email)) {
        return false;
    }
    return true;
}
function addrowlogexp() {
    var clasetipo = document.getElementById('txtclasetipo');
    if (clasetipo.value.trim().length <= 0) {
        alert('Por favor digite la Clase/Tipo del Vehiculo.');
        clasetipo.focus();
        clasetipo.style.cssText = "background-color:#ffffc6;color:Red;width:200;";
        return;
    }
    var marca = document.getElementById('txtmarca');
    if (marca.value.trim().length <= 0) {
        alert('Por favor digite la Marca del Vehiculo.');
        marca.focus();
        marca.style.cssText = "background-color:#ffffc6;color:Red;width:200;";
        return;
    }
    var modelo = document.getElementById('txtmodelo');
    if (modelo.value.trim().length <= 0) {
        alert('Por favor digite el Modelo del Vehiculo.');
        modelo.focus();
        modelo.style.cssText = "background-color:#ffffc6;color:Red;width:200;";
        return;
    }
    var color = document.getElementById('txtcolor');
    if (color.value.trim().length <= 0) {
        alert('Por favor digite el Color del Vehiculo.');
        color.focus();
        color.style.cssText = "background-color:#ffffc6;color:Red;width:200;";
        return;
    }
    var placa = document.getElementById('txtplaca');
    if (placa.value.trim().length <= 0) {
        alert('Por favor digite la Placa del Vehiculo.');
        placa.focus();
        placa.style.cssText = "background-color:#ffffc6;color:Red;width:200;";
        return;
    }
    if (!validavehiculo(placa.value)) {
        return false;
    };
    var table = this.document.getElementById('datlogexp');
    var row = table.insertRow(-1);
    row.className = "point";
    var cell0 = row.insertCell(0); //Clase/Tipo
    var cell1 = row.insertCell(1); //Marca
    var cell2 = row.insertCell(2); //Modelo
    var cell3 = row.insertCell(3); //Color
    var cell4 = row.insertCell(4); //Placa
    cell0.textContent = clasetipo.value;
    cell1.textContent = marca.value;
    cell2.textContent = modelo.value;
    cell3.textContent = color.value;
    cell4.textContent = placa.value;
    var xnum = xrowcounter + 1;
    var itemdoc = {
        clasetipo: clasetipo.value,
        marca: marca.value,
        modelo: modelo.value,
        color: color.value,
        placa: placa.value
    };
    lista.push(itemdoc);
}
function deleterowlogexp() {
//    var ttara = document.getElementById('tara').textContent;
//    if (ttara.trim().length <= 0 || parseFloat(ttara) <= 0) {
//        alert('Por favor seleccione el Booking');
//        return;
//    }
    var table = this.document.getElementById('datlogexp');
    if (table.rows.length <= 1) {
        return;
    }
    table.deleteRow(-1);
    var rem = lista.pop();
    xrowcounter--;

//    var pesito = 0;
//    for (var i = 0; i < lista.length; i++) {
//        pesito = parseFloat(lista[i].peso) + pesito;
//    }
//    pesito = (pesito / 1000)
//    document.getElementById('txtpeso').textContent = pesito.toFixed(2);
//    document.getElementById('conteo').textContent = 'Total de documentos de exportación ingresados:' + xrowcounter;
}
function validavehiculo(placa) {
    listavehiculos = [];
    var tbl = this.document.getElementById('datlogexp');
    if (tbl.rows.length > 1) {
        for (var f = 0; f < tbl.rows.length; f++) {
            var celColect = tbl.rows[f].getElementsByTagName('td');
            if (celColect != undefined && celColect != null && celColect.length > 0) {
                var tdetalle = {
                    placa: celColect[4].textContent
                };
                this.listavehiculos.push(tdetalle);
            }
        }
        for (var n = 0; n < this.listavehiculos.length; n++) {
            if (listavehiculos[n].placa == placa) {
                alert('Ya agrego la Placa: ' + placa);
                placa.focus();
                //placa.style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                return false;
            }
        }
        listavehiculos = [];
    }
    return true;
}
function tansporteServer(objeto, url) {
    $.ajax({
        type: "POST",
        url: url,
        data: "{'objeto':" + JSON.stringify(objeto) + "}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: continuar,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(textStatus + ": " + XMLHttpRequest.responseText);
            document.getElementById("loader").className = 'nover';
            document.getElementById('btsalvar').disabled = false;
        }
    });
}
function continuar(response) {
    var jmsg = JSON.parse(response.d);
    if (jmsg.resultado) {
        alert(jmsg.mensaje);
        if (jmsg.fluir) { eval(jmsg.data); }

    }
    else {
        alert(jmsg.mensaje);
    }
    document.getElementById("loader").className = 'nover';
    document.getElementById('btsalvar').disabled = false;
}
function checkDate(control) {
   
    var fecha = document.getElementById(control);
    if (fecha == undefined || fecha == null) {
        alert('Campo de fecha inválido es nulo');
        return false;
    }
    fecha.value = fecha.value.substring(0, 10);
    var expr = /^((((0[1-9])|([1-2][0-9])|(3[0-1]))|([1-9]))\x2F(((0[1-9])|(1[0-2]))|([1-9]))\x2F(([0-9]{2})|(((19)|([2]([0]{1})))([0-9]{2}))))$/;
    if (!expr.test(fecha.value)) {
        alert('Valor de fecha inválido, ' + fecha.value);
        return false;
    }
    return true;
}
function valruccipas(control, validador, ruc, ci, pas) {
    try {
        control.style.cssText = "background-color:White;color:Red;width:200px;";
        var codigo;
        codigo = control.value.trim().toUpperCase();
//        if (codigo.length <= 0) {
//            document.getElementById(validador).innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
//            return false;
//        }
//        //valida el largo de la cadena
//        if (codigo.length = 0 && codigo.length != 10) {
//            document.getElementById(validador).innerHTML = '<span class="obligado">El valor no corresponde a un No. de CI!</span>';
//            return false;
//        }
        var vruc = document.getElementById(ruc).checked;
        var vci = document.getElementById(ci).checked;
        var vpas = document.getElementById(pas).checked;
        if (vci == true) {
            if (codigo.length = 0) {
                document.getElementById(validador).innerHTML = '<span class="obligado">No. CI INCOMPLETO!</span>';
                return false;
            }
            if (codigo.length < 10) {
                document.getElementById(validador).innerHTML = '<span class="obligado">No. CI INCOMPLETO!</span>';
                return false;
            }
            if (!/^([0-9])*$/.test(codigo)) {
                document.getElementById(validador).innerHTML = '<span class="obligado">No. CI No es un Numero!</span>';
                return false;
            }
            var array = codigo.split("");
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
                    document.getElementById(validador).innerHTML = '<span class="obligado">No. CI no válido!</span>';
                    return false;
                }
            }
            else {
                if (final != 10) {
                    document.getElementById(validador).innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
                    return false;
                }
            }
        }
        if (vpas == true) {

            if (codigo.length <= 0) {
                document.getElementById(validador).innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
                return false;
            }
        }   
        if (vruc == true) {
            if (codigo.length < 13) {
                document.getElementById(validador).innerHTML = '<span class="obligado">No. RUC. INCOMPLETO!</span>';
                return false;
            }
            if (codigo.length > 13) {
                document.getElementById(validador).innerHTML = '<span class="obligado">El valor no corresponde a un No. de RUC!</span>';
                return false;
            }
            if (!/^([0-9])*$/.test(codigo)) {
                document.getElementById(validador).innerHTML = '<span class="obligado">No. RUC. No es un Numero!</span>';
                return false;
            }
            var numeroProvincias = 24;
            var numprov = codigo.substr(0, 2);
            if (numprov > numeroProvincias) {
                document.getElementById(validador).innerHTML = '<span class="obligado">El código de la provincia (dos primeros dígitos) es inválido!</span>';
                return false;
            }
            if (!validadocruc(codigo, validador)) {
                return false;
            }
        }        
        control.style.cssText = "background-color:none;color:none;width:200px;"
        document.getElementById(validador).innerHTML = '';
        return true;
    } catch (e) {
        alert(e.Message);
        return false;
    }
}
function valccipasrep(control, validador, ci, pas) {
    try {
        control.style.cssText = "background-color:White;color:Red;width:200px;";
        var codigo;
        codigo = control.value.trim().toUpperCase();
        //        if (codigo.length <= 0) {
        //            document.getElementById(validador).innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
        //            return false;
        //        }
        //        //valida el largo de la cadena
        //        if (codigo.length = 0 && codigo.length != 10) {
        //            document.getElementById(validador).innerHTML = '<span class="obligado">El valor no corresponde a un No. de CI!</span>';
        //            return false;
        //        }
        var vci = document.getElementById(ci).checked;
        var vpas = document.getElementById(pas).checked;
        if (vci == true) {
            if (codigo.length = 0) {
                document.getElementById(validador).innerHTML = '<span class="obligado">No. CI INCOMPLETO!</span>';
                return false;
            }
            if (codigo.length < 10) {
                document.getElementById(validador).innerHTML = '<span class="obligado">No. CI INCOMPLETO!</span>';
                return false;
            }
            if (!/^([0-9])*$/.test(codigo)) {
                document.getElementById(validador).innerHTML = '<span class="obligado">No. CI No es un Numero!</span>';
                return false;
            }
            var array = codigo.split("");
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
                    document.getElementById(validador).innerHTML = '<span class="obligado">No. CI no válido!</span>';
                    return false;
                }
            }
            else {
                if (final != 10) {
                    document.getElementById(validador).innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
                    return false;
                }
            }
        }
        if (vpas == true) {

            if (codigo.length <= 0) {
                document.getElementById(validador).innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
                return false;
            }
        }
      
        control.style.cssText = "background-color:none;color:none;width:200px;"
        document.getElementById(validador).innerHTML = '';
        return true;
    } catch (e) {
        alert(e.Message);
        return false;
    }
}
function valcipas(control, validador, ci, pas) {
    try {
        control.style.cssText = "background-color:White;color:Red;width:200px;";
        var codigo;
        codigo = control.value.trim().toUpperCase();
        //        if (codigo.length <= 0) {
        //            document.getElementById(validador).innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
        //            return false;
        //        }
        //        //valida el largo de la cadena
        //        if (codigo.length = 0 && codigo.length != 10) {
        //            document.getElementById(validador).innerHTML = '<span class="obligado">El valor no corresponde a un No. de CI!</span>';
        //            return false;
        //        }
        var vci = document.getElementById(ci).checked;
        var vpas = document.getElementById(pas).checked;
        if (vci == true) {
            if (codigo.length = 0) {
                document.getElementById(validador).innerHTML = '<span class="obligado">No. CI INCOMPLETO!</span>';
                return false;
            }
            if (codigo.length < 10) {
                document.getElementById(validador).innerHTML = '<span class="obligado">No. CI INCOMPLETO!</span>';
                return false;
            }
            if (!/^([0-9])*$/.test(codigo)) {
                document.getElementById(validador).innerHTML = '<span class="obligado">No. CI No es un Numero!</span>';
                return false;
            }
            var array = codigo.split("");
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
                    document.getElementById(validador).innerHTML = '<span class="obligado">No. CI no válido!</span>';
                    return false;
                }
            }
            else {
                if (final != 10) {
                    document.getElementById(validador).innerHTML = '<span class="obligado">No. CI no válido!</span>';
                    return false;
                }
            }
        }
        control.style.cssText = "background-color:none;color:none;width:200px;"
        document.getElementById(validador).innerHTML = '';
        return true;
    } catch (e) {
        alert(e.Message);
        return false;
    }
}
function validadocruc(campo, validador) {
    var numero = campo;
    var suma = 0;
    var residuo = 0;
    var pri = false;
    var pub = false;
    var nat = false;
    var numeroProvincias = 24;
    var modulo = 11;

    /* Verifico que el campo no contenga letras */

    /* Aqui almacenamos los digitos de la cedula en variables. */
    d1 = numero.substr(0, 1);
    d2 = numero.substr(1, 1);
    d3 = numero.substr(2, 1);
    d4 = numero.substr(3, 1);
    d5 = numero.substr(4, 1);
    d6 = numero.substr(5, 1);
    d7 = numero.substr(6, 1);
    d8 = numero.substr(7, 1);
    d9 = numero.substr(8, 1);
    d10 = numero.substr(9, 1);

    /* El tercer digito es: */
    /* 9 para sociedades privadas y extranjeros */
    /* 6 para sociedades publicas */
    /* menor que 6 (0,1,2,3,4,5) para personas naturales */

    if (d3 == 7 || d3 == 8) {
        //alert('El tercer dígito ingresado es inválido');
        document.getElementById(validador).innerHTML = '<span class="obligado">El tercer dígito ingresado es inválido!</span>';
        return false;
    }

    /* Solo para personas naturales (modulo 10) */
    if (d3 < 6) {
        nat = true;
        p1 = d1 * 2; if (p1 >= 10) p1 -= 9;
        p2 = d2 * 1; if (p2 >= 10) p2 -= 9;
        p3 = d3 * 2; if (p3 >= 10) p3 -= 9;
        p4 = d4 * 1; if (p4 >= 10) p4 -= 9;
        p5 = d5 * 2; if (p5 >= 10) p5 -= 9;
        p6 = d6 * 1; if (p6 >= 10) p6 -= 9;
        p7 = d7 * 2; if (p7 >= 10) p7 -= 9;
        p8 = d8 * 1; if (p8 >= 10) p8 -= 9;
        p9 = d9 * 2; if (p9 >= 10) p9 -= 9;
        modulo = 10;
    }

    /* Solo para sociedades publicas (modulo 11) */
    /* Aqui el digito verficador esta en la posicion 9, en las otras 2 en la pos. 10 */
    else if (d3 == 6) {
        pub = true;
        p1 = d1 * 3;
        p2 = d2 * 2;
        p3 = d3 * 7;
        p4 = d4 * 6;
        p5 = d5 * 5;
        p6 = d6 * 4;
        p7 = d7 * 3;
        p8 = d8 * 2;
        p9 = 0;
    }

    /* Solo para entidades privadas (modulo 11) */
    else if (d3 == 9) {
        pri = true;
        p1 = d1 * 4;
        p2 = d2 * 3;
        p3 = d3 * 2;
        p4 = d4 * 7;
        p5 = d5 * 6;
        p6 = d6 * 5;
        p7 = d7 * 4;
        p8 = d8 * 3;
        p9 = d9 * 2;
    }

    suma = p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9;
    residuo = suma % modulo;

    /* Si residuo=0, dig.ver.=0, caso contrario 10 - residuo*/
    digitoVerificador = residuo == 0 ? 0 : modulo - residuo;

    /* ahora comparamos el elemento de la posicion 10 con el dig. ver.*/
    if (pub == true) {
        if (digitoVerificador != d9) {
            //alert('El ruc de la empresa del sector público es incorrecto.');
            document.getElementById(validador).innerHTML = '<span class="obligado">El RUC de la empresa del sector público es incorrecto!</span>';
            return false;
        }
        /* El ruc de las empresas del sector publico terminan con 0001*/
        if (numero.substr(9, 4) != '0001') {
            //alert('El ruc de la empresa del sector público debe terminar con 0001');
            document.getElementById(validador).innerHTML = '<span class="obligado">El RUC de la empresa del sector público debe terminar con 0001!</span>';
            return false;
        }
    }
    else if (pri == true) {
        if (digitoVerificador != d10) {
            //alert('El ruc de la empresa del sector privado es incorrecto.');
            document.getElementById(validador).innerHTML = '<span class="obligado">El RUC de la empresa del sector privado es incorrecto!</span>';
            return false;
        }
        if (numero.substr(10, 3) != '001') {
            //alert('El ruc de la empresa del sector privado debe terminar con 001');
            document.getElementById(validador).innerHTML = '<span class="obligado">El RUC de la empresa del sector privado debe terminar con 001!</span>';
            return false;
        }
    }

    else if (nat == true) {
        if (digitoVerificador != d10) {
            //alert('El número de cédula de la persona natural es incorrecto.');
            document.getElementById(validador).innerHTML = '<span class="obligado">El número de cédula de la persona natural es incorrecto!</span>';
            return false;
        }
        if (numero.length > 10 && numero.substr(10, 3) != '001') {
            //alert('El ruc de la persona natural debe terminar con 001');
            document.getElementById(validador).innerHTML = '<span class="obligado">El RUC de la persona natural debe terminar con 001!</span>';
            return false;
        }
    }
    return true;
}
function validaextension(control) {
//    alert(control.value);
//    return false;

    var ext = control.getAttribute("extension");
    //alert(extension);
    //return false;
    extensiones_permitidas = new Array(ext);
    archivo = control.value;
    mierror = "";
    if (!archivo) {
        //Si no tengo archivo, es que no se ha seleccionado un archivo en el formulario 
        mierror = "No has seleccionado ningún archivo";
    } else {
        //recupero la extensión de este nombre de archivo 
        extension = (archivo.substring(archivo.lastIndexOf("."))).toLowerCase();
        //alert (extension); 
        //compruebo si la extensión está entre las permitidas 
        permitida = false;
        for (var i = 0; i < extensiones_permitidas.length; i++) {
            if (extensiones_permitidas[i] == extension) {
                permitida = true;
                break;
            }
        }
        if (!permitida) {
            mierror = "Comprueba la extensión de los archivos a subir. \nSólo se pueden subir archivos con extensiones: " + extensiones_permitidas.join();
            control.value = null;
        } else {
            //submito! 
            //alert("Todo correcto. Voy a submitir el formulario.");
        control.tooltip = control.value;
            return 1;
        }
    }
    //si estoy aqui es que no se ha podido submitir 
    alert(mierror);
    return 0;
}
function ToolTip(control) {
    var e = control;
    var strValue = e.options[e.selectedIndex].text;
    alert(strValue);
    control.tooltip = strValue;
    return 1;
}
//valida un control dropdow cuando cambia
function valdltipsol(control, validador) {
    if (control.value != 0) {
        control.style.cssText = "background-color:none;color:none;width:400px;"
        validador.innerHTML = '';
        return;
    }
    control.style.cssText = "background-color:White;color:Red;width:400px;";
    validador.innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
    return;
}
function valdltipsolhorario(control, validador) {
    if (control.value != 0) {
        control.style.cssText = "background-color:none;color:none;width:500px;"
        validador.innerHTML = '';
        return;
    }
    control.style.cssText = "background-color:White;color:Red;width:500px;";
    validador.innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
    return;
}
function validadropdownlist(control, validador) {
    if (control.value != 0) {
        control.style.cssText = "background-color:none;color:none;width:350px;"
        validador.innerHTML = '';
        return;
    }
    control.style.cssText = "background-color:White;color:Red;width:350px;";
    validador.innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
    return;
}
function valdlcatveh(control, validador) {
    if (control.value != 0) {
        control.style.cssText = "background-color:none;color:none;width:200px;"
        validador.innerHTML = '';
        return;
    }
    control.style.cssText = "background-color:White;color:Red;width:200px;";
    validador.innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
    return;
}
function calcular_edad(control, validador) {
    control.style.cssText = "background-color:White;color:Red;width:200px;";
    var codigo;
    codigo = control.value.trim().toUpperCase();
    if (codigo.length <= 0) {
        document.getElementById(validador).innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
        return false;
    }
//    var fecha = new Date(control.value)
//    var hoy = new Date()
//    var edad = parseInt((hoy - fecha) / 365 / 24 / 60 / 60 / 1000)
//    if (parseInt(edad) < 18 || edad == NaN) {
//        document.getElementById(validador).innerHTML = '<span class="obligado">Tiene que ser mayor a 18 años!</span>';
//        return false;
    //    }
//    var dia = codigo.getDate();
//    var mes = codigo.getMonth();
//    var ano = codigo.getYear();
//    alert(codigo);
//    fecha_hoy = new Date();
//    ahora_ano = fecha_hoy.getYear();
//    ahora_mes = fecha_hoy.getMonth();
//    ahora_dia = fecha_hoy.getDate();
//    edad = (ahora_ano + 1900) - ano;

//    if (ahora_mes < (mes - 1)) {
//        edad--;
//    }
//    if (((mes - 1) == ahora_mes) && (ahora_dia < dia)) {
//        edad--;
//    }
//    if (edad > 1900) {
//        edad -= 1900;
//    }

//    alert("¡Tienes " + edad + " años!");
    control.style.cssText = "background-color:none;color:none;width:200px;"
    validador.innerHTML = '';
    return true;
}