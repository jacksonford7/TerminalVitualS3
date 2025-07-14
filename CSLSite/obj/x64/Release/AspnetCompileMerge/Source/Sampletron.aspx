<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sampletron.aspx.cs" Inherits="CSLSite.Sampletron" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ejemplos</title>
    <script   src="https://code.jquery.com/jquery-3.5.1.min.js"  integrity="sha256-9/aliU8dGd2tb6OSsuzixeV4y/faTqgFtohetphbbj0=" crossorigin="anonymous"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.9.4/Chart.css" integrity="sha512-C7hOmCgGzihKXzyPU/z4nv97W0d9bv4ALuuEbSf6hm93myico9qa0hv4dODThvCsqQUmKmLcJmlpRmCaApr83g==" crossorigin="anonymous" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.9.4/Chart.min.js" integrity="sha512-d9xgZrVZpmmQlfonhQUvTR7lMPtO7NkZMkA0ABN3PHCbKA5nqylQ/yWlFAyY6hYgdF1Qh6nYiuADWwKB4C2WSw==" crossorigin="anonymous"></script>   
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.9.4/Chart.bundle.min.js" integrity="sha512-SuxO9djzjML6b9w9/I07IWnLnQhgyYVSpHZx0JV97kGBfTIsUYlWflyuW4ypnvhBrslz1yJ3R+S14fdCWmSmSA==" crossorigin="anonymous"></script>
   

</head>
<body>
    <form id="form1" runat="server">
          
        <div style="width:40%; height:40%; display:inline-block; border:1px dotted #ccc; padding:1px;">  
            <span id="DibujaBarras"></span>
             <canvas id="myChart" ></canvas>
        </div>

         <div style="width:40%; height:40%; display:inline-block; border:1px dotted #ccc; padding:1px;">   
              <span id="DibujaLineas"></span>
             <canvas id="myLine" ></canvas>
        </div>


           <div style="width:40%; height:40%; display:inline-block; border:1px dotted #ccc; padding:1px;">   
              <span id="DibujaPie"></span>
             <canvas id="myPie" ></canvas>
        </div>

         <%--cuando cargue la pagina llena estos controles con los datos--%>
         <asp:HiddenField  runat="server" ID="sruc" Value="0991370226001" ClientIDMode="Static"/>
         <asp:HiddenField  runat="server" ID="sdesde" Value="2020-01-01" ClientIDMode="Static"/>
         <asp:HiddenField  runat="server" ID="shasta" Value="2020-12-31" ClientIDMode="Static"/>
    </form>

    <script type="text/javascript">


        //objeto de esta peticion--> lleno los parametros//
        //si en pantalla hay que redibujar entonces debes enviar la peticion de nuevo, para esto debes crearla
        
        var peticion1 = {};
        peticion1.id = 9;
        peticion1.ruc = document.getElementById('sruc').value;
        peticion1.desde =  document.getElementById('sdesde').value;
        peticion1.hasta = document.getElementById('shasta').value;




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
            var nio = JSON.parse(x.d);
            if (nio.error) {
                document.getElementById('DibujaBarras').innerHTML = nio.error;
                return;
            }
            document.getElementById('DibujaBarras').innerHTML = nio.descripcion;
                var ctx = document.getElementById('myChart');
                var myChart = new Chart(ctx, {
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
                        scales: {
                            yAxes: [{
                                ticks: {
                                    beginAtZero: true
                                }
                            }]
                        }
                    }
                });
        }


         function DibujaLineas(x) {
             var nio = JSON.parse(x.d);
              if (nio.error) {
                document.getElementById('DibujaLineas').innerHTML = nio.error;
                return;
             }
             document.getElementById('DibujaLineas').innerHTML = nio.descripcion;
                var ctx = document.getElementById('myLine');
                var myChart = new Chart(ctx, {
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
                    }
                });
        }


           function DibujaPie(x) {
             var nio = JSON.parse(x.d);
              if (nio.error) {
                document.getElementById('DibujaPie').innerHTML = nio.error;
                return;
             }
             document.getElementById('DibujaPie').innerHTML = nio.descripcion;
                var ctx = document.getElementById('myPie');
                var myChart = new Chart(ctx, {
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
                    }
                });
        }

        function DibujaMulti(x) {
            var nio = JSON.parse(x.d);
            alert(x.d);
            if (nio.error) {
                document.getElementById('DibujaPie').innerHTML = nio.error;
                return;
            }
        }

            function DibujaMultiData(x) {
            var nio = JSON.parse(x.d);
                alert(x.d);
                 console.log(x.d);

            if (nio.error) {
                document.getElementById('DibujaPie').innerHTML = nio.error;
                return;
            }
        }


        //inicia el script
        $(document).ready(function () {
           //esto por cada grafico.
             peticion1.id = 1;
            GetGrafico(peticion1, DibujaBarras);
            //////solo cambie el id del grafico para la db
            ////peticion1.id = 1;
            ////GetGrafico(peticion1, DibujaLineas);
            //////solo cambie el id del grafico para la db
            ////  peticion1.id = 1;
            ////GetGrafico(peticion1, DibujaPie);

            ///nuevo grafico multiseries
           /*
            *El campo series, es un arreglo de objetos "serie"
            * serie.serie = 2020-01 (string)
            * serie.conteo = 1   (entero)
            * serie.total = $11 (montos)
            * 
            * Por cada objeto o en Objeto.series -> 
            * 
            * Lo que debes hacer es recorrerlo y crear 3 arreglos
            * 1. Para la configurarios "labels" del grafico
            * 2. Para la serie de cantidad
            * 3. Para la serie de valores
            * */

               peticion1.id = 18;
              GraficoDataMultiSerie(peticion1, DibujaMultiData);
        });
    </script>
</body>



</html>
