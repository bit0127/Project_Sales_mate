var $ = jQuery.noConflict();
$(function () {

    loadDMInfo();

    //$("html, body").animate({ scrollTop: $(document).height() - 20 }, "slow");
    $('#dvMiddleContent').html('');
    $('#dvMiddleContent').html("<div style='width:70%;height:36px;margin-top: 5px;background-color:#8dc060;text-align:center;color:#fff;font-size:25px;padding-top:10px;margin-top:10px;margin-left:15%;'><span>DM Information Entry panel</span></div>" +
                       "<div style='width:69.8%;height:auto;margin-top: 2px;border:1px solid #8dc060;padding-top:10px;margin-left:15%;'><table style='padding-top:10px;padding-bottom: 10px;padding-left:25%'>" +
                       "<tr><td>DM ID :</td><td><input type='text' id='txtId' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>DM Name :</td><td><input type='text' id='txtName' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>Password :</td><td><input type='text' id='txtPassword' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>Mobile Number :</td><td><input type='text' id='txtPhone' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>Email Address :</td><td><input type='text' id='txtEmail' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>Distributor ID :</td><td><input type='text' id='txtDistId' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>Country Name :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlCountry' name='ddlCountry'><option value='-1'>...Select...</option><option value='Bangladesh'>Bangladesh</option><option value='India'>India</option><option value='Nepal'>Nepal</option><option value='Malaysia'>Malaysia</option><option value='Oman'>Oman</option><option value='UAE'>UAE</option></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>Division :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlDivision' name='ddlDivision'></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>Zone :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlZone' name='ddlZone'></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>Active :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlDSRActive' name='ddlDSRActive'><option value='-1'>...Select...</option><option value='Y'>Yes</option><option value='N'>No</option></select></td><td></td></tr>" +
                       "<tr><td></td><td style='text-align:right;'> <button type='button' id='btnSave' style='background-color: #8dc060;border-color: #4cae4c;color: #fff;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;cursor: pointer;display: inline-block;font-size: 14px;font-weight: 400;line-height: 1.42857;margin-bottom: 0;padding: 6px 12px;text-align: center;vertical-align: middle;white-space: nowrap;'>Save</button> </td></tr></table></div>");

    $("#txtId").focus();

    //loadDivision();
    $.getScript("JS/loadDivision.js");
    //loadZone();
    $.getScript("JS/loadZone.js");

    $("#btnSave").click(function () {

        

        var dmId = $("#txtId").val();
        var dmName = $("#txtName").val();
        var pwd = $("#txtPassword").val();
        var Phone = $("#txtPhone").val();
        var emailAddress = $("#txtEmail").val();
        var distId = $("#txtDistId").val();
        var country = $("#ddlCountry").val();
        var division = $("#ddlDivision").val();
        var divisionName = $("#ddlDivision option:selected").text();
        var zone = $("#ddlZone").val();
        var zoneName = $("#ddlZone option:selected").text();
        var status = $("#ddlDSRActive").val();

        if (dmId == "") {
            alert('Enter DM ID');
            return;
        }
        else if (dmName == "") {
            alert('Enter DM Name');
            return;
        }
        else if (pwd == "") {
            alert('Enter Password');
            return;
        }        
        else if (Phone == "") {
            alert('Enter Mobile Number');
            return;
        }
        else if (emailAddress == "") {
            alert('Enter Email Address');
            return;
        }
        else if (distId == "") {
            alert('Enter Distributor ID');
            return;
        }
        else if (country == "-1") {
            alert('Select Country');
            return;
        }
        else if (division == "-1") {
            alert('Select Division');
            return;
        }
        else if (zone == "-1") {
            alert('Select Zone');
            return;
        }
        else if (status == "-1") {
            alert('Select Activeness');
            return;
        }
         
        $.ajax({
            type: "POST",
            url: "operation.aspx/AddDMInfo",
            data: "{dmId:'" + dmId + "',dmName:'" + dmName + "',pwd:'" + pwd + "',distId:'" + distId + "',Phone:'" + Phone + "',emailAddress:'" + emailAddress + "',status:'" + status + "',country:'" + country + "',division:'" + division + "',zone:'" + zone + "',divisionName:'" + divisionName + "',zoneName:'" + zoneName + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                var msg = response.d;

                alert(msg);

                $("#txtId").val('');
                $("#txtName").val('');
                $("#txtPassword").val('');
                $("#txtDistId").val('');
                $("#txtPhone").val('');
                $("#txtEmail").val('');

                loadDMInfo();

            },
            failure: function (response) {
                alert(response.d);
            }
        });

    });


    function loadDMInfo() {
        //--Load grid view---------------------
        $("#tblData").html("");
        $("#tblData").html("<div style='width:545px;'><span id='loader' style='float:left'><img src='images/ajax-loader.gif' alt='Loading...' style='width:300px;height:17px;margin-top:10px; margin-left:40%; display:block;'/></span><span id='spPercentage' style='float:right;color:#2eaccc;display:block;margin-top:10px;'></span></div>");

        var tbl = "<table id='example' class='display' style='color:#9C27B0' cellspacing='0' width='100%'style='padding-top:10px;'>";
        var thead = "<thead><tr><th>DM ID</th><th>DM Name</th><th>Password</th><th>Mobile No</th><th>Email Address</th><th>Dist ID</th><th>Zone</th><th>Division</th><th>Country</th><th>Status</th><th>Operaton</th></tr></thead>";

        var row = "";
        var dm = "dm";
        $.ajax({
            type: "POST",
            url: "operation.aspx/GetDMInfo",
            data: "{dm:'" + dm + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                var DSRInfo = response.d.split(';');

                for (var i = 1; i < DSRInfo.length; i = i + 12) {
                    var DSRId = DSRInfo[i];
                    var DSRName = DSRInfo[i + 1];
                    var PWD = DSRInfo[i + 2];
                    var MOBILE = DSRInfo[i + 3];
                    var EMAIL = DSRInfo[i + 4];
                    var DISTID = DSRInfo[i + 5];
                    var STATUS = DSRInfo[i + 6];

                    var COUNTRY_NAME = DSRInfo[i + 7];
                    var DIVISION_ID = DSRInfo[i + 8];
                    var DIVISION_NAME = DSRInfo[i + 9];
                    var ZONE_ID = DSRInfo[i + 10];
                    var ZONE_NAME = DSRInfo[i + 11];


                    row = row + "<tr style='text-align:left'><td>" + DSRId + "</td><td>" + DSRName + "</td><td>" + PWD + "</td><td>" + MOBILE + "</td><td>" + EMAIL + "</td><td>" + DISTID + "</td><td>" + ZONE_NAME + "</td><td>" + DIVISION_NAME + "</td><td>" + COUNTRY_NAME + "</td><td>" + STATUS + "</td><td><div style='float:right;'><button type='button' style='float: right; margin-top: 0px;' class='btn btn-xs btn-success editdsrid' id='" + DSRId + "'><span class ='glyphicon glyphicon-edit'></span>Edit</button></div></td></tr>";
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


        $('#tblData').on('click', '.editdsrid', function () {

            var dmID = this.id;
            //alert(srID);

            $.ajax({
                type: "POST",
                url: "operation.aspx/GetDMInfo",
                data: "{dm:'" + dmID + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {
                    var DSRInfo = response.d.split(';');

                    var DSRId = DSRInfo[1];
                    var DSRName = DSRInfo[2];
                    var PWD = DSRInfo[3];
                    var MOBILE = DSRInfo[4];
                    var EMAIL = DSRInfo[5];
                    var DISTID = DSRInfo[6];
                    var STATUS = DSRInfo[7];

                    var COUNTRY_NAME = DSRInfo[8];
                    var DIVISION_ID = DSRInfo[9];
                    var DIVISION_NAME = DSRInfo[10];
                    var ZONE_ID = DSRInfo[11];
                    var ZONE_NAME = DSRInfo[12];

                    $.ajax({
                        type: "POST",
                        url: "operation.aspx/GetDivision",
                        data: "{countryName:'" + COUNTRY_NAME + "'}",
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
                        data: "{division:'" + DIVISION_ID + "'}",
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

                    var country = $("#ddlCountry").val(COUNTRY_NAME);
                    var division = $("#ddlDivision").val(DIVISION_ID);
                    var zone = $("#ddlZone").val(ZONE_ID);

                    $("#txtId").val(DSRId);
                    $("#txtName").val(DSRName);
                    $("#txtPassword").val(PWD);
                    $("#txtPhone").val(MOBILE);
                    $("#txtEmail").val(EMAIL);
                    $("#txtDistId").val(DISTID);
                    $("#ddlDSRActive").val(STATUS);

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
    }

});