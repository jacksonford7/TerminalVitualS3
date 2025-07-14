<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="facturacionagencia.aspx.cs" Inherits="CSLSite.facturacionagencia" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

  <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
 <!-- Bootstrap core CSS -->
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>

  <!--external css-->
  <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
  <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>


 <link href="../lib/advanced-datatable/css/demo_page.css" rel="stylesheet" />
  <link href="../lib/advanced-datatable/css/demo_table.css" rel="stylesheet" />
  <link rel="stylesheet" href="../lib/advanced-datatable/css/DT_bootstrap2.css" />



        <style type="text/css">
        body
        {
            /*font-family: Arial;
            font-size: 10pt;*/
        }
        .modal
        {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }

        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
        }
        .modalPopup
        {
            background-color: #FFFFFF;
            width: 726px;
            border: 3px solid #FF3720;
            padding: 0;
        }
        .modalPopup .header
        {
            background-color: #2FBDF1;
            height: 30px;
            color: White;
            line-height: 30px;
            text-align: center;
            font-weight: bold;
        }
        .modalPopup .body
        {
            min-height: 50px;
            line-height: 25px;
            text-align: center;
            /*font-weight: bold;*/
            margin-bottom: 5px;
        }
    </style>
 
    
<script type="text/javascript">
$(document).ready(function(){
  $('[data-toggle="tooltip"]').tooltip();   
});
</script>

<script type="text/javascript">


 function BindFunctions() {

     $(document).ready(function() {
      /*
       * Insert a 'details' column to the table
       */
      var nCloneTh = document.createElement('th');
      var nCloneTd = document.createElement('td');
      nCloneTd.innerHTML = '<img src="../lib/advanced-datatable/images/details_open.png">';
      nCloneTd.className = "center";

      $('#<%= tablePagination.ClientID %> thead tr').each(function() {
        //this.insertBefore(nCloneTh, this.childNodes[0]);
      });

      $('#<%= tablePagination.ClientID %> tbody tr').each(function() {
        //this.insertBefore(nCloneTd.cloneNode(true), this.childNodes[0]);
      });

      /*
       * Initialse DataTables, with no sorting on the 'details' column
       */
      var oTable = $('#<%= tablePagination.ClientID %>').dataTable({
        "aoColumnDefs": [{
          "bSortable": false,
          "aTargets": [0]
        }],
        "aaSorting": [
          [1, 'asc']
        ]
      });

      /* Add event listener for opening and closing details
       * Note that the indicator for showing which row is open is not controlled by DataTables,
       * rather it is done here
       */
      $('#<%= tablePagination.ClientID %> tbody td img').live('click', function() {
        var nTr = $(this).parents('tr')[0];
        if (oTable.fnIsOpen(nTr)) {
          /* This row is already open - close it */
          this.src = "../lib/advanced-datatable/media/images/details_open.png";
          oTable.fnClose(nTr);
        } else {
          /* Open this row */
          this.src = "../lib/advanced-datatable/images/details_close.png";
          oTable.fnOpen(nTr, fnFormatDetails(oTable, nTr), 'details');
        }
      });
        });
    }
</script>


    
<script type="text/javascript">


 function BindFunctionsMuelle() {

     $(document).ready(function() {
      /*
       * Insert a 'details' column to the table
       */
      var nCloneTh = document.createElement('th');
      var nCloneTd = document.createElement('td');
      nCloneTd.innerHTML = '<img src="../lib/advanced-datatable/images/details_open.png">';
      nCloneTd.className = "center";

      $('#<%= tableMuelle.ClientID %> thead tr').each(function() {
        //this.insertBefore(nCloneTh, this.childNodes[0]);
      });

      $('#<%= tableMuelle.ClientID %> tbody tr').each(function() {
        //this.insertBefore(nCloneTd.cloneNode(true), this.childNodes[0]);
      });

      /*
       * Initialse DataTables, with no sorting on the 'details' column
       */
      var oTable = $('#<%= tableMuelle.ClientID %>').dataTable({
        "aoColumnDefs": [{
          "bSortable": false,
          "aTargets": [0]
        }],
        "aaSorting": [
          [1, 'asc']
        ]
      });

      /* Add event listener for opening and closing details
       * Note that the indicator for showing which row is open is not controlled by DataTables,
       * rather it is done here
       */
      $('#<%= tableMuelle.ClientID %> tbody td img').live('click', function() {
        var nTr = $(this).parents('tr')[0];
        if (oTable.fnIsOpen(nTr)) {
          /* This row is already open - close it */
          this.src = "../lib/advanced-datatable/media/images/details_open.png";
          oTable.fnClose(nTr);
        } else {
          /* Open this row */
          this.src = "../lib/advanced-datatable/images/details_close.png";
          oTable.fnOpen(nTr, fnFormatDetails(oTable, nTr), 'details');
        }
      });
        });
    }
