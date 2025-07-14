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