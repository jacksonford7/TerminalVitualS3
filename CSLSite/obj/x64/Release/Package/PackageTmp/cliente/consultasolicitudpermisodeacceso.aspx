<%@ Page Language="C#" AutoEventWireup="true" Title="Solicitud de Permiso de Acceso"
CodeBehind="consultasolicitudpermisodeacceso.aspx.cs" Inherits="CSLSite.consultasolicitudpermisodeacceso" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>Solicitud de Credencial/Permiso Provisional</title>
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

   <%-- <script src="../../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    <link href="../shared/estilo/catalogosolicitudcolaborador.css" rel="stylesheet" type="text/css" />
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
    </style>--%>
</head>
<body>
  
<div class="dashboard-container p-4" id="cuerpo" runat="server">

      <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Solicitud de Credencial</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Permiso Provisional</li>
          </ol>
        </nav>
      </div>

    <form id="bookingfrm"  runat="server">
    <input id="zonaid" type="hidden" value="7" />
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

    <div class="form-title">
        1) Tipo de Solicitud.
     </div>

     <div class="form-row" >
          <div class="form-group col-md-6">  
              <label for="inputEmail4">Tipo de Solicitud:<span style="color: #FF0000; font-weight: bold;">*</span></label>
               <asp:TextBox ID="txttipcli" runat="server" class="form-control" MaxLength="500" disabled
             style="text-align: center;"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>
          </div>
     </div>

     <div class="form-title">
        2) Datos Generales de los Permisos de Acceso.
     </div>

   
     

     <div class="alert alert-warning" id="alerta" runat="server" ></div>

     <div class="form-row">
      <div class="form-group col-md-12">             
         <asp:Repeater ID="tablePagination" runat="server"  >
                <HeaderTemplate>
                <table id="tableRpt"  cellspacing="1" cellpadding="1" class="table table-bordered invoice">