</script>

 <script type="text/javascript">
        $(document).ready(function () {
            $('#<%= tablePagination.ClientID %>').dataTable();
        });
       
    </script>

  <script type="text/javascript">
        $(document).ready(function () {
            $('#<%= tableMuelle.ClientID %>').dataTable();
        });
       
    </script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

  <div id="div_BrowserWindowName" style="visibility:hidden;">
    <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
     
  </div>
    <asp:HiddenField ID="manualHide" runat="server" />

      <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Bodega BTS</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">FACTURACIÓN POR BODEGA</li>
          </ol>
        </nav>
      </div>
<div class="dashboard-container p-4" id="cuerpo" runat="server">
    <div class="form-title">
         CRITERIO DE BUSQUEDA
     </div>
		
       <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
       <ContentTemplate>
       <div class="form-row"> 
           <div class="form-group col-md-4">
              <label for="inputZip">REFERENCIA<span style="color: #FF0000; font-weight: bold;">*</span></label>
                <asp:TextBox ID="TXTMRN" runat="server" class="form-control" MaxLength="16"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')"  
                                 placeholder="NAVE REFERENCIA"></asp:TextBox>
            </div>
             
            <div class="form-group col-md-2">
                 <label for="inputZip">&nbsp;</label>
                   <div class="d-flex">
                        <asp:Button ID="BtnBuscar" runat="server" class="btn btn-primary"   Text="BUSCAR"  OnClientClick="return mostrarloader('1')" OnClick="BtnBuscar_Click" />   
                         <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />
                         
                  </div>
            </div>
       </div>
		 <br/>
        <div class="row">
            <div class="col-md-12 d-flex justify-content-center">
                 <div class="alert alert-danger" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar la referencia......</div>
            </div>
         </div>				
                
        </ContentTemplate>
            <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
        </Triggers>
        </asp:UpdatePanel>
                        
        <h4 class="mb">DATOS DE LA FACTURA</h4>
        <asp:UpdatePanel ID="UPDATOSCLIENTE" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
        <div class="form-row">
             
             <div class="form-group col-md-12"> 
                 <label for="inputAddress">LINEA:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                  <asp:TextBox ID="TXTASUMEFACTURA" runat="server" class="form-control"  placeholder=""  Font-Bold="false" disabled Visible="false"></asp:TextBox>
                    <asp:HiddenField ID="hf_idasume" runat="server" />
                    <asp:HiddenField ID="hf_descasume" runat="server" />
                               
                    <asp:DropDownList runat="server" ID="CboAsumeFactura"    AutoPostBack="false"  class="form-control"  >
                        </asp:DropDownList>
             </div> 
            
       </div>
                        
        </ContentTemplate>
            <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
        </Triggers>
    </asp:UpdatePanel>

     <div class="form-row">
        <div class="form-group col-md-12">
                    <label for="inputZip">FACTURADO A:<span style="color: #FF0000; font-weight: bold;"></span></label>
                    <asp:TextBox ID="Txtempresa"  runat="server" class="form-control"  autocomplete="off"  onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789_')"></asp:TextBox>                      
                   <asp:HiddenField ID="IdTxtempresa" runat="server" ClientIDMode="Static"/> 
         </div>
     </div>
    <div class="form-row">
             
             <div class="form-group col-md-12"> 
                 <label for="inputAddress">EMITIR FACTURA DE:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                  
                    <asp:DropDownList runat="server" ID="CboTipoFactura"    AutoPostBack="false"  class="form-control"  >
                         <asp:ListItem Value="1">BODEGA FRIA/SECA</asp:ListItem>
                         <asp:ListItem Value="2">MUELLE</asp:ListItem>
                        </asp:DropDownList>
             </div> 
            
       </div>
     <div class="form-row">  
        <div class="form-group col-md-4">
                     
               <label for="inputEmail4">FECHA FACTURA<span style="color: #FF0000; font-weight: bold;">*</span></label>
               <div class="d-flex">
                    <asp:TextBox runat="server" ID="TxtFechaHasta"   MaxLength="16" 
                                    onkeypress="return soloLetras(event,'0123456789-')"    class="datetimepicker form-control" ></asp:TextBox>
                    &nbsp;&nbsp;
                   
               
                       <asp:TextBox ID="Txtcomentario" runat="server" class="form-control" MaxLength="150" Width="330px" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')"  
                       placeholder="Comentario" Visible="false"></asp:TextBox>
               </div>
                         
       </div>

        
    </div>
    
    <div class="form-row">  

      
        

        <div class="form-group col-md-12">
     <asp:UpdatePanel ID="UPDETALLE" runat="server" UpdateMode="Conditional" >  
       <ContentTemplate>
         
            
           <h4 id="LabelTotal" runat="server" class="mb">BODEGA FRIA/SECA</h4>
 
            <asp:GridView ID="tablePagination" runat="server" AutoGenerateColumns="False"  DataKeyNames="Fila" OnPageIndexChanging="tablePagination_PageIndexChanging" OnPreRender="tablePagination_PreRender"   OnRowDataBound="tablePagination_RowDataBound" 
                                    GridLines="None" 
                                    PageSize="30"
                                    AllowPaging="True"
                                   CssClass="table table-bordered invoice">
                    <PagerStyle HorizontalAlign = "Right" CssClass="table table-bordered invoice"   />
                    <RowStyle  BackColor="#F0F0F0" />
                    <alternatingrowstyle  BackColor="#FFFFFF" />
                           
                                    <Columns>
                                                 
                                        <asp:BoundField DataField="Fila" HeaderText="#"  HeaderStyle-HorizontalAlign="Center" />
                                      
                                        <asp:BoundField DataField="desc_bodega" HeaderText="BODEGA" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center"/>
                                        <asp:BoundField DataField="tipo_bodega" HeaderText="TIPO"   HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="desc_modalidad" HeaderText="MODALIDAD"   HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="codLine" HeaderText="LINEA"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="ruc" HeaderText="RUC"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="Exportador" HeaderText="EXPORTADOR"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="QTY_out" HeaderText="CAJAS" DataFormatString="{0:N}" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />

                                    </Columns>
                                </asp:GridView>
                  
 
           </ContentTemplate>
     </asp:UpdatePanel>   
     </div><!--content-panel-->
     
     <div class="form-group col-md-12">
     <asp:UpdatePanel ID="UPMUELLE" runat="server" UpdateMode="Conditional" >  
       <ContentTemplate>
         
            
           <h4 id="LabelMuelle" runat="server" class="mb">MUELLE</h4>
 
            <asp:GridView ID="tableMuelle" runat="server" AutoGenerateColumns="False"  DataKeyNames="Fila" OnPageIndexChanging="tableMuelle_PageIndexChanging" OnPreRender="tableMuelle_PreRender"   OnRowDataBound="tableMuelle_RowDataBound" 
                                    GridLines="None" 
                                    PageSize="100"
                                    AllowPaging="True"
                                   CssClass="table table-bordered invoice">
                    <PagerStyle HorizontalAlign = "Right" CssClass="table table-bordered invoice"   />
                    <RowStyle  BackColor="#F0F0F0" />
                    <alternatingrowstyle  BackColor="#FFFFFF" />
                           
                                    <Columns>
                                                 
                                        <asp:BoundField DataField="Fila" HeaderText="#"  HeaderStyle-HorizontalAlign="Center" />
                                       <asp:BoundField DataField="codLine" HeaderText="LINEA"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="aisv_codig_clte" HeaderText="RUC"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="aisv_nom_expor" HeaderText="EXPORTADOR"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="cajas" HeaderText="GRANEL" DataFormatString="{0:N}" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                         <asp:BoundField DataField="cajas_paletizado" HeaderText="PALETIZADO" DataFormatString="{0:N}" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    </Columns>
                                </asp:GridView>
                  
 
           </ContentTemplate>
     </asp:UpdatePanel>   
     </div><!--content-panel-->

    </div><!--row mb-->
   
    
    <asp:UpdatePanel ID="UPBOTONES" runat="server" UpdateMode="Conditional" >  
             <ContentTemplate>

               <div class="row">
                <div class="col-md-12 d-flex justify-content-center">
                         <div class="alert alert-danger" id="banmsg_det" runat="server" clientidmode="Static"><b>Error!</b>Debe ingresar el número de la carga MRN......</div>
                </div>
              </div>
              <br/>

             <div class="row">
                <div class="col-md-12 d-flex justify-content-center">
                      <div class="alert alert-danger" id="banmsg_Pase" runat="server" clientidmode="Static"><b>Error!</b>.</div>
                 </div>
             </div>
             <br/>

             <div class="row">
                <div class="col-md-12 d-flex justify-content-center">
                             <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCargaDet" class="nover"  />
                 </div>
            </div>    
            <br/>


             <div class="row">
             <div class="col-md-12 d-flex justify-content-center">
                             
                    <asp:Button ID="BtnNuevo" runat="server" class="btn btn-outline-primary mr-4"  Text="NUEVA TRANSACCION"  OnClick="BtnNuevo_Click"  />

                   <asp:Button ID="BtnExportar" runat="server" class="btn btn-outline-primary mr-4" Text="EXPORTAR A EXCEL" OnClientClick="exportar();" Visible="true"/>

                    
                   <asp:Button ID="BtnFacturar" runat="server" class="btn btn-primary" Text="GENERAR BORRADOR" OnClientClick="return mostrarloader('2')" OnClick="BtnFacturar_Click" />
               </div> 
             </div>
            </ContentTemplate>
             </asp:UpdatePanel>   
   

       
    
