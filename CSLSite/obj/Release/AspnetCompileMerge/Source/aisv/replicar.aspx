<%@ Page Title="Replicar AISV" Language="C#" MasterPageFile="~/site.Master" 
AutoEventWireup="true" CodeBehind="replicar.aspx.cs" Inherits="CSLSite.replica" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
     <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
       <!--Datatables-->
       <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" /><link href="../css/datatables.min.css" rel="stylesheet" /><link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css" />
        <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>
       <script type="text/javascript"  src="../js/datatables.js"></script>
       <script src="../Scripts/table_catalog.js" type="text/javascript"></script>  
       <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
    <!--Fin-->
    
  	   <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />

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
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">AISV</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Consulta, reimpresión y replicación de AISV (CGSA)</li>
          </ol>
        </nav>
      </div>

        <div class="dashboard-container p-4" id="cuerpo" runat="server">

         <div class="form-title">Datos del documento buscado</div>
		 
		  <div class="form-row">
		  
		   <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">AISV No<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                               <asp:TextBox ID="aisvn" runat="server"  CssClass="form-control" MaxLength="15" 
             onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>

			  </div>
		   </div>

               <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">Contenedor No<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                               <asp:TextBox ID="cntrn" runat="server"  CssClass="form-control" MaxLength="15"  
             
                  onkeypress="return soloLetras(event,'01234567890abcdefghijklmnopqrstuvwxyz',true)"></asp:TextBox>


			  </div>
		   </div>

                 <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">Booking No<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                                  <asp:TextBox ID="booking" runat="server" ClientIDMode="Static" CssClass="form-control" MaxLength="15" 
             onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890-_')" 
                 ></asp:TextBox>


			  </div>
		   </div>
		  </div>

          <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Desde<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                              <asp:TextBox ID="desded" runat="server"  MaxLength="10" CssClass="datetimepicker form-control"
             onkeypress="return soloLetras(event,'01234567890/')" 
                  onblur="valDate(this,true,valdate);" ClientIDMode="Static"></asp:TextBox>

			  </div>
		   </div>
              <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Hasta<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                  <asp:TextBox ID="hastad" runat="server" ClientIDMode="Static"  MaxLength="15" CssClass="datetimepicker form-control"
             onkeypress="return soloLetras(event,'01234567890/')" 
                  onblur="valDate(this,true,valdate);"></asp:TextBox>
            <span id="valdate" class="validacion"> * </span>

			  </div>
		   </div>
		  </div>

             <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
		     <div class="d-flex">
                           <span id="imagen"></span>
             <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda" CssClass="btn btn-primary"   onclick="btbuscar_Click" 
             OnClientClick="return validateDatesRange('desded','hastad','imagen');" />


		     </div>
		   </div> 
		   </div>

                      
               <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                        <script type="text/javascript">
                            Sys.Application.add_load(BindFunctions); 
                      </script>
             <div id="xfinder" runat="server" visible="false" >
             <div class=" alert alert-warning" id="alerta" runat="server" >
                 Recuerde: Si el documento no aparece en la lista es probable que el AISV no 
                 exista. Consulte y luego proceda a replicar bajo su responsabilidad.</div>
            
           
                  <div class=" form-title">Documentos encontrados</div>
                 
                 <asp:Repeater ID="tablePagination" runat="server" 
                         onitemcommand="tablePagination_ItemCommand" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered table-sm table-contecon">
                 <thead>
                 <tr>
                 <th>#</th>
                 <th>AISV #</th>
                 <th>Tipo</th>
                 <th>Booking</th>
                 <th>Registrado</th>
                 <th>Carga</th>
                 <th>Estado</th>
                 <th>Acciones</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td><%#Eval("item")%></td>
                  <td><%#Eval("aisv")%></td>
                  <td><%# tipos(Eval("tipo"), Eval("movi"))%></td>
                  <td>
                    <a class="xinfo" >
                    <span class="xclass">
                    <h3>Referencia:</h3> <%#Eval("referencia")%>
                    <h3>FreightKind</h3> <%#Eval("fk")%>
                    <h3>Puerto descarga:</h3> <%#Eval("pod")%>
                    <h3>Agencia:</h3> <%#Eval("agencia")%>
                    </span>
                        <%#Eval("boking")%>
                    </a>
                  </td>
                  <td><%#Eval("fecha")%></td>
                  <td>
                        <%#Eval("tool")%>
                  </td>
                  
                  <td><%# anulado(Eval("estado"))%></td>
                  <td>
                   <div class="tcomand">
                       <a href="impresion.aspx?sid=<%# securetext(Eval("aisv")) %>" class=" btn btn-link" target="_blank">Mostrar</a>|
                       <div class='<%# boton( Eval("estado"))%>' >
                       <asp:Button ID="btanula"  
                       OnClientClick="return confirm('Esta seguro que desea replicar este AISV?');" 
                       CommandArgument=   '<%# jsarguments( Eval("aisv"),Eval("referencia"),Eval("cntr"),Eval("tipo"),Eval("movi"),Eval("estado") )%>' runat="server" Text="Replicar" CssClass=" btn btn-secondary" ToolTip="Permite replicar este AISV" />
                       </div>
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
               <div id="sinresultado" runat="server" class=" alert alert-secondary"></div>
                  </ContentTemplate>
                     <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                     </Triggers>
                 </asp:UpdatePanel>
             

		 
     </div>


    <script src="../Scripts/pages.js" type="text/javascript"></script>
     <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
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
