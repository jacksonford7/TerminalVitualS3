<%@ Page Language="C#" MasterPageFile="~/site.Master" Title="Emisión Pase Provisional" MaintainScrollPositionOnPostback="true"
         AutoEventWireup="true" CodeBehind="solicitudcolaboradorprovisional.aspx.cs" Inherits="CSLSite.cliente.solicitudcolaboradorprovisional" %>
         <%@ MasterType VirtualPath="~/site.Master" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

    <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

     <%-- <link href="../shared/estilo/Reset.css" rel="stylesheet" />
 
     <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
     <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />--%>

    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
   <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

    <!--external css-->
  <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
  <link rel="stylesheet" type="text/css" href="../lib/gritter/css/jquery.gritter.css" />

    <!--mensajes-->
  <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>


    <%--<link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
<style type="text/css">
      .warning { background-color:Yellow;  color:Red;}
      #progressBackgroundFilter {
        position:fixed;
        bottom:0px;
        right:0px;
        overflow:hidden;
        z-index:1000;
        top: 0;
        left: 0;
        background-color: #CCC;
        opacity: 0.8;
        filter: alpha(opacity=80);
        text-align:center;
    }
    #processMessage 
    {
        text-align:center;
        position:fixed;
        top:30%;
        left:43%;
        z-index:1001;
        border: 5px solid #67CFF5;
        width: 200px;
        height: 100px;
        background-color: White;
        padding:0;
    }
     #aprint {
 	         color: #666;    
	         border: 1px solid #ccc;    
	         -moz-border-radius: 3px;    
	         -webkit-border-radius: 3px;    
	         background-color: #f6f6f6;    
	         padding: 0.3125em 1em;    
	         cursor: pointer;   
	         white-space: nowrap;   
	         overflow: visible;   
	         font-size: 1em;    
	         outline: 0 none /* removes focus outline in IE */;    
	         margin: 0px;    
	         line-height: 1.6em;    
	         background-image: url(../shared/imgs/action_print.gif);
	         background-repeat: no-repeat;
	         background-position:left center;
	         text-decoration:none;
	         padding:5px 2px 5px 30px;
	  
    }
    * input[type=text]
    {
        text-align:left!important;
        }
    .style1
    {
        width: 250px;
        height: 29px;
    }
    .style2
    {
        border-bottom: 1px solid #CCC;
        height: 29px;
    }
    </style>--%>
