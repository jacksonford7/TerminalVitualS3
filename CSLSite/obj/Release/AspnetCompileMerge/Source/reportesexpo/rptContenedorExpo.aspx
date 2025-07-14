<%--<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="rptContenedorExpo.aspx.cs" Inherits="SiteBillion.reportesexpo.rptContenedorExpo" %>--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="rptContenedorExpo.aspx.cs" Inherits="CSLSite.reportesexpo.rptContenedorExpo" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ MasterType VirtualPath="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
<%--  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../lib/bootstrap/css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
  <link rel="stylesheet" type="text/css" href="../lib/gritter/css/jquery.gritter.css" />
  <link href="../lib/advanced-datatable/css/demo_page.css" rel="stylesheet" />
  <link href="../lib/advanced-datatable/css/demo_table.css" rel="stylesheet" />
  <link rel="stylesheet" href="../lib/advanced-datatable/css/DT_bootstrap.css" />
  <link rel="stylesheet" href="../lib/file-uploader/css/jquery.fileupload.css"/>
  <link rel="stylesheet" href="../lib/file-uploader/css/jquery.fileupload-ui.css"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="../css/style-responsive.css" rel="stylesheet"/>
  <link href="../css/calendario_ajax.css" rel="stylesheet"/>--%>
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../img/favicon2.png" rel="icon"/>
    <link href="../img/icono.png" rel="apple-touch-icon"/>
    <link href="../css/bootstrap.min.css" rel="stylesheet"/>
    <link href="../css/dashboard.css" rel="stylesheet"/>
    <link href="../css/icons.css" rel="stylesheet"/>
    <link href="../css/style.css" rel="stylesheet"/>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
    <div id="div_BrowserWindowName" style="visibility:hidden;">
        <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
    </div>

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Expo Contenedores</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">CONTENEDORES DE EXPORTACION POR NAVE</li>
            </ol>
        </nav>
    </div>   

    <div class="dashboard-container p-4" id="cuerpo" runat="server">

        <div class="form-title ">
            DATOS DE LA NAVE
        </div>

        <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <div >
                            <div class="form-row" >
                                     
                                <div class="form-group col-md-3">
                                    <br />
                                    <div class="d-flex">
                                        &nbsp;
                                        <asp:TextBox ID="TXTMRN" runat="server" class="form-control" MaxLength="16" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')"  placeholder="NAVE REFERENCIA"></asp:TextBox>
                                        &nbsp;
                                        <asp:LinkButton runat="server" ID="BtnBuscar" Text="<span class='fa fa-search' style='font-size:24px'></span>"  OnClientClick="return mostrarloader('1')" OnClick="Btnbuscar_Click" class="btn btn-primary"/>
                                        &nbsp;
                                        <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />
                                    
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div><br /></div>
                    
                        <div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el número de la carga MRN......</div>

			     
                </ContentTemplate>
                    <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
                    </Triggers>
            </asp:UpdatePanel>
    </div>

    <br />

    <div class="dashboard-container p-4" id="Div1" runat="server">

        <div class="row mt">
            <div class="col-sm-12">
                <section class="panel">
                     <div class="form-title ">
                        REPORTE DE CONTENEDORES EXPORTACIÓN
                    </div>
                    <div class="panel-body minimal">
                        <div class="table-inbox-wrap">
                            <asp:UpdatePanel ID="UPPrincipal" runat="server"  UpdateMode="Conditional"  ChildrenAsTriggers="true">
                                <ContentTemplate>

                                    <div id="imagen" style="align-content:center;" runat="server" clientidmode="Static">
                                            <p align="center">
                                        <img src="../images/print_pase.png" width="440" height="450" alt ="" /></p>
                                    </div>
                                    <rsweb:reportviewer runat="server" Width="100%" ID="ReportViewer1" Font-Names="Verdana" 
                                        Font-Size="8pt" InteractiveDeviceInfos="(Colección)"  Height="100%"
                                        WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
                                        <LocalReport ReportPath="reportesexpo\rptContenedoresExpo.rdlc">
                                            <DataSources>
                                                <rsweb:ReportDataSource DataSourceId="odsContenedorExport" 
                                                    Name="dsContenedorExpo" />
                                           
                                            </DataSources>
                                        </LocalReport>
                                    </rsweb:reportviewer>

                                    <asp:ObjectDataSource runat="server" id="odsContenedorExport" 
                                        OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" 
                
                                        TypeName="CSLSite.reportesexpo.dsContenedorExpoTableAdapters.sp_Bil_Rpt_Contenedor_ExpoTableAdapter">
                                        <SelectParameters>
                                            <asp:SessionParameter DefaultValue="00" Name="i_NaveRef" 
                                            SessionField="i_NaveRef" Type="String" />
                                                                   
                                        </SelectParameters>
                                    </asp:ObjectDataSource>


                                </ContentTemplate>
                            </asp:UpdatePanel>


                        </div>
                    </div>
                </section>
            </div>
        </div>

        <asp:UpdatePanel ID="UPBOTONES" runat="server" UpdateMode="Conditional" >  
        <ContentTemplate>
                
        <div class="form-group">
        <div class="alert alert-warning" id="banmsg_det" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el número de la carga MRN......</div>
        </div>
                  
        </ContentTemplate>
        </asp:UpdatePanel>   

    </div>
   

<script type="text/javascript" src="../lib/jquery/jquery.min.js"></script>
<script type="text/javascript" src="../lib/bootstrap/js/bootstrap.min.js"></script>
<script  type="text/javascript" src="../lib/jquery.dcjqaccordion.2.7.js"></script>
<script type="text/javascript" src="../lib/jquery.scrollTo.min.js"></script>
<script type="text/javascript" src="../lib/jquery.nicescroll.js" ></script>
<script type="text/javascript" src="../lib/jquery.sparkline.js"></script>

<!--common script for all pages-->
<script type="text/javascript" src="../lib/common-scripts.js"></script>
<script type="text/javascript" src="../lib/gritter/js/jquery.gritter.js"></script>
<script type="text/javascript" src="../lib/gritter-conf.js"></script>
<!--script for this page-->
 
<script type="text/javascript" src="../lib/pages.js" ></script>
<script type="text/javascript" src="../lib/bootstrap-datepicker/js/bootstrap-datepicker.js"></script> 
<script type="text/javascript" src="../lib/advanced-form-components.js"></script>
<script type="text/javascript" src="../lib/popup_script_cta.js" ></script>
<script type="text/javascript" src='../lib/autocompletar/bootstrap3-typeahead.min.js'></script>
<script type="text/javascript">
    function mostrarloader(Valor) {
        try {
            if (Valor == "1") {
                document.getElementById("ImgCarga").className = 'ver';
            }
            else {
                document.getElementById("ImgCargaDet").className = 'ver';
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
                document.getElementById("ImgCargaDet").className = 'nover';
            }

        } catch (e) {
            alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
        }
    }
</script>

<script type="text/javascript">
    function fVer() {
        document.getElementById('divVer').style.display = 'block';
        return false;
    }
</script>

</asp:Content>
