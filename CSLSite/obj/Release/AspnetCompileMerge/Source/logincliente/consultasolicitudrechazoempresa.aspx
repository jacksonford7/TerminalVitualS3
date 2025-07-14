<%@ Page Language="C#" AutoEventWireup="true" Title="Consulta Solicitud Registro de Empresa" CodeBehind="consultasolicitudrechazoempresa.aspx.cs" Inherits="CSLSite.logincliente.consultasolicitudrechazoempresa" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Consulta Solicitud Registro de Empresa</title>
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

   <%-- <link href="../shared/estilo/catalogosolicitudempresa.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <link href="Inicio/base-site.css" rel="stylesheet" type="text/css" />
    <link href="Inicio/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/x-icon" href="../favicon.ico" />
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
    </style>
    <script type="text/javascript">
        function BindFunctions() {
            document.getElementById('imagen').innerHTML = '';
            $(document).ready(function () {
                $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 5: { sorter: false }, 6: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
            });
        }
    </script>--%>
</head>
<body>
<div class="dashboard-container p-4" id="cuerpo" runat="server">     

     <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">MCA - Módulo de Control de Acceso</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Formulario de Registro de Empresa</li>
          </ol>
        </nav>
    </div>

    <form id="bookingfrm" runat="server">

    <input id="zonaid" type="hidden" value="7" />

    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

     <div class="form-row">
        <div class="form-group col-md-12">
            <a class="btn btn-outline-primary mr-4"  runat="server" id="aprint" clientidmode="Static" >1</a>
            <a class="level1"  runat="server" id="a1" clientidmode="Static" >Tipo de Cliente - Empresa</a>
        </div>

        <div class="form-group col-md-12" >
                <label for="inputAddress">1. Tipo de Cliente:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
              <asp:TextBox ID="txttipcli" runat="server" class="form-control" MaxLength="500" disabled
             style="text-align: center;"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>
        </div>
       
     </div>
     <div class="form-row">
          <div class="form-group col-md-12">
                <a class="btn btn-outline-primary mr-4"  runat="server" id="a4" clientidmode="Static" >2</a>
                <a class="level1"  runat="server" id="a5" clientidmode="Static" >Información del Cliente</a>
            </div>
           <div class="form-group col-md-6"> 
                <label for="inputZip">2. Nombre/Razón Social:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                <asp:TextBox ID="txtrazonsocial" runat="server" class="form-control" MaxLength="500" style="text-align: center;text-transform:uppercase" 
                     onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>
           </div>
           <div class="form-group col-md-6"> 
                <label for="inputZip">3. RUC, C.I o Pasaporte<span style="color: #FF0000; font-weight: bold;">*</span></label>
               <asp:TextBox ID="txtruccipas" runat="server" class="form-control" MaxLength="25" 
                    style="text-align: center"
                    onkeypress="return soloLetras(event,'01234567890abcdefghijklnmñopqrstuvwxyz',true)"></asp:TextBox>
          </div>
         <div class="form-group col-md-6"> 
                <label for="inputZip">4. Actividad Comercial:<span style="color: #FF0000; font-weight: bold;">*</span></label> 
               <asp:TextBox ID="txtactividadcomercial" runat="server" class="form-control" MaxLength="500" Enabled="true"
             style="text-align: center" 
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>
         </div>
          <div class="form-group col-md-6"> 
                <label for="inputZip">5. Dirección Oficina:<span style="color: #FF0000; font-weight: bold;">*</span></label> 
               <asp:TextBox ID="txtdireccion" runat="server" class="form-control" MaxLength="500" 
             style="text-align: center"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890/.-_ ',true)"></asp:TextBox>
         </div>
          <div class="form-group col-md-6"> 
                <label for="inputZip">6. Teléfono Oficina:<span style="color: #FF0000; font-weight: bold;">*</span></label> 
               <asp:TextBox ID="txttelofi" runat="server" class="form-control" MaxLength="9" 
             style="text-align: center" onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
         </div>
          <div class="form-group col-md-6"> 
                <label for="inputZip">8. Persona Contacto:<span style="color: #FF0000; font-weight: bold;">*</span></label> 
                <asp:TextBox ID="txtcontacto" runat="server" class="form-control" MaxLength="500" style="text-align: center;text-transform:uppercase" 
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>
         </div>
         <div class="form-group col-md-6"> 
                <label for="inputZip">9. Celular Contacto:<span style="color: #FF0000; font-weight: bold;">*</span></label> 
              <asp:TextBox ID="txttelcelcon" runat="server" class="form-control" MaxLength="10" 
             style="text-align: center"
             onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
         </div>
         <div class="form-group col-md-6"> 
                <label for="inputZip">10. Mail Contacto:<span style="color: #FF0000; font-weight: bold;">*</span></label> 
               <asp:TextBox runat="server" id="txtmailinfocli" class="form-control" ></asp:TextBox>
         </div>
         <div class="form-group col-md-6"> 
                <label for="inputZip">11. Mail EBilling:<span style="color: #FF0000; font-weight: bold;">*</span></label> 
              <asp:TextBox runat="server" id="txtmailebilling" class="form-control" ></asp:TextBox>
         </div>
         <div class="form-group col-md-6"> 
                <label for="inputZip">12. Certificaciones:<span style="color: #FF0000; font-weight: bold;">*</span></label> 
              <asp:TextBox ID="txtcertificaciones" runat="server" class="form-control" MaxLength="500" Enabled="true"
             style="text-align: center"
                 onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890;./-_ ',true)"></asp:TextBox>
         </div>
         <div class="form-group col-md-6"> 
                <label for="inputZip">13. Sitio Web:<span style="color: #FF0000; font-weight: bold;">*</span></label> 
             <asp:TextBox
              id='turl' runat="server" 
              enableviewstate="false" clientidmode="Static" class="form-control"
              onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890_:.-/;',true)"  
              maxlength="250"> </asp:TextBox>
         </div>
         <div class="form-group col-md-6"> 
                <label for="inputZip">14. Afiliación a Gremios:<span style="color: #FF0000; font-weight: bold;">*</span></label> 
              <asp:TextBox ID="txtafigremios" runat="server" class="form-control" MaxLength="1000" 
             style="text-align: center"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890;./-_ ',true)"></asp:TextBox>
         </div>
          <div class="form-group col-md-6"> 
                <label for="inputZip">15. Referencia Comercial:<span style="color: #FF0000; font-weight: bold;">*</span></label> 
               <asp:TextBox ID="txtrefcom" runat="server" class="form-control"  MaxLength="500" 
             style="text-align: center" 
                 onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890;./-_ ',true)" 
                 ></asp:TextBox>
           </div>
     </div>

      <div class="form-row">
            <div class="form-group col-md-12">
                <a class="btn btn-outline-primary mr-4"   runat="server" id="a2" clientidmode="Static" >3</a>
                <a class="level1"  runat="server" id="a3" clientidmode="Static" >Información del Representante Legal</a>
            </div>
          <div class="form-group col-md-6"> 
                <label for="inputZip">16. Representante Legal:<span style="color: #FF0000; font-weight: bold;">*</span></label>   
                <asp:TextBox ID="txtreplegal" runat="server"  class="form-control" MaxLength="500" style="text-align: center; text-transform:uppercase" 
                 onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)" ></asp:TextBox>
          </div>
          <div class="form-group col-md-6"> 
                <label for="inputZip">17. Teléfono Domicilio:<span style="color: #FF0000; font-weight: bold;">*</span></label>   
               <asp:TextBox ID="txttelreplegal" runat="server"  class="form-control" MaxLength="9"
             style="text-align: center"
             onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
          </div>
          <div class="form-group col-md-6"> 
                <label for="inputZip">18. Dirección Domiciliaria:<span style="color: #FF0000; font-weight: bold;">*</span></label>   
              <asp:TextBox ID="txtdirdomreplegal" runat="server"  class="form-control" MaxLength="500"
             style="text-align: center" 
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ./_-',true)"></asp:TextBox>
          </div>
          <div class="form-group col-md-6"> 
                <label for="inputZip">19. Cédula de Identidad:<span style="color: #FF0000; font-weight: bold;">*</span></label>   
              <asp:TextBox ID="txtci" runat="server"  class="form-control" MaxLength="10"
                 style="text-align: center" 
                 onkeypress="return soloLetras(event,'01234567890abcdefghijklnmñopqrstuvwxyz',true)"></asp:TextBox>
          </div>
          <div class="form-group col-md-6"> 
                <label for="inputZip">20. Mail:<span style="color: #FF0000; font-weight: bold;">*</span></label>   
                <asp:TextBox runat="server" id="tmailRepLegal"  class="form-control" />
          </div>

      </div>
     
      <div class="row">
            <div class="form-group col-md-12">
                <a class="btn btn-outline-primary mr-4"  runat="server" id="a6" clientidmode="Static" >4</a>
                <a class="level1"  runat="server" id="ls6" clientidmode="Static" >Observación en caso de rechazo.</a>
            </div>
          <div class="form-group col-md-12"> 
                <label for="inputZip">&nbsp;<span style="color: #FF0000; font-weight: bold;">*</span></label>   
                  <asp:TextBox ID="txtmotivorechazo" runat="server" class="form-control" MaxLength="500" disabled  ForeColor="Red"
             style="text-align: center"  onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox>
          </div>
      </div>

      
         <div class="row">
          <div class="form-group col-md-12">
            <table id="tablerp" cellpadding="1" cellspacing="0">
            <asp:Repeater ID="tablePaginationDocumentos" runat="server">
            <HeaderTemplate>
            <table id="tablar"  cellspacing="1" cellpadding="1" class="table table-bordered invoice">
            <thead>
            <tr>
            <th>Tipo de Empresa</th>
            <th>Documentos</th>
            <th></th>
            <th>Documento Rechazado</th>
            <th>Comentario</th>
            <th>Corregir Documento (Formato: pdf)</th>
            <th style="display:none"># Solicitud</th>
            <th style="display:none">Id Documento</th>
            </tr>
            </thead> 
            <tbody>
            </HeaderTemplate>
            <ItemTemplate>
            <tr class="point">
            <td ><%#Eval("TIPOEMPRESA")%></td>
            <td ><%#Eval("DOCUMENTO")%></td>
            <td >

                <a href='<%#Eval("RUTADOCUMENTO") %>' style=" width:80px" class="topopup" target="_blank">
                    <i></i> Ver Documento </a>
            </td>
            <td ><asp:CheckBox runat="server" ToolTip="Documento rechazado" Checked='<%#Eval("ESTADOCOL")%>' Enabled="false" id="chkRevisado"/></td>
            <td><asp:TextBox ID="tcomentario" runat="server" ForeColor="Red" Width="200px" Enabled="false" Text='<%#Eval("COMENTARIO")%>' onkeypress="return soloLetras(event,' ',true)"></asp:TextBox></td>
            <td>
            <asp:FileUpload extension='<%#Eval("EXTENSION")%>' class="btn btn-outline-primary mr-4" id="fsupload"  width="200px"
                       onchange="validaextension(this)" style=" font-size:small; font-size:x-small" runat="server"/>
            </td>
            <td style="display:none">
                <asp:Label Text='<%#Eval("NUMSOLICITUD")%>' runat="server" ID="lblNumSolicitud" />
            </td>
            <td style="display:none">
                <asp:Label Text='<%#Eval("IDSOLDOC")%>' runat="server" ID="lblSolDoc" />
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
      
          <div class="row">
               <div class="form-group col-md-12">
                     <div id="sinresultado" runat="server" class="alert alert-warning" >
                                   No se encontraron resultados.
                     </div> 
               </div>
           </div>
    
          
        <div class="col-md-12 d-flex justify-content-center" id="salir" >
                <span id="imagen"></span>
                <asp:Button ID="btsalvar" runat="server" Text="Reenviar" CssClass="btn btn-primary"
                       OnClientClick="return prepareObject('¿Esta seguro de Reenviar la solicitud?');" onclick="btsalvar_Click" 
                       ToolTip="Reenvia la solicitud."/>
       </div>
     
    </form>
 
 </div>   
    <script src="../../Scripts/pages.js" type="text/javascript"></script>

    <script src="../../Scripts/credenciales.js" type="text/javascript"></script>

   <script type="text/javascript" >
       var ced_count = 0;
       var jAisv = {};
       $(window).load(function () {
           $(document).ready(function () {
               //colapsar y expandir
               $("div.colapser").toggle(function () { $(this).removeClass("colapsa").addClass("expande"); $(this).next().hide(); }
                                  , function () { $(this).removeClass("expande").addClass("colapsa"); $(this).next().show(); });
           });
       });
        function prepareObject(valor) {
            try {
                if (confirm(valor) == false) {
                    return false;
                }
                var vals = document.getElementById('<%=txtrazonsocial.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba la Razon Social *').set('label', 'Aceptar');
                    document.getElementById('<%=txtrazonsocial.ClientID %>').focus();
                    <%--document.getElementById('<%=txtrazonsocial.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }

                var vals = document.getElementById('<%=txtactividadcomercial.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba la Actividad Comercial *').set('label', 'Aceptar');
                    document.getElementById('<%=txtactividadcomercial.ClientID %>').focus();
                    <%--document.getElementById('<%=txtactividadcomercial.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                var vals = document.getElementById('<%=txtdireccion.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba la Dirección de Oficina *').set('label', 'Aceptar');
                    document.getElementById('<%=txtdireccion.ClientID %>').focus();
                    <%--document.getElementById('<%=txtdireccion.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                var vals = document.getElementById('<%=txttelofi.ClientID %>').value;
                if (!/^([0-9])*$/.test(vals)) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Teléfono de Oficina No es un Numero *').set('label', 'Aceptar');
                    document.getElementById('<%=txttelofi.ClientID %>').focus();
                   <%-- document.getElementById('<%=txttelofi.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                if (vals.length < 9) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Teléfono de Oficina Incompleto *').set('label', 'Aceptar');
                    document.getElementById('<%=txttelofi.ClientID %>').focus();
                    <%--document.getElementById('<%=txttelofi.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba el Teléfono de Oficina *').set('label', 'Aceptar');
                    document.getElementById('<%=txttelofi.ClientID %>').focus();
                   <%-- document.getElementById('<%=txttelofi.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                var vals = document.getElementById('<%=txtcontacto.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba la Persona de Contacto *').set('label', 'Aceptar');
                    document.getElementById('<%=txtcontacto.ClientID %>').focus();
                    <%--document.getElementById('<%=txtcontacto.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                var vals = document.getElementById('<%=txttelcelcon.ClientID %>').value;
                if (!/^([0-9])*$/.test(vals)) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * No. de Celular de Contacto No es un Numero *').set('label', 'Aceptar');
                    document.getElementById('<%=txttelcelcon.ClientID %>').focus();
                    <%--document.getElementById('<%=txttelcelcon.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                if (vals.length < 10) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * No. de Celular de Contacto Incompleto *').set('label', 'Aceptar');
                    document.getElementById('<%=txttelcelcon.ClientID %>').focus();
                    <%--document.getElementById('<%=txttelcelcon.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba el No. de Celular de Contacto *').set('label', 'Aceptar');
                    document.getElementById('<%=txttelcelcon.ClientID %>').focus();
                    <%--document.getElementById('<%=txttelcelcon.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                var mail1 = document.getElementById('<%=txtmailinfocli.ClientID %>').value;
                if (mail1 == null || mail1 == undefined || mail1 == '') {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba al menos un mail *').set('label', 'Aceptar');
                    document.getElementById('<%=txtmailinfocli.ClientID %>').focus();
                   <%-- document.getElementById('<%=txtmailinfocli.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                if (mail1.trim().length > 0) {
                    if (!validarEmail(mail1)) {
                        //alertify.alert('* Datos de Registro de Empresa: *\n * Mail no parece correcto *')
                        document.getElementById('<%=txtmailinfocli.ClientID %>').focus();
                        <%--document.getElementById('<%=txtmailinfocli.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                        return false;
                    }
                }
                var mail1 = document.getElementById('<%=txtmailebilling.ClientID %>').value;
                if (mail1 == null || mail1 == undefined || mail1 == '') {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba al menos un mail *').set('label', 'Aceptar');
                    document.getElementById('<%=txtmailebilling.ClientID %>').focus();
                    <%--document.getElementById('<%=txtmailebilling.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                if (mail1.trim().length > 0) {
                    if (!validarEmail(mail1)) {
                        alertify.alert('* Datos de Registro de Empresa: *\n * Mail no parece correcto *').set('label', 'Aceptar');
                        document.getElementById('<%=txtmailebilling.ClientID %>').focus();
                       <%-- document.getElementById('<%=txtmailebilling.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                        return false;
                    }
                }
                var vals = document.getElementById('<%=txtreplegal.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba el Representante Legal *').set('label', 'Aceptar');
                    document.getElementById('<%=txtreplegal.ClientID %>').focus();
                   <%-- document.getElementById('<%=txtreplegal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                var vals = document.getElementById('<%=txttelreplegal.ClientID %>').value;
                if (!/^([0-9])*$/.test(vals)) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Teléfono de Domicilio No es un Numero *').set('label', 'Aceptar');
                    document.getElementById('<%=txttelreplegal.ClientID %>').focus();
                  <%--  document.getElementById('<%=txttelreplegal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                if (vals.length < 9) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Teléfono de Domicilio Incompleto *').set('label', 'Aceptar');
                    document.getElementById('<%=txttelreplegal.ClientID %>').focus();
                   <%-- document.getElementById('<%=txttelreplegal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba el Teléfono de Domicilio *').set('label', 'Aceptar');
                    document.getElementById('<%=txttelreplegal.ClientID %>').focus();
                   <%-- document.getElementById('<%=txttelreplegal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                var vals = document.getElementById('<%=txtdirdomreplegal.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba la Dirección Domiciliaria *').set('label', 'Aceptar');
                    document.getElementById('<%=txtdirdomreplegal.ClientID %>').focus();
                    <%--document.getElementById('<%=txtdirdomreplegal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
//                var vals = document.getElementById('<%=txtci.ClientID %>').value;
//                if (valruccipas == '' || valruccipas == null || valruccipas == undefined) {
//                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba el No. de CI *');
//                    document.getElementById('<%=txtci.ClientID %>').focus();
//                    document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";
//                    return false;
//                }
                var mail1 = document.getElementById('<%=tmailRepLegal.ClientID %>').value;
                if (mail1 == null || mail1 == undefined || mail1 == '') {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba al menos un mail *').set('label', 'Aceptar');
                    document.getElementById('<%=tmailRepLegal.ClientID %>').focus();
                  <%--  document.getElementById('<%=tmailRepLegal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                if (mail1.trim().length > 0) {
                    if (!validarEmail(mail1)) {
                        alertify.alert('* Datos de Registro de Empresa: *\n * Mail de Representante Legal no parece correcto *').set('label', 'Aceptar');
                        document.getElementById('<%=tmailRepLegal.ClientID %>').focus();
                       <%-- document.getElementById('<%=tmailRepLegal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                        return false;
                    }
                }
//                if (!validaciservidor()) {
//                    return false;
//                };
                //Valida Documentos
                lista = [];
                    var tbl = document.getElementById('tablar');
                    for (var f = 0; f < tbl.rows.length; f++) {
                        var celColect = tbl.rows[f].getElementsByTagName('td');
                        if (celColect != undefined && celColect != null && celColect.length > 0) {
                            var tdetalle = {
                                documento: celColect[5].getElementsByTagName('input')[0].value,
                                rechazado: celColect[3].getElementsByTagName('input')[0].checked
                            };
                            this.lista.push(tdetalle);
                        }
                    }
                    var nomdoc = null;
                    for (var n = 0; n < this.lista.length; n++) {
                        if (lista[n].rechazado == true) {
                            if (lista[n].documento == '' || lista[n].documento == null || lista[n].documento == undefined) {
                                alertify.alert('* Datos de Registro de Empresa: *\n * Seleccione los documentos requeridos *').set('label', 'Aceptar');
                                return false;
                            }
                            if (nomdoc == lista[n].documento) {
                                alertify.alert('* Datos de Registro de Empresa: *\n * Existen archivos repetidos, revise por favor *').set('label', 'Aceptar');
                                return false;
                            }
                            nomdoc = lista[n].documento;
                        }
                    }
                document.getElementById('imagen').innerHTML = '<img alt="loading.."" src="../../shared/imgs/loader.gif"/>';
                return true;
            } catch (e) {
                alertify.alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message).set('label', 'Aceptar');
            }
        }
        function validaciservidor() {
            var valruccipas = document.getElementById('<%=txtci.ClientID %>').value;
            if (!/^([0-9])*$/.test(valruccipas)) {
                alertify.alert('* Datos de Registro de Empresa: *\n * No. C.I. No es un Numero *').set('label', 'Aceptar');
                document.getElementById('<%=txtci.ClientID %>').focus();
                <%--document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                return false;
            }
            else if (valruccipas == '' || valruccipas == null || valruccipas == undefined) {
                alertify.alert('* Datos de Registro de Empresa: *\n * Escriba el No. de C.I. *').set('label', 'Aceptar');
                document.getElementById('<%=txtci.ClientID %>').focus();
               <%-- document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                return false;
            }
            if (valruccipas.length = 0) {
                alertify.alert('* Datos de Registro de Empresa: *\n * Escriba el No. de C.I. *').set('label', 'Aceptar');
                document.getElementById('<%=txtci.ClientID %>').focus();
                <%--document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                return false;
            }
            if (valruccipas.length < 10) {
                alertify.alert('* Datos de Registro de Empresa: *\n * No. C.I. INCOMPLETO! *').set('label', 'Aceptar');
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
                    alertify.alert('* Datos de Registro de Empresa: *\n * No. C.I. no válido! *').set('label', 'Aceptar');
                    document.getElementById('<%=txtci.ClientID %>').focus();
                    <%--document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
            }
            else {
                if (final != 10) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * No. C.I. no válido! *').set('label', 'Aceptar');
                    document.getElementById('<%=txtci.ClientID %>').focus();
                   <%-- document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
            }
            return true;
        }
        function valruccipasservidor() {
           
            var valruccipas = document.getElementById('<%=txtruccipas.ClientID %>').value;
           
            if (valruccipas == null || valruccipas == undefined || valruccipas == '') {
                alertify.alert('* Datos de Registro de Empresa: *\n * Escriba el No. de RUC,CI o Pasaporte *').set('label', 'Aceptar');
                document.getElementById('<%=txtruccipas.ClientID %>').focus();
                <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                return false;
            }
            if (valruccipas.length < 10 ) {
                alertify.alert('* Datos de Registro de Empresa: *\n * No. de CI IMCOMPLETO *').set('label', 'Aceptar');
                document.getElementById('<%=txtruccipas.ClientID %>').focus();
                <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                return false;
            }
            if (valruccipas.length > 10 && valruccipas.length < 13) {
                alertify.alert('* Datos de Registro de Empresa: *\n * No. de RUC IMCOMPLETO *').set('label', 'Aceptar');
                document.getElementById('<%=txtruccipas.ClientID %>').focus();
               <%-- document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                return false;
            }
            if (valruccipas.length == 13) {
                if (!/^([0-9])*$/.test(valruccipas)) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * No. RUC No es un Numero *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                else if (valruccipas == '' || valruccipas == null || valruccipas == undefined) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba el No. de RUC *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                   <%-- document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                var numeroProvincias = 24;
                var numprov = valruccipas.substr(0, 2);
                if (numprov > numeroProvincias) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * El código de la provincia (dos primeros dígitos) es inválido! *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                   <%-- document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                validadocrucservidor(valruccipas);
            }
            if (valruccipas.length == 10) {
                if (!/^([0-9])*$/.test(valruccipas)) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * No. C.I. No es un Numero *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                  <%--  document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                else if (valruccipas == '' || valruccipas == null || valruccipas == undefined) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba el No. de C.I. *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                  <%--  document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                if (valruccipas.length = 0) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba el No. de C.I. *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                  <%--  document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                if (valruccipas.length < 10) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * No. C.I. INCOMPLETO! *').set('label', 'Aceptar');
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
                        alertify.alert('* Datos de Registro de Empresa: *\n * No. C.I. no válido! *').set('label', 'Aceptar');
                        document.getElementById('<%=txtruccipas.ClientID %>').focus();
                      <%--  document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                        return false;
                    }
                }
                else {
                    if (final != 10) {
                        alertify.alert('* Datos de Registro de Empresa: *\n * No. C.I. no válido! *').set('label', 'Aceptar');
                        document.getElementById('<%=txtruccipas.ClientID %>').focus();
                       <%-- document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                        return false;
                    }
                }
            }
            if (valruccipas.length > 13) {
                if (valruccipas == '' || valruccipas == null || valruccipas == undefined) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba el No. Pasaporte *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                   <%-- document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
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
                alertify.alert('* Datos de Registro de Empresa: *\n * No. RUC. INCOMPLETO! *').set('label', 'Aceptar');
                document.getElementById('<%=txtruccipas.ClientID %>').focus();
               <%-- document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                return false;
            }
            if (campo.length > 13) {
                alertify.alert('* Datos de Registro de Empresa: *\n * El valor no corresponde a un No. de RUC *').set('label', 'Aceptar');
                document.getElementById('<%=txtruccipas.ClientID %>').focus();
               <%-- document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
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

                alertify.alert('* Datos de Registro de Empresa: *\n * El tercer dígito ingresado es inválido! *').set('label', 'Aceptar');
                document.getElementById('<%=txtruccipas.ClientID %>').focus();
               <%-- document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
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
                    alertify.alert('* Datos de Registro de Empresa: *\n * El RUC de la empresa del sector público es incorrecto! *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                   <%-- document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                /* El ruc de las empresas del sector publico terminan con 0001*/
                if (numero.substr(9, 4) != '0001') {
                    alertify.alert('* Datos de Registro de Empresa: *\n * El RUC de la empresa del sector público debe terminar con 0001! *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                   <%-- document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
            }
            else if (pri == true) {
                if (digitoVerificador != d10) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * El RUC de la empresa del sector privado es incorrecto! *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                   <%-- document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                if (numero.substr(10, 3) != '001') {
                    alertify.alert('* Datos de Registro de Empresa: *\n * El RUC de la empresa del sector privado debe terminar con 001! *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                 <%--   document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
            }
            else if (nat == true) {
                if (digitoVerificador != d10) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * El número de cédula de la persona natural es incorrecto! *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                   <%-- document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
                    return false;
                }
                if (numero.length > 10 && numero.substr(10, 3) != '001') {
                    alertify.alert('* Datos de Registro de Empresa: *\n * El RUC de la persona natural debe terminar con 001! *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                 <%--   document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:99%;";--%>
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
//              alertify.alert('Por favor escriba una o varias \nletras del número');.
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
