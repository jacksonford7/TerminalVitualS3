<%@ Page Language="C#" AutoEventWireup="true" Title="Renovación de Permisos OPC" CodeBehind="solicitudCredencialesActivasOPCConsulta.aspx.cs" Inherits="CSLSite.solicitudCredencialesActivasOPCConsulta" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Solicitud de Credencial/Renovacion Permisos OPC</title>
     <link href="../img/favicon2.png" rel="icon"/>
      <link href="../img/icono.png" rel="apple-touch-icon"/>
      <link href="../css/bootstrap.min.css" rel="stylesheet"/>
      <link href="../css/dashboard.css" rel="stylesheet"/>
      <link href="../css/icons.css" rel="stylesheet"/>
      <link href="../css/style.css" rel="stylesheet"/>
      <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

          <!--external css-->
      <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
      <link rel="stylesheet" type="text/css" href="../lib/gritter/css/jquery.gritter.css" />

        <!--mensajes-->
      <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
      <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>
      <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>

      <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
      <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

  
</head>
<body>

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Solicitud de Credencial</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Renovacion Permisos OPC</li>
          </ol>
        </nav>
    </div>

<div class="dashboard-container p-4" id="cuerpo" runat="server"> 
    <form id="bookingfrm" runat="server">
    <input id="zonaid" type="hidden" value="7" />
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

<%--    <div class="form-title">
        1)  Datos del Permiso Peatonal Provisional.
     </div>--%>
    <div class="form-title col-md-12">
        <a class="btn btn-outline-primary mr-4" href="#" target="_blank" runat="server" id="aprint" clientidmode="Static" >1</a>
        <a class="level1" target="_blank" runat="server" id="a1" clientidmode="Static" >Datos de Compañia Solicitante</a>
    </div>
    <div class="form-row">
         <div class="form-group col-md-6">
              <label for="inputEmail4">Empresa que solicita el permiso:<span style="color: #FF0000; font-weight: bold;"></span></label>
                  <asp:TextBox ID="txttipcli" runat="server" class="form-control" MaxLength="500" disabled
             style="text-align: center;"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>             
         </div> 
        <div class="form-group col-md-6" style=" display:none">
              <label for="inputEmail4">Area Destino:<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:TextBox ID="txtarea" runat="server" class="form-control" MaxLength="500" disabled
             style="text-align: center;"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>               
         </div> 
        <div class="form-group col-md-6">
              <label for="inputEmail4">Usuario que solicita el permiso:<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:TextBox ID="txtusuariosolper" runat="server" class="form-control" MaxLength="500" disabled
             style="text-align: center;"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>               
         </div> 
        <div class="form-group col-md-6" style=" display:none">
              <label for="inputEmail4">Actividad permitida:<span style="color: #FF0000; font-weight: bold;"></span></label>
                   <asp:TextBox ID="txtactper" runat="server" class="form-control" MaxLength="500" disabled
             style="text-align: center;"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>            
         </div> 
        <div class="form-group col-md-6">
              <label for="inputEmail4">Fecha de Ingreso:<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:TextBox 
             style="text-align: center" 
             ID="txtfecing" runat="server"   MaxLength="15" CssClass="datetimepicker form-control" ClientIDMode="Static"
             onkeypress="return soloLetras(event,'0123456789/')"  disabled
             ></asp:TextBox>               
         </div> 
        <div class="form-group col-md-6">
              <label for="inputEmail4">Fecha de Caducidad:<span style="color: #FF0000; font-weight: bold;"></span></label>
                   <asp:TextBox 
             style="text-align: center" 
             ID="txtfecsal" runat="server"   MaxLength="15" CssClass="datetimepicker form-control" ClientIDMode="Static"
             onkeypress="return soloLetras(event,'0123456789/')"  disabled
             ></asp:TextBox>          
         </div> 
         <div class="form-group col-md-6" style=" display:none">
              <label for="inputEmail4">Turno::<span style="color: #FF0000; font-weight: bold;">*</span></label>
                    <asp:DropDownList runat="server" class="form-control" ID="ddlTurnoOnlyControl" onchange="valdltipsolhorario(this, valturno);">
                 <asp:ListItem Value="0">* Elija *</asp:ListItem>
           </asp:DropDownList>
           <span id='valturno' class="validacion" ></span>            
         </div> 
    </div>
    <%--<div class="form-title">
        2)  Personal resgistrado en permiso provisional.
     </div>--%>
    <div class="form-title col-md-12">
        <a class="btn btn-outline-primary mr-4" href="#" target="_blank" runat="server" id="a2" clientidmode="Static" >2</a>
        <a class="level1" target="_blank" runat="server" id="a3" clientidmode="Static" >Personal Resgistrado en Permiso Solicitado</a>
    </div>
  <div class="bokindetalle" style="width:100%;overflow:auto">
      <div class="alert alert-warning" id="alerta" visible="false" runat="server"></div>

       <asp:Repeater ID="tablePagination" runat="server"  >
                 <HeaderTemplate>
                 <table id="tabla"  cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                 <thead>
                 <tr>
                 <th style=" display:none"></th>
                 <th style=" display:none"></th>
                 <th>CI/Pasaporte</th>
                 <th>Nombre</th>
                 <th>Tipo Sangre</th>
                 <th class ="nover">Dirección Domiciliaria</th>
                 <th class ="nover">Telefono</th>
                 <th class ="nover">Email</th>
                 <th class ="nover">Lugar Nacimiento</th>
                 <th class ="nover">Fecha Nacimiento</th>
                 <th>Cargo</th>
                 <th style=" display:none;">Area</th>
                 <th>Colaborador Rechazado</th>
                 <th>Comentario</th>
                 <%--<th></th>--%>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td style=" display:none; width:1px"><%#Eval("NUMSOLICITUD")%></td>
                  <td style=" display:none; width:1px"><%#Eval("IDSOLCOL")%></td>
                  <td><asp:Label Text='<%#Eval("CIPAS")%>' runat="server" id="lblcipas"/></td>
                  <td><asp:Label Text='<%#Eval("NOMBRE")%>' runat="server" id="lblNombres"/></td>
                  <td><%#Eval("TIPOSANGRE")%></td>
                  <td class ="nover"><%#Eval("DIRECCIONDOM")%></td>
                  <td class ="nover"><%#Eval("TELFDOM")%></td>
                  <td class ="nover"><%#Eval("EMAIL")%></td>
                  <td class ="nover" style=" width:80px"><%#Eval("LUGARNAC")%></td>
                  <td class ="nover" style=" width:60px"><%#Eval("FECHANAC")%></td>
                  <td><%#Eval("CARGO")%></td>
                  <td style=" display:none;"><%#Eval("AREA")%></td>
                  <td> <label class="checkbox-container">
                      <asp:CheckBox runat="server" id="chkRevisado"/>
                     <span class="checkmark"></span>
                      </label>    
                  </td>
                  <td ><asp:TextBox ID="tcomentario" runat="server" class="form-control" Text='' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox></td>
                  <td class="nover">
                    <a id="adjDoc" class="btn btn-primary"  onclick="redirectsol('<%# Eval("NUMSOLICITUD") %>', '<%#Eval("CIPAS")%>', '<%#Eval("IDSOLCOL")%>');">
                    <i></i> Ver Documentos </a>
                  </td>
                 </tr>
                 </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>

 </div>
     
     
     <div class="form-row" id="factura" runat="server" style=" display:none" >
     
       <div class="alert alert-warning" id="alertafu" runat="server"></div>
       <div class="form-group col-md-6" runat="server" id="tablefac">
           <label for="inputEmail4">Adjuntar factura:<span style="color: #FF0000; font-weight: bold;"></span></label>
             <asp:FileUpload runat="server" ID="fuAdjuntarFactura" Width="100%" extension='.pdf' class="form-control" title="Adjunte el archivo en formato PDF." onchange="validaextension(this)"/>
        </div> 
      
   </div>

     <div class="row" runat="server" id="salir" visible="true">
          <div class="col-md-12 d-flex justify-content-center">
                <asp:Button ID="btnSalir" runat="server" Text="Regresar" class="btn btn-primary"  
                        onclick="btnSalir_Click" 
                        ToolTip="Regresa a la Pantalla Consultar Solicitud."/> 
          </div>
    </div>
   
      