</asp:Content>
<asp:Content ID="formcolaborador" ContentPlaceHolderID="placebody" runat="server">
    <input id="zonaid" type="hidden" value="7" />
  <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>

  <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
   <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">MCA - Módulo de Control de Acceso</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Emisión Pase Provisional</li>
          </ol>
        </nav>
      </div>

 <div class="dashboard-container p-4" id="cuerpo" runat="server">

         <div class="form-row">
            <div class="form-group col-md-12">
             <a class="btn btn-outline-primary mr-4" href="#" runat="server" id="aprint" clientidmode="Static" >1</a>
             <a class="level1" target="_blank" runat="server" id="a1" clientidmode="Static" >Datos Generales del Permiso</a>
         </div>

            <div class="form-group col-md-6">
                 <label for="inputAddress">Fecha de Ingreso:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
            </div>
            <div class="form-group col-md-6 d-flex">
            <asp:TextBox ID="txtfecing" runat="server" MaxLength="10" CssClass="datetimepicker form-control"
            onkeypress="return soloLetras(event,'01234567890/')" style="text-align: center" onblur="checkcaja(this,'valfecing',true);" ClientIDMode="Static"></asp:TextBox> 
                <span id="valfecing" class="validacion"> </span>
            </div>


            <div class="form-group col-md-6">
                <label for="inputAddress">Fecha de Caducidad:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
            </div>
            <div class="form-group col-md-6 d-flex">
                <asp:TextBox ID="txtfecsal" runat="server" ClientIDMode="Static" MaxLength="15" CssClass="datetimepicker form-control"
                    onkeypress="return soloLetras(event,'01234567890/')" style="text-align: center" onblur="checkcaja(this,'valfecsal',true);"></asp:TextBox>
                <span id="valfecsal" class="validacion"> </span>
            </div>
            

             <div class="form-group col-md-6">
                 <label for="inputAddress">Area:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                 </div>
                 <div class="form-group col-md-6">
                          <asp:DropDownList runat="server" ID="ddlAreaOnlyControl" class="form-control" onchange="validadropdownlist(this, valareaoc);">
                                    <asp:ListItem Value="0">* Elija *</asp:ListItem>
                          </asp:DropDownList>
                     <span id="valareaoc" class="validacion"> </span>
            </div>

             <div class="form-group col-md-6">
                 <label for="inputAddress">Actividad Permitida:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                 </div>
                 <div class="form-group col-md-6">
                          <asp:DropDownList runat="server" ID="ddlActividadOnlyControl" onchange="validadropdownlist(this, valactividad);" class="form-control">
                                    <asp:ListItem Value="0">* Elija *</asp:ListItem>
                          </asp:DropDownList>
                     <span id="valactividad" class="validacion"> </span>
            </div>
     </div>
 
     <div cellspacing="0" cellpadding="1"  style=" display:none">
     <asp:DropDownList ID="ddlTipoEmpresa" runat="server" onchange="valdltipsol(this, valtipempresa);">
             <asp:ListItem Value="0">* Seleccione origen *</asp:ListItem>
     </asp:DropDownList>
     <span id='valtipempresa' class="validacion" > * obligatorio</span>
     </div>

   <div class="seccion"  style=" display:none">

        <div class=" msg-critico" runat="server" id="infotipcre" style=" font-weight:bold">
        </div>

     <div>
     <asp:DropDownList ID="cbltiposolicitud" runat="server" onchange="valdltipsol(this, valtipsol);">
             <asp:ListItem Value="0">* Seleccione origen *</asp:ListItem>
     </asp:DropDownList>
     <span id='valtipsol' class="validacion" > * obligatorio</span>
     </div>

    </div>

   <div class="seccion" style=" display:none">
      <div class="informativo">
      <table class="controles" cellspacing="0" cellpadding="1">
      <tr><td rowspan="2" class="inum"> <div class="number">0</div></td><td class="level1" >
          Usuario que solicita el permiso (Representante Legal).</td></tr>
      <tr><td class="level2">Confirme que los datos sean los correctos.</td></tr>
      </table>
     </div>

     <div class="accion">
     <div class="controles" cellspacing="0" cellpadding="1">

         <label class="bt-bottom bt-right bt-left" style=" width:155px">Nombres y Apellidos:</label>
           <asp:TextBox ID="txtusuariosolicita" runat="server" MaxLength="500"
             style="text-align: center;text-transform :uppercase" onblur="checkcajalarge(this,'valusuariosolicita',true);"
             
                 onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>
            <span id="valusuariosolicita" class="validacion"> * obligatorio</span>

         <label class="bt-bottom  bt-right bt-left" style=" width:155px">Cédula:</label>
            <asp:TextBox ID="txtci" runat="server" MaxLength="10"
             style="text-align: center"
             onBlur="validalacedula(this,'valci',true);"
             onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
         <span id="valci" class="validacion"> * obligatorio</span>

     </div>
    </div>
    </div>

     <div class="form-row">
        <div class="form-group col-md-12">
             <a class="btn btn-outline-primary mr-4" href="#" target="_blank" runat="server" id="a2" clientidmode="Static" >2</a>
             <a class="level1" target="_blank" runat="server" id="a3" clientidmode="Static" >Datos Generales del Colaborador</a>
         </div>

         <div class="form-title col-md-12 d-flex">
                     <label for="inputEmail4">Criterios de consulta para Colaboradores:</label>
                        </div> 

                     <div class="form-group col-md-6 d-flex">
                         <label class="radiobutton-container" >
                               C.I./Pasaporte<input id="rbcedula" runat="server"  checked="true" type="radio" name="deck" value="ced" clientidmode="Static"/>
                                <span class="checkmark"></span></label>
                        <label for="inputEmail4">&nbsp;&nbsp;&nbsp;&nbsp;</label>
                         <label class="radiobutton-container" > 
                               Nombre(s)<input id="rbnombres" runat="server" type="radio" name="deck" value="nom" clientidmode="Static"/>
                                <span class="checkmark"></span></label>
                        <label for="inputEmail4">&nbsp;&nbsp;&nbsp;&nbsp;</label>
                         <label class="radiobutton-container" > 
                               Apellido(s)<input id="rbapellidos" runat="server" type="radio" name="deck" value="ape" clientidmode="Static"/>
                                <span class="checkmark"></span></label>
                        
                         </div> 
                         <div class="form-group col-md-6 d-flex">
                         <asp:TextBox runat="server" id="txtcriterioconsulta" class="form-control"/> <label for="inputEmail4">&nbsp;&nbsp;</label>
                            <asp:Button ID="btnBuscar" runat="server" class="btn btn-outline-primary mr-4"
                                OnClientClick="return fvalidaCriterios();" onclick="btnBuscar_Click" Text="Buscar" clientidmode="Static"></asp:Button> 
                     </div> 

           <hr style="width:100%;text-align:left;margin-left:0"/>

         <div class="form-group col-md-6 d-flex">
                         <label class="radiobutton-container" >
                               C.I.<input id="rbci" runat="server"  checked="true" type="radio" name="deck" value="ci" clientidmode="Static"/>
                                <span class="checkmark"></span></label>
                        <label for="inputEmail4">&nbsp;&nbsp;&nbsp;&nbsp;</label>
                         <label class="radiobutton-container" > 
                               Pasaporte<input id="rbpasaporte" runat="server" type="radio" name="deck" value="pas" clientidmode="Static"/>
                                <span class="checkmark"></span> <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                         </div> 
                         <div class="form-group col-md-6">
                         <asp:TextBox runat="server" id="txtcipas" class="form-control" MaxLength="25" onBlur="valcipas(this,'valcipas','rbci','rbpasaporte');"
                             style="text-align: center; text-transform :uppercase" onkeypress="return soloLetras(event,'01234567890abcdefghijklnmñopqrstuvwxyz',true)"/> 
                             <span id="valcipas" class="validacion"> </span>
                     </div> 
         <div class="form-group col-md-6">
                 <label for="inputAddress">Nombres:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                     </div>
                 <div class="form-group col-md-6">
                 <asp:TextBox style="text-align: center; text-transform :uppercase" ID="txtnombres" runat="server" ClientIDMode="Static" MaxLength="500" class="form-control"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 .')" onblur="checkcajalarge(this,'valnombre',true);"></asp:TextBox>
                     <span id="valnombre" class="validacion"> </span>
                 </div>

                 <div class="form-group col-md-6">
                 <label for="inputAddress">Apellidos:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                     </div>
                     <div class="form-group col-md-6">
                           <asp:TextBox style="text-align: center; text-transform :uppercase" ID="txtapellidos" runat="server" ClientIDMode="Static" MaxLength="500" class="form-control"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ')" onblur="checkcajalarge(this,'valapellido',true);"></asp:TextBox>
                         <span id="valapellido" class="validacion"> </span>
                 </div>

         <div class="form-group col-md-6">
                 <label for="inputAddress">Tipo de Sangre:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
        </div>
         <div class="form-group col-md-6">
                <%--<asp:TextBox style="text-align: center" ID="txttiposangre" runat="server" ClientIDMode="Static" MaxLength="500" class="form-control"
                    onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz+ ')" onblur="checkcaja(this,'valtipsangre',true);"></asp:TextBox>--%>
                <asp:DropDownList runat="server" ID="txttiposangre" class="form-control" 
                                onchange="valdltipsol(this, valtipsangre);" >
                                <asp:ListItem Value="O+">RH O+</asp:ListItem>
                                <asp:ListItem Value="O-">RH O-</asp:ListItem>
                                <asp:ListItem Value="A+">RH A+</asp:ListItem>
                                <asp:ListItem Value="A-">RH A-</asp:ListItem>
                                <asp:ListItem Value="B+">RH B+</asp:ListItem>
                                <asp:ListItem Value="B-">RH B-</asp:ListItem>
                                <asp:ListItem Value="AB+">RH AB+</asp:ListItem>
                                <asp:ListItem Value="AB-">RH AB-</asp:ListItem>
                            </asp:DropDownList>
                <span id='valtipsangre' class="validacion" ></span>
            </div>

         <div class="form-group col-md-6">
                 <label for="inputAddress">Dirección de Domicilio:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                     </div>
         <div class="form-group col-md-6">
                <asp:TextBox runat="server" id="txtdirdom" class="form-control" MaxLength="500" onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890/-_ ',true)" 
                    onblur="checkcajalarge(this,'valdirdom',true);"/> 
             <span id="valdirdom" class="validacion"> </span>
        </div>

        <div class="form-group col-md-6">
            <label for="inputAddress">Teléfono de Domicilio:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
        </div>
         <div class="form-group col-md-6">
                <asp:TextBox runat="server" id="txtteldom" class="form-control" MaxLength="9" onkeypress="return soloLetras(event,'01234567890',true)" 
                    onblur="telconvencional(this,'valtelofi',true);"/> 
             <span id="valtelofi" class="validacion"> </span>
        </div>

         <div class="form-group col-md-6">
            <label for="inputAddress">Correo Electrónico:</label>
         </div>
         <div class="form-group col-md-6">
             <asp:TextBox runat="server" id='tmailinfocli' name='textboxmailinfocli' class="form-control" placeholder="mail@mail.com" ToolTip="Para mas de un Mail separelos con punto y coma"
                onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_$@.;',true)" onblur="mailone(this,'valmailrl');" MaxLength="200"/>
        </div>

         <div class="form-group col-md-6">
                 <label for="inputAddress">Lugar de Nacimiento:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                     </div>
         <div class="form-group col-md-6">
                <asp:TextBox runat="server" id="txtlugarnacimiento" class="form-control" MaxLength="500" onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890/-_ ',true)" 
                    onblur="checkcajalarge(this,'vallugarnac',true);"/> 
             <span id="vallugarnac" class="validacion"> </span>
        </div>

            <div class="form-group col-md-6">
                 <label for="inputAddress">Fecha de Nacimiento:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
            </div>
            <div class="form-group col-md-6">
                 <asp:TextBox ID="txtfechanacimiento" runat="server" MaxLength="10" CssClass="datetimepicker form-control"
                 onkeypress="return soloLetras(event,'01234567890/')" style="text-align: center" onblur="checkcaja(this,'valfecnac',true);" ClientIDMode="Static"></asp:TextBox> 
                      <span id="valfecnac" class="validacion"> </span>
            </div>

         <div class="form-group col-md-6">
                 <label for="inputAddress">Cargo:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                  </div>
                  <div class="form-group col-md-6">
                          <asp:DropDownList runat="server" ID="ddlCargoOnlyControl" onchange="validadropdownlist(this, valcargoac);" class="form-control">
                                    <asp:ListItem Value="0">* Elija *</asp:ListItem>
                          </asp:DropDownList>
                      <span id="valcargoac" class="validacion"> </span>
            </div>

         <div class="form-group col-md-12 d-flex">
                 <label for="inputAddress">Nota:<span style="color: #FF0000; font-weight: bold;"> </span></label>
                           <asp:TextBox ID="txtNota" runat="server" MaxLength="3000" TextMode="MultiLine" Heigth="60px" class="form-control" style="overflow:auto;resize:none"
                              onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 /_-.',true)"/>
             
            </div>

             <div class="form-group col-md-12">
                            <asp:Button ID="btnAgregar" runat="server" Text="Agregar" class="btn btn-outline-primary mr-4"
                                OnClientClick="return validaCabecera();" onclick="btnAgregar_Click"/>
                 <span id="imgagrega" class="fa fa-plus-square-0"></span>
                  <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
             </div> 

      <div style=" display:none"><label class="bt-bottom  bt-right bt-left" >Tipo de Licencia:</label>
         <a class="tooltip"><span class="classic">
                Campo obligatorio solo para Compañia de Transporte.</span>
             <asp:TextBox style="text-align: center" ID="txttiplic" runat="server" ClientIDMode="Static" MaxLength="500"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 /_-.')"></asp:TextBox>
             </a>
         </div>
      <div style=" display:none"><label class="bt-bottom  bt-right bt-left" >Fecha Exp. Licencia:</label>
         <a class="tooltip"><span class="classic">
                Campo obligatorio solo para Compañia de Transporte.</span>
             <asp:TextBox style="text-align: center; text-transform :uppercase" ID="txtfecexplic" runat="server" MaxLength="15" CssClass="datetimepicker"
             onkeypress="return soloLetras(event,'0123456789/')"></asp:TextBox>
             </a>
         </div>

      <div class="controles" cellspacing="0" cellpadding="1" style=" display:none">
      <label class="bt-bottom  bt-right bt-left" style=" width:155px">Área Destino/Actividad:</label>
             <asp:TextBox style="text-align: center" ID="txtaredes" runat="server" ClientIDMode="Static" width="200px" MaxLength="500"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 /_-.')"></asp:TextBox>
      </div>

      <div class="controles" cellspacing="0" cellpadding="1" style=" display:none">
      <label class="bt-bottom  bt-right bt-left" style=" width:155px">Tiempo de Estadia:</label>
             <asp:TextBox 
             style="text-align: center" 
             ID="txttiempoestadia" runat="server" ClientIDMode="Static" width="200px" MaxLength="500"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 /_-')" 
             ></asp:TextBox>
      </div>

      <div class="form-group col-md-12">
            <asp:UpdatePanel ID="UPNotificacion" runat="server" UpdateMode="Conditional" >  
            <ContentTemplate>
                    <div id="msjNotificaciones" runat="server" class=" alert  alert-warning" visible="false" >
                            No se encontraron resultados, 
                            asegurese que exista los registros faciales.
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

        <div class="form-group col-md-12"> 
           <%-- <asp:UpdatePanel ID="upresult2" runat="server"  >
                    <ContentTemplate>--%>
                    <script type="text/javascript">          Sys.Application.add_load(BindFunctions);</script>
                        <div class="informativo" id="colector">
                                <%--<table runat="server" id="tableexpo">
                                <tr><td>--%>
                                    <div class="bokindetalle" style=" width:100%; overflow:auto">
                                        <asp:GridView runat="server" id="gvColaboradores" class="table table-bordered invoice" AutoGenerateColumns="False" Width="100%"
                                                onrowcommand="gvColaboradores_RowCommand"  onrowdeleting="gvColaboradores_RowDeleting">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Nombres">
                                                <ItemTemplate>
                                                    <asp:Label ID="tnombres" runat="server" Text='<%# Eval("[Nombres]") %>'
                                                    ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Apellidos">
                                                <ItemTemplate>
                                                    <asp:Label ID="tapellidos" runat="server" Text='<%# Eval("[Apellidos]") %>'
                                                    ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="C.I./Pasaporte">
                                                <ItemTemplate>
                                                    <asp:Label ID="tcipas" runat="server" Text='<%# Eval("[CIPasaporte]") %>' ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="TipoSangre" Visible="false">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="ttiposangre" runat="server" Text='<%# Eval("[TipoSangre]") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="DireccionDomicilio" Visible="false">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="tdirdom" runat="server" Text='<%# Eval("[DireccionDomicilio]") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="TelefonoDomicilio" Visible="false">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="ttelfdom" runat="server" Text='<%# Eval("[TelefonoDomicilio]") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Mail" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="tmail"  style="text-transform :uppercase" runat="server" Text='<%# Eval("[Mail]") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="LugarNacimiento" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="tlugnac"  style="text-transform :uppercase" runat="server" Text='<%# Eval("[LugarNacimiento]") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="FechaNacimiento" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="tfecnac"  style="text-transform :uppercase" runat="server" Text='<%# Eval("[FechaNacimiento]") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cargo">
                                                <ItemTemplate>
                                                    <asp:Label ID="tcargo"  style="text-transform :uppercase" runat="server" Text='<%# Eval("[Cargo]") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Nota">
                                                <ItemTemplate>
                                                <div style="overflow-y:scroll; overflow-x: hidden; height:30px; text-align:center">
                                                    <asp:Label ID="tnota"  style="text-transform :uppercase" runat="server" ToolTip='<%# Eval("[Nota]") %>' Text='<%# Eval("[Nota]") %>'></asp:Label>
                                                </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NO Cargados" Visible="false">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkEstadoDocumentos" runat="server" Enabled="false"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="False"  HeaderText="Adjuntar">
                                                <ItemTemplate>
                                                    <a id="btnVer" class="btn btn-outline-primary mr-4" onclick="redirectcatdoc('<%# Eval("CIPasaporte") %>');" >
                                                    <i class="fa fa-search" ></i> Documentos </a>
                                                </ItemTemplate>
                                            </asp:TemplateField>


                                        <asp:TemplateField ShowHeader="False"  HeaderText="Imagen 1" FooterText="Imagen 1" HeaderImageUrl="~/images/facial1.jpg">
                                                <ItemTemplate>
                                                    <asp:FileUpload extension=".jpg"  class="btn btn-outline-primary mr-4"
                                                        id="fsupload" title="Escoja el archivo con formato indicado en la siguiente columna." 
                                                        onchange="validaextensionJG(this)" style=" font-size:small" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:templatefield showheader="false"  headertext="imagen 2" HeaderImageUrl="~/images/facial2.jpg">
                                                <itemtemplate>
                                                    <asp:fileupload extension=".jpg"  class="btn btn-outline-primary mr-4" 
                                                        id="fsupload1" title="escoja el archivo con formato indicado en la siguiente columna." 
                                                        onchange="validaextensionjg(this)" style=" font-size:small" runat="server"/>
                                                </itemtemplate>
                                            </asp:templatefield>
                                            <asp:templatefield showheader="false"  headertext="imagen 3" HeaderImageUrl="~/images/facial3.jpg">
                                                <itemtemplate>
                                                    <asp:fileupload extension=".jpg"  class="btn btn-outline-primary mr-4" 
                                                        id="fsupload2" title="escoja el archivo con formato indicado en la siguiente columna." 
                                                        onchange="validaextensionjg(this)" style=" font-size:small" runat="server"/>
                                                </itemtemplate>
                                            </asp:templatefield>


                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-outline-primary mr-4" CausesValidation="False"
                                                        CommandName="Delete" Text="Eliminar" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    </div>    
                            <%--</td></tr>
                                </table>--%>
                        </div>
                   <%-- </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btsalvar" /> 
                </Triggers>
            </asp:UpdatePanel>--%>
        </div>

     </div>
       <div class="row">
        <div class="alert alert-danger col-md-12" style=" font-weight:bold">
         <span>Nota:</span>
         <br />
         <span>Certifico que la información aquí suministrada es verdadera y podrá ser verificada en cualquier momento por CONTECON Así mismo estoy dispuesto a brindar una ampliación de cualquier aspecto de los datos registrados.</span>
         <br />
         <span>Y me comprometo a no ingresar al TERMINAL MARITIMO en estado de embriaguez o bajo la acción de cualquier sustancia psicotrópicas así como cumplir con todas las normas, procedimientos  y disposiciones de CGSA.</span>
        </div>
          
        <div class="col-md-12 d-flex justify-content-center">
                <asp:Button ID="btsalvar" runat="server" Text="Enviar Solicitud" OnClientClick="return prepareObject('¿Esta seguro de enviar la solicitud?');" class="btn btn-primary" 
            onclick="btsalvar_Click" ToolTip="Confirma la información y genera el envio de la solicitud."/>
        </div>
       </div>
    </div>  
    
