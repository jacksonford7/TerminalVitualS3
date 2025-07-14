<%@ Page Title="Consulta Pago a Terceros" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="consulta_pago_terceros_expo.aspx.cs" Inherits="CSLSite.consulta_pago_terceros_expo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
     
    <!--Datatables-->
       <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />
       <link href="../css/datatables.min.css" rel="stylesheet" />
       <link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css"/>
        <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>
       <script type="text/javascript"  src="../js/datatables.js"></script>
       <script src="../Scripts/table_catalog.js" type="text/javascript"></script>  
       <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
    <!--Fin-->
    
    
    
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
     <script src="../Scripts/pages.js" type="text/javascript"></script>
      <!--mensajes-->
        <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
        <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>
    
    <script type="text/javascript">
        function imprSelec(muestra) {
            var ficha = document.getElementById(muestra); var ventimp = window.open(' ', 'popimpr'); ventimp.document.write(ficha.innerHTML); ventimp.document.close(); ventimp.print(); ventimp.close();
        }
    </script>
    <style type="text/css">
        .warning {
            background-color: Yellow;
            color: Red;
        }

        #progressBackgroundFilter {
            position: fixed;
            bottom: 0px;
            right: 0px;
            overflow: hidden;
            z-index: 1000;
            top: 0;
            left: 0;
            background-color: #CCC;
            opacity: 0.8;
            filter: alpha(opacity=80);
            text-align: center;
        }

        #processMessage {
            text-align: center;
            position: fixed;
            top: 30%;
            left: 43%;
            z-index: 1001;
            border: 5px solid #67CFF5;
            width: 200px;
            height: 100px;
            background-color: White;
            padding: 0;
        }

        #aprint {
            color: #666;
            border: 1px solid #ccc;
            -moz-border-radius: 3px;
            -webkit-border-radius: 3px;
            background-color: #f6f6f6;
            padding: 0.3125em 1em;
            cursor: pointer;
            white-space: nowrap;
            overflow: visible;
            font-size: 1em;
            outline: 0 none /* removes focus outline in IE */;
            margin: 0px;
            line-height: 1.6em;
            background-image: url(../shared/imgs/action_print.gif);
            background-repeat: no-repeat;
            background-position: left center;
            text-decoration: none;
            padding: 5px 2px 5px 30px;
        }

        * input[type=text] {
            text-align: left !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
 
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"></asp:ToolkitScriptManager>


        <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Gestión financiera</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Consulta de pagos a terceros</li>
          </ol>
        </nav>
      </div>


       <div class="dashboard-container p-4" id="cuerpo" runat="server">
         <div class="form-title">Datos del documento buscado</div>
		   <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		   	  <label for="inputAddress">Referencia<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                                          <asp:TextBox ID="txtReferencia" 
                                              runat="server" CssClass="form-control"
                             onblur="cajaControl(this);"
                            onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz01234567890-_/ ,',true)"></asp:TextBox>

                <asp:Button ID="btbuscar"  CssClass="btn btn-primary"
                    runat="server" Text="Iniciar la búsqueda" OnClientClick="return prepareObject()"
                    OnClick="btbuscar_Click" />
                                                                     <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover" />


			  </div>
		   </div>
		  </div>
              <div class="form-row">
		   <div class="col-md-12 "> 
		            

              
        
               
                      <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
                <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
                   
                        <div class="informativo" style="height: 95%; overflow: auto">
                       
                       
                                <asp:Repeater ID="tablePagination" runat="server">
                                    <HeaderTemplate>
                                        <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered table-sm table-contecon">
                                            <thead>
                                                <tr>
                                                    <th>Ruc del que Asume</th>
                                                    <th>Usuario Ingreso</th>
                                                    <th>Fecha Ingreso</th>
                                                    <th>Razon Social del que Asume</th>
                                                    <th>Ruc Ciente</th>
                                                    <th>Razon Social Cliente</th>
                                                    <th>Referencia</th>
                                                    <th>Booking</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr class="point">
                                            <td><%#Eval("SEPT_RUC_CLIENTE")%></td>
                                            <td><%#Eval("SEPT_USUARIOINGRESO")%></td>
                                            <td><%#Eval("SEPT_FECHAINGRESO")%></td>
                                            <td><%#Eval("NAME_RUC_CLIENTE")%></td>
                                            <td><%#Eval("SEPT_RUC_ASUME")%></td>
                                            <td><%#Eval("NAME_RUC_ASUME")%></td>
                                            <td><%#Eval("SAPT_REFERENCIA")%></td>
                                            <td><%#Eval("BOOKING")%></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                 </table>
                 </div>
                                    </FooterTemplate>
                                </asp:Repeater>
                           
                        </div>
                
           
           

        
               
                
                <div id="sinresultado" runat="server" class="  alert  alert-warning">No se encontraron resultados, asegurese que ha escrito correctamente los criterios de consulta.</div>

          
		   </div> 
		   </div>
                   <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center "> 
		                         <input clientidmode="Static" id="dataexport" onclick="generateexcel();" type="button" value="Exportar" runat="server" class="btn  btn-primary" />

		   </div> 
		   </div>
     </div>
      <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });
        function generateexcel(tableid) {
            var table = document.getElementById('tablasort');
            var html = table.outerHTML;
            window.open('data:application/vnd.ms-excel,' + encodeURIComponent(html));
        }
        var programacion = {};
        var lista = [];
        function prepareObject() {

            try {

                document.getElementById("loader").className = '';
                return true;
            } catch (e) {
              alertify.alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

        function getGifOculta() {
            document.getElementById('loader').className = 'nover';
        }
    </script>
    <asp:UpdateProgress ID="updateProgress" runat="server">
        <ProgressTemplate>
            <div id="progressBackgroundFilter"></div>
            <div id="processMessage">
                Estamos procesando la tarea que solicitó, por favor  espere...<br />
                <img alt="Loading" src="../shared/imgs/loader.gif" style="margin: 0 auto;" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
