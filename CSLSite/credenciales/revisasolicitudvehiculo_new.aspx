<%@ Page Language="C#" AutoEventWireup="true" Title="Emisión Permiso Vehicular" CodeBehind="revisasolicitudvehiculo_new.aspx.cs" Inherits="CSLSite.revisasolicitudvehiculo_new" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Solicitud de Sticker/Provisional Vehícular</title>
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

    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
   <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

 
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
     <div id="div_BrowserWindowName" style="visibility:hidden;">
        <asp:HiddenField ID="HiddenField1" runat="server" />
    </div>
    <input id="zonaid" type="hidden" value="7" />
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
     
    <div class="form-row">
        <div class="form-title col-md-12">
            <a class="btn btn-outline-primary mr-4" href="#" runat="server" id="aprint" clientidmode="Static" >1</a>
            <a class="level1" runat="server" id="a1" clientidmode="Static" >Tipo de Solicitud</a>
        </div>

        <div class="form-group col-md-3">
           
            <label for="inputAddress">Tipo de Solicitud :<span style="color: #FF0000; font-weight: bold;"> *</span></label>
            <asp:TextBox ID="txttipcli" runat="server" MaxLength="500" Enabled="false" style="text-align: center;"
            onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)" CssClass="form-control"></asp:TextBox>
        </div>

        
    </div>

    <div class="form-row">
        <div class="form-title col-md-12">
             <a class="btn btn-outline-primary mr-4" href="#" runat="server" id="a2" clientidmode="Static" >2</a>
             <a class="level1" runat="server" id="a3" clientidmode="Static" >Datos del Vehículo(s)</a>
         </div>

        <div class="informativo" id="colector" style=" overflow:auto">
            <div class="alert alert-danger" id="alerta" runat="server"></div>
            <asp:Repeater ID="tablePagination" runat="server" >
                <HeaderTemplate>
                <table id="tablar2"  cellspacing="1" cellpadding="1" class="table table-bordered invoice" style=" font-size:small">
                <thead>
                <tr>
                <th class="nover">IdSolVeh</th>
                <th>Placa</th>
                <th>ClaseTipo</th>
                <th>Marca</th>
                <th>Modelo</th>
                <th>Color</th>
                <th>Tipo</th>
                <th>Categoria</th>
                <th>Área Destino/Actividad</th>
                <th>Tipo Certificado</th>
                <th>Nº Certificado</th>
                <th>Fecha Poliza</th>
                <th>Fecha MTOP</th>
                <th>Vehiculo Rechazado</th>
                <th>Comentario</th>
                <th></th>
                <th style=" display:none"></th>
                </tr>
                </thead> 
                <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                <tr class="point" >
                <td class="nover"><%#Eval("IDSOLVEH")%></td>
                <td><asp:Label Text='<%#Eval("PLACA")%>' runat="server" ID="lblplaca"></asp:Label></td>
                <td><%#Eval("CLASETIPO")%></td>
                <td><%#Eval("MARCA")%></td>
                <td><%#Eval("MODELO")%></td>
                <td><%#Eval("COLOR")%></td>
                <td ><%#Eval("TIPO")%></td>
                <td><%#Eval("DESCRIPCIONCATEGORIA")%></td>
                <td ><%#Eval("AREA")%></td>
                <td><%#Eval("TIPOCERTIFICADO")%></td>
                <td><%#Eval("CERTIFICADO")%></td>
                <td ><%#Eval("FECHAPOLIZA")%></td>
                <td ><%#Eval("FECHAMTOP")%></td>
                <td ><asp:CheckBox runat="server" ForeColor="Red" Enabled="false" id="chkRevisado"/></td>
                <td ><asp:TextBox ID="tcomentario" ForeColor="Red" runat="server" Text='' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox></td>
                <td >
                <a id="adjDoc" class="btn btn-outline-primary" onclick="redirectsol('<%# Eval("NUMSOLICITUD") %>', '<%# Eval("IDSOLVEH") %>', '<%#Eval("PLACA")%>');">
                <i class="fa-fa-search"></i> Ver Documentos </a>
                </td>
                <td style=" display:none"><asp:TextBox ID="tced" runat="server" Text='<%#Eval("PLACA")%>'></asp:TextBox></td>
                </tr>
                </ItemTemplate>
                <FooterTemplate>
                </tbody>
                </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
      <%--<div class="alert alert-warning" id="alertafu" runat="server"></div>--%>

    <div  id="factura" runat="server" >
        <div class="form-row"> 
            <div class="form-group col-md-6">
                <label for="inputEmail4">Adjuntar factura:</label>
                <asp:FileUpload runat="server" ID="fuAdjuntarFactura"  extension='.pdf' class="btn btn-outline-primary m-4" title="Escoja el archivo en formato PDF." onchange="validaextension(this)"/>
                      
            </div> 
        </div> 

        <div class="form-row">
            <div class="form-group col-md-12">
                <label for="inputZip"># DOCUMENTO<span style="color: #FF0000; font-weight: bold;">*</span></label>
                <asp:TextBox ID="TxtNumdocumento" runat="server" class="form-control" MaxLength="25"  onkeypress="return soloLetras(event,'0123456789')"  
                                    placeholder="# FACTURA"></asp:TextBox>
            </div>
        </div>
    </div>
    
    <div class="row" runat="server" id="salir" visible="false">
        <div class="col-md-12 d-flex justify-content-center">
            <asp:Button ID="btnSalir" runat="server" Text="Regresar" class="btn btn-outline-primary"  
            onclick="btnSalir_Click" 
            ToolTip="Regresa a la Pantalla Consultar Solicitud."/>
        </div>
    </div>
        <%--OnClientClick="return prepareObject('¿Esta seguro de procesar la solicitud?');"--%>
      <div class="row" runat="server" id="botonera">
           <div class="col-md-12 d-flex justify-content-center">
                <img alt="loading.." src="../shared/imgs/loader.gif" id="Img1" class="nover"  />&nbsp;
                <asp:Button Text="Rechazar" ID="btnRechazar" runat="server" class="btn btn-primary" onclick="btnRechazar_Click" OnClientClick="return prepareObjectR('¿Esta seguro de rechazar la solicitud?');" ToolTip="Rechaza la solicitud."/>&nbsp;&nbsp;
                <img alt="loading.." src="../shared/imgs/loader.gif" id="Img2" class="nover"  />&nbsp;
                <asp:Button ID="btsalvar" runat="server" Text="Enviar Factura" class="btn btn-primary" onclick="btsalvar_Click" ToolTip="Aprueba la solicitud." OnClientClick="return prepareObject1('');" />
           </div>
      </div>
    </form>
    </div>

    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
   <script type="text/javascript" >
       var ced_count = 0;
       var jAisv = {};
       function setObject(row) {
            self.close();
       }

        function prepareObject1(valor) {
            try {
                if (confirm("Realmente desea finalizar la solicitud de documentos?") == false) {
                <%--    this.document.getElementById('<%= HiddenField1.ClientID %>').value = '0';--%>
                    return false;
                }
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }


        function prepareObject(valor) {
            try {
                if (confirm(valor) == false) {
                    return false;
                }
//                var vals = document.getElementById('<%=fuAdjuntarFactura.ClientID %>').value;
//                if (vals == '' || vals == null || vals == undefined) {
//                    alert('* Adjunte la factura.  *');
//                    document.getElementById('<%=fuAdjuntarFactura.ClientID %>').focus();
//                    document.getElementById('<%=fuAdjuntarFactura.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400;";
//                    return false;
//                }
                var lista2 = [];
                var tbl2 = document.getElementById('tablar2');
                for (var r = 0; r < tbl2.rows.length; r++) {
                    var celColect2 = tbl2.rows[r].getElementsByTagName('td');
                    if (celColect2 != undefined && celColect2 != null && celColect2.length > 0) {
                        celColect2[13].getElementsByTagName('input')[0].disabled = false;
                    }
                }
            } catch (e) {
                alertify.alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message).set('label', 'Aceptar');
            }
        }
        function prepareObjectR(valor) {
            try {
                if (confirm(valor) == false) {
                    return false;
                }
                var lista2 = [];
                var tbl2 = document.getElementById('tablar2');
                for (var r = 0; r < tbl2.rows.length; r++) {
                    var celColect2 = tbl2.rows[r].getElementsByTagName('td');
                    if (celColect2 != undefined && celColect2 != null && celColect2.length > 0) {
                        celColect2[13].getElementsByTagName('input')[0].disabled = false;
                    }
                }
                return true;
            } catch (e) {
                alertify.alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message).set('label', 'Aceptar');
            }
        }
        function popupCallback(objeto, catalogo) {
            if (objeto == null || objeto == undefined) {
                alertify.alert('Hubo un problema al setaar un objeto de catalogo').set('label', 'Aceptar');
                return;
            }
            if (objeto.valor == "1") {
                var lista2 = [];
                var tbl2 = document.getElementById('tablar2');
                for (var r = 0; r < tbl2.rows.length; r++) {
                    var celColect2 = tbl2.rows[r].getElementsByTagName('td');
                    if (celColect2 != undefined && celColect2 != null && celColect2.length > 0) {
                        if (celColect2[16].getElementsByTagName('input')[0].value == objeto.placa) {
                            celColect2[13].getElementsByTagName('input')[0].checked = true;
                        }
                    }
                }
            }
            else {
                var lista2 = [];
                var tbl2 = document.getElementById('tablar2');
                for (var r = 0; r < tbl2.rows.length; r++) {
                    var celColect2 = tbl2.rows[r].getElementsByTagName('td');
                    if (celColect2 != undefined && celColect2 != null && celColect2.length > 0) {
                        if (celColect2[16].getElementsByTagName('input')[0].value == objeto.placa) {
                            celColect2[13].getElementsByTagName('input')[0].checked = false;
                        }
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
       function Quit()
       {

       }

        function redirectsol(val, val2, val3) {
            var caja = val;
            var caja2 = val2;
            var caja3 = val3;
            window.open('../credenciales/revisasolicitudvehiculodocumentos_new.aspx?numsolicitud=' + caja + '&idsolveh=' + caja2 + '&placa=' + caja3)
        }
   </script>
</body>
</html>

