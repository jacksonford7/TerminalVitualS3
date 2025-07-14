<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" 
         CodeBehind="revisiontecnica.aspx.cs" Inherits="CSLSite.revisiontecnica" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />

    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />

    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/turnos.js" type="text/javascript"></script>
    <script src="../Scripts/centroServicios.js" type="text/javascript"></script>
    <link href="../shared/estilo/centroSolicitud.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function BindFunctions() {

            $(document).ready(function () {
                $("#tabla [id*=chkHeader]").click(function () {
                    if ($(this).is(":checked")) {
                        $("#tabla [id*=cbox]").attr("checked", "checked");
                    } else {
                        $("#tabla [id*=cbox]").removeAttr("checked");
                    }
                });

                $("#tabla [id*=cbox]").click(function () {
                    if ($("#tabla [id*=cbox]").length == $("#tabla [id*=cbox]:checked").length) {
                        $("#tabla [id*=chkHeader]").attr("checked", "checked");
                    } else {
                        $("#tabla [id*=chkHeader]").removeAttr("checked");
                    }
                });
            });
                
        }

    </script>
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <input id="zonaid" type="hidden" value="709" />
 <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>


         <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Servicios</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Solicitud de Revisión Técnica Refrigerada (RTR)</li>
          </ol>
        </nav>
      </div>
    <%-- nuevo contenedor de Bootstrap--%>
     <div class="dashboard-container p-4" id="cuerpo" runat="server">

         <div class="form-title">Criterios de consulta:</div>
           <div class="row">
                   <div class="col-md-12 d-flex">
                       <label for="inputAddress">Servicio<span style="color: #FF0000; font-weight: bold;"></span></label>
                              
                       <asp:DropDownList ID="dptiposervicios" runat="server"  CssClass="form-control" 
          onchange="selectServicio($('[id*=dptiposervicios]').val(),$('[id*=dptipotrafico]').val(),$('[id*=dptipocarga]').val(),$('[id*=txtcontenedor]').val(),$('[id*=txtNumBooking]').val(),$('[id*=txtnumcarga1]').val(),$('[id*=txtnumcarga2]').val())">  
                 <asp:ListItem Value="0">* Seleccione servicios *</asp:ListItem>
         
                           </asp:DropDownList>
                  </div>

   </div>

                 

              <div class="form-row">
                                  <div class="form-group col-md-6"> 
<label for="inputAddress">Tráfico<span style="color: #FF0000; font-weight: bold;"></span></label>
      <asp:DropDownList ID="dptipotrafico" runat="server" CssClass="form-control"
          onchange="selectServicio($('[id*=dptiposervicios]').val(),$('[id*=dptipotrafico]').val(),$('[id*=dptipocarga]').val(),$('[id*=txtcontenedor]').val(),$('[id*=txtNumBooking]').val(),$('[id*=txtnumcarga1]').val(),$('[id*=txtnumcarga2]').val())">     
                 <asp:ListItem Value="0">* Seleccione tráfico *</asp:ListItem>
          </asp:DropDownList>
                  </div>

                   
               <div class="form-group col-md-6"> 
 <label for="inputAddress">Tipo de Carga:<span style="color: #FF0000; font-weight: bold;"></span></label>

                        <asp:DropDownList ID="dptipocarga" runat="server"  CssClass="form-control" 
          onchange="selectServicio($('[id*=dptiposervicios]').val(),$('[id*=dptipotrafico]').val(),$('[id*=dptipocarga]').val(),$('[id*=txtcontenedor]').val(),$('[id*=txtNumBooking]').val(),$('[id*=txtnumcarga1]').val(),$('[id*=txtnumcarga2]').val())"> 
                 
          </asp:DropDownList>
                  </div>

