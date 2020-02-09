var $ = jQuery.noConflict();
$(function () {

    $("#tblData").html("");

    loadTSMSRInfo();

    $('#dvMiddleContent').html('');
    $('#dvMiddleContent').html("<div style='width:70%;height:36px;margin-top: 5px;background-color:#8dc060;text-align:center;color:#fff;font-size:25px;padding-top:10px;margin-top:10px;margin-left:15%;'><span>TSM & SR Mapping</span></div>" +
                       "<div style='width:69.8%;height:auto;margin-top: 2px;border:1px solid #8dc060;padding-top:10px;margin-left:15%;'><table style='padding-top:10px;padding-bottom: 10px;padding-left:25%'>" +
                       "<tr><td>Item Group :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlItemGroup' name='ddlItemGroup'></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       //"<tr><td>Country Name :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlCountry' name='ddlCountry'><option value='-1'>...Select...</option><option value='Bangladesh'>Bangladesh</option><option value='India'>India</option><option value='Nepal'>Nepal</option><option value='Malaysia'>Malaysia</option><option value='Oman'>Oman</option><option value='UAE'>UAE</option></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>Division :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlDivision' name='ddlDivision'></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>Zone :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlZone' name='ddlZone'></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>TSM ID :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlTSMZM' name='ddlTSM'></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>SR ID :</td><td><input type='text' id='txtSRId' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td></td><td style='text-align:right;'> <button type='button' id='btnSave' style='background-color: #8dc060;border-color: #4cae4c;color: #fff;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;cursor: pointer;display: inline-block;font-size: 14px;font-weight: 400;line-height: 1.42857;margin-bottom: 0;padding: 6px 12px;text-align: center;vertical-align: middle;white-space: nowrap;'>Save</button> </td></tr></table></div>");

    $("#txtTSMId").focus();
    $j.getScript("JS/loadGroup.js");
    $j.getScript("JS/loadDivisionByCompany.js");
    $j.getScript("JS/loadZone.js");

    $("#btnSave").click(function () {

        var tsmId = $("#txtTSMId").val();
        var srId = $("#txtSRId").val();

        if (tsmId == "") {
            alert('Enter DM ID');
            return;
        }
        else if (srId == "") {
            alert('Enter SR ID');
            return;
        }

        $.ajax({
            type: "POST",
            url: "operation.aspx/AddTSMSRMapping",
            data: "{tsmId:'" + tsmId + "',srId:'" + srId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                var msg = response.d;

                alert(msg);

                loadTSMSRInfo();

                $("#txtSRId").val('');
                $("#txtSRId").focus();

            },
            failure: function (response) {
                alert(response.d);
            }
        });



    });

    function loadTSMSRInfo() {
        //--Load grid view---------------------
        $("#tblData").html("");
        $("#tblData").html("<div style='width:545px;'><span id='loader' style='float:left'><img src='images/ajax-loader.gif' alt='Loading...' style='width:300px;height:17px;margin-top:10px; margin-left:40%; display:block;'/></span><span id='spPercentage' style='float:right;color:#2eaccc;display:block;margin-top:10px;'></span></div>");

        var tbl = "<table id='example' class='display' style='color:#9C27B0' cellspacing='0' width='100%'style='padding-top:10px;'>";
        var thead = "<thead><tr><th>TSM ID</th><th>TSM Name</th><th>TSM NUMBER</th><th>SR ID</th><th>SR Name</th><th>SR NUMBER</th><th>Edit</th></tr></thead>";

        var row = "";
        var sr = "sr";
        $.ajax({
            type: "POST",
            url: "operation.aspx/GetTSMSRInfo",
            data: "{srId:'" + sr + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                var DSRInfo = response.d.split(';');

                for (var i = 1; i < DSRInfo.length; i = i + 6) {
                    var TSMId = DSRInfo[i];
                    var TSMName = DSRInfo[i + 1];
                    var TSMNUMBER = DSRInfo[i + 2];
                    var SR_ID = DSRInfo[i + 3];
                    var SR_NAME = DSRInfo[i + 4];
                    var SR_NUMBER = DSRInfo[i + 5];

                    row = row + "<tr style='text-align:left'><td>" + TSMId + "</td><td>" + TSMName + "</td><td>" + TSMNUMBER + "</td><td>" + SR_ID + "</td><td>" + SR_NAME + "</td><td>" + SR_NUMBER + "</td><td><div style='float:right;'><button type='button' style='float: right; margin-top: 0px;' class='btn btn-xs btn-success editdsrid' id='" + SR_ID + "'><span class ='glyphicon glyphicon-edit'></span>Edit</button></div></td></tr>";
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

            var srID = this.id;
            //alert(srID);

            $.ajax({
                type: "POST",
                url: "operation.aspx/GetTSMSRInfo",
                data: "{srId:'" + srID + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {
                    var DSRInfo = response.d.split(';');

                    var TSMId = DSRInfo[1];
                    var TSMName = DSRInfo[2];
                    var TSMNUMBER = DSRInfo[3];
                    var SR_ID = DSRInfo[4];
                    var SR_NAME = DSRInfo[5];
                    var SR_NUMBER = DSRInfo[6];


                    $("#txtTSMId").val(TSMId);
                    $("#txtSRId").val(SR_ID);


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

        //$('#tblData').on('click', '.deletesrid', function () {

        //    var srID = this.id;
        //    //alert(srID);

        //    $.ajax({
        //        type: "POST",
        //        url: "operation.aspx/DeleteDMSRInfo",
        //        data: "{srId:'" + srID + "'}",
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        async: false,
        //        success: function (response) {

        //            alert(response.d);
        //            //loadDMSRInformation();
        //            //var table = $('#example').DataTable({
        //            //    lengthChange: true,
        //            //    "scrollX": true,
        //            //    buttons: true
        //            //});

        //            //table.buttons().container().insertBefore('#example_filter');


        //        },
        //        failure: function (response) {
        //            alert(response.d);
        //        }
        //    });

        //});

        //--end grid view panel---------------------------------------------------
    }


});