<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="buque.aspx.cs" Inherits="CSLSite.buque" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Catálogo Buques</title>
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
     <script src="../Scripts/pages.js" type="text/javascript"></script>
            <!--mensajes-->
        <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
        <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>

</head>
<body>
    <form id="bookingfrm" runat="server">
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
   
        <div class="dashboard-container p-4" id="cuerpo" runat="server">

         <div class="form-title">Datos del documento buscado</div>
		 
		  <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		   	  <label for="inputAddress">Número IMO<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                      <asp:TextBox ID="txtfinder" 
                 runat="server"  
                 CssClass=" form-control" 
                 onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890-*.')" 
                 MaxLength="30" 
                 onkeyup="msgfinder(this,'valintro');"
                  ></asp:TextBox>  
             <asp:Button ID="find" CssClass="btn btn-primary" runat="server" Text="Buscar" onclick="find_Click" OnClientClick="return initFinder();" />
             <span id="imagen"></span>

			  </div>
		   </div>
		  </div>

               <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
		                 <p class=" alert alert-light" id="valintro">Escriba una o varias letras del nombre y pulse buscar</p>

		   </div> 
		   </div>

              <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
		     <div class="cataresult" >
             <asp:UpdatePanel ID="upresult" runat="server"  >
             <ContentTemplate>
             <script type="text/javascript">Sys.Application.add_load(BindFunctions); </script>
               <div id="xfinder" runat="server" visible="false" >
             <div class="   alert alert-warning" id="alerta" >
                Confirme que los datos ingresados sean correctos.  En caso de error, por favor notifíquelo a las casilla ec.sac@contecon.com.ec o comuníquese a los teléfonos (04) 6006300 – 3901700 opción 4	
             </div>
                    
           
                  <div class=" form-title">Buques / Ships</div>
                
                 <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered table-sm table-contecon">
                 <thead>
                 <tr>
                 <th>No.</th>
                 <th class="nover">bkey</th>
                 <th>IMO</th>
                 <th>Nombre</th>
                 <th>Flag</th>
                 <th>Linea</th>
                 <th class="nover">blargo</th>
                 <th class="nover">bancho</th>
                 <th class="nover">bservicio</th>
                 <th class="nover">bgros</th>
                  <th class="nover">bnet</th>
                  <th class="nover">bradio</th>
                   <th class="nover">btipo</th>
                 <th>Acciones</th></tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" onclick="setBooking(this);">
                  <td><%#Eval("bnumber")%></td>
                  <td class="nover"><%#Eval("bkey")%></td>
                  <td><%#Eval("bid")%></td>
                  <td><%#Eval("bnombre")%></td>
                  <td><%#Eval("bpais")%></td>
                  <td><%#Eval("bline")%></td>
                  <td class=" nover"><%#Eval("blargo")%></td>
                  <td class="nover"><%#Eval("bancho")%></td>
                  <td class="nover"><%#Eval("bservicio")%></td>
                  <td class="nover"><%#Eval("bgros")%></td>
                  <td class="nover"><%#Eval("bnet")%></td>
                  <td class="nover"><%#Eval("bradio")%></td>
                  <td class="nover"><%#Eval("btipo")%></td>
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
               <div id="sinresultado" runat="server" class="alert  alert-warning ">
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
           self.close();
           var celColect = row.getElementsByTagName('td');
           var buque = {
               bnumber: celColect[0].textContent,
               bkey: celColect[1].textContent,
               bid: celColect[2].textContent,
               bnombre: celColect[3].textContent,
               bpais: celColect[4].textContent,
               bline: celColect[5].textContent,
               blargo: celColect[6].textContent,
               bancho: celColect[7].textContent,
               bservicio: celColect[8].textContent,
               bgros: celColect[9].textContent,
               bnet: celColect[10].textContent,
               bradio: celColect[11].textContent,
               btipo: celColect[12].textContent
           };
           if (window.opener != null) {
               window.opener.popupCallback(buque, 'Buque');
           }
       }
       function msgfinder(control, expresa) {
           if (control.value.trim().length <= 0) {
               this.document.getElementById(expresa).textContent = 'Escriba una o varias letras del nombre/IMO y pulse buscar';
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
