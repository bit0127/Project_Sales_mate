

var $ = jQuery.noConflict();
$(function () {


    //$.getScript("JS/loadDepotStock.js");
    function loadStockItems() {
        //--Load grid view---------------------
        $("#tblData").html("");
        $("#tblData").html("<div style='width:545px;'><span id='loader' style='float:left'><img src='images/ajax-loader.gif' alt='Loading...' style='width:300px;height:17px;margin-top:10px; margin-left:40%; display:block;'/></span><span id='spPercentage' style='float:right;color:#2eaccc;display:block;margin-top:10px;'></span></div>");

        var tbl = "<table id='example' class='display' style='color:#9C27B0' cellspacing='0' width='100%'style='padding-top:10px;'>";
        var thead = "<thead><tr><th>OC Number</th><th>Item ID</th><th>Item Name</th><th>Item Group</th><th>Carton</th><th>Piece</th><th>Depot Name</th><th>Country Name</th><th>Arrival Date</th><th>Status</th><th>Operaton</th></tr></thead>";

        var row = "";
        var itemId =  "itemId";
        var ocNum = $("#txtOCNumber").val();
        $.ajax({
            type: "POST",
            url: "operation.aspx/GetItemStock",
            data: "{itemId:'" + itemId + "',ocNum:'" + ocNum + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                var classInfo = response.d.split(';');

                for (var i = 1; i < classInfo.length; i = i + 12) {
                    var ocNumber = classInfo[i];
                    var itemId = classInfo[i + 1];
                    var itemName = classInfo[i + 2];
                    var groupId = classInfo[i + 3];
                    var groupName = classInfo[i + 4];
                    var carton = classInfo[i + 5];
                    var piece = classInfo[i + 6];
                    var depotId = classInfo[i + 7];
                    var depotName = classInfo[i + 8];
                    var country = classInfo[i + 9];
                    var arrivalDate = classInfo[i + 10];
                    var status = classInfo[i + 11];

                    row = row + "<tr style='text-align:left'><td>" + ocNumber + "</td><td>" + itemId + "</td><td>" + itemName + "</td><td>" + groupName + "</td><td>" + carton + "</td><td>" + piece + "</td><td>" + depotName + "</td><td>" + country + "</td><td>" + arrivalDate + "</td><td>" + status + "</td><td><div style='float:right;'><button type='button' style='float: right; margin-top: 0px;' class='btn btn-xs btn-success edititemid' id='" + itemId + "," + ocNumber + "'><span class ='glyphicon glyphicon-edit'></span>Edit</button></div></td></tr>";
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

            var itemIdOC = this.id;

            var arrayOC = itemIdOC.split(",");
            var itemId = arrayOC[0];
            var ocNum = arrayOC[1];
            //alert(srID);

            $.ajax({
                type: "POST",
                url: "operation.aspx/GetItemStock",
                data: "{itemId:'" + itemId + "',ocNum:'" + ocNum + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {
                    var classInfo = response.d.split(';');

                    var ocNumber = classInfo[1];
                    var itemId = classInfo[2];
                    var itemName = classInfo[3];
                    var groupId = classInfo[4];
                    var groupName = classInfo[5];
                    var carton = classInfo[6];
                    var piece = classInfo[7];
                    var depotId = classInfo[8];
                    var depotName = classInfo[9];
                    var country = classInfo[10];
                    var arrivalDate = classInfo[11];
                    var status = classInfo[12];

                     
                    $("#txtOCNumber").val(ocNumber);
                    $("#ddlCountry").val(country);
                    $("#ddlDepot").val(depotId);
                    $("#ddlItemGroup").val(groupId);
                    $("#ddlItem").val(itemId);
                    $("#txtCarton").val(carton);
                    $("#txtPiece").val(piece);
                    $("#txtArrivalDate").val(arrivalDate);
                    $("#ddlStockActive").val(status);


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

    $("#tblData").html("");
    //$("html, body").animate({ scrollTop: $(document).height() - 20 }, "slow");
    $('#dvMiddleContent').html('');
    $('#dvMiddleContent').html("<div style='width:70%;height:36px;margin-top: 5px;background-color:#8dc060;text-align:center;color:#fff;font-size:25px;padding-top:10px;margin-top:10px;margin-left:15%;'><span>Depot/Distributor Product Stock Entry</span></div>" +
                       "<div style='width:69.8%;height:auto;margin-top: 2px;border:1px solid #8dc060;padding-top:10px;margin-left:15%;'><table style='padding-top:10px;padding-bottom: 10px;padding-left:25%'>" +
                       "<tr><td>OC Number :</td><td><input type='text' id='txtOCNumber' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>Country Name :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlCountry' name='ddlCountry'></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>Depot/Dist Name :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlDepot' name='ddlDepot'></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>Item Group :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlItemGroup' name='ddlItemGroup'></select></td><td></td></tr>" +
                       "<tr><td>Item Name :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlItem' name='ddlItem'></select></td><td></td></tr>" +
                       "<tr><td>Carton/Box etc. :</td><td><input type='text' id='txtCarton' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>Piece :</td><td><input type='text' id='txtPiece' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>Arrival Date :</td><td><input type='text' id='txtArrivalDate' placeholder='DD/MM/YYYY' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>Active :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlStockActive' name='ddlStockActive'><option value='-1'>...Select...</option><option value='Y'>Yes</option><option value='N'>No</option></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td></td><td style='text-align:right;'> <button type='button' id='btnSave' style='background-color: #8dc060;border-color: #4cae4c;color: #fff;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;cursor: pointer;display: inline-block;font-size: 14px;font-weight: 400;line-height: 1.42857;margin-bottom: 0;padding: 6px 12px;text-align: center;vertical-align: middle;white-space: nowrap;'>Save</button></td></tr></table></div>");

    $.getScript("JS/loadCountry.js");

    $("#ddlCountry").change(function (e) {

        var country = $("#ddlCountry").val();
        $.ajax({
            type: "POST",
            url: "operation.aspx/GetDepot",
            data: "{country:'" + country + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
               
                var distInfo = response.d.split(';');
                var opt = "<option value='-1'>...Select...</option>";
                for (var i = 1; i < distInfo.length; i = i + 2) {
                    var distId = distInfo[i];
                    var distName = distInfo[i + 1];
                    opt = opt + "<option value='" + distId + "'>" + distName + "</option>";
                }
                $("#ddlDepot").html('');
                $("#ddlDepot").append(opt);
                //}                             
            },
            failure: function (response) {
                alert(response.d);
            }
        });


        $.ajax({
            type: "POST",
            url: "operation.aspx/GetItemGroupByCountry",
            data: "{country:'" + country + "'}",
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

    });

    
    $("#ddlDepot").change(function (e) {
        var distId = $("#ddlDepot").val();
                    
    });


    $("#ddlItemGroup").change(function (e) {
        var groupId = $("#ddlItemGroup").val();
                    $.ajax({
                        type: "POST",
                        url: "operation.aspx/GetItemByGroup",
                        data: "{groupId:'" + groupId + "'}",
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
                                var itemId = companyInfo[i];
                                var itemName = companyInfo[i + 1];
                                opt = opt + "<option value='" + itemId + "'>" + itemId + " - " + itemName + "</option>";
                            }
                            $("#ddlItem").html('');
                            $("#ddlItem").append(opt);
                            //}                             
                        },
                        failure: function (response) {
                            alert(response.d);
                        }
                    });
    });


    $("#ddlItem").change(function (e) {
        var itemId = $("#ddlItem").val();
        //--Load grid view---------------------
        $("#tblData").html("");
        $("#tblData").html("<div style='width:545px;'><span id='loader' style='float:left'><img src='images/ajax-loader.gif' alt='Loading...' style='width:300px;height:17px;margin-top:10px; margin-left:40%; display:block;'/></span><span id='spPercentage' style='float:right;color:#2eaccc;display:block;margin-top:10px;'></span></div>");

        var tbl = "<table id='example' class='display' style='color:#9C27B0' cellspacing='0' width='100%'style='padding-top:10px;'>";
        var thead = "<thead><tr><th>OC Number</th><th>Item ID</th><th>Item Name</th><th>Item Group</th><th>Carton</th><th>Piece</th><th>Depot Name</th><th>Country Name</th><th>Arrival Date</th><th>Status</th><th>Operaton</th></tr></thead>";

        var row = "";
        var ocNum = "ocNum";
        $.ajax({
            type: "POST",
            url: "operation.aspx/GetItemStock",
            data: "{itemId:'" + itemId + "',ocNum:'" + ocNum + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                var classInfo = response.d.split(';');

                for (var i = 1; i < classInfo.length; i = i + 12) {
                    var ocNumber = classInfo[i];
                    var itemId = classInfo[i + 1];
                    var itemName = classInfo[i + 2];
                    var groupId = classInfo[i + 3];
                    var groupName = classInfo[i + 4];
                    var carton = classInfo[i + 5];
                    var piece = classInfo[i + 6];
                    var depotId = classInfo[i + 7];
                    var depotName = classInfo[i + 8];
                    var country = classInfo[i + 9];
                    var arrivalDate = classInfo[i + 10];
                    var status = classInfo[i + 11];

                    row = row + "<tr style='text-align:left'><td>" + ocNumber + "</td><td>" + itemId + "</td><td>" + itemName + "</td><td>" + groupName + "</td><td>" + carton + "</td><td>" + piece + "</td><td>" + depotName + "</td><td>" + country + "</td><td>" + arrivalDate + "</td><td>" + status + "</td><td><div style='float:right;'><button type='button' style='float: right; margin-top: 0px;' class='btn btn-xs btn-success edititemid' id='" + itemId + "," + ocNumber + "'><span class ='glyphicon glyphicon-edit'></span>Edit</button></div></td></tr>";
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

            var itemIdOC = this.id;

            var arrayOC = itemIdOC.split(",");
            var itemId = arrayOC[0];
            var ocNum = arrayOC[1];
            //alert(srID);

            $.ajax({
                type: "POST",
                url: "operation.aspx/GetItemStock",
                data: "{itemId:'" + itemId + "',ocNum:'" + ocNum + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {
                    var classInfo = response.d.split(';');

                    var ocNumber = classInfo[1];
                    var itemId = classInfo[2];
                    var itemName = classInfo[3];
                    var groupId = classInfo[4];
                    var groupName = classInfo[5];
                    var carton = classInfo[6];
                    var piece = classInfo[7];
                    var depotId = classInfo[8];
                    var depotName = classInfo[9];
                    var country = classInfo[10];
                    var arrivalDate = classInfo[11];
                    var status = classInfo[12];

                     
                    $("#txtOCNumber").val(ocNumber);
                    $("#ddlCountry").val(country);
                    $("#ddlDepot").val(depotId);
                    $("#ddlItemGroup").val(groupId);
                    $("#ddlItem").val(itemId);
                    $("#txtCarton").val(carton);
                    $("#txtPiece").val(piece);
                    $("#txtArrivalDate").val(arrivalDate);
                    $("#ddlStockActive").val(status);


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

    $("#txtPiece").val('0');
    var d = new Date();
    var month = d.getMonth() + 1;
    var day = d.getDate();     
    var currentDate = (day < 10 ? '0' : '') + day + '/' + (month < 10 ? '0' : '') + month + '/' + d.getFullYear();     
    $("#txtArrivalDate").val(currentDate);
    $j("#txtArrivalDate").datepicker({ showAnim: "fold", dateFormat: "dd/mm/yy" });

    $("#txtOCNumber").focus(); 

    $("#btnSave").click(function () {
               
        var ocNumber = $("#txtOCNumber").val();
        var country = $("#ddlCountry").val();
        var depotId = $("#ddlDepot").val();
        var depotName = $("#ddlDepot option:selected").text();
        var itemGroup = $("#ddlItemGroup").val();
        var groupName = $("#ddlItemGroup option:selected").text();
        var item = $("#ddlItem").val();
        var itemName = $("#ddlItem option:selected").text();
        var carton = $("#txtCarton").val();
        var piece = $("#txtPiece").val();
        var arrivalDate = $("#txtArrivalDate").val();
        var status = $("#ddlStockActive").val().trim();

        if (ocNumber == "") {
            alert('Enter OC Number');
            return;
        }
        else if (country == "-1") {
            alert('Select Country');
            return;
        }
        else if (depotId == "-1") {
            alert('Select Depot/Distributor Name');
            return;
        }       
        else if (itemGroup == "-1") {
            alert('Select Item Group');
            return;
        }
        else if (item == "-1") {
            alert('Select Item');
            return;
        }
        else if (carton == "") {
            alert('Enter Carton');
            return;
        }
        else if (piece == "") {
            alert('Enter Piece');
            return;
        }
        else if (arrivalDate == "") {
            alert('Enter Date');
            return;
        }
        else if (status == "-1") {
            alert('Select Activeness');
            return;
        }

        $.ajax({
            type: "POST",
            url: "operation.aspx/AddDepotProductBalance",
            data: "{ocNumber:'" + ocNumber + "',country:'" + country + "',depotId:'" + depotId + "',depotName:'" + depotName + "',itemGroupId:'" + itemGroup + "',groupName:'" + groupName + "',itemId:'" + item + "',itemName:'" + itemName + "',carton:'" + carton + "',piece:'" + piece + "',arrivalDate:'" + arrivalDate + "',status:'" + status + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                var msg = response.d;

                alert(msg);
                $("#txtCarton").val('');
                $("#txtPiece").val('0');

                loadStockItems();

            },
            failure: function (response) {
                alert(response.d);
            }
        });


    });

});