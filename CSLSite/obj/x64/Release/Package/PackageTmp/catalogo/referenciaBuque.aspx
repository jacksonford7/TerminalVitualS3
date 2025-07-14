<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="referenciaBuque.aspx.cs"
    Inherits="CSLSite.referenciaBuque" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Referencias</title>
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
</head>
<body>

    <form id="bookingfrm" runat="server">
    <asp:ToolkitScriptManager ID="tkscata" runat="server">
    </asp:ToolkitScriptManager>
     <div class="dashboard-container p-4" id="cuerpo" runat="server">
		  <div class="form-row">
		   <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">Línea<span style="color: #FF0000; font-weight: bold;"></span></label>
			       <asp:TextBox ID="txtLinea" runat="server"
                       MaxLength="3" CssClass=" form-control"
                        onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')"
                        EnableViewState="False" placeholder="Linea"></asp:TextBox>
		   </div>
           <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">Año<span style="color: #FF0000; font-weight: bold;"></span></label>
			                     <asp:TextBox ID="txtAnio" runat="server"
                                      MaxLength="4"
                                     CssClass=" form-control"
                        onkeypress="return soloLetras(event,'1234567890')" EnableViewState="False" placeholder="Año"></asp:TextBox>

		   </div>
           <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">Buque<span style="color: #FF0000; font-weight: bold;"></span></label>
			     <asp:TextBox ID="txtBuque" runat="server"  MaxLength="11" 
                     CssClass=" form-control"
                        onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')"
                        EnableViewState="False" placeholder="Buque"></asp:TextBox>
		   </div>


		  </div>

          <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
               <div class="d-flex">
                            <asp:Button ID="find"  CssClass="btn btn-primary"
                        runat="server" Text="Buscar" 
                        OnClick="find_Click" OnClientClick="return initFinder();" />
                    <span id="imagen"></span>
               </div>
		   </div> 
		   </div>

          <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
                   <p class="  alert alert-light" id="valintro">
                    Escriba la línea y el año de la referencia y pulse buscar</p>
		   </div> 
		   </div>
		
            <div class="form-row">
		   <div class="col-md-12"> 
		                    <asp:UpdatePanel ID="upresult" runat="server">
                    <ContentTemplate>
                        <script type="text/javascript">                            Sys.Application.add_load(BindFunctions); </script>
                        <div class="  alert alert-warning" id="alerta" runat="server">
                        Confirme que los datos ingresados sean correctos.  En caso de error, por favor notifíquelo a las casilla ec.sac@contecon.com.ec o comuníquese a los teléfonos (04) 6006300 – 3901700 opción 4	
                        </div>
                        <div id="xfinder" runat="server" visible="false">
                        
                             
                                    <div class="form-title">
                                        Referencias</div>
                                  
                                        <asp:Repeater ID="tablePagination" runat="server">
                                            <HeaderTemplate>
                                                <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered table-sm table-contecon">
                                                    <thead>
                                                        <tr>
                                                            <th>
                                                                No.
                                                            </th>
                                                            <th>
                                                                Referencia
                                                            </th>
                                                            <th>
                                                                Agencia
                                                            </th>
                                                            <th>
                                                                Buque
                                                            </th>
                                                            <th>
                                                                Viaje
                                                            </th>
                                                            <th>
                                                                Tipo Reporte
                                                            </th>
                                                            <th style="display: none;">
                                                                Cutoff
                                                            </th>
                                                            <th>
                                                                Acciones
                                                            </th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr class="point" onclick="setContenedor(this);">
                                                    <td>
                                                        <%#Eval("noFila")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("referencia")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("agencia")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("buque")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("viaje")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("tipoReporte")%>
                                                    </td>
                                                    <td style="display: none;">
                                                        <%#Eval("cutoff")%>
                                                    </td>
                                                    <td>
                                                        <a href="#">Elegir</a>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </tbody> </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    
                             
                         
                
                        </div>
                        <div id="sinresultado" runat="server" class="  alert alert-primary">
                            No se encontraron resultados, asegurese que ha escrito correctamente el nombre/referencia
                            del contenedor
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="find" />
                    </Triggers>
                </asp:UpdatePanel>

		   </div> 
		   </div>
     </div>

    <input id="json_object" type="hidden" />
    </form>
    <script type="text/javascript">
        function setContenedor(row) {
            var celColect = row.getElementsByTagName('td');
            var buque = {
                referencia: celColect[1].textContent.trim(),
                nave: celColect[3].textContent.trim(),
                tipoReporte: celColect[5].textContent.trim(),
                cutoff: celColect[6].textContent.trim()
            };

            if (window.opener != null) {
                window.opener.popupCallback(buque);
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

            document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif">';
        }
    </script>
</body>
</html>