</div>

  <script type="text/javascript" src="../lib/jquery/jquery.min.js"></script>
     <script  type="text/javascript" src="../lib/jquery.dcjqaccordion.2.7.js"></script>
  <script type="text/javascript" src="../lib/jquery.scrollTo.min.js"></script>
  <script type="text/javascript" src="../lib/jquery.nicescroll.js" ></script>
 <script type="text/javascript" src="../lib/jquery.sparkline.js"></script>
 


   <script type="text/javascript" src="../lib/common-scripts.js"></script>
 
   <script type="text/javascript" src="../lib/pages.js" ></script>

   <script type="text/javascript" src="../lib/advanced-form-components.js"></script>
    <script type="text/javascript" src='../lib/autocompletar/bootstrap3-typeahead.min.js'></script>
 

    <script type="text/javascript">
   

    function mostrarloader(Valor) {

        try {
            if (Valor == "1") {
                document.getElementById("ImgCarga").className = 'ver';
            }
            else {
                document.getElementById("ImgCargaDet").className='ver';
            }
                
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }


    function ocultarloader(Valor) {
        try {

            if (Valor == "1") {
                document.getElementById("ImgCarga").className = 'nover';
            }
            else {
                document.getElementById("ImgCargaDet").className='nover';
            }

             } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

      
  </script>
       <script type="text/javascript">
            $(document).ready(function () {
                 $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'm/d/Y' });
              });    
      </script>

    <script type="text/javascript">

        $(function () {
            $('[id*=Txtempresa]').typeahead({
                hint: true,
                highlight: true,
                minLength: 5,
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("facturacionagencia.aspx/GetEmpresas") %>',
                    data: "{ 'prefix': '" + request + "'}",
                    dataType: "json",
                    type: "POST",
                    maxJsonLength: 1,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        items = [];
                        map = {};
                        $.each(data.d, function (i, item) {
                            var id = item.split('+')[0];
                            var name = item.split('+')[1];
                            map[name] = { id: id, name: name };
                            items.push(name);
                        });
                        response(items);
                        $(".dropdown-menu").css("height", "auto");
                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
            },
            updater: function (item) {
                $('[id*=IdTxtempresa]').val(map[item].id);
                //alert(map[item].id);
                //alert($("#IdTxtempresa").val());
                return item;
            }
            });
        });

