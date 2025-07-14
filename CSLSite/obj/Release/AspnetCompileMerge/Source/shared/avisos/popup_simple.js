    $(document).ready(function () {
            $('#ventana_popup').fadeIn('slow');
            $('#close').click(function () { $('#ventana_popup').fadeOut('slow'); return false; });
            $('#close').click(function () { $("#ventana_popup").fadeOut('slow'); $('#popup-overlay').fadeOut('slow'); });
			$('#popup-overlay').fadeIn('slow');
			$('#popup-overlay').height($(window).height());

			//funcion automatica cierra ventana popup en 6 segundos
			setTimeout(function(){ $("#ventana_popup").fadeOut("slow"); $('#popup-overlay').fadeOut('slow'); }, 10000 ); 
    });
	//presional cuiaquier tecla sale
	$(document).keydown(function(e) {  $('#ventana_popup').fadeOut('slow');  $('#popup-overlay').fadeOut('slow'); });
//	//da clic afuera sale
	$(document).click(function () {  $("#ventana_popup").fadeOut('slow'); $('#popup-overlay').fadeOut('slow');});