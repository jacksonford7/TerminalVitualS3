<%@ Page Title="Consultar AISV" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="consulta.aspx.cs" Inherits="CSLSite.consulta" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">


  <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
  
   <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>
 
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
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>

     <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Exportación</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Consulta, reimpresión y anulación de AISV</li>
          </ol>
        </nav>
      </div>
 
    <div class="dashboard-container p-4" id="cuerpo" runat="server">
      
        <div class="form-title">
           Datos del documento buscado
        </div>

      <div class="form-row">
          <div class="form-group col-md-6"> 
              <label for="inputAddress">AISV No. :<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:TextBox ID="aisvn" runat="server"  MaxLength="15" class="form-control"
             onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
          </div>
          <div class="form-group col-md-6"> 
            <label for="inputAddress">Contenedor:<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:TextBox ID="cntrn" runat="server"  MaxLength="15"  class="form-control"             
                  onkeypress="return soloLetras(event,'01234567890abcdefghijklmnopqrstuvwxyz',true)"></asp:TextBox>
                 <span id="valran" class="opcional"></span>
          </div>
          <div class="form-group col-md-6"> 
           <label for="inputAddress">Documento No. (DAE,DAS etc):<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:TextBox ID="docnum" runat="server"  MaxLength="15" class="form-control"     
                   onkeypress="return soloLetras(event,'01234567890abcdefghijklmnopqrstuvwxyz',true)"></asp:TextBox>
          </div>
          <div class="form-group col-md-6"> 
           <label for="inputAddress">Booking No.:<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:TextBox ID="booking" runat="server" ClientIDMode="Static" MaxLength="15" class="form-control"   
             onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890-_')" 
                 ></asp:TextBox>
              <span id="valdae" class="opcional"></span>
          </div>
          <div class="form-group col-md-6"> 
           <label for="inputAddress">Generados desde el día:<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:TextBox ID="desded" runat="server"  CssClass="datetimepicker form-control" 
             onkeypress="return soloLetras(event,'01234567890/')" 
                 onblur="valDate(this,true,valdate);" ClientIDMode="Static"></asp:TextBox>
          </div>
          <div class="form-group col-md-6"> 
           <label for="inputAddress">Hasta:<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:TextBox ID="hastad" runat="server" ClientIDMode="Static"  MaxLength="15" CssClass="datetimepicker form-control" 
             onkeypress="return soloLetras(event,'01234567890/')" 
                  onblur="valDate(this,true,valdate);"></asp:TextBox>
               <span id="valdate" class="validacion"></span>
          </div>
    </div>
      <div class="row">
            <div class="col-md-12 d-flex justify-content-center">
                 <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda"  
                     class="btn btn-primary" 
                     onclick="btbuscar_Click" OnClientClick="return validateDatesRange('desded','hastad','imagen');" />
                 <span id="imagen"></span>
            
         </div>
             
            </div>

             <div class="cataresult" >
               <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                        <script type="text/javascript">
                            Sys.Application.add_load(BindFunctions); 
                      </script>
             <div id="xfinder" runat="server" visible="false"  >
             <div class="alert alert-warning" id="alerta" runat="server" >
               Confirme que los datos sean correctos. En caso de error, favor comuníquese 
               con el Departamento de Planificación a los teléfonos: +593 
               (04) 6006300, 3901700 
             </div>
             <div >
             <div >
                  <div >Documentos encontrados</div>
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server" 
                         onitemcommand="tablePagination_ItemCommand" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1"  class="table table-bordered table-sm table-contecon"   >
                 <thead>
                 <tr>
                 <th scope="col">#</th>
                 <th scope="col">AISV #</th>
                 <th scope="col">Tipo</th>
                 <th scope="col">Booking</th>
                 <th scope="col">Registrado</th>
                 <th scope="col">Carga</th>
                 <th scope="col">Estado</th>
                 <th scope="col" >Ref. Apoyo</th>
                 <th>Acciones</th>
                 <th>Archivos</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td scope="row"><%#Eval("item")%></td>
                  <td scope="row"><%#Eval("aisv")%></td>
                  <td scope="row"><%# tipos(Eval("tipo"), Eval("movi"))%></td>
                  <td scope="row">
                    <a class="xinfo" >
                    <span class="xclass">
                    <h3>Referencia:</h3> <%#Eval("referencia")%>
                    <h3>FreightKind:</h3> <%#Eval("fk")%>
                    <h3>Puerto descarga:</h3> <%#Eval("pod")%>
                    <h3>Agencia:</h3> <%#Eval("agencia")%>
                    </span>
                        <%#Eval("boking")%>
                    </a>
                  </td>
                  <td scope="row"><%#Eval("fecha")%></td>
                  <td scope="row"> 
                        <%#Eval("tool")%>
                  </td>
                  
                  <td  scope="row"><%# anulado(Eval("estado"))%></td>
                     <td scope="row" >
                        <a href="editrefdata.aspx?sid=<%# securetext(Eval("aisv")) %>" target="_blank" onclick="popUpCal(this.href);  return false; "><%# refrigeracion(Eval("apoyo"), Eval("temperatura"))%></a>
                  </td>

                  <td scope="row">
                   <div class="tcomand" >
                       <a href="impresion.aspx?sid=<%# securetext(Eval("aisv")) %>"  target="_blank" class=" btn btn-link">Imprimir</a> | 
                       <div class='<%# boton( Eval("estado"))%>' >
                       <asp:Button ID="btanula"  
                       OnClientClick="return confirm('Esta seguro que desea eliminar este documento?');" 
                       CommandArgument=   '<%# jsarguments( Eval("aisv"),Eval("referencia"),Eval("cntr"),Eval("tipo"),Eval("movi"),Eval("estado") )%>' 
                           class="btn  btn-secondary" runat="server" Text="Anular" ToolTip="Permite anular este documento" />
                       </div>
                   </div>
                  </td>
                   <td scope="row"> 
                       <%#Eval("archivo")%>  
                  </td>
                 </tr>
                  </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>
           
                </div>
             </div>
             </div>
        
              </div>
               <div id="sinresultado" runat="server" class="  alert alert-primary"></div>
                  </ContentTemplate>
                     <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                     </Triggers>
                 </asp:UpdatePanel>
             </div>
     

     
  </div>

    
    <script src="../Scripts/pages.js" type="text/javascript"></script>
     <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' }); 
                            //$('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'd/m/Y H:i' });
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
   
    
    <script src="../Scripts/refrigerados.js" type="text/javascript"></script>
  </asp:Content>
