<%@ Page Language="C#" AutoEventWireup="true" Title="Solicitud Registro de Empresa"
CodeBehind="revisasolicitudempresa.aspx.cs" Inherits="CSLSite.revisasolicitudempresa" %>
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
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
   <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

    <!--external css-->
  <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
  <link rel="stylesheet" type="text/css" href="../lib/gritter/css/jquery.gritter.css" />

    <!--mensajes-->
  <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>

  <%--  <title>Catálogo de agentes</title>
    <link href="../shared/estilo/catalogosolicitudempresa.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
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
            </div>
            <div class="form-group col-md-12">  
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
            
            <div class="form-group col-md-6 d-flex">
                <asp:TextBox ID="txtrazonsocial" runat="server" MaxLength="500" Enabled="false" style="text-align: center;" 
                onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)" CssClass="form-control"></asp:TextBox>

                <asp:ImageButton ImageUrl="~/shared/imgs/encrypted.png" runat="server" id="ibUpdateEmpresa" OnClientClick="return getUrlUpdateEmpresa()"
                 ToolTip="Desbloquea la casilla Nombre/Razón Social registrada en la solicitud." Height="16px" />

                <asp:ImageButton ImageUrl="~/shared/imgs/ok.png" runat="server"  style=" display:none"
                 id="ibUpdateEmpresaOk"  OnClientClick="return getUrlUpdateEmpresaOk()"
                 ToolTip="Actualiza el Nombre/Razón Social registrada en la solicitud." 
                 Height="16px" onclick="ibUpdateEmpresaOk_Click" />
            </div> 
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
                <asp:TextBox ID="txtcontacto" runat="server" MaxLength="500" Enabled="false" style="text-align: center" 
                onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)" CssClass="form-control"></asp:TextBox>
            </div> 

            <div class="form-group col-md-6">
                <label class="form-control" >8. Celular Contacto:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                <asp:TextBox ID="txttelcelcon" runat="server" MaxLength="10" Enabled="false" style="text-align: center"
                onkeypress="return soloLetras(event,'01234567890',true)" CssClass="form-control"></asp:TextBox>
            </div> 

            <div class="form-group col-md-6">
                <label class="form-control" >9. Mail Contacto:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                <asp:TextBox runat="server" id="txtmailinfocli" Enabled="false" CssClass="form-control"></asp:TextBox>
            </div> 

            <div class="form-group col-md-6">
                <label class="form-control" >10. Mail EBilling:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                <asp:TextBox runat="server" id="txtmailebilling" Enabled="false" CssClass="form-control"></asp:TextBox>
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
                <asp:TextBox ID="txtreplegal" runat="server" MaxLength="500" style="text-align: center" Enabled="false"
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

        <div class="form-row">
            <div class="form-title col-md-12">
                <a class="btn btn-outline-primary mr-4" href="#" target="_blank" runat="server" id="a6" clientidmode="Static" >4</a>
                <a class="level1" runat="server" id="a7" clientidmode="Static" >Observación en caso de rechazo</a>
            </div>
            <%--Enabled="false"--%>
            <div class="form-group col-md-12">
                <asp:TextBox ID="txtmotivorechazo" ForeColor="Red" runat="server" MaxLength="500" 
             style="text-align: center"  onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
