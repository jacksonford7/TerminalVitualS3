<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="empresas.aspx.cs" Inherits="CSLSite.empresas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Catálogo de empresas</title>
         <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogo_mod.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function BindFunctions() {
            $(document).ready(function () {
                document.getElementById('imagen').innerHTML = '';
                $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 5: { sorter: false }, 6: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
            });
        }
    </script>
</head>
<body>

    <form id="bookingfrm" runat="server">
    <asp:ToolkitScriptManager ID="tkscata" runat="server"> </asp:ToolkitScriptManager>
  
             <div class="dashboard-container p-4" id="cuerpo" runat="server">
		  <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		   	  <label for="inputAddress">Nombre de la empresa<span style="color: #FF0000; font-weight: bold;"></span></label>
			    <asp:TextBox ID="txtfinder" 
                    runat="server" CssClass="form-control"
                    onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890-*.')"
                    MaxLength="50" 
                    onkeyup="msgfinder(this,'valintro');">
			    </asp:TextBox>

		   </div>
		  </div>
          <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		   	  <label for="inputAddress"> Identificacion de la Empresa<span style="color: #FF0000; font-weight: bold;"></span></label>
			   
                    <asp:TextBox ID="txtIdentificacion" runat="server" CssClass="form-control"
                        onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890-*.')"
                        MaxLength="50" onkeyup="msgfinder(this,'valintro');"></asp:TextBox>
		   </div>
		  </div>
          <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
               <div class="d-flex">
                           <asp:Button ID="find"  CssClass="btn btn-primary"
                        runat="server" Text="Buscar" OnClick="find_Click" OnClientClick="return initFinder();" />
                    <span id="imagen"></span>

               </div>
		   </div> 
		   </div>
		    <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
                          <p class="alert-dismissible" id="valintro">
                    Escriba una o varias letras del nombre y pulse buscar</p>
		   </div> 
		   </div>
                             <div class="cataresult">
                <asp:UpdatePanel ID="upresult" runat="server">
                    <ContentTemplate>
                        <script type="text/javascript">                            Sys.Application.add_load(BindFunctions); </script>
                        <div id="xfinder" runat="server" visible="false">
                            <div class="alert alert-warning" id="alerta">
                                Confirme que los datos ingresados sean correctos.  En caso de error, por favor notifíquelo a las casilla ec.sac@contecon.com.ec o comuníquese a los teléfonos (04) 6006300 – 3901700 opción 4	
                            </div>
                            <%-- catalogo de bookings--%>
                            <div class="findresult">
                                <div class="booking">
                                    <div class=" form-title">
                                        Empresas</div>
                                    <div class="bokindetalle">
                                        <asp:Repeater ID="tablePagination" runat="server">
                                            <HeaderTemplate>
                                                <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                                                    <thead>
                                                        <tr>
                                                            <th>
                                                                Identificación de la Empresa
                                                            </th>
                                                            <th>
                                                                Nombre de la Empresa
                                                            </th>
                                                            <th style="display: none;">
                                                                direccion
                                                            </th>
                                                            <th style="display: none;">
                                                                telefono
                                                            </th>
                                                            <th style="display: none;">
                                                                fax
                                                            </th>
                                                            <th style="display: none;">
                                                                website
                                                            </th>
                                                            <th style="display: none;">
                                                                correo
                                                            </th>
                                                            <th>
                                                                Acciones
                                                            </th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr class="point" onclick="setEmpresa(this);">
                                                    <td>
                                                        <%#Eval("identificacion")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("nombre")%>
                                                    </td>
                                                    <td style="display: none;">
                                                        <%#Eval("direccion")%>
                                                    </td>
                                                    <td style="display: none;">
                                                        <%#Eval("telefono")%>
                                                    </td>
                                                    <td style="display: none;">
                                                        <%#Eval("fax")%>
                                                    </td>
                                                    <td style="display: none;">
                                                        <%#Eval("website")%>
                                                    </td>
                                                    <td style="display: none;">
                                                        <%#Eval("email1")%>
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
                                </div>
                            </div>
                            <div id="pager">
                                Registros por página
                                <select class="pagesize">
                                    <option selected="selected" value="10">10</option>
                                    <option value="20">20</option>
                                </select>
                                <img alt="" src="../shared/imgs/first.gif" class="first" />
                                <img alt="" src="../shared/imgs/prev.gif" class="prev" />
                                <input type="text" class="pagedisplay" />
                                <img alt="" src="../shared/imgs/next.gif" class="next" />
                                <img alt="" src="../shared/imgs/last.gif" class="last" />
                            </div>
                        </div>
                        <div id="sinresultado" runat="server" class=" alert alert-primary">
                            No se encontraron resultados, asegúrese que haya escrito correctamente el nombre
                            de la empresa buscada
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
    <script type="text/javascript">
        function setEmpresa(row) {
            var celColect = row.getElementsByTagName('td');
            var empresa = {
                identificacion: celColect[0].textContent,
                nombreEmpresa: celColect[1].textContent,
                direccionEmpresa: celColect[2].textContent,
                telefonoEmpresa: celColect[3].textContent,
                faxEmpresa: celColect[4].textContent,
                websiteEmpresa: celColect[5].textContent,
                correoEmpresa: celColect[6].textContent
            };
            if (window.opener != null) {
                window.opener.document.getElementById('empresa').textContent = empresa.nombreEmpresa;
                window.opener.setDataEmpresa(empresa.nombreEmpresa.trim(), empresa.identificacion.trim(), empresa.direccionEmpresa.trim(), empresa.faxEmpresa.trim(), empresa.telefonoEmpresa.trim(), empresa.correoEmpresa.trim(), empresa.websiteEmpresa.trim());
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