<%--                 </thead> --%>
                <tr style="position:static">
                 <th style=" display:none"></th>
                 <th style=" display:none"></th>
                 <th>Cedula</th>
                 <th>Nombres</th>
                 <th>Apellidos</th>
                 <th>Área Destino/Actividad</th>
                 <th>Cargo</th>
                 <th style=" display:none">Permiso</th>
                 <th>Fecha Ingreso</th>
                 <th>Fecha Caducidad</th>
                 <th style=" display:none">Cambiar Permiso</th>
                 <th style=" display:none">Permiso OC</th>
                 <th style=" display:none">Fecha Ingreso OC</th>
                 <th style=" display:none">Fecha Caduc. OC</th>
                 <th style=" display:none">Turno. OC</th>
                 <th style=" display:none">Area. OC</th>
                 <th style=" display:none">Dpto. OC</th>
                 <th style=" display:none">Cargo. OC</th>
                 <th>Rechazado</th>
                 <th>Comentario</th>
                 <th>Ver</th>
                 </tr>
                 <%--<tbody>--%>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr >
                  <td style=" display:none; width:1px"><%#Eval("NUMSOLICITUD")%></td>
                  <td style=" display:none; width:1px"><%#Eval("IDSOLPER")%></td>
                  <td ><asp:Label Text='<%#Eval("CEDULA")%>' runat="server" id="lblcipas"/></td>
                  <td><asp:Label Text='<%#Eval("NOMBRES")%>' runat="server" id="lblnombres"/></td>
                  <td><asp:Label Text='<%#Eval("APELLIDOS")%>' runat="server" id="lblapellidos"/></td>
                  <td><%#Eval("AREADESTINO")%></td>
                  <td><%#Eval("CARGO")%></td>
                  <td style=" width:60px;display:none"><%#Eval("TIPO")%></td>
                  <td ><asp:Label Text='<%#Eval("FECHAINGRESO")%>'  runat="server" id="lblfecing"/></td>
                  <td ><asp:Label Text='<%#Eval("FECHACADUCIDAD")%>'  runat="server" id="lblfeccad"/></td>
                  <td style=" width:50px; display:none">
                      <label class="checkbox-container">
                      <asp:CheckBox runat="server"   id="chkPermiso" onchange="valPermiso()"/>
                           <span class="checkmark"></span>
                      </label>    
                          </td>
                  <td style=" width:80px; display:none"><asp:DropDownList runat="server" class="form-control" ID="ddlPermiso"></asp:DropDownList></td>
                  <td style=" width:80px; display:none">
                                    <asp:TextBox Style="text-align: center" ID="txtfecing" runat="server" 
                                    MaxLength="15" CssClass="datetimepicker form-control" ClientIDMode="Static" onkeypress="return soloLetras(event,'0123456789/')"
                                    ></asp:TextBox>
                  </td>
                  <td style=" width:80px; display:none">
                                <asp:TextBox Style="text-align: center" ID="txtfecsal" runat="server" 
                                    MaxLength="15" CssClass="datetimepicker form-control" ClientIDMode="Static" onkeypress="return soloLetras(event,'0123456789/')"
                                ></asp:TextBox>
                  </td>
                  <td style=" width:120px; display:none"><asp:DropDownList runat="server" class="form-control"  ID="ddlTurnoOnlyControl"></asp:DropDownList></td>
                  <td style=" width:120px; display:none"><asp:DropDownList runat="server" class="form-control"  ID="ddlAreaOnlyControl"></asp:DropDownList></td>
                  <td style=" width:120px; display:none"><asp:DropDownList runat="server" class="form-control"  ID="ddlDepartamentoOnlyControl"></asp:DropDownList></td>
                  <td style=" width:80px; display:none"><asp:DropDownList runat="server" class="form-control"  ID="ddlCargoOnlyControl"></asp:DropDownList></td>      
                  <td >
                      <label class="checkbox-container">
                      <asp:CheckBox runat="server" id="chkRevisado" Checked='<%#Eval("ESTADOSPA")%>' disabled ForeColor="Red" onchange="valRechazado()"/>
                          <span class="checkmark"></span>
                      </label>                          
                 </td>
                  <td ><asp:TextBox ID="tcomentario"  runat="server" Text='<%#Eval("COMENTARIO")%>' class="form-control" disabled ToolTip='<%#Eval("COMENTARIO")%>' 
                      
                      ForeColor="Red" onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox></td>
                  <td >
                    <a href='<%#Eval("RUTADOCUMENTO") %>' class="btn btn-primary" target="_blank">
                    <i></i>Documento(s)</a>
                  </td>
                 </tr>
                 </ItemTemplate>
                 <FooterTemplate>
                 <%--</tbody>--%>
                 </table>
                 </FooterTemplate>
    </asp:Repeater>
        </div>
    </div>

     <div class="form-row" id="factura" runat="server" style=" display:NONE" >
       <div class="alert alert-warning" id="alertafu" runat="server"></div>
         <div class="form-group col-md-6" runat="server"  id="tablefac">
               <label for="inputEmail4">Adjuntar factura:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                <asp:FileUpload runat="server" ID="fuAdjuntarFactura" class="form-control" extension='.pdf'  title="Adjunte el archivo en formato PDF." onchange="validaextension(this)"/>
         </div>
     </div>


     <div class="row" runat="server" id="salir" visible="false">
          <div class="col-md-12 d-flex justify-content-center">
        <asp:Button ID="btnSalir" runat="server" Text="Salir" class="btn btn-primary"  
                onclick="btnSalir_Click" 
                ToolTip="Regresa a la Pantalla Consultar Solicitud."/> 

          </div>
    </div>

     <div class="row" runat="server" id="botonera">
           <div class="col-md-12 d-flex justify-content-center">

                <asp:Button Text="Rechazar" ID="btnRechazar" runat="server" class="btn btn-primary"
                        onclick="btnRechazar_Click" OnClientClick="return prepareObjectRechazar('¿Esta seguro de rechazar la solicitud?');"
                        ToolTip="Rechaza la solicitud."/>
       
                <asp:Button ID="btsalvar" runat="server" Text="Crear Permiso(s)" class="btn btn-primary"
                        OnClientClick="return prepareObject();" onclick="btsalvar_Click" 
                        ToolTip="Crear el Permiso de Acceso."/> 

           </div>
      </div>

    
    </form>

  </div>
    <script src="../../Scripts/pages.js" type="text/javascript"></script>
    <script src="../../Scripts/credenciales.js" type="text/javascript"></script>
     <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    <%--<script src="../../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>--%>
   <script type="text/javascript" >
       //$(document).ready(function () {
       //    //inicia los fecha
       //    $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/Y' });
       //});

       function setObject(row) {
            self.close();
        }
        function prepareObjectRechazar() {
            var valor = '¿Esta seguro de rechazar la solicitud?';
            if (confirm(valor) == false) {
                return false;
            }
        }
        function prepareObject() {
            try {
                var valor = '¿Esta seguro de procesar la solicitud?';
                if (confirm(valor) == false) {
                    return false;
                }
//                var vals = document.getElementById('<%=fuAdjuntarFactura.ClientID %>').value;
//                if (vals == '' || vals == null || vals == undefined) {
//                    alert('Adjunte la factura.');
//                    document.getElementById('<%=fuAdjuntarFactura.ClientID %>').focus();
//                    document.getElementById('<%=fuAdjuntarFactura.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:100%;";
//                    return false;
                //                }
                lista = [];
                var tbl = document.getElementById('tableRpt');
                for (var f = 0; f < tbl.rows.length; f++) {
                    var celColect = tbl.rows[f].getElementsByTagName('td');
                    if (celColect != undefined && celColect != null && celColect.length > 0) {
                        var vals = celColect[10].getElementsByTagName('input')[0].checked;
                        if (vals == true) {
//                            var vals = celColect[11].getElementsByTagName('select')[0].value;
//                            if (vals == '0' || vals == null || vals == undefined) {
//                                alert('Seleccione el tipo de Permiso OC');
//                                celColect[11].getElementsByTagName('select')[0].focus();
//                                celColect[11].getElementsByTagName('select')[0].style.cssText = "background-color:#ffffc6;color:Red;width:80px;";
//                                return false;
                            //                            }
                            var vals = celColect[12].getElementsByTagName('input')[0].value;
                            if (vals == '' || vals == null || vals == undefined) {
                                alertify.alert('Seleccione la Fecha de Ingreso OC').set('label', 'Aceptar');
                                celColect[12].getElementsByTagName('input')[0].focus();
                                celColect[12].getElementsByTagName('input')[0].style.cssText = "background-color:#ffffc6;color:Red;width:80px;";
                                return false;
                            }
                            var vals = celColect[13].getElementsByTagName('input')[0].value;
                            if (vals == '' || vals == null || vals == undefined) {
                                alertify.alert('Seleccione la Fecha de Caducidad OC').set('label', 'Aceptar');
                                celColect[13].getElementsByTagName('input')[0].focus();
                                celColect[13].getElementsByTagName('input')[0].style.cssText = "background-color:#ffffc6;color:Red;width:80px;";
                                return false;
                            }
                            var vals = celColect[14].getElementsByTagName('select')[0].value;
                            if (vals == '0' || vals == null || vals == undefined) {
                                alertify.alert('Seleccione el Turno OC').set('label', 'Aceptar');
                                celColect[14].getElementsByTagName('select')[0].focus();
                                celColect[14].getElementsByTagName('select')[0].style.cssText = "background-color:#ffffc6;color:Red;width:80px;";
                                return false;
                            }
//                            var vals = celColect[15].getElementsByTagName('select')[0].value;
//                            if (vals == '0' || vals == null || vals == undefined) {
//                                alert('Seleccione el Departamento OC');
//                                celColect[15].getElementsByTagName('select')[0].focus();
//                                celColect[15].getElementsByTagName('select')[0].style.cssText = "background-color:#ffffc6;color:Red;width:80px;";
//                                return false;
//                            }
//                            var vals = celColect[16].getElementsByTagName('select')[0].value;
//                            if (vals == '0' || vals == null || vals == undefined) {
//                                alert('Seleccione el Cargo OC');
//                                celColect[16].getElementsByTagName('select')[0].focus();
//                                celColect[16].getElementsByTagName('select')[0].style.cssText = "background-color:#ffffc6;color:Red;width:80px;";
//                                return false;
//                            }
                        }
                        var vals = celColect[14].getElementsByTagName('select')[0].value;
                        var valst = celColect[14].getElementsByTagName('select')[0];
                        var valsturno = valst.options[valst.selectedIndex].text;
                        if (vals == '0' || vals == null || vals == undefined || valsturno == '* Elija *') {
                            alertify.alert('Seleccione el Turno OC').set('label', 'Aceptar');
                            celColect[14].getElementsByTagName('select')[0].focus();
                            celColect[14].getElementsByTagName('select')[0].style.cssText = "background-color:#ffffc6;color:Red;width:150px;";
                            return false;
                        }
                        var vals = celColect[18].getElementsByTagName('input')[0].checked;
                        if (vals == true) {
                            var vals = celColect[19].getElementsByTagName('input')[0].value;
                            if (vals == '' || vals == null || vals == undefined) {
                                alertify.alert('Escriba el comentario de rechazo').set('label', 'Aceptar');
                                celColect[18].getElementsByTagName('input')[0].focus();
                                celColect[18].getElementsByTagName('input')[0].style.cssText = "background-color:#ffffc6;color:Red;width:80px;";
                                return false;
                            }
                        }
                        var vals = celColect[10].getElementsByTagName('input')[0].checked;
                        if (vals == false) {
                            var vals1 = celColect[11].getElementsByTagName('select')[0].value;
                            var vals2 = celColect[12].getElementsByTagName('input')[0].value;
                            var vals3 = celColect[13].getElementsByTagName('input')[0].value;
                            var vals4 = celColect[14].getElementsByTagName('select')[0].value;
                            var vals5 = celColect[15].getElementsByTagName('select')[0].value;
                            var vals6 = celColect[16].getElementsByTagName('select')[0].value;
                            if (vals1 != '0' && vals2 != '' && vals3 != '' && vals4 != '0' && vals5 != '0' && vals6 != '0') {
                                alertify.alert('De check en la casilla Cambiar Permiso').set('label', 'Aceptar');
                                celColect[10].getElementsByTagName('input')[0].focus();
                                celColect[10].getElementsByTagName('input')[0].style.cssText = "background-color:Red;color:Red;width:50px;";
                                return false;
                            }
                        }
                    }
                }
                return true;
            } catch (e) {
                alertify.alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message).set('label', 'Aceptar');
            }
        }
