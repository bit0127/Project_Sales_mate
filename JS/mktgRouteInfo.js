

var $ = jQuery.noConflict();
$(function () {

    //--Load grid view---------------------
    $("#tblData").html("");
    $("#tblData").html("<div style='width:545px;'><span id='loader' style='float:left'><img src='images/ajax-loader.gif' alt='Loading...' style='width:300px;height:17px;margin-top:10px; margin-left:40%; display:block;'/></span><span id='spPercentage' style='float:right;color:#2eaccc;display:block;margin-top:10px;'></span></div>");

    var tbl = "<table id='example' class='display' style='color:#9C27B0' cellspacing='0' width='100%'style='padding-top:10px;'>";
    var thead = "<thead><tr><th>Route ID</th><th>Route Name</th><th>Zone Name</th><th>Region Name</th><th>Division Name</th><th>Country Name</th><th>Status</th><th>Operaton</th></tr></thead>";

    var row = "";
    var route = "route";
    $.ajax({
        type: "POST",
        url: "trademktg.aspx/GetRouteInfo",
        data: "{route:'" + route + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var routeInfo = response.d.split(';');

            for (var i = 1; i < routeInfo.length; i = i + 10) {
                var routeId = routeInfo[i];
                var routeName = routeInfo[i + 1];

                var zoneId = routeInfo[i + 2];
                var zoneName = routeInfo[i + 3];

                var regionId = routeInfo[i + 4];
                var regionName = routeInfo[i + 5];

                var divisionId = routeInfo[i + 6];
                var divisionName = routeInfo[i + 7];
                
                var country = routeInfo[i + 8];
                var status = routeInfo[i + 9];
               

                row = row + "<tr style='text-align:left'><td>" + routeId + "</td><td>" + routeName + "</td><td>" + zoneName + "</td><td>" + regionName + "</td><td>" + divisionName + "</td><td>" + country + "</td><td>" + status + "</td><td><div style='float:right;'><button type='button' style='float: right; margin-top: 0px;' class='btn btn-xs btn-success editrouteid' id='" + routeId + "'><span class ='glyphicon glyphicon-edit'></span>Edit</button></div></td></tr>";
            }

            tbl = tbl + thead + "<tbody>" + row + "</tbody></table>";

            //$("#loader").css("display", "none");
            $("#tblData").html("");
            $("#tblData").html(tbl);

            var table = $('#example').DataTable({
                lengthChange: true,
                "scrollX": true,
                buttons: true
            });

            table.buttons().container().insertBefore('#example_filter');


        },
        failure: function (response) {
            alert(response.d);
        }
    });


    $('#tblData').on('click', '.editrouteid', function () {

        var route = this.id;
        //alert(srID);

        $.ajax({
            type: "POST",
            url: "trademktg.aspx/GetRouteInfo",
            data: "{route:'" + route + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                var routeInfo = response.d.split(';');

                var routeId = routeInfo[1];
                var routeName = routeInfo[2];

                var zoneId = routeInfo[3];
                var zoneName = routeInfo[4];

                var regionId = routeInfo[5];
                var regionName = routeInfo[6];

                var divisionId = routeInfo[7];
                var divisionName = routeInfo[8];

                var country = routeInfo[9];
                var status = routeInfo[10];

                $.ajax({
                    type: "POST",
                    url: "trademktg.aspx/GetAllDivision",
                    data: "{countryName:'" + country + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (response) {

                        var companyInfo = response.d.split(';');
                        var opt = "<option value='-1'>...Select...</option>";
                        for (var i = 1; i < companyInfo.length; i = i + 2) {
                            var groupId = companyInfo[i];
                            var groupName = companyInfo[i + 1];
                            opt = opt + "<option value='" + groupId + "'>" + groupName + "</option>";
                        }
                        $("#ddlDivision").html('');
                        $("#ddlDivision").append(opt);

                    },
                    failure: function (response) {
                        alert(response.d);
                    }
                });

                $.ajax({
                    type: "POST",
                    url: "trademktg.aspx/GetRegionByDivision",
                    data: "{divisionId:'" + divisionId + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (response) {

                        var companyInfo = response.d.split(';');
                        var opt = "<option value='-1'>...Select...</option>";
                        //opt = opt + "<option value='AllDivision'>All Division</option>";
                        for (var i = 1; i < companyInfo.length; i = i + 2) {
                            var groupId = companyInfo[i];
                            var groupName = companyInfo[i + 1];
                            opt = opt + "<option value='" + groupId + "'>" + groupName + "</option>";
                        }
                        $("#ddlRegion").html('');
                        $("#ddlRegion").append(opt);

                    },
                    failure: function (response) {
                        alert(response.d);
                    }
                });

                $.ajax({
                    type: "POST",
                    url: "trademktg.aspx/GetAllZone",
                    data: "{regionId:'" + regionId + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (response) {

                        var companyInfo = response.d.split(';');
                        var opt = "<option value='-1'>...Select...</option>";
                        for (var i = 1; i < companyInfo.length; i = i + 2) {
                            var groupId = companyInfo[i];
                            var groupName = companyInfo[i + 1];
                            opt = opt + "<option value='" + groupId + "'>" + groupName + "</option>";
                        }
                        $("#ddlZone").html('');
                        $("#ddlZone").append(opt);

                    },
                    failure: function (response) {
                        alert(response.d);
                    }
                });

                $("#txtRouteId").val(routeId);
                $("#txtRouteName").val(routeName);
                $("#ddlCountry").val(country);
                $("#ddlDivision").val(divisionId);
                $("#ddlRegion").val(regionId);
                $("#ddlZone").val(zoneId);
                $("#ddlRouteActive").val(status);

                //var table = $('#example').DataTable({
                //    lengthChange: true,
                //    "scrollX": true,
                //    buttons: true
                //});

                //table.buttons().container().insertBefore('#example_filter');


            },
            failure: function (response) {
                alert(response.d);
            }
        });

    });

    //--end grid view panel---------------------------------------------------

});