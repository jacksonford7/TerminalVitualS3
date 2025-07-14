<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="consultaContenedores.aspx.cs" Inherits="CSLSite.consultaContenedores" %>
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
       <!--mensajes-->
        <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
        <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>
      

    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function BindFunctions() {
            document.getElementById('imagen').innerHTML = '';
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
		   	  <label for="inputAddress">Contenedor<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
       <asp:TextBox ID="txtfinder" 
           runat="server"  MaxLength="11" CssClass="form-control"
              onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')"
              onBlur="checkDC(this,'valintro',true);" EnableViewState="False"
              placeholder="Contenedor"
            
             ></asp:TextBox>
             <asp:Button ID="find" runat="server" CssClass="btn btn-primary"
                 Text="Buscar" onclick="find_Click" OnClientClick="return initFinder();" />
             <span id="imagen"></span>
			  </div>
		   </div>
		  </div>
		    <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
		           <p class="alert  alert-light" id="valintro">Escriba una o varias letras del código y pulse buscar</p>
           </div> 
		   </div>

                     <div class="cataresult" >
             <asp:UpdatePanel ID="upresult" runat="server"  >
             <ContentTemplate>
             <script type="text/javascript">Sys.Application.add_load(BindFunctions); </script>
               <div id="xfinder" runat="server" visible="false" >
             <div class="  alert alert-warning" id="alerta" >
                 Confirme que los datos sean correctos. En caso de error, favor comuníquese con 
                 el Departamento de Planificación a los teléfonos: +593 (04) 6006300, 3901700 
                 ext. 4039, 4040, 4060. 
             </div>
       
        
                  <div class="form-title">Contenedores</div>
              
                 <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered table-sm table-contecon">
                 <thead>
                 <tr>
                 <th>No.</th>
                 <th>Código</th>
                 <th>Booking</th>
                 <th style="display: none">Tipo Contenedor</th>
                 <th>Acciones</th></tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" onclick="setContenedor(this);">
                  <td ><%#Eval("idConsecutivo")%></td>
                  <td ><%#Eval("nombrecont")%></td>
                  <td ><%#Eval("booking")%></td>
                  <td align="center" style="display:none"><%#Eval("tipoContenedor")%></td>
                  <td >
                     <a href="#" class=" btn btn-link" >Elegir</a>
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
             <asp:AsyncPostBackTrigger ControlID="find" />
             </Triggers>
             </asp:UpdatePanel>
       </div>
 
    <input id="json_object" type="hidden" />

     </div>






    </form>

   <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
  
    

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
