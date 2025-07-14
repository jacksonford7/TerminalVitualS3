<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="facturacion_sav_cliente.aspx.cs" Inherits="CSLSite.facturacion_sav_cliente" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

  <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
 <!-- Bootstrap core CSS -->
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>

 

    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />


   <!--external css-->
  <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
  <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>


 <link href="../lib/advanced-datatable/css/demo_page.css" rel="stylesheet" />
  <link href="../lib/advanced-datatable/css/demo_table.css" rel="stylesheet" />
  <link rel="stylesheet" href="../lib/advanced-datatable/css/DT_bootstrap2.css" />





<script type="text/javascript">
$(document).ready(function(){
  $('[data-toggle="tooltip"]').tooltip();   
});
</script>


<script type="text/javascript">


     function BindFunctions()
       {
       $(document).ready(function() {    
        $('#tablePagination').DataTable({        
            language: {
                    "lengthMenu": "_MENU_ REGISTROS POR PAGINA",
                    "zeroRecords": "No se encontraron resultados",
                    "info": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                    "infoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "infoFiltered": "(filtrado de un total de _MAX_ registros)",
                    "sSearch": "Buscar:",
			         "sProcessing":"Procesando...",
                },
            //para usar los botones   
            responsive: "true",
            dom: 'Bfrtilp',    
            buttons: [  
                {
				    extend:    'excel',
				    text:      '<i class="fa fa-file-excel-o"></i> ',
				    titleAttr: 'Exportar a Excel',
				    className: 'btn btn-primary'
			    },
			    {
				    extend:    'pdf',
				    text:      '<i class="fa fa-file-pdf-o"></i> ',
				    titleAttr: 'Exportar a PDF',
				    className: 'btn btn-primary'
			    },
			    {
				    extend:    'print',
				    text:      '<i class="fa fa-print"></i> ',
				    titleAttr: 'Imprimir',
				    className: 'btn btn-primary'
			    },
            ]	 
        });     
    });

    }


</script>


    


    

<script type="text/javascript">

    function BindFunctionsError()
    {

        $(document).ready(function () {
            $('#<%=PaginationErrores.ClientID%>').DataTable({
                language: {
                    "lengthMenu": "_MENU_ REGISTROS POR PAGINA",
                    "zeroRecords": "No se encontraron resultados",
                    "info": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                    "infoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "infoFiltered": "(filtrado de un total de _MAX_ registros)",
                    "sSearch": "Buscar:",
                    "sProcessing": "Procesando...",
                },

            });


        });
    
    }


</script>


