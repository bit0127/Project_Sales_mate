
var $ = jQuery.noConflict();
$(function () {
    //--Load grid view---------------------
    $("#tblData").html("");
    $("#tblData").html("<div style='width:545px;'><span id='loader' style='float:left'><img src='images/ajax-loader.gif' alt='Loading...' style='width:300px;height:17px;margin-top:10px; margin-left:40%; display:block;'/></span><span id='spPercentage' style='float:right;color:#2eaccc;display:block;margin-top:10px;'></span></div>");

    var tbl = "<table id='example' class='display' style='color:#9C27B0' cellspacing='0' width='100%'style='padding-top:10px;'>";
    var thead = "<thead><tr><th>Dist ID</th><th>Name</th><th>Warehouse Code</th><th>Warehouse Name</th><th>Contact Person</th><th>Mobile No</th><th>Address</th><th>Email Address</th><th>Country</th><th>Division</th><th>Zone</th><th>Status</th><th>Operaton</th></tr></thead>";

    var row = "";
    var whCode = "whCode";
    $.ajax({
        type: "POST",
        url: "operation.aspx/GetWarehouseInfo",
        data: "{whCode:'" + whCode + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var whInfo = response.d.split(';');

            for (var i = 1; i < whInfo.length; i = i + 14) {
                var distId = whInfo[i];
                var distName = whInfo[i + 1];
                var whCode = whInfo[i + 2];
                var whName = whInfo[i + 3];
                var cotact = whInfo[i + 4];
                var phone = whInfo[i + 5];
                var address = whInfo[i + 6];
                var email = whInfo[i + 7];
                var country = whInfo[i + 8];
                var divisionID = whInfo[i + 9];
                var divisionName = whInfo[i + 10];
                var zoneID = whInfo[i + 11];
                var zoneName = whInfo[i + 12];
                var status = whInfo[i + 13];


                row = row + "<tr style='text-align:left'><td>" + distId + "</td><td>" + distName + "</td><td>" + whCode + "</td><td>" + whName + "</td><td>" + cotact + "</td><td>" + phone + "</td><td>" + address + "</td><td>" + email + "</td><td>" + country + "</td><td>" + divisionName + "</td><td>" + zoneName + "</td><td>" + status + "</td><td><div style='float:right;'><button type='button' style='float: right; margin-top: 0px;' class='btn btn-xs btn-success editwhCodeid' id='" + whCode + "'><span class ='glyphicon glyphicon-edit'></span>Edit</button></div></td></tr>";
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


    $('#tblData').on('click', '.editwhCodeid', function () {

        var whCode = this.id;

        $.ajax({
            type: "POST",
            url: "operation.aspx/GetWarehouseInfo",
            data: "{whCode:'" + whCode + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                var whInfo = response.d.split(';');

                var distId = whInfo[1];
                var distName = whInfo[2];
                var whCode = whInfo[3];
                var whName = whInfo[4];
                var contact = whInfo[5];
                var phone = whInfo[6];
                var address = whInfo[7];
                var email = whInfo[8];
                var country = whInfo[9];
                var divisionID = whInfo[10];
                var divisionName = whInfo[11];
                var zoneID = whInfo[i + 12];
                var zoneName = whInfo[13];
                var status = whInfo[14];

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
                    data: "{division:'" + divisionID + "'}",
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

                $("#txtDistId").val(distId);
                $("#txtDistName").val(distName);
                $("#txtWHCode").val(whCode);
                $("#txtWHName").val(whName);
                $("#txtContactName").val(contact);
                $("#txtAddress").val(address);
                $("#txtPhone").val(phone);
                $("#txtEmail").val(email);
                
                $("#ddlCountry").val(country);
                $("#ddlDivision").val(divisionID);
                $("#ddlZone").val(zoneID);                
                $("#ddlDistActive").val(status);

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