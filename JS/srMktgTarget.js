

var $ = jQuery.noConflict();
$(function () {

    //--Load grid view---------------------
    $("#tblData").html("");
    $("#tblData").html("<div style='width:545px;'><span id='loader' style='float:left'><img src='images/ajax-loader.gif' alt='Loading...' style='width:300px;height:17px;margin-top:10px; margin-left:40%; display:block;'/></span><span id='spPercentage' style='float:right;color:#2eaccc;display:block;margin-top:10px;'></span></div>");

    var tbl = "<table id='example' class='display' style='color:#9C27B0' cellspacing='0' width='100%'style='padding-top:10px;'>";
    var thead = "<thead><tr><th>SR ID</th><th>SR Name</th><th>Group</th><th>Country</th><th>Division</th><th>Region</th><th>Zone</th><th>Supervisor</th><th>Target Amount</th><th>Operating Cost</th><th>POP Cost</th><th>Promo Cost</th><th>Month</th><th>Year</th><th>Operaton</th></tr></thead>";

    var row = "";
    var srId = "srId";
    var monthName = "monthName";
    $.ajax({
        type: "POST",
        url: "trademktg.aspx/GetSRTargetInfo",
        data: "{srId:'" + srId + "',monthName:'" + monthName + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var hosTargetInfo = response.d.split(';');
             
            for (var i = 1; i < hosTargetInfo.length; i = i + 23) {
                var SR_ID = hosTargetInfo[i];
                var SR_NAME = hosTargetInfo[i + 1];
                var GROUP_ID = hosTargetInfo[i + 2];
                var GROUP_NAME = hosTargetInfo[i + 3];
                var COUNTRY = hosTargetInfo[i + 4];
                var DIVISION_ID = hosTargetInfo[i + 5];
                var DIVISION_NAME = hosTargetInfo[i + 6];
                var REGION_ID = hosTargetInfo[i + 7];
                var REGION_NAME = hosTargetInfo[i + 8];
                var ZONE_ID = hosTargetInfo[i + 9];
                var ZONE_NAME = hosTargetInfo[i + 10];
                var SUPERVISOR_ID = hosTargetInfo[i + 11];
                var SUPERVISOR_NAME = hosTargetInfo[i + 12];
                var TARGET_AMT = hosTargetInfo[i + 13];
                var OPERATING_COST = hosTargetInfo[i + 14];
                var POP_COST = hosTargetInfo[i + 15];
                var PROMO_COST = hosTargetInfo[i + 16];
                var MONTH_NAME = hosTargetInfo[i + 17];
                var YEAR_NAME = hosTargetInfo[i + 18];
                var OPT_ID = hosTargetInfo[i + 19];
                var OPT_NAME = hosTargetInfo[i + 20];
                var AREA_MNGR_ID = hosTargetInfo[i + 21];
                var AREA_MNGR_NAME = hosTargetInfo[i + 22];

                row = row + "<tr style='text-align:left'><td>" + SR_ID + "</td><td>" + SR_NAME + "</td><td>" + GROUP_NAME + "</td><td>" + COUNTRY + "</td><td>" + DIVISION_NAME + "</td><td>" + REGION_NAME + "</td><td>" + ZONE_NAME + "</td><td>" + SUPERVISOR_NAME + "</td><td>" + TARGET_AMT + "</td><td>" + OPERATING_COST + "</td><td>" + POP_COST + "</td><td>" + PROMO_COST + "</td><td>" + MONTH_NAME + "</td><td>" + YEAR_NAME + "</td><td><div style='float:right;'><button type='button' style='float: right; margin-top: 0px;' class='btn btn-xs btn-success editsrtid' id='" + SR_ID + "'><span class ='glyphicon glyphicon-edit'></span>Edit</button></div></td></tr>";
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


    $('#tblData').on('click', '.editsrtid', function () {

        var srId = this.id;
        var monthName = $(this).parents('tr').find('td:eq(12)').html();
        //var month = $(this).parents('tr').find('td:eq(2)').html();
        //var year = $(this).parents('tr').find('td:eq(3)').html();
        //alert(month + "--" + year);

        $.ajax({
            type: "POST",
            url: "trademktg.aspx/GetSRTargetInfo",
            data: "{srId:'" + srId + "',monthName:'" + monthName + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                var hosTargetInfo = response.d.split(';');
                
                var SR_ID = hosTargetInfo[1];
                var SR_NAME = hosTargetInfo[2];
                var GROUP_ID = hosTargetInfo[3];
                var GROUP_NAME = hosTargetInfo[4];
                var COUNTRY = hosTargetInfo[5];
                var DIVISION_ID = hosTargetInfo[6];
                var DIVISION_NAME = hosTargetInfo[7];
                var REGION_ID = hosTargetInfo[8];
                var REGION_NAME = hosTargetInfo[9];
                var ZONE_ID = hosTargetInfo[10];
                var ZONE_NAME = hosTargetInfo[11];
                var SUPERVISOR_ID = hosTargetInfo[12];
                var SUPERVISOR_NAME = hosTargetInfo[13];
                var TARGET_AMT = hosTargetInfo[14];
                var OPERATING_COST = hosTargetInfo[15];
                var POP_COST = hosTargetInfo[16];
                var PROMO_COST = hosTargetInfo[17];
                var MONTH_NAME = hosTargetInfo[18];
                var YEAR_NAME = hosTargetInfo[19];

                var OPT_ID = hosTargetInfo[20];
                var OPT_NAME = hosTargetInfo[21];
                var AREA_MNGR_ID = hosTargetInfo[22];
                var AREA_MNGR_NAME = hosTargetInfo[23];

                $("#ddlGroupName").val(GROUP_ID);
                $("#ddlCountry").val(COUNTRY);

                $.ajax({
                    type: "POST",
                    url: "trademktg.aspx/GetDivision",
                    data: "{countryName:'" + COUNTRY + "',group:'" + GROUP_ID + "'}",
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
                        $("#ddlDivision").html('');
                        $("#ddlDivision").append(opt);

                    },
                    failure: function (response) {
                        alert(response.d);
                    }
                });


                $("#ddlDivision").val(DIVISION_ID);

                $.ajax({
                    type: "POST",
                    url: "trademktg.aspx/GetRegionByDivision",
                    data: "{divisionId:'" + DIVISION_ID + "'}",
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

                $("#ddlRegion").val(REGION_ID);

                $.ajax({
                    type: "POST",
                    url: "trademktg.aspx/GetZoneInfoByRegion",
                    data: "{regionId:'" + REGION_ID + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (response) {

                        var companyInfo = response.d.split(';');
                        var opt = "<option value='-1'>...Select...</option>";
                        //opt = opt + "<option value='AllZone'>All Zone</option>";
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


                $("#ddlZone").val(ZONE_ID);

                $.ajax({
                    type: "POST",
                    url: "trademktg.aspx/GetGroupWiseOperationManager",
                    data: "{groupId:'" + GROUP_ID + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (response) {
                        var mngrInfo = response.d.split(';');
                        var opt = "<option value='-1'>...Select...</option>";
                        for (var i = 1; i < mngrInfo.length; i = i + 2) {
                            var mngrId = mngrInfo[i];
                            var mngrName = mngrInfo[i + 1];
                            opt = opt + "<option value='" + mngrId + "'>" + mngrId + "-" + mngrName + "</option>";
                        }
                        $("#ddlOperationMngr").html('');
                        $("#ddlOperationMngr").append(opt);

                    },
                    failure: function (response) {
                        alert(response.d);
                    }
                });

                $("#ddlOperationMngr").val(OPT_ID);

                $.ajax({
                    type: "POST",
                    url: "trademktg.aspx/GetAreaMngrByOptMngr",
                    data: "{opmId:'" + OPT_ID + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (response) {

                        var mngrInfo = response.d.split(';');
                        var opt = "<option value='-1'>...Select...</option>";
                        for (var i = 1; i < mngrInfo.length; i = i + 2) {
                            var mngrId = mngrInfo[i];
                            var mngrName = mngrInfo[i + 1];
                            opt = opt + "<option value='" + mngrId + "'>" + mngrId + "-" + mngrName + "</option>";
                        }
                        $("#ddlAreaMngr").html('');
                        $("#ddlAreaMngr").append(opt);

                    },
                    failure: function (response) {
                        alert(response.d);
                    }
                });

                $("#ddlAreaMngr").val(AREA_MNGR_ID);

                $.ajax({
                    type: "POST",
                    url: "trademktg.aspx/GetSupervisorByAreaMngr",
                    data: "{areaMngrId:'" + AREA_MNGR_ID + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (response) {

                        var mngrInfo = response.d.split(';');
                        var opt = "<option value='-1'>...Select...</option>";
                        for (var i = 1; i < mngrInfo.length; i = i + 2) {
                            var mngrId = mngrInfo[i];
                            var mngrName = mngrInfo[i + 1];
                            opt = opt + "<option value='" + mngrId + "'>" + mngrId + "-" + mngrName + "</option>";
                        }
                        $("#ddlSupervisor").html('');
                        $("#ddlSupervisor").append(opt);

                    },
                    failure: function (response) {
                        alert(response.d);
                    }
                });

                $("#ddlSupervisor").val(SUPERVISOR_ID);

                $.ajax({
                    type: "POST",
                    url: "trademktg.aspx/GetMarchandiserBySupervisor",
                    data: "{superId:'" + SUPERVISOR_ID + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (response) {

                        var mngrInfo = response.d.split(';');
                        var opt = "<option value='-1'>...Select...</option>";
                        for (var i = 1; i < mngrInfo.length; i = i + 2) {
                            var mngrId = mngrInfo[i];
                            var mngrName = mngrInfo[i + 1];
                            opt = opt + "<option value='" + mngrId + "'>" + mngrId + "-" + mngrName + "</option>";
                        }
                        $("#ddlMerchandiser").html('');
                        $("#ddlMerchandiser").append(opt);

                    },
                    failure: function (response) {
                        alert(response.d);
                    }
                });

                $("#ddlMerchandiser").val(SR_ID);
                
                

                $("#txtTargetAmount").val(TARGET_AMT);
                $("#txtOperatingCost").val(OPERATING_COST);
                $("#txtPOPCost").val(POP_COST);
                $("#txtPromoCost").val(PROMO_COST);

                $("#ddlMonth").val(MONTH_NAME);
                $("#ddlYear").val(YEAR_NAME);

                

                
                
                
                
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