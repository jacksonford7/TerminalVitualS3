<%@ Page  Language="C#"  AutoEventWireup="true" Title="Confirmación de Pago/Registro de Vehículo" CodeBehind="consultacomprobantedepagovehiculos.aspx.cs" Inherits="CSLSite.consultacomprobantedepagovehiculos" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>Confirmación de Pago/Registro de Vehículo</title>
  
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

    <script type="text/javascript">
        function BindFunctions()
        {
            $(document).ready(function () {
                        $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'd/m/Y' });
            });    
        }
    </script>
    
</head>
<body>
<div class="dashboard-container p-4" id="cuerpo" runat="server">

      <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Consola de Comprobantes</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Datos del Vehículo(s)</li>
          </ol>
        </nav>
      </div>

 <form id="bookingfrm" runat="server">

    <input id="zonaid" type="hidden" value="7" />
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

     <div class="form-title">
         1) Datos del Vehículo(s).
     </div>

       


<%--     <div class="form-row">--%>
           <div class="bokindetalle" style="width:100%;overflow:auto">
          <asp:Repeater ID="rpVehiculos" runat="server" >
                     <HeaderTemplate>
                     <table id="tabla"  cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                     <thead>
                     <tr>
       
                     <th class="nover">IdSolVeh</th>
                     <th class="nover">Tipo Solicitud</th>
                     <th>Placa</th>
                     <th>ClaseTipo</th>
                     <th>Marca</th>
                     <th>Modelo</th>
                     <th>Color</th>
                     <th>Categoria</th>
                     <th>Área Destino/Actividad</th>
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
               
                      <td class="nover"><%#Eval("IDSOLVEH")%></td>
                      <td class="nover"><%#Eval("TIPOSOLICITUD")%></td>
                      <td><asp:Label Text='<%#Eval("PLACA")%>' runat="server" ID="lblplaca"></asp:Label></td>
                      <td><%#Eval("CLASETIPO")%></td>
                      <td><%#Eval("MARCA")%></td>
                      <td><%#Eval("MODELO")%></td>
                      <td><%#Eval("COLOR")%></td>
                      <td><%#Eval("DESCRIPCIONCATEGORIA")%></td>
                      <td style=" width:100px"><%#Eval("AREA")%></td>
                      <td><%#Eval("TIPOCERTIFICADO")%></td>
                      <td><%#Eval("CERTIFICADO")%></td>
                      <td><%#Eval("FECHAPOLIZA")%></td>
                      <td><%#Eval("FECHAMTOP")%></td>
                      <td> 
                          <label class="checkbox-container">
                          <asp:CheckBox runat="server" ForeColor="Red" Checked='<%#Eval("ESTADOCOL")%>' Enabled="false" id="chkRevisado"/>
                          <span class="checkmark"></span>
                          </label>
                      </td>
                      <td ><asp:TextBox ID="tcomentario" class="form-control" ForeColor="Red" Text='<%#Eval("COMENTARIO")%>' runat="server" disabled  onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox></td>
                      <td >
                        <a  id="adjDoc" class="btn btn-primary" onclick="redirectsol('<%# Eval("NUMSOLICITUD") %>', '<%# Eval("IDSOLVEH") %>', '<%#Eval("PLACA")%>');">
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
     <%--</div>--%>

       <div class="form-title">
         2)  Documentación.
     </div>


        <div class="form-row" style="display:none">
            <div class="form-group col-md-6">
                   <label for="inputEmail4">Número de Solicitud:</label>
                      <asp:TextBox ID="txtsolicitud" runat="server" class="form-control"  MaxLength="11"
                         style="text-align: center"
                         onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
                
            </div> 
            <div class="form-group col-md-6" style=" display:none">
                   <label for="inputEmail4">Generados desde / hasta:</label>
                 <div class="d-flex">
                      <asp:TextBox ID="tfechaini" runat="server"  MaxLength="10" CssClass="datetimepicker form-control"
                     onkeypress="return soloLetras(event,'01234567890/')" style="text-align: center" 
                         onblur="valDate(this,true,valdate);" ClientIDMode="Static"></asp:TextBox>
           
                    <asp:TextBox ID="tfechafin" runat="server" ClientIDMode="Static" MaxLength="15" CssClass="datetimepicker form-control"
                     onkeypress="return soloLetras(event,'01234567890/')"  style="text-align: center" 
                          onblur="valDate(this,true,valdate);"></asp:TextBox>

                 </div> 
            </div> 
             <div class="form-group col-md-6" style=" display:none">
                   <label for="inputEmail4">Todas las facturas.</label>
                  <label class="checkbox-container">
                  <asp:CheckBox Text="" ID="chkTodos" runat="server" />
                      <span class="checkmark"></span>
                   </label>  
            </div> 

            <br/>
            <div class="form-group col-md-12">
                <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda" class="btn btn-primary"
                onclick="btbuscar_Click"/>
           </div>
        </div>
       
    
        <div class="form-row">

            <div class="alert alert-warning" id="alertapagado" runat="server" ></div>

                   <div class="form-group col-md-12">                    
                     <asp:GridView runat="server" id="gvComprobantes" class="table table-bordered invoice" AutoGenerateColumns="False">
                     <Columns>
                        <asp:TemplateField HeaderText="# Solicitud" ItemStyle-Width="80px">
                          <ItemTemplate>
                              <asp:Label runat="server" Text='<%#Eval("NUMSOLICITUD")%>' ID="lblIdSolicitud" />
                          </ItemTemplate>
                      </asp:TemplateField>
                      <asp:TemplateField HeaderText="Tipo Solicitud">
                          <ItemTemplate>
                              <asp:Label runat="server" Text='<%#Eval("TIPOSOLICITUD")%>' ID="lblTipoSolicitud" />
                          </ItemTemplate>
                      </asp:TemplateField>
                      <asp:TemplateField HeaderText="Documento/Estado">
                          <ItemTemplate>
                              <asp:Label runat="server" Text='<%#Eval("ESTADO")%>' ID="lblCodEstado" />
                          </ItemTemplate>
                      </asp:TemplateField>
                      <asp:TemplateField HeaderText="">
                          <ItemTemplate>
                              <a href='<%#Eval("RUTADOCUMENTO") %>' class="btn btn-primary" target="_blank">
                              <i></i> Ver Documento </a>
                          </ItemTemplate>
                      </asp:TemplateField>
                     </Columns>
                     </asp:GridView>
                   
                    </div>
            </div>

        <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
        <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
        <div id="xfinderpagado" runat="server">

            <div runat="server" id="divseccion2">
       
                 <div class="form-title">
                    3) Dcocumentos a revisar.
                 </div>

                  <div runat="server" id="divcabecera" >
                       <div class="form-row" runat="server" id="divnumfactura">
                            <div class="form-group col-md-6">
                                 <label for="inputEmail4">Número de Factura:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                                  <asp:TextBox ID="txtnumfactura" runat="server" class="form-control" MaxLength="30 "
                             style="text-align: center" onblur="checkcaja(this,'valnumfac',true);"
                             onkeypress="return soloLetras(event,'01234567890-',true)"></asp:TextBox>
                                <span id="valnumfac" class="validacion"> </span>
                            </div> 
                             <div class="form-group col-md-6" style=" display:none">
                                 <label for="inputEmail4">Permiso:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                                    <asp:DropDownList ID="dptipoevento" runat="server" class="form-control" AutoPostBack="false" OnSelectedIndexChanged="dptipoevento_SelectedIndexChanged"
                                        onchange="valdltipsol(this, valtipeve);">
                                 <asp:ListItem Value="0">* Seleccione permiso *</asp:ListItem>
                                </asp:DropDownList>
                                <span id='valtipeve' class="validacion" ></span>
                            </div> 
                            <div class="form-group col-md-6" style=" display:none">
                                 <label for="inputEmail4">Fecha de Ingreso:<span style="color: #FF0000; font-weight: bold;"> *</span></label> 
                                  <asp:TextBox 
                                     style="text-align: center" 
                                     ID="txtfecing" runat="server"   MaxLength="15" CssClass="datetimepicker form-control" ClientIDMode="Static"
                                     onkeypress="return soloLetras(event,'0123456789/')" onblur="checkcaja(this,'valfecing',true);"
                                     ></asp:TextBox>
                                    <span id="valfecing" class="validacion"></span>
                            </div> 
                            <div class="form-group col-md-6" style=" display:none">
                                 <label for="inputEmail4">Fecha de Caducidad:<span style="color: #FF0000; font-weight: bold;"> *</span></label> 
                                 <asp:TextBox 
                                     style="text-align: center" 
                                     ID="txtfecsal" runat="server" MaxLength="15" CssClass="datetimepicker form-control" ClientIDMode="Static"
                                     onkeypress="return soloLetras(event,'0123456789/')" onblur="checkcaja(this,'valfecsal',true);"
                                     ></asp:TextBox>
                                <span id="valfecsal" class="validacion"></span>
                            </div> 

                        </div>
                  </div>
            </div>

             <div class="form-title">
                4) Observación en caso de rechazo.
             </div>
           
            <div class="form-row" runat="server" id="divseccion3">
                 <div class="form-group col-md-12">
                       <asp:TextBox ID="txtmotivorechazo" ForeColor="Red" runat="server" class="form-control" MaxLength="500"
                     style="text-align: center"  onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox>
                 </div>

            </div>
       
        </div>

         <div id="sinresultado" runat="server" class="alert alert-warning"></div>
    
         <div class="row"  runat="server" id="botonera">
           <div class="col-md-12 d-flex justify-content-center">
              <span id="imagen"></span>
                <asp:Button Text="Rechazar" ID="btnRechazar" runat="server" class="btn btn-primary"  
                        onclick="btnRechazar_Click" OnClientClick="return prepareObjRechazo();"
                        ToolTip="Rechaza la solicitud."/>&nbsp;&nbsp;
                <asp:Button ID="btsalvar" runat="server" Text="Crear Vehiculo(s)" class="btn btn-primary"  
                        OnClientClick="return prepareObject();" onclick="btsalvar_Click" 
                        ToolTip="Crea ek vehiculo(s) en OnlyControl."/>
          </div>
        </div>
        
          
      
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>
     <%--<script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>--%>

      <script type="text/javascript">
       $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });
    </script>
   <%-- <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
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
        function popupCallback(objeto, catalogo) {

        }

        function clear() {
            document.getElementById('numbook').textContent = '...';
            document.getElementById('nbrboo').value = '';
        }
        function clear2() {
            document.getElementById('txtfecha').textContent = '...';
            document.getElementById('xfecha').value = '';
        }

        function prepareObjRechazo() {
            try {
                lista = [];
                if (confirm('¿Esta seguro de rechazar la confirmaciòn de pago?') == false) {
                    return false;
                }
                var vals = document.getElementById('<%=txtmotivorechazo.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* Escriba la observación de rechazo. *').set('label', 'Aceptar');
                    document.getElementById('<%=txtmotivorechazo.ClientID %>').focus();

                  
                    return false;
                }
                return true;
             } catch (e) {
                 alertify.alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message).set('label', 'Aceptar');
             }
        }
        function prepareObject() {
            try {
                lista = [];
                if (confirm('¿Esta seguro de la confirmación de pago?') == false) {
                    return false;
                }
                var vals = document.getElementById('<%=txtsolicitud.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('*Escriba el numero de solicitud *').set('label', 'Aceptar');
                    document.getElementById('<%=txtsolicitud.ClientID %>').focus();

                    return false;
                }
                var vals = document.getElementById('<%=txtnumfactura.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('*Escriba el Número de factura *').set('label', 'Aceptar');
                    document.getElementById('<%=txtnumfactura.ClientID %>').focus();
  

                    return false;
                }

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
   
            window.open('../revisasolicitudvehiculodocumentos.aspx/?numsolicitud=' + caja + '&idsolveh=' + caja2 + '&placa=' + caja3)
        }
        function getGif() {
            document.getElementById('imagen').innerHTML = '<img alt="" src="../loader.gif"/>';
            return true;
        }
        function getGifOculta() {
            document.getElementById('imagen').innerHTML = '<img alt="" src=""/>';
            return true;
        }
    </script>

    </form>
   </div> 
     <%-- <script type="text/javascript">
            $(document).ready(function () {
                 $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/y' });
              });    
      </script>--%>

         </body>
    </html>
    