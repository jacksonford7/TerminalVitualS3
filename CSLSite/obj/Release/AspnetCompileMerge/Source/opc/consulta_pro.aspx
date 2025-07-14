<%@ Page Title="Consultar proformas" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" 
    CodeBehind="consulta_pro.aspx.cs" Inherits="CSLSite.consulta_pro" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
  	
    
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/opc_tables.css" rel="stylesheet" />


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
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Consulta, reimpresión y anulación de proformas</li>
          </ol>
        </nav>
      </div>

         <div class="dashboard-container p-4" id="cuerpo" runat="server">
         <div class="form-title">Datos de la proforma buscada</div>
		   
             <div class="form-row">
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Referencia<span style="color: #FF0000; font-weight: bold;"></span></label>
			               <asp:TextBox ID="_referencia" runat="server" MaxLength="8"  CssClass="form-control"
             onkeypress="return soloLetras(event,'01234567890abcdefghijklmnopqrstuvwxyz',true)"></asp:TextBox>
		   </div>
               <div class="form-group col-md-6"> 
                   		  
                   
                   <label for="inputAddress">OPC Ruc<span style="color: #FF0000; font-weight: bold;"></span></label>

                         <asp:TextBox ID="ruc_opc" runat="server"  CssClass="form-control"
                 MaxLength="14"  
                  onkeypress="return soloLetras(event,'01234567890',true)">
             </asp:TextBox>
                   </div>

		  </div>
             <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">No. Proforma<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <asp:TextBox ID="prof_id" runat="server"  MaxLength="15"   CssClass=" form-control"
                   onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
		   </div>
                  <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Estado<span style="color: #FF0000; font-weight: bold;"></span></label>
			                  <asp:DropDownList ID="dpestado" runat="server" CssClass="form-control" ></asp:DropDownList>

		   </div>
		  </div>
             <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Desde<span style="color: #FF0000; font-weight: bold;"></span></label>
			         <asp:TextBox ID="desded" runat="server"  MaxLength="10" CssClass="datetimepicker form-control"
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
                           <span id="valdate" class="validacion">*</span>


			  </div>
		   </div>
		  </div>
                <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
                        
             <asp:Button ID="btbuscar" CssClass="btn btn-primary"
                 runat="server" Text="Iniciar la búsqueda"   onclick="btbuscar_Click" OnClientClick="return validateDatesRange('desded','hastad','imagen');" />
                <span id="imagen"></span>
		   </div> 
		   </div>

       
              <div class="form-row">
	 <div class="form-group col-md-12"> 
                        <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
             <script type="text/javascript">Sys.Application.add_load(BindFunctions); </script>
                         <div id="xfinder" runat="server" visible="false" >
             
           
         
                  <div class="form-title">Documentos encontrados</div>
             
                 <asp:Repeater ID="tablePagination" runat="server" 
                         onitemcommand="tablePagination_ItemCommand" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered table-sm table-contecon" >
                 <thead>
                 <tr>
                 <th>No.</th>
                 <th>Referencia</th>
                 <th>OPC</th>
                 <th>Fecha Generación</th>
                 <th>Estado</th>
                 <th ></th><th ></th><th ></th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td><%#formatPro(Eval("id"))%></td>
                  <td><%#Eval("vessel_visit_reference")%></td>
                  <td><%#Eval("opc_name")%></td>
                  <td><%#formatProDate(Eval("create_date"))%></td>
                  <td><%#anulado(Eval("status"))%></td>
                     <td> <a href="proforma_preview.aspx?sid=<%# securetext(Eval("id")) %>" class="btn-link" target="_blank">Imprimir</a></td>
                    <td><div class="<%#xver(Eval("status"))%>"><a href="ride_upload.aspx?sid=<%# securetext(Eval("id")) %>" class="btn-link" target="_blank">Factura</a> </div></td>
                    <td> <div class="<%#xver(Eval("status"))%>">
                       <asp:Button ID="btenviar"  
                       CommandArgument= '<%#Eval("id")%>' 
                       runat="server" Text="Reenviar" 
                       CssClass=" btn btn-secondary" ToolTip="Permite reenviar la proforma al mail del OPC" 
                       CommandName="Enviar"  />
                       </div>
                   
                  </td>
                 </tr>
                  </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>
              
        
               
              </div>
               <div id="sinresultado" runat="server" class=" alert  alert-secondary"></div>
                  </ContentTemplate>
                     <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                     </Triggers>
                 </asp:UpdatePanel>
	 </div>
	 
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
