
var $ = jQuery.noConflict();
$(function () {

$('#dvMiddleContent').html('');
$('#dvMiddleContent').html("<div style='width:70%;height:36px;margin-top: 5px;background-color:#8dc060;text-align:center;color:#fff;font-size:25px;padding-top:10px;margin-top:10px;margin-left:15%;'><span>Outlet Information Search panel</span></div>" +
                   "<div style='width:69.8%;height:auto;margin-top: 2px;border:1px solid #8dc060;padding-top:10px;margin-left:15%;'>" +
                      "<table style='padding-top:10px;padding-bottom: 10px;padding-left:21%'>" +
                      
                        "<tr><td>Country Name :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlCountry' name='ddlCountry'><option value='-1'>...Select...</option><option value='Bangladesh'>Bangladesh</option><option value='India'>India</option><option value='Nepal'>Nepal</option><option value='Malaysia'>Malaysia</option><option value='Oman'>Oman</option><option value='UAE'>UAE</option></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +

                        "<tr><td>Division Name :</td>" +
                            "<td>" +
                                 "<select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlDivision' name='ddlCountry'>" +

                                 "</select>" +
                            "</td><td><span style='color:#ec407a;'>*</span></td>" +
                        "</tr>" +
                        "<tr><td>Zone :</td>" +
                            "<td>" +
                                 "<select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlZone' name='ddlZone'>" +

                                 "</select>" +
                            "</td><td><span style='color:#ec407a;'>*</span></td>" +
                        "</tr>" +

                         "<tr><td>Route :</td>" +
                            "<td>" +
                                 "<select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlRoute' name='ddlRoute'>" +

                                 "</select>" +
                            "</td><td><span style='color:#ec407a;'>*</span></td>" +
                         "</tr>" +                          

                        "<tr><td></td><td style='text-align:right;'> <button type='button' id='btnSearch' style='background-color: #8dc060;border-color: #4cae4c;color: #fff;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;cursor: pointer;display: inline-block;font-size: 14px;font-weight: 400;line-height: 1.42857;margin-bottom: 0;padding: 6px 12px;text-align: center;vertical-align: middle;white-space: nowrap;'>Search</button> </td></tr>" +
                      "</table>" +
                    "</div>");
    
loadDivision();
loadZone();

$("#ddlZone").change(function () {
    var zoneId = $("#ddlZone").val();
    $.ajax({
        type: "POST",
        url: "operation.aspx/GetRoute",
        data: "{zoneId:'" + zoneId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {

            var routeInfo = response.d.split(';');
            var opt = "<option value='-1'>...Select...</option>";
            for (var i = 1; i < routeInfo.length; i = i + 2) {
                var routeId = routeInfo[i];
                var routeName = routeInfo[i + 1];
                opt = opt + "<option value='" + routeId + "'>" + routeName + "</option>";
            }
            $("#ddlRoute").html('');
            $("#ddlRoute").append(opt);

        },
        failure: function (response) {
            alert(response.d);
        }
    });
});

$("#btnSearch").click(function () {

     
    var country = $("#ddlCountry").val();
    var division = $("#ddlDivision").val();
    var zone = $("#ddlZone").val();
    var route = $("#ddlRoute").val();     

    if (country == "...Select...") {
        alert('Select Country');
        return;
    }
    else if (division == "...Select...") {
        alert('Select Division');
        return;
    }
    else if (zone == "...Select...") {
        alert('Select Zone');
        return;
    }
    else if (route == "...Select...") {
        alert('Select Route');
        return;
    }
     

    $.ajax({
        type: "POST",
        url: "operation.aspx/SearchOutletInfo",
        data: "{country:'" + country + "',division:'" + division + "',zone:'" + zone + "',route:'" + route + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            
            $("#tblData").html("");
            $("#tblData").html("<div style='width:545px;'><span id='loader' style='float:left'><img src='images/ajax-loader.gif' alt='Loading...' style='width:300px;height:17px;margin-top:10px; margin-left:40%; display:block;'/></span><span id='spPercentage' style='float:right;color:#2eaccc;display:block;margin-top:10px;'></span></div>");

            var tbl = "<table id='example' class='display' style='color:#9C27B0' cellspacing='0' width='100%'style='padding-top:10px;'>";
            var thead = "<thead><tr><th>Outlet ID</th><th>Outlet Name</th><th>Outlet Address</th><th>Propritor Name</th><th>Phone Number</th><th>Email Address</th><th>Country</th><th>Division</th><th>Zone</th><th>Route</th><th>Category</th><th>Grade</th><th>Fridge</th><th>Signboard</th><th>Rack/Unit</th><th>Status</th><th>Operaton</th></tr></thead>";

            var row = "";

            var outletInfo = response.d.split(';');

            for (var i = 1; i < outletInfo.length; i = i + 19) {
                var outletId = outletInfo[i];
                var outletName = outletInfo[i + 1];
                var address = outletInfo[i + 2];
                var propritorName = outletInfo[i + 3];
                var phone = outletInfo[i + 4];
                var email = outletInfo[i + 5];

                var country = outletInfo[i + 6];
                var divisionId = outletInfo[i + 7];
                var divisionName = outletInfo[i + 8];
                var zoneId = outletInfo[i + 9];
                var zoneName = outletInfo[i + 10];
                var routeId = outletInfo[i + 11];
                var routeName = outletInfo[i + 12];

                var fride = outletInfo[i + 13];
                var signboard = outletInfo[i + 14];
                var rack = outletInfo[i + 15];

                var category = outletInfo[i + 16];
                var grade = outletInfo[i + 17];
                var status = outletInfo[i + 18];

                row = row + "<tr style='text-align:left'><td>" + outletId + "</td><td>" + outletName + "</td><td>" + address + "</td><td>" + propritorName + "</td><td>" + phone + "</td><td>" + email + "</td><td>" + country + "</td><td>" + divisionName + "</td><td>" + zoneName + "</td><td>" + routeName + "</td><td>" + category + "</td><td>" + grade + "</td><td>" + fride + "</td><td>" + signboard + "</td><td>" + rack + "</td><td>" + status + "</td><td><div style='float:right;'><button type='button' style='float: right; margin-top: 0px;' class='btn btn-xs btn-success editoutletid' id='" + outletId + "'><span class ='glyphicon glyphicon-edit'></span>Edit</button></div></td></tr>";
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

    $('#tblData').on('click', '.editoutletid', function () {

        var outlet = this.id;
        //alert(srID);

        $.ajax({
            type: "POST",
            url: "operation.aspx/GetOutletInfo",
            data: "{outlet:'" + outlet + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                var outletInfo = response.d.split(';');

                var outletId = outletInfo[1];
                var outletName = outletInfo[2];
                var address = outletInfo[3];
                var propritorName = outletInfo[4];
                var phone = outletInfo[5];
                var email = outletInfo[6];

                var country = outletInfo[7];
                var divisionId = outletInfo[8];
                var divisionName = outletInfo[9];
                var zoneId = outletInfo[10];
                var zoneName = outletInfo[11];
                var routeId = outletInfo[12];
                var routeName = outletInfo[13];

                var fride = outletInfo[14];
                var signboard = outletInfo[15];
                var rack = outletInfo[16];

                var category = outletInfo[17];
                var grade = outletInfo[18];
                var status = outletInfo[19];


                $.ajax({
                    type: "POST",
                    url: "operation.aspx/GetDivision",
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
                    url: "operation.aspx/GetZone",
                    data: "{division:'" + divisionId + "'}",
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

                $.ajax({
                    type: "POST",
                    url: "operation.aspx/GetRoute",
                    data: "{zoneId:'" + zoneId + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (response) {

                        var routeInfo = response.d.split(';');
                        var opt = "<option value='-1'>...Select...</option>";
                        for (var i = 1; i < routeInfo.length; i = i + 2) {
                            var routeId = routeInfo[i];
                            var routeName = routeInfo[i + 1];
                            opt = opt + "<option value='" + routeId + "'>" + routeName + "</option>";
                        }
                        $("#ddlRoute").html('');
                        $("#ddlRoute").append(opt);

                    },
                    failure: function (response) {
                        alert(response.d);
                    }
                });

                $.getScript("JS/editSpecificOutlet.js");

                $("#txtOutletID").val(outletId);
                $("#txtOutletName").val(outletName);
                $("#txtOutletAddress").val(address);
                $("#txtPropritorName").val(propritorName);

                //var outletNameBangla = $("#txtOutletNameBangla").val();
                //var outletAddressBangla = $("#txtOutletAddressBangla").val();
                //var outletPropritorNameBangla = $("#txtPropritorNameBangla").val();

                $("#txtOutletPhone").val(phone);
                $("#txtOutletEmailAddress").val(email);

                $("#ddlCountry").val(country);
                $("#ddlDivision").val(divisionId);
                $("#ddlZone").val(zoneId);
                $("#ddlRoute").val(routeId);

                $("#ddlCategory").val(category);
                $("#ddlGrade").val(grade);

                $("#ddlFridge").val(fride);
                $("#ddlSignboard").val(signboard);
                $("#ddlRack").val(rack);
                $("#ddlStatus").val(status);

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

});


function loadDivision() {
    $("#ddlCountry").change(function () {
        var countryName = $("#ddlCountry option:selected").text();
        $.ajax({
            type: "POST",
            url: "operation.aspx/GetDivision",
            data: "{countryName:'" + countryName + "'}",
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
    });
}

function loadZone() {
    $("#ddlDivision").change(function () {
        var division = $("#ddlDivision").val();
        $.ajax({
            type: "POST",
            url: "operation.aspx/GetZone",
            data: "{division:'" + division + "'}",
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
    });
}
});