</script>


    <script type="text/javascript">

        function exportar()
        {
            var Referencia = document.getElementById('<%= TXTMRN.ClientID %>').value;
            var Cbolinea = document.getElementById('<%= hf_idasume.ClientID %>');
            var SelLin = Cbolinea.value;
          

            var xhr = new XMLHttpRequest();
            xhr.open('POST', 'facturacionagencia.aspx/ExportarExcel', true);
            xhr.setRequestHeader('Content-Type', 'application/json; charset=utf-8');
            xhr.responseType = 'blob';

            xhr.onload = function ()
            {
                var fileName = "Reporte_Soporte.xlsx";
                if (xhr.status === 200) {
                    var blob = xhr.response;

                  
                    var link = document.createElement('a');
                    link.href = window.URL.createObjectURL(blob);
                    link.download = fileName;
                    link.style.display = 'none';
                    document.body.appendChild(link);
                    link.click();
                    document.body.removeChild(link);
                } else {
                    console.log('Error al exportar el archivo Excel. Código de estado: ' + xhr.status);
                }
            };

            xhr.onerror = function () {
                console.log('Error al enviar la solicitud de exportación del archivo Excel.');
            };

            xhr.send(JSON.stringify({ pReferencia: Referencia, pLinea: SelLin }));
        }
    </script>
     
</asp:Content>