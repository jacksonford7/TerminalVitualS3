<%@ Page Language="C#" MasterPageFile="~/site.Master" Title="Emisión/Renovación de Credencial" MaintainScrollPositionOnPostback="true"
         AutoEventWireup="true" CodeBehind="solicitudcolaborador.aspx.cs" Inherits="CSLSite.cliente.solicitudcolaborador" %>
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

      <%--<link href="../shared/estilo/Reset.css" rel="stylesheet" />
 
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
    <link href="../shared/estilo/w3-progressbar.css" rel="stylesheet" type="text/css" />
    <link href="../shared/avisos/ppt.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    .div {
    border: 2px solid;
    padding: 20px;
    width: 300px;
    resize: none;
    overflow: auto;
}
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
</style>--%>
    <style type="text/css">
body {
  font-family: Arial, Helvetica, sans-serif;
}

/* Dashed border */
hr.dashed {
  border-top: 3px dashed #bbb;
}

/* Dotted border */
hr.dotted {
  border-top: 3px dotted #bbb;
}

/* Solid border */
hr.solid {
  border-top: 3px solid #bbb;
}

/* Rounded border */
hr.rounded {
  border-top: 8px solid #bbb;
  border-radius: 5px;
}
</style>
</asp:Content>
<asp:Content ID="formcolaborador" ContentPlaceHolderID="placebody" runat="server">
    <%--<a name="MOVEHERE"></a>--%>
  <input id="zonaid" type="hidden" value="7" />
  <asp:ScriptManager ID="sMan" EnableScriptGlobalization="true" runat="server"></asp:ScriptManager>
  <div>
        <asp:Timer ID="TimerPb" OnTick="TimerPb_Tick" Enabled="false" runat="server" Interval="10"></asp:Timer>
  </div>
  <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">MCA - Módulo de Control de Acceso</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Emisión/Renovación de Credencial</li>
          </ol>
        </nav>
      </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">

         <div class="form-row">

              <div class="form-title col-md-12">
             <a class="btn btn-outline-primary mr-4" href="#" target="_blank" runat="server" id="aprint" clientidmode="Static" >1</a>
             <a class="level1" target="_blank" runat="server" id="a1" clientidmode="Static" >Tipo de Empresa</a>
             </div>

             <div class="form-group col-md-12 d-flex">
                 <label for="inputAddress"><span style="color: #FF0000; font-weight: bold;"> *</span></label>
                          <asp:DropDownList runat="server" ID="ddlTipoEmpresa" class="form-control" 
                                    onchange="fEmpresa();valdltipsol(this, valtipempresa);" >
                                    <asp:ListItem Value="0">* Seleccione origen *</asp:ListItem>
                                </asp:DropDownList>
             <span id='valtipempresa' class="validacion" ></span>
            </div>
        </div>

        <div class="form-row">
            <div class="form-group col-md-12">
                <a class="btn btn-outline-primary mr-4" href="#" target="_blank" runat="server" id="a2" clientidmode="Static" >2</a>
                <a class="level1" target="_blank" runat="server" id="a3" clientidmode="Static" >Tipo de Credencial</a>
            </div>
        

            <div class="form-group col-md-12">
                <div class="alert alert-danger" runat="server" id="infotipcre" style=" font-weight:bold">
                </div>
            </div>

            <div class="form-group col-md-12 d-flex">
                <label for="inputAddress"><span style="color: #FF0000; font-weight: bold;"> *</span></label>
                        <asp:DropDownList runat="server" ID="cbltiposolicitud" class="form-control" 
                                onchange="valdltipsol(this, valtipsol);" >
                                <asp:ListItem Value="0">* Seleccione origen *</asp:ListItem>
                            </asp:DropDownList>
                <span id='valtipsol' class="validacion" ></span>
            </div>

        </div>

        <div class="form-row">
            <div class="form-title col-md-12">
                <a class="btn btn-outline-primary mr-4" href="#" target="_blank" runat="server" id="a4" clientidmode="Static" >3</a>
                <a class="level1" target="_blank" runat="server" id="a5" clientidmode="Static" >Datos Generales del Colaborador</a>
            </div>

            <div class="form-group col-md-12 d-flex">
                <label class="radiobutton-container" >
                    Registro de Credencial<input id="rbemision" onclick="fValidaEmision();" runat="server" checked="true" type="radio" name="deck2" value="ci"/>
                    <span class="checkmark"></span></label>
                <label for="inputEmail4">&nbsp;&nbsp;&nbsp;&nbsp;</label>
                <label class="radiobutton-container" > 
                    Renovación de Credencial<input id="rbrenovacion" onclick="fValidaRenovacion();" runat="server" type="radio" name="deck2" value="pas"/>
                    <span class="checkmark"></span> <span style="color: #FF0000; font-weight: bold;"> *</span></label>
            </div> 

            <div class="form-title col-md-12 d-flex">
                <label for="inputEmail4">Criterios de consulta para Colaboradores:</label>
            </div> 

            <div class="form-group col-md-6 d-flex">
                <label class="radiobutton-container" >
                    C.I./Pasaporte<input id="rbcedula" runat="server" checked="true" onclick="fIrAlCampoCriterioDeBusqueda();" type="radio" name="deck" value="ced" clientidmode="Static"/>
                    <span class="checkmark"></span></label>
                <label for="inputEmail4">&nbsp;&nbsp;&nbsp;&nbsp;</label>
                <label class="radiobutton-container" > 
                    Nombre(s)<input id="rbnombres" runat="server" onclick="fIrAlCampoCriterioDeBusqueda();" type="radio" name="deck" value="nom" clientidmode="Static"/>
                    <span class="checkmark"></span></label>
                <label for="inputEmail4">&nbsp;&nbsp;&nbsp;&nbsp;</label>
                <label class="radiobutton-container" > 
                    Apellido(s)<input id="rbapellidos" runat="server" onclick="fIrAlCampoCriterioDeBusqueda();" type="radio" name="deck" value="ape" clientidmode="Static" />
                    <span class="checkmark"></span></label>
                        
            </div> 
            <div class="form-group col-md-6 d-flex">
                <asp:TextBox runat="server" id="txtcriterioconsulta" class="form-control" Style="text-transform :uppercase"/> <label for="inputEmail4">&nbsp;&nbsp;</label>
                <asp:Button ID="btnBuscar" runat="server" class="btn btn-outline-primary mr-4"
                    OnClientClick="return fvalidaCriterios();" onclick="btnBuscar_Click" Text="Buscar" clientidmode="Static"></asp:Button> 
            </div> 

        <%--    <div class="form-title col-md-12 d-flex">
                <label for="inputEmail4">&nbsp;</label>
            </div> --%>

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
                <asp:TextBox runat="server" id="txtcipas" class="form-control" MaxLength="25" 
                    style="text-align: center;text-transform :uppercase" onkeypress="return soloLetras(event,'01234567890abcdefghijklnmñopqrstuvwxyz',true)"/> 
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
                    onblur="telconvencional(this,'valtelofi',true);" onpaste="return false"/> 
             <span id="valtelofi" class="validacion"> </span>
        </div>

         <div class="form-group col-md-6">
             <label for="inputAddress">Correo Electrónico:</label>
         </div>
         <div class="form-group col-md-6">
             <asp:TextBox runat="server" id='tmailinfocli' name='textboxmailinfocli' class="form-control" placeholder="mail@mail.com" ToolTip="Para mas de un Mail separelos con punto y coma"
                onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_$@.;',true)" onblur="mailopcional(this,'mailopcional');" MaxLength="200"/>
             <span id="mailopcional" class="validacion"> </span>
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
            <label for="inputAddress">Tipo de Licencia:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
        </div>
        <div class="form-group col-md-6">
            <asp:DropDownList runat="server" ID="ddlTipoLicencia" onchange="fValidaTipLic();valdlcatveh(this, valtipolic);" class="form-control">
                    <asp:ListItem Value="0">* Elija el tipo de licencia *</asp:ListItem>
            </asp:DropDownList>
            <span id="valtipolic" class="validacion"> </span>
        </div>

        <div class="form-group col-md-6">
                <label for="inputAddress">Fecha Exp. Licencia:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
        </div>
        <div class="form-group col-md-6">
            <asp:TextBox ID="txtfecexplic" runat="server" MaxLength="10" CssClass="datetimepicker form-control"
                onkeypress="return soloLetras(event,'01234567890/')" style="text-align: center" onblur="checkcaja(this,'valfecexplic',true);" ClientIDMode="Static"></asp:TextBox> 
            <span id="valfecexplic" class="validacion"> </span>
                  
        </div>

        <div class="form-group col-md-6">
            <label for="inputAddress">Cargo del Empleado:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
        </div>
        <div class="form-group col-md-6">
            <asp:TextBox style="text-align: center" ID="txtcargo" runat="server" ClientIDMode="Static" MaxLength="500" class="form-control"
                onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 /_-.')" onblur="checkcaja(this,'valcargo',true);"></asp:TextBox>
            <span id="valcargo" class="validacion"> </span>
        </div>

        <div class="form-group col-md-12 d-flex">
            <label for="inputAddress">Nota:<span style="color: #FF0000; font-weight: bold;"> </span></label>
            <asp:TextBox ID="txtNota" runat="server" MaxLength="3000" TextMode="MultiLine" Heigth="60px" class="form-control" style="overflow:auto;resize:none"
                        onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 /_-.',true)"/>
             
        </div>

        <div class="form-group col-md-12">
                    <asp:Button ID="btnAgregar" runat="server" Text="Agregar" class="btn btn-outline-primary mr-4"
                   OnClientClick="return validaCabecera();"     onclick="btnAgregar_Click"/>
             <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
        </div> 

      <div class="controles" cellspacing="0" cellpadding="1" style=" display:none">
         <label class="bt-bottom  bt-right bt-left" style=" width:155px">Área Destino/Actividad:</label>
             <asp:TextBox style="text-align: center" ID="txtaredes" runat="server" ClientIDMode="Static" width="200px" MaxLength="500"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 /_-.')"></asp:TextBox>
      </div>
      
      <div class="controles" cellspacing="0" cellpadding="1" style=" display:none">
         <label class="bt-bottom  bt-right bt-left" style=" width:155px">Tiempo de Estadia:</label>
             <asp:TextBox style="text-align: center" ID="txttiempoestadia" runat="server" ClientIDMode="Static" width="200px" MaxLength="500"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 /_-')"></asp:TextBox>
      </div>

      <asp:UpdatePanel ID="upresult2" runat="server"  >
      <ContentTemplate>
      <script type="text/javascript">          Sys.Application.add_load(BindFunctions);</script>
      <div class="informativo" id="colector">
      <table runat="server" id="tableexpo">
      <tr><td>
              <asp:GridView runat="server" id="gvColaboradores" class="table table-bordered invoice" AutoGenerateColumns="False" Width="100%"
                       onrowcommand="gvColaboradores_RowCommand"  onrowdeleting="gvColaboradores_RowDeleting">
              <Columns>
                  <asp:TemplateField HeaderText="Nombres">
                      <ItemTemplate>
                          <asp:Label ID="tnombres" runat="server" Enabled="false" ToolTip='<%# Eval("[Nombres]") %>' Text='<%# Eval("[Nombres]") %>'></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Apellidos">
                      <ItemTemplate>
                          <asp:Label ID="tapellidos" runat="server" Enabled="false" ToolTip='<%# Eval("[Apellidos]") %>' Text='<%# Eval("[Apellidos]") %>'
                          ></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="C.I./Pasaporte">
                      <ItemTemplate>
                          <asp:Label ID="tcipas" runat="server" ToolTip='<%# Eval("[CIPasaporte]") %>' Text='<%# Eval("[CIPasaporte]") %>' ></asp:Label>
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
                  <asp:TemplateField HeaderText="TipoLicencia" Visible="false">
                      <ItemTemplate>
                         <asp:Label ID="tareades"  style="text-transform :uppercase" runat="server" Text='<%# Eval("[TipoLicencia]") %>'></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Cargo">
                      <ItemTemplate>
                          <asp:Label ID="tcargo"  style="text-transform :uppercase" runat="server" ToolTip='<%# Eval("[Cargo]") %>' Text='<%# Eval("[Cargo]") %>'></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                   <asp:TemplateField HeaderText="Nota">
                      <ItemTemplate>
                      <div style="overflow-y:scroll; overflow-x: hidden; height:30px; text-align:center">
                          <asp:Label ID="tnota"  style="text-transform :uppercase" runat="server" ToolTip='<%# Eval("[Nota]") %>' Text='<%# Eval("[Nota]") %>'></asp:Label>
                      </div>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="FechaExpLicencia" Visible="false">
                      <ItemTemplate>
                          <asp:Label ID="ttiempoest"  style="text-transform :uppercase" runat="server" Text='<%# Eval("[FecExpLicencia]") %>'></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                   <asp:TemplateField HeaderText="NO Cargados" Visible="false">
                      <ItemTemplate>
                          <asp:CheckBox ID="chkEstadoDocumentos" runat="server" Enabled="false"/>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField ShowHeader="False" HeaderText="Adjuntar">
                        <ItemTemplate>
                            <a style=" id="adjDoc" class="btn btn-outline-primary mr-4" onclick="redirectcatdoc('<%# Eval("CIPasaporte") %>');" >
                            <i class="fa fa-search" ></i> Documentos </a>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-outline-primary mr-4" CausesValidation="False" 
                                CommandName="Delete" Text="Eliminar" />
                        </ItemTemplate>
                  </asp:TemplateField>
              </Columns>
          </asp:GridView>
       </td></tr>
       </table>
       </div>
       </ContentTemplate>
      <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btsalvar" />
      </Triggers>
     </asp:UpdatePanel>
      </div>

      <div class="row">
          <div class="form-group col-md-12" >
            <div class="alert alert-danger" style=" font-weight:bold">
             <span>Nota:</span>
             <br />
             <span>Certifico que la información aquí suministrada es verdadera y podrá ser verificada en cualquier momento por CONTECON Así mismo estoy dispuesto a brindar una ampliación de cualquier aspecto de los datos registrados.</span>
             <br />
             <span>Y me comprometo a no ingresar al TERMINAL MARITIMO en estado de embriaguez o bajo la acción de cualquier sustancia psicotrópicas así como cumplir con todas las normas, procedimientos  y disposiciones de CGSA.</span>
            </div>
            </div>
    
            <asp:UpdatePanel ID="UpdatePanelpb" UpdateMode="Conditional" runat="server">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="TimerPb" EventName="Tick" />
                </Triggers>
                <ContentTemplate>
                    <div class="col-md-12 d-flex justify-content-center">
                    <asp:Button ID="btsalvar" runat="server" Text="Enviar Solicitud"  onclick="btsalvar_Click" ClientIDMode="Static"
                                    OnClientClick="return prepareObject('¿Esta seguro de enviar la solicitud?');" CssClass="btn btn-primary"
                                    ToolTip="Confirma la información y genera el envio de la solicitud."/>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
   </div>

   <asp:HiddenField runat="server" ID="emailsec2" />
    <%--<script src='http://www.w3schools.com/lib/w3.css'></script>--%>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
    <script src='../shared/avisos/popup_script.js' type='text/javascript' ></script> 
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
                var vals = document.getElementById('<%=ddlTipoEmpresa.ClientID %>').value;
                if (vals == 0) {
                    alert('* Seleccione el Tipo de Empresa *');
                    document.getElementById('<%=ddlTipoEmpresa.ClientID %>').focus();
                    <%--document.getElementById('<%=ddlTipoEmpresa.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=cbltiposolicitud.ClientID %>').value;
                if (vals == 0) {
                    alert('* Seleccione al menos un Tipo de Solicitud *');
                    document.getElementById('<%=cbltiposolicitud.ClientID %>').focus();
                    <%--document.getElementById('<%=cbltiposolicitud.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                document.getElementById('<%=ddlTipoEmpresa.ClientID %>').focus();
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }
        function Panel() {
            $(document).ready(function () {
                $('#ventana_popup').fadeIn('slow');
                $('#popup-overlay').fadeIn('slow');
                $('#popup-overlay').height($(window).height());
            });
        }
        function validaCabecera() {
            try {
                //Valida los datos de las secciones
                var vals = document.getElementById('<%=ddlTipoEmpresa.ClientID %>').value;
                if (vals == 0) {
                    alert('* Seleccione el Tipo de Empresa *');
                    document.getElementById('<%=ddlTipoEmpresa.ClientID %>').focus();
                   
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=cbltiposolicitud.ClientID %>').value;
                if (vals == 0) {
                    alert('* Seleccione el Tipo de Solicitud *');
                    document.getElementById('<%=cbltiposolicitud.ClientID %>').focus();
                  
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtnombres.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Escriba los Nombres *');
                    document.getElementById('<%=txtnombres.ClientID %>').focus();
                   
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtapellidos.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Escriba los Apellidos *');
                    document.getElementById('<%=txtapellidos.ClientID %>').focus();
                   
                    document.getElementById("loader").className = 'nover';
                    return false;
                }

                if (!valcipasservidor()) {
                    return false;
                };
                var vals = document.getElementById('<%=txttiposangre.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Escriba el Tipo de Sangre *');
                    document.getElementById('<%=txttiposangre.ClientID %>').focus();
                
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtdirdom.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Escriba la Dirección de Domicilio *');
                    document.getElementById('<%=txtdirdom.ClientID %>').focus();
                 
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
               var vals = document.getElementById('<%=txtteldom.ClientID %>').value;
                if (!/^([0-9])*$/.test(vals)) {
                    alert('* Teléfono de Domicilio No es un Numero *');
                    document.getElementById('<%=txtteldom.ClientID %>').focus();
                  
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
              <%--  var vals = document.getElementById('<%=txtteldom.ClientID %>').value;--%>
                if (vals.length < 9) {
                    alert('* Teléfono de Domicilio Incompleto *');
                    document.getElementById('<%=txtteldom.ClientID %>').focus();
                   
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Escriba el Teléfono de Domicilio *');
                    document.getElementById('<%=txtteldom.ClientID %>').focus();
                   
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                //var control = document.getElementById('tmailinfocli').value.trim();
                //if (control.length > 0) {
                //    if (!validarVariosEmail(control)) {
                //        alert('Mail no parece correcto');
                //        document.getElementById('mailopcional').innerHTML = '<span class="obligado">Mail no parece correcto</span>';
                //        document.getElementById('tmailinfocli').focus();
                //        return false;
                //    }
                //}
                document.getElementById('mailopcional').innerHTML = ' * opcional';

  
                var vals = document.getElementById('<%=txtlugarnacimiento.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Escriba el Lugar de Nacimiento *');
                    document.getElementById('<%=txtlugarnacimiento.ClientID %>').focus();
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (!valedad()) {
                    return false;
                };

                var vals = document.getElementById('<%=ddlTipoEmpresa.ClientID %>').value;
                if (vals == 12) {
                    //var vals = document.getElementById('<%=ddlTipoLicencia.ClientID %>').value;
                    var vals = document.getElementById('<%=ddlTipoLicencia.ClientID %>').options[document.getElementById("<%=ddlTipoLicencia.ClientID%>").selectedIndex].text;
                    if (vals == '* Elija el tipo de licencia *')
                    //if (vals == 0) {
                    {
                        alert('* Seleccione el Tipo de Licencia *');
                        document.getElementById('<%=ddlTipoLicencia.ClientID %>').focus();
                     
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                    var vals = document.getElementById('<%=txtfecexplic.ClientID %>').value;
                    if (vals == '' || vals == null || vals == undefined) {
                        alert('* Escriba la Fecha de Expiración de la Licencia *');
                        document.getElementById('<%=txtfecexplic.ClientID %>').focus();
                      
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                }
                var vals = document.getElementById('<%=txtcargo.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Escriba el Cargo *');
                    document.getElementById('<%=txtcargo.ClientID %>').focus();
                
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                document.getElementById('<%=txtcargo.ClientID %>').disabled = false;
                document.getElementById("loader").className = '';
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema..:\n' + e.Message);
            }
        }
        function valedad() {
            var codigo;
            codigo = document.getElementById('<%=txtfechanacimiento.ClientID %>').value;
            if (codigo.length <= 0) {
                alert('* Escoja la Fecha de Nacimiento *');
                document.getElementById('<%=txtfechanacimiento.ClientID %>').focus();
                <%--document.getElementById('<%=txtfechanacimiento.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400;";--%>
                document.getElementById("loader").className = 'nover';
                return false;
            }
            return true;
        }

        function validNumericos(inputtxt) {
            var valor = inputtxt.value;
                 for(i=0;i<valor.length;i++){
                     var code=valor.charCodeAt(i);
                         if(code<=48 || code>=57){          
                           return;
                         }    
                   }
        }

        function valcipasservidor() {
           

            var valruccipas = document.getElementById('<%=txtcipas.ClientID %>').value;
            var vci = document.getElementById('rbci').checked;
            var vpas = document.getElementById('rbpasaporte').checked;
            if (vci == true) {

                if (!/^([0-9])*$/.test(valruccipas)) {
                    alert('* No. C.I. No es un Numero *');
                    document.getElementById('<%=txtcipas.ClientID %>').focus();
                   
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                else if (valruccipas == '' || valruccipas == null || valruccipas == undefined) {
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

                if (digito != 0) {
                    if (final != digito) {
                        alert('* Solicitud de Credencial/Permiso Provisional: *\n * No. C.I. no válido! *');
                        document.getElementById('<%=txtcipas.ClientID %>').focus();
                       
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                }
                else {
                    if (final != 10) {
                        alert('* Solicitud de Credencial/Permiso Provisional: *\n * No. C.I. no válido! *');
                        document.getElementById('<%=txtcipas.ClientID %>').focus();
                      
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                }
            }
            if (vpas == true) {
              
                if (valruccipas == '' || valruccipas == null || valruccipas == undefined) {
                    alert('* Solicitud de Credencial/Permiso Provisional: *\n * Escriba el No. de Pasaporte *');
                    document.getElementById('<%=txtcipas.ClientID %>').focus();

                    document.getElementById("loader").className = 'nover';
                    return false;
                }
            }
            return true;
        }
        function redireccionar() {
            window.locationf = "~/cuenta/menu.aspx";
        }
        function redirectcatdoc(val) {
            var caja = val;
            //window.open('../credenciales/documentos/?placa=' + caja, 'name', 'width=850,height=480')
            window.open('../cliente/solicitudcolaboradordocumentos.aspx?CIPAS=' + caja, 'name', 'width=950,height=540')
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
            document.getElementById('<%=txtcargo.ClientID %>').value = objeto.cargo;
            document.getElementById('<%=ddlTipoLicencia.ClientID %>').value = objeto.tiplic;
            document.getElementById('<%=txtfecexplic.ClientID %>').value = objeto.fecexplic;
            document.getElementById('valnombre').textContent = "";
            document.getElementById('valapellido').textContent = "";
            document.getElementById('valcipas').textContent = "";
            document.getElementById('valtipsangre').textContent = "";
            document.getElementById('valdirdom').textContent = "";
            document.getElementById('valtelofi').textContent = "";
            //                document.getElementById('mailopcional').textContent = "";
            document.getElementById('vallugarnac').textContent = "";
            document.getElementById('valfecnac').textContent = "";
            document.getElementById('valfecexplic').textContent = "";
            document.getElementById('valtipolic').textContent = "";
            document.getElementById('valcargo').textContent = "";

            <%--document.getElementById('<%=txtnombres.ClientID %>').style.cssText = "background-color:White;width:400px;";
            document.getElementById('<%=txtapellidos.ClientID %>').style.cssText = "background-color:White;width:400px;";
            document.getElementById('<%=txtcipas.ClientID %>').style.cssText = "background-color:White;width:200px;";

            document.getElementById('<%=txttiposangre.ClientID %>').style.cssText = "background-color:White;width:200px;";
            document.getElementById('<%=txtdirdom.ClientID %>').style.cssText = "background-color:White;width:400px;";
            document.getElementById('<%=txtteldom.ClientID %>').style.cssText = "background-color:White;width:200px;";
            document.getElementById('<%=txtlugarnacimiento.ClientID %>').style.cssText = "background-color:White;width:200px;";
            document.getElementById('<%=txtfechanacimiento.ClientID %>').style.cssText = "background-color:White;width:200px;";--%>

//            document.getElementById('<%=ddlTipoLicencia.ClientID %>').value = "0";
//            document.getElementById('<%=ddlTipoLicencia.ClientID %>').disabled = false;
//            document.getElementById('<%=ddlTipoLicencia.ClientID %>').style.cssText = "background-color:White;width:200px;";        
            fEmpresa();
//            document.getElementById('<%=txtcargo.ClientID %>').value = "";
//            document.getElementById('<%=txtcargo.ClientID %>').disabled = false;
//            document.getElementById('<%=txtcargo.ClientID %>').style.cssText = "background-color:White;width:200px;";
//            document.getElementById('<%=txtfecexplic.ClientID %>').value = "";
//            document.getElementById('<%=txtfecexplic.ClientID %>').disabled = false;
//            document.getElementById('<%=txtfecexplic.ClientID %>').style.cssText = "background-color:White;width:200px;";
        }
        function fvalidaCriterios() {
            var renovacion = document.getElementById('<%=rbrenovacion.ClientID %>').checked;
            if (renovacion) {
                var vals = document.getElementById('<%=ddlTipoEmpresa.ClientID %>').value;
                if (vals == 0) {
                    alert('Seleccione el Tipo de Empresa');
                    document.getElementById('<%=ddlTipoEmpresa.ClientID %>').focus();
                    <%--document.getElementById('<%=ddlTipoEmpresa.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=cbltiposolicitud.ClientID %>').value;
                if (vals == 0) {
                    alert('Seleccione al menos un Tipo de Solicitud');
                    document.getElementById('<%=cbltiposolicitud.ClientID %>').focus();
                    <%--document.getElementById('<%=cbltiposolicitud.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
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
                document.getElementById('<%=txtcriterioconsulta.ClientID %>').value = document.getElementById('<%=txtcriterioconsulta.ClientID %>').value.toUpperCase();
            }
            else {
                alert('Solo puede buscar cuando es una Renovación de Credencial.');
                document.getElementById('<%=txtcriterioconsulta.ClientID %>').focus();
                <%--document.getElementById('<%=txtcriterioconsulta.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";   --%>             
                return false;
            }
            return true;
        }
        function fValidaRenovacion() {
            var renovacion = document.getElementById('<%=rbrenovacion.ClientID %>').checked;
            if (renovacion) {
                document.getElementById('<%=txtcriterioconsulta.ClientID %>').focus();
                document.getElementById('<%=txtcriterioconsulta.ClientID %>').value = "";
                document.getElementById('<%=txtcipas.ClientID %>').disabled = true;
                <%--document.getElementById('<%=txtcipas.ClientID %>').style.cssText = "background-color:Gray;width:200px;";--%>
                document.getElementById('<%=txtnombres.ClientID %>').disabled = true;
                <%--document.getElementById('<%=txtnombres.ClientID %>').style.cssText = "background-color:Gray;width:400px;";--%>
                document.getElementById('<%=txtapellidos.ClientID %>').disabled = true;
                <%--document.getElementById('<%=txtapellidos.ClientID %>').style.cssText = "background-color:Gray;width:400px;";--%>

                document.getElementById('<%=txtNota.ClientID %>').disabled = true;
                <%--document.getElementById('<%=txtNota.ClientID %>').style.cssText = "background-color:Gray;width:400px;";--%>

                document.getElementById('<%=txttiposangre.ClientID %>').disabled = true;
                <%--document.getElementById('<%=txttiposangre.ClientID %>').style.cssText = "background-color:Gray;width:200px;";--%>
                document.getElementById('<%=txtdirdom.ClientID %>').disabled = true;
                <%--document.getElementById('<%=txtdirdom.ClientID %>').style.cssText = "background-color:Gray;width:400px;";--%>
                document.getElementById('<%=txtteldom.ClientID %>').disabled = true;
                <%--document.getElementById('<%=txtteldom.ClientID %>').style.cssText = "background-color:Gray;width:200px;";--%>
                document.getElementById('<%=tmailinfocli.ClientID %>').disabled = true;
                <%--document.getElementById('<%=tmailinfocli.ClientID %>').style.cssText = "background-color:Gray;width:400px;";--%>
                document.getElementById('<%=txtlugarnacimiento.ClientID %>').disabled = true;
                <%--document.getElementById('<%=txtlugarnacimiento.ClientID %>').style.cssText = "background-color:Gray;width:200px;";--%>
                document.getElementById('<%=txtfechanacimiento.ClientID %>').disabled = true;
                <%--document.getElementById('<%=txtfechanacimiento.ClientID %>').style.cssText = "background-color:Gray;width:200px;";--%>
                document.getElementById('<%=txtcargo.ClientID %>').disabled = true;
                <%--document.getElementById('<%=txtcargo.ClientID %>').style.cssText = "background-color:Gray;width:200px;";--%>
                document.getElementById('<%=ddlTipoLicencia.ClientID %>').disabled = true;
                <%--document.getElementById('<%=ddlTipoLicencia.ClientID %>').style.cssText = "background-color:Gray;width:200px;";--%>
                document.getElementById('<%=txtfecexplic.ClientID %>').disabled = true;
                <%--document.getElementById('<%=txtfecexplic.ClientID %>').style.cssText = "background-color:Gray;width:200px;";--%>
                document.getElementById('<%=ddlTipoLicencia.ClientID %>').value = "0";
//                document.getElementById('<%=txtcargo.ClientID %>').disabled = true;
//                document.getElementById('<%=txtcargo.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtcipas.ClientID %>').value = "";
                document.getElementById('<%=txtnombres.ClientID %>').value = "";
                document.getElementById('<%=txtapellidos.ClientID %>').value = "";
                document.getElementById('<%=txttiposangre.ClientID %>').value = "";
                document.getElementById('<%=txtdirdom.ClientID %>').value = "";
                document.getElementById('<%=txtteldom.ClientID %>').value = "";
                document.getElementById('<%=tmailinfocli.ClientID %>').value = "";
                document.getElementById('<%=txtlugarnacimiento.ClientID %>').value = "";
                document.getElementById('<%=txtfechanacimiento.ClientID %>').value = "";
                document.getElementById('<%=txtcargo.ClientID %>').value = "";
                document.getElementById('<%=ddlTipoLicencia.ClientID %>').value = "";
                document.getElementById('<%=txtfecexplic.ClientID %>').value = "";
                document.getElementById('valnombre').textContent = "";
                document.getElementById('valapellido').textContent = "";
                document.getElementById('valcipas').textContent = "";
                document.getElementById('valtipsangre').textContent = "";
                document.getElementById('valdirdom').textContent = "";
                document.getElementById('valtelofi').textContent = "";
//                document.getElementById('mailopcional').textContent = "";
                document.getElementById('vallugarnac').textContent = "";
                document.getElementById('valfecnac').textContent = "";

                document.getElementById('valfecexplic').textContent = "";
                document.getElementById('valtipolic').textContent = "";
                document.getElementById('valcargo').textContent = "";

                document.getElementById('<%=ddlTipoLicencia.ClientID %>').value = "0";
                document.getElementById('<%=ddlTipoLicencia.ClientID %>').disabled = true;
                <%--document.getElementById('<%=ddlTipoLicencia.ClientID %>').style.cssText = "background-color:Gray;width:200px;";--%>
                document.getElementById('<%=txtcargo.ClientID %>').value = "";
                document.getElementById('<%=txtcargo.ClientID %>').disabled = true;
                <%--document.getElementById('<%=txtcargo.ClientID %>').style.cssText = "background-color:Gray;width:200px;";--%>
//                fEmpresa();
            }
        }
        function fValidaEmision() {
            var emision = document.getElementById('<%=rbemision.ClientID %>').checked;
            if (emision) {
                
                document.getElementById('<%=txtcriterioconsulta.ClientID %>').value = "";
                document.getElementById('<%=txtcipas.ClientID %>').disabled = false;
                <%--document.getElementById('<%=txtcipas.ClientID %>').style.cssText = "background-color:White;width:200px;";--%>
                document.getElementById('<%=txtnombres.ClientID %>').disabled = false;
                <%--document.getElementById('<%=txtnombres.ClientID %>').style.cssText = "background-color:White;width:400px;";--%>
                document.getElementById('<%=txtapellidos.ClientID %>').disabled = false;
                <%--document.getElementById('<%=txtapellidos.ClientID %>').style.cssText = "background-color:White;width:400px;";--%>

                document.getElementById('<%=txtNota.ClientID %>').disabled = false;
                <%--document.getElementById('<%=txtNota.ClientID %>').style.cssText = "background-color:White;width:400px;";--%>

                document.getElementById('<%=txttiposangre.ClientID %>').disabled = false;
                <%--document.getElementById('<%=txttiposangre.ClientID %>').style.cssText = "background-color:White;width:200px;";--%>
                document.getElementById('<%=txtdirdom.ClientID %>').disabled = false;
                <%--document.getElementById('<%=txtdirdom.ClientID %>').style.cssText = "background-color:White;width:400px;";--%>
                document.getElementById('<%=txtteldom.ClientID %>').disabled = false;
                <%--document.getElementById('<%=txtteldom.ClientID %>').style.cssText = "background-color:White;width:200px;";--%>
                document.getElementById('<%=tmailinfocli.ClientID %>').disabled = false;
                <%--document.getElementById('<%=tmailinfocli.ClientID %>').style.cssText = "background-color:White;width:400px;";--%>
                document.getElementById('<%=txtlugarnacimiento.ClientID %>').disabled = false;
                <%--document.getElementById('<%=txtlugarnacimiento.ClientID %>').style.cssText = "background-color:White;width:200px;";--%>
                document.getElementById('<%=txtfechanacimiento.ClientID %>').disabled = false;
                <%--document.getElementById('<%=txtfechanacimiento.ClientID %>').style.cssText = "background-color:White;width:200px;";--%>
                document.getElementById('<%=txtcargo.ClientID %>').disabled = true;
                <%--document.getElementById('<%=txtcargo.ClientID %>').style.cssText = "background-color:Gray;width:200px;";--%>
                document.getElementById('<%=txtfecexplic.ClientID %>').disabled = false;
                <%--document.getElementById('<%=txtfecexplic.ClientID %>').style.cssText = "background-color:White;width:200px;";--%>
                document.getElementById('<%=txtcipas.ClientID %>').value = "";
                document.getElementById('<%=txtnombres.ClientID %>').value = "";
                document.getElementById('<%=txtapellidos.ClientID %>').value = "";
                document.getElementById('<%=txttiposangre.ClientID %>').value = "";
                document.getElementById('<%=txtdirdom.ClientID %>').value = "";
                document.getElementById('<%=txtteldom.ClientID %>').value = "";
                document.getElementById('<%=tmailinfocli.ClientID %>').value = "";
                document.getElementById('<%=txtlugarnacimiento.ClientID %>').value = "";
                document.getElementById('<%=txtfechanacimiento.ClientID %>').value = "";
                document.getElementById('<%=txtcargo.ClientID %>').value = "";
                document.getElementById('<%=ddlTipoLicencia.ClientID %>').value = "";
                document.getElementById('<%=txtfecexplic.ClientID %>').value = "";
                document.getElementById('<%=txtcipas.ClientID %>').focus();
                document.getElementById('valnombre').textContent = " * obligatorio";
                document.getElementById('valapellido').textContent = " * obligatorio";
                document.getElementById('valcipas').textContent = " * obligatorio";
                document.getElementById('valtipsangre').textContent = " * obligatorio";
                document.getElementById('valdirdom').textContent = " * obligatorio";
                document.getElementById('valtelofi').textContent = " * obligatorio";
//                document.getElementById('mailopcional').textContent = " * obligatorio";
                document.getElementById('vallugarnac').textContent = " * obligatorio";
                document.getElementById('valfecnac').textContent = " * obligatorio";
                document.getElementById('valfecexplic').textContent = " * obligatorio";
                document.getElementById('valtipolic').textContent = " * obligatorio";
                document.getElementById('valcargo').textContent = " * obligatorio";

                document.getElementById('<%=ddlTipoLicencia.ClientID %>').value = "0";
                document.getElementById('<%=ddlTipoLicencia.ClientID %>').disabled = true;
                <%--document.getElementById('<%=ddlTipoLicencia.ClientID %>').style.cssText = "background-color:Gray;width:200px;";--%>
//                document.getElementById('<%=txtcargo.ClientID %>').value = "";
//                document.getElementById('<%=txtcargo.ClientID %>').disabled = true;
                //                document.getElementById('<%=txtcargo.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                fEmpresa();
            }
        }
        function fIrAlCampoCriterioDeBusqueda() {
            document.getElementById('<%=txtcriterioconsulta.ClientID %>').focus();
            //            document.getElementById('<%=txtcriterioconsulta.ClientID %>').value = "";
            fValidaRenovacion();
        } 
        function rbemision_onclick() {

        }
        function fValidaTipLic() {
//            var vals = document.getElementById('<%=ddlTipoLicencia.ClientID %>').value;
//            if (vals == 'E' || vals == 'G') {
                document.getElementById('<%=txtcargo.ClientID %>').value = "CONDUCTOR";
                <%--document.getElementById('<%=txtcargo.ClientID %>').style.cssText = "background-color:White;width:200px;";--%>
                document.getElementById('<%=txtcargo.ClientID %>').disabled = true;
                document.getElementById('valtipolic').textContent = "";
//            }
//            else {
//                document.getElementById('<%=txtcargo.ClientID %>').value = "";
//                document.getElementById('<%=txtcargo.ClientID %>').disabled = false;
//                document.getElementById('<%=txtcargo.ClientID %>').style.cssText = "background-color:White;width:200px;";
//                document.getElementById('valtipolic').textContent = " * obligatorio";
//            }
        }
        function fEmpresa() {
            var vals = document.getElementById('<%=ddlTipoEmpresa.ClientID %>').value;
            if (vals == 12) {
                document.getElementById('<%=ddlTipoLicencia.ClientID %>').disabled = false;
                document.getElementById('<%=txtfecexplic.ClientID %>').disabled = false;
                document.getElementById('<%=txtcargo.ClientID %>').disabled = true;
                document.getElementById('<%=txtcargo.ClientID %>').value = '';
                //                document.getElementById('<%=txtcargo.ClientID %>').style.cssText = "background-color:White;width:200px;";

                <%--document.getElementById('<%=ddlTipoLicencia.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('<%=txtfecexplic.ClientID %>').style.cssText = "background-color:White;width:200px;";--%>
                document.getElementById('valtipolic').textContent = " * obligatorio";
                document.getElementById('valfecexplic').textContent = " * obligatorio";
                document.getElementById('<%=ddlTipoLicencia.ClientID %>').value = "0";
                document.getElementById('<%=txtfecexplic.ClientID %>').value = "";
            }
            else {
                document.getElementById('<%=ddlTipoLicencia.ClientID %>').disabled = true;
                document.getElementById('<%=txtfecexplic.ClientID %>').disabled = true;
                document.getElementById('<%=txtcargo.ClientID %>').disabled = false;
                document.getElementById('<%=txtcargo.ClientID %>').value = '';
                //                document.getElementById('<%=txtcargo.ClientID %>').style.cssText = "background-color:Gray;width:200px;";

                <%--document.getElementById('<%=ddlTipoLicencia.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtfecexplic.ClientID %>').style.cssText = "background-color:Gray;width:200px;";--%>
                document.getElementById('valtipolic').textContent = "";
                document.getElementById('valfecexplic').textContent = "";
                document.getElementById('<%=ddlTipoLicencia.ClientID %>').value = "0";
                document.getElementById('<%=txtfecexplic.ClientID %>').value = "";
            }
        }
        function modalPanel() {
            document.getElementById('close').style.display = "block";
            window.location = '../cuenta/menu.aspx';
        }
        function move(incio, intervalo) {
            document.getElementById('divpb').style.display = "block";
            var elem = document.getElementById("myBar");
            var width = incio;
            var id = setInterval(frame, 15);
            function frame() {
                if (width >= intervalo) {
                    clearInterval(id);
                } else {
                    width++;
                    elem.style.width = width + '%';
                    document.getElementById("demo").innerHTML = width * 1 + '%';
                }
            }
            return true;
        }
    </script>
    <div id="ventana_popup" style=" display:none;">
    <div id="ventana_content-popup">
            <div>
             <table style=" width:100%">
             <tr><td align="center"><p id='manage_ventana_popup'><asp:Label Text="Procesando!" id="lblProgreso" runat="server" /></p></td></tr>
             </table>
		        <div id='borde_ventana_popup' style=" height:267px"> 
                <br /><br /><br /><br />
			    <div  style='text-align:center;' >
                    <div id="divpb">
                    <div class="w3-progress-container">
                        <div id="myBar" class="w3-progressbar w3-green" style="width:0%">
                        </div>
                    </div>
                    <p id="demo">0%</p>
                    </div>
			    </div>
			    <br/>
			     <div  id="close" style=" display:none" >SALIR</div>
  			    </div>
            </div>
        </div>
    </div>
    <div id="popup-overlay"></div>
</asp:Content>
