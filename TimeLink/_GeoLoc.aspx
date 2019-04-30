<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="_GeoLoc.aspx.cs" Inherits="TimeLink._GeoLoc" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="shortcut icon" href="~/Img/appIcon3.png" type="image/x-icon" />
    <link rel="stylesheet" type="text/css" href="Css/geoloc.css" />
    <link rel="stylesheet" type="text/css" href="https://unpkg.com/leaflet@1.4.0/dist/leaflet.css"
        integrity="sha512-puBpdR0798OZvTTbP4A8Ix/l+A4dHDD0DGqYW6RQ+9jxkRFclaxxQb/SJAWZfWAkuyeQUytO7+7N4QKrDh+drA=="
        crossorigin="" />
    <script src="https://unpkg.com/leaflet@1.4.0/dist/leaflet.js"
        integrity="sha512-QVftwZFqvtRNi0ZyCtsznlKSWOStnDORoefr1enyq5mVL4tmKB3S/EnC3rRJcxCPavG10IcrVGSmPh6Qw5lwrg=="
        crossorigin=""></script>

    <script src='https://api.tiles.mapbox.com/mapbox-gl-js/v0.53.1/mapbox-gl.js'></script>
    <link rel="stylesheet" type="text/css" href='https://api.tiles.mapbox.com/mapbox-gl-js/v0.53.1/mapbox-gl.css' />

    <link rel="stylesheet" type="text/css" href="https://openlayers.org/en/v5.3.0/css/ol.css" />

</head>
<body>
    <form id="form1" runat="server">
        <div id="map">
            map1
        </div>
        <div id="map2" class="map22">map2</div>

        <script>
            //привязка карты к div
            var mymap = L.map('map');
            //указание координат и масштаб (https://2gis.ru/v_novgorod/)
            mymap.setView([58.521426, 31.275416], 16)
            //указывается источник карт, откуда будут загружатся
            L.tileLayer('http://{s}.tile.osm.org/{z}/{x}/{y}.png',
                {
                    attribution: 'Map data &copy; <a href="https://2gis.ru/v_novgorod/">2gis.ru</a>',
                    minZoom: 15,
                    maxZoom: 17,
                    id: 'mapbox.streets',
                    accessToken: 'your.mapbox.access.token'
                }).addTo(mymap);
            //маркер на карту
            var marker = L.marker([58.521426, 31.275416]).addTo(mymap);
            //попап при нажатии маркера
            marker.bindPopup("<p>Some text about <a target=\"_blank\" href=\"https://www.novgorod.ru\">Novgorod</a>...</p>");
            //маркер круг
            var circle = L.circle([58.521036, 31.276875], 20, {
                color: 'red',
                fillColor: '#f03',
                fillOpacity: 0.5
            }).addTo(mymap);
            circle.bindPopup("<p>Some text about <a target=\"_blank\" href=\"https://www.novgorod.ru\">Novgorod</a>...</p>");

            //полигон
            var polygon = L.polygon([
                [58.521913, -0.08],
                [31.276811, -0.06],
                [32.276811, -0.047]
            ]).addTo(mymap);

            //прикрепить некоторую информацию к конкретному объекту на карте
            var popup = L.popup()
                .setLatLng([58.521913, 31.276811])
                .setContent("Софийский собор")
                .openOn(mymap);

            //function onMapClick(e) {
            //    alert("You clicked the map at " + e.latlng);
            //}
            //mymap.on('click', onMapClick);

            var popup = L.popup();

            function onMapClick(e) {
                popup
                     .setLatLng(e.latlng)
                     .setContent("You clicked the map at " + e.latlng.toString())
                      .openOn(mymap);
            }

            mymap.on('click', onMapClick);

            //свой маркер
            var greenIcon = L.icon({
                iconUrl: 'C:\\1.png',
                shadowUrl: 'C:\\1.png',

                iconSize: [38, 95], // size of the icon
                shadowSize: [50, 64], // size of the shadow
                iconAnchor: [22, 94], // point of the icon which will correspond to marker's location
                shadowAnchor: [4, 62],  // the same for the shadow
                popupAnchor: [-3, -76] // point from which the popup should open relative to the iconAnchor
            });

            L.marker([58.521913, 31.276811], { icon: greenIcon }).addTo(mymap);

            //свой маркер но на его основе можно создать другие поменяв параметр\ы
            var LeafIcon = L.Icon.extend({
                options: {
                    shadowUrl: 'leaf-shadow.png',
                    iconSize: [38, 95],
                    shadowSize: [50, 64],
                    iconAnchor: [22, 94],
                    shadowAnchor: [4, 62],
                    popupAnchor: [-3, -76]
                }
            });

            var greenIcon = new LeafIcon({ iconUrl: 'leaf-green.png' }),
            redIcon = new LeafIcon({ iconUrl: 'leaf-red.png' }),
            orangeIcon = new LeafIcon({ iconUrl: 'leaf-orange.png' });

            L.control.scale().addTo(mymap);
           // mymap.addControl(new mapboxgl.NavigationControl());
          

        </script>
        <br/>
<%--        <script>
      var map = new Map({
        layers: [
          new TileLayer({
            source: new OSM()
          })
        ],
        target: 'map2',
        view: new View({
          center: [14200000, 4130000],
          rotation: Math.PI / 6,
          zoom: 10
        })
      });
    </script>--%>



    </form>
</body>
</html>
