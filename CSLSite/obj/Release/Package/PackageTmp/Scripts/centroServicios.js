function linkcontenedor1(ctrafico, tipo) {
    try {
        var trafico = document.getElementById(ctrafico).value;
        var traficoTexto = document.getElementById(ctrafico).options[document.getElementById(ctrafico).selectedIndex].text;
        if (trafico == null || trafico == undefined || trafico == 0 || trafico == '') {
            alert(' * Por favor escoja un tipo tráfico *');
            return;
        }
        var x = '../catalogo/contenedores.aspx?trafico=' + traficoTexto + '&contenido=' + tipo;
        window.open(x, '', 'width=850,height=480');

    } catch (e) {
        alert(e.Message);
    }
}
function linkcontenedor2(ctrafico, contenedor1, tipo) {
    try {
        var trafico = document.getElementById(ctrafico).value;
        var traficoTexto = document.getElementById(ctrafico).options[document.getElementById(ctrafico).selectedIndex].text;
        if (trafico == null || trafico == undefined || trafico == 0 || trafico == '') {
            alert(' * Por favor escoja un tipo de tráfico *');
            return;
        }
        var contenedor = document.getElementById(contenedor1).value;
        if (contenedor == null || contenedor == undefined || contenedor == '') {
            alert(' * Por favor escoja un contenedor en el campo de "contenedor uno" *');
            return;
        }
       window.open('../catalogo/contenedores.aspx?trafico=' + traficoTexto + '&contenido=' + tipo, '', 'width=850,height=880');

    } catch (e) {
        alert(e.Message);
    }
}
function linkconsultacontenedor(usuario) {
    try {
        var userC = document.getElementById(usuario).value;
        if (userC == null || userC == undefined || userC == 0 || userC == '') {
            alert(' * Usuario no logoneado *');
            return;
        }
        window.open('../catalogo/consultaContenedores.aspx?usuario=' + userC, '', 'width=850,height=880');

    } catch (e) {
        alert(e.Message);
    }
}
function linkconsultadetalle(idSolicitud, codigoServicio) {
    try {
        //var idsol = document.getElementById(idSolicitud).value;
        var idSol = idSolicitud;
        if (idSol == null || idSol == undefined || idSol == 0 || idSol == '') {
            alert(' * Debe de seleccionar una solicitud. *');
            return;
        }

        if (codigoServicio == 'CIE') {
            window.open('../catalogo/consultaSolicitudDetalleExportacion.aspx?idSolicitud=' + idSol, '', 'width=850,height=880');
        }
        else {
            window.open('../catalogo/consultaSolicitudDetalle.aspx?idSolicitud=' + idSol, '', 'width=850,height=880');
        }
    } catch (e) {
        alert(e.Message);
    }
}
function linkconsultacontenedorOperario() {
    try {
        window.open('../catalogo/consultaContenedores.aspx', 'Contenedores', 'width=850,height=880');

    } catch (e) {
        alert(e.Message);
    }
}
function linkContenedoresBySolicitud(idSolicitud) {
    try {
        //var idsol = document.getElementById(idSolicitud).value;
        var idSol = idSolicitud;
        if (idSol == null || idSol == undefined || idSol == 0 || idSol == '') {
            alert(' * Debe de seleccionar una solicitud. *');
            return;
        }
        window.open('../catalogo/consultaContenedoresAnalista.aspx?idSolicitud=' + idSol, '' + idSolicitud, 'width=900,height=880');
    } catch (e) {
        alert(e.Message);
    }
}
function linkBuque() {
    try {

        window.open('../catalogo/referenciaBuque.aspx', 'Booking', 'width=850,height=880');

    } catch (e) {
        alert(e.Message);
    }
}
function changeControls(controlID, action) {
    if (action == "TRUE") {
        $('[id*=' + controlID + ']').attr("disabled", "disabled");
        //$('[id*=' + controlID + ']').val("");
        $('[id*=' + controlID + ']').attr("style", "background-color: #CECECE; width=90%!important");
    } else {
        $('[id*=' + controlID + ']').removeAttr("disabled", "disabled");
        //$('[id*=' + controlID + ']').val("");
        $('[id*=' + controlID + ']').removeAttr("style", "background-color: #000000; width=90%!important");
    }
}
function selectServicio(tipoServicio, tipoTrafico, tipoCarga, contenedor, numBooking, numCargaU, numCargoD) {
    try {

        var tipoS = tipoServicio;
        var tipoT = tipoTrafico;
        var tipoC = tipoCarga;
        var cont = contenedor;
        var numBook = numBooking;
        var noCargaU = numCargaU;
        var noCargaD = numCargoD;

        if (tipoS == null || tipoS == undefined || tipoS == 0 || tipoS == '') {
            alert(' * Debe de seleccionar un servicio. *');
            return false;
        } else {
            switch (tipoS.trim()) {
                case 'BAS': //Servicio de Repesaje
                    if (tipoT == null || tipoT == undefined || tipoT == 0 || tipoT == '') {
                        alert('* Debe de seleccionar un tipo de tráfico. *');
                        return false;
                    } else {
                        if (tipoT === "IMPRT") {
                            $('[id*=dptipocarga]').html("<option value='0'>* Seleccione *</option><option value='CO'>Contenedor</option>");
                            $('[id*=dptipocarga]').val(tipoC);
                        } else {
                            $('[id*=dptipocarga]').html("<option value='0'>* Seleccione *</option><option value='CO'>Contenedor</option>");
                            $('[id*=dptipocarga]').val(tipoC);
                        }
                    }


                    if (tipoT === "EXPRT" && tipoC === "CO") {
                        changeControls('txtnumcarga1', "TRUE");
                        changeControls('txtnumcarga2', "TRUE");
                        changeControls('txtnumcarga3', "TRUE");
                        changeControls('txtcontenedor', "FALSE");
                        changeControls('txtNumBooking', "FALSE");

                        $('[id*=txtcontenedor]').attr("onBlur", "checkDC(this,'valintro',true);");
                        $('[id*=txtnumcarga1]').removeAttr("onBlur", "checkNumCarga(this,'valintro',true,'MSR');");
                        $('[id*=txtnumcarga2]').removeAttr("onBlur", "checkNumCarga(this,'valintro',true,'MSN');");
                        $('[id*=txtnumcarga3]').removeAttr("onBlur", "checkNumCarga(this,'valintro',true,'HSN');");
                    } else if (tipoT === "IMPRT" && tipoC === "CA") {
                        changeControls('txtnumcarga1', "FALSE");
                        changeControls('txtnumcarga2', "FALSE");
                        changeControls('txtnumcarga3', "FALSE");
                        changeControls('txtcontenedor', "TRUE");
                        changeControls('txtNumBooking', "TRUE");
                        $('[id*=txtcontenedor]').removeAttr("onBlur", "checkDC(this,'valintro',true);");
                        $('[id*=txtnumcarga1]').attr("onBlur", "checkNumCarga(this,'valintro',true,'MSR');");
                        $('[id*=txtnumcarga2]').attr("onBlur", "checkNumCarga(this,'valintro',true,'MSN');");
                        $('[id*=txtnumcarga3]').attr("onBlur", "checkNumCarga(this,'valintro',true,'HSN');");
                    } else if (tipoT === "IMPRT" && tipoC === "CO") {
                        changeControls('txtnumcarga1', "FALSE");
                        changeControls('txtnumcarga2', "FALSE");
                        changeControls('txtnumcarga3', "FALSE");
                        changeControls('txtcontenedor', "FALSE");
                        changeControls('txtNumBooking', "TRUE");

                        $('[id*=txtcontenedor]').attr("onBlur", "checkDC(this,'valintro',true);");
                        $('[id*=txtnumcarga1]').attr("onBlur", "checkNumCarga(this,'valintro',true,'MSR');");
                        $('[id*=txtnumcarga2]').attr("onBlur", "checkNumCarga(this,'valintro',true,'MSN');");
                        $('[id*=txtnumcarga3]').attr("onBlur", "checkNumCarga(this,'valintro',true,'HSN');");
                    }
                    $('[id*=btbuscar]').removeAttr("disabled", "disabled");
                    break;

                case 'SEL': //Verificación de Sellos
                    if (tipoT == null || tipoT == undefined || tipoT == 0 || tipoT == '') {
                        alert(' * Debe de seleccionar un tipo de tráfico. *');
                        return false;
                    }

                    if (tipoT === "EXPRT" && tipoC === "CO") {
                        changeControls('txtnumcarga1', "TRUE");
                        changeControls('txtnumcarga2', "TRUE");
                        changeControls('txtnumcarga3', "TRUE");
                        changeControls('txtcontenedor', "FALSE");
                        changeControls('txtNumBooking', "FALSE");

                        $('[id*=txtcontenedor]').attr("onBlur", "checkDC(this,'valintro',true);");
                        $('[id*=txtnumcarga1]').removeAttr("onBlur", "checkNumCarga(this,'valintro',true,'MSR');");
                        $('[id*=txtnumcarga2]').removeAttr("onBlur", "checkNumCarga(this,'valintro',true,'MSN');");
                        $('[id*=txtnumcarga3]').removeAttr("onBlur", "checkNumCarga(this,'valintro',true,'HSN');");
                    } else if (tipoT === "IMPRT" && tipoC === "CA") {
                        changeControls('txtnumcarga1', "FALSE");
                        changeControls('txtnumcarga2', "FALSE");
                        changeControls('txtnumcarga3', "FALSE");
                        changeControls('txtcontenedor', "TRUE");
                        changeControls('txtNumBooking', "TRUE");
                        $('[id*=txtcontenedor]').removeAttr("onBlur", "checkDC(this,'valintro',true);");
                        $('[id*=txtnumcarga1]').attr("onBlur", "checkNumCarga(this,'valintro',true,'MSR');");
                        $('[id*=txtnumcarga2]').attr("onBlur", "checkNumCarga(this,'valintro',true,'MSN');");
                        $('[id*=txtnumcarga3]').attr("onBlur", "checkNumCarga(this,'valintro',true,'HSN');");
                    } else if (tipoT === "IMPRT" && tipoC === "CO") {
                        changeControls('txtnumcarga1', "FALSE");
                        changeControls('txtnumcarga2', "FALSE");
                        changeControls('txtnumcarga3', "FALSE");
                        changeControls('txtcontenedor', "FALSE");
                        changeControls('txtNumBooking', "TRUE");

                        $('[id*=txtcontenedor]').attr("onBlur", "checkDC(this,'valintro',true);");
                        $('[id*=txtnumcarga1]').attr("onBlur", "checkNumCarga(this,'valintro',true,'MSR');");
                        $('[id*=txtnumcarga2]').attr("onBlur", "checkNumCarga(this,'valintro',true,'MSN');");
                        $('[id*=txtnumcarga3]').attr("onBlur", "checkNumCarga(this,'valintro',true,'HSN');");
                    }

                    $('[id*=btbuscar]').removeAttr("disabled", "disabled");
                    break;

                case 'IMO': //Etiquetado - Desetiquetado Unidades
                    if (tipoT == null || tipoT == undefined || tipoT == 0 || tipoT == '') {
                        alert(' * Debe de seleccionar un tipo de tráfico. *');
                        return false;
                    }

                    $('[id*=dptipocarga]').html("<option value='CO'>Contenedor</option>");
                    changeControls('txtnumcarga1', "FALSE");
                    changeControls('txtnumcarga2', "FALSE");
                    changeControls('txtnumcarga3', "FALSE");
                    changeControls('txtcontenedor', "FALSE");
                    changeControls('txtNumBooking', "FALSE");
                    $('[id*=btbuscar]').removeAttr("disabled", "disabled");
                    break;
                case 'RTR': //Revisión del Contenedor Refrigerado
                    $('[id*=dptipotrafico]').html("<option value='EXPRT'>EXPRT</option>");
                    $('[id*=dptipocarga]').html("<option value='CO'>Contenedor</option>");
                    changeControls('txtnumcarga1', "TRUE");
                    changeControls('txtnumcarga2', "TRUE");
                    changeControls('txtnumcarga3', "TRUE");
                    changeControls('txtcontenedor', "FALSE");
                    changeControls('txtNumBooking', "FALSE");
                    $('[id*=btbuscar]').removeAttr("disabled", "disabled");
                    break;
            }
        }

    } catch (e) {
        alert(e.Message);
    }
}
function linkcontenedorCorrecionDAEExportacion(ctrafico, tipo, ccorreccion, aisv, idUsuario) {
    try {
        var trafico = document.getElementById(ctrafico).value;
        var traficoTexto = document.getElementById(ctrafico).options[document.getElementById(ctrafico).selectedIndex].text;
        var correccion = document.getElementById(ccorreccion).value;
        var correccionTexto = document.getElementById(ccorreccion).options[document.getElementById(ccorreccion).selectedIndex].text;

        var aisvTexto = document.getElementById(aisv).value;
        var idUsuario = document.getElementById(idUsuario).value;
        if (trafico == null || trafico == undefined || trafico == 0 || trafico == '') {
            alert(' * Por favor escoja un tipo tráfico *');
            return;
        }

        if (correccion == null || correccion == undefined || correccion == 0 || correccion == '') {
            alert(' * Por favor escoja un tipo de corrección *');
            return;
        }

        if (correccion == 'DE*') {
            if (aisvTexto == null || aisvTexto == undefined || aisvTexto == '') {
                alert(' * Por favor, para ese tipo de corrección se requiere que ingrese el AISV *')
                return;
            }

        }
        window.open('../catalogo/contenedoresExportacion.aspx?trafico=' + trafico + '&contenido=' + tipo + '&aisv=' + aisvTexto + '&usuario=' + idUsuario + '&correccion=' + correccion, '', 'width=850,height=880');

    } catch (e) {
        alert(e.Message);
    }
}
function linkReferencia() {
    try {
        window.open('../catalogo/referenciaBuque.aspx', ' ', 'width=850,height=880');

    } catch (e) {
        alert(e.Message);
    }
}
function selectTrafico(tipoTrafico, numBooking) {
    try {
        var tipoT = tipoTrafico;
        var numBook = numBooking;

        if (tipoT != null || tipoT != 0 || tipoT == '') {

            if (tipoT === "IMPRT") {
                changeControls('bookingContenedor', "TRUE");
            } else if (tipoT === "EXPRT") {
                changeControls('bookingContenedor', "FALSE");
            }
        }
    } catch (e) {
        alert(e.Message);
    }
}
function checkNumCarga(control, validador, opcional, numCarga) {
    try {
        control.style.cssText = "background-color:#FDFD96;";
        var codigo;
        codigo = control.value.trim().toUpperCase();
        //Opcional no vino, opcional es nulo opcional es falso
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

        if (numCarga != 'MSR') {
            if (codigo.length != 4) {
                document.getElementById(validador).innerHTML = '<span class="obligado">El ' + numCarga + ' está incompleto. Debe ser de 4 caracteres.</span>';
                return false;
            }
        }

    } catch (e) {
        alert(e.Message);
        return false;
    }
}