<%--        <div class="row" runat="server" id="botonera">
              <div class="col-md-12 d-flex justify-content-center">

                    <asp:Button Text="Rechazar" ID="btnRechazar" runat="server" class="btn btn-primary"  
                    onclick="btnRechazar_Click" OnClientClick="return prepareObject('¿Esta seguro de rechazar la solicitud?');"
                    ToolTip="Rechaza la solicitud."/>&nbsp;&nbsp;

                    <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />&nbsp;&nbsp;

                    <asp:Button ID="btsalvar" runat="server" Text="Crear Permiso" class="btn btn-primary"  
                    OnClientClick="return prepareObject('¿Esta seguro de procesar la solicitud?');" onclick="btsalvar_Click" 
                    ToolTip="Crea el permiso provisional."/> 

              </div>
    </div>--%>

   
       
  </form>

</div>

    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>
     <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

   <%-- <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>--%>
    <script type="text/javascript" >
        var ced_count = 0;
        var jAisv = {};
        //$(document).ready(function () {
        //    $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        //});
        //$(window).load(function () {
        //    $(document).ready(function () {
        //        //colapsar y expandir
        //        $("div.colapser").toggle(function () { $(this).removeClass("colapsa").addClass("expande"); $(this).next().hide(); }
        //                          , function () { $(this).removeClass("expande").addClass("colapsa"); $(this).next().show(); });
        //    });
        //});
       function setObject(row) {
            self.close();
        }
        function prepareObject() {
            try {
                if (confirm("Esta seguro de crear el permiso provisional.") == false) {
                    return false;
                }
                var vals = document.getElementById('<%=ddlTurnoOnlyControl.ClientID %>').value;
                if (vals == 0) {
                    alertify.alert('*Seleccione el Turno *').set('label', 'Aceptar');
            
                    document.getElementById('<%=ddlTurnoOnlyControl.ClientID %>').focus();
                    document.getElementById('<%=ddlTurnoOnlyControl.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";
//                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                return true;
            } catch (e) {
                alertify.alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message).set('label', 'Aceptar');
            }
        }
      function initFinder() {
          if (document.getElementById('txtname').value.trim().length <= 0) {
              alertify.alert('Por favor escriba una o varias \nletras del número').set('label', 'Aceptar');
              return false;
          }
          document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif">';
      }
      function Quit() {
//          event.returnValue = '[ ! ] Se perdera el trabajo realizado [ ! ]';
//          return;
      }
      function redirectsol(val, val2, val3) {
          var caja = val;
          var caja2 = val2;
          var caja3 = val3;
          window.open('../cliente/consultasolicitudpermisoprovisionaldocumentos.aspx/?numsolicitud=' + caja + '&idsolcol=' + caja3 + '&cedula=' + caja2)
      }
   </script>
  
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../../Scripts/credenciales.js" type="text/javascript"></script>
  
       <script type="text/javascript">
            $(document).ready(function () {
                 $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/y' });
              });    
      </script>

</body>
</html>
