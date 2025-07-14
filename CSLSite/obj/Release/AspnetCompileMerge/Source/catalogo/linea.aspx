<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="linea.aspx.cs" Inherits="CSLSite.linea" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Catálogo de líneas navieras</title>
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


 

      <!--Datatables-->
       <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />
       <link href="../css/datatables.min.css" rel="stylesheet" />
       <link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css"/>
        <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>
       <script type="text/javascript"  src="../js/datatables.js"></script>
       <script src="../Scripts/table_catalog.js" type="text/javascript"></script>  
       <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
    <!--Fin-->
       <script src="../Scripts/pages.js" type="text/javascript"></script>
</head>
<body>
    <form id="bookingfrm" runat="server">
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
           <div class="dashboard-container p-4" id="cuerpo" runat="server">
		  <div class="form-row">
		  
		   <div class="form-group col-md-8"> 
		   	  <label for="inputAddress">Nombre de la línea <span style="color: #FF0000; font-weight: bold;"></span></label>
			 <asp:TextBox ID="txtfinder" 
                 runat="server"  
                 CssClass=" form-control" 
                 onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890-*.')" 
                 MaxLength="30" 
                 onkeyup="msgfinder(this,'valintro');"
                  ></asp:TextBox>  
		   </div>
		 
		   <div class="form-group col-md-4"> 
               		   	  <label for="inputAddress">&nbsp; <span style="color: #FF0000; font-weight: bold;"></span></label>

		  <div class="d-flex">

               <asp:Button ID="find" runat="server" Text="Buscar"  CssClass="btn btn-primary"
                 onclick="find_Click" OnClientClick="return initFinder();" />
             <span id="imagen"></span>

		  </div>
		   </div>
		  </div>
          <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		          <p class=" alert  alert-light" id="valintro">Escriba una o varias letras del nombre y pulse buscar</p>

		   </div>
		  </div>


   <div class="form-row">
		   <div class="col-md-12 "> 
		   <div class="cataresult" >
             <asp:UpdatePanel ID="upresult" runat="server"  >
             <ContentTemplate>
             <script type="text/javascript">Sys.Application.add_load(BindFunctions); </script>
               <div id="xfinder" runat="server" visible="false" >
             <div class="  alert alert-warning" id="alerta" >
                Confirme que los datos ingresados sean correctos.  En caso de error, por favor notifíquelo a las casilla ec.sac@contecon.com.ec o comuníquese a los teléfonos (04) 6006300 – 3901700 opción 4	
             </div>
  
            
                  <div class="form-title">Líneas Navieras</div>
                
                 <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered table-sm table-contecon">
                 <thead>
                 <tr>
                 <th>No.</th>
                 <th>Código</th>
                 <th>Descripción</th>
                 <th>Acciones</th></tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" onclick="setBooking(this);">
                  <td><%#Eval("number")%></td>
                  <td><%#Eval("codigo")%></td>
                  <td><%#Eval("descripcion")%></td>
                  <td>
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
               <div id="sinresultado" runat="server" class=" alert alert-primary">
            No se encontraron resultados, asegúrese que haya escrito correctamente el nombre de la línea buscada
              </div>
             </ContentTemplate>
             <Triggers>
             <asp:AsyncPostBackTrigger ControlID="find" />
             </Triggers>
             </asp:UpdatePanel>
       </div>

		   </div> 
		   </div>

     </div>



    <input id="json_object" type="hidden" />
    </form>
   
   <script type="text/javascript" >
       function setBooking(row) {
            var celColect = row.getElementsByTagName('td');
              var linea = {
                  item: celColect[0].textContent,
                  codigo: celColect[1].textContent,
                  descripcion: celColect[2].textContent
              };
              if (window.opener != null) {
                 window.opener.popupCallback(linea, 'lineaCI');
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
