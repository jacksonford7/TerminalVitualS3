﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="appListEventos.aspx.cs" Inherits="CSLSite.appListEventos"  Title="Buscar Eventos N4" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Buscar Eventos</title>
   
  
   <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

    <!--external css-->
  <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
  <link rel="stylesheet" type="text/css" href="../lib/gritter/css/jquery.gritter.css" />

    <!--mensajes-->
  <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
  <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>
  <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>

    <script type="text/javascript">
       function BindFunctions(){
           $(document).ready(function () {
               document.getElementById('imagen').innerHTML = '';
               $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 5: { sorter: false }, 6: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
           });
        }
    </script>
</head>
<body>
   <div class="dashboard-container p-4" id="cuerpo" runat="server">
    <form id="bookingfrm" runat="server">
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
    <div class="form-row">
        <div class="form-group col-md-12"> 
              <label for="inputEmail4">Nombre del evento:</label>
             <div class="d-flex">
                
                 <asp:TextBox ID="txtfinder" 
                 runat="server"  
                class="form-control"
                 onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890-*.')" 
                 MaxLength="30" 
                 onkeyup="msgfinder(this,'valintro');"
                  ></asp:TextBox>  
                  <asp:LinkButton runat="server" ID="find" Text="<span class='fa fa-search' style='font-size:24px'></span>"  class="btn btn-outline-primary mr-4"   
                              onclick="find_Click" OnClientClick="return initFinder();"/>
                 <span id="imagen"></span>
             </div>  
        </div>
        <div  class="alert alert-warning" id="valintro" runat="server" clientidmode="Static">
                          Escriba una o varias letras para buscar, y presione buscar
        </div>
    </div>  
 
      <div class="row">
         <div class="form-group col-md-12"> 
               <asp:UpdatePanel ID="upresult" runat="server"  >
             <ContentTemplate>
             <script type="text/javascript">Sys.Application.add_load(BindFunctions); </script>
             <div id="xfinder" runat="server" visible="false" >

                  <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                 <thead>
                 <tr>
 
                 <th scope="col">ID</th>
                 <th scope="col">NOMBRE </th>
                 <th scope="col">Accion</th>

                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" onclick="setEventos(this);">
                  <td scope="row"><%#Eval("id")%></td>
                  <td scope="row"><%#Eval("name")%></td>
                  <td style="text-align:center;">
                     <a href="#" >Elegir</a>
                  </td>
                 </tr>
                  </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>
                  
             <div id="pager">
             Registros por página
                  <select class="pagesize">
                  <option selected="selected" value="10">10</option>
                  <option value="20">20</option>
                  </select>
                 <img alt="" src="../shared/imgs/first.gif" class="first"/>
                 <img alt="" src="../shared/imgs/prev.gif" class="prev"/>
                 <input  type="text" class="pagedisplay" size="5px"/>
                 <img alt="" src="../shared/imgs/next.gif" class="next"/>
                 <img alt="" src="../shared/imgs/last.gif" class="last"/>
            </div>

              </div>
               <div id="sinresultado" runat="server" class="alert alert-warning">
            No se encontraron resultados, asegúrese que haya escrito correctamente el nombre de la línea buscada
              </div>
             </ContentTemplate>
             <Triggers>
             <asp:AsyncPostBackTrigger ControlID="find" />
             </Triggers>
             </asp:UpdatePanel>
         </div>  
     </div> 

    <input id="json_object" type="hidden" />
    </form>
 
  </div>  
    
    <script type="text/javascript">window.jQuery || document.write('<script src="../assets/js/vendor/jquery.slim.min.js"><\/script>')</script>
    <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
    <script type="text/javascript" src="../lib/jquery/feather.min.js"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
    <script src="../Scripts/pages.js" type="text/javascript"></script>

    <script type="text/javascript" >

       function setEventos(row)
       {
            var celColect = row.getElementsByTagName('td');
              var lookup_get_evento = {
                  sel_g_id: celColect[0].textContent,
                  sel_g_nombre: celColect[1].textContent,
                 
           };

           if (window.opener != null) {

               window.opener.popupCallback_Eventos(lookup_get_evento);
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
               alertify.alert('Advertencia','Escriba una o varias letras para iniciar la búsqueda').set('label', 'Aceptar');
              return false;
          }
          document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif">';
      }
   </script>
</body>

</html>
