<%@ Page Title="Consultar trabajos" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" 
    CodeBehind="consulta_trabajos.aspx.cs" Inherits="CSLSite.consulta_trabajos" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
      <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
         <!--Datatables-->
       <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />
       <link href="../css/datatables.min.css" rel="stylesheet" />
       <link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css"/>
        <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>
       <script type="text/javascript"  src="../js/datatables.js"></script>
       <script src="../Scripts/table_catalog.js" type="text/javascript"></script>  
       <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
    <!--Fin-->



    <style type="text/css">
        
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
    
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
           <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">CPF</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Consulta de trabajos realizados</li>
          </ol>
        </nav>
      </div>

      <div class="dashboard-container p-4" id="cuerpo" runat="server">

         <div class="form-title">Datos del documento buscado</div>
		 
		  <div class="form-row">
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Referencia<span style="color: #FF0000; font-weight: bold;"></span></label>
			        <asp:TextBox ID="_referencia" runat="server"  CssClass="form-control" MaxLength="12" 
             onkeypress="return soloLetras(event,'01234567890abcdefghijklmnopqrstuvwxyz',true)">

             </asp:TextBox>
		   </div>
               <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">OPC Ruc<span style="color: #FF0000; font-weight: bold;"></span></label>
			            <asp:TextBox ID="ruc_opc" runat="server"  MaxLength="20"  
                CssClass="form-control"
                  onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>

                   <span id="valran" class="opcional nover"></span>
		   </div>
		  </div>


           <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Desde desde el día<span style="color: #FF0000; font-weight: bold;"></span></label>
			       <asp:TextBox ID="desded" runat="server" MaxLength="10" CssClass="datetimepicker form-control"
             onkeypress="return soloLetras(event,'01234567890/')" 
                 onblur="valDate(this,true,valdate);" ClientIDMode="Static"></asp:TextBox>
		   </div>

                <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Hasta<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
      <asp:TextBox ID="hastad" runat="server" ClientIDMode="Static" 
                  MaxLength="15" CssClass="datetimepicker form-control"
             onkeypress="return soloLetras(event,'01234567890/')" 
                  onblur="valDate(this,true,valdate);"></asp:TextBox>

                   <span id="valdate" class="validacion"> * </span>
			  </div>
		   </div>
		  </div>
		 
		 
             <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
                        
             <asp:Button ID="btbuscar"  CssClass="btn btn-primary"
                 runat="server" Text="Iniciar la búsqueda"   onclick="btbuscar_Click" OnClientClick="return validateDatesRange('desded','hastad','imagen');" />
                <span id="imagen"></span>

		   </div> 
		   </div>

                       <div class="cataresult" >
               <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                        <script type="text/javascript">
                            Sys.Application.add_load(BindFunctions); 
                      </script>
             <div id="xfinder" runat="server" visible="false" >
             
           
             <div class="booking" >
               
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server" 
                          >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered table-sm table-contecon">
                 <thead>
                 <tr>
                
                 <th>Referencia</th>
                 <th>Nave</th>
                 <th>Atraque</th>
                 <th>Zarpe</th>
                 <th>OPCs</th>
                 <th>Grúas</th>
                  <th>Turnos</th>
                 <th>Fecha Generación</th>
                 <th>Estado</th>
                 <th >Acciones</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td><%#Eval("referencia")%></td>
                  <td><%#Eval("nave")%></td>
                  <td><%#formatProDate(Eval("atraque"))%></td>
                  <td><%#formatProDate(Eval("zarpe"))%></td>
                  <td><%#Eval("opcs")%></td>
                  <td><%#Eval("gruas")%></td>
                  <td><%#Eval("turnos")%></td>
                  <td><%#formatProDate(Eval("creada"))%></td>
                  <td><%#anulado(Eval("status"))%></td>
                  <td> <a href="plan_preview.aspx?sid=<%# securetext(Eval("id")) %>" class=" btn btn-link" target="_blank">Imprimir</a></td>
                   
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
               <div id="sinresultado" runat="server" class=" alert-secondary"></div>
                  </ContentTemplate>
                     <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                     </Triggers>
                 </asp:UpdatePanel>
             </div>

     </div>
    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
              $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });
  </script>

  <asp:updateprogress associatedupdatepanelid="upresult"
    id="updateProgress" runat="server">
    <progresstemplate>
        <div id="progressBackgroundFilter"></div>
        <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor 
            espere...<br />
             <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
        </div>
    </progresstemplate>
</asp:updateprogress>
  </asp:Content>
