
<%@ Page  Language="C#" AutoEventWireup="true" Title="Renovación de Permisos OPC"  CodeBehind="revisasolicitudpermisoOPC.aspx.cs" Inherits="CSLSite.revisasolicitudpermisoOPC" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Renovación de Permisos OPC</title>

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

    
    <script type="text/javascript">
         function fechas()
           {
            $(document).ready(function () {
                    //$('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'd/m/Y H:i' });
                    $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'd/m/Y' });
                });
      
                $(document).ready(function () {
                    $('.datetimepickerAlt').datetimepicker().datetimepicker({ lang: 'es', closeOnDateSelect: true, timepicker: false, step: 30, format: 'd/m/Y' });
                });
     
                $(document).ready(function () {
                    $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/Y', closeOnDateSelect: true, minDate: '0' });

                });

            }
    </script>
 

    <script type="text/javascript">
        function BindFunctions() {

                $(document).ready(function () {
                // Add minus icon for collapse element which is open by default
                $(".collapse.show").each(function () {
                    $(this).prev(".card-header").find(".fa-plus").addClass("fa-minus").removeClass("fa-plus");
                });

                // Toggle plus minus icon on show hide of collapse element
                $(".collapse").on('show.bs.collapse', function () {
                    $(this).prev(".card-header").find(".fa-plus").removeClass("fa-plus").addClass("fa-minus");
                }).on('hide.bs.collapse', function () {
                    $(this).prev(".card-header").find(".fa-minus").removeClass("fa-minus").addClass("fa-plus");
                });
            });
        }
    </script>