</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

  <div id="div_BrowserWindowName" style="visibility:hidden;">
    <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
     
  </div>
    <asp:HiddenField ID="manualHide" runat="server" />
      <input id="ID_FILA" type="hidden" value="" runat="server" clientidmode="Static" />

      <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">DEVOLUCIÓN DE CONTENEDORES</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">FACTURACIÓN EMITIDA POR EL CLIENTE</li>
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
                <asp:TextBox ID="TxtFechaDesde" runat="server"  class="datetimepicker form-control"  MaxLength="10"  placeholder="dd/MM/yyyy" onkeypress="return soloLetras(event,'1234567890/:')"></asp:TextBox>
        </div>
        <div class="form-group col-md-6">  
                <label for="inputEmail4">FECHA HASTA:</label>
                <asp:TextBox ID="TxtFechaHasta" runat="server"  class="datetimepicker form-control" MaxLength="10"  placeholder="dd/MM/yyyy" onkeypress="return soloLetras(event,'1234567890/:')"></asp:TextBox>
                          
        </div>
     </div> 

       <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional">
       <ContentTemplate>
       
		 <div class="row">
                <div class="col-md-12 d-flex justify-content-center">
                        <asp:Button ID="BtnBuscar" runat="server" class="btn btn-primary"   Text="BUSCAR"  OnClientClick="return mostrarloader('1')" OnClick="BtnBuscar_Click" />                             
                       
                </div>    
            </div>    
           <br/>
        <div class="row">
            <div class="col-md-12 d-flex justify-content-center">
                 <div class="alert alert-danger" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >......</div>
                <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />
            </div>
         </div>				
                
        </ContentTemplate>
        </asp:UpdatePanel>
                        
     
      
   
    <div class="form-row">  

      
         <asp:UpdatePanel ID="UPBUSCACOLABORADOR" runat="server" UpdateMode="Conditional" >  
                                <ContentTemplate>
                                     <div class="d-flex">
                                         <asp:TextBox ID="txtFiltro" runat="server" class="form-control" MaxLength="100" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" placeholder="FILTRO"></asp:TextBox>
						                 &nbsp;
                                        <asp:LinkButton runat="server" ID="BtnFiltrarGrid" Text="<span class='fa fa-search' style='font-size:24px'></span>"     OnClick="BtnFiltrarGrid_Click"  class="btn btn-primary" />
                                
                                         

                                     </div>
                                     <div class="d-flex">
                                           <label class="checkbox-container">
                                                &nbsp;&nbsp;<asp:CheckBox ID="ChkTodos" runat="server"  Text="Seleccionar Todos"  OnCheckedChanged="ChkTodos_CheckedChanged"  Checked="true"   AutoPostBack="True" />
                                                <span class="checkmark"></span>
                                                </label>
                                    </div>
                                </ContentTemplate>
                                </asp:UpdatePanel>

         <br />
         <br />
        <div class="form-group col-md-12">
     <asp:UpdatePanel ID="UPDETALLE" runat="server" UpdateMode="Conditional" >  
       <ContentTemplate>
         
              
          
           <h4 id="LabelTotal" runat="server" class="mb">PASE PUERTA</h4>
 
             <asp:Repeater ID="tablePagination" runat="server" onitemcommand="tablePagination_ItemCommand" onitemdatabound="tablePagination_ItemDataBound">
                       <HeaderTemplate>
                       <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered table-sm table-contecon" id="tablePagination">
                           <thead>
                          <tr>
                            
                            <th class="center hidden-phone">#</th>
                            <th class="center hidden-phone"># FACTURA</th>
                            <th class="center hidden-phone">T/FECHA</th>
                            <th class="center hidden-phone">T/HORA</th>
                            <th class="center hidden-phone">RUC ASUME</th>
                            <th class="center hidden-phone">EXPORTADOR ASUME</th>
                            <th class="center hidden-phone">CONTENEDOR</th>
                            <th class="center hidden-phone">BOOKING</th>
                            <th class="center hidden-phone">LINEA</th>
                            <th class="center hidden-phone">ESTADO</th>
                            <th class="center hidden-phone">FACT</th>
                            <th class="center hidden-phone">IMPRIMIR</th>
                          </tr>
                        </thead>
                        <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                           <tr class="gradeC"> 
                                <td class="center hidden-phone"><span class="fila"><asp:Label Text='<%#Eval("fila")%>' ID="Lblfila" runat="server"  /></span></td>
                                <td class="center hidden-phone"><span class="numero_factura"><asp:Label Text='<%#Eval("numero_factura")%>' ID="LblFactura" runat="server"  /></span></td>
                                <td class="center hidden-phone"><span class="turno_fecha"><asp:Label Text='<%# Eval("turno_fecha", "{0: dd/MM/yyyy}")%>' ID="LblFechaTurno" runat="server"  /></span></td>
                                <td class="center hidden-phone"><span class="turno_hora"><asp:Label Text='<%#Eval("turno_hora")%>' ID="LblHoraTurno" runat="server"  /></span></td>
                                <td class="center hidden-phone"><span class="asume_ruc_facturar"><asp:Label Text='<%#Eval("asume_ruc_facturar")%>' ID="Lblruc" runat="server"  /></span></td>                                
                                <td class="center hidden-phone"><span class="asume_cliente_facturar"><asp:Label Text='<%#Eval("asume_cliente_facturar")%>' ID="LblExportador" runat="server"  /></span></td>
                                <td class="center hidden-phone"><span class="unidad_id"><asp:Label Text='<%#Eval("unidad_id")%>' ID="LblUnidad" runat="server"  /></span></td>
                                <td class="center hidden-phone"><span class="unidad_booking"><asp:Label Text='<%#Eval("unidad_booking")%>' ID="LblBooking" runat="server"  /></span></td>
                                <td class="center hidden-phone"><span class="unidad_linea"><asp:Label Text='<%#Eval("unidad_linea")%>' ID="LblLinea" runat="server"  /></span></td>
                                <td class="center hidden-phone"><span class="estado_pago"><asp:Label Text='<%#Eval("estado_pago")%>' ID="LblPago" runat="server"  /></span></td>
                             
                                <td class="center hidden-phone">  
                                
                                    <asp:UpdatePanel ID="UPSELECCIONAR" runat="server" ChildrenAsTriggers="true">
                                        <ContentTemplate>
                                             <label class="checkbox-container">
                                            <asp:CheckBox id="chkPase" runat="server"  Checked='<%#Eval("visto")%>' AutoPostBack="True" OnCheckedChanged="chkPase_CheckedChanged" />
                                             <span class="checkmark"></span>
                                             </label>
                                        </ContentTemplate>
                                         <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="chkPase" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                </td> 
                                  <td class="center hidden-phone">  
                                      
                                     <asp:Button ID="BtnImprimir" CommandArgument= '<%#Eval("Fila")%>' runat="server" Text="Imprimir"  OnClientClick="Imprimir(this);"
                                        class="btn btn-primary"  />
                                  </td> 
                               
                           </tr>    

                       </ItemTemplate>
                       <FooterTemplate>
                        </tbody>
                      </table>
                     </FooterTemplate>
                    </asp:Repeater>

          
                  
 
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

                   <asp:Button ID="BtnCotizar" runat="server" class="btn btn-outline-primary mr-4" Text="GENERAR PROFORMA" OnClientClick="return mostrarloader('2')" OnClick="BtnCotizar_Click" Visible="false"/>

                   <asp:Button ID="BtnFacturar" runat="server" class="btn btn-primary" Text="FACTURAS INDIVIDUAL"  OnClientClick="return confirmacion();"  OnClick="BtnFacturar_Click" />
                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="BtnFacturaAgrupada" runat="server" class="btn btn-primary" Text="FACTURAS AGRUPADAS"  OnClientClick="return confirmacion_grupo();"  OnClick="BtnFacturaAgrupada_Click" />

                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="BtnErrores" runat="server" class="btn btn-primary" Text="DETALLE ERRORES"    data-toggle="modal" 
                                               data-target="#exampleModalToggleError"  />

                   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="BtnExcel" runat="server" class="btn btn-primary" Text="EXCEL"   OnClientClick="exportar();" />

               </div> 
             </div>
            </ContentTemplate>
             </asp:UpdatePanel>   
   

       
    
