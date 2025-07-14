<%@ Page Title="Administrar Usuarios STC" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="plus_consola.aspx.cs" Inherits="CSLSite.consolasna" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
     <!--mensajes-->
        <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
        <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>
    
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
        <script type="text/javascript">
            function BindFunctions() {
                $(document).ready(function () {
                    document.getElementById('imagen').innerHTML = '';
                    $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 8: { sorter: false }, 9: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
                });
            }
    </script>
    <style type="text/css">

        .edita 
{
      padding-left:14px; 
      display:inline-block;
      width:40px; 
      height:15px; 
      margin:0; 
      background-image:url(../shared/imgs/edita.png)!important; 
      background-position:left center!important; 
      background-repeat:no-repeat!important;}
        
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
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">STC</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Consulta y edición de clientes</li>
          </ol>
        </nav>
      </div>
     <div class="dashboard-container p-4" id="cuerpo" runat="server">

         <div class="form-title">Datos del cliente buscado</div>
		 
		  <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">RUC No.<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                    <asp:TextBox ID="txtruc" runat="server" MaxLength="15"  CssClass="form-control"
             onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>

			  </div>
		   </div>
 </div>
               <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Nombres o Descripción<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                            <asp:TextBox ID="txtnombre" runat="server" MaxLength="100"   CssClass="form-control"
             
                  onkeypress="return soloLetras(event,'01234567890abcdefghijklmnopqrstuvwxyz',true)"></asp:TextBox>


			  </div>
		   </div>
		  </div>

           <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
		     <div class="d-flex">

                           <span id="imagen"></span>
             <asp:Button ID="btbuscar" runat="server" Text="Buscar"   CssClass="btn btn-primary" 
                 onclick="btbuscar_Click" OnClientClick="return validarUsuario();" />

		     </div>
		   </div> 
		   </div>
		 
           <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
		     <div class="cataresult" >
               <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                        <script type="text/javascript">
                            Sys.Application.add_load(BindFunctions); 
                      </script>
             <div id="xfinder" runat="server" visible="false" >
             
             <div class="findresult" >
             <div class="booking" >
                  <div class=" form-title">Clientes encontrados</div>
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server" 
                         onitemcommand="tablePagination_ItemCommand" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                 <thead>
                 <tr>
                
                 <th>ID</th>
                 <th>RUC</th>
                 <th>Nombres/Descripción</th>
                 <th>Categoría</th>
                 <th>Registrado</th>
                 <th>Estado</th>
                 <th>Acciones</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  
                  <td><%#Eval("cliente_id")%></td>
                  <td><%#Eval("cliente_ruc")%></td>
                  <td><%#Eval("cliente_nombres")%></td>
                  <td><%#Eval("cliente_categoria")%></td>
                  <td><%#Eval("cliente_creado")%></td>
                  <td  ><%# anulado(Eval("cliente_estado"))%></td>
          

                  <td>
                   <div class="tcomand">
                       <a href="../sna/plus_usuario.aspx?sid=<%# securetext(Eval("cliente_id")) %>" 
                           class=" btn btn-link" target="_blank">Editar</a>|
                  
                       <asp:Button ID="Activar"  
                       OnClientClick="return confirm('Esta seguro que desea actualizar este registro?');" 
                       CommandArgument ='<%# jsarguments( Eval("cliente_id"),Eval("cliente_estado"),Eval("cliente_categoria") )%>'
                       CommandName="update"
                       runat="server" 
                       Text='<%# botonText(Eval("cliente_estado"))%>'  
                       CssClass=" btn btn-secondary" 
                       ToolTip="Permite anular este documento" 
                           />
                     
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
             </div>
             </div>
             <div id="pager">
               Registros por página
                  <select class="pagesize">
                  <option selected="selected" value="10">10</option>
                  <option value="20">20</option>
                  </select>
                 <img alt="" src="../shared/imgs/first.gif" class="first"/>
                 <img alt="" src="../shared/imgs/prev.gif" class="prev"/>
                 <input  type="text" class="pagedisplay"  />
                 <img alt="" src="../shared/imgs/next.gif" class="next"/>
                 <img alt="" src="../shared/imgs/last.gif" class="last"/>
                 &nbsp;
            </div>
              </div>
               <div id="sinresultado" runat="server" class=" alert-info"></div>
                  </ContentTemplate>
                     <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                     </Triggers>
                 </asp:UpdatePanel>
             </div>
		   </div> 
		   </div>

		 
     </div>


    <script src="../Scripts/pages.js" type="text/javascript"></script>

    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>


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
