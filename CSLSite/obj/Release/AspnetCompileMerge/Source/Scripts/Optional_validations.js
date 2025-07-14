
//Parameter: entidad { mid= numeric -> validation caller, par [ {name:value , name:value} ] }
var sobjeto = {};
function buble_warning() {
    var driver = document.getElementById('driID').value;
    var placa = document.getElementById('txtplaca').value;
    if (driver === null || driver === 'undefined' || driver.length < 4) { return;}
    if (placa === null || placa === 'undefined' || placa.length < 4) { return; }
    sobjeto.mid = '1';
    var par = [{ 'nombre': 'driver', 'valor': driver }, { 'nombre': 'placa', 'valor': placa } ];
    sobjeto.parametros = par;
    AsyncValidation(sobjeto, '../services/aisv.asmx/chofer_compania');
}
function respuestaOk(response) {
    if (response == null) { return; }
    var objjson = JSON.parse(response.d);
    //esvalido, mensaje, accion
    if (objjson.esvalido == '0') {
        if (objjson.accion == 'ShowAlert') {
            var popup = document.getElementById("verPop");
            popup.innerHTML = objjson.mensaje;
            popup.classList.toggle("show");
        }
    }
}
function AsyncValidation(objeto, url) {
    $.ajax({
        type: "POST",
        url: url,
        data: "{'objeto':" + JSON.stringify(objeto) + "}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: respuestaOk,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(textStatus + ": " + XMLHttpRequest.responseText);
        }
    });
}


/*
 * //sample
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

<div class="popup" >Aviso!
  <span class="popuptext" id="verPop">Texto</span>
</div>

onblur="buble_warning()"

 */