<%@ Page Language="C#" AutoEventWireup="true" Title="Emisión/Renovación de Credencial"
CodeBehind="revisasolicitudcolaborador.aspx.cs" Inherits="CSLSite.revisasolicitudcolaborador" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head title="" runat="server">
     <title>Revisa Solicitud Colaborador</title>
    <link href="../img/favicon2.png" rel="icon"/>
    <link href="../img/icono.png" rel="apple-touch-icon"/>
    <link href="../css/bootstrap.min.css" rel="stylesheet"/>
    <link href="../css/dashboard.css" rel="stylesheet"/>
    <link href="../css/icons.css" rel="stylesheet"/>
    <link href="../css/style.css" rel="stylesheet"/>
<%--  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>--%>

     <link href="../shared/estilo/modal.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />

     <!--formatos de tabla y controles de tabla con pagineo css-->
    <link href="../lib/advanced-datatable/css/demo_page.css" rel="stylesheet" />
    <link href="../lib/advanced-datatable/css/demo_table.css" rel="stylesheet" />
    <link rel="stylesheet" href="../lib/advanced-datatable/css/DT_bootstrap.css" />
   
    <!--external css-->
    <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="../lib/gritter/css/jquery.gritter.css" />

    <!--mensajes-->
<%--  <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>--%>
     <link href="../css/datatables.min.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css"/>

    <script type="text/javascript">
        function BindFunctions() {
            document.getElementById('imagen').innerHTML = '';
            $(document).ready(function () {
                $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 5: { sorter: false }, 6: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
            });
        }
    </script>

    <%-- <style type="text/css">
        .warning { background-color:Yellow;  color:Red;}
        .panel-reveal-modal-bg { background: #000; background: rgba(0,0,0,.8);cursor:progress;	}
    </style>

     <style type="text/css">
        body2
        {
            font-family: Arial;
            font-size: 10pt;
        }
        .modal
        {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }

        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
        }
        .modalPopup
        {
            background-color: #FFFFFF;
            width: 500px;
            border: 3px solid #FF3720;
            padding: 0;
        }
        .modalPopup .header
        {
            background-color: #2FBDF1;
            height: 30px;
            color: White;
            line-height: 30px;
            text-align: center;
            font-weight: bold;
        }
        .modalPopup .body
        {
            min-height: 50px;
            line-height: 30px;
            text-align: center;
            font-weight: bold;
            margin-bottom: 5px;
        }
    </style>--%>
