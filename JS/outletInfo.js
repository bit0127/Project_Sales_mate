
var $ = jQuery.noConflict();
$(function () {

    //--Load grid view---------------------
    $("#tblData").html("");
    $("#tblData").html("<div style='width:545px;'><span id='loader' style='float:left'><img src='images/ajax-loader.gif' alt='Loading...' style='width:300px;height:17px;margin-top:10px; margin-left:40%; display:block;'/></span><span id='spPercentage' style='float:right;color:#2eaccc;display:block;margin-top:10px;'></span></div>");

    var tbl = "<table id='example' class='display' style='color:#9C27B0' cellspacing='0' width='100%'style='padding-top:10px;'>";
    var thead = "<thead><tr><th>Outlet ID</th><th>Outlet Name</th><th>Outlet Address</th><th>Propritor Name</th><th>Phone Number</th><th>Email Address</th><th>Country</th><th>Division</th><th>Zone</th><th>Route</th><th>Category</th><th>Grade</th><th>Fridge</th><th>Signboard</th><th>Rack/Unit</th><th>Status</th><th>Operaton</th></tr></thead>";

    var row = "";
    var outlet = "outlet";
    $.ajax({
        type: "POST",
        url: "operation.aspx/GetOutletInfo",
        data: "{outlet:'" + outlet + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
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

    //--end grid view panel---------------------------------------------------

});