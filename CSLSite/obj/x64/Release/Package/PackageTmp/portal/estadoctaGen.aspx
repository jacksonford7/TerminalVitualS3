<%@ Page Title="Saldos pendientes" Language="C#" MasterPageFile="~/site.Master" 
AutoEventWireup="true" CodeBehind="estadoctaGen.aspx.cs" Inherits="CSLSite.estadoctaGen" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <!--Datatables-->
       <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />
       <link href="../css/datatables.min.css" rel="stylesheet" />
       <link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css"/>
        <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>
       <script type="text/javascript"  src="../js/datatables.js"></script>
       <script src="../Scripts/table_catalog.js" type="text/javascript"></script>  
       <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
     <link href="../css/style.css" rel="stylesheet"/>
    <!--Fin-->


     <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
        <script src="../Scripts/exportar.js" type="text/javascript"></script>



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
<input id="zonaid" type="hidden" value="204" />
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>


    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Gestión Financiera</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Consulta de Saldos Pendientes</li>
          </ol>
        </nav>
      </div>

     <div class="dashboard-container p-4" id="cuerpo" runat="server">

        <div class="form-title">Filtro de busqueda</div>

         <div class="form-row">
            <div id="divFiltro" clientidmode="Static"  runat="server" class="form-group   col-md-6">     
                <div class="catawrap" >
                    <label for="inputAddress">Ruc<span style="color: #FF0000; font-weight: bold;"></span></label>
                    <div class="d-flex">
                            <asp:TextBox ID="txtRuc" runat="server" class="form-control" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890',true)"  Font-Bold="true" placeholder="DIGITE EL RUC"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="form-group   col-md-2">     
                <label for="inputAddress">Año<span style="color: #FF0000; font-weight: bold;"></span></label>
                <div class="d-flex">
                    <asp:DropDownList ID="cmbAnio" class="form-control" runat="server"  Font-Size="Medium"  Font-Bold="true" ></asp:DropDownList>
                </div>
            </div>

                <div class="form-group   col-md-4">     
                <label for="inputAddress">Mes<span style="color: #FF0000; font-weight: bold;"></span></label>
                <div class="d-flex">
                    <asp:DropDownList ID="cmbMes" class="form-control" runat="server" Font-Size="Medium"  Font-Bold="true" ></asp:DropDownList>
                        &nbsp
                        &nbsp
                        <asp:Button class="btn btn-primary" ID="btnconsultar" runat="server" Text="Consultar" Height="40"  onclick="btnconsultar_Click" ToolTip="Consultar estado de cuenta."/>
                </div>
            </div>
           
        </div>
         <div class="form-title">Datos del Cliente</div>

            <%--<div class="form-group col-md-6"> 
		   	    <label for="inputAddress">RUC<span style="color: #FF0000; font-weight: bold;"></span></label>
			     <div class="d-flex">
                     
                      <asp:TextBox ID="txtRuc" runat="server" class="form-control"onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890',true)" placeholder="DIGITE RUC"></asp:TextBox>
                      <asp:Button class="btn btn-primary" ID="btnconsultar" runat="server" Text="Consultar"  onclick="btnconsultar_Click" ToolTip="Consultar estado de cuenta."/>

			     </div>
		   </div>--%>



		  <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">RUC<span style="color: #FF0000; font-weight: bold;"></span></label>
			                 <span id="num_ruc" class=" form-control col-md-12" runat="server" clientidmode="Static"  >...</span>

		   </div>

               <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Razón Social<span style="color: #FF0000; font-weight: bold;"></span></label>
			             <span id="razon_soc" class="form-control col-md-12"  runat="server" clientidmode="Static"  >...</span>

		   </div>
		  </div>

           <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Plazo crédito<span style="color: #FF0000; font-weight: bold;"></span></label>
			          <span id="plazo" class="form-control col-md-12"  runat="server" clientidmode="Static"  >...</span>

		   </div>

               <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Monto Total<span style="color: #FF0000; font-weight: bold;"></span></label>
			             <span id="saldo" class="form-control col-md-12"  runat="server" clientidmode="Static"  >...</span>

		   </div>
		  </div>

         	<div class="form-row">
		  
		       <div class="form-group col-md-6"> 
		   	      <label for="inputAddress">Fact. por vencer<span style="color: #FF0000; font-weight: bold;"></span></label>
			            <span id="fac_pend" class="form-control col-md-12"  runat="server" clientidmode="Static"  >...</span>

		       </div>

                <div class="form-group col-md-6"> 
		   	      <label for="inputAddress">Fact. vencidas<span style="color: #FF0000; font-weight: bold;"></span></label>
			                <span id="fac_ven" class="form-control col-md-12"  runat="server" clientidmode="Static"  >...</span>

		       </div>
		  </div>

            <div class="form-row">
		       <div class="col-md-12 "> 
         
                 <div id="xfinder" runat="server"  >
                   <div class=" alert alert-warning" id="alerta" runat="server" >
                   Estimado Cliente;<br />Para mayor información sobre detalle de facturas agradeceremos contactarse con la casilla 
                   <a href="mailto:tesoreria@cgsa.com.ec ">tesoreria@cgsa.com.ec </a>
                 </div>
                      <div class=" form-title">Facturas Asociadas</div>
                     <asp:Repeater ID="tablePagination" runat="server" >
                             <HeaderTemplate>
                             <table id="tablasort" cellspacing="1" cellpadding="1" 
                                 class="table table-bordered table-sm table-contecon"  >
                
                             <thead>
                             <tr>
                                 <th >FACTURA</th>
                                 <th>LIQUIDACION</th>
                                 <th>RAZON SOCIAL</th>
                                 <th>FECHA LIQUIDA</th>
                                 <th>MONTO TOTAL</th>
                                 <th>PAGADO</th>
                                 <th>RETENCION</th>
                                 <th>COMPENSADO</th>
                                 <th>SALDO</th>
                             </tr>
                             </thead> 
                             <tbody>
                             </HeaderTemplate>
                             <ItemTemplate>
                             <tr class="point" >
                                 <td>'<%#Eval("NUMERO_FACTURA")%></td>
                                 <td>'<%#Eval("NUMERO_LIQUIDACION")%></td>
                                 <td><%#Eval("RAZON_SOCIAL")%></td>
                                 <td><%# FormatoDates(Eval("FECHA_LIQUIDACION"))%></td>
                                 <td><%#FormatoDecimals(Eval("MONTO_TOTAL"))%></td>
                                 <td><%#FormatoDecimals(Eval("PAGADO"))%></td>
                                 <td><%#FormatoDecimals(Eval("RETENCION"))%></td>
                                 <td><%#FormatoDecimals(Eval("COMPENSADO"))%></td>
                                 <td><%#FormatoDecimals(Eval("SALDO"))%></td>
                             </tr>
                              </ItemTemplate>
                             <FooterTemplate>
                             </tbody>
                             </table>
                             </FooterTemplate>
                     </asp:Repeater>
              
                 </div>
		       </div> 
		   </div>

            <div class="form-row">
		        <div class="col-md-12 d-flex justify-content-center "> 
		            <input clientidmode="Static" id="dataexport" onclick="generateexcel();" type="button" value="Exportar" runat="server" class="btn btn-primary" />
		        </div> 
		    </div>
     </div>

    <script src="../Scripts/pages.js" type="text/javascript"></script>
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

                var vals = document.getElementById('<%=txtRuc.ClientID %>');
                if (vals == null || vals == undefined || vals.value == '') {
                 alertify.alert('¡ Escriba la Referencia.');
                    document.getElementById("loader").className = 'nover';
                    document.getElementById('<%=txtRuc.ClientID %>').focus();
                    return false;
                }

                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

        function getGifOculta() {
            document.getElementById('loader').className = 'nover';
        }
    </script>


  </asp:Content>