</div>

 


 <!--errores-->
 <div class="modal fade" id="exampleModalToggleError" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog" role="document" style="max-width: 1350px">
         <div class="modal-content">
              <div class="dashboard-container p-4" id="Div2" runat="server">  
                   <div class="modal-header">
                       <asp:UpdatePanel ID="UPTITULOERROR" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                       <ContentTemplate>
                                    <h5 class="modal-title" id="H1" runat="server">Detalle de contenedores con errores</h5>
                           </ContentTemplate>
                        </asp:UpdatePanel>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                   <div class="modal-body">
                        <asp:UpdatePanel ID="UPDETALLEERROR" runat="server" UpdateMode="Conditional" > 
                        <ContentTemplate>
                           
                          <script type="text/javascript">
                                Sys.Application.add_load(BindFunctionsError); 
                         </script>
                              
                             <div class="form-row">
                                 <div class="form-group col-md-12">
                                   <asp:Repeater ID="PaginationErrores" runat="server" >
                                   <HeaderTemplate>
                                   <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered table-sm table-contecon" id="PaginationErrores">
                                       <thead>
                                      <tr>
                                        <th class="center hidden-phone">#</th>                          
                                        <th class="center hidden-phone">CONTENEDOR</th>
                                        <th class="center hidden-phone">CLIENTE</th>
                                        <th style="width:150px">MENSAJE</th>
                                      </tr>
                                    </thead>
                                    <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr class="gradeC"> 
                                            <td class="center hidden-phone"><asp:Label Text='<%#Eval("fila")%>' ID="Lblfila" runat="server"  /></td>
                                            <td class="center hidden-phone"><asp:Label Text='<%#Eval("contenedor")%>' ID="LblContenedor" runat="server"  /> </td>
                                            <td class="center hidden-phone"><asp:Label Text='<%#Eval("cliente")%>' ID="LblCliente" runat="server"  /></td>
                                            <th style="width:150px"><%#Eval("error")%></td>
                                           
                                         </tr>    
                                   </ItemTemplate>
                                   <FooterTemplate>
                                    </tbody>
                                  </table>
                                 </FooterTemplate>
                                </asp:Repeater>
                                 </div>
                             </div>

                             

                         </ContentTemplate>
                          
                        </asp:UpdatePanel>
                    </div>
                   <div class="modal-footer">
                   <asp:UpdatePanel ID="UPPIEERROR" runat="server" UpdateMode="Conditional" >  
                         <ContentTemplate>
                             <br/>
                              <div class="row">
                                 <div class="col-md-12 d-flex justify-content-center">
 
                                     &nbsp;&nbsp;
                                       
                                       &nbsp;&nbsp;
                                     <button type="button" class="btn btn-outline-primary " data-dismiss="modal">Cerrar</button>
                              </div>
                            </div>
                        </ContentTemplate>
                         </asp:UpdatePanel>   
            </div>

              </div> 
        </div>
    </div>
