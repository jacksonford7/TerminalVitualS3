<%@ Page Language="C#" AutoEventWireup="true" Title="Documentos Solicitud de Credencial/Permiso Provisional"
CodeBehind="revisasolicitudcolaboradordocumentos_new.aspx.cs" Inherits="CSLSite.revisasolicitudcolaboradordocumentos_new" %>
<%--<%@ MasterType VirtualPath="~/site.Master" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

     <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
  <link rel="stylesheet" type="text/css" href="../lib/gritter/css/jquery.gritter.css" />


   <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>
   <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
  <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    
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
     <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
   
        <div class="dashboard-container p-4" id="cuerpo" runat="server">
         <div class="form-title">
            Documentos Colaborador
        </div>
         <div class="form-row">

         <div class="cataresult" >
       <div id="xfinder" runat="server">
       <div class="findresult" >
             <div class="msg-alerta" runat="server" id="alerta" >
                            Confirme que los documentos sean los correctos.
             </div>
      
        <div class="informativo">
            <asp:Repeater ID="tablePaginationDocumentos" runat="server">
            <HeaderTemplate>
            <table id="tablar"  cellspacing="1" cellpadding="1" class="table table-bordered invoice">
            <thead>
            <tr>
            <th>Tipo de Empresa</th>
            <th>Documentos</th>
            <th>Ver</th>
            <th>Documento Rechazado</th>
            <th>Comentario</th>
           <th>Fecha Documento</th>
            </tr>
            </thead> 
            <tbody>
            </HeaderTemplate>
            <ItemTemplate>
            <tr class="point">
            <td style=" width:150px"><%#Eval("TIPOEMPRESA")%></td>
            <td style=" width:350px; font-size:inherit"><%#Eval("DOCUMENTO")%></td>
            <td style=" width:80px">
               
                <a class="btn btn-outline-primary mr-4" href='<%#Eval("RUTADOCUMENTO")%>'   target="_blank">
                  <i class="fa fa-search" ></i> Documento
                    </a>
            </td>
            <td style=" width:70px"><asp:CheckBox runat="server" ForeColor="Red" id="chkRevisado" Checked='<%#Eval("ESTADOVEH")%>'/></td>

            <td><asp:TextBox ID="tcomentario" class="form-control" runat="server" Text='<%#Eval("COMENTARIO")%>' ForeColor="Red" ToolTip='<%#Eval("COMENTARIO")%>' 
                onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox>

            </td>
              <td>
               <asp:TextBox ID="txtfecha" runat="server" Text='<%#Eval("FECHA_DOCUMENTO")%>'
                    ToolTip='<%#Eval("FECHA_DOCUMENTO")%>'  onkeypress="return soloLetras(event,'0123456789-')"    class="datetimepicker form-control"></asp:TextBox>
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
              No se encontraron resultados, 
              asegurese que ha seleccionado correctamente un tipo de solicitud.
              </div>
              <div class="botonera">
              <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
                <asp:Button ID="btsalvar" runat="server" Text="Regresar" class="btn btn-outline-primary mr-4" tooltip="Regresa a la pantalla Solicitud de Sticker/Provisional Vehícular."
                               OnClientClick="return prepareObject();" onclick="btsalvar_Click"/>
       </div>      
        </div>
      </div>
             </div>
            
  </div>
      <asp:HiddenField runat="server" id="hfCedula" />
        </form>
      
   <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script type="text/javascript">window.jQuery || document.write('<script src="../assets/js/vendor/jquery.slim.min.js"><\/script>')</script>
    <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
    <script type="text/javascript" src="../lib/jquery/feather.min.js"></script>
     <script src="../Scripts/pages.js" type="text/javascript"></script>
   <script src="../Scripts/credenciales.js" type="text/javascript"></script>
 

   <script type="text/javascript" >
        function prepareObject() {
            try {
                var colaborador = {};
                lista = [];
                var valida = "0";
                var tbl = document.getElementById('tablar');
                for (var f = 0; f < tbl.rows.length; f++) {
                    var celColect = tbl.rows[f].getElementsByTagName('td');
                    if (celColect != undefined && celColect != null && celColect.length > 0) {
                        var tdetalle = {
                            documento: celColect[3].getElementsByTagName('input')[0].checked,
                            comentario: celColect[4].getElementsByTagName('input')[0].value
                        };
                        this.lista.push(tdetalle);
                    }
                }                
                for (var n = 0; n < this.lista.length; n++) {
                    if (lista[n].documento == true) {
                        if (lista[n].comentario == '' || lista[n].comentario == null || lista[n].comentario == undefined) {
                            alert('* Documentos Solicitud de Credencial/Permiso Provisional: *\n * Escriba el Comentario del documento rechazado.*');
                            return false;
                        }
                        valida = "1";
                    }
                }
                colaborador = {
                    valor: valida,
                    cedula: document.getElementById('<%=hfCedula.ClientID %>').value
                };
                if (window.opener != null) {
                    window.opener.popupCallback(colaborador, '');
                }
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }
      function initFinder() {
          if (document.getElementById('txtname').value.trim().length <= 0) {
              alert('Por favor escriba una o varias \nletras del número');
              return false;
          }
          document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif">';
      }
      function Quit() {
         
      }
      function verDocumento(val) {
          var caja = val;
          window.open('../credenciales/solicituddocumentos/?placa=' + caja, 'name', 'width=850,height=480')
      }
      function setComentario(control) {
          var ext = control.getAttribute("extension");
          alert(ext)
      }
   </script>

        <script type="text/javascript">
            $(document).ready(function () {
                 $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'd/m/Y' });
              });    
      </script>

</body>

</html>
