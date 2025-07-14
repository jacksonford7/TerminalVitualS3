
//viata previa de impresion
function popOpen(URL) {
    window.open(URL, '', 'width=850, height=620, top=0, left=100, scrollbar=no, resize=no, menus=no');
}

//

function check_info() {
    var str = this.document.getElementById("identificacion").value;
    var cb = this.document.getElementById("dpopc").value;
    if (cb === '0') {
        alert("Por favor selecione la empresa operaria de cuadrilla");
        //cancel postback
        return false;
    }
    if (!str || 0 === str.length) {
        alert("Por favor selecione el operario de la lista");
        //cancel postback
        return false;
    }
    return true;
}

function Golink() {
  
    window.open('../nota_credito/lookup_usuario.aspx', 'name', 'width=850,height=480');
  
}

function GolinkGroup() {

    window.open('../nota_credito/lookup_usuario_grupo.aspx', 'name', 'width=850,height=480');

}



function popupCallback(lookup_usuario) {
    //los acultos para enviar al servidor
  
    this.document.getElementById("IdUsuario").value = lookup_usuario.sel_IdUsuario;
    this.document.getElementById("Usuario").value = lookup_usuario.sel_Usuario;
    this.document.getElementById("Nombres").value = lookup_usuario.sel_Nombres;
    this.document.getElementById("llave1").textContent = lookup_usuario.sel_Nombres;

    this.document.getElementById('<%= TxtDescripcion.ClientID %>').value = lookup_usuario.sel_Nombres;

    
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
            span.textContent = 'obligatorio!';
        }
    }
}

function checkPost() {
    var cb = document.getElementById('dpestado');
    if (cb != null && cb != undefined && cb.value != '0') {
        return true;
    }
    alert('Selecione la OPC de la lista');
    return false;
}