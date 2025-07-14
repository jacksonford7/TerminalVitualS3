<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="naves_opc.aspx.cs" Inherits="CSLSite.naves_opc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Catálogo de naves</title>
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
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Nombre de nave/referencia<span style="color: #FF0000; font-weight: bold;"></span></label>
                  <asp:TextBox ID="txtfinder" 
                 runat="server"  
                 CssClass="form-control"
                 onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')" 
                 MaxLength="30" 
                 onkeyup="msgfinder(this,'valintro');"
                  ></asp:TextBox> 
		   </div>
		   <div class="form-group col-md-6"> 
               		   	  <label for="inputAddress">&nbsp;<span style="color: #FF0000; font-weight: bold;"></span></label>

			  <div class="d-flex">
                        <asp:Button ID="find" runat="server" CssClass="btn btn-primary"
                 Text="Buscar" onclick="find_Click" OnClientClick="return initFinder();" />
             <span id="imagen"></span>

			  </div>
		   </div>
		  </div>
		   <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
                        <p class="alert  alert-light" id="valintro">Escriba una o varias letras del nombre/referencia y pulse buscar</p>

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
                 Si una nave no se refleja en este catálogo es posible que que la misma se encuentre en estado DEPARTED/CLOSE
             </div>
    
                  <div class="form-title">Naves / Referencias</div>
              
                 <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered table-sm table-contecon">
                 <thead>
                 <tr>
                     <th class="nover">NO</th>
                     <th >Referencia</th>
                     <th>ETA</th>
                     <th>ETD</th>
                     <th>Nave</th>
                     <th class="nover">NO</th>
                     <th class="nover">NO</th>
                     <th>Viaje</th>
                     <th class="nover">NO</th>
                     <th class="nover">NO</th>
                     <th class="nover">NO</thclass="nover">
                     <th>Acciones</th>


                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" onclick="setNave(this);">
                      <td class="nover"><%#Eval("gkey")%></td>		
                     <td ><%#Eval("id")%></td>		
                     <td><%#Eval("eta")%></td>		
                     <td><%#Eval("etd")%></td>		
                     <td><%#Eval("name")%></td>		
                     <td class="nover"><%#Eval("ata")%></td>		
                     <td class="nover"><%#Eval("atd")%></td>		
                     <td ><%#Eval("viaje")%></td>		
                     <td class="nover"><%#Eval("muelle")%></td>		
                     <td class="nover"><%#Eval("starwork")%></td>		
                     <td class="nover"><%#Eval("endwork")%></td>		

                  
                     
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
               <div id="sinresultado" runat="server" class="alert  alert-primary">
                   No se encontraron resultados, asegurese que ha escrito correctamente el nombre / referencia de la nave
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
       function setNave(row) {
            var celColect = row.getElementsByTagName('td');
           var nave = {
                gkey=celColect[0].textContent,
                id: celColect[1].textContent,
                eta: celColect[2].textContent,
                etd: celColect[3].textContent,
                name: celColect[4].textContent,
                ata:celColect[5].textContent,
                atd:celColect[6].textContent,
                viaje:celColect[7].textContent,
                muelle:celColect[8].textContent,
                starwork:celColect[9].textContent,
                endwork:celColect[0].textContent
              };
             if (window.opener != null) {
                 window.opener.popupCallback(nave);
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