</head>
<body>

     <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Consola de Comprobantes</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Revisión Emisión/Renovación de Credencial</li>
            </ol>
        </nav>
    </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">
        <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>

       

   
        <form id="bookingfrm" runat="server">
            <input id="zonaid" type="hidden" value="7" />
            <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
            <asp:HiddenField ID="manualHide" runat="server" />

            <div id="div_BrowserWindowName" style="visibility:hidden;">
                <asp:HiddenField ID="HiddenField1" runat="server" />
            </div>

            <div class="form-row">
                <div class="form-title col-md-12">
                    <a class="btn btn-outline-primary mr-4" href="#" target="_blank" runat="server" id="aprint" clientidmode="Static" >1</a>
                    <a class="level1" target="_blank" runat="server" id="a1" clientidmode="Static" >Tipo de Credencial</a>
                </div>

                <div class="form-group col-md-4">
                    <label for="inputAddress">Transacción :<span style="color: #FF0000; font-weight: bold;"></span></label>
                    <div class="d-flex">
                        <asp:TextBox ID="txttipcli"  runat="server" class="form-control"   placeholder=""  Font-Bold="false" disabled></asp:TextBox>
                    </div>
                </div>

                <div class="form-group col-md-3"> 
                    <label for="inputAddress">Norma :<span style="color: #FF0000; font-weight: bold;"></span></label>
                    <div class="d-flex">
                        <asp:DropDownList ID="cmbNorma" class="form-control" runat="server" Font-Size="Medium" AutoPostBack="true" Font-Bold="true" OnSelectedIndexChanged="cmbNorma_SelectedIndexChanged" > 
                        </asp:DropDownList>
                        <a class="tooltip" ><span class="classic" >Producto que se espera</span><img alt='' src='../shared/imgs/info.gif' class='datainfo'/></a>
                    </div>
                </div>

                <div class="form-group col-md-3"> 
                    <label for="inputAddress">Tipo :<span style="color: #FF0000; font-weight: bold;"></span></label>
                    <div class="d-flex">
                        <asp:DropDownList ID="cmbTipo" class="form-control" runat="server" Font-Size="Medium" AutoPostBack="true" Font-Bold="true" OnSelectedIndexChanged="cmbTipo_SelectedIndexChanged">
                        </asp:DropDownList>
                        <a class="tooltip" ><span class="classic" >Producto que se espera</span><img alt='' src='../shared/imgs/info.gif' class='datainfo'/></a>
                    </div>
                </div>
            </div>                 

            <div class="form-row">
                <div class="form-title col-md-12">
                    <a class="btn btn-outline-primary mr-4" href="#" target="_blank" runat="server" id="a2" clientidmode="Static" >2</a>
                    <a class="level1" target="_blank" runat="server" id="a3" clientidmode="Static" >Datos Generales del Colaborador(es)</a>
                </div>
            </div>

            <asp:UpdatePanel ID="UPdetalle" runat="server"  UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="cataresult" >



                        <div class="findresult" >
          
                            <div class="bokindetalle" style="width:100%;overflow:auto">
                                <div class="alert alert-warning" id="alerta" runat="server" ></div>
                                <asp:Repeater ID="tablePagination" runat="server" OnItemCommand="tablePagination_ItemCommand"  >
                                    <HeaderTemplate>
                                        <table id="tablar2"  cellspacing="1" cellpadding="1" class="table table-bordered invoice" style=" font-size:small;">
                                            <thead>
                                                <tr>
                                                <th style=" display:none"></th>
                                                <th style=" display:none"></th>
                                            
                                                <th>CI/Pasaporte</th>
                                                <th>Nombre</th>
                                                <th>Tipo Sangre</th>
                                                <th>Dirección Domiciliaria</th>
                                                <th>Telefono</th>
                                                <th>Email</th>
                                                <th>Lugar Nacimiento</th>
                                                <th>Fecha Nacimiento</th>
                                                <th>Cargo</th>
                                                <th>Licencia</th>
                                                <th>Fecha Exp. Licencia</th>
                                                <th>Colaborador Rechazado</th>
                                                <th>Comentario</th>
                                                <th>Tipo</th>
                                                <th>Permiso</th>
                                                                <th  style=" display:none; "></th>
                                                <th>Estado Actual</th>
                                                <th></th>
                                                    <th class="nover"></th>
                                                    <th class="nover"></th>
                                                    <th></th>
                                                </tr>
                                            </thead> 
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr class="point" >
                                            <td style=" display:none;"><%#Eval("NUMSOLICITUD")%></td>
                                            <td style=" display:none;"><%#Eval("IDSOLCOL")%></td>

                                            <td>
                                            <asp:Label Text='<%#Eval("CIPAS")%>' runat="server" id="lblcipas"/>
                                            </td>
                                            <td><%#Eval("NOMBRE")%></td>
                                            <td><%#Eval("TIPOSANGRE")%></td>
                                            <td><%#Eval("DIRECCIONDOM")%></td>
                                            <td><%#Eval("TELFDOM")%></td>
                                            <td><%#Eval("EMAIL")%></td>
                                            <td ><%#Eval("LUGARNAC")%></td>
                                            <td ><%#Eval("FECHANAC")%></td>
                                            <td><%#Eval("CARGO")%></td>
                                            <td><%#Eval("TIPOLICENCIA")%></td>
                                            <td ><%#Eval("FECHAEXPLICENCIA")%></td>
                                            <td><asp:CheckBox runat="server" Checked='<%#Eval("ESTADOCOL")%>'  Enabled="false" ForeColor="Red" id="chkRevisado"/></td>
                                            <td ><asp:TextBox ID="tcomentario" runat="server" ForeColor="Red" ToolTip='<%#Eval("COMENTARIO")%>' Text='<%#Eval("COMENTARIO")%>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox></td>
                                                            <td ><asp:Label runat="server" ID="lblTipo" Text='<%#Eval("TIPOD")%>'></asp:Label></td>
                                            <td ><%#Eval("PERMISO")%></td>
                                            <td  style=" display:none; "><asp:TextBox ID="txtCiPas" runat="server" Text='<%#Eval("CIPAS")%>'></asp:TextBox></td>
                                            <td  >
                                                <table ><td>
                                                <asp:CheckBox ID="chkHuellaEstado" Text="HUELLA" TextAlign="Left" Font-Bold="true" Font-Size="X-Small" Enabled="false" runat="server" />
                                                </td>
                                                <td>
                                                <asp:CheckBox ID="chkFotoEstado" Text="FOTO" TextAlign="Left" Font-Bold="true" Font-Size="X-Small"  Enabled="false" runat="server" />
                                                </td>
                                                </table>
                                            </td>
                                            <td  >
                                                <a  id="adjDoc" class="btn btn-outline-primary mr-md-n1"  onclick="redirectsol('<%# Eval("NUMSOLICITUD") %>', '<%# Eval("IDSOLCOL") %>', '<%#Eval("CIPAS")%>');">
                                                <i class="fa fa-search"></i>  Docs </a>
                                            </td>

                                            <td class="nover"><asp:Label Text='<%#Eval("NUMSOLICITUD")%>' runat="server" id="txtNumeroSolicitudColab"/></td>
                                            <td class="nover"><asp:Label Text='<%#Eval("IDSOLCOL")%>' runat="server" id="txtNumeroSolicitudColColab"/></td>
                                            
                                            <td scope="row" title="Rechazar" > 
                                                <asp:Button runat="server" ID="btnQuitar" Height="55px" CommandName="Rechazar" Text="Rechazar" 
                                                    CommandArgument='<%# Eval("NUMSOLICITUD") + "," + Eval("IDSOLCOL")     %>'  
                                                    class="btn btn-primary"   />
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
                </ContentTemplate>
            </asp:UpdatePanel>
                    
            <br />
                  
            <div class="row" >
      

                <div id="factura" runat="server">
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <div class="alert alert-danger col-md-12" id="alertafu" runat="server"  style=" font-weight:bold"></div>
          
                                <span class="bt-bottom bt-top  bt-right bt-left" >Adjuntar factura:</span> 
                                <asp:FileUpload runat="server" ID="fuAdjuntarFactura" CssClass="btn btn-outline-primary mr-4" Width="100%" extension='.pdf' class="uploader" title="Adjunte el archivo en formato PDF." onchange="validaextension(this)"/>
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
          
                <div class="col-md-12 d-flex justify-content-center" runat="server" id="salir" visible="false">
                    <asp:Button ID="btnSalir" runat="server" Text="Regresar" onclick="btnSalir_Click" 
                            ToolTip="Regresa a la Pantalla Consultar Solicitud." class="btn btn-outline-primary"/>
                </div>

                    <%--OnClientClick="return prepareObject('¿Esta seguro de rechazar la solicitud?');"      OnClientClick="return prepareObject('¿Esta seguro de procesar la solicitud?');" --%>

