<%@ Page Title="Consulta y Reimpresion" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="consulta.aspx.cs" Inherits="CSLSite.atraque.consulta" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
  <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

  <link href="../shared/estilo/Reset.css" rel="stylesheet" />
  <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />

     <!--Datatables-->
       <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />
       <link href="../css/datatables.min.css" rel="stylesheet" />
       <link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css"/>
        <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>
       <script type="text/javascript"  src="../js/datatables.js"></script>
       <script src="../Scripts/table_catalog.js" type="text/javascript"></script>  
       <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
    <!--Fin-->



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
    <input id="zonaid" type="hidden" value="10" />
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
  
    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Mis Naves</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Consulta y reimpresión de solicitudes</li>
            </ol>
        </nav>
    </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">
      
        <div class="form-title">
           Datos del documento buscado
        </div>

        <div class="form-row">

            <div class="form-group col-md-6"> 
                <label for="inputAddress">Referencia<span style="color: #FF0000; font-weight: bold;"></span></label>
                <asp:TextBox  class="form-control"  ID="treferencia" runat="server" MaxLength="10" onkeypress="return soloLetras(event,'01234567890abcdefghijklmnopqrstuvwxyz',true)"></asp:TextBox>
            </div>

            <div class="form-group col-md-6"> 
                <label for="inputAddress">IMO<span style="color: #FF0000; font-weight: bold;"></span></label>
                <asp:TextBox  class="form-control"  ID="timo" runat="server" MaxLength="10" onkeypress="return soloLetras(event,'01234567890abcdefghijklmnopqrstuvwxyz -_')"></asp:TextBox>
                <span id="valran" class="opcional"> </span>
            </div>

            <div class="form-group col-md-6"> 
                <label for="inputAddress">Nave<span style="color: #FF0000; font-weight: bold;"></span></label>
                <asp:TextBox  class="form-control"  ID="tnave" runat="server" MaxLength="50" onkeypress="return soloLetras(event,'01234567890abcdefghijklmnopqrstuvwxyz_/ ')"></asp:TextBox>
            </div>

            <div class="form-group col-md-6"> 
                <label for="inputAddress">Manifiesto<span style="color: #FF0000; font-weight: bold;"></span></label>
                <asp:TextBox  class="form-control"  ID="tmani" runat="server" ClientIDMode="Static" MaxLength="16" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890-_')"></asp:TextBox>
                <span id="valdae" class="opcional"></span>
            </div>
          
            <div class="form-group col-md-6"> 
                <label for="inputAddress">Desde<span style="color: #FF0000; font-weight: bold;"></span></label>
                <asp:TextBox ID="desded" runat="server"  CssClass="datetimepicker form-control" onkeypress="return soloLetras(event,'01234567890/')" onblur="valDate(this,true,valdate);" ClientIDMode="Static"></asp:TextBox>
            </div>

            <div class="form-group col-md-6"> 
                <label for="inputAddress">Hasta<span style="color: #FF0000; font-weight: bold;"></span></label>
               <div class="d-flex">
                <asp:TextBox ID="hastad" runat="server" ClientIDMode="Static"  MaxLength="15" CssClass="datetimepicker form-control" onkeypress="return soloLetras(event,'01234567890/')" onblur="valDate(this,true,valdate);"></asp:TextBox>
                <span id="valdate" class="validacion"></span>
                   </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12 d-flex justify-content-center">
                <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda" 
                    class="btn btn-primary" 
                    onclick="btbuscar_Click" 
                    OnClientClick="return validateDatesRange('desded','hastad','imagen');" />
                <span id="imagen"></span>
            </div>
        </div>
        
        <div class="form-row">
            <div class="form-group col-md-12"> 
                <div class="cataresult" >
                    <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <script type="text/javascript">
                                Sys.Application.add_load(BindFunctions); 
                            </script>
                            <div id="xfinder" runat="server" visible="false" >
                                    <div ><br /></div>
                                    <div class="alert alert-warning" id="alerta" runat="server" >
                                    Confirme que los datos sean correctos. En caso de error, favor comuníquese 
                                    con el Departamento de Planificación a los teléfonos: +593 
                                    (04) 6006300, 3901700 
                                    </div>
                    
                                        <div class="form-title" >Documentos encontrados</div>
                                                <asp:Repeater ID="tablePagination" runat="server"  >
                                                    <HeaderTemplate>
                                                    <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered table-sm table-contecon">
                                                        <thead>
                                                        <tr>
                                                        <th>#</th>
                                                        <th>Referencia</th>
                                                        <th>Nave</th>
                                                        <th>Viaje In</th>
                                                        <th>Viaje Out</th>
                                                        <th>Registrada</th>
                                                        <th>Acciones</th>
                                                        </tr>
                                                        </thead> 
                                                        <tbody>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr class="point" >
                                                            <td><%#Eval("item")%></td>
                                                            <td><%#Eval("referencia")%></td>
                                                            <td><%#Eval("nave")%></td>
                                                            <td><%#Eval("viajeIn")%></td>
                                                            <td><%#Eval("ViajeOut")%></td>
                                                            <td><%#Eval("fecha")%></td>
                                                            <td>
                                                            <div class="tcomand">
                                                                <a href="../atraque/printer.aspx?sid=<%# securetext(Eval("referencia")) %>" class=" btn btn-link" target="_blank">Imprimir</a>
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
                        <div id="sinresultado" runat="server" class=" alert  alert-primary"></div>
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
     <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    <script type="text/javascript">
                        $(document).ready(function () {
                            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
                        });
  </script>
  



  </asp:Content>
