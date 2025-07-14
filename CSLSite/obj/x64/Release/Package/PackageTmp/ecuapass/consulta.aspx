<%@ Page Title="Consultar DAE" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true"
    CodeBehind="consulta.aspx.cs" Inherits="CSLSite.ecuapass.consulta" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
       <!--Datatables-->
       <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" /><link href="../css/datatables.min.css" rel="stylesheet" /><link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css" />
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
<input id="zonaid" type="hidden" value="204" />
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>

           <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Servicios</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Consulta de la DAE</li>
          </ol>
        </nav>
      </div>

         <div class="dashboard-container p-4" id="cuerpo" runat="server">

         <div class="form-title">Datos del documento buscado</div>
		 
		  <div class="form-row">
		  
		   <div class="form-group col-md-8"> 
		   	  <label for="inputAddress">Documento No. (DAE)<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                     <asp:TextBox ID="docnum" runat="server"  MaxLength="20"  
                   onkeypress="return soloLetras(event,'01234567890',true)"
                   onblur="cadenareqerida(this,17,20,'valdae');" CssClass="form-control"
                   ></asp:TextBox>
                  <span id="valdae" class="validacion">*</span>

			  </div>
		   </div>

		   <div class="form-group col-md-4"> 
               		   	  <label for="inputAddress">&nbsp;<span style="color: #FF0000; font-weight: bold;"></span></label>
              <div class="d-flex">
             
             <asp:Button 
                 ID="btbuscar" 
                 runat="server" 
                 Text="Iniciar la búsqueda"    
                 CssClass="btn btn-primary"
                 onclick="btbuscar_Click" />
                 <span id="imagen"></span>
                  </div>
		   </div> 
		   </div>

              <div class="form-row">
	 <div class="form-group col-md-12">
                        <asp:UpdatePanel ID="upresult" runat="server" >
                     <ContentTemplate>
                        <script type="text/javascript">
                            Sys.Application.add_load(BindFunctions); 
                      </script>
             <div id="xfinder" runat="server" visible="false" >
                  <div class="form-title">Documentos encontrados</div>
                 <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered table-sm table-contecon">
                 <thead>
                 <tr>
                 <th>F. Aceptación</th>
                 <th>No.DAE</th>
                 <th>RUC</th>
                 <th>Razón Social</th>
                 <th>Tipo de carga</th>
                 <th>Estado de la DAE</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                      <td><%#Eval("fecha_creada")%></td>
                      <td><%#Eval("dae")%></td>
                      <td><%#Eval("ruc")%></td>
                      <td><%#Eval("exportador")%></td>
                      <td><%#Eval("tipo_dae")%></td>
                      <td><%# Eval("estado")%></td>


                 </tr>
                  </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>
              </div>
               <div id="sinresultado" runat="server" class=" alert alert-secondary"></div>
                  </ContentTemplate>
                     <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                     </Triggers>
                 </asp:UpdatePanel>

	 </div>
	 
	 </div> 
     </div>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
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
