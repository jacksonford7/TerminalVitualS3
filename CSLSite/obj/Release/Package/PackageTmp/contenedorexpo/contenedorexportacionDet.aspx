<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="contenedorexportacionDet.aspx.cs" Inherits="CSLSite.contenedorexpo.contenedorexportacionDet" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<!DOCTYPE html>

<link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <!-- Bootstrap core CSS -->
  <link href="../lib/bootstrap/css/bootstrap.min.css" rel="stylesheet"/>
  <!--external css-->
  <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
  <link rel="stylesheet" type="text/css" href="../lib/gritter/css/jquery.gritter.css" />


  <link href="../lib/advanced-datatable/css/demo_page.css" rel="stylesheet" />
  <link href="../lib/advanced-datatable/css/demo_table.css" rel="stylesheet" />
  <link rel="stylesheet" href="../lib/advanced-datatable/css/DT_bootstrap.css" />
   
  <link rel="stylesheet" type="text/css" href="../lib/bootstrap-datepicker/css/datepicker.css" />
  <link rel="stylesheet" type="text/css" href="../lib/bootstrap-daterangepicker/daterangepicker.css" />
  <link rel="stylesheet" type="text/css" href="../lib/bootstrap-timepicker/compiled/timepicker.css" />
  <link rel="stylesheet" type="text/css" href="../lib/bootstrap-datetimepicker/datertimepicker.css" />

  <link rel="stylesheet" href="../lib/file-uploader/css/jquery.fileupload.css"/>
  <link rel="stylesheet" href="../lib/file-uploader/css/jquery.fileupload-ui.css"/>


  <link href="../css/style.css" rel="stylesheet"/>
  <link href="../css/style-responsive.css" rel="stylesheet"/>
  <link href="../css/pagination.css" rel="stylesheet"/>

 


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>

   <%-- <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.5.1/jquery.min.js"></script>
    <script type="text/javascript" src="http://ajax.microsoft.com/ajax/jquery.ui/1.8.6/jquery-ui.min.js"></script>
    <link type="text/css" rel="Stylesheet" href="http://ajax.microsoft.com/ajax/jquery.ui/1.8.6/themes/smoothness/jquery-ui.css">--%>
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

        });

        $('#<%= tablePagination.ClientID %> tbody tr').each(function () {

        });

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
    </script>

     <script type="text/javascript">
         $(document).ready(function () {
             $('#<%= tablePagination.ClientID %>').dataTable();
         });

    </script> 
</head>
<body>
    
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

        <asp:UpdatePanel ID="UPDETALLE" runat="server" UpdateMode="Conditional" >  
            <ContentTemplate>
    
               
                <asp:Panel ID="pnlEdit" runat="server">
         
                <p>BOOKING:<asp:TextBox ID="txtID" ReadOnly="true" runat="server"></asp:TextBox></p>
                    
                
                </asp:Panel>
   
     
                   <section class="wrapper2">
                   <%--  <h3>DETALLE DE CONTENEDORES</h3>--%>
                     <div class="row mb"> 
                      <div class="content-panel">
                          <div class="adv-table">
                 
                                <asp:GridView ID="tablePagination" runat="server" AutoGenerateColumns="False"  DataKeyNames="CNTR_CONTAINER"
                                                        GridLines="None" OnPageIndexChanging="tablePagination_PageIndexChanging" OnPreRender="tablePagination_PreRender"   
                                                       PageSize="20"
                                                       AllowPaging="True"
                                                        CssClass="display table table-bordered">
                                      <PagerStyle HorizontalAlign = "Right" CssClass="pagination-ys"  />
                                       <RowStyle  BackColor="#F0F0F0" />
                                       <alternatingrowstyle  BackColor="#FFFFFF" />
                           
                                    <Columns>                              
                                
                                        <asp:BoundField DataField="CNTR_CONTAINER"  HeaderText="CONTENEDOR" SortExpression="CNTR_CONTAINER" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                        <asp:BoundField DataField="CNTR_TYSZ_SIZE" HeaderText="SIZE"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                        <asp:BoundField DataField="CNTR_TYSZ_ISO" HeaderText="ISO"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                        <asp:BoundField DataField="CNTR_TYSZ_TYPE" HeaderText="TIPO" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                        <asp:BoundField DataField="CNTR_FULL_EMPTY_CODE" HeaderText="FULL/EMPTY" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                        <asp:BoundField DataField="CNTR_YARD_STATUS" HeaderText="ESTADO"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                        <asp:BoundField DataField="CNTR_AISV" HeaderText="AISV" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                        <asp:BoundField DataField="CNTR_HOLD" HeaderText="HOLD" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                        <asp:BoundField DataField="CNTR_REEFER_CONT" HeaderText="REEFER_CONT"   HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                

                                    </Columns>
                               </asp:GridView>
                  
                          </div>

                       </div><!--content-panel-->
                     </div><!--row mb-->

                </section><!--wrapper2-->
     
           </ContentTemplate>
     </asp:UpdatePanel>   

    </form>
</body>
</html>
