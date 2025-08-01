﻿<%@ Page Title="Reporte Truck Time" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="depot_out.aspx.cs" Inherits="CSLSite.exportables.depot_out" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ MasterType VirtualPath="~/site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    
    
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
  <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>




 

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
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Depot OUT</li>
          </ol>
        </nav>
      </div>
     <div class="dashboard-container p-4" id="cuerpo" runat="server">
           <div class="form-title">
             CRITERIOS DE BÚSQUEDAS
          </div>

          <div class="form-row"> 
                <div class="form-group col-md-6"> 
                         <label for="inputEmail4">FECHA DESDE:</label>
                        <asp:TextBox ID="TxtFechaDesde" runat="server"   class="datetimepicker form-control"  MaxLength="10"  placeholder="dd/MM/yyyy" onkeypress="return soloLetras(event,'1234567890/:')"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">  
                        <label for="inputEmail4">FECHA HASTA:</label>
                        <asp:TextBox ID="TxtFechaHasta" runat="server" class="datetimepicker form-control" MaxLength="10"  placeholder="dd/MM/yyyy" onkeypress="return soloLetras(event,'1234567890/:')"></asp:TextBox>
                          
                </div>
                 <div class="form-group col-md-6"> 
                    <label for="inputAddress">Deposito :</label>
                    <asp:DropDownList ID="cmbDeposito" class="form-control" runat="server" Enabled="false" Font-Size="Medium"   Font-Bold="true" ></asp:DropDownList>
                </div>
               <div class="form-group col-md-6"> 
                    <label for="inputAddress">Contenedor:</label>
                    <asp:TextBox ID="TxtContenedor" runat="server" class="form-control" MaxLength="20"
                    onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890 -',true)" ></asp:TextBox>
                </div>
         </div> 

          <asp:UpdatePanel ID="upresult" runat="server" >
     <ContentTemplate>
        <div class="row">
            <div class="col-md-12 d-flex justify-content-center">
                    <asp:Button ID="btbuscar" runat="server" class="btn btn-primary"   Text="BUSCAR"  OnClick="btbuscar_Click" />                             
                    <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />
            </div>    
        </div>    
            <br/>
        <div class="row">
        <div class="col-md-12 d-flex justify-content-center">
               <div id="sinresultado" runat="server" class=" alert  alert-warning" clientidmode="Static"></div>
        </div>
        </div>
                      
     </ContentTemplate>
        <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btbuscar" />
    </Triggers>
    </asp:UpdatePanel>


	<%--	  <div class="form-row">
		   <div class="form-group col-md-12"> 
		       <asp:UpdatePanel ID="upresult" runat="server" >
                     <ContentTemplate>
     

            <p id="corte" runat="server">Corte a la fecha:</p>

             
             <div class="botonera">
             <asp:Button ID="btbuscar"
                 runat="server" Text="Exportar documento"  
                 onclick="btbuscar_Click" 
                    CssClass="btn btn-primary"
                     ToolTip="Genera reporte a la fecha"  />
           </div>
   

                
                     </ContentTemplate>
          <Triggers>
              <asp:AsyncPostBackTrigger ControlID="btbuscar" />
          </Triggers>
                 </asp:UpdatePanel>
		   </div>
		  </div>--%>
		 
     </div>

  <div class="seccion">
                    


  </div>


     <script type="text/javascript" src="../lib/pages.js" ></script>

 <script type="text/javascript" src="../lib/advanced-form-components.js"></script>



    <script type="text/javascript">
        function descarga(fname, hname, tbname) {
            var iframe = document.createElement("iframe");
            iframe.src = "../handler/fileExcel.ashx?name="+fname+"&page="+hname+"&obj="+tbname
            iframe.style.display = "none";
            document.body.appendChild(iframe);
        }
    </script>

   <script type="text/javascript">
            $(document).ready(function () {
                 $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'm/d/Y' });
              });    
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