</div>                  
              
              

               <div class="form-row">
                     <div class="form-group col-md-6"> 
 <label for="inputAddress">Contenedor<span style="color: #FF0000; font-weight: bold;"></span></label>
    <asp:TextBox ID="txtcontenedor" runat="server" MaxLength="11" CssClass="form-control" 
              onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')"
              EnableViewState="False"
              placeholder="Contenedor"></asp:TextBox> 

                         <label for="inputAddress">Todos<span style="color: #FF0000; font-weight: bold;"></span></label>
  <asp:CheckBox Text="" ID="chkTodos" runat="server" 
      
      ToolTip="Al darle click a esta opción se procesarán todos los containers del grid para el servicio que haya seleccionado." />
                  </div>
                     <div class="form-group col-md-6"> 
 <label for="inputAddress">Booking<span style="color: #FF0000; font-weight: bold;"></span></label>
                <asp:TextBox ID="txtNumBooking" runat="server" MaxLength="50"
            CssClass="form-control" 
             onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890-',true)"></asp:TextBox>
                  </div>


               </div>

               <div class="form-row">
                   <div class="form-group col-md-12">
 <label for="inputAddress">Número de Carga<span style="color: #FF0000; font-weight: bold;"></span></label>
                     <div class="d-flex">
                       <asp:TextBox ID="txtnumcarga1" runat="server" MaxLength="15"
             CssClass="form-control" 
             onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890',true)" onBlur="checkNumCarga(this,'valNumCarga',true,'MRN')" placeholder = "MRN"></asp:TextBox>
        
             <asp:TextBox ID="txtnumcarga2" runat="server"  MaxLength="4"
             CssClass="form-control" 
             onkeypress="return soloLetras(event,'01234567890',true)" onBlur="checkNumCarga(this,'valNumCarga',true,'MSN')" placeholder = "MSN"></asp:TextBox>

             <asp:TextBox ID="txtnumcarga3" runat="server"  MaxLength="4"
             CssClass="form-control" 
             onkeypress="return soloLetras(event,'01234567890',true)" onBlur="checkNumCarga(this,'valNumCarga',true,'HSN')" placeholder = "HSN"></asp:TextBox>


                     </div>             
                       
                       

                       
                   </div>

                   </div>
         <div class="row">
              <div class="col-md-12 d-flex justify-content-center">
             <span id="valNumCarga" class="validacion"> * obligatorio</span>
                  </div>
         </div>

               <div class="row">
		  
		   <div class="col-md-12 d-flex justify-content-center"> 
                            
             <asp:Button ID="btbuscar" 
                 runat="server" 
                 Text="Iniciar la búsqueda"   
                 CssClass="btn btn-primary" 
               onclick="btbuscar_Click"
                 OnClientClick="return selectServicio($('[id*=dptiposervicios]').val(),$('[id*=dptipotrafico]').val(),$('[id*=dptipocarga]').val(),$('[id*=txtcontenedor]').val(),$('[id*=txtNumBooking]').val(),$('[id*=txtnumcarga1]').val(),$('[id*=txtnumcarga2]').val());"/>
                 <span id="imagen" runat="server"></span>
		   </div>
		  </div>




                    <div class="cataresult" >
        <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>

        <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
        <input id="diponible" type="hidden" runat="server" clientidmode="Static" />

        <div id="xfinder" runat="server" visible="false">
            <div class="msg-alerta" id="alerta" runat="server" ></div>
           
            <asp:GridView ID="grvContenedores" runat="server" 
                CssClass="table table-bordered invoice" AutoGenerateColumns="false"
                AllowPaging="True" onpageindexchanging="grvContenedores_PageIndexChanging" onrowdatabound="grvContenedores_OnRowCreated"   
               
                >
                <Columns>
            <%--0--%>
                    <asp:TemplateField HeaderText="# Contenedor" ItemStyle-Width="25%" Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblIdContenedor" Visible="false" runat="server" Text='<%# Eval("gkey") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>
           <%--1--%>
                    <asp:TemplateField HeaderText="No. Contenedor" ItemStyle-Width="25%" Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblNombreContenedor" runat="server" Text='<%# Eval("cntr") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>
           <%--2--%>
                     <asp:TemplateField  ItemStyle-Width="25%"  Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblNombreCat" runat="server" Text='<%# Eval("categoria") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>
          <%--3--%>
                    <asp:TemplateField HeaderText="Tipo" ItemStyle-Width="25%"  Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblFk" runat="server" Text='<%# Eval("fk") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>

          <%--4--%>
                     <asp:TemplateField HeaderText="Sello1" ItemStyle-Width="25%"  Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblS1" runat="server" Text='<%# Eval("s1") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>
         <%--5--%>
                     <asp:TemplateField HeaderText="Sello2" ItemStyle-Width="25%"  Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblS2" runat="server" Text='<%# Eval("s2") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>
        <%--6--%>
                    <asp:TemplateField HeaderText="Sello3" ItemStyle-Width="25%"  Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblS3" runat="server" Text='<%# Eval("s3") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>
         <%--7--%>
                    <asp:TemplateField HeaderText="Sello4" ItemStyle-Width="25%"  Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblS4" runat="server" Text='<%# Eval("s4") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>
       <%--8--%>
                    <asp:TemplateField HeaderText="ISO" ItemStyle-Width="25%"  Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lbliso" runat="server" Text='<%# Eval("iso") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>

