<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="contenedoresExportacion.aspx.cs" Inherits="CSLSite.contenedoresExportacion" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
		   <div class="form-group col-md-10"> 
		   	  <label for="inputAddress">Contenedor:<span style="color: #FF0000; font-weight: bold;"></span></label>
		               <asp:TextBox ID="txtfinder" runat="server" 
                 MaxLength="11" CssClass="form-control"
              onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')"
              onBlur="checkDC(this,'valintro',true);" EnableViewState="False"
              placeholder="Contenedor"
            
             ></asp:TextBox>
		   </div>
		  
		   <div class="form-group col-md-2"> 
               		   	  <label for="inputAddress">&nbsp;<span style="color: #FF0000; font-weight: bold;"></span></label>

               <div class="d-flex">
                      <asp:Button ID="find"  CssClass="btn btn-primary"
                 runat="server" Text="Buscar" onclick="find_Click" 
                 OnClientClick="return initFinder();" />
             <span id="imagen"></span>

               </div>
		   </div> 
		   </div>
		 	   <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
            <p class=" alert alert-light" id="valintro">Escriba una o varias letras del código y pulse buscar</p>

		   </div> 
		   </div>

                 <div class="cataresult" >
             <asp:UpdatePanel ID="upresult" runat="server"  >
             <ContentTemplate>
             <script type="text/javascript">                 Sys.Application.add_load(BindFunctions); </script>
               <div id="xfinder" runat="server" visible="false" >
              <div class="alert alert-warning" id="alerta"  >
                Esta(s) transmisión(es) tienen un costo, el cual será facturado a su representada y deberá ser cancelado antes del embarque de su contenedor.
             </div>
           
                  <div class=" form-title">Contenedores</div>
              
                 <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered table-sm table-contecon">
                 <thead>
                 <tr>
                 <th align="center">No.</th>
                 <th align="center">Código</th>
                 <th align="center">Booking</th>
                 <th align="center" style="display: none">Tipo Contenedor</th>
                 <th align="center">DAE</th>
                 <th align="center" style="display: none">S1</th>
                 <th align="center" style="display: none">S2</th>
                 <th align="center" style="display: none">S3</th>
                 <th align="center" style="display: none">S4</th>
                 <th align="center" style="display: none">Carga</th>
                 <th align="center" style="display: none">Peso</th>
                 <th align="center" style="display: none">AISV</th>
                 <th align="center" style="display: none">iso</th>
                 <th align="center">Acciones</th></tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" onclick="setContenedor(this);">
                  <td><%#Eval("idConsecutivo")%></td>
                  <td><%#Eval("nombrecont")%></td>
                  <td><%#Eval("booking")%></td>
                  <td style="display:none"><%#Eval("tipoContenedor")%></td>
                  <td><%#Eval("dae")%></td>
                   <td style="display:none"><%#Eval("sello1")%></td>
                    <td style="display:none"><%#Eval("sello2")%></td>
                     <td style="display:none"><%#Eval("sello3")%></td>
                      <td style="display:none"><%#Eval("sello4")%></td>
                       <td style="display:none"><%#Eval("codigo_carga")%></td>
                        <td style="display:none"><%#Eval("pesoContenedor")%></td>
                        <td style="display:none"><%#Eval("aisv")%></td>
                        <td style="display:none"><%#Eval("iso")%></td>
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
               <div id="sinresultado" runat="server" class=" alert  alert-warning">
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
   <script type="text/javascript" >
       function setContenedor(row) {
          
           var celColect = row.getElementsByTagName('td');
           var contenedor = {
               item: celColect[0].textContent,
               codigo: celColect[1].textContent,
               booking: celColect[2].textContent,
               tipoContenedor: celColect[3].textContent,
               dae: celColect[4].textContent,
               sello1: celColect[5].textContent,
               sello2: celColect[6].textContent,
               sello3: celColect[7].textContent,
               sello4: celColect[8].textContent,
               codigoCarga: celColect[9].textContent,
               peso: celColect[10].textContent,
               aisv: celColect[11].textContent,
               iso: celColect[12].textContent
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