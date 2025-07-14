<%@ Page Title="Saldos pendientes" Language="C#" MasterPageFile="~/site.Master" 
AutoEventWireup="true" CodeBehind="estadocta.aspx.cs" Inherits="CSLSite.estadocta" %>
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

         <div class="form-title">Datos del Cliente</div>
		 
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
                 <th >No.</th>
                 <th>Saldo Pendiente</th>
                 <th>Fecha Vencimiento</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td><%#Eval("NUMERO_FACTURA")%></td>
                  <td><%#FormatoDecimal(Eval("SALDO_PENDIENTE"))%></td>
                  <td><%# FormatoDate(Eval("FECHA_VENCIMIENTO"))%></td>
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
     </div>

    <script src="../Scripts/pages.js" type="text/javascript"></script>
         <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
              $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
          });
  </script>
  </asp:Content>
