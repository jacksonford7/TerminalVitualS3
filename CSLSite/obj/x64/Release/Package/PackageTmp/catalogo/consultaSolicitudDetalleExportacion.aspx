
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="consultaSolicitudDetalleExportacion.aspx.cs" Inherits="CSLSite.consultaSolicitudDetalleExportacion" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Solicitud Detalle</title>
         <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>
    
    <!--mensajes-->
        <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
        <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>


    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
               function BindFunctions() {
           $(document).ready(function () {
               $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 5: { sorter: false }, 6: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
           });
        }
    </script>
</head>
<body>
    <form id="bookingfrm" runat="server">
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
         <div class="dashboard-container p-4" id="cuerpo" runat="server">

		  <div class="form-row">
		   <div class="form-group col-md-12"> 
	         <div class="cataresult" >
             <asp:UpdatePanel ID="upresult" runat="server"  >
             <ContentTemplate>
             <script type="text/javascript">                 Sys.Application.add_load(BindFunctions); </script>
               <div id="xfinder" runat="server" visible="false" >
             <div class="alert-primary " id="alerta" >
                 Confirme que los datos sean correctos. En caso de error, favor comuníquese con 
                 el Departamento de Planificación a los teléfonos: +593 (04) 6006300, 3901700 
                 ext. 4039, 4040, 4060. 
             </div>
   
             <div class="booking" >
                  <div class="form-title">Solicitudes</div>
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                 <thead>
                 <tr>
                 <th align="center">No.</th>
                 <th align="center">Contenedor</th>
                 <th align="center">Número de Entrega</th>
                 <th align="center">Aisv</th>
                 <th align="center">DAE de solicitud</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point">
                  <td><%#Eval("noFila")%></td>
                  <td><%#Eval("descripcionContenedor")%></td>
                  <td><%#Eval("numeroEntrega")%></td>  
                  <td><%#Eval("aisv")%></td>
                  <td><%#Eval("dae")%></td>                      
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
               <div id="sinresultado" runat="server" class=" alert-primary">
              No se encontraron resultados,  asegurese que ha escrito correctamente el nombre/referencia del contenedor
              </div>
             </ContentTemplate>
             <Triggers>
             </Triggers>
             </asp:UpdatePanel>
       </div>
 
    <input id="json_object" type="hidden" />
		   </div>
		  </div>
		 
     </div> 
        
        
        


    </form>
    <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
   <script type="text/javascript" >
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
