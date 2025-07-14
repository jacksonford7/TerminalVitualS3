
//viata previa de impresion
function popOpen(URL) {
    window.open(URL, '', 'width=850, height=620, top=0, left=100, scrollbar=no, resize=no, menus=no');
}

//

function check_info() {
    var str = this.document.getElementById("identificacion").value;
    var cb = this.document.getElementById("dpopc").value;
    if (cb === '0') {
      alertify.alert("Por favor selecione la empresa operaria de cuadrilla");
        //cancel postback
        return false;
    }
    if (!str || 0 === str.length) {
        alertify.alert("Por favor selecione el operario de la lista");
        //cancel postback
        return false;
    }
    return true;
}

function Golink() {
    var cb = this.document.getElementById("dpopc").value;
    if (cb === '0') {
        alertify.alert("Por favor selecione la empresa operaria de cuadrilla");
        return;
    }
    window.open('../catalogo/operario.aspx?opc=' + cb + '', 'name', 'width=850,height=880');
    /*onclick="window.open('../catalogo/operario','name','width=850,height=480')" */
}

function popupCallback(operario) {
    //los acultos para enviar al servidor
    this.document.getElementById("identificacion").value = operario.identificacion;
    this.document.getElementById("nombres").value = operario.nombre;
    this.document.getElementById("apellidos").value = operario.apellidos;
    this.document.getElementById("operario_name").textContent = operario.nombre + " " + operario.apellidos;
}

function dropcheck(control, span_name, texto) {
    var span = document.getElementById(span_name);
    if (control.value != texto) {
        if (span != null && span != undefined) {
            span.textContent = '';
        }
    }
    else {
        if (span != null && span != undefined) {
            span.textContent = '*';
        }
    }
}

function checkPost() {
    var cb = document.getElementById('dpestado');
    if (cb != null && cb != undefined && cb.value != '0') {
        return true;
    }
    alertify.alert('Selecione la OPC de la lista');
    return false;
}