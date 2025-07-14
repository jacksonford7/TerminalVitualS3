<%@ Page  Language="C#" AutoEventWireup="true" Title="Confirmación de Pago/Creaciòn de Permiso"  CodeBehind="consultacomprobantedepagocolaborador.aspx.cs" Inherits="CSLSite.consultacomprobantedepagocolaborador" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
    <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>
    <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>

    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

    <script type="text/javascript">

        function BindFunctions1()
        {
            $(document).ready(function () {
                        $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'd/m/Y' });
            });    
        }

    </script>

    
    <script type="text/javascript">
         function fechas()
           {
            //$(document).ready(function () {
            //        //$('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'd/m/Y H:i' });
            //        $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'd/m/Y' });
            //    });
      
            //    $(document).ready(function () {
            //        $('.datetimepickerAlt').datetimepicker().datetimepicker({ lang: 'es', closeOnDateSelect: true, timepicker: false, step: 30, format: 'd/m/Y' });
            //    });
     
            //    $(document).ready(function () {
            //        $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/Y', closeOnDateSelect: true, minDate: '0' });

            //    });
             $(document).ready(function () {
                        $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'd/m/Y' });
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
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Consola de Comprobantes</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Datos Generales del Colaborador</li>
          </ol>
        </nav>
      </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">
        <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
    


        <form id="bookingfrm2" runat="server">
            <input id="zonaid" type="hidden" value="7" />
            <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>


            <div class="form-row">
                <div class="form-title">
                     <a class="btn btn-outline-primary mr-4" href="#" runat="server" id="aprint" clientidmode="Static" >1</a> Datos Generales del Colaborador(es)
                 </div>
            </div>
   

            <div  visible="false" class="alert alert-warning" id="alerta" runat="server"></div>
   

            <div class="cataresult" >
                <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <script type="text/javascript">
                            Sys.Application.add_load(fechas); 
                        </script>

                    <%--<div class="msg-alerta" id="Div1" runat="server" style=" display:none" ></div>--%>
                    <%-- <div id="xfinder" runat="server" visible="false" >--%>
                    <div class="findresult" >
     
                    <div class="informativo" style=" height:100%;">
          
                                <div class="bokindetalle" style="width:100%;overflow:auto">
                                <asp:Repeater ID="rpDetalle" runat="server" OnItemCommand="rpDetalle_ItemCommand">
                                    <HeaderTemplate>
                                        <table id="tabla"  cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                                        <thead>
                                            <tr>
                                                <th></th>
                                                <th class="nover"></th>
                                                <th>Estado</th>
                                                <th class="nover"></th>
                                                <th class="nover"></th>
                                                <th>Tipo</th>
                                                <th>Permiso</th>
                                                <th>Estado Actual</th>
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
                                                <th class="nover">Tipo</th>
                                                <%--<th>Estado Actual</th>--%>
                                                <th></th>
                                            </tr>
                                        </thead> 
                                        <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr class="point" >

                                

                                                <td scope="row" title="Ver liquidación" > 
                                                <asp:Button runat="server" ID="btnFotos" Height="55px" CommandName="Fotos" Text="Ver Fotos" 
                                                    CommandArgument='<%# Eval("NUMSOLICITUD") + "," + Eval("IDSOLCOL") + "," + Eval("ESTADO")    %>'  class="btn btn-primary"  data-toggle="modal" data-target="#myModal"  />
                                            </td>
                                            <td class="nover" ><%#Eval("ESTADO")%></td>
                                            <td style="font-size:x-small"><%#Eval("ESTADODESC")%></td>
                                            <td class="nover"><%#Eval("NUMSOLICITUD")%></td>
                                            <td class="nover"><%#Eval("IDSOLCOL")%></td>
                                            <td ><asp:Label Font-Size="X-Small"  runat="server"  class="form-control" ID="lblTipo"  Text='<%#Eval("TIPOD")%>'></asp:Label></td>
                                            <td ><%#Eval("PERMISO")%></td>
                                            <td >
                    
                                                <label class="checkbox-container">
                                                <asp:CheckBox ID="chkHuellaEstado" Text="HUELLA" TextAlign="Left" Font-Bold="true" Font-Size="X-Small"   disabled runat="server" />
                                                <span class="checkmark"></span>
                                                </label>
                                                <label class="checkbox-container">
                                                <asp:CheckBox ID="chkFotoEstado" Text="FOTO" TextAlign="Left" Font-Bold="true" Font-Size="X-Small"  disabled runat="server" />
                                                <span class="checkmark"></span>
                                                </label>
                                            </td>
                                            <td><asp:Label Font-Size ="Small"  Text='<%#Eval("CIPAS")%>' runat="server" id="lblcipas" class="form-control" /></td>
                                            <td style="font-size:x-small"><%#Eval("NOMBRE")%></td>
                                            <td><%#Eval("TIPOSANGRE")%></td>
                                            <td style="font-size:x-small"><%#Eval("DIRECCIONDOM")%></td>
                                            <td style="font-size:x-small"><%#Eval("TELFDOM")%></td>
                                            <td style="font-size:x-small"><%#Eval("EMAIL")%></td>
                                            <td style="font-size:x-small"><%#Eval("LUGARNAC")%></td>
                                            <td style="font-size:small"><%#Eval("FECHANAC")%></td>
                                            <td style="font-size:x-small"><%#Eval("CARGO")%></td>
                                            <td  style="font-size:small"><%#Eval("TIPOLICENCIA")%></td>
                                            <td style="font-size:small"><%#Eval("FECHAEXPLICENCIA")%></td>
                                            <td >
                                                <label class="checkbox-container">
                                                <asp:CheckBox runat="server" ForeColor="Red" disabled id="chkRevisado" Checked='<%#Eval("ESTADOCOL")%>'/>
                                                <span class="checkmark"></span>
                                                </label>
                                            </td>
                                            <td >
                                            <asp:TextBox ID="tcomentario" runat="server" ForeColor="Red" disabled  class="form-control"  ToolTip='<%#Eval("COMENTARIO")%>' Text='<%#Eval("COMENTARIO")%>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox>
                                            </td>
                                            <td class="nover" style="font-size:x-small"><%#Eval("TIPO")%></td>
                                            <td >
                                            <a  id="adjDoc" class="btn btn-primary"  onclick="redirectsol('<%# Eval("NUMSOLICITUD") %>', '<%# Eval("IDSOLCOL") %>', '<%#Eval("CIPAS")%>');">
                                            <i class="fa fa-search"></i> Doc</a>
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
                    <%--</div>--%>
                        <%--<div id="Div3" runat="server" class="alert alert-warning"></div>--%>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                       
                    </Triggers>
                </asp:UpdatePanel>
            </div>

            <div class="form-row">
                <div class="form-title">
                     <a class="btn btn-outline-primary mr-4" href="#" runat="server" id="a1" clientidmode="Static" >2</a> Documentación
                 </div>
            </div>
         

            <div style=" display:none">
                <table class="xcontroles" cellspacing="0" cellpadding="1">
      
                    <tr>
                        <td class="bt-bottom bt-right bt-left" >Número de Solicitud:</td>
                        <td class="bt-bottom bt-right ">
                                <asp:TextBox ID="txtsolicitud" runat="server" Width="120px" MaxLength="11"
                                style="text-align: center"
                                onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
                        </td>
                    </tr>
                    <tr style=" display:none">
                        <td class="bt-bottom bt-right bt-left">Generados desde / hasta:</td>
                        <td class="bt-bottom bt-right">
                   
                        <asp:TextBox ID="tfechaini" runat="server" Width="120px" MaxLength="10" CssClass="datetimepicker"
                            onkeypress="return soloLetras(event,'01234567890/')" style="text-align: center" 
                                onblur="valDate(this,true,valdate);" ClientIDMode="Static"></asp:TextBox>
                   
                        <asp:TextBox ID="tfechafin" runat="server" ClientIDMode="Static" Width="120px" MaxLength="15" CssClass="datetimepicker"
                            onkeypress="return soloLetras(event,'01234567890/')"  style="text-align: center" 
                                onblur="valDate(this,true,valdate);"></asp:TextBox>
                   
                        </td>
                    </tr>
                    <tr style=" display:none">
                        <td class="bt-bottom bt-right bt-left">Todas las facturas.</td>
                        <td class="bt-bottom bt-right">
                            <asp:CheckBox Text="" ID="chkTodos" runat="server" />
                        </td>
                    </tr>
                </table>
                <div class=" botonera">
                    <img alt="loading.." src="../shared/imgs/loader.gif" id="Img1" class="nover"/>
                    <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda" 
                    onclick="btbuscar_Click"/>
                </div>
            </div>
      
           
            <div runat="server" id="div2">
                <div class="form-row">
                    <div class="alert alert-warning" id="alertapagado" runat="server" ></div>
                        <div class="form-group col-md-12">            
              
                        <asp:GridView runat="server" id="gvComprobantes" class="table table-bordered invoice" AutoGenerateColumns="False" >
                        <Columns>
                        <asp:TemplateField HeaderText="# Solicitud" ItemStyle-Width="80px">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Eval("NUMSOLICITUD")%>'  Width="50px" ID="lblIdSolicitud" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tipo de Empresa" Visible="False">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Eval("TIPOEMPRESA")%>' ID="lblTipoEmpresa" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tipo de Solicitud">
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
                                <a href='<%#Eval("RUTADOCUMENTO") %>' class="btn btn-primary"  target="_blank">
                                <i class="fa fa-search"></i> Ver Documento </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        </Columns>
                        </asp:GridView>
                 
                    </div>
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
       
                        <div  runat="server" id="divseccion2">
       
                            <%--<div runat="server" id="divcabecera" >
                                <div class="form-row" runat="server" id="divnumfactura">
                                    <div class="form-group col-md-6">
                                        <label for="inputEmail4">Número de Factura:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                                        <asp:TextBox ID="txtnumfactura" ReadOnly="true" runat="server" class="form-control" MaxLength="30 "
                                                style="text-align: center" onblur="checkcaja(this,'valnumfac',true);"
                                                onkeypress="return soloLetras(event,'01234567890-',true)"></asp:TextBox>
                                        <span id="valnumfac" class="validacion"></span>
                                    </div> 
                                    <div class="form-group col-md-6" style="display:none">
                                        <label for="inputEmail4">Permiso:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                                        <asp:DropDownList ID="dptipoevento" runat="server" class="form-control" AutoPostBack="false" OnSelectedIndexChanged="dptipoevento_SelectedIndexChanged"
                                                onchange="valdltipsol(this, valtipeve);">
                                                <asp:ListItem Value="0">* Seleccione permiso *</asp:ListItem>
                                            </asp:DropDownList>
                                        <span id='valtipeve' class="validacion" ></span>
                                    </div> 
                                    <div class="form-group col-md-6">
                                        <label for="inputEmail4">Fecha de Ingreso:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                                        <asp:TextBox ID="txtfecing" runat="server" MaxLength="10" CssClass="datetimepicker form-control" onkeypress="return soloLetras(event,'01234567890/')" style="text-align: center"  ClientIDMode="Static"></asp:TextBox> 
                                        <span id="valfecing" class="validacion"></span>
                                    </div> 
                                    <div class="form-group col-md-6">
                                        <label for="inputEmail4">Fecha de Caducidad:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                                        <asp:TextBox ID="txtfecsal" runat="server" MaxLength="10" CssClass="datetimepicker form-control" onkeypress="return soloLetras(event,'01234567890/')" style="text-align: center"  ClientIDMode="Static"></asp:TextBox> 
                                        <span id="valfecsal" class="validacion"> </span>
                                    </div> 
                                    <div class="form-group col-md-6">
                                        <label for="inputEmail4">Area:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                                        <asp:DropDownList runat="server" class="form-control" ID="ddlAreaOnlyControl" onchange="valdltipsol(this, valareaoc);">
                                                    <asp:ListItem Value="0">* Elija *</asp:ListItem>
                                            </asp:DropDownList>
                                        <span id='valareaoc' class="validacion" ></span>
                                    </div> 
                                    <div class="form-group col-md-6" style=' display:none'>
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
                                    <div class="form-group col-md-6" >
                                        <label for="inputEmail4">Turno:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                                        <asp:DropDownList runat="server" class="form-control" ID="ddlTurnoOnlyControl">
                                                    <asp:ListItem Value="0">* Elija *</asp:ListItem>
                                            </asp:DropDownList>
                                    </div> 

                                    <div class="form-group col-md-6" >
                                        <label for="inputEmail4">Permiso:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                                        <asp:DropDownList runat="server" class="form-control" ID="cmbPermiso" AutoPostBack ="true" OnSelectedIndexChanged="cmbPermiso_SelectedIndexChanged">
                                                    <asp:ListItem Value="0">* Elija *</asp:ListItem>
                                            </asp:DropDownList>
                                        <input id="txtIdPermiso" type="hidden" runat="server" clientidmode="Static" />
                                    </div> 
                                </div>
                            </div>--%>

                            <div runat="server" id="divcabecera" >
                                 <div class="form-row" runat="server" id="divnumfactura">
                                    <div class="form-group col-md-2">
                                        <label for="inputEmail4">Número de Factura:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                                        <asp:TextBox ID="txtnumfactura" ReadOnly="true" runat="server" class="form-control" MaxLength="30 "
                                                style="text-align: center" onblur="checkcaja(this,'valnumfac',true);"
                                                onkeypress="return soloLetras(event,'01234567890-',true)"></asp:TextBox>
                                        <span id="valnumfac" class="validacion"></span>
                                    </div> 
                               
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
                                    <div class="form-group col-md-2">
                                        <label for="inputEmail4">Area:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                                        <asp:DropDownList runat="server" class="form-control" ID="ddlAreaOnlyControl" onchange="valdltipsol(this, valareaoc);">
                                                    <asp:ListItem Value="0">* Elija *</asp:ListItem>
                                            </asp:DropDownList>
                                        <span id='valareaoc' class="validacion" ></span>
                                    </div> 
                                
                                
                                    <div class="form-group col-md-2" >
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
                           
                                     <div class="form-group col-md-2" style="display:none">
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


                        </div>
   
                        <div class="form-title">
                           <a class="btn btn-outline-primary mr-4" href="#" target="_blank" runat="server" id="a2" clientidmode="Static" >4</a> Observación en caso de rechazo
                        </div>
     
                        <div class="form-row"  runat="server" id="divseccion3">
     
                            <div class="form-group col-md-12">
                                <asp:TextBox ID="txtmotivorechazo" ForeColor="Red" runat="server" class="form-control" MaxLength="500"
                                    style="text-align: center"  onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox>

      
                            </div>
      
                        </div>
                
                        <div id="sinresultado" runat="server" class="alert alert-warning"></div>
                    </div>
       
                </ContentTemplate>
            </asp:UpdatePanel>


        <%--<div class="form-row">--%>
            <div class="col-md-12 d-flex justify-content-center" id="salir" runat="server" visible="false">
                <asp:Button ID="btnSalir" runat="server" Text="Regresar" onclick="btnSalir_Click" 
                CssClass="btn btn-outline-primary mr-4" ToolTip="Regresa a la Pantalla Consultar Solicitud."/>
            </div>

            <div class="row" runat="server" id="botonera">
                <div class="col-md-12 d-flex justify-content-center">
                    <img alt="loading.." src="../shared/imgs/loader.gif" id="Img2" class="nover"  />&nbsp;&nbsp;
                    <br/>

                

                    <asp:Button Visible="false" Text="Rechazar" ID="btnRechazar" runat="server" class="btn btn-primary"  
                            onclick="btnRechazar_Click" OnClientClick="return prepareObjRechazo();"
                            ToolTip="Rechaza la solicitud."/>&nbsp;&nbsp;

                    <asp:Button ID="btsalvar" runat="server" Text="Crear Permiso(s)" class="btn btn-primary"  
                            OnClientClick="return prepareObject();" onclick="btsalvar_Click" 
                            ToolTip="Crea el Permiso de Acceso."/>

                     <asp:UpdatePanel ID="UPCAB" runat="server"  UpdateMode="Conditional">
                        <ContentTemplate>
                             <script type="text/javascript">
                                Sys.Application.add_load(fechas); 
                            </script>
                            <asp:Button runat="server" ID="btnCreaPermisosRF" CommandName="Permisos" Text="Omitir Registro Facial"  class="btn btn-outline-primary ml-2"    data-toggle="modal" data-target="#myModal2"  />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        <%-- </div> --%>
           

            <div id="myModal" class="modal fade" tabindex="-1" role="dialog">
                <div class="modal-dialog" role="document" style="max-width: 940px"> <!-- Este tag style controla el ancho del modal -->
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">REGISTRO FOTOGRÁFICO</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <asp:UpdatePanel ID="UPFotos" runat="server" UpdateMode="Conditional" >  
                            <ContentTemplate>
                                 
                                <div class="modal-body">
            
                                    <input id="lblidSolicitud" type="hidden" runat="server" clientidmode="Static" />
                                    <input id="lblidSolcol" type="hidden" runat="server" clientidmode="Static" />
                                    <input id="lblEstado" type="hidden" runat="server" clientidmode="Static" />
                                    <script type="text/javascript">
                                        Sys.Application.add_load(BindFunctions);
                                        Sys.Application.add_load(fechas); 
                                    </script>
          
                                    <div id="xfinderDes" runat="server" visible="false" >

                                        <!-- page start-->
                                        <div class="chat-room mt">
                                            <aside class="mid-side">
                                                <br />         
                                                <div class="catawrap" >
                                                    <div class="room-desk" id="htmlDespachos" runat="server">
                                                    </div>
                                                </div>
                                    
                                                <br />
                                                <div id="xfinderDet" runat="server" visible="false">
                                                    <div class="catawrap" >
                                                        <div class="form-row" runat="server" id="div3">

                                                            <div class="form-group col-md-12">
                                                
                                                                <div class="d-flex">
                                                                    &nbsp;
                                                                    &nbsp;
                                                                    <label for="inputEmail4">Imagen 1:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                                                                    &nbsp;
                                                                    &nbsp;

                                                                        <label>RECHAZADA &nbsp;</label>
                                                                    <div class="custom-control custom-switch">
                                                   
                                                                        <input type="checkbox" class="custom-control-input" checked="checked" id="customSwitch" runat="server"/>
                                                                        <label class="custom-control-label" for="customSwitch">AUTORIZADA</label>
                                                                    </div>
                                                                </div>
                                                            </div> 
                                                            <div class="form-group col-md-12">
                                                
                                                                <div class="d-flex">
                                                                    &nbsp;
                                                                    &nbsp;
                                                                    <label for="inputEmail4">Imagen 2:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                                                                    &nbsp;
                                                                    &nbsp;
                                                                    <label>RECHAZADA &nbsp;</label>
                                                                    <div class="custom-control custom-switch">
                                                   
                                                                        <input type="checkbox" class="custom-control-input" checked="checked" id="customSwitch2" runat="server"/>
                                                                        <label class="custom-control-label" for="customSwitch2">AUTORIZADA</label>
                                                                    </div>
                                                                </div>
                                                            </div> 

                                                            <div class="form-group col-md-12">
                                                

                                                                <div class="d-flex">
                                                                    &nbsp;
                                                                    &nbsp;
                                                                    <label for="inputEmail4">Imagen 3:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                                                                    &nbsp;
                                                                    &nbsp;
                                                                    <label>RECHAZADA &nbsp;</label>
                                                                    <div class="custom-control custom-switch">
                                                   
                                                                        <input type="checkbox" class="custom-control-input" checked="checked" id="customSwitch3" runat="server"/>
                                                                        <label class="custom-control-label" for="customSwitch3">AUTORIZADA</label>
                                                                    </div>
                                                                </div>
                                                            </div> 

                                                            <div class="form-group col-md-12">
                                                                <asp:TextBox ID="txtREchazoFotoMotivo" placeholder=" Observación en caso de rechazo." ForeColor="Red" runat="server" class="form-control" MaxLength="500"
                                                                style="text-align: center"  onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <br />
                                                </div>
                            
                                            </aside>
         
                                        </div>
                                        <!-- page end-->
                                    </div>
                                    <div id="sinresultadoDespacho" runat="server" class=" alert  alert-warning" visible="false" >
                                        No se encontraron resultados, 
                                        asegurese que exista los registros faciales.
                                    </div>
                                    <div id="msjErrores" runat="server" class=" alert  alert-warning" visible="false" >
                                        No se encontraron resultados, 
                                        asegurese que exista los registros faciales.
                                    </div>
                   
                                </div>
                                <div class="modal-footer d-flex justify-content-center">
                                    <button type="button" class="btn btn-outline-primary " data-dismiss="modal">Cancelar</button>

                                    <asp:Button runat="server" ID="btnGrabarVerificacion"  Text="Guardar"  class="btn btn-primary"  OnClick="btnGrabarVerificacion_Click"  />
                                </div>
                            </ContentTemplate>   
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

            

            <asp:UpdatePanel ID="UPMENSAJE" runat="server" UpdateMode="Conditional" >  
            <ContentTemplate>
                <script type="text/javascript">
                    Sys.Application.add_load(fechas); 
                </script>

                  <div class="modal fade" id="myModal2" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                    <div class="modal-dialog">
                      <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title" id="myModalLabels">Confirmar Acción</h4>
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                          
                        </div>
                        <div class="modal-body">
                            <br/>
                                Si usted da click en SI, se procederá a omitir el Registro Facial 
                            <br/>
                                Desea Continua?
                            <br />
                            <br />

                          <%--  <div class="form-row">
                                <div class="form-group col-md-6"> 
                                    <label for="inputAddress">Seleccione la Linea<span style="color: #FF0000; font-weight: bold;"></span></label>
                                    <div class="d-flex">
                                        <asp:DropDownList ID="cmbLineas" class="form-control" runat="server" Font-Size="Medium"  Font-Bold="true" ></asp:DropDownList>
                                    </div>
                                </div>
                            </div>--%>
                            
                            
                        </div>
                        <div class="modal-footer">
                             <asp:Button ID="btnOmitirRF" runat="server" class="btn btn-default"  Text="SI" OnClick="btnOmitirRF_Click"  UseSubmitBehavior="false" data-dismiss="modal" />
                          <button type="button" class="btn btn-primary" data-dismiss="modal">NO</button>
                        </div>
                      </div>
                    </div>
                  </div>
             </ContentTemplate>
           <%-- <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnOmitirRF" />
            </Triggers>--%>
         </asp:UpdatePanel>  

        </form>

    </div>



   <%-- <script src="../Scripts/pages.js" type="text/javascript"></script>

    <script src="../../Scripts/credenciales.js" type="text/javascript"></script>--%>


<%--    <script type="text/javascript">
       $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });
    </script>--%>

    <script type="text/javascript">
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
            lista = [];
            if (confirm('¿Esta seguro de rechazar la confirmaciòn de pago?') == false) {
                return false;
            }
            var vals = document.getElementById('<%=txtmotivorechazo.ClientID %>').value;
            if (vals == '' || vals == null || vals == undefined) {
                alertify.alert('* Escriba la observación de rechazo. *').set('label', 'Aceptar');
                document.getElementById('<%=txtmotivorechazo.ClientID %>').focus();
              <%--  document.getElementById('<%=txtmotivorechazo.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:98%;";--%>
                //document.getElementById("loader").className = 'nover';
                return false;
            }
            return true;
        }
        function prepareObject() {
            try {
                lista = [];
                if (confirm('¿Esta seguro de la confirmación de pago?') == false) {
                    return false;
                }
                var vals = document.getElementById('<%=txtsolicitud.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('*Consultar Comprobante de Pago: *\n *Escriba el numero de solicitud*').set('label', 'Aceptar');
                    document.getElementById('<%=txtsolicitud.ClientID %>').focus();
                  <%--  document.getElementById('<%=txtsolicitud.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";--%>
                    //document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtnumfactura.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('*Escriba el Número de factura *').set('label', 'Aceptar');
                    document.getElementById('<%=txtnumfactura.ClientID %>').focus();
                   <%-- document.getElementById('<%=txtnumfactura.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";--%>
                    //document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=ddlAreaOnlyControl.ClientID %>').options[document.getElementById("<%=ddlAreaOnlyControl.ClientID%>").selectedIndex].text;
                if (vals == '* Elija *') {
                    alertify.alert('*Seleccione el Area *').set('label', 'Aceptar');
                    document.getElementById('<%=ddlAreaOnlyControl.ClientID %>').focus();
                   <%-- document.getElementById('<%=ddlAreaOnlyControl.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";--%>
                    //document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=ddlTurnoOnlyControl.ClientID %>').options[document.getElementById("<%=ddlTurnoOnlyControl.ClientID%>").selectedIndex].text;
                if (vals == '* Elija *') {
                    alertify.alert('*Seleccione el Turno *').set('label', 'Aceptar');
                    document.getElementById('<%=ddlTurnoOnlyControl.ClientID %>').focus();
                   <%-- document.getElementById('<%=ddlAreaOnlyControl.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";--%>
                    //document.getElementById("loader").className = 'nover';
                    return false;
                }

                var vals = document.getElementById('<%=cmbPermiso.ClientID %>').options[document.getElementById("<%=cmbPermiso.ClientID%>").selectedIndex].text;
                if (vals == '* Elija *') {
                    alertify.alert('*Seleccione el Permiso *').set('label', 'Aceptar');
                    document.getElementById('<%=cmbPermiso.ClientID %>').focus();
                   <%-- document.getElementById('<%=ddlAreaOnlyControl.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";--%>
                    //document.getElementById("loader").className = 'nover';
                    return false;
                }


                var vals = document.getElementById('<%=txtfecing.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('*Seleccione la Fecha de Ingreso *').set('label', 'Aceptar');
                    document.getElementById('<%=txtfecing.ClientID %>').focus();
                    <%--document.getElementById('<%=txtfecing.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";--%>
                    //document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtfecsal.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('*Seleccione la Fecha de Caducidad *').set('label', 'Aceptar');
                    document.getElementById('<%=txtfecsal.ClientID %>').focus();
                  <%--  document.getElementById('<%=txtfecsal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";--%>
                    //document.getElementById("loader").className = 'nover';
                    return false;
                }
                //document.getElementById("loader").className = '';
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
            window.open('../revisasolicitudcolaboradordocumentos.aspx/?numsolicitud=' + caja + '&idsolcol=' + caja2 + '&cedula=' + caja3)
        }
    </script>



     
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
