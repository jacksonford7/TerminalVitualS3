<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="datosusuario.aspx.cs" Inherits="CSLSite.admin.datosusuario" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../img/favicon2.png" rel="icon"/>
    <link href="../img/icono.png" rel="apple-touch-icon"/>
    <link href="../css/bootstrap.min.css" rel="stylesheet"/>
    <link href="../css/dashboard.css" rel="stylesheet"/>
    <link href="../css/icons.css" rel="stylesheet"/>
    <link href="../css/style.css" rel="stylesheet"/>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

 
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function BindFunctions() {

            $(document).ready(function () {
                //inicia los fecha-hora

                //colapsar y expandir
                $("div.colapser").toggle(function () { $(this).removeClass("colapsa").addClass("expande"); $(this).next().hide(); }
                    , function () { $(this).removeClass("expande").addClass("colapsa"); $(this).next().show(); });
                //poner valor en campo

            });
        }
    </script>




</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
     <script type="text/javascript">
         Sys.Application.add_load(Calendario);
    </script>  
    <div id="div_BrowserWindowName" style="visibility:hidden;">
        <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
    </div>


    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Servicio al Cliente</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">ADMINISTRACIÓN DE USUARIOS</li>
            </ol>
        </nav>
    </div>


    <asp:UpdatePanel ID="updUsuario" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="dashboard-container p-4" id="cuerpo" runat="server">
    
                 <div class="form-title ">
                    Ingreso/Modificación de Datos de Sesión
                </div>
                <h6>Ingrese o modifique los datos de sesión del usuario.</h6>
        
                <div class="form-row">
                    <div class="form-group col-md-2">
                        <label for="inputAddress">1. Username:<span style="color: #FF0000; font-weight: bold;"></span></label>
                        <div class="d-flex">
                            <asp:TextBox ID="txtUsuario" CssClass="form-control" runat="server"  onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_',true)" MaxLength="20"  onblur="cadenareqerida(this,1,20,'valsel1');" placeholder="Usuario"  autocomplete="off"></asp:TextBox>
                            <span class="validacion" id="valsel1">* </span>
                        </div>
                    </div>

                    <div class="form-group col-md-2">
                        <label for="inputAddress">2. Contraseña:<span style="color: #FF0000; font-weight: bold;"></span></label>
                        <div class="d-flex">
                            <asp:TextBox ID="txtPassword" CssClass="form-control" runat="server"  TextMode="Password" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890',true)" MaxLength="50" onpaste="return false;" onblur="cadenareqerida(this,1,50,'valcon');" placeholder="Contraseña" autocomplete="new-password" ></asp:TextBox>
                            <span class="validacion" id="valcon">* </span>
                        </div>
                    </div>

                    <div class="form-group col-md-3">
                        <label for="inputAddress">3. Categoria de Usuario:<span style="color: #FF0000; font-weight: bold;"></span></label>
                        <div class="d-flex">
                            <asp:DropDownList  CssClass="form-control" ID="ddlTipoUsuario" runat="server" AppendDataBoundItems="true" onblur="opcionrequerida(this,0,'valCategoriaoUsuario');"></asp:DropDownList>
                            <span class="validacion" id="valCategoriaoUsuario"> * </span>
                        </div>
                    </div>

                    <div class="form-group col-md-2">
                        <label for="inputAddress">4. Estado: <span style="color: #FF0000; font-weight: bold;"></span></label>
                        <div class="d-flex">
                            <asp:DropDownList CssClass="form-control" ID="ddlEstado" runat="server" >
                                <asp:ListItem Text="ACTIVO" Value="A"></asp:ListItem>
                                <asp:ListItem Text="INACTIVO" Value="I"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="form-group col-md-3">
                        <label id="lbltipo" visible="false" runat="server" for="inputAddress">5. Tipo de Usuario:<span style="color: #FF0000; font-weight: bold;"></span></label>
                        <div class="d-flex">
                            <asp:DropDownList visible="false" CssClass="form-control" ID="cmbTipoUsuario" runat="server" AppendDataBoundItems="true" onblur="opcionrequerida(this,0,'valTipoUsuario');"></asp:DropDownList>
                            <span visible="false" class="validacion" id="valTipoUsuario"> * </span>
                        </div>
                    </div>

                </div>

            </div>

            <br />

            <section id="main-content">
                <section class="wrapper">


                    <div class="row mt">
                        <div class="col-sm-6" >
                
                                <div class="dashboard-container p-4" id="Div11" runat="server"  >
                           
                                <div class="form-title ">
                                    Ingreso/Modificación de Datos Personales 
                                </div>

                                <h6>Ingrese o modifique los datos de sesión del usuario.</h6>
        
                                    <asp:UpdatePanel ID="updCombos" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="form-row">
                                            <div class="form-group col-md-12">
                                                <label for="inputAddress">6. Nombres:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                <div class="d-flex">
                                                    <asp:TextBox class="form-control" ID="txtUsuarioNombre" runat="server"  
                                                        onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890áéíóú ',true)"
                                                            MaxLength="50"  onblur="cadenareqerida(this,1,50,'valnom');"
                                                            placeholder="NOMBRES" ></asp:TextBox>
                                                    <span class="validacion" id="valnom"> * </span>
                                                </div>
                                            </div>

                                            <div class="form-group col-md-12">
                                                <label for="inputAddress">7. Apellidos: <span style="color: #FF0000; font-weight: bold;"></span></label>
                                                <div class="d-flex">
                                                    <asp:TextBox class="form-control" ID="txtUsuarioApellido" 
                                                        runat="server" 
                                                        onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890áéíóú ',true)"
                                                            MaxLength="50"  onblur="cadenareqerida(this,1,50,'valape');"
                                                            placeholder="Apellidos"></asp:TextBox>
                                                    <span class="validacion" id="valape">* </span>
                                                </div>
                                            </div>

                                            <div class="form-group col-md-12">
                                                <label for="inputAddress">8. Identificación:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                <div class="d-flex">
                                                    <script language='javascript' type="text/javascript">

                                                        var g_identificacionID = '<%=hdfIdentificacion.ClientID%>'

                                                    </script>
                                                    <asp:HiddenField ID="hdfIdentificacion" runat="server" />
                                                    <asp:TextBox class="form-control" ID="txtUsuarioRuc" 
                                                        runat="server"  onkeypress="return soloLetras(event,'1234567890',true)"
                                                            MaxLength="15" onblur="validarIdentificacion(this,'validIden', document.getElementById(g_identificacionID));"
                                                            placeholder="IDENTIFICACION"></asp:TextBox>
                                                    <span class="validacion" id="validIden"> * </span>
                                                </div>
                                            </div>

                                            <div class="form-group col-md-12">
                                                <label for="inputAddress">9. País:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                <div class="d-flex">
                                                    <asp:DropDownList class="form-control" ID="ddlPais" runat="server"  AppendDataBoundItems="true"
                                                            OnSelectedIndexChanged="ddlPais_SelectedIndexChanged" AutoPostBack="true" onchange="refrescar_div();">
                                                        </asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="form-group col-md-12">
                                                <label for="inputAddress">10. Provincia:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                <div class="d-flex">
                                                    <asp:DropDownList class="form-control" ID="ddlProvincia" runat="server"  AppendDataBoundItems="true"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged"
                                                            onchange="refrescar_div();">
                                                        </asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="form-group col-md-12">
                                                <label for="inputAddress">11. Ciudad:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                <div class="d-flex">
                                                    <asp:DropDownList class="form-control" ID="ddlCiudad" runat="server"  AppendDataBoundItems="true"
                                                            onchange=" Sys.Application.add_load(BindFunctions);">
                                                        </asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="form-group col-md-12">
                                                <label for="inputAddress">12. Email:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                <div class="d-flex">
                                                    <script language='javascript' type="text/javascript">

                                                        var g_correoUsuarioID = '<%=hdfCorreoUsuario.ClientID%>'

                                                    </script>
                                                    <asp:HiddenField ID="hdfCorreoUsuario" runat="server" />
                                                    <asp:TextBox class="form-control" ID="txtEmail" 
                                                    runat="server"  onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_-$@.',true)"
                                                        MaxLength="50"  onblur="validarEmail(this,'valemailusu', document.getElementById(g_correoUsuarioID));"
                                                        placeholder="MAIL@MAIL.COM" ></asp:TextBox>
                                                    <span class="validacion" id="valemailusu"> * </span>
                                                </div>
                                            </div>



                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlPais" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlProvincia" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>

                        </div> 

            
                        <div class="col-sm-6">
                   
                                <div class="dashboard-container p-4" id="Div12" runat="server">
                  
                                    <div class="form-title ">
                                        Ingreso/Modificación de Datos de Empresa
                                    </div>

                                    <h6>Ingrese o modifique los datos de sesión del usuario.</h6>
                                       
                                    <asp:UpdatePanel ID="updDatosEmpresa" runat="server">
                                        <ContentTemplate>
                                            <div class="form-row">
                                                <div class="form-group col-md-12">
                                                    <label for="inputAddress">13. Empresa:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                    <div class="d-flex">
                                                        <span id="empresa" class="form-control" >...</span>
                                                        <div id="td_buscar" runat="server">
                                                            <a  class="btn btn-outline-primary mr-4"  target="popup" onclick="window.open('../catalogo/empresas.aspx','name','width=850,height=880')">
                                                            <span class='fa fa-search' style='font-size:24px'></span>  </a>
                                                        </div>
                                                    </div>
                                                </div>


                                                <div class="form-group col-md-12">
                                                    <label for="inputAddress">14. Identificación de la Empresa:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                    <div class="d-flex">
                                                        <asp:TextBox ID="txtEmpresaIdentificacion" CssClass="form-control" runat="server" onkeydown="return false;"></asp:TextBox>
                                                        <span class="validacion" id="valempid">*</span>
                                                    </div>
                                                </div>
                                                <div class="form-group col-md-12">
                                                    <label for="inputAddress">15. Nombre de la Empresa:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                    <div class="d-flex">
                                                        <asp:TextBox ID="txtEmpresaNombre"  CssClass="form-control" runat="server" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890 .',true)" MaxLength="255" placeholder="Nombre"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group col-md-12">
                                                    <label for="inputAddress">16. Dirección de la Empresa:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                    <div class="d-flex">
                                                        <asp:TextBox  CssClass="form-control" ID="txtEmpresaDireccion" runat="server"  onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890áéíóú -/#.',true)" MaxLength="255"  placeholder="Direccion"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group col-md-12">
                                                    <label for="inputAddress">17. Teléfono de la Empresa:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                    <div class="d-flex">
                                                        <asp:TextBox  CssClass="form-control" ID="txtEmpresaTelefono" runat="server"  onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890-/ ',true)" MaxLength="20"  placeholder="Telefono"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <table>
                                                    <tr class="nover" >
                                                        <td class="bt-bottom  bt-right bt-left">
                                                            00. Fax de la Empresa:
                                                        </td>
                                                        <td class="bt-bottom bt-right" colspan="3">
                                                            <asp:TextBox  CssClass="form-control" ID="txtEmpresaFax" runat="server"  onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890-/ ',true)"
                                                                MaxLength="20" placeholder="FAX" Enabled="False">000</asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>

                                                <div class="form-group col-md-12">
                                                    <label for="inputAddress">18. Correo E-Billing:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                    <div class="d-flex">
                                                        <asp:TextBox  CssClass="form-control"  ID="txtEmpresaWebSite" runat="server"  onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890- ./@_',true)" MaxLength="50"  placeholder="WEBSITE"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group col-md-12">
                                                    <label for="inputAddress">19. Correo SNA:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                    <%--<div class="d-flex">--%>
                                                        <div id='Div1' class="d-flex">
                                                            <%--<div id="Div2" >--%>
                                                                <script language='javascript' type="text/javascript">

                                                                    var g_correoEmpresaID = '<%=hdfCorreoEmpresa.ClientID%>'
                                                                    $("txtEmpresaIdentificacion").keydown(false);
                                                                </script>
                                                                <asp:HiddenField ID="hdfCorreoEmpresa" runat="server" />
                                                                <asp:TextBox ID='txtEmpresaCorreo' runat="server"   CssClass="form-control" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890- _$@.',true)" placeholder="mail@mail.com" MaxLength="50" />
                                                            <%--</div>--%>
                                                        </div>
                                                    <%--</div>--%>
                                                </div>
                                            </div>

                                <%--            <div class="botonera">
                                                <asp:UpdateProgress AssociatedUpdatePanelID="updUsuario" ID="updateProgress" runat="server">
                                                    <ProgressTemplate>
                                                        <div id="progressBackgroundFilter">
                                                        </div>
                                                        <div id="processMessage">
                                                            Estamos procesando la tarea que solicitó, por favor espere...
                                                            <img alt="Loading" src="../shared/imgs/loader.gif" style="margin: 0 auto;" />
                                                        </div>
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" />
                                                &nbsp;
                                                <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" OnClick="btnLimpiar_Click" />
                                                &nbsp;
                                                <asp:Button ID="btnRegresar" runat="server" Text="Regresar" OnClick="btnRegresar_Click" />
                                            </div>--%>

                                          

                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                     
                                </div>
                   
                        </div>
                    </div>

                    <br />
                    <div class="alert alert-warning" id="alerta" runat="server"></div>
                    <div id="error" runat="server" class="alert alert-danger" visible="false"></div>
                     
                    <div class="row">

                        <asp:UpdateProgress AssociatedUpdatePanelID="updUsuario" ID="updateProgress" runat="server">
                            <ProgressTemplate>
                                <div id="progressBackgroundFilter">
                                </div>
                                <div id="processMessage">
                                    Estamos procesando la tarea que solicitó, por favor espere...
                                    <img alt="Loading" src="../shared/imgs/loader.gif" style="margin: 0 auto;" />
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>

                        <div class="col-md-12 d-flex justify-content-center">
                         
                                <asp:Button ID="btnLimpiar" class="btn btn-outline-primary mr-4" runat="server" Text="Limpiar" OnClick="btnLimpiar_Click" />
                                    <asp:Button ID="btnRegresar" 
                                        class="btn btn-outline-primary mr-4" runat="server" Text="Regresar" OnClick="btnRegresar_Click" />
                            <asp:Button ID="btnGuardar" class="btn btn-primary"  runat="server" Text="Guardar" OnClick="btnGuardar_Click" />
                        </div>
                    </div>

                     <br />                                   

                   <%-- <asp:UpdatePanel ID="UPBOTONES" runat="server" UpdateMode="Conditional" >  
                        <ContentTemplate>
                
                            <div class="form-group">
                                    <div class="alert alert-warning" id="banmsg_det" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el número de la carga MRN......</div>
                            </div>
                  
                        </ContentTemplate>
                    </asp:UpdatePanel>   --%>
	    


           
                </section>
            </section>
        </ContentTemplate>
    </asp:UpdatePanel>
  
  <script type="text/javascript" src="../lib/pages.js" ></script>
 