<%--        <div class="form-group col-md-12">--%>
       <div class="cataresult" >
           <div id="xfinder" runat="server">
           <div class="findresult" >
                 <div class="alert alert-warning" runat="server" id="alerta" >
                                Confirme que los documentos sean los correctos.
                 </div>
            <div class="booking">
                <div class="bokindetalle" style=" overflow:auto; font-size:inherit">
                 <%--<div class="bokindetalle">--%>
                    <table id="tablerp" cellpadding="1" cellspacing="0">
                        <asp:Repeater ID="tablePaginationDocumentos" runat="server">
                        <HeaderTemplate>
                        <table id="tablar"  cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                        <thead>
                        <tr>
                        <th>Tipo Cliente</th>
                        <th>Documentos</th>
                        <th></th>
                        <th>Documento Rechazado</th>
                        <th>Comentario</th>
                        </tr>
                        </thead> 
                        <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                        <tr class="point">
                        <td ><%#Eval("TIPOEMPRESA")%></td>
                        <td style="  font-size:inherit"><%#Eval("DOCUMENTO")%></td>
                        <td >
                            <a href='<%#Eval("RUTADOCUMENTO") %>'  class="btn btn-outline-primary mr-4" target="_blank">
                                <i class="fa fa-search"></i> Ver Documento </a>
                        </td>
                        <td ><asp:CheckBox runat="server" Checked='<%#Eval("ESTADODOC")%>' id="chkRevisado"/></td>
                        <td><asp:TextBox ID="tcomentario" runat="server" ForeColor="Red" Text='<%#Eval("COMENTARIO")%>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox></td>
                        </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                        </tbody>
                        </table>
                        </FooterTemplate>
                        </asp:Repeater>
                    </table>
                <%--</div>--%>
                </div>
            </div>

                <div id="sinresultado" runat="server" class="alert alert-danger" >
                  No se encontraron resultados, 
                  asegurese que ha seleccionado correctamente un tipo de solicitud.
                </div>
            </div>
            </div>
               <div class="col-md-12 d-flex justify-content-rigth" id="salir" runat="server">
                    <asp:Button ID="btnSalir" runat="server" Text="Regresar" onclick="btnSalir_Click" 
                    CssClass="btn btn-outline-primary mr-4" ToolTip="Regresa a la Pantalla Consultar Solicitud."/>
                   <span>&nbsp;</span>
                   <asp:Button Text="Anular" ID="btnAnular" runat="server" OnClick="btnAnular_Click" CssClass="btn btn-primary mr-4"
                        OnClientClick="return prepareObject('¿Esta seguro de Anular la solicitud?');" ToolTip="Anular la solicitud."/>
                </div>
               <br />
               <div class="col-md-12 d-flex justify-content-rigth" id="botonera" runat="server">
                    <asp:Button Text="Rechazar" ID="btnRechazar" runat="server" onclick="btnRechazar_Click" CssClass="btn btn-primary mr-4"
                        OnClientClick="return prepareObject('¿Esta seguro de Rechazar la solicitud?');" ToolTip="Rechaza la solicitud."/>
                   <span>&nbsp;</span>
                   <asp:Button ID="btsalvar" runat="server" Text="Finalizar" onclick="btsalvar_Click" CssClass="btn btn-primary mr-4"
                   OnClientClick="return prepareObject('¿Esta seguro de Finalizar la solicitud?');" ToolTip="Finaliza la solicitud."/>
               
                </div>

      </div>
     <%-- </div>--%>
      <asp:HiddenField runat="server" ID="hfRazSocial" />
    </form>
    </div>

    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
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
//                document.getElementById("loader").className = '';
                if (confirm(valor) == true)
                    return true;
                else
                    return false;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
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
      function getUrlUpdateEmpresa() {
          if (document.getElementById('txtrazonsocial').disabled) {
              document.getElementById('ibUpdateEmpresa').src = "../../shared/imgs/decrypted.png"
              document.getElementById('txtrazonsocial').disabled = false;
              document.getElementById('ibUpdateEmpresaOk').style.display = "block";
              document.getElementById('txtrazonsocial').focus();
          }
          else {
              document.getElementById('ibUpdateEmpresa').src = "../../shared/imgs/encrypted.png"
              document.getElementById('txtrazonsocial').disabled = true;
              document.getElementById('txtrazonsocial').value = document.getElementById('hfRazSocial').value;
              document.getElementById('ibUpdateEmpresaOk').style.display = "none"
          }
          return false;
      }
      function getUrlUpdateEmpresaOk() {
          document.getElementById('txtrazonsocial').value = document.getElementById('txtrazonsocial').value.toUpperCase();
          if (!document.getElementById('txtrazonsocial').disabled) {
              if (confirm("Esta seguro de Actualizar el Nombre/Razón Social registrada en la solicitud.") == true) {
                  return true;
              }
              else {
                  document.getElementById('ibUpdateEmpresa').src = "../../shared/imgs/encrypted.png"
                  document.getElementById('ibUpdateEmpresaOk').style.display = "none"
                  document.getElementById('txtrazonsocial').disabled = true;
                  document.getElementById('txtrazonsocial').value = document.getElementById('hfRazSocial').value;
                  return false;
              }
          }
          else {
              alert("No se puede Actualizar, no se ha realizado nigun cambio en el \nNombre/Razón Social registrada en la solicitud");
              return false;
          }
      }
   </script>
</body>
</html>
