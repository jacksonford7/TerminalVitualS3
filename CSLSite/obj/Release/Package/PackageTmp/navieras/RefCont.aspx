<%@ Page Title="Refeers" Language="C#" MasterPageFile="~/site.Master" 
         AutoEventWireup="true" CodeBehind="RefCont.aspx.cs" Inherits="CSLSite.ref_cont" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/turnos.js" type="text/javascript"></script>

    <link href="../img/favicon2.png" rel="icon"/>
    <link href="../img/icono.png" rel="apple-touch-icon"/>
    <link href="../css/bootstrap.min.css" rel="stylesheet"/>
    <link href="../css/dashboard.css" rel="stylesheet"/>
    <link href="../css/icons.css" rel="stylesheet"/>
    <link href="../css/style.css" rel="stylesheet"/>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

    <link href="../shared/estilo/Reset.css" rel="stylesheet" />
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />



    <style type="text/css">
        .warning { background-color:Yellow;  color:Red;}

  #progressBackgroundFilter {
    position:fixed;
    bottom:0px;
    right:0px;
    overflow:hidden;
    z-index:1000;
    top: 0;
    left: 0;
    background-color: #CCC;
    opacity: 0.8;
    filter: alpha(opacity=80);
    text-align:center;
}
#processMessage 
{
    text-align:center;
    position:fixed;
    top:30%;
    left:43%;
    z-index:1001;
    border: 5px solid #67CFF5;
    width: 200px;
    height: 100px;
    background-color: White;
    padding:0;
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
	     background-position:left center;
	     text-decoration:none;
	     padding:5px 2px 5px 30px;
	  
}
    </style>

    <script type="text/javascript" src="../lib/jquery/jquery-3.5.1.slim.min.js" integrity="sha384-DfXdz2htPH0lsSSs5nCTpuj/zy4C+OGpamoFVy38MVBnE+IbbVYUew+OrCXaRkfj" crossorigin="anonymous"></script>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>

    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <input id="zonaid" type="hidden" value="610" />
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Mis Naves</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Refrigerated Containers(Temperature) Report</li>
            </ol>
        </nav>
    </div>
  <div class="dashboard-container p-4" id="cuerpo" runat="server">
      
           <input id="agencia" type="hidden" runat="server"  clientidmode="Static"/>
        <div class="form-title">
            Filtros para el reporte
        </div>
        
        <div class="form-row">
            <div class="form-group col-md-6"> 
                <label for="inputAddress">Fecha Desde:<span style="color: #FF0000; font-weight: bold;"></span></label>
                <div class="d-flex"> 
                    <asp:TextBox 
                        style="text-align: center" 
                        ID="tfechaini" runat="server" ClientIDMode="Static" MaxLength="15" CssClass="datetimepicker form-control"
                        onkeypress="return soloLetras(event,'01234567890/')" 
                        onblur="valDate(this,true,valfechaini);">
                    </asp:TextBox>
                    <span id="valfechaini" class="validacion" ></span>                   
                </div>
            </div>

            <div class="form-group col-md-6"> 
                <label for="inputAddress">Fecha Hasta:<span style="color: #FF0000; font-weight: bold;"></span></label>
                <div class="d-flex"> 
                    <asp:TextBox 
                                style="text-align: center" 
                                ID="tfechafin" runat="server" ClientIDMode="Static" MaxLength="15" CssClass="datetimepicker form-control"
                                onkeypress="return soloLetras(event,'01234567890/')" 
                                onblur="valDate(this,true,valfechafin);">
                    </asp:TextBox>
                    <span id="valfechafin" class="validacion" ></span>
                </div>
            </div>


            <div class="form-group col-md-6"> 
                <label for="inputAddress">Referencia de Nave:<span style="color: #FF0000; font-weight: bold;"></span></label>
                <div class="d-flex"> 
                    <asp:TextBox  class="form-control"  ID="tbooking" runat="server" MaxLength="15" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')"></asp:TextBox>
                      
                    <a class="btn btn-outline-primary mr-4" style='font-size:24px' target="popup" onclick="window.open('../catalogo/naves.aspx','name','width=900,height=880')">
                            <span class='fa fa-search' style='font-size:24px'></span> 
                    </a>
                </div>
            </div>

            <div class="form-group col-md-6"> 
                <label for="inputAddress">Contenedor:<span style="color: #FF0000; font-weight: bold;"></span></label>
                <div class="d-flex"> 
                    <asp:TextBox
                                style="text-align: center" 
                                ID="contain" runat="server" ClientIDMode="Static" MaxLength="15" class="form-control"
                                onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890')" >
                    </asp:TextBox>
                      
                </div>
            </div>
        </div>
            <%--<table class="xcontroles" cellspacing="0" cellpadding="1">
        <tr><th class="bt-bottom bt-right  bt-left bt-top" colspan="3">Filtros para el reporte</th></tr>
          <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:130px">Fecha Desde:</td>
          <td class="bt-bottom bt-right " >
          <table>
                            <tr>
                                <td>
             <asp:TextBox 
             style="text-align: center" 
             ID="tfechaini" runat="server" ClientIDMode="Static" width="277px" MaxLength="15" CssClass="datetimepicker"
             onkeypress="return soloLetras(event,'01234567890/')" 
             onblur="valDate(this,true,valfechaini);"></asp:TextBox>
             </td>
             <td>
             <span id="valfechaini" class="validacion" ></span>
             </td>
             </tr>
             </table>
         </td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:130px">Fecha Hasta:</td>
          <td class="bt-bottom bt-right " >
                   <table>
                            <tr>
                                <td>
             <asp:TextBox 
             style="text-align: center" 
             ID="tfechafin" runat="server" ClientIDMode="Static" width="277px" MaxLength="15" CssClass="datetimepicker"
             onkeypress="return soloLetras(event,'01234567890/')" 
             onblur="valDate(this,true,valfechafin);"></asp:TextBox>
             </td>
             <td>
             <span id="valfechafin" class="validacion" ></span>
             </td>
             </tr>
             </table>
         </td>
         </tr>
         <tr>
         <td class="bt-bottom bt-left bt-right" style=" width:130px" >Referencia de Nave:</td>
         <td class="bt-bottom bt-right" >
         <table>
                            <tr>
                                <td>
             <asp:TextBox
             style="text-align: center" 
             ID="tbooking" runat="server" ClientIDMode="Static" CssClass="catamayusc" 
                 width="277px" MaxLength="15"
             onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')">
                 
                 </asp:TextBox>
                 </td>
                 <td>
             <a class="topopup" target="popup" onclick="window.open('../catalogo/naves.aspx','name','width=1110,height=480')" >
          <i class="ico-find" ></i> Nave </a>
          </td>
          </tr>
          </table>
    </td>

         </tr>
                  <tr>
         <td class="bt-bottom bt-left bt-right" style=" width:130px" >Contenedor:</td>
         <td class="bt-bottom bt-right" >
         <table>
                            <tr>
                                <td>
             <asp:TextBox
             style="text-align: center" 
             ID="contain" runat="server" ClientIDMode="Static" width="277px" MaxLength="15" CssClass="mayusc" 
             onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890')" ></asp:TextBox>
              </td>
              </tr>
              </table>
          </td>
         </tr>
         </table>--%>
         <%--<div class="botonera">
          <span id="imagen"></span>
             <asp:Button ID="btbuscar" runat="server" Text="Consultar"   
                 onclick="btbuscar_Click"
                  />
         </div>--%>
        <div ><br /></div>
        <div class="row">
            <div class="col-md-12 d-flex justify-content-center">
                <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda" 
                    class="btn btn-primary" 
                    onclick="btbuscar_Click" />
                <span id="imagen"></span>
            </div>
        </div>
        <div class="form-row">
            <div class="form-group col-md-12"> 
             <div class="cataresult" >
               <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                       <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
                       <input id="diponible" type="hidden" runat="server" clientidmode="Static" />

                         <div><br /></div>

             <div id="xfinder" runat="server" visible="false" title="PRUEBA" >

                 

             <div class="alert alert-warning" id="alerta" runat="server" ></div>
             <div class="findresult" >
             <div class="booking" >
                 <table>
                 <tr>
                 <td>
                 <a href="#" class="btn btn-primary"  target="_blank" runat="server" id="aprint" clientidmode="Static" >Vista Preliminar / Imprimir</a>
                 </td>
                 <td>
             <input class="btn btn-primary"  clientidmode="Static" id="dataexport" onclick="getTable('reportRefCont');" type="button" value="Exportar" runat="server" />
             </td>
             </tr>
             </table>
                 <br />
                 

                 <div class="form-group col-md-12"> 
                    <div class="form-title">Refrigerated Containers (Temperature)</div>
                </div>

                 <div class="bokindetalle" style=" width:100%; overflow:auto">
                 <%--<div style=" overflow : auto; height : 350px ">--%>
                 <%--width : 700px;--%>
                     <asp:Repeater ID="tablePagination" runat="server"  >
                     <HeaderTemplate>
                     <table id="tabla" cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                     <thead>
                     <tr>
                     <th style="width:250px;">Vessel</th>
                     <th style="width:60px">Voyage</th>
                     <th style="width:60px">Reference</th>
                     <th style="width:30px">Container</th>
                     <th style="width:30px">Size</th>
                     <th style="width:30px">Type</th>
                     <th style="width:70px">Bill of Lading</th>
                     <th style="width:60px">Booking</th>
                     <th style="width:50px">Document</th>
                     <th style="width:80px">Type Doc</th>
                     <th style="width:70px">FPOD</th>
                     <th style="width:70px">Time</th>
                     <th style="width:50px">Event</th>
                     <th style="width:30px">Temp</th>
                     <th style="width:30px">O2</th>
                     <th style="width:30px">CO2</th>
                     <th style="width:30px">Unit</th>
                     <th style="width:30px">ReadMode</th>
                     </tr>
                     </thead> 
                     <tbody>
                     </HeaderTemplate>
                     <ItemTemplate>
                     <tr class="point" >
                      <%--<td class="nover"><%#Eval("ROW")%></td>--%>
                      <td><%#Eval("VESSEL")%></td>
                      <td><%#Eval("VOYAGE")%></td>
                      <td><%#Eval("REFERENCE")%></td>
                      <td><%#Eval("CONTAINER")%></td>
                      <td><%#Eval("SIZE")%></td>
                      <td><%#Eval("TYPE")%></td>
                      <td><%#Eval("BILL OF LADING")%></td>
                      <td><%#Eval("BOOKING")%></td>
                      <td><%#Eval("DOCUMENT")%></td>
                      <td><%#Eval("TYPE DOC")%></td>
                      <td><%#Eval("FPOD")%></td>
                      <td><%#Eval("TIME")%></td>
                      <td><%#Eval("EVENT")%></td>
                      <td><%#Eval("TEMP")%></td>
                      <td><%#Eval("O2")%></td>
                      <td><%#Eval("CO2")%></td>
                      <td><%#Eval("UNIT")%></td>
                      <td><%#Eval("READMODE")%></td>
                     <%-- <td align="center">
                      <div style=" overflow-y:scroll; max-height:50px; width:110px">
                      -<%-<asp:TextBox ID="lblCONTENEDOR" Text='<%# Eval("CNTR") %>' Enabled="false" runat="server" style="height:auto; width:100px" TextMode="MultiLine"/>-%>-
                      <%#Eval("CNTR")%>
                      </div>
                      </td>--%>
                     </tr>
                     </ItemTemplate>
                     <FooterTemplate>
                     </tbody>
                     </table>
                     </FooterTemplate>
             </asp:Repeater>
                <%--</div>--%>
                </div>
             </div>
             </div>
              </div>
               <div id="sinresultado" runat="server" class="alert alert-info"></div>
                  </ContentTemplate>
                     <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                     </Triggers>
                 </asp:UpdatePanel>
             </div>
            </div>
        </div>
  </div>

    <script type="text/javascript" src="../lib/jquery/jquery-3.5.1.slim.min.js" integrity="sha384-DfXdz2htPH0lsSSs5nCTpuj/zy4C+OGpamoFVy38MVBnE+IbbVYUew+OrCXaRkfj" crossorigin="anonymous"></script>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>

    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });

        function popupCallback(catalogo) {
            if (catalogo == null || catalogo == undefined) {
                alert('Hubo un problema al setaar un objeto de catalogo');
                return;
            }
            //alert(catalogo);
            this.document.getElementById('<%= tbooking.ClientID %>').value = catalogo.codigo;
            

        }

        function clear() {
            document.getElementById('numbook').textContent = '...';
            document.getElementById('nbrboo').value = '';
        }
        function clear2() {
            document.getElementById('txtfecha').textContent = '...';
            document.getElementById('xfecha').value = '';
        }

        var programacion = {};
        var lista = [];
        function prepareObject() {

            try {
                document.getElementById("loader").className = '';
                lista = [];
                //validaciones básicas
                var vals = document.getElementById('nbrboo');
                if (vals == null || vals == undefined || vals.value.trim().length <= 2) {
                    alert('* Datos de programación *\n *Escriba el numero de Booking*');
                    document.getElementById("loader").className = 'nover';
                    return;
                }
                vals = document.getElementById('xfecha');
                if (vals == null || vals == undefined || vals.value.trim().length <= 5) {
                    alert('* Datos de programación *\n Escriba la fecha de programación');
                    document.getElementById("loader").className = 'nover';
                    return;
                }
                vals = document.getElementById('tmail');
                if (vals == null || vals == undefined || vals.value.trim().length <= 5) {
                    alert('* Datos de programación *\n *Escriba el correo electrónico para la notificación');
                    document.getElementById("loader").className = 'nover';
                    return;
                }
                this.programacion.booking = document.getElementById('nbrboo').value;
                this.programacion.fecha_pro = document.getElementById('xfecha').value;
                this.programacion.mail = document.getElementById('tmail').value;
                this.programacion.idlinea = document.getElementById('idlin').value;
                this.programacion.linea = document.getElementById('agencia').value;
                this.programacion.total = document.getElementById('diponible').value;
                
                //recorrer tabla->
                var tbl = document.getElementById('tabla');
                for (var f = 0; f < tbl.rows.length; f++) {
                    var celColect = tbl.rows[f].getElementsByTagName('td');
                    if (celColect != undefined && celColect != null && celColect.length > 0) {

                        var tdetalle = {
                            num: celColect[0].textContent,
                            desde: celColect[1].textContent,
                            hasta: celColect[2].textContent,
                            dispone: celColect[4].textContent,
                            idh: celColect[5].textContent,
                            idd: celColect[6].textContent,
                            total: celColect[7].textContent
                        };
                        tdetalle.reserva = celColect[8].getElementsByTagName('input')[0].value;
                        this.lista.push(tdetalle);
                    }
                }
                this.programacion.detalles = this.lista;
                var qtlimite = parseInt(document.getElementById('diponible').value);
                var total = 0;
                for (var n = 0; n < this.lista.length; n++) {
                    if (lista[n].reserva != '') {
                        if (parseInt(lista[n].dispone) < parseInt(lista[n].reserva)) {
                            alert('El Horario ' + lista[n].desde + '-' + lista[n].hasta + ' excede su disponibilidad, favor verifique');
                            return;
                        }
                        total += parseInt(lista[n].reserva);
                    }
                }
                if (total > qtlimite) {
                    alert('* Reserva *\n La cantidad de reserva excede el cupo disponible \n Cupo: ' + qtlimite + '\n Reserva: ' + total);
                    return;
                }
                if (total <= 0) {
                    alert('* Reserva *\n La cantidad de reservas debe ser mayor que 0');
                    return;
                }
                tansporteServer(this.programacion, 'turnos.aspx/ValidateJSON');
               
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

        function openPop() {
            var bo = document.getElementById('nbrboo').value;
            if (bo == undefined || bo == '' || bo == null) {
                alert('Por favor seleccione el booking primero');
                return;
            }
            window.open('../catalogo/calendarioConsolidacion.aspx?bk=' + bo, 'name', 'width=850,height=880')
        }

    </script>
<asp:updateprogress  id="updateProgress" runat="server">
    <progresstemplate>
        <div id="progressBackgroundFilter"></div>
        <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor  espere...<br />
         <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
        </div>
    </progresstemplate>
</asp:updateprogress>
  </asp:Content>