<asp:HiddenField runat="server" ID="emailsec2" />
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
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
        //controlador de mails
        var counter = 2;
        $("#addButton").click(function () {
            if (counter > 5) {
                alert("Solo se permiten 5 mails");
                return false;
            }
            $('<div/>', { 'id': 'TextBoxDiv' + counter }).html($('<span/>').html('mail #' + counter + ':')).append($('<input type="text" placeholder="mail@mail.com">').attr({ 'id': 'textbox' + counter, 'name': 'textbox' + counter, onkeypress: 'return soloLetras(event,"abcdefghijklmnñopqrstuvwxyz1234567890_@.",true);' })).appendTo('#TextBoxesGroup')
            counter++;
        });
        $("#removeButton").click(function () {
            if (counter == 2) {
                alert("Un mail es obligatorio");
                return false;
            }
            counter--;
            $("#TextBoxDiv" + counter).remove();
        });
        //controlador de mails representante legal
        var counter2 = 2;
        $("#addButtonRepLegal").click(function () {
            if (counter2 > 5) {
                alert("Solo se permiten 5 mails");
                return false;
            }
            $('<div/>', { 'id': 'TextBoxDivRepLegal' + counter2 }).html($('<span/>').html('mail #' + counter2 + ':')).append($('<input type="text" placeholder="mail@mail.com">').attr({ 'id': 'textboxreplegal' + counter2, 'name': 'textboxreplegal' + counter2, onkeypress: 'return soloLetras(event,"abcdefghijklmnñopqrstuvwxyz1234567890_@.",true);' })).appendTo('#TextBoxesGroupRepLegal')
            counter2++;
        });
        $("#removeButtonRepLegal").click(function () {
            if (counter2 == 2) {
                alert("Un mail es obligatorio");
                return false;
            }
            counter2--;
            $("#TextBoxDivRepLegal" + counter2).remove();
        });
        //controlador de mails autorizacion terceros
        var counter3 = 2;
        $("#addButtonMailAuTer").click(function () {
            if (counter3 > 5) {
                alert("Solo se permiten 5 mails");
                return false;
            }
            $('<div/>', { 'id': 'textboxmailauter' + counter3 }).html($('<span/>').html('mail #' + counter3 + ':')).append($('<input type="text" placeholder="mail@mail.com">').attr({ 'id': 'textboxmailauter' + counter3, 'name': 'textboxmailauter' + counter3, onkeypress: 'return soloLetras(event,"abcdefghijklmnñopqrstuvwxyz1234567890_@.",true);' })).appendTo('#TextBoxesGroupAuTer')
            counter3++;
        });
        $("#removeButtonMailAuTer").click(function () {
            if (counter3 == 2) {
                alert("Un mail es obligatorio");
                return false;
            }
            counter3--;
            $("#textboxmailauter" + counter3).remove();
        });
        //esta futura funcion va a preparar el objeto a transportar.
        var registroempresa = {};
        var lista = [];
        var cblarray = [];
        var carray = [];
        function prepareObject(valor) {
            try {
                if (confirm(valor) == false) {
                    return false;
                }
                var vals = document.getElementById('<%=txtusuariosolicita.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Escriba los Nombres del Representante Legal*');
                    document.getElementById('<%=txtusuariosolicita.ClientID %>').focus();
                    <%--document.getElementById('<%=txtusuariosolicita.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtci.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Escriba la Cedula del Representante Legal*');
                    document.getElementById('<%=txtci.ClientID %>').focus();
                    <%--document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (!valcisolicita()) {
                    return false;
                };
                var vals = document.getElementById('<%=txtfecing.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('*Seleccione la Fecha de Ingreso *');
                    document.getElementById('<%=txtfecing.ClientID %>').focus();
                    <%--document.getElementById('<%=txtfecing.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtfecsal.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('*Seleccione la Fecha de Caducidad *');
                    document.getElementById('<%=txtfecsal.ClientID %>').focus();
                    <%--document.getElementById('<%=txtfecsal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=ddlAreaOnlyControl.ClientID %>').options[document.getElementById("<%=ddlAreaOnlyControl.ClientID%>").selectedIndex].text;
                if (vals == '* Elija *') {
                    alert('* Seleccione el Area *');
                    document.getElementById('<%=ddlAreaOnlyControl.ClientID %>').focus();
                    <%--document.getElementById('<%=ddlAreaOnlyControl.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:350px;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').options[document.getElementById("<%=ddlActividadOnlyControl.ClientID%>").selectedIndex].text;
                if (vals == '* Elija *') {
                    alert('* Seleccione la Actividad Permitida *');
                    document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').focus();
                    <%--document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:350px;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                    return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }
        function validaCabecera() {
            try {
 
                var vals = document.getElementById('<%=ddlCargoOnlyControl.ClientID %>').options[document.getElementById("<%=ddlCargoOnlyControl.ClientID%>").selectedIndex].text;
                if (vals == '* Elija *') {
                    alert('* Seleccione el Cargo *');
                    document.getElementById('<%=ddlCargoOnlyControl.ClientID %>').focus();
                    <%--document.getElementById('<%=ddlCargoOnlyControl.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:350px;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtnombres.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Escriba los Nombres *');
                    document.getElementById('<%=txtnombres.ClientID %>').focus();
                    <%--document.getElementById('<%=txtnombres.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtapellidos.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Escriba los Apellidos *');
                    document.getElementById('<%=txtapellidos.ClientID %>').focus();
                    <%--document.getElementById('<%=txtapellidos.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (!valcipasservidor()) {
                    return false;
                };
                <%--var vals = document.getElementById('<%=txttiposangre.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Escriba el Tipo de Sangre *');
                    document.getElementById('<%=txttiposangre.ClientID %>').focus();
                    document.getElementById('<%=txttiposangre.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }--%>
                var vals = document.getElementById('<%=txtdirdom.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('*Escriba la Dirección de Domicilio *');
                    document.getElementById('<%=txtdirdom.ClientID %>').focus();
                    <%--document.getElementById('<%=txtdirdom.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtteldom.ClientID %>').value;
                if (!/^([0-9])*$/.test(vals)) {
                    alert('* Teléfono de Domicilio No es un Numero *');
                    document.getElementById('<%=txtteldom.ClientID %>').focus();
                    <%--document.getElementById('<%=txtteldom.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (vals.length < 9) {
                    alert('* Teléfono de Domicilio Incompleto *');
                    document.getElementById('<%=txtteldom.ClientID %>').focus();
                    <%--document.getElementById('<%=txtteldom.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Escriba el Teléfono de Domicilio *');
                    document.getElementById('<%=txtteldom.ClientID %>').focus();
                    <%--document.getElementById('<%=txtteldom.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                //Valida Mail
//                var mail1 = document.getElementById('tmailinfocli').value;
//                if (mail1 == null || mail1 == undefined || mail1 == '') {
//                    alert('* Escriba al menos un mail *');
//                    document.getElementById('tmailinfocli').focus();
//                    document.getElementById('tmailinfocli').style.cssText = "background-color:#ffffc6;color:Red;width:400;";
//                    document.getElementById("loader").className = 'nover';
//                    return false;
//                }
                var vals = document.getElementById('<%=txtlugarnacimiento.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Escriba el Lugar de Nacimiento *');
                    document.getElementById('<%=txtlugarnacimiento.ClientID %>').focus();
                    <%--document.getElementById('<%=txtlugarnacimiento.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (!valedad()) {
                    return false;
                };
//                var vals = document.getElementById('<%=txtaredes.ClientID %>').value;
//                if (vals == '' || vals == null || vals == undefined) {
//                    alert('* Datos de Solicitud de Credencial: *\n * Escriba el Area de Destino *');
//                    document.getElementById('<%=txtaredes.ClientID %>').focus();
//                    document.getElementById('<%=txtaredes.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
//                    document.getElementById("loader").className = 'nover';
//                    return false;
//                }

//                var vals = document.getElementById('<%=txttiplic.ClientID %>').value;
//                if (vals == '' || vals == null || vals == undefined) {
//                    alert('* Datos de Solicitud de Credencial: *\n * Escriba el Tipo de Licencia *');
//                    document.getElementById('<%=txttiplic.ClientID %>').focus();
//                    document.getElementById('<%=txttiplic.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
//                    document.getElementById("loader").className = 'nover';
//                    return false;
//                }
//                var vals = document.getElementById('<%=txtfecexplic.ClientID %>').value;
//                if (vals == '' || vals == null || vals == undefined) {
//                    alert('* Datos de Solicitud de Credencial: *\n * Escriba la Fecha de Expiración de la Licencia *');
//                    document.getElementById('<%=txtfecexplic.ClientID %>').focus();
//                    document.getElementById('<%=txtfecexplic.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
//                    document.getElementById("loader").className = 'nover';
//                    return false;
//                }
//                var vals = document.getElementById('<%=txttiempoestadia.ClientID %>').value;
//                if (vals == '' || vals == null || vals == undefined) {
//                    alert('* Datos de Solicitud de Credencial: *\n * Escriba el Tiempo de Estadia *');
//                    document.getElementById('<%=txttiempoestadia.ClientID %>').focus();
//                    document.getElementById('<%=txttiempoestadia.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
//                    document.getElementById("loader").className = 'nover';
//                    return false;
//                }
                document.getElementById("loader").className = '';
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }
        function valedad() {
            var codigo;
            codigo = document.getElementById('<%=txtfechanacimiento.ClientID %>').value;
            if (codigo.length <= 0) {
                alert('* Datos de Solicitud de Credencial: *\n * Escoja la Fecha de Nacimiento *');
                document.getElementById('<%=txtfechanacimiento.ClientID %>').focus();
                <%--document.getElementById('<%=txtfechanacimiento.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400;";--%>
                document.getElementById("loader").className = 'nover';
                return false;
            }
//            var desdeDate = new Date();
//            var desde = desdeDate.format("d/mm/yy");
//            var hasta = codigo;
//            alert(desde);
//            alert(hasta);
//            return true;
//            if (desde == undefined || desde == null || hasta == undefined || hasta == null) {
//                alert('Por favor agregue o revise las fechas inicio/expiracion');
//                return false;
//            }
//            var datePart = codigo.value.split('/');
//            var date = new Date(datePart[1] + '/' + datePart[0] + '/' + datePart[2]);
//            if (!date instanceof Date || isNaN(date.valueOf())) {
//                alert('Por favor revise/escriba la fecha de inicio');
//                return false;
//            }
//            var datePart2 = hasta.value.split('/');
//            var date2 = new Date(datePart2[1] + '/' + datePart2[0] + '/' + datePart2[2]);
//            if (!date2 instanceof Date || isNaN(date2.valueOf())) {
//                alert('Por favor revise/escriba la fecha de caducidad');
//                return false;
//            }
//            if (date > date2) {
//                alert('El contrato no puede iniciar despues de su fecha fin');
//                return false;
//            }
            return true;
        }
        function valcisolicita() {
            //codigo = control.value.trim().toUpperCase();
            var valruccipas = document.getElementById('<%=txtci.ClientID %>').value;
           <%-- if (!/^([0-9])*$/.test(valruccipas)) {
                alert('* No. C.I. No es un Numero *');
                document.getElementById('<%=txtci.ClientID %>').focus();
              
                document.getElementById("loader").className = 'nover';
                return false;
            }
            else--%>
            if (valruccipas == '' || valruccipas == null || valruccipas == undefined) {
                alert('* Escriba el No. de C.I. *');
                document.getElementById('<%=txtci.ClientID %>').focus();
              
                document.getElementById("loader").className = 'nover';
                return false;
            }
            if (valruccipas.length = 0) {
                alert('* Escriba el No. de C.I. *');
                document.getElementById('<%=txtci.ClientID %>').focus();
              
                document.getElementById("loader").className = 'nover';
                return false;
            }
            if (valruccipas.length < 10) {
                alert('* No. C.I. INCOMPLETO! *');
                document.getElementById('<%=txtci.ClientID %>').focus();
               
                document.getElementById("loader").className = 'nover';
                return false;
            }
            var array = valruccipas.split("");
            var num = array.length;
            var total = 0;
            var digito = (array[9] * 1);
            for (i = 0; i < (num - 1); i++) {
                var mult = 0;
                if ((i % 2) != 0) {
                    total = total + (array[i] * 1);
                }
                else {
                    mult = array[i] * 2;
                    if (mult > 9)
                        total = total + (mult - 9);
                    else
                        total = total + mult;
                }
            }
            var decena = total / 10;
            decena = Math.floor(decena);
            decena = (decena + 1) * 10;
            var final = (decena - total);

           <%-- if (digito != 0) {
                if (final != digito) {
                    alert('* No. C.I. no válido! *');
                    document.getElementById('<%=txtci.ClientID %>').focus();
                  
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
            }
            else {
                if (final != 10) {
                    alert('* No. C.I. no válido! *');
                    document.getElementById('<%=txtci.ClientID %>').focus();
                  
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
            }--%>

            return true;
        }
        function valcipasservidor() {
            //codigo = control.value.trim().toUpperCase();
            var valruccipas = document.getElementById('<%=txtcipas.ClientID %>').value;
            var vci = document.getElementById('rbci').checked;
            var vpas = document.getElementById('rbpasaporte').checked;
            if (vci == true)
            {
              <%--  if (!/^([0-9])*$/.test(valruccipas)) {
                    alert('* No. C.I. No es un Numero *');
                    document.getElementById('<%=txtcipas.ClientID %>').focus();
                  
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                else--%>
               if (valruccipas == '' || valruccipas == null || valruccipas == undefined) {
                    alert('* Escriba el No. de C.I. *');
                    document.getElementById('<%=txtcipas.ClientID %>').focus();
                 
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (valruccipas.length = 0) {
                    alert('* Escriba el No. de C.I. *');
                    document.getElementById('<%=txtcipas.ClientID %>').focus();
                   
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (valruccipas.length < 10) {
                    alert('* No. C.I. INCOMPLETO! *');
                    document.getElementById('<%=txtcipas.ClientID %>').focus();
                  
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var array = valruccipas.split("");
                var num = array.length;
                var total = 0;
                var digito = (array[9] * 1);
                for (i = 0; i < (num - 1); i++) {
                    var mult = 0;
                    if ((i % 2) != 0) {
                        total = total + (array[i] * 1);
                    }
                    else {
                        mult = array[i] * 2;
                        if (mult > 9)
                            total = total + (mult - 9);
                        else
                            total = total + mult;
                    }
                }
                var decena = total / 10;
                decena = Math.floor(decena);
                decena = (decena + 1) * 10;
                var final = (decena - total);

                <%--if (digito != 0) {
                    if (final != digito) {
                        alert('* No. C.I. no válido! *');
                        document.getElementById('<%=txtcipas.ClientID %>').focus();
                   
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                }
                else {
                    if (final != 10) {
                        alert('* No. C.I. no válido! *');
                        document.getElementById('<%=txtcipas.ClientID %>').focus();
                     
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                }--%>
            }
            if (vpas == true) {
                if (valruccipas == '' || valruccipas == null || valruccipas == undefined) {
                    alert('* Escriba el No. de Pasaporte *');
                    document.getElementById('<%=txtcipas.ClientID %>').focus();
                  
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
            }
            return true;
        }
        function popupCallback(data, control) {
            this.document.getElementById(control).value = data;
        }
        function redireccionar() {
            window.locationf = "~/cuenta/zones.aspx";
        }
        function redirectcatdoc(val) {
            var cajaced = val;
            var cajatip = document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').value;
            //window.open('../credenciales/documentos/?placa=' + caja, 'name', 'width=850,height=480')
            window.open('../cliente/solicitudcolaboradordocumentosprovisional.aspx?CIPAS=' + cajaced + '&tipodoc=' + cajatip, 'name', 'width=950,height=880')
        }
        function fvalidaCriterios() {
                var cedula = document.getElementById('<%=rbcedula.ClientID %>').checked;
                var nombres = document.getElementById('<%=rbnombres.ClientID %>').checked;
                var apellidos = document.getElementById('<%=rbapellidos.ClientID %>').checked;
                var vals = document.getElementById('<%=txtcriterioconsulta.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    document.getElementById('<%=txtcriterioconsulta.ClientID %>').focus();
                    <%--document.getElementById('<%=txtcriterioconsulta.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";--%>
                    if (cedula) {
                        alert('Escriba la Cedula del Colaborador.');
                        return false;
                    }
                    if (nombres) {
                        alert('Escriba el Nombre(s) del Colaborador.');
                        return false;
                    }
                    if (apellidos) {
                        alert('Escriba el Apellido(s) del Colaborador.');
                        return false;
                    }
                }
            return true;
        }
        function popupCallback(objeto, catalogo) {
            if (objeto == null || objeto == undefined) {
                alert('Hubo un problema al setaar un objeto de catalogo');
                return;
            }
            document.getElementById('<%=txtcriterioconsulta.ClientID %>').value = "";
            document.getElementById('<%=txtnombres.ClientID %>').value = objeto.nombres;
            document.getElementById('<%=txtapellidos.ClientID %>').value = objeto.apellidos;
            document.getElementById('<%=txtcipas.ClientID %>').value = objeto.cedula;
            document.getElementById('<%=txttiposangre.ClientID %>').value = objeto.tiposangre;
            document.getElementById('<%=txtdirdom.ClientID %>').value = objeto.dirdom;
            document.getElementById('<%=txtteldom.ClientID %>').value = objeto.telfdom;
            document.getElementById('tmailinfocli').value = objeto.email;
            document.getElementById('<%=txtlugarnacimiento.ClientID %>').value = objeto.lugnac;
            document.getElementById('<%=txtfechanacimiento.ClientID %>').value = objeto.fecnac;
            document.getElementById('<%=ddlCargoOnlyControl.ClientID %>').value = objeto.cargo;
//            document.getElementById('<%=txttiplic.ClientID %>').value = objeto.tiplic;
//            document.getElementById('<%=txtfecexplic.ClientID %>').value = objeto.fecexplic;
//            document.getElementById('valnombre').textContent = "";
//            document.getElementById('valapellido').textContent = "";
//            document.getElementById('valcipas').textContent = "";
//            document.getElementById('valtipsangre').textContent = "";
//            document.getElementById('valdirdom').textContent = "";
//            document.getElementById('valtelofi').textContent = "";
//            //                document.getElementById('mailopcional').textContent = "";
//            document.getElementById('vallugarnac').textContent = "";
//            document.getElementById('valfecnac').textContent = "";
//            document.getElementById('valcargoac').textContent = "";
            //            document.getElementById('<%=txtnombres.ClientID %>').style.cssText = "background-color:White;width:400px;";
            //            document.getElementById('<%=txtapellidos.ClientID %>').style.cssText = "background-color:White;width:400px;";
            //            document.getElementById('<%=txtcipas.ClientID %>').style.cssText = "background-color:White;width:200px;";
        }



        function validaextensionJG(control) {
        var ext = control.getAttribute("extension");

        extensiones_permitidas = new Array(ext);
        archivo = control.value;
        mierror = "";
        if (!archivo) {
            //Si no tengo archivo, es que no se ha seleccionado un archivo en el formulario 
            mierror = "No has seleccionado ningún archivo";
        } else {
            //recupero la extensión de este nombre de archivo 
            var extension = (archivo.substring(archivo.lastIndexOf("."))).toLowerCase();

            permitida = false;
            for (var i = 0; i < extensiones_permitidas.length; i++) {

           
                if (extensiones_permitidas[i] == extension) {
                    permitida = true;
                    break;
                }
            }
            if (!permitida) {
                mierror = "Comprueba la extensión de los archivos a subir. \nSólo se pueden subir archivos con extensiones: " + extensiones_permitidas.join();
                control.value = null;
            } else {
                //submito! 
               // alert("Todo correcto. Voy a submitir el formulario.");

                //////////////////////////
                //validacion de tamaño
                //////////////////////////
                const fi = control;
      
                // Check if any file is selected.
                if (fi.files.length > 0) {
                    for (const i = 0; i <= fi.files.length - 1; i++) {
  
                        const fsize = fi.files.item(i).size;
                        const file = Math.round((fsize / 1024));

                        // alert(file);
                        // The size of the file.
                        if (file > 300) {
                            alert(
                              "Archivo demasiado grande, seleccione un archivo de menos de 301 KB. Resolución máxima de la foto 1280 x 720");
                        } else if (file < 12) {
                            alert(
                              "Archivo demasiado pequeño, seleccione un archivo de más de 12 KB. Resolución de la foto mínima 640 x 480");
                        } else {
                           control.tooltip = control.value;
                            return 1;
                        }
                    }
                }
            }
        }
        //si estoy aqui es que no se ha podido submitir 
        alert(mierror);
        return 0;
    }
    </script>
  <asp:updateprogress  id="updateProgress" runat="server">
    <progresstemplate>
        <div id="progressBackgroundFilter"></div>
        <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor  espere...<br />
         <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
        </div>
    </progresstemplate>
    </asp:updateprogress>
</asp:Content>
