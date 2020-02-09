
var $ = jQuery.noConflict();
$(function () {
    var tranId = "tranId";

    $("#tblData").html("");
    $('#dvMiddleContent').html('');
    $('#dvMiddleContent').html("<div style='width:70%;height:36px;margin-top: 5px;background-color:#8dc060;text-align:center;color:#fff;font-size:25px;padding-top:10px;margin-top:10px;margin-left:15%;'><span>Order Correction panel</span></div>" +
                       "<div style='width:69.8%;height:auto;margin-top: 2px;border:1px solid #8dc060;padding-top:10px;margin-left:15%;'>" +
                          "<table style='padding-top:10px;padding-bottom: 10px;padding-left:25%'>" +
                            "<tr><td>Order Date :</td><td><input type='text' id='txtOrderDate' placeholder='DD/MM/YYYY' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                            
                            "<tr><td>Country Name :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlCountry' name='ddlCountry'></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                            "<tr><td>Company Name :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlCompany' name='ddlCompany'></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                            "<tr><td>Group Name :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlGroup' name='ddlGroup'></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +

                            "<tr><td>SR Name :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlSRName' name='ddlSRName'></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                            "<tr><td>Outlet :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlOutlet' name='ddlOutlet'></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                            "<tr><td>Item Name :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlItem' name='ddlItem'></select></td><td></td></tr>" +
                            "<tr><td>Carton :</td><td><input type='text' id='txtCarton' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>*</span></td></tr>"+
                            "<tr><td>Piece :</td><td><input type='text' id='txtPiece' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>*</span></td></tr>" +

                            "<tr><td></td><td style='text-align:right;'><button type='button' id='btnSave' style='background-color: #8dc060;border-color: #4cae4c;color: #fff;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;cursor: pointer;display: inline-block;font-size: 14px;font-weight: 400;line-height: 1.42857;margin-bottom: 0;padding: 6px 12px;text-align: center;vertical-align: middle;white-space: nowrap;'>Update</button></td></tr>" +
                          "</table>" +
                        "</div>");

       
    var d = new Date();
    var month = d.getMonth() + 1;
    var day = d.getDate();    
    var currentDate = (day < 10 ? '0' : '') + day + '/' + (month < 10 ? '0' : '') + month + '/' + d.getFullYear();     
    $("#txtOrderDate").val(currentDate);
    $j("#txtOrderDate").datepicker({ showAnim: "fold", dateFormat: "dd/mm/yy" });

    $.getScript("JS/loadCountry.js");
        
    $("#ddlCountry").change(function (e) {
        var countryName = $("#ddlCountry").val();
        $.ajax({
            type: "POST",
            url: "operation.aspx/GetCompanyByCountry",
            data: "{countryName:'" + countryName + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {

                var outletInfo = response.d.split(';');
                var opt = "<option value='-1'>...Select...</option>";
                for (var i = 1; i < outletInfo.length; i = i + 2) {
                    var companyId = outletInfo[i];
                    var companyName = outletInfo[i + 1];
                    opt = opt + "<option value='" + companyId + "'>" + companyName + "</option>";
                }
                $("#ddlCompany").html('');
                $("#ddlCompany").append(opt);

            },
            failure: function (response) {
                alert(response.d);
            }
        });
    });

    $("#ddlCompany").change(function (e) {
        var ownCompany = $("#ddlCompany").val();        
        $.ajax({
            type: "POST",
            url: "operation.aspx/GetCompanyGroup",
            data: "{ownCompany:'" + ownCompany + "'}",
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
                $("#ddlGroup").html('');
                $("#ddlGroup").append(opt);

            },
            failure: function (response) {
                alert(response.d);
            }
        });
        

    });

    $("#ddlGroup").change(function (e) {
        var groupId = $("#ddlGroup").val();
        var cDate = $("#txtOrderDate").val();
        $.ajax({
            type: "POST",
            url: "operation.aspx/GetSRByGroup",
            data: "{orderDate:'" + cDate + "',groupId:'" + groupId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {

                var outletInfo = response.d.split(';');
                var opt = "<option value='-1'>...Select...</option>";
                for (var i = 1; i < outletInfo.length; i = i + 2) {
                    var srId = outletInfo[i];
                    var srName = outletInfo[i + 1];
                    opt = opt + "<option value='" + srId + "'>" + srId + "-" + srName + "</option>";
                }
                $("#ddlSRName").html('');
                $("#ddlSRName").append(opt);

            },
            failure: function (response) {
                alert(response.d);
            }
        });
    });

    $("#ddlSRName").change(function (e) {
        var srId = $("#ddlSRName").val();
        var cDate = $("#txtOrderDate").val();
        $.ajax({
            type: "POST",
            url: "operation.aspx/GetOutletBySR",
            data: "{orderDate:'" + cDate + "',srId:'" + srId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {

                var outletInfo = response.d.split(';');
                var opt = "<option value='-1'>...Select...</option>";
                for (var i = 1; i < outletInfo.length; i = i + 2) {
                    var oltId = outletInfo[i];
                    var oltName = outletInfo[i + 1];
                    opt = opt + "<option value='" + oltId + "'>" + oltName + "</option>";
                }
                $("#ddlOutlet").html('');
                $("#ddlOutlet").append(opt);

            },
            failure: function (response) {
                alert(response.d);
            }
        });
    });

    $("#ddlOutlet").change(function (e) {
                
        $("#txtCarton").val('');
        $("#txtPiece").val('');

        var outletId = $("#ddlOutlet").val();
        var cDate = $("#txtOrderDate").val();
        var groupId = $("#ddlGroup").val();
        var srId = $("#ddlSRName").val();
        $.ajax({
            type: "POST",
            url: "operation.aspx/GetItemByOutlet",
            data: "{orderDate:'" + cDate + "',srId:'" + srId + "',groupId:'" + groupId + "',outletId:'" + outletId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {

                var itemInfo = response.d.split(';');
                var opt = "<option value='-1'>...Select...</option>";
                for (var i = 1; i < itemInfo.length; i = i + 2) {
                    var itemId = itemInfo[i];
                    var itemName = itemInfo[i + 1];
                    opt = opt + "<option value='" + itemId + "'>" + itemId + " - " + itemName + "</option>";
                }
                $("#ddlItem").html('');
                $("#ddlItem").append(opt);

            },
            failure: function (response) {
                alert(response.d);
            }
        });


        //--load data---------------------------------------------------------------------
        $("#tblData").html("");
        $("#tblData").html("<div style='width:545px;'><span id='loader' style='float:left'><img src='images/ajax-loader.gif' alt='Loading...' style='width:300px;height:17px;margin-top:10px; margin-left:40%; display:block;'/></span><span id='spPercentage' style='float:right;color:#2eaccc;display:block;margin-top:10px;'></span></div>");

        var tbl = "<table id='example' class='display' style='color:#9C27B0' cellspacing='0' width='100%'style='padding-top:10px;'>";
        var thead = "<thead><tr><th>Transaction ID</th><th>Order Date</th><th>Item ID</th><th>Item Name</th><th>Carton</th><th>Piece</th><th>Factor</th><th>Out Price</th><th>Outlet Name</th></tr></thead>";

        var row = "";
        var outletId = $("#ddlOutlet").val();
        var orderDate = $("#txtOrderDate").val();

        $.ajax({
            type: "POST",
            url: "operation.aspx/GetOrderedItemByOutletSR",
            data: "{srid:'" + srId + "',outletId:'" + outletId + "',orderDate:'" + cDate + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                var orderInfo = response.d.split(';');

                for (var i = 1; i < orderInfo.length; i = i + 10) {
                    var orderDate = orderInfo[i];
                    var itemId = orderInfo[i + 1];
                    var itemName = orderInfo[i + 2];
                    var cartonQty = orderInfo[i + 3];
                    var pieceQty = orderInfo[i + 4];
                    var factor = orderInfo[i + 5];
                    var price = orderInfo[i + 6];
                    var outletName = orderInfo[i + 7];
                    var outletId = orderInfo[i + 8];
                    var tranId = orderInfo[i + 9];

                    row = row + "<tr style='text-align:left'><td>" + tranId + "</td><td>" + orderDate + "</td><td>" + itemId + "</td><td>" + itemName + "</td><td>" + cartonQty + "</td><td>" + pieceQty + "</td><td>" + factor + "</td><td>" + price + "</td><td>" + outletName + "</td></tr>";
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
        //-----------------------------------------------------------------------------------

    });

    $("#ddlItem").change(function (e) {

        var outletId = $("#ddlOutlet").val();
        var cDate = $("#txtOrderDate").val();
        var groupId = $("#ddlGroup").val();
        var srId = $("#ddlSRName").val();
        var itemId = $("#ddlItem").val();
        $.ajax({
            type: "POST",
            url: "operation.aspx/GetItemCtnByOutlet",
            data: "{orderDate:'" + cDate + "',srId:'" + srId + "',groupId:'" + groupId + "',outletId:'" + outletId + "',itemId:'" + itemId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                var itemInfo = response.d.split(';');                
                var itemPcs = itemInfo[1];
                var itemCtn = itemInfo[2];
                $("#txtCarton").val(itemCtn);
                $("#txtPiece").val(itemPcs);                
            },
            failure: function (response) {
                alert(response.d);
            }
        });
    });
     
    $("#btnSave").click(function () {

        var srid = $("#ddlSRName").val();         
        var outeltId = $("#ddlOutlet").val();
        var itemId = $("#ddlItem").val();
        var itemCtn = $("#txtCarton").val();
        var itemPcs = $("#txtPiece").val();         
        var orderDate = $("#txtOrderDate").val();

        if (srid == "") {
            alert('Enter SR Id');
            return;
        }         
        else if (outeltId == "-1") {
            alert('Select Outelt');
            return;
        }
        else if (itemCtn == "") {
            alert('Enter Carton');
            return;
        }
        else if (itemPcs == "") {
            alert('Enter Piece');
            return;
        }         
        else if (orderDate == "") {
            alert('Enter Order Date');
            return;
        } 

        $.ajax({
            type: "POST",
            url: "operation.aspx/UpdateOrder",
            data: "{srid:'" + srid + "',outeltId:'" + outeltId + "',itemId:'" + itemId + "',itemCtn:'" + itemCtn + "',itemPcs:'" + itemPcs + "',orderDate:'" + orderDate + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                var msg = response.d;

                alert(msg);
                //--load data---------------------------------------------------------------------
                $("#tblData").html("");
                $("#tblData").html("<div style='width:545px;'><span id='loader' style='float:left'><img src='images/ajax-loader.gif' alt='Loading...' style='width:300px;height:17px;margin-top:10px; margin-left:40%; display:block;'/></span><span id='spPercentage' style='float:right;color:#2eaccc;display:block;margin-top:10px;'></span></div>");

                var tbl = "<table id='example' class='display' style='color:#9C27B0' cellspacing='0' width='100%'style='padding-top:10px;'>";
                var thead = "<thead><tr><th>Transaction ID</th><th>Order Date</th><th>Item ID</th><th>Item Name</th><th>Carton</th><th>Piece</th><th>Factor</th><th>Out Price</th><th>Outlet Name</th></tr></thead>";

                var row = "";
                var outletId = $("#ddlOutlet").val();
                var orderDate = $("#txtOrderDate").val();

                $.ajax({
                    type: "POST",
                    url: "operation.aspx/GetOrderedItemByOutletSR",
                    data: "{srid:'" + srid + "',outletId:'" + outeltId + "',orderDate:'" + orderDate + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (response) {
                        var orderInfo = response.d.split(';');

                        for (var i = 1; i < orderInfo.length; i = i + 10) {
                            var orderDate = orderInfo[i];
                            var itemId = orderInfo[i + 1];
                            var itemName = orderInfo[i + 2];
                            var cartonQty = orderInfo[i + 3];
                            var pieceQty = orderInfo[i + 4];
                            var factor = orderInfo[i + 5];
                            var price = orderInfo[i + 6];
                            var outletName = orderInfo[i + 7];
                            var outletId = orderInfo[i + 8];
                            var tranId = orderInfo[i + 9];

                            row = row + "<tr style='text-align:left'><td>" + tranId + "</td><td>" + orderDate + "</td><td>" + itemId + "</td><td>" + itemName + "</td><td>" + cartonQty + "</td><td>" + pieceQty + "</td><td>" + factor + "</td><td>" + price + "</td><td>" + outletName + "</td></tr>";
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
                //-----------------------------------------------------------------------------------
            },
            failure: function (response) {
                alert(response.d);
            }
        });

    });
});