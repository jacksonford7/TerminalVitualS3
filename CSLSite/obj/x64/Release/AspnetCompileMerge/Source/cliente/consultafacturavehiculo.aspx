<%@ Page  Language="C#"  AutoEventWireup="true" Title="Consultar Factura"
         CodeBehind="consultafacturavehiculo.aspx.cs" Inherits="CSLSite.cliente.consultafacturavehiculo" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Consultar Factura</title>
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
<div class="dashboard-container p-4" id="cuerpo" runat="server">

     <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Consola de Comprobantes</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Consultar Factura</li>
          </ol>
        </nav>
      </div>

   

    <form id="bookingfrm" runat="server">
    <input id="zonaid" type="hidden" value="7" />
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

     <div class="form-title">
         1) Tipo de Solicitud.
     </div>

     <div class="form-row">
         <div class="form-group col-md-6">
               <label for="inputEmail4">Tipo de Solicitud:<span style="color: #FF0000; font-weight: bold;"></span></label>
              <asp:TextBox ID="txttipcli" runat="server" class="form-control" MaxLength="500" disabled
             style="text-align: center;"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>
         </div>
     </div>

   <div class="" style=" display:none"> Criterios de consulta:</div>
    <div class="form-row" style=" display:none">

         <div class="form-group col-md-6">
              <label for="inputEmail4">Número de Solicitud:</label>
              <asp:TextBox ID="txtsolicitud" runat="server" class="form-control" MaxLength="11"
             style="text-align: center" onblur="cajaControl(this);"
             onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
         </div> 
         <div class="form-group col-md-6">
              <label for="inputEmail4">Generados desde / hasta:</label>
             <asp:TextBox ID="tfechaini" runat="server" class="form-control" MaxLength="10" CssClass="datetimepicker form-control"
             onkeypress="return soloLetras(event,'01234567890/')" style="text-align: center" 
                 onblur="valDate(this,true,valdate);" ClientIDMode="Static"></asp:TextBox>
         </div> 
         <div class="form-group col-md-6">
              <label for="inputEmail4">Generados desde / hasta:</label>
               <asp:TextBox ID="tfechafin" runat="server" ClientIDMode="Static" class="form-control" MaxLength="15" CssClass="datetimepicker form-control"
             onkeypress="return soloLetras(event,'01234567890/')"  style="text-align: center" 
                  onblur="valDate(this,true,valdate);"></asp:TextBox>
         </div> 
         <div class="form-group col-md-6">
              <label for="inputEmail4">Todas las facturas.</label>
             <label class="checkbox-container">
              <asp:CheckBox Text="" ID="chkTodos" runat="server" />
                  <span class="checkmark"></span>
                   </label>  
         </div> 
        <div class="form-group col-md-6">
              <label for="inputEmail4">Generados desde / hasta:</label>

         </div> 

    </div> 

    <div class="row" style=" display:none">
           <div class="col-md-12 d-flex justify-content-center">
                  <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
               <br/>
                     <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda" class="btn btn-primary"  
                       onclick="btbuscar_Click"/>
           </div>
    </div>

     <div class="form-title">
        2) Datos del Vehículo(s).
     </div>
        
      <div class="form-row" id="colector" >
           <div class="form-group col-md-12">   
                 <asp:Repeater ID="rpVehiculos" runat="server" >
                 <HeaderTemplate>
                 <table id="tabla"  cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                 <thead>
                 <tr>
                 <%--<th># Solicitud</th>--%>
                 <th class="nover" >IdSolVeh</th>
                 <th class="nover" >Tipo Solicitud</th>
                 <th>Placa</th>
                 <th>ClaseTipo</th>
                 <th>Marca</th>
                 <th>Modelo</th>
                 <th>Color</th>
                 <th>Categoria</th>
                 <th>Tipo Certificado</th>
                 <th>Nº Certificado</th>
                 <th>Fecha Poliza</th>
                 <th>Fecha MTOP</th>
                 <th>Vehiculo Rechazado</th>
                 <th>Comentario</th>
                 <th></th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <%--<td style=" width:65px"><%#Eval("NUMSOLICITUD")%></td>--%>
                  <td class="nover" ><%#Eval("IDSOLVEH")%></td>
                  <td class="nover" ><%#Eval("TIPOSOLICITUD")%></td>
                  <td><asp:Label Text='<%#Eval("PLACA")%>' runat="server" ID="lblplaca"></asp:Label></td>
                  <td><%#Eval("CLASETIPO")%></td>
                  <td><%#Eval("MARCA")%></td>
                  <td><%#Eval("MODELO")%></td>
                  <td><%#Eval("COLOR")%></td>
                  <td><%#Eval("DESCRIPCIONCATEGORIA")%></td>
                  <td><%#Eval("TIPOCERTIFICADO")%></td>
                  <td><%#Eval("CERTIFICADO")%></td>
                  <td ><%#Eval("FECHAPOLIZA")%></td>
                  <td ><%#Eval("FECHAMTOP")%></td>
                  <td > <label class="checkbox-container">
                      <asp:CheckBox runat="server" ForeColor="Red" Checked='<%#Eval("ESTADOCOL")%>' Enabled="false" id="chkRevisado"/>
                        <span class="checkmark"></span>
                   </label>  
                      </td>
                  <td><asp:TextBox ID="tcomentario" Enabled="false" ForeColor="Red" Text='<%#Eval("COMENTARIO")%>' runat="server" class="form-control"
                      onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox></td>
                  <td>
                    <a  id="adjDoc" class="btn btn-outline-primary"onclick="redirectsol('<%# Eval("NUMSOLICITUD") %>', '<%# Eval("IDSOLVEH") %>', '<%#Eval("PLACA")%>');">
                    <i class="fa fa-search"></i> Ver Documentos </a>
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

    <div class="form-title">
        3) Datos de la factura.
     </div>
     <div class="">Revise la factura y una vez realizado el pago o deposito por favor adjunte el comprobante o retención.</div>
        
        <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
        <input id="diponible" type="hidden" runat="server" clientidmode="Static" />

        <div id="xfinder" runat="server" visible="false">

            <div class="alert alert-warning" id="alerta" runat="server" ></div>
        
             <div class="form-row">
                <div class="separator">Información disponibles:</div>
                    <div class="form-group col-md-12">                   
                  <%--   <table id="tablerp" class="table table-bordered invoice" cellpadding="1" cellspacing="0">--%>
                     <asp:Repeater ID="tablePagination" runat="server">
                     <HeaderTemplate>
                     <table id="tablar"  cellspacing="1" cellpadding="1" class="table table-bordered invoice" >
                     <thead>
                     <tr>
                     <th># Solicitud</th>
                     <th>Tipo Solicitud</th>
                     <th>Factura</th>
                     <th>Estado</th>
                     <th>Adjunte el comprobante de pago/Retención.</th>
                     <th style=" display:none"></th>
                     <th>Observación de rechazo.</th>
                     <th>Comprobante de pago/Retención rechazado.</th>
                     </tr>
                     </thead> 
                     <tbody>
                     </HeaderTemplate>
                     <ItemTemplate>
                     <tr class="point" >
                      <td ><asp:Label runat="server" Text='<%#Eval("NUMSOLICITUD")%>' ID="lblIdSolicitud" /></td>
                      <td><%#Eval("TIPOSOLICITUD")%></td>
                      <td >
                      <a href='<%#Eval("RUTADOCUMENTO") %>' class="btn btn-outline-primary" target="_blank">
                        <i class="fa fa-search"></i> Ver Documento </a>
                     </td>
                     <td ><%#Eval("ESTADO")%></td>
                     <td >
                        <asp:FileUpload extension='<%#Eval("EXTENSION") %>' class="btn btn-outline-primary" id="fsupload" title="Escoja el archivo en formato PDF."
                               onchange="validaextension(this)" style=" font-size:small" runat="server"/>
                    </td>
                    <td style=" display:none"><asp:Label runat="server" Text='<%#Eval("CODESTADO")%>' ID="lblEstado" /></td>
                    <td><%#Eval("COMRECHAZO")%></td>
                    <td>
                      <a href='<%#Eval("RUTADOCRECHAZO") %>' style=" width:80px" class="btn btn-outline-primary" target="_blank">
                        <i></i> Ver Documento </a>
                     </td>
                     </tr>
                     </ItemTemplate>
                     <FooterTemplate>
                     </tbody>
                     </table>
                     </FooterTemplate>
             </asp:Repeater>
                    <%-- </table>--%>
                    </div>
       
            </div>
      
            <div class="row" runat="server" id="botonera">
                           <div class="col-md-12 d-flex justify-content-center">
                    <img alt="loading.." src="../shared/imgs/loader.gif" id="Img1" class="nover"  /><br/>
                <asp:Button ID="btsalvar" runat="server" Text="Enviar Comprobante" class="btn btn-primary"  
                        OnClientClick="return prepareObject('¿Esta seguro de enviar el comprobante de pago?');" onclick="btsalvar_Click" 
                        ToolTip="Envia el comprobante de pago."/>
                </div>
            </div>
        </div>
     
                  
                  
       <div id="xfinderpagado" runat="server" style=" display:none">
        <div class="alert alert-warning" id="alertapagado" runat="server" ></div>

        <div class="separator">Facturas disponibles:</div>

            <div class="form-row">
                  <div class="form-group col-md-12">       
               
                     <asp:Repeater ID="tablePagado" runat="server" >
                     <HeaderTemplate>
                     <table id="tablar"  cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                     <thead>
                     <tr>
                     <th># Solicitud</th>
                     <th>Tipo Solicitud</th>
                     <th>Estado</th>
              
                     </tr>
                     </thead> 
                     <tbody>
                     </HeaderTemplate>
                     <ItemTemplate>
                     <tr class="point" >
                      <td><asp:Label runat="server" Text='<%#Eval("NUMSOLICITUD")%>' ID="lblIdSolicitud" /></td>
                      <td ><%#Eval("TIPOSOLICITUD")%></td>
                      <td><%#Eval("ESTADO")%></td>
                
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
     

       <div id="sinresultado" runat="server" class="alert alert-warning"></div>
       
       
 </form>


 </div>  
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>
     <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

    <script type="text/javascript">
       
        function popupCallback(objeto, catalogo) {
            if (objeto == null || objeto == undefined) {
                alertify.alert('Hubo un problema al setaar un objeto de catalogo').set('label', 'Aceptar');
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
        function prepareObject() {
            try {
                if (confirm('¿Esta seguro de enviar el comprobante de pago/Retención.?') == false) {
                    return false;
                }
                lista = [];
                var vals = document.getElementById('tablar');
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* Consultar Solicitud de Factura: *\n *No se encontraron Documentos*').set('label', 'Aceptar');
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (vals != null) {
                    var tbl = document.getElementById('tablar');
                    for (var f = 0; f < tbl.rows.length; f++) {
                        var celColect = tbl.rows[f].getElementsByTagName('td');
                        if (celColect != undefined && celColect != null && celColect.length > 0) {
                            var tdetalle = {
                                documento: celColect[4].getElementsByTagName('input')[0].value
                            };
                            this.lista.push(tdetalle);
                        }
                    }
                    //                var nomdoc = null;
                    for (var n = 0; n < this.lista.length; n++) {
                        if (lista[n].documento == '' || lista[n].documento == null || lista[n].documento == undefined) {
                            alertify.alert('* Consultar Solicitud de Factura: *\n * Adjunte el recibo de pago. *').set('label', 'Aceptar');
                            document.getElementById("loader").className = 'nover';
                            return false;
                        }
                       
                    }
                }
                document.getElementById("loader").className = '';
                return true;
            } catch (e) {
                alertify.alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message).set('label', 'Aceptar');
            }
        }

        function openPop() {
            var bo = document.getElementById('nbrboo').value;
            if (bo == undefined || bo == '' || bo == null) {
                alertify.alert('Por favor seleccione el booking primero').set('label', 'Aceptar');
                return;
            }
            window.open('../catalogo/calendarioConsolidacion.aspx?bk=' + bo, 'name', 'width=850,height=880')
        }

        function redirectsol(val, val2, val3) {
            var caja = val;
            var caja2 = val2;
            var caja3 = val3;
            window.open('../cliente/consultasolicitudvehiculodocumentos.aspx?numsolicitud=' + caja + '&idsolveh=' + caja2 + '&placa=' + caja3)
        }
    </script>

      <script type="text/javascript">
            $(document).ready(function () {
                 $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/y' });
              });    
      </script>

</body>
</html>

