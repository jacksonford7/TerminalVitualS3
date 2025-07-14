<%@ Page  Title="Reporte de operaciones" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="preimpresion.aspx.cs" Inherits="CSLSite.preimpresion" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ MasterType VirtualPath="~/site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
        <script type="text/javascript">
            function BindFunctions() {
                $(document).ready(function () {
                    document.getElementById('imagen').innerHTML = '';
                    $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 8: { sorter: false }, 9: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
                });
            }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
 <input id="zonaid" type="hidden" value="103" />
 <input id="usuario" type="hidden" runat="server" clientidmode="Static" />
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>

           <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Exportacion</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Consulta e impresión de avisos de contenedores exportación/vacíos</li>
          </ol>
        </nav>
      </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">

         <div class="form-title">Datos de las   unidades buscadas</div>
		 
		  <div class="form-row">
		  
		   <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">Referencia<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                   <asp:TextBox ID="refer" runat="server"  MaxLength="15"  CssClass="form-control"
             onkeypress="return soloLetras(event,'01234567890abcdefghijklmnopqrstuvwxyz',true)">

             </asp:TextBox>
			  </div>
		   </div>

                <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">Contenedor<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                  <asp:TextBox ID="cntrn" runat="server"  CssClass="form-control" MaxLength="15"  
                  onkeypress="return soloLetras(event,'01234567890abcdefghijklmnopqrstuvwxyz',true)">
                  </asp:TextBox>

			  </div>
		   </div>

              	   <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">Booking<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                                 <asp:TextBox ID="docnum" runat="server"  CssClass="form-control" MaxLength="20"  
                   onkeypress="return soloLetras(event,'01234567890abcdefghijklmnopqrstuvwxyz_-',true)">

               </asp:TextBox>

			  </div>
		   </div>
		  </div>

           <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Desde<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                   <asp:TextBox ID="desded" runat="server"   MaxLength="10" CssClass="datetimepicker form-control"
             onkeypress="return soloLetras(event,'01234567890/')" 
                 onblur="valDate(this,true,valdate);" ClientIDMode="Static"></asp:TextBox>

			  </div>
		   </div>

               <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Hasta<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                     <asp:TextBox ID="hastad" runat="server" ClientIDMode="Static"  MaxLength="15" 
                 CssClass="datetimepicker form-control"
             onkeypress="return soloLetras(event,'01234567890/')" 
                  onblur="valDate(this,true,valdate);"></asp:TextBox>
                <span id="valdate" class="validacion"> * </span>

			  </div>
		   </div>
		  </div>

         <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
		     <div class="d-flex">
                       <span id="imagen"> </span>
             <asp:Button ID="btbuscar" runat="server" Text="Generar reporte"   CssClass="btn btn-primary"
                 OnClientClick="return validateDatesRange('desded','hastad','imagen')" 
                 onclick="btbuscar_Click" />

		     </div>
		   </div> 
		   </div>

                     <div class="cataresult" >
               <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true"  >
                     <ContentTemplate>
                        <script type="text/javascript">
                            Sys.Application.add_load(BindFunctions); 
                      </script>
                
                <div id="xfinder" runat="server" visible="false" clientidmode="Static" >
                <div id="postable" runat="server">

                </div>
                 <div id="pager">
               Registros por página
                  <select class="pagesize">
                  <option selected="selected" value="10">10</option>
                  <option value="20">20</option>
                  </select>
                 <img alt="" src="../shared/imgs/first.gif" class="first"/>
                 <img alt="" src="../shared/imgs/prev.gif" class="prev"/>
                 <input  type="text" class="pagedisplay" />
                 <img alt="" src="../shared/imgs/next.gif" class="next"/>
                 <img alt="" src="../shared/imgs/last.gif" class="last"/>
                  <input id="btprinter" type="button" runat="server" value="Imprimir" onclick="window.open('preaviso.aspx','Impresion','width=850,height=500,scrollbars=yes');" />
            </div>
              </div>
                <span id="htmlstring"  runat="server" clientidmode="Static" style=" width:1px; height:1px; max-width:1px; max-height:1px; overflow:hidden; visibility:hidden; display:inline; z-index:4000; float:right;"   ></span>
               <div id="sinresultado" runat="server" class=" alert-secondary"></div>
                  </ContentTemplate>
                     <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                     </Triggers>
                 </asp:UpdatePanel>
             </div>

		 
     </div>




    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });

        function loader() {
            document.getElementById('htmlstring').value = '';
            document.getElementById('imagen').innerHTML='<img alt="" src="../shared/imgs/loader.gif">'
            return true;
        }
  </script>
</asp:Content>
