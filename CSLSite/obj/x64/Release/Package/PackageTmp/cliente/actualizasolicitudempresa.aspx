<%@ Page Language="C#" AutoEventWireup="true" Title="Actualiza Solicitud Registro de Empresa"
CodeBehind="actualizasolicitudempresa.aspx.cs" Inherits="CSLSite.cliente.actualizasolicitudempresa" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

     <!--mensajes-->
        <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
        <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>
        <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>
    <!--external css-->
  <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
  <link rel="stylesheet" type="text/css" href="../lib/gritter/css/jquery.gritter.css" />

    <%--<title>Catálogo de agentes</title>
    <link href="../shared/estilo/catalogosolicitudempresa.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <link href="Inicio/base-site.css" rel="stylesheet" type="text/css" />
    <link href="Inicio/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/x-icon" href="favicon.ico" />
    <link href="shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../Inicio/base-site.css" rel="stylesheet" type="text/css" />
    <link href="../Inicio/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link rel="../shortcut icon" type="image/x-icon" href="../favicon.ico" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
     * input[type=text]
        {
            text-align:left!important;
        }
    </style>--%>
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
    <div class="dashboard-container p-4" id="cuerpo" runat="server">
     <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
    <form id="bookingfrm" runat="server">
    <input id="zonaid" type="hidden" value="7" />
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

     <div class="form-row">
             <div class="form-title col-md-12">
             <a class="btn btn-outline-primary mr-4" href="#" runat="server" id="aprint" clientidmode="Static" >1</a>
             <a class="level1" runat="server" id="a1" clientidmode="Static" >Tipo de Cliente - Empresa</a>
             </div>

             <div class="form-group col-md-12" >
                 <label for="inputAddress">1. Tipo de Cliente:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                    <asp:TextBox ID="txttipcli" runat="server" MaxLength="500" Enabled="false" style="text-align: center;"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)" CssClass="form-control"></asp:TextBox>
            </div>

        </div>

        <div class="form-row">
            <div class="form-title col-md-12">
                <a class="btn btn-outline-primary mr-4" href="#" target="_blank" runat="server" id="a4" clientidmode="Static" >2</a>
                <a class="level1"  runat="server" id="a5" clientidmode="Static" >Información del Cliente</a>
            </div>

            <div class="form-group col-md-6">
                <label class="form-control" >2. Nombre/Razón Social:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                <asp:TextBox ID="txtrazonsocial" runat="server" MaxLength="500" Enabled="false" style="text-align: center;text-transform:uppercase" 
                onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="form-group col-md-6">
                <label class="form-control" >3. RUC, C.I o Pasaporte:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                <asp:TextBox ID="txtruccipas" runat="server" MaxLength="25" Enabled="false" style="text-align: center"
                onkeypress="return soloLetras(event,'01234567890abcdefghijklnmñopqrstuvwxyz',true)" CssClass="form-control"></asp:TextBox>
            </div> 

            <div class="form-group col-md-6">
                <label class="form-control" >4. Actividad Comercial:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                <asp:TextBox ID="txtactividadcomercial" runat="server" MaxLength="500" Enabled="false" style="text-align: center" 
                onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)" CssClass="form-control"></asp:TextBox>
            </div> 

            <div class="form-group col-md-6">
                <label class="form-control" >5. Dirección Oficina:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                <asp:TextBox ID="txtdireccion" runat="server" MaxLength="500" Enabled="false" style="text-align: center" 
                onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890/.-_ ',true)" CssClass="form-control"></asp:TextBox>
            </div> 

            <div class="form-group col-md-6">
                <label class="form-control" >6. Teléfono Oficina:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                <asp:TextBox ID="txttelofi" runat="server" MaxLength="9" Enabled="false" style="text-align: center"
                onkeypress="return soloLetras(event,'01234567890',true)" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="form-group col-md-6">
                <label class="form-control" >7. Persona Contacto:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                <asp:TextBox ID="txtcontacto" runat="server" MaxLength="500" Enabled="false" style="text-align: center;text-transform:uppercase" 
                onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)" CssClass="form-control"></asp:TextBox>
            </div> 

            <div class="form-group col-md-6">
                <label class="form-control" >8. Celular Contacto:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                 <asp:TextBox ID="txttelcelcon" runat="server" MaxLength="10" Enabled="false" style="text-align: center"
                onkeypress="return soloLetras(event,'01234567890',true)" CssClass="form-control"></asp:TextBox>
            </div> 

            <div class="form-group col-md-6">
                <label class="form-control" >9. Mail Contacto:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                <asp:TextBox runat="server" id="tmailinfocli" Enabled="false" CssClass="form-control"></asp:TextBox>
            </div> 

            <div class="form-group col-md-6">
                <label class="form-control" >10. Mail EBilling:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                <asp:TextBox runat="server" id="tmailebilling" Enabled="false" CssClass="form-control"></asp:TextBox>
            </div> 

            <div class="form-group col-md-6">
                <label class="form-control" >11. Certificaciones:<span style="color: #FF0000; font-weight: bold;"> </span></label>
                <asp:TextBox ID="txtcertificaciones" runat="server" MaxLength="500" Enabled="false" style="text-align: center"
                    onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890;./-_ ',true)" CssClass="form-control"></asp:TextBox>
            </div> 

            <div class="form-group col-md-6">
                <label class="form-control" >12. Sitio Web:<span style="color: #FF0000; font-weight: bold;"> </span></label>
                <asp:TextBox id='turl' runat="server" style= 'font-size:small' enableviewstate="false" clientidmode="Static" Enabled="false"
                onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890_:.-/;',true)" maxlength="250" CssClass="form-control"> </asp:TextBox>
            </div> 

            <div class="form-group col-md-6">
                <label class="form-control" >13. Afiliación a Gremios:<span style="color: #FF0000; font-weight: bold;"> </span></label>
                <asp:TextBox ID="txtafigremios" runat="server" MaxLength="1000" Enabled="false" style="text-align: center"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890;./-_ ',true)" CssClass="form-control"></asp:TextBox>
            </div> 

            <div class="form-group col-md-6">
                <label class="form-control" >14. Referencia Comercial:<span style="color: #FF0000; font-weight: bold;"> </span></label>
                <asp:TextBox ID="txtrefcom" runat="server" MaxLength="500" Enabled="false" style="text-align: center" 
                 onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890;./-_ ',true)" CssClass="form-control"></asp:TextBox>
            </div>

        </div>

        <div class="form-row">
            <div class="form-title col-md-12">
                <a class="btn btn-outline-primary mr-4" href="#" target="_blank" runat="server" id="a2" clientidmode="Static" >3</a>
                <a class="level1" runat="server" id="a3" clientidmode="Static" >Información del Representante Legal</a>
            </div>

            <div class="form-group col-md-6">
                <label class="form-control" >15. Representante Legal:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                <asp:TextBox ID="txtreplegal" runat="server" MaxLength="500" style="text-align: center;text-transform:uppercase" Enabled="false"
                 onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)" CssClass="form-control" ></asp:TextBox>
            </div> 

            <div class="form-group col-md-6">
                <label class="form-control" >16. Teléfono Domicilio:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                <asp:TextBox ID="txttelreplegal" runat="server" MaxLength="9" style="text-align: center" Enabled="false"
                onkeypress="return soloLetras(event,'01234567890',true)" CssClass="form-control"></asp:TextBox>
            </div> 

            <div class="form-group col-md-6">
                <label class="form-control" >17. Dirección Domiciliaria:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                <asp:TextBox ID="txtdirdomreplegal" runat="server" MaxLength="500" style="text-align: center" Enabled="false"
                onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ./_-',true)" CssClass="form-control"></asp:TextBox>
            </div> 

            <div class="form-group col-md-6">
                <label class="form-control" >18. Cédula de Identidad:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                <asp:TextBox ID="txtci" runat="server" MaxLength="10" style="text-align: center"  Enabled="false"
                onkeypress="return soloLetras(event,'01234567890',true)" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="form-group col-md-12">
                <label class="form-control" >19. Mail:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                <asp:TextBox runat="server" id="tmailRepLegal" Enabled="false" CssClass="form-control"/>
            </div>
        </div>

        <div class="form-group col-md-12">
       <div class="cataresult" >
       <div id="xfinder" runat="server">
       <div class="findresult" >
        <div class="booking">
        <div class="bokindetalle" style=" overflow:auto; font-size:inherit">
         <div class="bokindetalle">
            <table id="tablerp" cellpadding="1" cellspacing="0">
            <asp:Repeater ID="tablePaginationDocumentos" runat="server">
            <HeaderTemplate>
            <table id="tablar"  cellspacing="1" cellpadding="1" class="table table-bordered invoice">
            <thead>
            <tr>
            <th>Tipo de Empresa</th>
            <th>Documentos</th>
            <th></th>
            <th>Actualizar Documento</th>
            <th style="display:none"># Solicitud</th>
            <th style="display:none">Id Documento</th>
            </tr>
            </thead> 
            <tbody>
            </HeaderTemplate>
            <ItemTemplate>
            <tr class="point">
            <td ><%#Eval("TIPOEMPRESA")%></td>
            <td style="font-size:inherit"><%#Eval("DOCUMENTO")%></td>
            <td >
                <a href='<%#Eval("RUTADOCUMENTO") %>'  class="topopup" target="_blank">
                    <i class="fa fa-search"></i> Ver Documento </a>
            </td>
            <td >
            <asp:FileUpload extension='<%#Eval("EXTENSION")%>' class="btn btn-outline-primary mr-4" id="fsupload" title="Escoja el archivo con formato indicado en la siguiente columna."
                       onchange="validaextension(this)" style=" font-size:small" runat="server"/>
            </td>
            <td style="display:none">
                <asp:Label Text='<%#Eval("NUMSOLICITUD")%>' runat="server" ID="lblNumSolicitud" ClientIDMode="Static" />
            </td>
            <td style="display:none">
                <asp:Label Text='<%#Eval("IDSOLDOC")%>' runat="server" ID="lblSolDoc" ClientIDMode="Static"/>
            </td>
            </tr>
            </ItemTemplate>
            <FooterTemplate>
            </tbody>
            </table>
            </FooterTemplate>
            </asp:Repeater>
        </table>
        </div>
        </div>
        </div>
                         <div class="alert alert-warning" runat="server" id="alerta" >
                            Confirme que los documentos sean los correctos.
             </div>
                   <div id="sinresultado" runat="server" class="alert-danger" >
              No se encontraron resultados, 
              asegurese que ha seleccionado correctamente un tipo de solicitud.
              </div>
        </div>
        </div>

       <div class="col-md-12 d-flex justify-content-rigth" runat="server" id="salir" visible="false">
        <asp:Button ID="btnSalir" runat="server" Text="Salir" onclick="btnSalir_Click" 
                ToolTip="Regresa a la Pantalla Consultar Solicitud." CssClass="btn btn-outline-primary mr-4"/>
       </div>
       <div class="col-md-12 d-flex justify-content-rigth" runat="server" id="botonera">
       <span id="imagen"></span>
       <asp:Button ID="btsalvar" runat="server" Text="Actualizar" onclick="btsalvar_Click"
               OnClientClick="return prepareObject('¿Esta seguro de actualizar la solicitud?');" ToolTip="Actualiza la solicitud." CssClass="btn btn-primary mr-4"/>
        </div>
      </div>
      </div>
    </form>
    </div>
    <script src="../../Scripts/pages.js" type="text/javascript"></script>
    <script src="../../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
   <script type="text/javascript" >
       var ced_count = 0;
       var jAisv = {};
       function setObject(row) {
//           var celColect = row.getElementsByTagName('td');
//          var bookin = {
//              fila: celColect[0].textContent,
//              gkey: celColect[1].textContent,
//              nbr: celColect[2].textContent,
//              linea: celColect[3].textContent,
//              fk: celColect[4].textContent
//              };
//            if (window.opener != null) {
//                window.opener.popupCallback(bookin, 'bk');
//            }
            self.close();
        }
        function prepareObject(valor) {
            try {
                if (confirm(valor) == false) {
                    return false;
                }
                var vals = document.getElementById('<%=txtrazonsocial.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Datos de Registro de Empresa: *\n * Escriba la Razon Social *');
                    document.getElementById('<%=txtrazonsocial.ClientID %>').focus();
                    <%--document.getElementById('<%=txtrazonsocial.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                if (!valruccipasservidor()) {
                    return false;
                };
                var vals = document.getElementById('<%=txtactividadcomercial.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Datos de Registro de Empresa: *\n * Escriba la Actividad Comercial *');
                    document.getElementById('<%=txtactividadcomercial.ClientID %>').focus();
                    <%--document.getElementById('<%=txtactividadcomercial.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                var vals = document.getElementById('<%=txtdireccion.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Datos de Registro de Empresa: *\n * Escriba la Dirección de Oficina *');
                    document.getElementById('<%=txtdireccion.ClientID %>').focus();
                    <%--document.getElementById('<%=txtdireccion.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                var vals = document.getElementById('<%=txttelofi.ClientID %>').value;
                if (!/^([0-9])*$/.test(vals)) {
                    alert('* Datos de Registro de Empresa: *\n * Teléfono de Oficina No es un Numero *');
                    document.getElementById('<%=txttelofi.ClientID %>').focus();
                    <%--document.getElementById('<%=txttelofi.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                if (vals.length < 9) {
                    alert('* Datos de Registro de Empresa: *\n * Teléfono de Oficina Incompleto *');
                    document.getElementById('<%=txttelofi.ClientID %>').focus();
                    <%--document.getElementById('<%=txttelofi.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Datos de Registro de Empresa: *\n * Escriba el Teléfono de Oficina *');
                    document.getElementById('<%=txttelofi.ClientID %>').focus();
                    <%--document.getElementById('<%=txttelofi.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                var vals = document.getElementById('<%=txtcontacto.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Datos de Registro de Empresa: *\n * Escriba la Persona de Contacto *');
                    document.getElementById('<%=txtcontacto.ClientID %>').focus();
                    <%--document.getElementById('<%=txtcontacto.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                var vals = document.getElementById('<%=txttelcelcon.ClientID %>').value;
                if (!/^([0-9])*$/.test(vals)) {
                    alert('* Datos de Registro de Empresa: *\n * No. de Celular de Contacto No es un Numero *');
                    document.getElementById('<%=txttelcelcon.ClientID %>').focus();
                    <%--document.getElementById('<%=txttelcelcon.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                if (vals.length < 10) {
                    alert('* Datos de Registro de Empresa: *\n * No. de Celular de Contacto Incompleto *');
                    document.getElementById('<%=txttelcelcon.ClientID %>').focus();
                    <%--document.getElementById('<%=txttelcelcon.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Datos de Registro de Empresa: *\n * Escriba el No. de Celular de Contacto *');
                    document.getElementById('<%=txttelcelcon.ClientID %>').focus();
                    <%--document.getElementById('<%=txttelcelcon.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                var mail1 = document.getElementById('<%=tmailinfocli.ClientID %>').value;
                if (mail1 == null || mail1 == undefined || mail1 == '') {
                    alert('* Datos de Registro de Empresa: *\n * Escriba al menos un mail *');
                    document.getElementById('<%=tmailinfocli.ClientID %>').focus();
                    <%--document.getElementById('<%=tmailinfocli.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                if (mail1.trim().length > 0) {
                    if (!validarEmail(mail1)) {
                        //alert('* Datos de Registro de Empresa: *\n * Mail no parece correcto *')
                        document.getElementById('<%=tmailinfocli.ClientID %>').focus();
                        <%--document.getElementById('<%=tmailinfocli.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                        return false;
                    }
                }
                var mail1 = document.getElementById('<%=tmailebilling.ClientID %>').value;
                if (mail1 == null || mail1 == undefined || mail1 == '') {
                    alert('* Datos de Registro de Empresa: *\n * Escriba al menos un mail *');
                    document.getElementById('<%=tmailebilling.ClientID %>').focus();
                    <%--document.getElementById('<%=tmailebilling.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                if (mail1.trim().length > 0) {
                    if (!validarEmail(mail1)) {
                        alert('* Datos de Registro de Empresa: *\n * Mail no parece correcto *')
                        document.getElementById('<%=tmailebilling.ClientID %>').focus();
                        <%--document.getElementById('<%=tmailebilling.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                        return false;
                    }
                }
                var vals = document.getElementById('<%=txtreplegal.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Datos de Registro de Empresa: *\n * Escriba el Representante Legal *');
                    document.getElementById('<%=txtreplegal.ClientID %>').focus();
                    <%--document.getElementById('<%=txtreplegal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                var vals = document.getElementById('<%=txttelreplegal.ClientID %>').value;
                if (!/^([0-9])*$/.test(vals)) {
                    alert('* Datos de Registro de Empresa: *\n * Teléfono de Domicilio No es un Numero *');
                    document.getElementById('<%=txttelreplegal.ClientID %>').focus();
                    <%--document.getElementById('<%=txttelreplegal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                if (vals.length < 9) {
                    alert('* Datos de Registro de Empresa: *\n * Teléfono de Domicilio Incompleto *');
                    document.getElementById('<%=txttelreplegal.ClientID %>').focus();
                    <%--document.getElementById('<%=txttelreplegal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Datos de Registro de Empresa: *\n * Escriba el Teléfono de Domicilio *');
                    document.getElementById('<%=txttelreplegal.ClientID %>').focus();
                    <%--document.getElementById('<%=txttelreplegal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                var vals = document.getElementById('<%=txtdirdomreplegal.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Datos de Registro de Empresa: *\n * Escriba la Dirección Domiciliaria *');
                    document.getElementById('<%=txtdirdomreplegal.ClientID %>').focus();
                    <%--document.getElementById('<%=txtdirdomreplegal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                //                var vals = document.getElementById('<%=txtci.ClientID %>').value;
                //                if (valruccipas == '' || valruccipas == null || valruccipas == undefined) {
                //                    alert('* Datos de Registro de Empresa: *\n * Escriba el No. de CI *');
                //                    document.getElementById('<%=txtci.ClientID %>').focus();
                //                    document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";
                //                    return false;
                //                }
                var mail1 = document.getElementById('<%=tmailRepLegal.ClientID %>').value;
                if (mail1 == null || mail1 == undefined || mail1 == '') {
                    alert('* Datos de Registro de Empresa: *\n * Escriba al menos un mail *');
                    document.getElementById('<%=tmailRepLegal.ClientID %>').focus();
                    <%--document.getElementById('<%=tmailRepLegal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                if (mail1.trim().length > 0) {
                    if (!validarEmail(mail1)) {
                        alert('* Datos de Registro de Empresa: *\n * Mail de Representante Legal no parece correcto *')
                        document.getElementById('<%=tmailRepLegal.ClientID %>').focus();
                        <%--document.getElementById('<%=tmailRepLegal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                        return false;
                    }
                }
                if (!validaciservidor()) {
                    return false;
                };
                //Valida Documentos
                lista = [];
                var tbl = document.getElementById('tablar');
                for (var f = 0; f < tbl.rows.length; f++) {
                    var celColect = tbl.rows[f].getElementsByTagName('td');
                    if (celColect != undefined && celColect != null && celColect.length > 0) {
                        if (celColect[3].getElementsByTagName('input')[0].value != '') {
                            var tdetalle = {
                                documento: celColect[3].getElementsByTagName('input')[0].value
                            };
                            this.lista.push(tdetalle);
                        }
                    }
                }
                if (lista.length > 0) {
                    var nomdoc = null;
                    for (var n = 0; n < this.lista.length; n++) {
                        if (nomdoc == lista[n].documento) {
                            alert('* Datos de Registro de Empresa: *\n * Existen archivos repetidos, revise por favor *');
                            return false;
                        }
                        nomdoc = lista[n].documento;
                    }
                }
                document.getElementById('imagen').innerHTML = '<img alt="loading.."" src="../../shared/imgs/loader.gif"/>';
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }
        function validaciservidor() {
            var valruccipas = document.getElementById('<%=txtci.ClientID %>').value;
            if (!/^([0-9])*$/.test(valruccipas)) {
                alert('* Datos de Registro de Empresa: *\n * No. C.I. No es un Numero *');
                document.getElementById('<%=txtci.ClientID %>').focus();
                <%--document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                return false;
            }
            else if (valruccipas == '' || valruccipas == null || valruccipas == undefined) {
                alert('* Datos de Registro de Empresa: *\n * Escriba el No. de C.I. *');
                document.getElementById('<%=txtci.ClientID %>').focus();
                <%--document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                return false;
            }
            if (valruccipas.length = 0) {
                alert('* Datos de Registro de Empresa: *\n * Escriba el No. de C.I. *');
                document.getElementById('<%=txtci.ClientID %>').focus();
                <%--document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                return false;
            }
            if (valruccipas.length < 10) {
                alert('* Datos de Registro de Empresa: *\n * No. C.I. INCOMPLETO! *');
                document.getElementById('<%=txtci.ClientID %>').focus();
                <%--document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                return false;
            }
            var array = valruccipas.split("");
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
                    alert('* Datos de Registro de Empresa: *\n * No. C.I. no válido! *');
                    document.getElementById('<%=txtci.ClientID %>').focus();
                    <%--document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
            }
            else {
                if (final != 10) {
                    alert('* Datos de Registro de Empresa: *\n * No. C.I. no válido! *');
                    document.getElementById('<%=txtci.ClientID %>').focus();
                    <%--document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
            }
            return true;
        }
        function valruccipasservidor() {
            //codigo = control.value.trim().toUpperCase();
            var valruccipas = document.getElementById('<%=txtruccipas.ClientID %>').value;
            //var vruc = document.getElementById('rbruc').checked;
            //var vci = document.getElementById('rbci').checked;
            //var vpas = document.getElementById('rbpasaporte').checked;
            if (valruccipas == null || valruccipas == undefined || valruccipas == '') {
                alert('* Datos de Registro de Empresa: *\n * Escriba el No. de RUC,CI o Pasaporte *');
                document.getElementById('<%=txtruccipas.ClientID %>').focus();
                <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                return false;
            }
            if (valruccipas.length < 10) {
                alert('* Datos de Registro de Empresa: *\n * No. de CI IMCOMPLETO *');
                document.getElementById('<%=txtruccipas.ClientID %>').focus();
                <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                return false;
            }
            if (valruccipas.length > 10 && valruccipas.length < 13) {
                alert('* Datos de Registro de Empresa: *\n * No. de RUC IMCOMPLETO *');
                document.getElementById('<%=txtruccipas.ClientID %>').focus();
                <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                return false;
            }
            if (valruccipas.length == 13) {
                if (!/^([0-9])*$/.test(valruccipas)) {
                    alert('* Datos de Registro de Empresa: *\n * No. RUC No es un Numero *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                else if (valruccipas == '' || valruccipas == null || valruccipas == undefined) {
                    alert('* Datos de Registro de Empresa: *\n * Escriba el No. de RUC *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                var numeroProvincias = 24;
                var numprov = valruccipas.substr(0, 2);
                if (numprov > numeroProvincias) {
                    alert('* Datos de Registro de Empresa: *\n * El código de la provincia (dos primeros dígitos) es inválido! *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                validadocrucservidor(valruccipas);
            }
            if (valruccipas.length == 10) {
                if (!/^([0-9])*$/.test(valruccipas)) {
                    alert('* Datos de Registro de Empresa: *\n * No. C.I. No es un Numero *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                else if (valruccipas == '' || valruccipas == null || valruccipas == undefined) {
                    alert('* Datos de Registro de Empresa: *\n * Escriba el No. de C.I. *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                if (valruccipas.length = 0) {
                    alert('* Datos de Registro de Empresa: *\n * Escriba el No. de C.I. *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                if (valruccipas.length < 10) {
                    alert('* Datos de Registro de Empresa: *\n * No. C.I. INCOMPLETO! *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                var array = valruccipas.split("");
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
                        alert('* Datos de Registro de Empresa: *\n * No. C.I. no válido! *');
                        document.getElementById('<%=txtruccipas.ClientID %>').focus();
                        <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                        return false;
                    }
                }
                else {
                    if (final != 10) {
                        alert('* Datos de Registro de Empresa: *\n * No. C.I. no válido! *');
                        document.getElementById('<%=txtruccipas.ClientID %>').focus();
                        <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                        return false;
                    }
                }
            }
            if (valruccipas.length > 13) {
                if (valruccipas == '' || valruccipas == null || valruccipas == undefined) {
                    alert('* Datos de Registro de Empresa: *\n * Escriba el No. Pasaporte *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
            }
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
                alert('* Datos de Registro de Empresa: *\n * No. RUC. INCOMPLETO! *');
                document.getElementById('<%=txtruccipas.ClientID %>').focus();
                <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                return false;
            }
            if (campo.length > 13) {
                alert('* Datos de Registro de Empresa: *\n * El valor no corresponde a un No. de RUC *');
                document.getElementById('<%=txtruccipas.ClientID %>').focus();
                <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
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
                alert('* Datos de Registro de Empresa: *\n * El tercer dígito ingresado es inválido! *');
                document.getElementById('<%=txtruccipas.ClientID %>').focus();
                <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
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
                    alert('* Datos de Registro de Empresa: *\n * El RUC de la empresa del sector público es incorrecto! *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                /* El ruc de las empresas del sector publico terminan con 0001*/
                if (numero.substr(9, 4) != '0001') {
                    alert('* Datos de Registro de Empresa: *\n * El RUC de la empresa del sector público debe terminar con 0001! *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
            }
            else if (pri == true) {
                if (digitoVerificador != d10) {
                    alert('* Datos de Registro de Empresa: *\n * El RUC de la empresa del sector privado es incorrecto! *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                if (numero.substr(10, 3) != '001') {
                    alert('* Datos de Registro de Empresa: *\n * El RUC de la empresa del sector privado debe terminar con 001! *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
            }
            else if (nat == true) {
                if (digitoVerificador != d10) {
                    alert('* Datos de Registro de Empresa: *\n * El número de cédula de la persona natural es incorrecto! *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                if (numero.length > 10 && numero.substr(10, 3) != '001') {
                    alert('* Datos de Registro de Empresa: *\n * El RUC de la persona natural debe terminar con 001! *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
            }
            return true;
        }
        function validarEmail(email) {
            expr = /^([a-zA-Z0-9@;_\.\-])+\@(([a-zA-Z0-9@;\-])+\.)+([a-zA-Z0-9@;]{2,4})+$/; ;
            if (!expr.test(email)) {
                return false;
            }
            return true;
        }
        function initFinder() {
            if (document.getElementById('txtname').value.trim().length <= 0) {
                //              alert('Por favor escriba una o varias \nletras del número');
                return false;
            }
            document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif">';
        }
        function Quit() {
            //          event.returnValue = '[ ! ] Se perdera el trabajo realizado [ ! ]';
            //          return;
        }
        function soloLetras(e, caracteres, espacios) {
            key = e.keyCode || e.which;
            tecla = String.fromCharCode(key).toLowerCase();
            if (caracteres) {
                letras = caracteres;
            }
            else {
                letras = " áéíóúabcdefghijklmnñopqrstuvwxyz1234567890-_/ ";
            }
            if (espacios == undefined || espacios == null) {
                especiales = [8, 13, 32, 9, 16, 20];
            }
            else {
                especiales = [8, 13, 9, 16, 20];
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
        function verDocumento(val) {
            var caja = val;
            window.open('../credenciales/solicituddocumentos/?placa=' + caja, 'name', 'width=850,height=480')
        }
   </script>
</body>
</html>
