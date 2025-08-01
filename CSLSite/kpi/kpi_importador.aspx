﻿<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="kpi_importador.aspx.cs" Inherits="CSLSite.kpi_importador" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

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


 <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>
 <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />


 <%-- <link href="../css/datatables.min.css" rel="stylesheet" />--%>
  <link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css"/>

    <script src="../dist/Chart.min.js" type="text/javascript"></script>
	<script src="../utils/utils.js" type="text/javascript"></script>

<%--  <link href="../css/loader.css" rel="stylesheet"/>--%>


</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >

    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

     

     <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
              <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">IMPORTADOR</li>
            <li class="breadcrumb-item" id="opcion_principal" runat="server">Indicadores</li>
            
          </ol>
        </nav>
      </div>

<div class="dashboard-container p-4" id="cuerpo" runat="server">
    <div class="form-title">
            INDICADORES DE GESTIÓN
    </div>
    <asp:HiddenField  runat="server" ID="sruc" Value="0990005087001" ClientIDMode="Static"/>
   

    <p class="font-weight-bold">CRITERIOS DE BÚSQUEDAS</p>
        <div class="form-row">
             <div class="form-group col-md-6"> 
                 <label for="inputEmail4">FECHA DESDE:</label>
                <asp:TextBox ID="sdesde" runat="server"  class="datetimepicker form-control" ClientIDMode="Static" MaxLength="10"  
                    placeholder="MM/dd/yyyy" onkeypress="return soloLetras(event,'1234567890/:')"
                    
                    ></asp:TextBox><span id="fechadesde" class="validacion" ></span>
            </div>
            <div class="form-group col-md-6">  
                    <label for="inputEmail4">FECHA HASTA:</label>
                    <asp:TextBox ID="shasta" runat="server"  class="datetimepicker form-control" ClientIDMode="Static" MaxLength="10"  
                        placeholder="MM/dd/yyyy" onkeypress="return soloLetras(event,'1234567890/:')"
                          
                        ></asp:TextBox><span id="fechahasta" class="validacion" ></span>
                          
            </div>
        </div> 
  
     <div class="form-row">  
           <div class="col-md-12 d-flex justify-content-left"> 
                <input id="btsalvar" type="button" value="Generar Estadística" onclick="return recargar()" class="btn btn-primary" />
                <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover" onclick ="recargar();"   />
           </div>
          <div class="col-md-12 d-flex justify-content-left"> 
               <div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >......</div>
           </div>
      </div>

         <div class="form-row">
             <br/><br/>
         </div>

           <div class="form-row"> 
             <div class="form-group col-md-12"  style="background-color:#FAFAFA" > 
                 <strong><span id="DibujaLinea"></span></strong>
               <canvas id="GraficoLinea" ></canvas>
             </div>
           </div>
        
           <div class="row">
              <div class="form-group col-md-12"  > 
                  <br/>
                  <br/>
              </div>  
           </div>    

            <div class="row">
                <div class="form-group col-md-12" style="background-color:#FAFAFA" > 
                    <strong> <span id="DibujaEmpresaTransporte"></span></strong>
                      <canvas id="GraficoBarempTransporte" ></canvas>
                </div>
           </div> 


           <div class="row">
              <div class="form-group col-md-12"  > 
                  <br/><br/>
              </div>  
           </div>    
    
          <div class="form-row"> 
            <div class="form-group col-md-12" style="background-color:#FAFAFA" > 
                <strong> <span id="DibujaBarras"></span></strong>
                  <canvas id="GraficoPasePuerta"></canvas>
            </div>
          </div>

            <div class="row">
              <div class="form-group col-md-12"  > 
                  <br/><br/>
              </div>  
           </div>    

            <div class="form-row"> 
                <div class="form-group col-md-12" style="background-color:#FAFAFA" > 
                     <strong> <span id="DibujaHorizontalBar"></span></strong>
                      <canvas id="GraficoHorizontalBar" ></canvas>
                </div>
            </div> 
   
 </div>
   

   <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>

   <script type="text/javascript" src="../lib/pages.js" ></script>

   <script type="text/javascript" src="../lib/advanced-form-components.js"></script>


 


   <script type="text/javascript">


        //objeto de esta peticion--> lleno los parametros//
        //si en pantalla hay que redibujar entonces debes enviar la peticion de nuevo, para esto debes crearla
        
         //FUNCION DE PETICION, esto no cambia es para todos
       function GetGrafico(peticion, funcion) {

           params = JSON.stringify(peticion);
            $.ajax({
                type: "POST",
                url: "../services/Graficos.asmx/GraficoData",
                data: params,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: funcion,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ": " + XMLHttpRequest.responseText);
                }
            });
       }


       function GetGraficoMulti(peticion, funcion) {

           params = JSON.stringify(peticion);
              
            $.ajax({
                type: "POST",
                url: "../services/Graficos.asmx/GraficoDataMulti",
                data: params,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: funcion,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ": " + XMLHttpRequest.responseText);
                }
            });
        }
  
        function GraficoDataMultiSerie(peticion, funcion) {
               params = JSON.stringify(peticion);
              
            $.ajax({
                type: "POST",
                url: "../services/Graficos.asmx/GraficoDataMultiSerie",
                data: params,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: funcion,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ": " + XMLHttpRequest.responseText);
                }
            });
       }


        //funcion dibujado del grafico, esto por cada grafico.
        function DibujaBarras(x) {

             if(window.myChartsPase != undefined)
                window.myChartsPase.destroy();

            var nio = JSON.parse(x.d);
            if (nio.error) {
                document.getElementById('DibujaBarras').innerHTML = nio.error;
                return;
            }
            document.getElementById('DibujaBarras').innerHTML = nio.descripcion;

            var ctx = document.getElementById('GraficoPasePuerta');

            window.myChartsPase = new Chart(ctx, {
                    type: nio.tipo,
                    data: {
                        labels: nio.labels,
                        datasets: [{
                            label: nio.label,
                            data: nio.data,
                            backgroundColor: nio.backgroundColor,
                            borderColor: nio.borderColor,
                            borderWidth: 1
                        }]
                    },
                options: {
                         legend: {
						    position: 'right',
					    },
                        scales: {
                            yAxes: [{
                                ticks: {
                                    beginAtZero: true
                                }
                            }]
                        }
                    }
                });

            myChartsPase.update();

        }

        function DibujaHorizontalBar(x) {

           if(window.myChartsVol != undefined)
                window.myChartsVol.destroy();

             var nio = JSON.parse(x.d);
              if (nio.error) {
                document.getElementById('DibujaHorizontalBar').innerHTML = nio.error;
                return;
             }

             document.getElementById('DibujaHorizontalBar').innerHTML = nio.descripcion;
                var ctx = document.getElementById('GraficoHorizontalBar');
                window.myChartsVol = new Chart(ctx, {
                    type: nio.tipo,
                    data: {
                        labels: nio.labels,
                        datasets: [{
                            label: nio.label,
                            data: nio.data,
                            backgroundColor: nio.backgroundColor,
                            borderColor: nio.borderColor,
                            borderWidth: 1
                           
                        }]
                    },
                    options: {
                        responsive: true,
                        legend: {
                            position: 'right',
                        }
                    }
                });

            myChartsVol.update();
        }

        function DibujaLinea(x)
        {
            if(window.myChartsLinea != undefined)
                window.myChartsLinea.destroy();

            var nio = JSON.parse(x.d);
           
            if (nio.error) {
                document.getElementById('DibujaLinea').innerHTML = nio.error;
                return;
            }

            document.getElementById('DibujaLinea').innerHTML = nio.descripcion;

            var data = nio.series;
            var titulos = [];
            for (var i = 0; i < data.length; i++)
            {
                 titulos[i] =nio.series[i].serie;
            }
            var conteo = [];
            for (var i = 0; i < data.length; i++)
            {
                 conteo[i] =nio.series[i].conteo;
            }
            var total = [];
            for (var i = 0; i < data.length; i++)
            {
                 total[i] =nio.series[i].total;
            }

             var lineChartData = {
			labels: titulos,
			datasets: [{
				label: 'Número Facturas',
				borderColor: window.chartColors.red,
				backgroundColor: window.chartColors.red,
				fill: false,
				data: conteo,
				yAxisID: 'y-axis-1',
			}, {
				label: 'Monto USD',
				borderColor: window.chartColors.blue,
				backgroundColor: window.chartColors.blue,
				fill: false,
				data: total,
				yAxisID: 'y-axis-2'
			}]
            };


           var ctx = document.getElementById('GraficoLinea');

           window.myChartsLinea =  new Chart(ctx, {
               type: 'line',
               data: lineChartData,
               options: {
                   responsive: true,
                   hoverMode: 'index',
                   stacked: true,
                   title: {
                       display: true,
                       text: ''
                   },
                   scales: {
                       yAxes: [{
                           type: 'linear', 
                           display: true,
                           position: 'left',
                           id: 'y-axis-1',
                       }, {
                           type: 'linear', 
                           display: true,
                           position: 'right',
                           id: 'y-axis-2',

                           // grid line settings
                           gridLines: {
                               drawOnChartArea: false, 
                           },
                       }],

                   }
               }
           });

            myChartsLinea.update();
       }

        function DibujaBarEmpresaTransporte(x) {

           if(window.myChartsTransp != undefined)
                window.myChartsTransp.destroy();

             var nio = JSON.parse(x.d);
              if (nio.error) {
                document.getElementById('DibujaEmpresaTransporte').innerHTML = nio.error;
                return;
            }

            
            var nSeries = [];

            var colorNames = [window.chartColors.red, window.chartColors.blue, window.chartColors.orange, window.chartColors.green, window.chartColors.yellow,
                window.chartColors.black, window.chartColors.cyan];

            for (var i = 0; i < nio.series.length; i++)
            {

                var newcolor = Chart.helpers.color;
                const  newDataset = {
				label: nio.series[i].grupo_id,
				backgroundColor: newcolor(colorNames[i]).alpha(0.5).rgbString(),
				borderColor: colorNames[i],
				borderWidth: 1,
				data: nio.series[i].grupo_valores
                };
              
                nSeries[i] = newDataset;

            }
           
            document.getElementById('DibujaEmpresaTransporte').innerHTML = nio.descripcion;
            var ctx = document.getElementById('GraficoBarempTransporte');

           
            var color = Chart.helpers.color;
            var barChartData = {
			labels: nio.labels,
			datasets:nSeries

            };

            window.myChartsTransp  = new Chart(ctx, {
                    type: 'bar',
                    data: barChartData,
                    options: {
					    responsive: true,
					    legend: {
						    position: 'right',
					    },
					title: {
						    display: true,
						    text: ''
					    }
                    }
            });

             myChartsTransp.update();


       }

        //inicia el script
      $(document).ready(function ()
      { 
           var fechainicial = document.getElementById('<%=sdesde.ClientID %>').value;
           var fechafinal = document.getElementById('<%=shasta.ClientID %>').value;
           var fecha1 = fechainicial.substr(3, 2) + "/" + fechainicial.substr(0, 2) + "/" + fechainicial.substr(6, 4);//nuevo formato
           var fecha2 = fechafinal.substr(3, 2) + "/" + fechafinal.substr(0, 2) + "/" + fechafinal.substr(6, 4);//nuevo formato

           var peticion1 = {};
           peticion1.id = 1;
           peticion1.ruc = document.getElementById('<%=sruc.ClientID %>').value;
           peticion1.desde = fecha1; 
           peticion1.hasta = fecha2; 
           GetGrafico(peticion1, DibujaBarras);//FLETES CONTENEDORES DE IMPORTACIÓN

           var peticion10 = {};
           peticion10.id = 10;
           peticion10.ruc = document.getElementById('<%=sruc.ClientID %>').value;
           peticion10.desde = fecha1; 
           peticion10.hasta = fecha2; 
           GetGrafico(peticion10, DibujaHorizontalBar);//VOLUMEN DE CONTENEDORES DE IMPORTACIÓN


           var peticion2 = {};
           peticion2.id = 2;
           peticion2.ruc = document.getElementById('<%=sruc.ClientID %>').value;
           peticion2.desde = fecha1; 
           peticion2.hasta = fecha2; 
           GetGraficoMulti(peticion2, DibujaBarEmpresaTransporte);//TOP 5 EMPRESAS TRANSPORTE: FLETES CONTENEDORES DE IMPORTACIÓN

           var peticion3 = {};
           peticion3.id = 3;
           peticion3.ruc = document.getElementById('<%=sruc.ClientID %>').value;
           peticion3.desde = fecha1; 
           peticion3.hasta = fecha2; 

           GraficoDataMultiSerie(peticion3, DibujaLinea);//FACTURACIÓN DE CONTENEDORES DE IMPORTACIÓN

          
           
       });

       function recargar()
       {
           try
           {
                var vals = document.getElementById('<%=sdesde.ClientID %>').value;
               if (vals == '' || vals == null || vals == undefined) {
                     alertify.alert('* Escriba la Fecha de inicio de búsqueda *').set('label', 'Aceptar');
                    document.getElementById('<%=sdesde.ClientID %>').focus();
                    document.getElementById("ImgCarga").className = 'nover';
                    return false;
               }
               var vals = document.getElementById('<%=shasta.ClientID %>').value;
               if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* Escriba la Fecha funal de búsqueda *').set('label', 'Aceptar');
                    document.getElementById('<%=sdesde.ClientID %>').focus();
                    document.getElementById("ImgCarga").className = 'nover';
                    return false;
               }

                var fechainicial = document.getElementById('<%=sdesde.ClientID %>').value;
                var fechafinal = document.getElementById('<%=shasta.ClientID %>').value;

               var fecha1 = fechainicial.substr(3, 2) + "/" + fechainicial.substr(0, 2) + "/" + fechainicial.substr(6, 4);//nuevo formato
               var fecha2 = fechafinal.substr(3, 2) + "/" + fechafinal.substr(0, 2) + "/" + fechafinal.substr(6, 4);//nuevo formato

               if(Date.parse(fecha1) > Date.parse(fecha2)) {

                   alertify.alert("La fecha inicial no debe ser mayor a la fecha final").set('label', 'Aceptar');
                   return false;
               }

               var fechaIni = new Date(fecha1);
               var fechaFin = new Date(fecha2);
               var diff = fechaFin - fechaIni;
               diferenciaDias = Math.floor(diff / (1000 * 60 * 60 * 24));

               
               if(diferenciaDias > 365) {

                   alertify.alert("No se puede realizar consultas mayores 365 días").set('label', 'Aceptar');
                   return false;
               }

               var peticion1 = {};
               peticion1.id = 1;
               peticion1.ruc = document.getElementById('<%=sruc.ClientID %>').value;
               peticion1.desde = fecha1; 
               peticion1.hasta = fecha2; 
               GetGrafico(peticion1, DibujaBarras);//FLETES CONTENEDORES DE IMPORTACIÓN

               var peticion10 = {};
               peticion10.id = 10;
               peticion10.ruc = document.getElementById('<%=sruc.ClientID %>').value;
               peticion10.desde = fecha1; 
               peticion10.hasta = fecha2; 
               GetGrafico(peticion10, DibujaHorizontalBar);//VOLUMEN DE CONTENEDORES DE IMPORTACIÓN


               var peticion2 = {};
               peticion2.id = 2;
               peticion2.ruc = document.getElementById('<%=sruc.ClientID %>').value;
               peticion2.desde = fecha1; 
               peticion2.hasta = fecha2; 
               GetGraficoMulti(peticion2, DibujaBarEmpresaTransporte);//TOP 5 EMPRESAS TRANSPORTE: FLETES CONTENEDORES DE IMPORTACIÓN

               var peticion3 = {};
               peticion3.id = 3;
               peticion3.ruc = document.getElementById('<%=sruc.ClientID %>').value;
               peticion3.desde = fecha1; 
               peticion3.hasta = fecha2; 

               GraficoDataMultiSerie(peticion3, DibujaLinea);//FACTURACIÓN DE CONTENEDORES DE IMPORTACIÓN
            

               

           }
           catch (e)
           {
  
               alertify.alert('Error al consultar').set('label', 'Aceptar');
           }
       }

  
    
    function mostrarloader() {

        try {
            
                document.getElementById("ImgCarga").className = 'ver';
            
            
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }


    function ocultarloader(Valor) {
        try {

           
                document.getElementById("ImgCarga").className = 'nover';
           
            
             } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }



            $(document).ready(function () {
                  $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
              });    
  </script>




 
</asp:Content>