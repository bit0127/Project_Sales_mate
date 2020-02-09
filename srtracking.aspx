<%@ Page Language="C#" AutoEventWireup="true" CodeFile="srtracking.aspx.cs" Inherits="srtracking" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SR Tracking</title>
    <link rel="shortcut icon" href="Nimages/logopran.gif" />
    <%--<script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false"></script>--%>
    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyDpvfxj-OZUOitRtAvruyJBJvsqj6c6zig&callback=initMap"></script>
    <script type="text/javascript">
        var markers = [
        <asp:Repeater ID="rptMarkers" runat="server">
        <ItemTemplate>
                    {
                        "title": '<%# Eval("SR_ID") %>',
                    "lat": '<%# Eval("LATITUDE") %>',
                    "lng": '<%# Eval("LONGITUDE") %>',
                    "description": '<%# Eval("DESCRIPTIONS") %>',
                    "datetime": '<%# Eval("currentTime") %>',
                    "outletinfo": '<%# Eval("OUTLET_INFO") %>'
                }
</ItemTemplate>
<SeparatorTemplate>
    ,
</SeparatorTemplate>
</asp:Repeater>
    ];

  

    </script>
    <script type="text/javascript">
        window.onload = function () {
            var mapOptions = {
                center: new google.maps.LatLng(markers[0].lat, markers[0].lng),
                zoom: 8,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            var infoWindow = new google.maps.InfoWindow();
            var map = new google.maps.Map(document.getElementById("dvMap"), mapOptions);
            for (i = 0; i < markers.length; i++) {
                var data = markers[i]
                var myLatlng = new google.maps.LatLng(data.lat, data.lng);
                var marker = new google.maps.Marker({
                    position: myLatlng,
                    map: map,
                    title: data.title
                });
                (function (marker, data) {
                    google.maps.event.addListener(marker, "click", function (e) {
                        infoWindow.setContent(data.description + '</br>' + data.datetime + '</br></br>' + data.outletinfo);                     
                        infoWindow.open(map, marker);
                    });
                })(marker, data);
            }
        }
    </script>

    <style type="text/css">
        .mapArea{
            width:100%;
            height:900px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="bodywrapper">
            <div id="header" style="background-color:#8dc060;">
                <div>
                    <img src="images/pran-logo.png" width="80" height="25" title="Sales Mate" /> <span style='color:#d62d20;font-size:20px;font-weight: bold;'>SR Tracking System</span>
                </div>
            </div>
            <div id="dvMap" class="mapArea">
            </div>
        </div>
    </form>
</body>
</html>
