<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true"
    CodeBehind="solicitudCorreccionExportacion.aspx.cs" Inherits="CSLSite.solicitudCorreccionExportacion" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
  
       <!--mensajes-->
        <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
        <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>
   
    <script src="../Scripts/centroServicios.js" type="text/javascript"></script>

    <style type="text/css">
        .warning
        {
            background-color: Yellow;
            color: Red;
        }
        
        #progressBackgroundFilter
        {
            position: fixed;
            bottom: 0px;
            right: 0px;
            overflow: hidden;
            z-index: 1000;
            top: 0;
            left: 0;
            background-color: #CCC;
            opacity: 0.8;
            filter: alpha(opacity=80);
            text-align: center;
        }
        #processMessage
        {
            text-align: center;
            position: fixed;
            top: 30%;
            left: 43%;
            z-index: 1001;
            border: 5px solid #67CFF5;
            width: 200px;
            height: 100px;
            background-color: White;
            padding: 0;
        }
        #aprint
        {
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
            background-position: left center;
            text-decoration: none;
            padding: 5px 2px 5px 30px;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <input id="zonaid" type="hidden" value="710" />
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"
        AsyncPostBackTimeout="1200">
    </asp:ToolkitScriptManager>

        <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Servicios</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">
        
               Solicitud Corrección de Ingreso de Exportación (IIE)

             </li>
          </ol>
        </nav>
      </div>
    <asp:UpdatePanel ID="updSolicitud" runat="server">
        <ContentTemplate>
            
            <div class="dashboard-container p-4" id="cuerpo" runat="server">

         <div class="form-title">Datos del documento buscado</div>
		 
		  <div class="form-row">
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Servicio<span style="color: #FF0000; font-weight: bold;"></span></label>
			          <asp:DropDownList ID="ddlTipoServicios" runat="server" 
                           CssClass="form-control"
                          >
                                    <asp:ListItem Value="0">* Seleccione servicios *</asp:ListItem>
                                </asp:DropDownList>
                      <asp:HiddenField ID="hdfUsuario" runat="server" />
		   </div>
		
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Tráfico<span style="color: #FF0000; font-weight: bold;"></span></label>
			        <asp:DropDownList ID="ddlTipoTrafico" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="0">* Seleccione  *</asp:ListItem>
                                </asp:DropDownList>
		   </div>
		  </div>

       <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Tipo de Corrección<span style="color: #FF0000; font-weight: bold;"></span></label>
			        
               <div  class="d-flex"">
                           <asp:DropDownList ID="ddlTipoCorreccion" runat="server"  CssClass="form-control"
                          OnSelectedIndexChanged="ddlTipoCorreccion_SelectedIndexChanged"
                                    AutoPostBack="true" onblur="opcionrequerida(this,0,'valTipoCorreccion');">
                                    <asp:ListItem Value="0">* Seleccione tipo de corrección *</asp:ListItem>
                                </asp:DropDownList>

                    <span class="validacion" id="valTipoCorreccion">*</span>

               </div>
               
       
		   </div>

             <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Mail<span style="color: #FF0000; font-weight: bold;"></span></label>
			     <div  class="d-flex"">
                              <script language='javascript' type="text/javascript">

                                    var g_correoUsuarioID = '<%=hdfCorreoUsuario.ClientID%>'

                                </script>
               
                   <asp:HiddenField ID="hdfCorreoUsuario" runat="server" />
                                <asp:TextBox ID="txtMail" runat="server"  CssClass="form-control"
                                    onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_$@.',true)"
                                    MaxLength="150" onpaste="return false;" 
                                    onblur="validarEmail(this,'valemailusu', document.getElementById(g_correoUsuarioID));"
                                    placeholder="MAIL@MAIL.COM" />
                     <span class="validacion" id="valemailusu">*</span>
                     </div>
               
               
      
		   </div>

		  </div>

          <div class="nover">
		  
		   <div class="form-group col-md-8"> 
		   	  <label for="inputAddress">Telefono<span style="color: #FF0000; font-weight: bold;"></span></label>
			                                 <asp:TextBox ID="txtTelefono" runat="server"  CssClass="form-control"
                                                 onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890 .',true)"
                                    MaxLength="20" onpaste="return false;" onblur="cadenareqerida(this,1,20,'valtelefono');" />
		   </div>

               <div class="form-group col-md-4"> 
                    <span class="validacion" id="valtelefono">* </span>
                   </div>
		  </div>


                <div class="form-title">Datos del contenedor</div>

                <div class="form-row">
		   <div class="form-group col-md-12"> 
		   	  <label for="inputAddress">Número<span style="color: #FF0000; font-weight: bold;"></span></label>
			       
               <div class="d-flex">
                           <asp:TextBox ID="contenedor1" runat="server" Style="text-align: center;" Text="..."
                                    Width="90%" Enabled="false"></asp:TextBox>
                                <input id="contenedor1HF" runat="server" type="hidden" />
                              <a class="btn btn-outline-primary mr-4" target="popup" 
                       onclick="linkcontenedorCorrecionDAEExportacion('<%=ddlTipoTrafico.ClientID %>','FCL', '<%=ddlTipoCorreccion.ClientID %>' ,'<%=txtAISV.ClientID %>','<%=hdfUsuario.ClientID %>');">
                                    <span class='fa fa-search' style='font-size:24px'></span>

                   </a>
                   <span id="valcont2" class="validacion">*</span>
               </div>
               
       
		   </div>


		  </div>


                <div class="form-row">
		  
		   <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">AISV<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                   <asp:TextBox ID="txtAISV" runat="server"  CssClass="form-control"
                                     MaxLength="15"></asp:TextBox>

                         <asp:Panel ID="pnlAisv" runat="server">
                                    <span class="validacion" id="valAisv"></span>

                                </asp:Panel>

			  </div>
		   </div>
		   
		    <div class="form-group col-md-3"> 
                 <label for="inputAddress">Booking<span style="color: #FF0000; font-weight: bold;"></span></label>
			         <asp:TextBox ID="bookingContenedor" runat="server" 
                         CssClass="form-control"
                         Text="..."
                                    
                         Enabled="false"></asp:TextBox>
                                <input id="bookingContenedorHF" runat="server" type="hidden" />
            
            </div>

                     <div class="form-group col-md-3"> 
                          <label for="inputAddress">Código<span style="color: #FF0000; font-weight: bold;"></span></label>
                         <asp:TextBox ID="txtNumeroCarga" runat="server" 
                              Text="..." CssClass="form-control"
                                   Enabled="false"></asp:TextBox>
                                <asp:HiddenField ID="hdfIso" runat="server" />
			 </div>

                    <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Peso<span style="color: #FF0000; font-weight: bold;"></span></label>
			       <asp:TextBox ID="txtPeso" runat="server" 
                        CssClass="form-control" Text="..." 
                      
                                    Enabled="false"></asp:TextBox>
		   </div>

		  </div>

                <div class="form-row">
		  
		   <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">S. Agencia<span style="color: #FF0000; font-weight: bold;"></span></label>
			        <asp:TextBox ID="txtSello1" runat="server" CssClass="form-control" Text="..."
                                    Enabled="false"></asp:TextBox>
		   </div>
		      <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">S. Ventolera<span style="color: #FF0000; font-weight: bold;"></span></label>
			            <asp:TextBox ID="txtSello2" runat="server" 
                            CssClass="form-control" Text="..."
                                   Enabled="false"></asp:TextBox>
		   </div>
                       <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">S. Opcional1<span style="color: #FF0000; font-weight: bold;"></span></label>
			    <asp:TextBox ID="txtSello3" runat="server"  CssClass="form-control" Text="..."
                                     Enabled="false"></asp:TextBox>
		   </div>
                       <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">S. Opcional2<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <asp:TextBox ID="txtSello4" runat="server"  CssClass="form-control" Text="..."
                                    Enabled="false"></asp:TextBox>
		   </div>
		   
		  </div>

                  <div class="form-title">Datos del documento de Aduana</div>

                <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		   	  <label for="inputAddress">DAE Incorrecta<span style="color: #FF0000; font-weight: bold;"></span></label>
			         <div class="d-flex">     
               
               <asp:TextBox ID="txtActualDae" runat="server" 
                               CssClass="form-control"

                                    Enabled="false" onblur="cadenareqerida(this,1,200,'valDaeActual');"></asp:TextBox>

                                <input id="hdfDaeActual" runat="server" type="hidden" />

                   <asp:Panel ID="pnlDaeActual" runat="server">
                                    <span class="validacion" id="valDaeActual">* </span>
                                </asp:Panel>
                         </div> 
		   </div>
		   
		
                    
		


		  </div>

                <div class="form-row">
		   <div class="form-group col-md-12"> 
		   	  <label for="inputAddress">DAE Correcta<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                       <asp:TextBox ID="txtNuevoDae" runat="server" 
                            CssClass="form-control"
                                    onblur="cadenareqerida(this,1,17,'valDaeNueva');" MaxLength="17" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890',true)"></asp:TextBox>
                                <input id="hdfDaeNueva" runat="server" type="hidden" />

                   <asp:Panel ID="pnlDaeNueva" runat="server">
                                    <span class="validacion" id="valDaeNueva">* </span>
                                </asp:Panel>

			  </div>
		   </div>
		  </div>
<div class="row">
		   <div class="col-md-12 d-flex justify-content-center"> 
                        <input type="button" id="btgenerar" class="btn btn-primary"
                            value="Generar solicitud" onclick="confirmSolicitud()"
                             />
                        <asp:Button ID="btgenerarServer"
                            runat="server" Text="Generar solicitud" 
                            OnClientClick="showGif('placebody_imagen')"
                            OnClick="btgenerar_Click" CssClass="form-control" 
                            style="display:none;" />
               <span id="imagen" runat="server"></span>
		   </div> 


</div>
  <div class="cataresult">
                        <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                            <ContentTemplate>
                                <div class="alert-light" id="alerta" runat="server">
                                </div>
                                <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
                                <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
                                <div id="sinresultado" runat="server" class="alert-light">
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btgenerarServer" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
     </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/pages_mod.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });
        function showGif(ctrl) {
            if (ctrl != null && ctrl != undefined && ctrl.trim().length > 0) {
                document.getElementById(ctrl).innerHTML = '<img alt="" src="../shared/imgs/loader.gif">'
            }
        }
        function popupCallback(objeto) {
            if (objeto == null || objeto == undefined) {
                alertify.alert('Hubo un problema al setear un objeto de catalogo');
                return;
            }
            //si contenedor está lleno (FCL) o vacio (MTY)
            document.getElementById('<%=contenedor1.ClientID %>').value = objeto.codigo;
            document.getElementById('<%=contenedor1HF.ClientID %>').value = objeto.item;
            document.getElementById('<%=bookingContenedor.ClientID %>').value = objeto.booking;
            document.getElementById('<%=bookingContenedorHF.ClientID %>').value = objeto.booking;
            document.getElementById('<%=txtActualDae.ClientID %>').value = objeto.dae;
            document.getElementById('<%=hdfDaeActual.ClientID %>').value = objeto.dae;
            document.getElementById('<%=txtSello1.ClientID %>').value = objeto.sello1;
            document.getElementById('<%=txtSello2.ClientID %>').value = objeto.sello2;
            document.getElementById('<%=txtSello3.ClientID %>').value = objeto.sello3;
            document.getElementById('<%=txtSello4.ClientID %>').value = objeto.sello4;
            document.getElementById('<%=txtPeso.ClientID %>').value = objeto.peso;
            document.getElementById('<%=txtNumeroCarga.ClientID %>').value = objeto.codigoCarga;
            document.getElementById('<%=hdfIso.ClientID %>').value = objeto.iso;
            if(objeto.aisv !='')
            document.getElementById('<%=txtAISV.ClientID %>').value = objeto.aisv;
            document.getElementById('<%=txtAISV.ClientID %>').setAttribute("disabled", "disabled");
            return;
        }

        function confirmSolicitud() {
            var trafico = document.getElementById('<%=ddlTipoCorreccion.ClientID %>').value;
            var msg = '';
            switch (trafico) {
                case 'DA':
                    msg = 'Se generará una eliminación de ingreso de exportación y un nuevo ingreso de exportación ¿Está seguro de realizar la solicitud?"';
                    break;
                case 'DE':
                    msg = 'Se generará un nuevo ingreso de exportación ¿Está seguro de realizar la solicitud?';
                    break;
                case 'DI':
                    msg = 'Se generará una eliminación de ingreso de exportación y un nuevo ingreso de exportación ¿Está seguro de realizar la solicitud?';
                    break;
                default:
                    msg = '¿Está seguro de realizar la solicitud?';
                    break;

            }
            alertify.confirm(msg, function () { $("#<%=btgenerarServer.ClientID%>").click(); alertify.success('Procesando') },
                function () { return; });
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
