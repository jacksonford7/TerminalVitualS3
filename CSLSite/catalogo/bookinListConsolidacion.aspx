﻿<%@ Page Language="C#" Title="Lista de Bookings" AutoEventWireup="true" 
         CodeBehind="bookinListConsolidacion.aspx.cs" Inherits="CSLSite.bookinListConsolidacion" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lista de Bookings</title>
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

</head>
<body>
    <form id="bookingfrm" runat="server">
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
    <div class="dashboard-container p-4" id="cuerpo" runat="server">
          	     <div class="form-row">
		   <div class="form-group col-md-12"> 
		   	  <label for="inputAddress">Numero de booking<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                      <asp:TextBox ID="txtname" runat="server"  CssClass="form-control" 
                 onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890-_')" 
                 MaxLength="50"  onkeyup="msgfinder(this,'valintro','Escriba un número de booking y pulse buscar');" ></asp:TextBox>  
             <asp:Button ID="find" runat="server"  CssClass="btn btn-primary"
                 Text="Buscar" onclick="find_Click" OnClientClick="return initFinder();" />
             <span id="imagen" ></span>

			  </div>
		   </div>
		  </div>
            <div class="form-row">
		   <div class="form-group col-md-12"> 
		   	  <p class= "alert alert-light" id="valintro" runat="server">Escriba un número de booking y pulse buscar</p>
		   </div>
		  </div>
         <div class="form-row">
		   <div class=" form-group  col-md-12 "> 
		          <div class="cataresult" >
             <asp:UpdatePanel ID="upresult" runat="server"  >
             <ContentTemplate>
             <script type="text/javascript">Sys.Application.add_load(BindFunctions); </script>
             <div id="xfinder" runat="server" visible="false" >
                 <div class=" alert   alert-warning">
                      <div  runat="server" id="alerta" ></div>

                 </div>
            
               
             </div>
          
            
                  <div class="form-title">Bookings / Reservas</div>
              
                 <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered table-sm table-contecon">
                 <thead>
                 <tr>
                 <th class="nover">No.</th>
                 <th class="nover">Key</th>
                 <th>Booking</th>
                 <th></th>
                 <th>Nave</th>
                 <th class="nover">Referencia</th>
                 <th class="nover">Ruc</th>
                 <th class="nover">Status</th>
                 <th>Corte</th>
                 <th>Booking</th>
                 <th>Reservado</th>
                 <th>Disponible</th>
                 <th></th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" onclick="setObject(this);">
                  <td class="nover" ><%#Eval("row")%></td>
                  <td  class="nover" ><%#Eval("gkey")%></td>
                  <td><%#Eval("nbr")%></td>
                  <td><%#Eval("alerta")%></td>
                  <td><%#Eval("name")%></td>
                  <td class="nover"><%#Eval("id")%></td>
                  <td class="nover"><%#Eval("ruc")%></td>
                  <td class="nover"><%#Eval("fk")%></td>
                  <td><%#Eval("cutof")%></td>
                  <td><%#Eval("cant_bkg")%></td>
                  <td><%#Eval("reservado")%></td>
                  <td><%#Eval("disponible")%></td>
                  <td>
                     <a href="#" class="btn btn-link" >Elegir</a>
                  </td>
                 </tr>
                  </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>
             
          
           
      
              </div>
              <div id="sinresultado" runat="server" class=" alert   alert-primary" >
              No se encontraron resultados, 
              asegurese que ha escrito correctamente el número/nombre
              buscado  
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
    </form>
   <script type="text/javascript" >
       function setObject(row) {
           var celColect = row.getElementsByTagName('td');
          var bookin = {
              fila: celColect[0].textContent,
              gkey: celColect[1].textContent,
              nbr: celColect[2].textContent,
              linea: celColect[3].textContent,
              fk: celColect[4].textContent
              };
            if (window.opener != null) {
                window.opener.popupCallback(bookin, 'bk');
            }
            self.close();
      }
      function initFinder() {
          if (document.getElementById('txtname').value.trim().length <= 0) {
            alertify.alert('Por favor escriba una o varias \nletras del número');
              return false;
          }
          document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif">';
      }

   </script>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
</body>
</html>
