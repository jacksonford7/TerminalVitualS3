<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Mapa con Iconos Personalizados</title>
    <style type="text/css">
        #map {
            height: 900px;
            width: 100%;
        }
    </style>
</head>
<body>
    <div id="map"></div>

   <script>
       function iniciarMap() {
           // Coordenadas de las ubicaciones de los carros y los trailers
           var ubicaciones = [

               { lat: -2.280301292104892, lng: -79.90701633217711, tipo: 'carro' },
               { lat: -2.2805907421147085, lng: -79.90767615602593, tipo: 'trailer' },
               { lat: -2.2801856790524724, lng: -79.90996609007001, tipo: 'barco' },
               { lat: -2.2811006739497484, lng: -79.90862183235755, tipo: 'barco' },
               { lat: -2.2834891545404266, lng: -79.90483219807852, tipo: 'barco' },
               { lat: -2.2820021463288063, lng: -79.90361740752758, tipo: 'rtg' },
               { lat: -2.2830177348923657, lng: -79.9044560377013, tipo: 'rtg' },
           ];

           // Crear un objeto de mapa de Google
           var mapa = new google.maps.Map(document.getElementById('map'), {
               center: ubicaciones[0], // Centro del mapa
               zoom: 18 // Nivel de zoom
           });

           // Icono personalizado para carros
           var iconoCarro = {
               url: 'https://imgur.com/ODYqyok.png',
               scaledSize: new google.maps.Size(50, 50),
               origin: new google.maps.Point(0, 0),
               anchor: new google.maps.Point(16, 32)
           };

           // Icono personalizado para trailers
           var iconoTrailer = {
               url: 'https://i.imgur.com/0b2xCZ7.png',
               scaledSize: new google.maps.Size(70, 70),
               origin: new google.maps.Point(0, 0),
               anchor: new google.maps.Point(16, 32)
           };
           var iconoBarco = {
               url: 'https://i.imgur.com/VzwdkwG.png',
               scaledSize: new google.maps.Size(50, 50),
               origin: new google.maps.Point(0, 0),
               anchor: new google.maps.Point(16, 32)
           };

           var iconoRtg = {
               url: 'https://i.imgur.com/zrThkkj.png',
               scaledSize: new google.maps.Size(40, 40),
               origin: new google.maps.Point(0, 0),
               anchor: new google.maps.Point(16, 32)
           };

           // Agregar marcadores con diferentes iconos y ventanas de información
           for (var i = 0; i < ubicaciones.length; i++) {
               var ubicacion = ubicaciones[i];
               var icono;
               var tipo;

               if (ubicacion.tipo === 'carro') {
                   icono = iconoCarro;
                   tipo = 'Carro';
               } else if (ubicacion.tipo === 'trailer') {
                   icono = iconoTrailer;
                   tipo = 'Trailer'; 
               }
               else if (ubicacion.tipo === 'barco') {
                   icono = iconoBarco;
                   tipo = 'Barco';
               }
               else if (ubicacion.tipo === 'rtg') {
                   icono = iconoRtg;
                   tipo = 'RTG';
               }

               var marker = new google.maps.Marker({
                   position: ubicacion,
                   map: mapa,
                   icon: icono,
                   title: tipo + ' ' + (i + 1)
               });

               // Crear una ventana de información para el marcador
               var infoWindow = new google.maps.InfoWindow();

               // Agregar eventos para mostrar la ventana de información al hacer clic
               google.maps.event.addListener(marker, 'click', (function (marker, tipo) {
                   return function () {
                       infoWindow.setContent(tipo + ' ' + ubicacion.lat.toFixed(6) + ', ' + ubicacion.lng.toFixed(6));
                       infoWindow.open(mapa, marker);
                   };
               })(marker, tipo));
           }
       }
   </script>


     <%--<script type="text/javascript" src='https://maps.google.com/maps/api/js?key=AIzaSyA0f3IQRMX1fmn-35UxyLJSDvKv3BbKBhI&sensor=false&libraries=places'></script>--%>

    <!-- Cargar la API de Google Maps -->
    <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyA0f3IQRMX1fmn-35UxyLJSDvKv3BbKBhI&callback=iniciarMap" async defer></script>
</body>
</html>
