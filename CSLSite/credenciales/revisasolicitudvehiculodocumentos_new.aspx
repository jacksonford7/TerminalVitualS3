<%@ Page Language="C#"  AutoEventWireup="true" Title="Documentos Solicitud de Sticker/Provisional Vehícular" CodeBehind="revisasolicitudvehiculodocumentos_new.aspx.cs" Inherits="CSLSite.revisasolicitudvehiculodocumentos_new" %>
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

      <!--external css-->
  <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
  <link rel="stylesheet" type="text/css" href="../lib/gritter/css/jquery.gritter.css" />

    <!--mensajes-->
  <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
  <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>
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
 <div class="dashboard-container p-4" id="Div1" runat="server">
       <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Consola de Solicitudes</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Documentos vehículo</li>
          </ol>
        </nav>
      </div>

    <form id="bookingfrm" runat="server">
     <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
     <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>


         
       <div id="xfinder" runat="server">
      
            
           <div class="alert alert-warning" runat="server" id="alerta" >
                            Confirme que los documentos sean los correctos.
             </div>
      
          <div class="form-row">
                <div class="form-group col-md-12">
                <asp:Repeater ID="tablePaginationDocumentos" runat="server">
                <HeaderTemplate>
                <table id="tablar"  cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                <thead>
                <tr>
                <th>Tipo Cliente</th>
                <th>Documentos</th>
                <th></th>
                <th>Documento Rechazado</th>
                <th>Comentario</th>
                <th>Fecha Documento</th>
                </tr>
                </thead> 
                <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                <tr class="point">
                <td ><%#Eval("TIPOEMPRESA")%></td>
                <td ><%#Eval("DOCUMENTO")%></td>
                <td >
                    <a class="btn btn-outline-primary mr-4" href='<%#Eval("RUTADOCUMENTO") %>'  class="topopup" target="_blank">
                        <i class="fa fa-search"></i> Ver Documento </a>
                </td>
                <td >
                    <asp:CheckBox runat="server" ForeColor="Red" id="chkRevisado" Checked='<%#Eval("ESTADOVEH")%>'/>
                 
                    </td>
                <td>
                    <asp:TextBox ID="tcomentario" runat="server" Text='<%#Eval("COMENTARIO")%>' ForeColor="Red" 
                    ToolTip='<%#Eval("COMENTARIO")%>' class="form-control" onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox>
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
      
       
           <div class="row">
             <div class="col-md-12 d-flex justify-content-center">
              
                    <div id="sinresultado" runat="server" class="alert alert-warning" >No se encontraron resultados, asegurese que ha seleccionado correctamente un tipo de solicitud.
              </div>

             </div>
           </div>

           <div class="row">
                  <div class="col-md-12 d-flex justify-content-center">
                    <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
                    <asp:Button ID="btsalvar"  runat="server" Text="Regresar" class="btn btn-outline-primary mr-4" ToolTip="Regresa a la pantalla Solicitud de Sticker/Provisional Vehícular."
                                   OnClientClick="return prepareObject();" onclick="btsalvar_Click"/>
                 </div>
              </div>      
        
     </div>
    
      <asp:HiddenField runat="server" id="hfPlaca" />
 </form>
  </div>
     <script type="text/javascript">window.jQuery || document.write('<script src="../assets/js/vendor/jquery.slim.min.js"><\/script>')</script>
    <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
    <script type="text/javascript" src="../lib/jquery/feather.min.js"></script>
     <script src="../Scripts/pages.js" type="text/javascript"></script>


   
   <script src="../Scripts/credenciales.js" type="text/javascript"></script>
 
   <script type="text/javascript" >
       function setObject(row) {
            self.close();
        }
        function prepareObject() {
            try {
                var vehiculo = {};
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
                            alertify.alert('* Documentos Solicitud de Sticker/Provisional Vehícular: *\n * Escriba el Comentario del documento rechazado.*').set('label', 'Aceptar');
                            return false;
                        }
                        valida = "1";
                    }
                }
                vehiculo = {
                    valor: valida,
                    placa: document.getElementById('<%=hfPlaca.ClientID %>').value
                };
                if (window.opener != null) {
                    window.opener.popupCallback(vehiculo, '');
                }
                return true;
            } catch (e) {
                alertify.alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message).set('label', 'Aceptar');
            }
        }
      function initFinder() {
          if (document.getElementById('txtname').value.trim().length <= 0) {
              alertify.alert('Por favor escriba una o varias \nletras del número').set('label', 'Aceptar');
              return false;
          }
          document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif">';
      }
      function Quit() {
          //event.returnValue = '[ ! ] Se perdera el trabajo realizado [ ! ]';
          //return;
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