<%--                   <asp:UpdatePanel ID="uptabla" runat="server" ChildrenAsTriggers="true" >
                        <ContentTemplate>

                       </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger  ControlID="btsalvar"/>
                    </Triggers>
                </asp:UpdatePanel>--%>

                <div class="col-md-12 d-flex justify-content-center" id="botonera" runat="server">
                    <asp:Button ID="btnRechazar" runat="server" Text="Rechazar" onclick="btnRechazar_Click" ToolTip="Rechaza la solicitud." class="btn btn-primary"/>
                    <span>&nbsp;&nbsp;</span>
                    <asp:Button ID="btsalvar" runat="server" Text="Generar Factura" onclick="btsalvar_Click" ToolTip="Aprueba la solicitud." class="btn btn-primary" OnClientClick="return prepareObject1('');"  />
                    <%--<asp:Button runat="server" ID="btsalvar" ToolTip="Aprueba la solicitud."   Text="Enviar Solicitud..."  class="btn btn-primary"  data-toggle="modal" data-target="#myModal2"  />                                                                                                                           --%>
                </div>

            </div>

                            <%--</ContentTemplate>
                    <Triggers>
                        <!--<asp:AsyncPostBackTrigger ControlID="btnBuscar" />-->
                    </Triggers>
            </asp:UpdatePanel>--%>

