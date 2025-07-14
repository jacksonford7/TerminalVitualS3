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

function cajaControl(caja) {
    
    var x = caja.parentNode.parentNode.parentNode;
    var total = 0;
    if (x != null && x != undefined) {
        var ar = x.getElementsByTagName('input');
        for (var i = 0; i <= ar.length - 1; i++) {
            if (ar[i].value.trim().length > 0) {
                total += parseInt(ar[i].value);
            }
        }
       
    }
    document.getElementById('ttd').textContent = "Total de unidades reservadas: " + total;
    var valor = caja.value;
    var control = caja.getAttribute("xval");
    if (valor == null || valor == undefined || valor == '') {
        valor = 1;
    }
    if (control == null || control == undefined || control == '') {
        control = -1;
    }
    if (parseInt(valor) > parseInt(control)) {
        caja.style.color = "red";
        caja.style.backgroundColor = "yellow";
        return;
    }
    caja.style.color = '';
    caja.style.backgroundColor = '';
   
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