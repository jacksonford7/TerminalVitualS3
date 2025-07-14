<%@ Page Title="Reporte de inventario Unidades Llenas" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="dwelltime.aspx.cs" Inherits="CSLSite.exportables.dwelltime" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ MasterType VirtualPath="~/site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
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
              <input id="bandera"     type="hidden"   runat="server" clientidmode="Static"  />
              <input id="procesar"    type="hidden"   runat="server" clientidmode="Static"  />
              <input id="itemT4"      type="hidden"   runat="server" clientidmode="Static"  />


         <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Reportes</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">DWELL TIME</li>
          </ol>
        </nav>
      </div>
     <div class="dashboard-container p-4" id="cuerpo" runat="server">
                         <div   class=" alert alert-warning">
                              Este archivo exportable contiene la información, 
           sobre el tiempo de permanencia de las unidades en la terminal CGSA.

                         </div>
		  <div class="form-row">
		   <div class="form-group col-md-12"> 
		       <asp:UpdatePanel ID="upresult" runat="server" >
                     <ContentTemplate>
     

            <p id="corte" runat="server">Corte a la fecha:</p>

             
             <div class="botonera">
             <asp:Button ID="btbuscar"
                 runat="server" Text="Exportar Información"  
                 onclick="btbuscar_Click" 
                    CssClass="btn btn-primary"
                     ToolTip="Genera reporte a la fecha"  />
           </div>
   

                 <div id="sinresultado" runat="server" class=" alert  alert-warning" clientidmode="Static"></div>
                     </ContentTemplate>
          <Triggers>
              <asp:AsyncPostBackTrigger ControlID="btbuscar" />
          </Triggers>
                 </asp:UpdatePanel>
		   </div>
		  </div>
		 
     </div>

  <div class="seccion">
                    


  </div>






    <script type="text/javascript">
        function descarga(fname, hname, tbname) {
            var iframe = document.createElement("iframe");
            iframe.src = "../handler/fileExcel.ashx?name="+fname+"&page="+hname+"&obj="+tbname
            iframe.style.display = "none";
            document.body.appendChild(iframe);
        }
    </script>




    <asp:updateprogress  associatedupdatepanelid="upresult"   id="updateProgress" runat="server">
    <progresstemplate>
        <div id="progressBackgroundFilter"></div>
        <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor 
            espere...<br />
             <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
        </div>
    </progresstemplate>
</asp:updateprogress>
</asp:Content>