<%@ Page Language="C#" Title="Lista Cia. Transporte" AutoEventWireup="true" CodeBehind="consultabuques.aspx.cs" Inherits="CSLSite.consultabuques" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lista de Buques</title>
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
		   	  <label for="inputAddress">Buque/Nave<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                        <asp:TextBox ID="txtname" runat="server"  
                            CssClass=" form-control" placeholder="Nombre" 
                 onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')" 
                 MaxLength="50"  ></asp:TextBox>  
             <asp:Button ID="find" runat="server" Text="Buscar"
                 onclick="find_Click"  CssClass="btn btn-primary"
                 OnClientClick="return initFinder();"/>
             <span id="imagen"></span>

			  </div>
		   </div>
		  </div>
		 
             <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
                        <p class=" alert alert-light">Escriba una o varias caracteres del buque o referencia del mismo, luego pulse buscar</p>

		   </div>
		  </div>
                    <div class="cataresult" >
             <asp:UpdatePanel ID="upresult" runat="server"  >
             <ContentTemplate>
             <script type="text/javascript">Sys.Application.add_load(BindFunctions); </script>
             <div id="xfinder" runat="server" visible="false" >
             <div class="  alert alert-warning" runat="server" id="alerta" >
                            Confirme que los datos ingresados sean correctos.  En caso de error, por favor notifíquelo a las casilla ec.sac@contecon.com.ec o comuníquese a los teléfonos (04) 6006300 – 3901700 opción 4	.</div>

           
                  <div class="  form-title">Detalle de buques</div>
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered table-sm table-contecon">
                 <thead>
                 <tr>
                 <th>ID</th>
                 <th>REFERENCIA</th>
                 <th>BUQUE</th>
                 <th>Acciones</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" onclick="setObject(this);">
                  <td ><%#Eval("GKEY")%></td>
                  <td><%#Eval("REFERENCIA")%></td>
                  <td><%#Eval("NAME")%></td>
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
           

              </div>
              <div id="sinresultado" runat="server" class="msg-info" >
             
              </div>
             </ContentTemplate>
             <Triggers>
             <asp:AsyncPostBackTrigger ControlID="find" />
             </Triggers>
             </asp:UpdatePanel>
       </div>
     </div>
        
    </form>

   <script type="text/javascript" >
       function setObject(row) {
           var celColect = row.getElementsByTagName('td');
          var ciatrans = {
              ruc: celColect[0].textContent,
              razonsocial: celColect[1].textContent
              };
            if (window.opener != null) {
                window.opener.popupCallbackEmp(ciatrans, 'cia');
            }
            self.close();
      }
      function initFinder() {
          if (document.getElementById('txtname').value.trim().length <= 0) {
            alertify.alert('Por favor escriba el Ruc o Razón Social');
              return false;
          }
          if (document.getElementById('txtname').value.trim().length > 0 && document.getElementById('txtname').value.trim().length < 3) {
              alert('Escriba al menos 3 caracteres que sean parte del Ruc o Razón Social');
              return false;
          }
          document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif">';
      }


      
   </script>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
</body>
</html>
