<%@ Page  Title="Emisión/Renovación de Permiso Vehicular" Language="C#" MasterPageFile="~/site.Master" MaintainScrollPositionOnPostback="true"
         AutoEventWireup="true" CodeBehind="solicitudvehiculo.aspx.cs" 
         Inherits="CSLSite.cliente.solicitudvehiculo" %>
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
</asp:Content>
<asp:Content ID="formcolaborador" ContentPlaceHolderID="placebody" runat="server">
    <input id="zonaid" type="hidden" value="7" />
 <%-- <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>--%>
  <asp:ScriptManager ID="sMan" EnableScriptGlobalization="true" runat="server"></asp:ScriptManager>
  <div>
        <asp:Timer ID="TimerPb" OnTick="TimerPb_Tick" Enabled="false" runat="server" Interval="10"></asp:Timer>
  </div>
  <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">MCA - Módulo de Control de Acceso</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Solicitar Emisión/Renovación de Registro de Vehículo (Pesado o Liviano)</li>
          </ol>
        </nav>
      </div>

 <div class="dashboard-container p-4" id="cuerpo" runat="server">

         <div class="form-row">
             <div class="form-title col-md-12">
             <a class="btn btn-outline-primary mr-4" href="#" target="_blank" runat="server" id="aprint" clientidmode="Static" >1</a>
             <a class="level1" target="_blank" runat="server" id="a1" clientidmode="Static" >Tipo de Empresa</a>
                <div class="colapser colapsa">
                </div>
             </div>
             <div class="form-group col-md-12 d-flex">
                 <label for="inputAddress"><span style="color: #FF0000; font-weight: bold;"> *</span></label>
                          <asp:DropDownList runat="server" ID="ddlTipoEmpresa" class="form-control" 
                                    onchange="valdltipsol(this, valtipempresa);" >
                                    <asp:ListItem Value="0">* Seleccione origen *</asp:ListItem>
                                </asp:DropDownList>
             <span id='valtipempresa' class="validacion" ></span>
            </div>
     <div class="colapser colapsa"></div>
    </div>

     <div class="form-row">
             <div class="form-title col-md-12">
             <a class="btn btn-outline-primary mr-4" href="#" target="_blank" runat="server" id="a2" clientidmode="Static" >2</a>
             <a class="level1" target="_blank" runat="server" id="a3" clientidmode="Static" >Tipo de Solicitud</a>
                <div class="colapser colapsa">
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
                <a class="level1" target="_blank" runat="server" id="a5" clientidmode="Static" >Datos del Vehículo(S)</a>
            </div>

            <div class="alert alert-danger col-md-12" style=" font-weight:bold">
                <span>Consideraciones para vehículos PESADOS/MONTACARGAS:</span>
                <br />
                <span style=" font-weight:normal">* Pintar el número de placa en los techos de los vehículos, de acuerdo a las normativas vigentes.</span>
                <br />
                <span style=" font-weight:normal">* Tener correctamente colocadas las placas otorgadas por la autoridad competente en la parte frontal y posterior del vehículo.</span>
                <br />
                <span style=" font-weight:normal">* El vehículo debe tener el Logotipo de la Organización de transporte a la que pertenece, pintado en lugar visible.</span>
                <br />
                <span style=" font-weight:normal">* La fecha de vigencia de la póliza determina la fecha hasta la cual el vehículo podrá ingresar a la Terminal.<br /></span>
               <span> Consideraciones para vehículos LIVIANOS:</span><br />
               <span style=" font-weight:normal"> * Luego de aceptada la solicitud de registro del vehículo, realizar la solicitud 
                de ingreso temporal a la Terminal desde la opción: Control de Acceso - Vehículos / Solicitar Acceso Temporal de Vehículos Livianos previamente registrados.</span>

            </div>
        </div>
        <div class="form-row">
        <div class="form-group col-md-6 d-flex">
            <label class="radiobutton-container" >
                Registro Nuevo Vehículo<input id="rbemision" runat="server" onclick="fValidaEmision();" checked="true" type="radio" name="deck" value="ci" clientidmode="Static"/>
                <span class="checkmark"></span></label>
        </div>

        <div class="form-group col-md-6 d-flex">
            <label class="radiobutton-container" > 
                Renovación Registro Vehículo<input id="rbrenovacion" onclick="fValidaRenovacion();" runat="server" type="radio" name="deck" value="pas" clientidmode="Static"/>
            <span class="checkmark"></span></label>
        </div> 

        <div class="form-group col-md-12 d-flex">
            <label for="inputAddress">Categoría del Vehículo:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
            <asp:DropDownList runat="server" ID="ddlCategoria" class="form-control" 
                    onchange="fCategoria();valdlcatveh(this, valcategoriavehiculo);" onselectedindexchanged="ddlCategoria_SelectedIndexChanged" >
                    <asp:ListItem Value="0">* Elija el tipo de categoría *</asp:ListItem>
            </asp:DropDownList>
            <span id="valcategoriavehiculo" class="validacion"> </span>
        </div>
        
        <div class="form-group col-md-6">
                 <label for="inputAddress">Área Destino/Actividad:</label>
                 </div>
                 <div class="form-group col-md-6">
                          <asp:DropDownList runat="server" ID="ddlActividadOnlyControl" onchange="validadropdownlist(this, valareaoc);" class="form-control" >
                                    <asp:ListItem Value="0">* Elija *</asp:ListItem>
                          </asp:DropDownList>
                       <span id='valareaoc' class="validacion" > </span>
            </div>

        <div class="form-group col-md-6">
            <label for="inputEmail4">Placa / No. Serie (para Montacargas):</label>
        </div>

        <div class="form-group col-md-6 d-flex">
            <asp:TextBox runat="server" id="txtPlaca" class="form-control" axLength="10" onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)" onblur="checkcaja(this,'valPlaca',true);"/> 
            <span id="valPlaca" class="validacion"> </span>
            <label >&nbsp;&nbsp;</label>
                <asp:Button ID="btnBuscar" runat="server" class="btn btn-outline-primary mr-4"
                    OnClientClick="return fvalidaPlaca();" onclick="btnBuscar_Click" Text="Buscar" clientidmode="Static"></asp:Button> 
        </div>

        <div class="form-group col-md-6">
            <label for="inputEmail4">Clase / Tipo:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
        </div>

        <div class="form-group col-md-6">
            <asp:TextBox runat="server" id="txtClaseTipo" class="form-control" MaxLength="10" onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890/-_ ',true)" onblur="checkcaja(this,'valClaseTipo',true);"/> 
            <span id="valClaseTipo" class="validacion"> </span>
        </div>

        <div class="form-group col-md-6">
            <label for="inputEmail4">Marca:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
        </div>

        <div class="form-group col-md-6">
            <asp:TextBox runat="server" id="txtMarca" class="form-control" MaxLength="10" onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)" onblur="checkcaja(this,'valMarca',true);"/>
            <span id="valMarca" class="validacion"> </span>
        </div>

        <div class="form-group col-md-6">
            <label for="inputEmail4">Modelo:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
        </div>

        <div class="form-group col-md-6">
            <asp:TextBox runat="server" id="txtModelo" class="form-control" MaxLength="10" onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)" onblur="checkcaja(this,'valModelo',true);"/>
            <span id="valModelo" class="validacion"> </span>
        </div>

        <div class="form-group col-md-6">
            <label for="inputEmail4">Color:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
        </div>

        <div class="form-group col-md-6">
            <asp:TextBox runat="server" id="txtColor" class="form-control" axLength="10" onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)" onblur="checkcaja(this,'valColor',true);"/> 
            <span id="valColor" class="validacion"> </span>
        </div>

        <div class="form-group col-md-6">
            <label for="inputEmail4">Tipo Certificado de Pesos y Medidas:</label>
        </div>

        <div class="form-group col-md-6">
            <asp:TextBox runat="server" id="txttipocertificado" class="form-control" disabled MaxLength="10" onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)" onblur="checkcaja(this,'valtipcer',true);"/>
            <span id="valtipcer" class="validacion"> </span>
        </div>

        <div class="form-group col-md-6">
            <label for="inputEmail4">Nùmero de Certificado de Pesos y Medidas:</label>
        </div>

        <div class="form-group col-md-6 d-flex">
            <asp:TextBox runat="server" id="txtnumcertificado" class="form-control" disabled MaxLength="10" onkeypress="return soloLetras(event,'0123456789',true)" onblur="checkcaja(this,'valnumcer',true);"/> 
            <span id="valnumcer" class="validacion"> </span>
        </div>
     
        <div class="form-group col-md-6">
                 <label for="inputAddress">Fecha de Vigencia Permiso MTOP:</label>
            </div>
                  <div class="form-group col-md-6">
                 <asp:TextBox ID="txtfechamtop" runat="server" MaxLength="10" CssClass="datetimepicker form-control"
                    onkeypress="return soloLetras(event,'01234567890/')" style="text-align:center"
                    onblur="checkcaja(this,'valfecmtop',true);" ClientIDMode="Static" Enabled="false"></asp:TextBox> 
                      <span id="valfecmtop" class="validacion"> </span>
                  
            </div>

             <div class="form-group col-md-6">
                 <label for="inputAddress">Fecha de Vigencia de Póliza:</label>
                 </div>
             <div class="form-group col-md-6">
                      <asp:TextBox ID="txtfechapoliza" runat="server" MaxLength="15" CssClass="datetimepicker form-control"
                        onkeypress="return soloLetras(event,'01234567890/')" style="text-align:center" 
                        onblur="checkcaja(this,'valfecpol',true);" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                      <span id="valfecpol" class="validacion"> </span>
                  
            </div>

             <div class="form-group col-md-6">
                 <label for="inputAddress">Nota:<span style="color: #FF0000; font-weight: bold;"> </span></label>
                 </div>
             <div class="form-group col-md-6">
                 <asp:TextBox ID="txtNota" runat="server" MaxLength="3000" TextMode="MultiLine" Heigth="60px" class="form-control" style="overflow:auto;resize:none"
                              onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 /_-.',true)"/>
             
            </div>

             <div class="form-group col-md-12">
                            <asp:Button ID="btnAgregar" runat="server" Text="Agregar" class="btn btn-outline-primary mr-4"
                                OnClientClick="return validaCabecera();" onclick="btnAgregar_Click"/>
                 <span id="imgagrega" class="fa fa-plus-square-0"></span>
                <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"/>
             </div> 

      <div class="cataresult" >
            <asp:UpdatePanel ID="upresult2" runat="server"  >
      <ContentTemplate>
      <script type="text/javascript">          Sys.Application.add_load(BindFunctions);</script>
                    <div class="informativo" id="colector">
      <table runat="server" id="tableexpo">
      <tr><td>

              <asp:GridView runat="server" id="gvVehiculos" class="table table-bordered invoice" AutoGenerateColumns="False" Width="100%"
                        onrowcommand="gvVehiculos_RowCommand" onrowdeleting="gvVehiculos_RowDeleting">
              <Columns>
              <asp:TemplateField HeaderText="IdTipoEmpresa" Visible="False">
                      <ItemTemplate>
                          <%--<asp:TextBox ID="tplaca" runat="server" Width="70px" Text='<%# Eval("Placa") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)"></asp:TextBox>--%>
                          <asp:Label ID="tidtipemp"  style="text-transform :uppercase"  runat="server" Text='<%# Eval("IdTipoEmpresa") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)"></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="IdCategoria" Visible="False">
                      <ItemTemplate>
                          <%--<asp:TextBox ID="tplaca" runat="server" Width="70px" Text='<%# Eval("Placa") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)"></asp:TextBox>--%>
                          <asp:Label ID="tidcategoria"  style="text-transform :uppercase"  runat="server" Text='<%# Eval("IdCategoria") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)"></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                 <asp:TemplateField HeaderText="Placa">
                      <ItemTemplate>
                          <%--<asp:TextBox ID="tplaca" runat="server" Width="70px" Text='<%# Eval("Placa") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)"></asp:TextBox>--%>
                          <asp:Label ID="tplaca"  style="text-transform :uppercase" ToolTip='<%# Eval("Placa") %>' runat="server" Text='<%# Eval("Placa") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)"></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Categoría">
                      <ItemTemplate>
                          <%--<asp:TextBox ID="tplaca" runat="server" Width="70px" Text='<%# Eval("Placa") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)"></asp:TextBox>--%>
                          <asp:Label ID="tcategoria" ToolTip='<%# Eval("DesCategoria") %>'  style="text-transform :uppercase"  runat="server" Text='<%# Eval("DesCategoria") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)"></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Area">
                      <ItemTemplate>
                          <%--<asp:TextBox ID="tplaca" runat="server" Width="70px" Text='<%# Eval("Placa") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)"></asp:TextBox>--%>
                          <asp:Label ID="tareaactv" ToolTip='<%# Eval("Area") %>'  style="text-transform :uppercase"  runat="server" Text='<%# Eval("Area") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)"></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Clase/Tipo">
                      <ItemTemplate>
                          <asp:Label ID="tclasetipo" ToolTip='<%# Eval("ClaseTipo") %>'  runat="server" Text='<%# Eval("[ClaseTipo]") %>' 
                          onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 -/_.',true)"></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Marca">
                      <ItemTemplate>
                          <asp:Label ID="tmarca" runat="server"  ToolTip='<%# Eval("Marca") %>' Text='<%# Eval("[Marca]") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 -/_.',true)"></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Modelo">
                      <ItemTemplate>
                          <asp:Label ID="tmodelo" runat="server"  ToolTip='<%# Eval("Modelo") %>' Text='<%# Eval("[Modelo]") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 -/_.',true)"></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Nota">
                      <ItemTemplate>
                      <div style="overflow-y:scroll; overflow-x: hidden; height:30px; text-align:center">
                          <asp:Label ID="tnota"  style="text-transform :uppercase" runat="server" ToolTip='<%# Eval("Nota") %>' Text='<%# Eval("[Nota]") %>'></asp:Label>
                      </div>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Color" Visible="False">
                      <ItemTemplate>
                          <asp:Label ID="tcolor" runat="server" Text='<%# Eval("[Color]") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)"></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="NO Cargados" Visible="false">
                      <ItemTemplate>
                          <asp:CheckBox ID="chkEstadoDocumentos" runat="server" Enabled="false"/>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField ShowHeader="False" HeaderText="Adjuntar">
                        <ItemTemplate>
                            <a id="adjDoc" class="btn btn-outline-primary mr-4" onclick="redirectcatdoc('<%# Eval("Placa") %>', '<%# Eval("IdTipoEmpresa") %>', '<%# Eval("IdCategoria") %>');" >
                            <i class="fa fa-search" ></i> Documentos </a>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField ShowHeader="False" Visible="false">
                        <ItemTemplate>
                            <asp:Button ID="btnAdd" runat="server" CausesValidation="False" CommandArgument="Add"
                                CommandName="Add" Text="Agregar" CssClass="btn btn-outline-primary mr-4"/>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:Button ID="btnDelete" runat="server" CausesValidation="False" 
                                CommandName="Delete" Text="Eliminar" CssClass="btn btn-outline-primary mr-4" />
                        </ItemTemplate>
                  </asp:TemplateField>
              </Columns>
          </asp:GridView>

       </td></tr>
       </table>
       </div>
        </ContentTemplate>
      <Triggers>
        <asp:AsyncPostBackTrigger ControlID="hfupdatepanel" />
        <asp:AsyncPostBackTrigger ControlID="btsalvar" />
      </Triggers>
     </asp:UpdatePanel>
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
         <asp:UpdatePanel ID="UpdatePanelpb" UpdateMode="Conditional" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="TimerPb" EventName="Tick" />
        </Triggers>
        
            <ContentTemplate>
                   <asp:Button ID="btsalvar" runat="server" Text="Enviar Solicitud" OnClientClick="return prepareObject('¿Esta seguro de enviar la solicitud?');" class="btn btn-primary" 
               onclick="btsalvar_Click" ToolTip="Confirma la información y genera el envio de la solicitud." ClientIDMode="Static"/>
            
             </ContentTemplate>
        </asp:UpdatePanel>
            </div>
     </div>
    </div>
   <asp:HiddenField runat="server" ID="emailsec2" />
   <asp:HiddenField runat="server" ID="hfupdatepanel" />
   <asp:HiddenField runat="server" ID="hfcategoria" />
   <asp:HiddenField runat="server" ID="categoriaveh" />
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
        function validadropdownlistveh(control, validador) {
            if (control.value != 0) {
               
                validador.innerHTML = '';
                return;
            }
            
            validador.innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
            return;
        }
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
                
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=cbltiposolicitud.ClientID %>').value;
                if (vals == 0) {
                    alert('* Seleccione al menos un Tipo de Solicitud *');
                    document.getElementById('<%=cbltiposolicitud.ClientID %>').focus();
                   
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                document.getElementById('<%=ddlTipoEmpresa.ClientID %>').focus();
                return true;
                
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }
        function validaCabecera() {
            try {
                var vals = document.getElementById('<%=ddlTipoEmpresa.ClientID %>').value;
                if (vals == 0) {
                    alert('Seleccione al menos un Tipo de Empresa.');
                    document.getElementById('<%=ddlTipoEmpresa.ClientID %>').focus();
                 
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=cbltiposolicitud.ClientID %>').value;
                if (vals == 0) {
                    alert('Seleccione al menos un Tipo de Solicitud.');
                    document.getElementById('<%=cbltiposolicitud.ClientID %>').focus();
                    
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var emision = document.getElementById('<%=rbemision.ClientID %>').checked;
                var renovacion = document.getElementById('<%=rbrenovacion.ClientID %>').checked;
                if (emision == false && renovacion == false) {
                    alert('Indique si es un Registro Nuevo Vehículo o Renovación Registro Vehículo.');
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtPlaca.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('Escriba la Placa o No. Serie (para Montacargas).');
                    document.getElementById('<%=txtPlaca.ClientID %>').focus();
                   
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtMarca.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('Escriba la Marca del vehiculo.');
                    document.getElementById('<%=txtMarca.ClientID %>').focus();
                  
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtColor.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('Escriba el Color del vehiculo.');
                    document.getElementById('<%=txtColor.ClientID %>').focus();
                   
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=ddlCategoria.ClientID %>').value;
                if (vals == 0) {
                    alert('Seleccione la Categoria del Vehiculo.');
                    document.getElementById('<%=ddlCategoria.ClientID %>').focus();
                   
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (vals == 'CAT') {
                    var vals = document.getElementById('<%=txttipocertificado.ClientID %>').value;
                    if (vals == '' || vals == null || vals == undefined) {
                        alert('Escriba el Tipo de Certificado.');
                        document.getElementById('<%=txttipocertificado.ClientID %>').focus();
                      
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                    var vals = document.getElementById('<%=txtnumcertificado.ClientID %>').value;
                    if (vals == '' || vals == null || vals == undefined) {
                        alert('Escriba el Nùmero de Certificado.');
                        document.getElementById('<%=txtnumcertificado.ClientID %>').focus();
                       
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                    vals = document.getElementById('<%=txtfechapoliza.ClientID %>').value;
                    if (/*!validarFecha(vals)*/vals == '' || vals == null || vals == undefined) {
                        alert('Seleccione la Fecha de Poliza.');
                        document.getElementById('<%=txtfechapoliza.ClientID %>').focus();
                       
                        document.getElementById("loader").className = 'nover';
                        return false;
                    };
                    vals = document.getElementById('<%=txtfechamtop.ClientID %>').value;
                    if (/*!validarFecha(vals)*/vals == '' || vals == null || vals == undefined) {
                        alert('Seleccione la Fecha de MTOP.');
                        document.getElementById('<%=txtfechamtop.ClientID %>').focus();
                      
                        document.getElementById("loader").className = 'nover';
                        return false;
                    };
                }
                else if (vals == 'MON') {
                    var vals = document.getElementById('<%=txtPlaca.ClientID %>').value;
                    if (vals == '' || vals == null || vals == undefined) {
                        alert('Escriba la Placa o No. Serie (para Montacargas).');
                        document.getElementById('<%=txtPlaca.ClientID %>').focus();
                      
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                    var vals = document.getElementById('<%=txtMarca.ClientID %>').value;
                    if (vals == '' || vals == null || vals == undefined) {
                        alert('Escriba la Marca del vehiculo.');
                        document.getElementById('<%=txtMarca.ClientID %>').focus();
                     
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                    var vals = document.getElementById('<%=txtColor.ClientID %>').value;
                    if (vals == '' || vals == null || vals == undefined) {
                        alert('Escriba el Color del vehiculo.');
                        document.getElementById('<%=txtColor.ClientID %>').focus();
                     
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                }
                else {
                    var vals = document.getElementById('<%=txtClaseTipo.ClientID %>').value;
                    if (vals == '' || vals == null || vals == undefined) {
                        alert('Escriba la Clase/Tipo del vehiculo.');
                        document.getElementById('<%=txtClaseTipo.ClientID %>').focus();
                    
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                    var vals = document.getElementById('<%=txtModelo.ClientID %>').value;
                    if (vals == '' || vals == null || vals == undefined) {
                        alert('Escriba el Modelo del vehiculo.');
                        document.getElementById('<%=txtModelo.ClientID %>').focus();
                  
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                    var vals = document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').value;
                    if (vals == 0) {
                        alert('Seleccione el Área Destino/Actividad.');
                        document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').focus();
                        
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                }
                if (document.getElementById('<%=ddlCategoria.ClientID %>').value == 'CAT') {
                    document.getElementById('<%=txttipocertificado.ClientID %>').disabled = false;
                    document.getElementById('<%=txtnumcertificado.ClientID %>').disabled = false;
                    document.getElementById('<%=txtfechamtop.ClientID %>').disabled = false;
                    document.getElementById('<%=txtfechapoliza.ClientID %>').disabled = false;
                }
                document.getElementById('<%=ddlCategoria.ClientID %>').disabled = false;
                document.getElementById('<%=txtMarca.ClientID %>').disabled = false;
                document.getElementById('<%=txtClaseTipo.ClientID %>').disabled = false;
                document.getElementById('<%=txtModelo.ClientID %>').disabled = false;
                document.getElementById('<%=txtColor.ClientID %>').disabled = false;
                document.getElementById("loader").className = '';
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }
        function popupCallback(objeto, catalogo) {
            if (objeto == null || objeto == undefined) {
                alert('Hubo un problema al setaar un objeto de catalogo');
                return;
            }
            document.getElementById('<%=txtPlaca.ClientID %>').value = objeto.placa;
            document.getElementById('<%=txtMarca.ClientID %>').value = objeto.marca;
            document.getElementById('<%=txtClaseTipo.ClientID %>').value = objeto.clasetipo;
            document.getElementById('<%=txtModelo.ClientID %>').value = objeto.modelo;
            document.getElementById('<%=txtColor.ClientID %>').value = objeto.color;
            document.getElementById('<%=txttipocertificado.ClientID %>').value = objeto.tipocertificado;
            document.getElementById('<%=txtnumcertificado.ClientID %>').value = objeto.certificado;
            document.getElementById('<%=txtfechamtop.ClientID %>').value = objeto.fechamtop.substr(0, 10);
            document.getElementById('<%=txtfechapoliza.ClientID %>').value = objeto.fechapoliza.substr(0, 10);
            document.getElementById('<%=ddlCategoria.ClientID %>').value = objeto.tipocategoria;
           
            document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').value = "0";
            if (objeto.tipocategoria == 'LIV') {
                
                document.getElementById('<%=hfcategoria.ClientID %>').value = objeto.tipocategoria;
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').disabled = false;
               
                document.getElementById('<%=txttipocertificado.ClientID %>').value   = "";
                document.getElementById('<%=txtnumcertificado.ClientID %>').value = "";
                document.getElementById('<%=txtfechamtop.ClientID %>').value = "";
                document.getElementById('<%=txtfechapoliza.ClientID %>').value = "";
                document.getElementById('<%=txttipocertificado.ClientID %>').disabled = true;
                document.getElementById('<%=txtnumcertificado.ClientID %>').disabled = true;
                document.getElementById('<%=txtfechamtop.ClientID %>').disabled = true;
                document.getElementById('<%=txtfechapoliza.ClientID %>').disabled = true;
            }
            if (objeto.tipocategoria == 'CAT' || objeto.tipocategoria == 'MON') {
              
                document.getElementById('<%=hfcategoria.ClientID %>').value = objeto.tipocategoria;
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').disabled = true;
               
                document.getElementById('<%=txttipocertificado.ClientID %>').disabled = false;
                document.getElementById('<%=txtnumcertificado.ClientID %>').disabled = false;
                document.getElementById('<%=txtfechamtop.ClientID %>').disabled = false;
                document.getElementById('<%=txtfechapoliza.ClientID %>').disabled = false;
            }
            document.getElementById('valcategoriavehiculo').textContent = "";
            document.getElementById('valClaseTipo').textContent = "";
            document.getElementById('valMarca').textContent = "";
            document.getElementById('valModelo').textContent = "";
            document.getElementById('valColor').textContent = "";
            document.getElementById('valfecmtop').textContent = "";
            document.getElementById('valfecpol').textContent = "";
            document.getElementById('valtipcer').textContent = "";
            document.getElementById('valnumcer').textContent = "";
            document.getElementById('<%=txtNota.ClientID %>').disabled = false;
           
        }
        function redireccionar() {
            window.locationf = "../cuenta/menu.aspx";
        }
        function redirectcatdoc(val, val2, val3) {
            var caja = val;
            var tipoempresa = val2;
            var categoria = val3;
            //window.open('../credenciales/documentos/?placa=' + caja, 'name', 'width=850,height=480')
            window.open('../cliente/solicitudvehiculodocumentos.aspx?PLACA=' + caja + '&CATEGORIA=' + categoria + '&SID=' + tipoempresa, 'name', 'width=950,height=550')
        }
        function isValidDate(day, month, year) {
            var dteDate;
            // En javascript, el mes empieza en la posicion 0 y termina en la 11 
            //   siendo 0 el mes de enero
            // Por esta razon, tenemos que restar 1 al mes
            month = month - 1;
            // Establecemos un objeto Data con los valore recibidos
            // Los parametros son: año, mes, dia, hora, minuto y segundos
            // getDate(); devuelve el dia como un entero entre 1 y 31
            // getDay(); devuelve un num del 0 al 6 indicando siel dia es lunes,
            //   martes, miercoles ...
            // getHours(); Devuelve la hora
            // getMinutes(); Devuelve los minutos
            // getMonth(); devuelve el mes como un numero de 0 a 11
            // getTime(); Devuelve el tiempo transcurrido en milisegundos desde el 1
            //   de enero de 1970 hasta el momento definido en el objeto date
            // setTime(); Establece una fecha pasandole en milisegundos el valor de esta.
            // getYear(); devuelve el año
            // getFullYear(); devuelve el año
            dteDate = new Date(year, month, day);
            alert(dteDate);
            return ((day == dteDate.getDate()) && (month == dteDate.getMonth()) && (year == dteDate.getFullYear()));
        }
        function validate_fecha(fecha) {
            var patron = new RegExp("^(19|20)+([0-9]{2})([-])([0-9]{1,2})([-])([0-9]{1,2})$");
            if (fecha.search(patron) == 0) {
                var values = fecha.split("-");
                if (isValidDate(values[2], values[1], values[0])) {
                    return true;
                }
            }
            return false;
        }
        function validarFecha(fecha) {
            if (validate_fecha(fecha) == false) {
                var val = "El formato de fecha " + fecha + " es incorrecta, debe ser dia/mes/anio";
                alert(val);
                return false;
            }
            return true;
        }
        function fCategoria() {
            var vals = document.getElementById('<%=ddlCategoria.ClientID %>').value;
            document.getElementById('<%=categoriaveh.ClientID %>').value = vals;
            if (vals == 'CAT') {
                document.getElementById('<%=txttipocertificado.ClientID %>').disabled = false;
                document.getElementById('<%=txtnumcertificado.ClientID %>').disabled = false;
                document.getElementById('<%=txtfechamtop.ClientID %>').disabled = false;
                document.getElementById('<%=txtfechapoliza.ClientID %>').disabled = false;
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').disabled = true;
             
                document.getElementById('<%=txtClaseTipo.ClientID %>').disabled = false;
                document.getElementById('<%=txtModelo.ClientID %>').disabled = false;
              
                document.getElementById('valfecmtop').textContent = " * obligatorio";
                document.getElementById('valfecpol').textContent = " * obligatorio";
                document.getElementById('valtipcer').textContent = " * obligatorio";
                document.getElementById('valnumcer').textContent = " * obligatorio";
                document.getElementById('valClaseTipo').textContent = " * obligatorio";
                document.getElementById('valModelo').textContent = " * obligatorio";
                document.getElementById('valareaoc').textContent = "";
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').value = "0";
            }
            else if (vals == 'MON') {
                document.getElementById('<%=txttipocertificado.ClientID %>').disabled = false;
                document.getElementById('<%=txtnumcertificado.ClientID %>').disabled = false;
                document.getElementById('<%=txtfechamtop.ClientID %>').disabled = false;
                document.getElementById('<%=txtfechapoliza.ClientID %>').disabled = false;
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').disabled = true;
                document.getElementById('<%=txtClaseTipo.ClientID %>').disabled = true;
                document.getElementById('<%=txtModelo.ClientID %>').disabled = true;
                document.getElementById('<%=txttipocertificado.ClientID %>').disabled = true;
                document.getElementById('<%=txtnumcertificado.ClientID %>').disabled = true;
                document.getElementById('<%=txtfechamtop.ClientID %>').disabled = true;
                document.getElementById('<%=txtfechapoliza.ClientID %>').disabled = true;
              
                document.getElementById('valfecmtop').textContent = "";
                document.getElementById('valfecpol').textContent = "";
                document.getElementById('valtipcer').textContent = "";
                document.getElementById('valnumcer').textContent = "";
                document.getElementById('valClaseTipo').textContent = "";
                document.getElementById('valModelo').textContent = "";
                document.getElementById('valareaoc').textContent = "";
                document.getElementById('<%=txtClaseTipo.ClientID %>').value = "";
                document.getElementById('<%=txtModelo.ClientID %>').value = "";
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').value = "0";
            }
            else if (vals == 'LIV') {
//                var vals = document.getElementById('<%=cbltiposolicitud.ClientID %>').value;
//                if (vals == 0) {
//                    alert('* Seleccione un Tipo de Solicitud *');
//                    document.getElementById('<%=cbltiposolicitud.ClientID %>').focus();
//                    document.getElementById('<%=cbltiposolicitud.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";
//                    document.getElementById('<%=ddlCategoria.ClientID %>').value = 0;
//                    return false;
//                }
                document.getElementById('<%=txttipocertificado.ClientID %>').disabled = true;
                document.getElementById('<%=txtnumcertificado.ClientID %>').disabled = true;
                document.getElementById('<%=txtfechamtop.ClientID %>').disabled = true;
                document.getElementById('<%=txtfechapoliza.ClientID %>').disabled = true;
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').disabled = false;
             
                document.getElementById('<%=txtClaseTipo.ClientID %>').disabled = false;
                document.getElementById('<%=txtModelo.ClientID %>').disabled = false;
                
                document.getElementById('valfecmtop').textContent = "";
                document.getElementById('valfecpol').textContent = "";
                document.getElementById('valtipcer').textContent = "";
                document.getElementById('valnumcer').textContent = "";
                document.getElementById('valClaseTipo').textContent = " * obligatorio";
                document.getElementById('valModelo').textContent = " * obligatorio";
                document.getElementById('valareaoc').textContent = " * obligatorio";
            }
        }
        function fvalidaPlaca() {
            var renovacion = document.getElementById('<%=rbrenovacion.ClientID %>').checked;
            if (renovacion) {
                var vals = document.getElementById('<%=ddlTipoEmpresa.ClientID %>').value;
                if (vals == 0) {
                    alert('Seleccione el Tipo de Empresa');
                    document.getElementById('<%=ddlTipoEmpresa.ClientID %>').focus();
                  <%--  document.getElementById('<%=ddlTipoEmpresa.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=cbltiposolicitud.ClientID %>').value;
                if (vals == 0) {
                    alert('Seleccione al menos un Tipo de Solicitud');
                    document.getElementById('<%=cbltiposolicitud.ClientID %>').focus();
                  <%--  document.getElementById('<%=cbltiposolicitud.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtPlaca.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('Escriba la Placa o No. Serie (para Montacargas).');
                    document.getElementById('<%=txtPlaca.ClientID %>').focus();
                   <%-- document.getElementById('<%=txtPlaca.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                window.open('../catalogo/consultaPlacas.aspx' + '?sidpons=' + vals, 'name', 'width=850,height=880');
                return false;
            }
            else {
                alert('Solo puede buscar cuando es una Renovación Registro Vehículo.');
                document.getElementById('<%=txtPlaca.ClientID %>').focus();
               <%-- document.getElementById('<%=txtPlaca.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";--%>
                document.getElementById("loader").className = 'nover';
                return false;
            }
        }
        function fValidaRenovacion() {
            var renovacion = document.getElementById('<%=rbrenovacion.ClientID %>').checked;
            if (renovacion) {
//                document.getElementById('<%=btnBuscar.ClientID %>').disabled = false;
                document.getElementById('<%=ddlCategoria.ClientID %>').disabled = true;
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').disabled = true;
                document.getElementById('<%=txtMarca.ClientID %>').disabled = true;
                document.getElementById('<%=txtClaseTipo.ClientID %>').disabled = true;
                document.getElementById('<%=txtPlaca.ClientID %>').focus();
                document.getElementById('<%=txtModelo.ClientID %>').disabled = true;
                document.getElementById('<%=txtColor.ClientID %>').disabled = true;
                document.getElementById('<%=txttipocertificado.ClientID %>').disabled = true;
                document.getElementById('<%=txtnumcertificado.ClientID %>').disabled = true;
                document.getElementById('<%=txtfechamtop.ClientID %>').disabled = true;
                document.getElementById('<%=txtfechapoliza.ClientID %>').disabled = true;
        
                document.getElementById('<%=txtNota.ClientID %>').disabled = true;
            
                document.getElementById('valcategoriavehiculo').textContent = "";
                document.getElementById('valClaseTipo').textContent = "";
                document.getElementById('valMarca').textContent = "";
                document.getElementById('valModelo').textContent = "";
                document.getElementById('valColor').textContent = "";
                document.getElementById('valfecmtop').textContent = "";
                document.getElementById('valfecpol').textContent = "";
                document.getElementById('valtipcer').textContent = "";
                document.getElementById('valnumcer').textContent = "";
                document.getElementById('<%=ddlCategoria.ClientID %>').value = "0";
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').value = "0";             
                document.getElementById('<%=hfcategoria.ClientID %>').value = "";
                document.getElementById('<%=txtMarca.ClientID %>').value = "";
                document.getElementById('<%=txtClaseTipo.ClientID %>').value = "";
                document.getElementById('<%=txtModelo.ClientID %>').value = "";
                document.getElementById('<%=txtColor.ClientID %>').value = "";
                document.getElementById('<%=txttipocertificado.ClientID %>').value = "";
                document.getElementById('<%=txtnumcertificado.ClientID %>').value = "";
                document.getElementById('<%=txtfechamtop.ClientID %>').value = "";
                document.getElementById('<%=txtfechapoliza.ClientID %>').value = "";
                document.getElementById('<%=txtNota.ClientID %>').disabled = true;
          
            }
        }
        function fValidaEmision() {
            var emision = document.getElementById('<%=rbemision.ClientID %>').checked;
            if (emision) {
//                document.getElementById('<%=btnBuscar.ClientID %>').disabled = true;
                document.getElementById('<%=ddlCategoria.ClientID %>').disabled = false;
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').disabled = true;
                document.getElementById('<%=txtMarca.ClientID %>').disabled = false;
                document.getElementById('<%=txtClaseTipo.ClientID %>').disabled = false;
                document.getElementById('<%=txtPlaca.ClientID %>').focus();
                document.getElementById('<%=txtModelo.ClientID %>').disabled = false;
                document.getElementById('<%=txtColor.ClientID %>').disabled = false;
                document.getElementById('<%=txttipocertificado.ClientID %>').disabled = true;
                document.getElementById('<%=txtnumcertificado.ClientID %>').disabled = true;
                document.getElementById('<%=txtfechamtop.ClientID %>').disabled = true;
                document.getElementById('<%=txtfechapoliza.ClientID %>').disabled = true;
              
                document.getElementById('<%=txtNota.ClientID %>').disabled = false;
             
                document.getElementById('valcategoriavehiculo').textContent = " * obligatorio";
                document.getElementById('valClaseTipo').textContent = " * obligatorio";
                document.getElementById('valMarca').textContent = " * obligatorio";
                document.getElementById('valModelo').textContent = " * obligatorio";
                document.getElementById('valColor').textContent = " * obligatorio";
                document.getElementById('valfecmtop').textContent = "";
                document.getElementById('valfecpol').textContent = "";
                document.getElementById('valtipcer').textContent = "";
                document.getElementById('valnumcer').textContent = "";
                document.getElementById('<%=ddlCategoria.ClientID %>').value = "0";
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').value = "0";
                document.getElementById('<%=hfcategoria.ClientID %>').value = "";
                document.getElementById('<%=txtMarca.ClientID %>').value = "";
                document.getElementById('<%=txtClaseTipo.ClientID %>').value = "";
                document.getElementById('<%=txtModelo.ClientID %>').value = "";
                document.getElementById('<%=txtColor.ClientID %>').value = "";
                document.getElementById('<%=txttipocertificado.ClientID %>').value = "";
                document.getElementById('<%=txtnumcertificado.ClientID %>').value = "";
                document.getElementById('<%=txtfechamtop.ClientID %>').value = "";
                document.getElementById('<%=txtfechapoliza.ClientID %>').value = "";
                document.getElementById('<%=txtNota.ClientID %>').disabled = false;
              
            }
        }
        function fValidaEmisionf() {
            document.getElementById('<%=rbemision.ClientID %>').checked = true;
            document.getElementById('<%=rbrenovacion.ClientID %>').checked = false;
            var emision = document.getElementById('<%=rbemision.ClientID %>').checked;
            if (emision) {
                document.getElementById('<%=btnBuscar.ClientID %>').disabled = true;
                document.getElementById('<%=ddlCategoria.ClientID %>').disabled = false;
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').disabled = true;
                document.getElementById('<%=txtMarca.ClientID %>').disabled = false;
                document.getElementById('<%=txtClaseTipo.ClientID %>').disabled = false;
                document.getElementById('<%=txtPlaca.ClientID %>').focus();
                document.getElementById('<%=txtModelo.ClientID %>').disabled = false;
                document.getElementById('<%=txtColor.ClientID %>').disabled = false;
                document.getElementById('<%=txttipocertificado.ClientID %>').disabled = true;
                document.getElementById('<%=txtnumcertificado.ClientID %>').disabled = true;
                document.getElementById('<%=txtfechamtop.ClientID %>').disabled = true;
                document.getElementById('<%=txtfechapoliza.ClientID %>').disabled = true;
              
                document.getElementById('<%=txtNota.ClientID %>').disabled = false;
              
                document.getElementById('valcategoriavehiculo').textContent = " * obligatorio";
                document.getElementById('valClaseTipo').textContent = " * obligatorio";
                document.getElementById('valMarca').textContent = " * obligatorio";
                document.getElementById('valModelo').textContent = " * obligatorio";
                document.getElementById('valColor').textContent = " * obligatorio";
                document.getElementById('valfecmtop').textContent = "";
                document.getElementById('valfecpol').textContent = "";
                document.getElementById('valtipcer').textContent = "";
                document.getElementById('valnumcer').textContent = "";
                document.getElementById('<%=ddlCategoria.ClientID %>').value = "0";
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').value = "0";
                document.getElementById('<%=hfcategoria.ClientID %>').value = "";
                document.getElementById('<%=txtMarca.ClientID %>').value = "";
                document.getElementById('<%=txtClaseTipo.ClientID %>').value = "";
                document.getElementById('<%=txtModelo.ClientID %>').value = "";
                document.getElementById('<%=txtColor.ClientID %>').value = "";
                document.getElementById('<%=txttipocertificado.ClientID %>').value = "";
                document.getElementById('<%=txtnumcertificado.ClientID %>').value = "";
                document.getElementById('<%=txtfechamtop.ClientID %>').value = "";
                document.getElementById('<%=txtfechapoliza.ClientID %>').value = "";
                document.getElementById('<%=txtNota.ClientID %>').disabled = false;
           
            }
        }
        function Panel() {
            $(document).ready(function () {
                $('#ventana_popup').fadeIn('slow');
                $('#popup-overlay').fadeIn('slow');
                $('#popup-overlay').height($(window).height());
            });
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
		        <div id='borde_ventana_popup'> 
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
