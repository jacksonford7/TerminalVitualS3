function BindFunctions() {
    $(document).ready(function () {
       
        var zc = document.getElementById('imagen');
        if (zc) {
            zc.innerHTML = '';
        }

        $('#tablasort').DataTable({
            language: {
                "lengthMenu": "_MENU_ REGISTROS POR PAGINA",
                "zeroRecords": "Sin resultados",
                "info": "Registros del _START_ al _END_ : Total:  _TOTAL_",
                "infoEmpty": "Registros del 0 al 0 : Total 0 ",
                "infoFiltered": "(filtrado de un total de _MAX_ registros)",
                "sSearch": "Buscar:",
                "sProcessing": "Procesando...",
            },
            //para usar los botones   
            responsive: "true",
            dom: 'Bfrtilp'

        });
    });

    //tabla order 2hhh
    $('#tablasort1').DataTable({
        language: {
            "lengthMenu": "_MENU_ REGISTROS POR PAGINA",
            "zeroRecords": "No se encontraron resultados",
            "info": "Registros del _START_ al _END_ : Total:  _TOTAL_",
            "infoEmpty": "Registros del 0 al 0 : Total 0 ",
            "infoFiltered": "(filtrado de un total de _MAX_ registros)",
            "sSearch": "Buscar:",
            "sProcessing": "Procesando...",
        },
        //para usar los botones   
        responsive: "true",
        dom: 'Bfrtilp'

    });
}
