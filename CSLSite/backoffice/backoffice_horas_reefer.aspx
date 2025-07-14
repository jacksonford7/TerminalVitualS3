<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="backoffice_horas_reefer.aspx.cs" Inherits="CSLSite.backoffice_horas_reefer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

    <link href="../img/favicon2.png" rel="icon" />
    <link href="../img/icono.png" rel="apple-touch-icon" />
    <!-- Bootstrap core CSS -->
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/dashboard.css" rel="stylesheet" />
    <link href="../css/icons.css" rel="stylesheet" />
    <link href="../css/style.css" rel="stylesheet" />

    <!--external css-->
    <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />



    <script type="text/javascript">

        function BindFunctions() {

            $(document).ready(function () {
                /*
                 * Insert a 'details' column to the table
                 */
                var nCloneTh = document.createElement('th');
                var nCloneTd = document.createElement('td');
                nCloneTd.innerHTML = '<img src="../lib/advanced-datatable/images/details_open.png">';
                nCloneTd.className = "center";

                $('#<%= tablePagination.ClientID %> thead tr').each(function () {
             //this.insertBefore(nCloneTh, this.childNodes[0]);
         });

         $('#<%= tablePagination.ClientID %> tbody tr').each(function () {
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
         $('#<%= tablePagination.ClientID %> tbody td img').live('click', function () {
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



        function BindFunctions2() {

            $(document).ready(function () {
                /*
                 * Insert a 'details' column to the table
                 */
                var nCloneTh = document.createElement('th');
                var nCloneTd = document.createElement('td');
                nCloneTd.innerHTML = '<img src="../lib/advanced-datatable/images/details_open.png">';
                nCloneTd.className = "center";

                $('#<%= tablePagination2.ClientID %> thead tr').each(function () {
             //this.insertBefore(nCloneTh, this.childNodes[0]);
         });

         $('#<%= tablePagination2.ClientID %> tbody tr').each(function () {
             //this.insertBefore(nCloneTd.cloneNode(true), this.childNodes[0]);
         });

         /*
          * Initialse DataTables, with no sorting on the 'details' column
          */
         var oTable = $('#<%= tablePagination2.ClientID %>').dataTable({
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
         $('#<%= tablePagination2.ClientID %> tbody td img').live('click', function () {
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
            $('#<%= tablePagination2.ClientID %>').dataTable();
        });

    </script>



</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">

    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

    <div id="div_BrowserWindowName" style="visibility: hidden;">
        <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />

    </div>

    <div class="mt-4">
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">BackOffice</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">SUBIR HORAS REEFER (IMPO/EXPO)</li>
          </ol>
        </nav>
    </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">
         <div class="form-title">
            DATOS DEL USUARIO
        </div>
        
        <div class="form-row">
            <div class="form-group col-md-6"> 
                <label for="inputAddress">ESTIMADO CLIENTE:<span style="color: #FF0000; font-weight: bold;"></span></label>
                   <asp:TextBox ID="Txtcliente" runat="server" class="form-control" 
                                placeholder=""  Font-Bold="true" disabled ></asp:TextBox>
            </div>
            <div class="form-group col-md-2">
                <label for="inputAddress">RUC:<span style="color: #FF0000; font-weight: bold;"></span></label>
                  <span class="help-block">RUC:</span>
						        <asp:TextBox ID="Txtruc" runat="server" class="form-control" 
                                placeholder=""  Font-Bold="true" disabled></asp:TextBox>
            </div>
            <div class="form-group col-md-4">
                <label for="inputAddress">EMPRESA:<span style="color: #FF0000; font-weight: bold;"></span></label>
                  <asp:TextBox ID="Txtempresa" runat="server" class="form-control"  
                                placeholder=""  Font-Bold="true" disabled></asp:TextBox>
            </div>
        </div>
        <div class="form-title">
            DATOS DE LA CARGA
        </div>
         <div class="form-row">
             <div class="form-group col-md-6"> 
                 <label for="inputAddress">BUSCAR ARCHIVO XLSX<span style="color: #FF0000; font-weight: bold;"></span></label>
                  <asp:FileUpload id="FileUpload1"  runat="server" title="Escoja el archivo con formato indicado .xlsx"  class="form-control"  visible="true"> </asp:FileUpload>

             </div>
              <div class="form-group col-md-2">
                 <label for="inputAddress">&nbsp;<span style="color: #FF0000; font-weight: bold;"></span></label>
                   <asp:Label ID="LblRuta" runat="server" Text="" Font-Bold="true" ForeColor="Red" class="form-control"></asp:Label>
             </div>
             <div class="form-group col-md-4">
                <label for="inputAddress">&nbsp;<span style="color: #FF0000; font-weight: bold;"></span></label>
                  <div class="d-flex">
                       <asp:Button ID="BtnNuevo" runat="server" class="btn btn-outline-primary mr-4"  Text="Nuevo"    OnClick="BtnNuevo_Click" />   &nbsp; 
                      <asp:Button ID="BtnCargar" runat="server" class="btn btn-primary"  Text="Cargar Archivo"   OnClick="BtnCargar_Click" />   &nbsp;
                        <asp:Button ID="BtnGrabar" runat="server" class="btn btn-primary"  Text="Subir/Procesar Archivo"   OnClientClick="return confirmacion()" OnClick="BtnGrabar_Click" />  &nbsp;  
                        <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />
                  </div>
            </div>
         </div>

         <div class="row">
            <div class="col-md-12 d-flex justify-content-center">
                 <div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >......</div>
           </div>
         </div>
         <div class="row">
            <div class="col-md-12 d-flex justify-content-center">
                   <div class="alert alert-warning" id="banmsg2" runat="server" clientidmode="Static"><b>Error!</b> >......</div>
           </div>
         </div>

       <h4 class="mb" runat="server" id="det1">DETALLE DE UNIDADES A PROCESAR</h4>
        <div class="form-row">
            <div class="form-group col-md-12">
                 <div class="table table-bordered invoice">   
                  <asp:GridView ID="tablePagination" runat="server" AutoGenerateColumns="False"  DataKeyNames="gkey"  OnRowDataBound="tablePagination_RowDataBound" 
                                                GridLines="None" 
                                               PageSize="1500"
                                     
                                              CssClass="table table-bordered invoice">
                              <PagerStyle HorizontalAlign = "Right" CssClass="table table-bordered invoice" />
                               <RowStyle  BackColor="#F0F0F0" />
                               <alternatingrowstyle  BackColor="#FFFFFF" />
                           
                                                <Columns>
                                                 
                                                     <asp:BoundField DataField="linea" HeaderText="LINEA"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="referencia" HeaderText="REFERENCIA" SortExpression="CONTENEDOR"  HeaderStyle-CssClass="gradeC center hidden-phone" HeaderStyle-HorizontalAlign="Center"/>
                                                    <asp:BoundField DataField="trafico" HeaderText="CATEGORIA"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone" HeaderStyle-HorizontalAlign="Center"/>
                                                    <asp:BoundField DataField="id" HeaderText="UNIDAD"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone" HeaderStyle-HorizontalAlign="Center"/>
                                                    <asp:BoundField DataField="horas" HeaderText="HORAS"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone" HeaderStyle-HorizontalAlign="Center"/>
                                                    <asp:BoundField DataField="gkey" HeaderText="GKEY"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"  Visible="false"/>
                                                    <asp:BoundField DataField="valido" HeaderText="VALIDO"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"  Visible="false"/>
                                                    <asp:BoundField DataField="novedad" HeaderText="NOVEDAD"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone" HeaderStyle-HorizontalAlign="Center"/>
                                                </Columns>
                  </asp:GridView>
                 </div>
            </div>
        </div>
        
        <h4 class="mb" runat="server" id="det2">DETALLE DE UNIDADES CON ERRORES</h4>
        <div class="form-row">
            <div class="form-group col-md-12">
                 <div class="table table-bordered invoice">   
                    <asp:GridView ID="tablePagination2" runat="server" AutoGenerateColumns="False"  DataKeyNames="gkey"  
                                                GridLines="None" 
                                               PageSize="1500"
                                               
                                                CssClass="table table-bordered invoice">
                              <PagerStyle HorizontalAlign = "Right" CssClass="table table-bordered invoice"  />
                               <RowStyle  BackColor="#F0F0F0" />
                               <alternatingrowstyle  BackColor="#FFFFFF" />
                           
                                                <Columns>
                                                 
                                                     <asp:BoundField DataField="linea" HeaderText="LINEA"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone" HeaderStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="referencia" HeaderText="REFERENCIA" SortExpression="CONTENEDOR"  HeaderStyle-CssClass="gradeC center hidden-phone" HeaderStyle-HorizontalAlign="Center"/>
                                                    <asp:BoundField DataField="trafico" HeaderText="CATEGORIA"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone" HeaderStyle-HorizontalAlign="Center"/>
                                                    <asp:BoundField DataField="id" HeaderText="UNIDAD"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone" HeaderStyle-HorizontalAlign="Center"/>
                                                    <asp:BoundField DataField="horas2" HeaderText="HORAS"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone" HeaderStyle-HorizontalAlign="Center"/>
                                                    <asp:BoundField DataField="gkey" HeaderText="GKEY"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"  Visible="false"/>
                                                    <asp:BoundField DataField="valido" HeaderText="VALIDO"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"  Visible="false"/>
                                                    <asp:BoundField DataField="novedad" HeaderText="NOVEDAD"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone" HeaderStyle-HorizontalAlign="Center"/>
                                                  </Columns>
                             </asp:GridView>
                 </div>
            </div>
        </div>

       

    </div>

     <script type="text/javascript" src="lib/jquery/jquery.min.js"></script>
   <script type="text/javascript" src="lib/bootstrap/js/bootstrap.min.js"></script>
  <script type="text/javascript" src='lib/autocompletar/bootstrap3-typeahead.min.js'></script>
 

  <script type="text/javascript" src="../lib/pages.js"></script>



    <script type="text/javascript">

        function confirmacion() {
            var mensaje;
            var opcion = confirm("Estimado usuario, está seguro que desea subir las horas reefer? ");
            if (opcion == true) {

                return true;
            } else {

                return false;
            }


        }

        function mostrarloader(Valor) {

            try {
                if (Valor == "1") {
                    document.getElementById("ImgCarga").className = 'ver';
                }
                else {

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

                }

            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

    </script>


</asp:Content>
