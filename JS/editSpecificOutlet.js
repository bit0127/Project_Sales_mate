
var $ = jQuery.noConflict();
$(function () {

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
                var products = outletInfo[20];
                var srId = outletInfo[21];

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

                $("#txtSRID").val(srId);
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
                $("#txtProduct").val(products);

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