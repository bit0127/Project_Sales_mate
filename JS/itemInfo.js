

var $ = jQuery.noConflict();
$(function () {

    //--Load grid view---------------------
    $("#tblData").html("");
    $("#tblData").html("<div style='width:545px;'><span id='loader' style='float:left'><img src='images/ajax-loader.gif' alt='Loading...' style='width:300px;height:17px;margin-top:10px; margin-left:40%; display:block;'/></span><span id='spPercentage' style='float:right;color:#2eaccc;display:block;margin-top:10px;'></span></div>");

    var tbl = "<table id='example' class='display' style='color:#9C27B0' cellspacing='0' width='100%'style='padding-top:10px;'>";
    var thead = "<thead><tr><th>Item ID</th><th>Item Name</th><th>Item Short Name</th><th>Item Bangla Name</th><th>Class Name</th><th>Item Group</th><th>Mother Company</th><th>Item Company</th><th>Category</th><th>Factor</th><th>DP</th><th>TP</th><th>MRP</th><th>WS</th><th>VAT</th><th>Active</th><th>Operaton</th></tr></thead>";

    var row = "";
    var itemId = "itemId";
    var ownCompany = $("#ddlOwnCompany").val();
    $.ajax({
        type: "POST",
        url: "operation.aspx/GetItemInfo",
        data: "{itemId:'" + itemId + "',ownCompany:'" + ownCompany + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var itemInfo = response.d.split(';');

            for (var i = 1; i < itemInfo.length; i = i + 19) {
                var itemId = itemInfo[i];
                var itemName = itemInfo[i + 1];
                var itemShortName = itemInfo[i + 2];
                var itemBanglaName = itemInfo[i + 3];

                var classId = itemInfo[i + 4];
                var className = itemInfo[i + 5];
                var groupId = itemInfo[i + 6];
                var groupName = itemInfo[i + 7];

                var companyId = itemInfo[i + 8];
                var companyName = itemInfo[i + 9];
                var motherCom = itemInfo[i + 10];

                var cat = itemInfo[i + 11];
                var factor = itemInfo[i + 12];
                var dp = itemInfo[i + 13];
                var tp = itemInfo[i + 14];
                var mrp = itemInfo[i + 15];
                var ws = itemInfo[i + 16];
                var vat = itemInfo[i + 17];
                var status = itemInfo[i + 18];
                 

                row = row + "<tr style='text-align:left'><td>" + itemId + "</td><td>" + itemName + "</td><td>" + itemShortName + "</td><td>" + itemBanglaName + "</td><td>" + className + "</td><td>" + groupName + "</td><td>" + motherCom + "</td><td>" + companyName + "</td><td>" + cat + "</td><td>" + factor + "</td><td>" + dp + "</td><td>" + tp + "</td><td>" + mrp + "</td><td>" + ws + "</td><td>" + vat + "</td><td>" + status + "</td><td><div style='float:right;'><button type='button' style='float: right; margin-top: 0px;' class='btn btn-xs btn-success edititemid' id='" + itemId + "'><span class ='glyphicon glyphicon-edit'></span>Edit</button></div></td></tr>";
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


    $('#tblData').on('click', '.edititemid', function () {

        var itemId = this.id;
        var ownCompany = "com";

        $.ajax({
            type: "POST",
            url: "operation.aspx/GetItemInfo",
            data: "{itemId:'" + itemId + "',ownCompany:'" + ownCompany + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                var itemInfo = response.d.split(';');

                var itemId = itemInfo[1];
                var itemName = itemInfo[2];
                var itemShortName = itemInfo[3];
                var itemBanglaName = itemInfo[4];

                var classId = itemInfo[5];
                var className = itemInfo[6];
                var groupId = itemInfo[7];
                var groupName = itemInfo[8];

                var companyId = itemInfo[9];
                var companyName = itemInfo[10];
                var motherCom = itemInfo[11];

                var cat = itemInfo[12];
                var factor = itemInfo[13];
                var dp = itemInfo[14];
                var tp = itemInfo[15];
                var mrp = itemInfo[16];
                var ws = itemInfo[17];
                var vat = itemInfo[18];
                var status = itemInfo[19];

                $.ajax({
                    type: "POST",
                    url: "operation.aspx/GetCompany",
                    data: "{motherCompany:'" + motherCom + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (response) {

                        //if (respond.d == "NotExist") {
                        //alert(response.d);
                        // }
                        //else {
                        var companyInfo = response.d.split(';');
                        var opt = "<option value='-1'>...Select...</option>";
                        for (var i = 1; i < companyInfo.length; i = i + 2) {
                            var comId = companyInfo[i];
                            var comName = companyInfo[i + 1];
                            opt = opt + "<option value='" + comId + "'>" + comName + "</option>";
                        }
                        $("#ddlOwnCompany").html('');
                        $("#ddlOwnCompany").append(opt);
                        //}                             
                    },
                    failure: function (response) {
                        alert(response.d);
                    }
                });

                $.ajax({
                    type: "POST",
                    url: "operation.aspx/GetItemGroup",
                    data: "{ownCompany:'" + companyId + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (response) {

                        //if (respond.d == "NotExist") {
                        //alert(response.d);
                        // }
                        //else {
                        var companyInfo = response.d.split(';');
                        var opt = "<option value='-1'>...Select...</option>";
                        for (var i = 1; i < companyInfo.length; i = i + 2) {
                            var comId = companyInfo[i];
                            var comName = companyInfo[i + 1];
                            opt = opt + "<option value='" + comId + "'>" + comName + "</option>";
                        }
                        $("#ddlItemGroup").html('');
                        $("#ddlItemGroup").append(opt);
                        //}                             
                    },
                    failure: function (response) {
                        alert(response.d);
                    }
                });

                $.ajax({
                    type: "POST",
                    url: "operation.aspx/GetItemClass",
                    data: "{itemGroup:'" + groupId + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (response) {

                        var companyInfo = response.d.split(';');
                        var opt = "<option value='-1'>...Select...</option>";
                        for (var i = 1; i < companyInfo.length; i = i + 2) {
                            var comId = companyInfo[i];
                            var comName = companyInfo[i + 1];
                            opt = opt + "<option value='" + comId + "'>" + comName + "</option>";
                        }
                        $("#ddlItemClass").html('');
                        $("#ddlItemClass").append(opt);

                    },
                    failure: function (response) {
                        alert(response.d);
                    }
                });

                $("#txtItemCode").val(itemId);
                $("#txtItemName").val(itemName);
                $("#txtItemShortName").val(itemShortName);
                $("#txtItemNameBangla").val(itemBanglaName);
                $("#ddlMotherCompany").val(motherCom);
                $("#ddlOwnCompany").val(companyId);
                $("#ddlItemGroup").val(groupId);
                $("#ddlItemClass").val(classId);
                $("#ddlFactorCategory").val(cat);
                $("#txtFactor").val(factor);
                $("#txtMRP").val(mrp);
                $("#txtWS").val(ws);
                $("#txtDP").val(dp);
                $("#txtTP").val(tp);
                $("#txtVat").val(vat);
                $("#ddlActive").val(status);


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