<%--                <asp:updateprogress associatedupdatepanelid="uptabla"
                    id="updateProgress" runat="server">
                    <progresstemplate>
                        <div id="progressBackgroundFilter"></div>
                        <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor 
                            espere...<br />
                             <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
                        </div>
                    </progresstemplate>
                </asp:updateprogress>
    
                <asp:ModalPopupExtender ID="mpeLoading" runat="server" BehaviorID="idmpeLoading" PopupControlID="pnlLoading" TargetControlID="btnLoading" EnableViewState="false" DropShadow="true" BackgroundCssClass="modalBackground" />

                <asp:Panel ID="pnlLoading" runat="server"  HorizontalAlign="Center" CssClass="modalPopup" align="center"  EnableViewState="false" Style="display: none">
                    <br />Procesando información....
                    <div class="body2">
                        <asp:UpdatePanel ID="msgload" runat="server" UpdateMode="Conditional"  >
                            <ContentTemplate>
                                <div align="center">   
                                    <asp:Image ID="loading" runat="server" ImageUrl="../lib/file-uploader/img/loading.gif"  Visible="true" Width="40" Height="40" />
                                </div>
                  
                                <br/>
                                    Estimado Cliente, se está procesando su solicitud....<br/>Por favor espere.... 
                                <br />
                                <br />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </asp:Panel>
                <asp:Button ID="btnLoading" runat="server" Style="display: none" />--%>
   
            </form>
   
    </div>

    
<%--    <script type="text/javascript">

    var mpeLoading;
    function initializeRequest(sender, args){
        mpeLoading = $find('idmpeLoading');
        mpeLoading.show();
        mpeLoading._backgroundElement.style.zIndex += 10;
        mpeLoading._foregroundElement.style.zIndex += 10;
    }
        function endRequest(sender, args) {
             $find('idmpeLoading').hide();

        }

    Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(initializeRequest);
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest); 

    if (typeof(Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();
        $find('idmpeLoading').hide();

    </script>--%>



    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
    <script type="text/javascript" >
       var ced_count = 0;
       var jAisv = {};
       function setObject() {
        }
        function prepareObject1(valor) {
            try {
                if (confirm(this.document.getElementById('<%= HiddenField1.ClientID %>').value) == false) {
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
                var vals = document.getElementById('<%=fuAdjuntarFactura.ClientID %>').value;
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
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
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
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }
      function initFinder() {
          if (document.getElementById('txtname').value.trim().length <= 0) {
              alert('Por favor escriba una o varias \nletras del número');
              return false;
          }
          document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif">';
      }
      function redirectsol(val, val2, val3) {
          var caja = val;
          var caja2 = val2;
          var caja3 = val3;
          window.open('../credenciales/revisasolicitudcolaboradordocumentos.aspx?numsolicitud=' + caja + '&idsolcol=' + caja2 + '&cedula=' + caja3)
      }
      function popupCallback(objeto, catalogo) {
          if (objeto == null || objeto == undefined) {
              alert('Hubo un problema al setaar un objeto de catalogo');
              return;
          }
          if (objeto.valor == '1') {
              var lista2 = [];
              var tbl2 = document.getElementById('tablar2');
              for (var r = 0; r < tbl2.rows.length; r++) {
                  var celColect2 = tbl2.rows[r].getElementsByTagName('td');
                  if (celColect2 != undefined && celColect2 != null && celColect2.length > 0) {
                      if (celColect2[17].getElementsByTagName('input')[0].value == objeto.cedula) {
                          celColect2[13].getElementsByTagName('input')[0].checked = true;
                          //UPdetalle.Update();
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
                      if (celColect2[17].getElementsByTagName('input')[0].value == objeto.cedula) {
                          celColect2[13].getElementsByTagName('input')[0].checked = false;
                      }
                  }
              }
          }
      }
    </script>
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>


</body>
</html>
