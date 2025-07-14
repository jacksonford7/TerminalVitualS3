<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="consultaContenedoresAnalista.aspx.cs" Inherits="CSLSite.consultaContenedoresAnalista" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Contenedores</title>
         <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>
    
        <!--Datatables-->
       <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />
       <link href="../css/datatables.min.css" rel="stylesheet" />
       <link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css"/>
        <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>
       <script type="text/javascript"  src="../js/datatables.js"></script>
       <script src="../Scripts/table_catalog.js" type="text/javascript"></script>  
       <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
    <!--Fin-->


        <!--mensajes-->
        <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
        <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>



    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <link href="../shared/estilo/centroSolicitud.css" rel="stylesheet" type="text/css" />
   
</head>
<body>

    <form id="bookingfrm" runat="server">
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
   <div class="dashboard-container p-4" id="cuerpo" runat="server">
		
       	  <div class="form-row">

		   <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Solicitud<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <asp:Label ID="lblCodigoSolicitud" CssClass="form-control col-md-12" Text="" runat="server"></asp:Label>
               <asp:Label ID="lblCodigoSol" Text="No. Solicitud:" runat="server" Visible="false"></asp:Label>
		   </div>
                   
                 <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Tráfico<span style="color: #FF0000; font-weight: bold;"></span></label>
			     <asp:Label CssClass="form-control col-md-12" ID="lblTipoTrafico" Text="" runat="server"></asp:Label>

		                    <asp:Label ID="lblTrafico" Text="Tráfico:" Visible="false" runat="server"></asp:Label>

		    </div>
                   <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Booking<span style="color: #FF0000; font-weight: bold;"></span></label>
			 	<asp:Label ID="lblBooking" Visible="false" Text="Num. Booking:" runat="server"></asp:Label>
                  <asp:Label ID="lblNumBooking"  CssClass="form-control col-md-12" Text="" runat="server"></asp:Label>

		   </div>
                   <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Carga<span style="color: #FF0000; font-weight: bold;"></span></label>
			  				                    <asp:Label ID="lblCarga" Visible="false" Text="Carga:" runat="server"></asp:Label>
                 <asp:Label ID="lblNumCarga" Text="" runat="server" CssClass="form-control col-md-12"></asp:Label>

		   </div>

		  </div>
      

       
       
       <div class="form-row">



      

                 <div class="form-group  col-md-12"> 
                     	  <label for="inputAddress">Estado<span style="color: #FF0000; font-weight: bold;"></span></label>
		                    <asp:Label ID="lblEstado" Text="Estado:" Visible="false" runat="server"></asp:Label>
		   
                 
                    <asp:DropDownList ID="dpestados" runat="server"  CssClass="form-control" >
                         <asp:ListItem Value="0">* Seleccione *</asp:ListItem>
                    </asp:DropDownList>
		</div>

		  </div>



        <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
             <asp:TextBox TextMode="MultiLine"  CssClass=" form-control"
                 MaxLength="300" id="txtobservacion" 
                 required="required" runat="server"  Height="60px" Text=""/>

		   </div> 
		   </div>

          <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
                <div class="d-flex">
                     <asp:Button ID="save"  CssClass="btn btn-primary"
                         runat="server" Text="Guardar" onclick="save_Click"/>
                    <span id="imagen"></span>

                </div>
		   </div> 
		   </div>


		          <div class="cataresult" >
             <asp:UpdatePanel ID="upresult" runat="server"  >
             <ContentTemplate>
             <script type="text/javascript">Sys.Application.add_load(BindFunctions); </script>
               <div id="xfinder" runat="server" visible="false" >
             <div class=" alert alert-warning" id="alerta" >
                 Confirme que los datos sean correctos. En caso de error, favor comuníquese con 
                 el Departamento de Planificación a los teléfonos: +593 (04) 6006300, 3901700 
                 ext. 4039, 4040, 4060. 
             </div>
     
                  <div class=" form-title">Contenedores</div>
               
                 <asp:Repeater ID="tablePagination" runat="server" OnItemDataBound="tablePagination_ItemDataBound">
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                 <thead>
                 <tr>
                 <th align="center">No.</th>
                 <th align="center">Contenedor</th>                 
                 <th align="center">Procesado</th>
                 <th align="center" id="tipoVerificacionTH" runat="server" visible="true">Tipo Verificación</th>
                 <th align="center">Observación</th>
                 <th align="center">Aplica servicio</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point">
                  <td><asp:Label ID="idDetalleSolicitud" Text='<%#Eval("idDetalleSolicitud")%>' runat="server"></asp:Label></td>
                  <td><%#Eval("descripcionContenedor")%></td>
                  <td><%#Eval("confirmado")%></td>
                  <td id="tipoVerificacionTD" runat="server" visible="true"><%#Eval("tipoVerificacion")%></td>
                  <td><%#Eval("observacion")%></td>
                  <td>
                    <asp:DropDownList ID="servicioddl"  runat="server" Visible="false" CssClass="form-control">
                        <asp:ListItem Text="Si" Value="SI"></asp:ListItem>
                        <asp:ListItem Text="No" Value="NO"></asp:ListItem>
                    </asp:DropDownList>
                  </td>                  
                 </tr>
                  </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>
               
           

              </div>
               <div id="sinresultado" runat="server" class="  alert alert-primary">
              No se encontraron resultados,  asegurese que ha escrito correctamente el nombre/referencia del contenedor
              </div>
             </ContentTemplate>
             <Triggers>
             <asp:AsyncPostBackTrigger ControlID="save" />
             </Triggers>
             </asp:UpdatePanel>
       </div>
      
    <input id="json_object" type="hidden" />
   </div>

    </form>
   <script type="text/javascript" >
       function setContenedor(row) {
            var celColect = row.getElementsByTagName('td');
              var contenedor = {
                  item: celColect[0].textContent,
                  codigo: celColect[1].textContent,
                  booking: celColect[2].textContent,
                  tipoContenedor: celColect[3].textContent
              };

             if (window.opener != null) {
                 window.opener.popupCallback(contenedor);
            }
            self.close();
      }
      function msgfinder(control, expresa) {
          if (control.value.trim().length <= 0) {
              this.document.getElementById(expresa).textContent = 'Escriba una o varias letras del nombre/código y pulse buscar';
              return;
          }
          this.document.getElementById(expresa).textContent = 'Se buscará [' + control.value.toUpperCase() + '], presione el botón';
      }
      function initFinder() {
          if (document.getElementById('txtfinder').value.trim().length <= 0) {
            alertify.alert('Escriba una o varias letras para iniciar la búsqueda');
              return false;
          }
          document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif">';
      }
   </script>
</body>

</html>
