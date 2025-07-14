<%@ Page Title="Reporte despachos" Language="C#" MasterPageFile="~/site.Master" 
    AutoEventWireup="true" CodeBehind="vacios_despacho.aspx.cs" 
    Inherits="CSLSite.exportables.vacios_despacho" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
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
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Reportes</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Reporte despacho de unidades vacías (CONTAINER)</li>
          </ol>
        </nav>
      </div>

     <div class="dashboard-container p-4" id="cuerpo" runat="server">
         <div class="form-title">Datos de unidades buscadas</div>
		 
		  <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Salieron desde el día<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                     <asp:TextBox ID="desded" runat="server" MaxLength="10" 
                CssClass="datetimepicker form-control"
             onkeypress="return soloLetras(event,'01234567890/')" 
                 onblur="valDate(this,true,valdate);" ClientIDMode="Static"></asp:TextBox>

			  </div>
		   </div>

               <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Hasta<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                         <asp:TextBox ID="hastad" runat="server" 
                 ClientIDMode="Static"  MaxLength="15" 
                 CssClass="datetimepicker  form-control"
             onkeypress="return soloLetras(event,'01234567890/')" 
                  onblur="valDate(this,true,valdate);"></asp:TextBox>
                     <span id="valdate" class="validacion"> * </span>
			  </div>
		   </div>
		  </div>

           <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
		     <div class="d-flex">
                              <div class="cataresult" >
               <asp:UpdatePanel ID="upresult" runat="server" >
                     <ContentTemplate>
         <div class="botonera">
          <span id="imagen"></span>
             <asp:Button ID="btbuscar"  CssClass="btn btn-primary"
                 runat="server" Text="Iniciar la búsqueda"  
                 onclick="btbuscar_Click" 
                 OnClientClick="return validateDatesRange('desded','hastad','');" />
         </div>
             <div id="xfinder" runat="server" visible="false" >
             <div class="findresult" >
             <div class="booking" >
                  <div id="sinresultado" runat="server" class=" alert-info"></div>
             </div>
             </div>
              </div>
                  </ContentTemplate>
                     <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                     </Triggers>
                 </asp:UpdatePanel>
             </div>


		     </div>
		   </div> 
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
  </script>

<%--  <asp:updateprogress associatedupdatepanelid="upresult"
    id="updateProgress" runat="server">
    <progresstemplate>
        <div id="progressBackgroundFilter"></div>
        <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor 
            espere...<br />
             <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
        </div>
    </progresstemplate>
</asp:updateprogress>--%>


        <script type="text/javascript">
        function descarga(fname, hname, tbname) {
            var iframe = document.createElement("iframe");
            iframe.src = "../handler/fileExcel.ashx?name="+fname+"&page="+hname+"&obj="+tbname
            iframe.style.display = "none";
            document.body.appendChild(iframe);
        }
    </script>


  </asp:Content>
