<%@ Page  Language="C#" AutoEventWireup="true" Title="Consola de Solicitudes"
         CodeBehind="consulta_solicitud.aspx.cs" Inherits="CSLSite.transportista.consulta_solicitud" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Consola de Solicitudes</title>
    <link href="../shared/estilo/base-site.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
     <link href="../Inicio/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogoestadosolicitud.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <link href="../Inicio/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../Inicio/base-site.css" rel="stylesheet" type="text/css" />
    <link rel="../shortcut icon" type="image/x-icon" href="../favicon.ico" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
     * input[type=text]
        {
            text-align:left!important;
        }
        .style1
        {
            border-bottom: 1px solid #CCC;
            width: 530px;
        }
    </style>
    <script type="text/javascript">
        function BindFunctions() {
            document.getElementById('imagen').innerHTML = '';
            $(document).ready(function () {
                $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 5: { sorter: false }, 6: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
            });
        }
    </script>

</head>
<body>
  <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
   <form id="wfconestsol" runat="server">
   <input id="zonaid" type="hidden" value="7" />
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
    <div class="catabody" style=" height:100%">
     <div class="catawrap" >
 <div>
 <table>
 <tr>
 <td>   
 <div class="envolve">
      <a href="../csl/menudefault"><i class="element-menu"></i>Menú principal</a>
 </div>
 </td>
 <td>
  <i class="ico-titulo-1"></i><h2>Asociación de Transportistas</h2><h2>&nbsp;</h2><br />
   <i class="ico-titulo-2"></i><h1>Consola de Solicitudes</h1><br />
 </td>
 </tr>
 </table>
 </div>
 <div class="seccion">
       <div class="accion">
       <table class="xcontroles" cellspacing="0" cellpadding="1">
       <tr><th class="bt-bottom bt-right bt-left bt-top" colspan="4"> Criterios de consulta:</th></tr>
              <tr>
        <td class="bt-bottom bt-right bt-left" >Número de Solicitud:</td>
        <td class="bt-bottom bt-right">
             <asp:TextBox ID="txtsolicitud" runat="server" Width="120px" MaxLength="11"
             style="text-align: center" onblur="cajaControl(this);"
             onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
        </td>
        
       </tr>
       <tr>
        <td class="bt-bottom bt-right bt-left" >Ruc Cia. Transporte:</td>
        <td class="bt-bottom bt-right">
             <asp:TextBox ID="txtruccipas" runat="server" Width="120px" MaxLength="13"
             style="text-align: center" onblur="cajaControl(this);"
             onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
        </td>
        
       </tr>
       <tr style=" display:none">
          <td class="bt-bottom bt-right bt-left">Tipo de Solicitud:</td>
          <td class="bt-bottom bt-right">
          <%--<a class="tooltip" >
          <span class="classic">Esta informacion contiene los tipos de solicitudes disponibles.</span>--%>
          <asp:DropDownList ID="dptiposolicitud" runat="server" Width="200px" >
                 <asp:ListItem Value="0">* Seleccione origen *</asp:ListItem>
          </asp:DropDownList>
          <%--</a>--%>
        </td>
        </tr>
       <tr style=" display:none">
        <td class="bt-bottom bt-left bt-right">Tipo de Usuario:</td>
        <td class="bt-bottom bt-right">
          <%--<a class="tooltip" >
          <span class="classic">Esta informacion contiene los tipos de usuarios disponibles.</span>--%>
          <asp:DropDownList ID="dptipousuario" runat="server" Width="300px" >
                 <asp:ListItem Value="0">* Seleccione origen *</asp:ListItem>
          </asp:DropDownList>
          <%--</a>--%>
        </td>
        </tr>
       <tr>
         <td class="bt-bottom bt-right bt-left">Generados desde / hasta:</td>
         <td class="bt-bottom bt-right">
            <%--<a class="tooltip" >
            <span class="classic">Fecha desde.</span>--%>
            <asp:TextBox ID="tfechaini" runat="server" Width="120px" MaxLength="10" CssClass="datetimepicker"
             onkeypress="return soloLetras(event,'01234567890/')" style="text-align: center" 
                 onblur="valDate(this,true,valdate);" ClientIDMode="Static"></asp:TextBox>
            <%--</a>
            <a class="tooltip" >
            <span class="classic">Fecha hasta.</span>--%>
            <asp:TextBox ID="tfechafin" runat="server" ClientIDMode="Static" Width="120px" MaxLength="15" CssClass="datetimepicker"
             onkeypress="return soloLetras(event,'01234567890/')"  style="text-align: center" 
                  onblur="valDate(this,true,valdate);"></asp:TextBox>
            <%--</a>--%>
          </td>
        </tr>
       <tr style=" display:None"><td class="bt-bottom  bt-right bt-left">Estado:</td>
       <td class="bt-bottom bt-right">
          <%--<a class="tooltip" >
          <span class="classic">Esta informacion contiene los estados disponibles.</span>--%>
          <asp:DropDownList ID="dpestados" runat="server" Width="250px"  >
                 <asp:ListItem Value="0">* Seleccione origen *</asp:ListItem>
          </asp:DropDownList>
          <%--</a>--%>
       </td>
       </tr>
       <tr style=" display:none">
         <td class="bt-bottom bt-right bt-left">Todas las solicitudes.</td>
         <td class="bt-bottom bt-right">
             <asp:CheckBox Text="" ID="chkTodos" runat="server" AutoPostBack="false"
                 oncheckedchanged="chkTodos_CheckedChanged" />
         </td>
       </tr>
       </table>
       <div class="botonera">
            <span id="imagen"></span>
             <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda" OnClientClick="return valRucCiaTrans();"
               onclick="btbuscar_Click"/>
       </div>
       </div>
       </div>
        <div class="cataresult" >
        <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
        <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
        <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
        <div class="msg-alerta" id="alerta" runat="server" style=" display:none" ></div>
        <div id="xfinder" runat="server" visible="false" >
        <div class="findresult" >
        <%--<div class="booking" >--%>
        <div class="informativo" style=" height:100%;">
             <%--    <div class="separator">Solicitudes disponibles:</div>--%>
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server"  >
                 <HeaderTemplate>
                 <table id="tablasort"  cellspacing="1" cellpadding="1" border="solid"  class="tabRepeat">
                 <thead>
                 <tr>
                 <th># Solicitud</th>
                 <th style=" display:none">Ruc</th>
                 <th style=" display:none">Tipo</th>
                 <th>Empresa</th>
                 <th>Tipo de Empresa</th>
                 <th>Tipo de Solicitud</th>
                 <th>Fecha Registro</th>
                 <th>Usuario Atiende</th>
                 <th>Estado</th>
                 <th style=" display:none">AsoTransporte</th>
                 <th></th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td style=" width:60px;" title="# Solicitud"><%#Eval("NUMSOLICITUD")%></td>
                  <td style=" display:none"><asp:Label ID="lruccipas"  style="text-transform :uppercase" runat="server" Text='<%# Eval("RUCCIPAS") %>'></asp:Label></td>
                  <td style=" display:none"><%#Eval("TIPO")%></td>
                  <td title="Empresa"><asp:Label ID="lempresa"  style="text-transform :uppercase" runat="server" Text='<%# Eval("EMPRESA") %>'></asp:Label></td>
                  <td title="Tipo de Empresa"><%#Eval("TIPOEMPRESA")%></td>
                  <td title="Tipo de Solicitud"><%#Eval("TIPOSOLICITUD")%></td>
                  <td title="Fecha de Registro" style=" width:80px"><%#Eval("FECHAING")%></td>
                  <td title="Usuario Atiende" style=" width:100px;text-transform :uppercase"><%#Eval("USUARIOMOD")%></td>
                  <td title="Estado"><%#Eval("ESTADO")%></td>
                  <td style=" display:none"><%#Eval("ASO_TRANSPORTE")%></td>
                  <td title="Ver detalle de la Solicitud" style=" width:70px">
                    <a style=" width:70px" id="adjDoc" class="topopup" onclick="redirectsol('<%# Eval("NUMSOLICITUD") %>', '<%# Eval("TIPO") %>', '<%#Eval("TIPOSOLICITUD")%>', '<%#Eval("ESTADO")%>', '<%# Eval("RUCCIPAS") %>', '<%# Eval("ASO_TRANSPORTE") %>');">
                    <i class="ico-find" ></i> Ver 
                    </a>
                  </td>
                 </tr>
                 </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>
                </div>
        </div>
        </div>
        </div>
            <div id="sinresultado" runat="server" class="msg-info" style=" display:none"></div>
        </ContentTemplate>
                     <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                     </Triggers>
        </asp:UpdatePanel>
        </div>
 </div>
 </form>
  <script src="../Scripts/pages.js" type="text/javascript"></script>
 <script src="../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
    <script type="text/javascript">
        (function ($) {
            $("#header").ready(function () { $("#cargando").stop().animate({ width: "25%" }, 1500) });
            $("#footer").ready(function () { $("#cargando").stop().animate({ width: "75%" }, 1500) });
            $(window).load(function () {
                $("#cargando").stop().animate({ width: "100%" }, 600, function () {
                    $("#cargando").fadeOut("fast", function () { $(this).remove(); });
                });
            });
        })($);
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });
        function popupCallback(objeto, catalogo) {
            if (objeto == null || objeto == undefined) {
                alert('Hubo un problema al setaar un objeto de catalogo');
                return;
            }
            //si catalogos es booking
            if (catalogo == 'bk') {
                document.getElementById('numbook').textContent = objeto.nbr;
                document.getElementById('nbrboo').value = objeto.nbr;
                return;
            }

            //si catalogos es booking
            if (catalogo == 'cc') {
                document.getElementById('txtfecha').textContent = objeto.fecha;
                document.getElementById('xfecha').value = objeto.fecha;
                return;
            }

        }

        function clear() {
            document.getElementById('numbook').textContent = '...';
            document.getElementById('nbrboo').value = '';
        }
        function clear2() {
            document.getElementById('txtfecha').textContent = '...';
            document.getElementById('xfecha').value = '';
        }

        var programacion = {};
        var lista = [];
        function prepareObject() {

            try {
                document.getElementById("loader").className = '';
                lista = [];
                //validaciones básicas
                var vals = document.getElementById('nbrboo');
                if (vals == null || vals == undefined || vals.value.trim().length <= 2) {
                    alert('* Datos de programación*Escriba el numero de Booking*');
                    document.getElementById("loader").className = 'nover';
                    return;
                }
                vals = document.getElementById('xfecha');
                if (vals == null || vals == undefined || vals.value.trim().length <= 5) {
                    alert('* Datos de programaciónEscriba la fecha de programación');
                    document.getElementById("loader").className = 'nover';
                    return;
                }
                vals = document.getElementById('tmail');
                if (vals == null || vals == undefined || vals.value.trim().length <= 5) {
                    alert('* Datos de programación*Escriba el correo electrónico para la notificación');
                    document.getElementById("loader").className = 'nover';
                    return;
                }
                this.programacion.booking = document.getElementById('nbrboo').value;
                this.programacion.fecha_pro = document.getElementById('xfecha').value;
                this.programacion.mail = document.getElementById('tmail').value;
                this.programacion.idlinea = document.getElementById('idlin').value;
                this.programacion.linea = document.getElementById('agencia').value;
                this.programacion.total = document.getElementById('diponible').value;

                //recorrer tabla->
                var tbl = document.getElementById('tabla');
                for (var f = 0; f < tbl.rows.length; f++) {
                    var celColect = tbl.rows[f].getElementsByTagName('td');
                    if (celColect != undefined && celColect != null && celColect.length > 0) {

                        var tdetalle = {
                            num: celColect[0].textContent,
                            desde: celColect[1].textContent,
                            hasta: celColect[2].textContent,
                            dispone: celColect[4].textContent,
                            idh: celColect[5].textContent,
                            idd: celColect[6].textContent,
                            total: celColect[7].textContent
                        };
                        tdetalle.reserva = celColect[8].getElementsByTagName('input')[0].value;
                        this.lista.push(tdetalle);
                    }
                }
                this.programacion.detalles = this.lista;
                var qtlimite = parseInt(document.getElementById('diponible').value);
                var total = 0;
                for (var n = 0; n < this.lista.length; n++) {
                    if (lista[n].reserva != '') {
                        if (parseInt(lista[n].dispone) < parseInt(lista[n].reserva)) {
                            alert('El Horario ' + lista[n].desde + '-' + lista[n].hasta + ' excede su disponibilidad, favor verifique');
                            return;
                        }
                        total += parseInt(lista[n].reserva);
                    }
                }
                if (total > qtlimite) {
                    alert('* ReservaLa cantidad de reserva excede el cupo disponible \n Cupo: ' + qtlimite + '\n Reserva: ' + total);
                    return;
                }
                if (total <= 0) {
                    alert('* ReservaLa cantidad de reservas debe ser mayor que 0');
                    return;
                }
                tansporteServer(this.programacion, 'turnos.aspx/ValidateJSON');

            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

        function openPop() {
            var bo = document.getElementById('nbrboo').value;
            if (bo == undefined || bo == '' || bo == null) {
                alert('Por favor seleccione el booking primero');
                return;
            }
            window.open('../catalogo/calendarioConsolidacion.aspx?bk=' + bo, 'name', 'width=850,height=880')
        }
        function redirectsol(val, tipo, tiposolicitud, estado, idemp, asotransporte) {
            var caja = val;
            if (estado == 'PAGO CONFIRMADO') {
                window.open('../transportista/consulta-solicitud-colaborador-info/?numsolicitud=' + caja);
            }
            else {
                window.open('../transportista/consulta-solicitud-colaborador/?numsolicitud=' + caja);
            }
        }
        function getGif() {
            document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif"/>';
            return true;
        }
        function getGifOculta() {
            document.getElementById('imagen').innerHTML = '<img alt="" src=""/>';
            return true;
        }
        function valRucCiaTrans() {
            getGif();
            var vals = document.getElementById('<%=txtruccipas.ClientID %>').value;
            if (vals == null || vals == undefined || vals.value.trim().length > 0) {
                if (!valruccipasservidor()) {
                    getGifOculta();
                    return false;
                };
            }
            return true;
        }
        function valruccipasservidor() {
            //codigo = control.value.trim().toUpperCase();
            var valruccipas = document.getElementById('<%=txtruccipas.ClientID %>').value;
            if (!/^([0-9])*$/.test(valruccipas)) {
                alert('* No. RUC No es un Numero. *');
                document.getElementById('<%=txtruccipas.ClientID %>').focus();
                document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:120px;";
                document.getElementById("loader").className = 'nover';
                return false;
            }
            else if (valruccipas == '' || valruccipas == null || valruccipas == undefined) {
                alert('* Escriba el No. de RUC. *');
                document.getElementById('<%=txtruccipas.ClientID %>').focus();
                document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:120px;";
                document.getElementById("loader").className = 'nover';
                return false;
            }
            var numeroProvincias = 24;
            var numprov = valruccipas.substr(0, 2);
            if (numprov > numeroProvincias) {
                alert('* El código de la provincia (dos primeros dígitos) es inválido! *');
                document.getElementById('<%=txtruccipas.ClientID %>').focus();
                document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:120px;";
                document.getElementById("loader").className = 'nover';
                return false;
            }
            //validadocrucservidor(valruccipas);
            if (!validadocrucservidor(valruccipas)) {
                return false;
            };

            return true;
        }
        function validadocrucservidor(campo) {

            var numero = campo;
            var suma = 0;
            var residuo = 0;
            var pri = false;
            var pub = false;
            var nat = false;
            var numeroProvincias = 24;
            var modulo = 11;

            if (campo.length < 13) {
                alert('* No. RUC. INCOMPLETO. *');
                document.getElementById('<%=txtruccipas.ClientID %>').focus();
                document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:120px;";
                document.getElementById("loader").className = 'nover';
                return false;
            }
            if (campo.length > 13) {
                alert('* El valor no corresponde a un No. de RUC. *');
                document.getElementById('<%=txtruccipas.ClientID %>').focus();
                document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:120px;";
                document.getElementById("loader").className = 'nover';
                return false;
            }
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
                alert('* El tercer dígito ingresado es inválido. *');
                document.getElementById('<%=txtruccipas.ClientID %>').focus();
                document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:120px;";
                document.getElementById("loader").className = 'nover';
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
                    alert('* El RUC de la empresa del sector público es incorrecto. *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:120px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                /* El ruc de las empresas del sector publico terminan con 0001*/
                if (numero.substr(9, 4) != '0001') {
                    alert('* El RUC de la empresa del sector público debe terminar con 0001. *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:120px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
            }
            else if (pri == true) {
                if (digitoVerificador != d10) {
                    alert('* El RUC de la empresa del sector privado es incorrecto. *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:120px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (numero.substr(10, 3) != '001') {
                    alert('* El RUC de la empresa del sector privado debe terminar con 001. *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:120px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
            }
            else if (nat == true) {
                if (digitoVerificador != d10) {
                    alert('* El número de cédula de la persona natural es incorrecto. *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:120px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (numero.length > 10 && numero.substr(10, 3) != '001') {
                    alert('* El RUC de la persona natural debe terminar con 001. *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:120px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
            }
            return true;
        }
    </script>
    </body>
</html>