//        function valToolTip(index) {
//            lista = [];
//            var tbl = document.getElementById('tableRpt');
//            for (var f = 0; f < tbl.rows.length; f++) {
//                var celColect = tbl.rows[f].getElementsByTagName('td');
//                if (celColect != undefined && celColect != null && celColect.length > 0) {
//                    var vals = celColect[index].getElementsByTagName('select')[0].value;
//                    if (vals != '0' || vals != null || vals != undefined) {
//                        alert(vals);
//                        celColect[index].getElementsByTagName('select')[0].tooltip = vals;
//                    }
//                }
//            }
//        }
        function valPermiso() {
            lista = [];
            var tbl = document.getElementById('tableRpt');
            for (var f = 0; f < tbl.rows.length; f++) {
                var celColect = tbl.rows[f].getElementsByTagName('td');
                if (celColect != undefined && celColect != null && celColect.length > 0) {
                    if (celColect[10].getElementsByTagName('input')[0].checked == true) {
                        celColect[17].getElementsByTagName('input')[0].checked = false;
                    }
                }
            }
        }
        function valRechazado() {
            lista = [];
            var tbl = document.getElementById('tableRpt');
            for (var f = 0; f < tbl.rows.length; f++) {
                var celColect = tbl.rows[f].getElementsByTagName('td');
                if (celColect != undefined && celColect != null && celColect.length > 0) {
                    if (celColect[17].getElementsByTagName('input')[0].checked == true) {
                        celColect[10].getElementsByTagName('input')[0].checked = false
                    }
                }
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
          window.open('../credenciales/revisasolicitudcolaboradordocumentos.aspx?numsolicitud=' + caja + '&idsolcol=' + caja2 + '&cedula=' + caja3)
      }
   </script>
      <script type="text/javascript">
            $(document).ready(function () {
                 $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/y' });
              });    
      </script>
</body>
</html>