<%--9--%>
                    <asp:TemplateField HeaderText="Servicio Pendiente" ItemStyle-Width="10%"  Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblgrupo" runat="server"  Text='<%# Ogrupo( Eval("grupo")) %>'> </asp:Label>
                            <asp:HiddenField ID="grupoReal" runat="server" Value='<%# Eval("grupo") %>' />
                               </ItemTemplate>
                        <ItemStyle Width="10%" />
                    </asp:TemplateField>

  <%--10--%>
                    <asp:TemplateField HeaderText="Peso" ItemStyle-Width="25%" Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblPesoContenedor" runat="server" Text='<%# Eval("peso") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>
  <%--11--%>
                    <asp:TemplateField HeaderText="BOKING" ItemStyle-Width="25%"  Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblboking" runat="server" Text='<%# Eval("boking") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>
  <%--12--%>
                    <asp:TemplateField HeaderText="MRN" ItemStyle-Width="25%"  Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblmrn" runat="server" Text='<%# Eval("mrn") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>
  <%--13--%>
                       <asp:TemplateField HeaderText="MSN" ItemStyle-Width="25%"  Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblmsn" runat="server" Text='<%# Eval("msn") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>
   <%--14--%>
                       <asp:TemplateField HeaderText="HSN" ItemStyle-Width="25%"  Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblhsn" runat="server" Text='<%# Eval("hsn") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>
   <%--15--%>
                       <asp:TemplateField HeaderText="PROPID" ItemStyle-Width="25%"  Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblpropID" runat="server" Text='<%# Eval("propID") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>
   <%--16--%>
                    <asp:TemplateField HeaderText="PROPOM" ItemStyle-Width="25%"  Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblpropNombre" runat="server" Text='<%# Eval("propNombre") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>
   <%--17--%>
                     <asp:TemplateField HeaderText="DOC" ItemStyle-Width="25%"  Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lbldoc" runat="server" Text='<%# Eval("doc") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>

   <%--18--%>
                     <asp:TemplateField HeaderText="AISV" ItemStyle-Width="25%"  Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblAISV" runat="server" Text='<%# Eval("aisv") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>
   <%--19--%>
                    <asp:TemplateField HeaderText="Referencia" ItemStyle-Width="25%"  Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblref" runat="server" Text='<%# Eval("referencia") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>
   <%--20--%>
                    <asp:TemplateField HeaderText="UserID" ItemStyle-Width="25%"  Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblusid" runat="server" Text='<%# Eval("expoUser") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>
   <%--21--%>
                  
                    <asp:TemplateField HeaderText="ImoK" ItemStyle-Width="25%"  Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblImo" runat="server" Text='<%# Eval("imoKey") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>
   <%--22--%>
                    <asp:TemplateField HeaderText="Tiene Etiqueta" ItemStyle-Width="10%"  Visible="true">
                        <ItemTemplate>
                            <asp:CheckBox ID="cImo" Checked='<%# Eval("esImo") %>' runat="server" />
                        </ItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>
   <%--23--%>
                  <asp:TemplateField HeaderText="Es Refeer" ItemStyle-Width="25%"  Visible="true">
                        <ItemTemplate>
                              <asp:CheckBox ID="cRef" Checked='<%# Eval("esRefer") %>' runat="server" Enabled="false" />

                        </ItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>

   <%--24--%>
                    <asp:TemplateField HeaderText="LINEA" ItemStyle-Width="25%"  Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblinea" runat="server" Text='<%# Eval("linea") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>

   <%--25--%>
                     <asp:TemplateField HeaderText="PATIO" ItemStyle-Width="25%"  Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lbpatio" runat="server" Text='<%# Eval("patio") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>
   <%--26--%>

                    <asp:TemplateField HeaderText="Seleccionar" ItemStyle-Width="25%" Visible="true">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkRow" Checked='<%# Eval("check") %>' runat="server" />
                        </ItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>

                </Columns>                
                <PagerSettings FirstPageImageUrl="~/shared/imgs/first.gif" 
                    LastPageImageUrl="~/shared/imgs/last.gif" Mode="NextPreviousFirstLast" 
                    NextPageImageUrl="~/shared/imgs/next.gif" 
                    PreviousPageImageUrl="~/shared/imgs/prev.gif" />
                <RowStyle CssClass="columnasGrid" />
            </asp:GridView>
         
            
                <div class="row">
		  
		   <div class="col-md-12 d-flex justify-content-center"> 
                    <span id="imagenSolicitud" runat="server"></span>        
                    <input type="button" id="btgenerar"  class="btn btn-primary" 
                        value="Generar solicitud" onclick="confirmGenerarSolicitud()"
                             />
                    <asp:Button ID="btgenerarServer" runat="server" Text="Generar solicitud" OnClientClick="showGif('placebody_imagenSolicitud')"
                        OnClick="btgenerarServer_Click"  style="display:none;" />
                                  
                  </div></div>
                              
       
            
        </div>
        <div id="sinresultado" runat="server" class="msg-info"></div>
        </ContentTemplate>
                     <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                     </Triggers>
        </asp:UpdatePanel>
        </div>

           </div>

         
 




 <script src="../Scripts/pages.js" type="text/javascript"></script>

    
    
    <script type="text/javascript">
        function confirmGenerarSolicitud() {
            var r = alertify.confirm("Se generará una nueva solicitud, ¿Desea continuar?"
                , function () { $("#<%=btgenerarServer.ClientID%>").click(); alertify.success('Procesando') },
                function () {return;}
            );
       }
        function showGif(ctrl) {
            if (ctrl != null && ctrl != undefined && ctrl.trim().length > 0) {
                document.getElementById(ctrl).innerHTML = '<img alt="" src="../shared/imgs/loader.gif">';                
            }
        }
        function popupCallback(objeto) {
            if (objeto == null || objeto == undefined) {
                alertify.alert('Hubo un problema al setaar un objeto de catalogo');
                return;
            }            
        }
        function clear() {
            document.getElementById('numbook').textContent = '...';
            document.getElementById('nbrboo').value = '';
        }
        function clear2() {
            document.getElementById('txtfecha').textContent = '...';
            document.getElementById('xfecha').value = '';
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
