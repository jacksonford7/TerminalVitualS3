<%@ Page Title="OPC Consultar Grupos" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" 
    CodeBehind="consulta_grupo.aspx.cs" Inherits="CSLSite.consulta_grupo" %>
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
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">PCF</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Consulta y edición de grupos</li>
          </ol>
        </nav>
      </div>

     <div class="dashboard-container p-4" id="cuerpo" runat="server">
         <div class="form-title">Datos de los grupos búscados</div>
		    <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		   	  <label for="inputAddress">Operadora<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                          <asp:DropDownList ClientIDMode="Static" ID="dpestado" 
                              runat="server"  CssClass="form-control"
                 onchange="dropcheck(this,'valran','0')"></asp:DropDownList>
                  <span id="valran" class="validacion"> * obligatorio</span>
			  </div>
		   </div>
		  </div>
            <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
               <div class="d-flex">
                <span id="imagen"></span>
             <asp:Button ID="btbuscar" runat="server"  CssClass="btn btn-primary"
                 Text="Iniciar la búsqueda"  OnClientClick="return checkPost();" 
                 onclick="btbuscar_Click"  />
                </div>
		   </div> 
		   </div>
            <div class="form-row">
	 <div class="form-group col-md-12"> 
               <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                        <script type="text/javascript">
                            Sys.Application.add_load(BindFunctions); 
                      </script>
             <div id="xfinder" runat="server" visible="false" >
                  <div class="form-title">Documentos encontrados</div>
         
                 <asp:Repeater ID="tablePagination" runat="server"
                     onitemcommand="tablePagination_ItemCommand" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered table-sm table-contecon">
                 <thead>
                 <tr>
                 <th>RUC</th>
                 <th>Nombre</th>
                 <th>Secuencia</th>
                 <th>Operadores</th>
                 <th>Ultima Modifi.</th>
                 <th>Usuario</th>
                 <th ></th><th ></th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                 <td><%#Eval("opc_id")%></td>
                 <td><%#Eval("grupo_name")%></td>
                 <td><%#Eval("grupo_id")%></td>
                 <td><%#Eval("operadores")%></td>
                 <td><%# formatProDate(Eval("ultima_fecha"))%></td>
                  <td><%#Eval("ultimo_usuario")%></td>
                     <td> <asp:Button ID="btelim"  
                         CommandArgument='<%# Eval("id")%>' 
                         runat="server" Text="ELIMINAR"  CssClass=" btn btn-secondary" OnClientClick="return confirm('Esta seguro de eliminar totalmente este grupo?');" /></td>
                      <td><a class="btn btn-link" href="grupo.aspx?sid=<%# securetext(Eval("id")) %>"  target="_blank">EDITAR</a></td>
                 </tr>
                  </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>
             
            
    
              </div>
               <div id="sinresultado" runat="server" class="alert alert-secondary"></div>
                  </ContentTemplate>
                     <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                     </Triggers>
                 </asp:UpdatePanel>
	 </div>
	 
	 </div>
     </div>


    <script src="../Scripts/opc_control.js" type="text/javascript"></script>

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