</div>

 
   <script type="text/javascript" src="../lib/common-scripts.js"></script>
 
   <script type="text/javascript" src="../lib/pages.js" ></script>

   <script type="text/javascript" src="../lib/advanced-form-components.js"></script>


<script type="text/javascript">

    function exportar()
    {
        var fechaDesde = document.getElementById('<%= TxtFechaDesde.ClientID %>').value;

        var fechaHasta = document.getElementById('<%= TxtFechaHasta.ClientID %>').value;
    
        const table = document.getElementById("tablePagination");
        const rows = table.querySelectorAll("tbody tr");
        const data = [];

        rows.forEach(row => {

            const fila = row.querySelector(".fila").innerText;
            const numero_factura = row.querySelector(".numero_factura").innerText;  
            const turno_fecha = row.querySelector(".turno_fecha").innerText;
            const turno_hora = row.querySelector(".turno_hora").innerText;
            const asume_ruc_facturar = row.querySelector(".asume_ruc_facturar").innerText;
            const asume_cliente_facturar = row.querySelector(".asume_cliente_facturar").innerText;
            const contenedor = row.querySelector(".unidad_id").innerText;
            const booking = row.querySelector(".unidad_booking").innerText;
            const linea = row.querySelector(".unidad_linea").innerText;
            const estado_pago = row.querySelector(".estado_pago").innerText;

            // Crear objeto de datos para esta fila
            const rowData = {
                fila: fila,
                numero_factura: numero_factura,
                turno_fecha: turno_fecha,
                turno_hora: turno_hora,
                asume_ruc_facturar: asume_ruc_facturar,
                asume_cliente_facturar: asume_cliente_facturar,
                contenedor: contenedor,
                booking: booking,
                linea: linea,
                estado_pago: estado_pago,
            };

            // Agregar el objeto de la fila al array de datos
            data.push(rowData);
        });

        const json = JSON.stringify(data);
        console.log(json);


            var xhr = new XMLHttpRequest();
            xhr.open('POST', 'facturacion_sav_cliente.aspx/ExportarExcel', true);
            xhr.setRequestHeader('Content-Type', 'application/json; charset=utf-8');
            xhr.responseType = 'blob';

            xhr.onload = function () {
                var fileName = "Reporte_" + fechaDesde + ".xlsx";
                if (xhr.status === 200) {
                    var blob = xhr.response;

                    // Configura la respuesta HTTP para la descarga del archivo
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

            xhr.send(JSON.stringify({ fechaDesde: fechaDesde, fechaHasta: fechaHasta, repeater: json }));
        }
    </script>
 

  <script type="text/javascript">

    function confirmacion()
    {
        var mensaje;
        var opcion = confirm("Esta seguro que desea generar la factura individual?");
        if (opcion == true)
        {
            document.getElementById("ImgCargaDet").className = 'ver';
            return true;
        } else
        {
            //loader();
	         return false;
        }

       
    }


    function confirmacion_grupo()
    {
        var mensaje;
        var opcion = confirm("Esta seguro que desea generar la factura agrupada por cliente?");
        if (opcion == true)
        {
            document.getElementById("ImgCargaDet").className = 'ver';
            return true;
        } else
        {
            //loader();
	         return false;
        }

       
    }

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

     function initFinder() {
          if (document.getElementById('txtfinder').value.trim().length <= 0) {
               alertify.alert('Advertencia','Escriba una o varias letras para iniciar la búsqueda').set('label', 'Aceptar');
              return false;
          }
          document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif">';
     }

    function CierraPopup()
    {
        $("#exampleModalToggle").modal('hide');//ocultamos el modal
        $('body').removeClass('modal-open');//eliminamos la clase del body para poder hacer scroll
        $('.modal-backdrop').remove();//eliminamos el backdrop del modal
     }


      
  </script>
   
    <script type="text/javascript">

           function Imprimir(btn)
           {
               var row = btn.closest("tr"); // Obtener la fila padre del botón
               var factura = row.cells[1].textContent;
               var link = "../sav/facturacion_sav_print.aspx?id_comprobante=" + factura;

               window.open(link, 'name', 'width=850,height=480');
 
           }

      </script>

  <script type="text/javascript">
            $(document).ready(function () {
                 $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'm/d/Y' });
              });    
   </script>


     
</asp:Content>