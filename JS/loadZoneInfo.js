
var $ = jQuery.noConflict();
$(function () {
     
        //--Load grid view---------------------
        $("#tblData").html("");
        $("#tblData").html("<div style='width:545px;'><span id='loader' style='float:left'><img src='images/ajax-loader.gif' alt='Loading...' style='width:300px;height:17px;margin-top:10px; margin-left:40%; display:block;'/></span><span id='spPercentage' style='float:right;color:#2eaccc;display:block;margin-top:10px;'></span></div>");

        var tbl = "<table id='example' class='display' style='color:#9C27B0' cellspacing='0' width='100%'style='padding-top:10px;'>";
        var thead = "<thead><tr><th>Zone ID</th><th>Zone Name</th><th>Division Name</th><th>Country Name</th><th>Status</th><th>Operaton</th></tr></thead>";

        var row = "";
        var zone = "zone";
        $.ajax({
            type: "POST",
            url: "operation.aspx/GetZoneInfo",
            data: "{zone:'" + zone + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {

                var jsonData = JSON.parse(response.d);

                for (var i = 0; i < jsonData.length; i++)
                {
                    var zoneId = jsonData[i].ZONE_ID;
                    var zoneName = jsonData[i].ZONE_NAME;
                    var divName = jsonData[i].DIVISION_NAME;
                    var country = jsonData[i].COUNTRY_NAME;
                    var status = jsonData[i].STATUS;

                    row = row + "<tr style='text-align:left'><td>" + zoneId + "</td><td>" + zoneName + "</td><td>" + divName + "</td><td>" + country + "</td><td>" + status + "</td><td><div style='float:right;'><button type='button' style='float: right; margin-top: 0px;' class='btn btn-xs btn-success editzoneid' id='" + zoneId + "'><span class ='glyphicon glyphicon-edit'></span>Edit</button></div></td></tr>";
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


        $('#tblData').on('click', '.editzoneid', function () {

            var zone = this.id;
            //alert(srID);

            $.ajax({
                type: "POST",
                url: "operation.aspx/GetZoneInfo",
                data: "{zone:'" + zone + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {
                    //var zoneInfo = response.d.split(';');
                    var jsonData = JSON.parse(response.d);

                    var zoneId = jsonData[0].ZONE_ID;
                    var zoneName = jsonData[0].ZONE_NAME;
                    var divId = jsonData[0].DIVISION_ID;
                    var divName = jsonData[0].DIVISION_NAME;
                    var countryID = jsonData[0].COUNTRY_ID;
                    var country = jsonData[0].COUNTRY_NAME;
                    var status = jsonData[0].STATUS;                

                    
                    $.ajax({
                        type: "POST",
                        url: "operation.aspx/GetDivision",
                        data: "{countryID:'" + countryID + "'}",
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

                    $("#txtZoneId").val(zoneId)
                    $('#txtZoneId').prop('readonly', true);
                    $("#txtZoneName").val(zoneName);
                    $("#ddlCountry").val(countryID);
                    $("#ddlDivision").val(divId);
                    $("#ddlZoneActive").val(status);

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