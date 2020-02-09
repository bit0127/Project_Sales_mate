<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SrLocationTracking.aspx.cs" Inherits="SrLocationTracking" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SR Location Tracking</title>

    <!--<script src="js/vendor.js"></script>
    <script src="js/app.js"></script>-->


    <link rel="stylesheet" href="css/date-picker-css.css" />
    <script src="NJavaScript/jquery-1.10.2.js"></script>
    <!--<script src="js/jquery-ui.js"></script>-->
    <script src="NgridJs/jquery-1.12.0.min.js" type="text/javascript"></script> 
</head>
<body>
     <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?key=AIzaSyBz0enLv47fb-nW0FNATwVCdOC1UAFWN9s"></script>
<script type="text/javascript">

    var markers = [];
    var geocoder;
    var map;
    //var $j = jQuery.noConflict();

    $(document).ready(function () {



        var srID = '152015';
        var date = '05/02/2017';



        $.ajax({
            type: "POST",
            url: "Service.asmx/GetInfo",
            data: "{'SR_ID':'" + srID + "', 'Date_Time':'" + date + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {


                markers = JSON.parse(response.d);

                console.log(markers);
                Test();
            },
            failure: function (response) {
                alert(response.d);
            }
        });


        function Test() {
            var marker = [];
            var mapOptions = {
                center: new google.maps.LatLng(markers[0].LAT, markers[0].LNG),
                zoom: 10,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            var map = new google.maps.Map(document.getElementById("dvMap"), mapOptions);
            var infoWindow = new google.maps.InfoWindow();
            var lat_lng = new Array();
            var latlngbounds = new google.maps.LatLngBounds();
            for (i = 0; i < markers.length; i++) {
                var data = markers[i]
                var myLatlng = new google.maps.LatLng(data.LAT, data.LNG);
                lat_lng.push(myLatlng);

                if (i == 0) {
                    marker = new google.maps.Marker({
                        position: myLatlng,
                        map: map,
                        label: {
                            color: 'black',
                            fontWeight: 'bold',
                            text: 'Start',
                        },
                        icon: {
                            labelOrigin: new google.maps.Point(11, 50),
                            url: 'http://maps.google.com/mapfiles/ms/icons/green-dot.png',
                            size: new google.maps.Size(22, 40),
                            origin: new google.maps.Point(0, 0),
                            animation: google.maps.Animation.DROP,
                            anchor: new google.maps.Point(11, 40),
                        },
                        title: data.TITLE
                    });
                }
                else if (i == markers.length - 1) {
                    marker = new google.maps.Marker({
                        position: myLatlng,
                        map: map,
                        label: {
                            color: 'black',
                            fontWeight: 'bold',
                            text: 'End',
                        },
                        icon: {
                            labelOrigin: new google.maps.Point(11, 50),
                            url: 'http://maps.google.com/mapfiles/ms/icons/yellow-dot.png',
                            size: new google.maps.Size(22, 40),
                            origin: new google.maps.Point(0, 0),
                            animation: google.maps.Animation.DROP,
                            anchor: new google.maps.Point(11, 40),
                        },
                        title: data.TITLE
                    });
                }

                else {
                    marker = new google.maps.Marker({
                        position: myLatlng,
                        map: map,
                        title: data.TITLE
                    });
                }


                latlngbounds.extend(marker.position);
                (function (marker, data) {
                    google.maps.event.addListener(marker, "click", function (e) {
                        infoWindow.setContent(data.DESCRIPTION);
                        infoWindow.open(map, marker);
                    });
                })(marker, data);
            }
            map.setCenter(latlngbounds.getCenter());
            map.fitBounds(latlngbounds);

            //***********ROUTING****************//

            //Intialize the Path Array
            var path = new google.maps.MVCArray();

            //Intialize the Direction Service
            var service = new google.maps.DirectionsService();

            //Set the Path Stroke Color
            var poly = new google.maps.Polyline({ map: map, strokeColor: '#4986E7' });

            //Loop and Draw Path Route between the Points on MAP
            for (var i = 0; i < lat_lng.length; i++) {
                if ((i + 1) < lat_lng.length) {
                    var src = lat_lng[i];
                    var des = lat_lng[i + 1];
                    path.push(src);
                    poly.setPath(path);
                    service.route({
                        origin: src,
                        destination: des,
                        travelMode: google.maps.DirectionsTravelMode.BICYCLING
                    }, function (result, status) {
                        if (status == google.maps.DirectionsStatus.OK) {
                            for (var i = 0, len = result.routes[0].overview_path.length; i < len; i++) {
                                path.push(result.routes[0].overview_path[i]);
                            }
                        }
                    });
                }
            }
        }

    });


    </script>
    <form id="form1" runat="server">
    <div id="dvMap" style="width: 1150px; height: 700px">
    </div>
    </form>
</body>
</html>
