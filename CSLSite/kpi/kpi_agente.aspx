<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="kpi_agente.aspx.cs" Inherits="CSLSite.kpi_agente" %>
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
           
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">AGENTE</li>
              <li class="breadcrumb-item" id="opcion_principal" runat="server" >Indicadores</li>
          </ol>
        </nav>
      </div>

<div class="dashboard-container p-4" id="cuerpo" runat="server">
     
    <div class="form-title" id="titulo" runat="server" clientidmode="Static">
            INDICADORES DE GESTIÓN
    </div>
    <asp:HiddenField  runat="server" ID="sruc" Value="0990005087001" ClientIDMode="Static"/>
     <asp:HiddenField  runat="server" ID="idagente" Value="0" ClientIDMode="Static"/>
     <%--<asp:HiddenField  runat="server" ID="sdesde" Value="2020-01-01" ClientIDMode="Static"/>
     <asp:HiddenField  runat="server" ID="shasta" Value="2020-12-31" ClientIDMode="Static"/>--%>

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
              <br/><br/>
          </div>  
       </div>    
        <div class="form-row"> 
              <div class="form-group col-md-12" style="background-color:#FAFAFA" > 
                  <strong><span id="DibujaHorizontalBarclienteMonto"></span></strong>
                  <canvas id="GraficoClienteMonto" ></canvas>
            </div>
        </div> 
    
        <div class="row">
          <div class="form-group col-md-12"  > 
              <br/><br/>
          </div>  
       </div>    
        
       <div class="form-row">  
            <div class="form-group col-md-12"  style="background-color:#FAFAFA" >
                 <strong><span id="DibujaHorizontalBarcliente"></span></strong>
                  <canvas id="GraficoCliente" ></canvas>
            </div>   
        </div>
       
      <div class="row">
          <div class="form-group col-md-12"  > 
              <br/><br/>
          </div>  
       </div>    
        
        <div class="form-row">  
            <div class="form-group col-md-12"  style="background-color:#FAFAFA" >
                 <strong><span id="DibujaHorizontalBar"></span></strong>
                  <canvas id="GraficoTransporte" ></canvas>
            </div>  
        </div>  

       <div class="row">
          <div class="form-group col-md-12"  > 
              <br/><br/>
          </div>  
       </div>    

         <div class="form-row">  
              <div class="form-group col-md-12" style="background-color:#FAFAFA" > 
                  <strong><span id="DibujaHorizontalBar2"></span></strong>
                  <canvas id="GraficoPasePuerta" ></canvas>
             </div>
         </div>
        <div class="row">
          <div class="form-group col-md-12"  > 
              <br/><br/>
          </div>  
       </div>    
        <div class="form-row">  
             <div class="form-group col-md-12" style="background-color:#FAFAFA" > 
               <strong> <span id="DibujaBarVolumen"></span></strong>
                  <canvas id="GraficoBarVolumen" ></canvas>
            </div>
        </div> 
        
          <div class="row">
               <div class="form-group col-md-12"  > 
             
              </div>  
          </div>    
            <div class="form-row">  
              <div class="form-group col-md-5" style="background-color:#FAFAFA" > 
                 
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

        function DibujaHorizontalBarTranps(x) {

            if(window.myChartsTrans != undefined)
                window.myChartsTrans.destroy();


             var nio = JSON.parse(x.d);
              if (nio.error) {
                document.getElementById('DibujaHorizontalBar').innerHTML = nio.error;
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

            document.getElementById('DibujaHorizontalBar').innerHTML = nio.descripcion;
            var ctx = document.getElementById('GraficoTransporte');

             var color = Chart.helpers.color;
            var barChartData = {
			labels: nio.labels,
			datasets:nSeries

            };
               
                window.myChartsTrans = new Chart(ctx, {
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
            myChartsTrans.update();
           
           

        }

        function DibujaHorizontalBarPase(x) {

             if(window.myChartsPase != undefined)
                window.myChartsPase.destroy();

             var nio = JSON.parse(x.d);
              if (nio.error) {
                document.getElementById('DibujaHorizontalBar2').innerHTML = nio.error;
                return;
             }

            document.getElementById('DibujaHorizontalBar2').innerHTML = nio.descripcion;

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
                         responsive: true,
                         legend: {
                            position: 'right'
                        }
                        
                    }
            });
            
            
            myChartsPase.update();

       }

        function DibujaLinea(x)
        {
            if(window.myChartsLin != undefined)
                window.myChartsLin.destroy();

            var nio = JSON.parse(x.d);
           
            if (nio.error) {
                document.getElementById('DibujaLinea').innerHTML = nio.error;
                return;
            }

            document.getElementById('DibujaLinea').innerHTML = nio.descripcion;

            //console.log(x.d);
            //console.log(nio.series);

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

           window.myChartsLin = new Chart(ctx, {
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

            myChartsLin.update();

       }




       function DibujarBarVolumen(x) {

             if(window.myChartsVol != undefined)
               window.myChartsVol.destroy();

            var nio = JSON.parse(x.d);
            if (nio.error) {
                document.getElementById('DibujaBarVolumen').innerHTML = nio.error;
                return;
           }

             
            document.getElementById('DibujaBarVolumen').innerHTML = nio.descripcion;
            var ctx = document.getElementById('GraficoBarVolumen');

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

             myChartsVol.update();
        }

       function DibujaHorizontalBarCliente(x) {

            if(window.myChartsCli != undefined)
               window.myChartsCli.destroy();

             var nio = JSON.parse(x.d);
              if (nio.error) {
                document.getElementById('DibujaHorizontalBarcliente').innerHTML = nio.error;
                return;
             }

            var nSeries2 = [];

            var colorNames = [window.chartColors.silver, window.chartColors.blue, window.chartColors.orange, window.chartColors.green, window.chartColors.yellow,
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
                nSeries2[i] = newDataset;
            }

            document.getElementById('DibujaHorizontalBarcliente').innerHTML = nio.descripcion;
            var ctx = document.getElementById('GraficoCliente');

             var color = Chart.helpers.color;
            var barChartData = {
			labels: nio.labels,
			datasets:nSeries2

            };

                window.myChartsCli = new Chart(ctx, {
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
            myChartsCli.update();
       }

       function DibujaHorizontalBarClienteMonto(x) {

            if(window.myChartsMon != undefined)
               window.myChartsMon.destroy();

             var nio = JSON.parse(x.d);
              if (nio.error) {
                document.getElementById('DibujaHorizontalBarclienteMonto').innerHTML = nio.error;
                return;
             }

            var nSeries3 = [];

            var colorNames = [window.chartColors.green, window.chartColors.purple, window.chartColors.orange, window.chartColors.black, window.chartColors.yellow,
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
                nSeries3[i] = newDataset;
            }

            document.getElementById('DibujaHorizontalBarclienteMonto').innerHTML = nio.descripcion;
            var ctx = document.getElementById('GraficoClienteMonto');

             var color = Chart.helpers.color;
            var barChartData = {
			labels: nio.labels,
			datasets:nSeries3

            };

                window.myChartsMon = new Chart(ctx, {
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

           myChartsMon.update();
       }


        //inicia el script
      $(document).ready(function ()
      {
            var fechainicial = document.getElementById('<%=sdesde.ClientID %>').value;
            var fechafinal = document.getElementById('<%=shasta.ClientID %>').value;
            var fecha1 = fechainicial.substr(3, 2) + "/" + fechainicial.substr(0, 2) + "/" + fechainicial.substr(6, 4);//nuevo formato
            var fecha2 = fechafinal.substr(3, 2) + "/" + fechafinal.substr(0, 2) + "/" + fechafinal.substr(6, 4);//nuevo formato

            
           var peticion5 = {};
           peticion5.id = 5;
           peticion5.ruc =  document.getElementById('<%=sruc.ClientID %>').value;//FACTURACIÓN DE CONTENEDORES DE IMPORTACIÓN
           peticion5.desde =  fecha1; 
           peticion5.hasta = fecha2; 
           GraficoDataMultiSerie(peticion5, DibujaLinea);
          
            var peticion8 = {};
            peticion8.id = 8;
            peticion8.ruc = document.getElementById('<%=sruc.ClientID %>').value;
            peticion8.desde = fecha1; 
            peticion8.hasta = fecha2; 
            GetGrafico(peticion8, DibujaHorizontalBarPase);//TOP 5 IMPORTADORES: FLETES CONTENEDORES DE IMPORTACIÓN

            var peticion7 = {};
            peticion7.id = 7;
            peticion7.ruc = document.getElementById('<%=sruc.ClientID %>').value;
            peticion7.desde = fecha1; 
            peticion7.hasta = fecha2; 
            GetGraficoMulti(peticion7, DibujaHorizontalBarTranps);//TOP 5 EMPRESAS TRANSPORTE: FLETES CONTENEDORES DE IMPORTACIÓN


           var peticion11 = {};
            peticion11.id = 11;
            peticion11.ruc = document.getElementById('<%=idagente.ClientID %>').value; 
            peticion11.desde = fecha1; 
            peticion11.hasta = fecha2; 
            GetGrafico(peticion11, DibujarBarVolumen);//VOLUMEN DE CONTENEDORES DE IMPORTACIÓN

           
            var peticion12 = {};
            peticion12.id = 12;
            peticion12.ruc = document.getElementById('<%=sruc.ClientID %>').value;
            peticion12.desde = fecha1; 
            peticion12.hasta = fecha2; 
            GetGraficoMulti(peticion12, DibujaHorizontalBarCliente);//TOP 5 IMPORTADORES: NÚMERO DE FACTURAS EMITIDAS 

            var peticion13 = {};
            peticion13.id = 13;
            peticion13.ruc = document.getElementById('<%=sruc.ClientID %>').value;
            peticion13.desde = fecha1; 
            peticion13.hasta = fecha2; 
            GetGraficoMulti(peticion13, DibujaHorizontalBarClienteMonto);//TOP 5 IMPORTADORES: FACTURACIÓN UDS

         
        
          
           
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

             
               var peticion5 = {};
               peticion5.id = 5;
               peticion5.ruc =  document.getElementById('<%=sruc.ClientID %>').value;//FACTURACIÓN DE CONTENEDORES DE IMPORTACIÓN
               peticion5.desde =  fecha1; 
               peticion5.hasta = fecha2; 
              
               GraficoDataMultiSerie(peticion5, DibujaLinea);
               

                var peticion8 = {};
                peticion8.id = 8;
                peticion8.ruc = document.getElementById('<%=sruc.ClientID %>').value;
                peticion8.desde = fecha1; 
                peticion8.hasta = fecha2; 
                GetGrafico(peticion8, DibujaHorizontalBarPase);//TOP 5 IMPORTADORES: FLETES CONTENEDORES DE IMPORTACIÓN

                var peticion7 = {};
                peticion7.id = 7;
                peticion7.ruc = document.getElementById('<%=sruc.ClientID %>').value;
                peticion7.desde = fecha1; 
                peticion7.hasta = fecha2; 
                GetGraficoMulti(peticion7, DibujaHorizontalBarTranps);//TOP 5 EMPRESAS TRANSPORTE: FLETES CONTENEDORES DE IMPORTACIÓN


                var peticion11 = {};
                peticion11.id = 11;
                peticion11.ruc = document.getElementById('<%=idagente.ClientID %>').value; 
                peticion11.desde = fecha1; 
                peticion11.hasta = fecha2; 
                GetGrafico(peticion11, DibujarBarVolumen);//VOLUMEN DE CONTENEDORES DE IMPORTACIÓN

           
                var peticion12 = {};
                peticion12.id = 12;
                peticion12.ruc = document.getElementById('<%=sruc.ClientID %>').value;
                peticion12.desde = fecha1; 
                peticion12.hasta = fecha2; 
                GetGraficoMulti(peticion12, DibujaHorizontalBarCliente);//TOP 5 IMPORTADORES: NÚMERO DE FACTURAS EMITIDAS 

                var peticion13 = {};
                peticion13.id = 13;
                peticion13.ruc = document.getElementById('<%=sruc.ClientID %>').value;
                peticion13.desde = fecha1; 
                peticion13.hasta = fecha2; 
                GetGraficoMulti(peticion13, DibujaHorizontalBarClienteMonto);//TOP 5 IMPORTADORES: FACTURACIÓN UDS

              
           }
           catch (e) {
  
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