<script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/pages_mod.js" type="text/javascript"></script>
    <script type="text/javascript">
        var ced_count = 0;
        var jAisv = {};
        $(window).load(function () {
            //objeto a transportar.
            $(document).ready(function () {
                //inicia los fecha-hora


            });
        });
        //Esta funcion va a validar que cuando presionen booking debe poner los 3 parametros
        function validateBook(objeto) {

        }

        function refrescar_div() {
            Sys.Application.add_load(BindFunctions);
        }

        //Imprimir.......................
        function imprimir() {

            //Si es contenedor validar cedula

        }

        //esta futura funcion va a preparar el objeto a transportar.
        function prepareObject() {






        }
        function popupCallback(data, control) {
            this.document.getElementById(control).value = data;
        }

        function btclear_onclick() {

        }

        function setDataEmpresa(nombre, identificacion, direccion, fax, telefono, correo, website) {
            $("#<%=txtEmpresaIdentificacion.ClientID%>").val(identificacion); //set value
            $("#<%=txtEmpresaNombre.ClientID%>").val(nombre); //set value
            $("#<%=txtEmpresaDireccion.ClientID%>").val(direccion); //set value
            $("#<%=txtEmpresaTelefono.ClientID%>").val(telefono); //set value
            $("#<%=txtEmpresaFax.ClientID%>").val(fax); //set value
            $("#<%=txtEmpresaWebSite.ClientID%>").val(website); //set value
            $("#<%=txtEmpresaCorreo.ClientID%>").val(correo); //set value

        }

    </script>




</asp:Content>