</head>
<body>

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Consola de Solicitudes</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Renovación de Permiso OPC</li>
          </ol>
        </nav>
      </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">
        <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>



    <form id="bookingfrm" runat="server">
    <input id="zonaid" type="hidden" value="7" />
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>


    <div class="form-row">
         <div class="form-title">
             <a class="btn btn-outline-primary mr-4" href="#" runat="server" id="aprint" clientidmode="Static" >1</a> Datos del Solicitante del Permiso 
             <%--<a class="level1" runat="server" id="a1" clientidmode="Static" >Datos del Permiso Peatonal Provisional</a>--%>
         </div>

        <div class="form-group col-md-6">
            <label for="inputAddress" Class="form-control">Empresa que solicita el permiso:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
        </div>
        <div class="form-group col-md-6">
            <asp:TextBox ID="txttipcli" runat="server" MaxLength="500" Enabled="false" style="text-align: center;"
            onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)" CssClass="form-control"></asp:TextBox>
        </div>

        <div class="form-group col-md-6" style=" display:none">
            <label for="inputAddress" Class="form-control">Area Destino:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
        </div>
        <div class="form-group col-md-6" style=" display:none">
            <asp:TextBox ID="txtarea" runat="server" MaxLength="500" Enabled="false" style="text-align: center;"
            onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)" CssClass="form-control"></asp:TextBox>
        </div>

        <div class="form-group col-md-6">
                 <label for="inputAddress" Class="form-control">Usuario que solicita el permiso:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                     </div>
                 <div class="form-group col-md-6">
                 <asp:TextBox ID="txtusuariosolper" runat="server" MaxLength="500" Enabled="false" style="text-align: center;"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)" CssClass="form-control"></asp:TextBox>
                 </div>

        <div class="form-group col-md-6" style=" display:none">
                 <label for="inputAddress" Class="form-control">Actividad permitida:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                     </div>
                 <div class="form-group col-md-6" style=" display:none">
                 <asp:TextBox ID="txtactper" runat="server" MaxLength="500" Enabled="false" style="text-align: center;"
                       onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)" CssClass="form-control"></asp:TextBox>
                 </div>

       </div>

        <%--<div class="form-row">--%>
            <div class="form-title">
                 <a class="btn btn-outline-primary mr-4" href="#" target="_blank" runat="server" id="a2" clientidmode="Static" >2</a> Personal Resgistrado en Solicitud de Renovación
                 <%--<a class="level1" runat="server" id="a3" clientidmode="Static" >Personal resgistrado en permiso provisional</a>--%>
             </div>
      <%--  </div>--%>

        <asp:UpdatePanel ID="UPdetalle" runat="server"  UpdateMode="Conditional">
            <ContentTemplate>
                    <div class="accion">
                    <div><%-- class="informativo" id="colector" style=" overflow:auto">--%>
                    <div class="alert alert-danger" id="alerta" visible="false" runat="server"></div>

                        <div class="bokindetalle" style="width:100%;overflow:auto">
                            <asp:Repeater ID="tablePagination" runat="server"  OnItemCommand="tablePagination_ItemCommand" >
                            <HeaderTemplate>
                            <table id="tablar2"  cellspacing="1" cellpadding="1" class="table table-bordered invoice" style=" font-size:small">
                                <thead>
                                <tr>
                                    <th class="nover"></th>
                                    <th class="nover"></th>
                                    <th class="nover">Estado Fotos</th>

                                    <th class="nover"></th>
                                    <th class="nover"></th>

                                    <th style=" display:none"></th>
                                    <th style=" display:none"></th>
                                    <th>CI/Pasaporte</th>
                                    <th>Nombre</th>
                                    <th class="nover">Tipo Sangre</th>
                                    <th class="nover">Dirección Domiciliaria</th>
                                    <th class="nover">Telefono</th>
                                    <th class="nover">Email</th>
                                    <th class="nover">Lugar Nacimiento</th>
                                    <th class="nover">Fecha Nacimiento</th>
                                    <th>Cargo</th>
                                    <th style=" display:none;">Area</th>
                                    <th class="nover">Colaborador Rechazado</th>
                                    <th>Comentario</th>
                                    <th class="nover"></th>
                                    <th></th>
                                    <th style=" display:none"></th>
                                </tr>
                                </thead> 
                            <tbody>
                            </HeaderTemplate>
                                <ItemTemplate>
                                        <tr class="point" >

                                        <td class="nover"> 
                                           
                                        </td>
                                        <td class="nover" ><%#Eval("ESTADO")%></td>
                                        <td class="nover" style="font-size:x-small"><%#Eval("ESTADODESC")%></td>

                                            <td class="nover"><asp:Label Text='<%#Eval("NUMSOLICITUD")%>' runat="server" id="txtNumeroSolicitudColab"/></td>
                                            <td class="nover"><asp:Label Text='<%#Eval("IDSOLCOL")%>' runat="server" id="txtNumeroSolicitudColColab"/></td>

                                            <td style=" display:none; width:1px"><%#Eval("NUMSOLICITUD")%></td>
                                            <td style=" display:none; width:1px"><%#Eval("IDSOLCOL")%></td>
                                            <td><asp:Label Text='<%#Eval("CIPAS")%>' runat="server" id="lblcipas"/></td>
                                            <td><asp:Label Text='<%#Eval("NOMBRE")%>' runat="server" id="lblNombres"/></td>
                                            <td class="nover"><%#Eval("TIPOSANGRE")%></td>
                                            <td class="nover"><%#Eval("DIRECCIONDOM")%></td>
                                            <td class="nover"><%#Eval("TELFDOM")%></td>
                                            <td class="nover"><%#Eval("EMAIL")%></td>
                                            <td class="nover" ><%#Eval("LUGARNAC")%></td>
                                            <td class="nover" ><%#Eval("FECHANAC")%></td>
                                            <td><%#Eval("CARGO")%></td>
                                            <td style=" display:none;"><%#Eval("AREA")%></td>
                                            <td class="nover"><asp:CheckBox runat="server" id="chkRevisado" Enabled="false"/></td>
                                            <td ><asp:TextBox ID="tcomentario" runat="server" Text='' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox></td>
                                            <td  class="nover">
                                            <%--<a id="adjDoc" class="btn btn-outline-primary mr-4"  onclick="redirectsol('<%# Eval("NUMSOLICITUD") %>', '<%#Eval("CIPAS")%>', '<%#Eval("IDSOLCOL")%>');">
                                                 <i class="fa fa-search"></i> Ver Documentos </a>   --%>
                                        
                                            </td>
                                            <td scope="row" title="Rechazar" > 
                                                <asp:Button runat="server" ID="btnQuitar" Height="55px" CommandName="Rechazar" Text="Rechazar" 
                                                    CommandArgument='<%# Eval("NUMSOLICITUD") + "," + Eval("IDSOLCOL")     %>'  
                                                    class="btn btn-primary"   />
                                            </td>

                                            <td style=" display:none"><asp:TextBox ID="txtcedula" runat="server" Text='<%#Eval("CIPAS")%>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox></td>
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

        <div class="form-row">
          <div class="col-md-12 d-flex justify-content-rigth" id="factura" runat="server" style=" display:none">
           <div class="alert alert-danger" id="alertafu" runat="server"></div>
            <span class="bt-bottom bt-top  bt-right bt-left" >Adjuntar factura:</span>
             <asp:FileUpload runat="server" ID="fuAdjuntarFactura" extension='.pdf' class="uploader btn btn-outline-primary mr-4" title="Adjunte el archivo en formato PDF." onchange="validaextension(this)"/>
           </div>
        </div>

        <asp:UpdatePanel ID="UPPAGADO" runat="server" UpdateMode="Conditional" >  
            <ContentTemplate>
                <script type="text/javascript">
                    Sys.Application.add_load(fechas); 
                </script>

                <div id="xfinderpagado" runat="server" visible="false">                   
                    <div class="form-title">
                       <a class="btn btn-outline-primary mr-4" href="#" target="_blank" runat="server" id="a4" clientidmode="Static" >3</a> Datos del Permiso de Acceso
                    </div>
        
                    <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
                    <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
       
                    <%--<div  runat="server" id="divseccion2">--%>
       
                        <div runat="server" id="divcabecera" >
                            <div class="form-row">
                         <%--       <div class="form-group col-md-6">
                                    <label for="inputEmail4">Número de Factura:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                                    <asp:TextBox ID="txtnumfactura" runat="server" class="form-control" MaxLength="30 "
                                            style="text-align: center" onblur="checkcaja(this,'valnumfac',true);"
                                            onkeypress="return soloLetras(event,'01234567890-',true)"></asp:TextBox>
                                    <span id="valnumfac" class="validacion"></span>
                                </div> --%>
                               
                                <div class="form-group col-md-2">
                                    <label for="inputEmail4">Fecha de Ingreso:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                                    <asp:TextBox ID="txtfecing" runat="server" MaxLength="10" CssClass="datetimepicker form-control" onkeypress="return soloLetras(event,'01234567890/')" style="text-align: center"  ClientIDMode="Static"></asp:TextBox> 
                                    <span id="valfecing" class="validacion"></span>
                                </div> 
                                <div class="form-group col-md-2">
                                    <label for="inputEmail4">Fecha de Caducidad:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                                    <asp:TextBox ID="txtfecsal" runat="server" MaxLength="10" CssClass="datetimepicker form-control" onkeypress="return soloLetras(event,'01234567890/')" style="text-align: center"  ClientIDMode="Static"></asp:TextBox> 
                                    <span id="valfecsal" class="validacion"> </span>
                                </div> 
                                <div class="form-group col-md-3">
                                    <label for="inputEmail4">Area:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                                    <asp:DropDownList runat="server" class="form-control" ID="ddlAreaOnlyControl" onchange="valdltipsol(this, valareaoc);">
                                                <asp:ListItem Value="0">* Elija *</asp:ListItem>
                                        </asp:DropDownList>
                                    <span id='valareaoc' class="validacion" ></span>
                                </div> 
                                
                                
                                <div class="form-group col-md-3" >
                                    <label for="inputEmail4">Turno:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                                    <asp:DropDownList runat="server" class="form-control" ID="ddlTurnoOnlyControl">
                                                <asp:ListItem Value="0">* Elija *</asp:ListItem>
                                        </asp:DropDownList>
                                </div> 

                                <div class="form-group col-md-2" >
                                    <label for="inputEmail4">Permiso:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                                    <asp:DropDownList runat="server" class="form-control" ID="cmbPermiso" AutoPostBack ="true" OnSelectedIndexChanged="cmbPermiso_SelectedIndexChanged">
                                                <asp:ListItem Value="0">* Elija *</asp:ListItem>
                                        </asp:DropDownList>
                                    <input id="txtIdPermiso" type="hidden" runat="server" clientidmode="Static" />
                                </div> 
                           
                                 <div class="form-group col-md-3" style="display:none">
                                    <label for="inputEmail4">Permiso:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                                    <asp:DropDownList ID="dptipoevento" runat="server" class="form-control" AutoPostBack="false" OnSelectedIndexChanged="dptipoevento_SelectedIndexChanged"
                                            onchange="valdltipsol(this, valtipeve);">
                                            <asp:ListItem Value="0">* Seleccione permiso *</asp:ListItem>
                                        </asp:DropDownList>
                                    <span id='valtipeve' class="validacion" ></span>
                                </div> 
                                <div class="form-group col-md-4" style=' display:none'>
                                    <label for="inputEmail4">Departamento:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                                    <asp:DropDownList runat="server" class="form-control" ID="ddlDepartamentoOnlyControl" onchange="valdltipsol(this, valdepoc);">
                                                <asp:ListItem Value="0">* Elija *</asp:ListItem>
                                        </asp:DropDownList>
                                    <span id='valdepoc' class="validacion" ></span>
                                </div> 

                                <div class="form-group col-md-6" style=' display:none'>
                                    <label for="inputEmail4">Cargo:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                                    <asp:DropDownList runat="server" class="form-control" ID="ddlCargoOnlyControl" onchange="valdltipsol(this, valcargoac);">
                                                <asp:ListItem Value="0">* Elija *</asp:ListItem>
                                        </asp:DropDownList>
                                    <span id='valcargoac' class="validacion" > </span>
                                </div> 
                            </div>
                        </div>

                    <%--</div>--%>
   
                    <div class="form-title">
                        4)   Observación en caso de rechazo.
                    </div>
     
                    <div class="form-row"  runat="server" id="divseccion3">
     
                        <div class="form-group col-md-12">
                            <asp:TextBox ID="txtmotivorechazo" ForeColor="Red" runat="server" class="form-control" MaxLength="500"
                                style="text-align: center"  onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox>

      
                        </div>
      
                    </div>
                
                    <%--<div id="sinresultado" runat="server" class="alert alert-warning"></div>--%>
                </div>
       
            </ContentTemplate>
        </asp:UpdatePanel>


   

    <div class="form-row">
        <div class="col-md-12 d-flex justify-content-center" id="salir" runat="server" visible="false">
            <asp:Button ID="btnSalir" runat="server" Text="Regresar" onclick="btnSalir_Click" 
            CssClass="btn btn-outline-primary mr-4" ToolTip="Regresa a la Pantalla Consultar Solicitud."/>
        </div>

            
        <div class="col-md-12 d-flex justify-content-center" id="botonera" runat="server" >
            <asp:Button Text="Rechazar" ID="btnRechazar" runat="server" onclick="btnRechazar_Click" CssClass="btn btn-primary mr-4"
                OnClientClick="return prepareObject('¿Esta seguro de Rechazar la solicitud?');" ToolTip="Rechaza la solicitud."/>
            <span>&nbsp;</span><img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
            <asp:Button ID="btsalvar" runat="server" Text="Finalizar" onclick="btsalvar_Click" CssClass="btn btn-primary mr-4"
            OnClientClick="return prepareObject('¿Esta seguro de procesar la solicitud?');" ToolTip="Crea el permiso provisional"/>
        </div>

     </div>



         


        




    </form>
    </div>

    <script type="text/javascript" >
       function setObject(row) {
            self.close();
        }
        function prepareObject1() {
            try {
                var colaborador = {};
                lista = [];
                var valida = "0";
                var tbl = document.getElementById('tablar');
                for (var f = 0; f < tbl.rows.length; f++) {
                    var celColect = tbl.rows[f].getElementsByTagName('td');
                    if (celColect != undefined && celColect != null && celColect.length > 0) {
                        var tdetalle = {
                            documento: celColect[3].getElementsByTagName('input')[0].checked,
                            comentario: celColect[4].getElementsByTagName('input')[0].value
                        };
                        this.lista.push(tdetalle);
                    }
                }                
                for (var n = 0; n < this.lista.length; n++) {
                    if (lista[n].documento == true) {
                        if (lista[n].comentario == '' || lista[n].comentario == null || lista[n].comentario == undefined) {
                            alert('* Documentos Solicitud de Credencial/Permiso Provisional: *\n * Escriba el Comentario del documento rechazado.*');
                            return false;
                        }
                        valida = "1";
                    }
                }
                colaborador = {
                    valor: valida,
                   <%-- cedula: document.getElementById('<%=hfCedula.ClientID %>').value--%>
                };
                if (window.opener != null) {
                    window.opener.popupCallback(colaborador, '');
                }
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }
      
   </script>

   

    
    <script type="text/javascript" >
        var ced_count = 0;
        var jAisv = {};
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });
        $(window).load(function () {
            $(document).ready(function () {
                //colapsar y expandir
                $("div.colapser").toggle(function () { $(this).removeClass("colapsa").addClass("expande"); $(this).next().hide(); }
                                  , function () { $(this).removeClass("expande").addClass("colapsa"); $(this).next().show(); });
            });
        });
       function setObject(row) {
            self.close();
        }
        function popupCallback(objeto, catalogo) {
            if (objeto == null || objeto == undefined) {
                alert('Hubo un problema al setaar un objeto de catalogo');
                return;
            }
            if (objeto.valor == "1") {
                var lista2 = [];
                var tbl2 = document.getElementById('tablar2');
                for (var r = 0; r < tbl2.rows.length; r++) {
                    var celColect2 = tbl2.rows[r].getElementsByTagName('td');
                    if (celColect2 != undefined && celColect2 != null && celColect2.length > 0) {
                        if (celColect2[15].getElementsByTagName('input')[0].value == objeto.cedula) {
                            celColect2[12].getElementsByTagName('input')[0].checked = true;
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
                        if (celColect2[15].getElementsByTagName('input')[0].value == objeto.cedula) {
                            celColect2[12].getElementsByTagName('input')[0].checked = false;
                        }
                    }
                }
            }
        }
        function prepareObject(valor) {
            try {
                
                if (confirm(valor) == false) {
                    return false;
                }

                //var x = document.getElementById('xfinderpagado');
                //if (x.style.visibility === 'hidden')
                //{
                //    alert('*La solicitud cuenta con estados qwue no permite continuar con la creación del permiso, favor verificar.*');
                //    return false;
                //}
                
                var vals = document.getElementById('<%=ddlTurnoOnlyControl.ClientID %>').value;
                if (vals == 0) {
                    alert('*Seleccione el Turno *');
                    document.getElementById('<%=ddlTurnoOnlyControl.ClientID %>').focus();
                    <%--document.getElementById('<%=ddlTurnoOnlyControl.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";--%>
                    //                    document.getElementById("loader").className = 'nover';
                    //var tbl2 = document.getElementById('tablar2');
                    //for (var r = 0; r < tbl2.rows.length; r++) {
                    //    var celColect2 = tbl2.rows[r].getElementsByTagName('td');
                    //    if (celColect2 != undefined && celColect2 != null && celColect2.length > 0) {
                    //        celColect2[12].getElementsByTagName('input')[0].disabled = false;
                    //   }
                   }
                   return false;
                }

                var vals = document.getElementById('<%=ddlAreaOnlyControl.ClientID %>').options[document.getElementById("<%=ddlAreaOnlyControl.ClientID%>").selectedIndex].text;
                if (vals == '* Elija *') {
                    alertify.alert('*Seleccione el Area *').set('label', 'Aceptar');
                    document.getElementById('<%=ddlAreaOnlyControl.ClientID %>').focus();
                    return false;
                }
                var vals = document.getElementById('<%=ddlTurnoOnlyControl.ClientID %>').options[document.getElementById("<%=ddlTurnoOnlyControl.ClientID%>").selectedIndex].text;
                if (vals == '* Elija *') {
                    alertify.alert('*Seleccione el Turno *').set('label', 'Aceptar');
                    document.getElementById('<%=ddlTurnoOnlyControl.ClientID %>').focus();
                    return false;
                }

                var vals = document.getElementById('<%=cmbPermiso.ClientID %>').options[document.getElementById("<%=cmbPermiso.ClientID%>").selectedIndex].text;
                if (vals == '* Elija *') {
                    alertify.alert('*Seleccione el Permiso *').set('label', 'Aceptar');
                    document.getElementById('<%=cmbPermiso.ClientID %>').focus();
                    return false;
                }

                var vals = document.getElementById('<%=txtfecing.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('*Seleccione la Fecha de Ingreso *').set('label', 'Aceptar');
                    document.getElementById('<%=txtfecing.ClientID %>').focus();
                    return false;
                }
                var vals = document.getElementById('<%=txtfecsal.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('*Seleccione la Fecha de Caducidad *').set('label', 'Aceptar');
                    document.getElementById('<%=txtfecsal.ClientID %>').focus();
                    return false;
                }

                return true;
            } catch (e) {
                //alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }
        function prepareObjectRechazar(valor) {
            try {
                if (confirm(valor) == false) {
                    return false;
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
      function Quit() {
//          event.returnValue = '[ ! ] Se perdera el trabajo realizado [ ! ]';
//          return;
      }
      function redirectsol(val, val2, val3) {
          var caja = val;
          var caja2 = val2;
          var caja3 = val3;
          window.open('../credenciales/revisasolicitudpermisoprovisionaldocumentos.aspx?numsolicitud=' + caja + '&idsolcol=' + caja3 + '&cedula=' + caja2)
        }
   </script>


  <%--  <script type="text/javascript">

          $(document).ready(function () {
                    $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'd/m/Y H:i' });
                });
      
                $(document).ready(function () {
                    $('.datetimepickerAlt').datetimepicker().datetimepicker({ lang: 'es', closeOnDateSelect: true, timepicker: false, step: 30, format: 'd/m/Y' });
                });
     
                $(document).ready(function () {
                    $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/Y', closeOnDateSelect: true, minDate: '0' });

                });

        </script>--%>

    
   <%-- <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>--%>

     <script type="text/javascript">
        $(document).ready(function () {
                $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
            });    
    </script>
</body>
   <script type="text/javascript">window.jQuery || document.write('<script src="../assets/js/vendor/jquery.slim.min.js"><\/script>')</script>
    <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
    <script type="text/javascript" src="../lib/jquery/feather.min.js"></script>
    <script type="text/javascript" src="../js/dashboard.js"></script>

</html>

