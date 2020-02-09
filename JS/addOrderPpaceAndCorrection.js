
var $ = jQuery.noConflict();
$(function () {
var tranId = "tranId";

$("#tblData").html("");
$('#dvMiddleContent').html('');
$('#dvMiddleContent').html("<div style='width:70%;height:36px;margin-top: 5px;background-color:#8dc060;text-align:center;color:#fff;font-size:25px;padding-top:10px;margin-top:10px;margin-left:15%;'><span>Order Informaiton Entry panel</span></div>" +
                   "<div style='width:69.8%;height:auto;margin-top: 2px;border:1px solid #8dc060;padding-top:10px;margin-left:15%;'>" +
                      "<table style='padding-top:10px;padding-bottom: 10px;padding-left:25%'>" +
                        "<tr><td>Order Date :</td><td><input type='text' id='txtOrderDate' placeholder='DD/MM/YYYY' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>*</span></td></tr>"+
                        "<tr><td>Order Type :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlOrderType' name='ddlOrderType'><option value='-1'>...Select...</option><option value='OC'>Pre Order</option><option value='DS'>Direct Sales</option><option value='SR'>Sales Return</option><option value='DM'>Damage Return</option></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                        "<tr><td>Country Name :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlCountry' name='ddlCountry'></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                        "<tr><td>Company Name :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlCompany' name='ddlCompany'></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                        
                        "<tr><td>Delivery Man :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlDeliveryMan' name='ddlDeliveryMan'></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +

                        "<tr><td>SR Name :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlSRName' name='ddlSRName'></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +                                              
                        "<tr><td>Division Name :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlDivision' name='ddlDivision'></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                        "<tr><td>Zone :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlZone' name='ddlZone'></select></td><td><span style='color:#ec407a;'>*</span></td></tr><tr><td>Route :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlRoute' name='ddlRoute'></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                        "<tr><td>Outlet :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlOutlet' name='ddlOutlet'></select></td><td><span style='color:#ec407a;'>*</span></td></tr><tr><td>Item Name :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlItem' name='ddlItem'></select></td><td></td></tr><tr><td>Carton :</td><td><input type='text' id='txtCarton' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>*</span></td></tr><tr><td>Piece :</td><td><input type='text' id='txtPiece' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                        "<tr><td>Price :</td><td><input type='text' id='txtPrice' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td></td></tr>" +
                        
                        "<tr><td>Delivery Date :</td><td><input type='text' id='txtDeliveryDate' placeholder='DD/MM/YYYY' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                        "<tr><td></td><td style='text-align:right;'><button type='button' id='btnSave' style='background-color: #8dc060;border-color: #4cae4c;color: #fff;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;cursor: pointer;display: inline-block;font-size: 14px;font-weight: 400;line-height: 1.42857;margin-bottom: 0;padding: 6px 12px;text-align: center;vertical-align: middle;white-space: nowrap;'>Add</button></td></tr>" +
                      "</table>" +
                    "</div>");


$("#txtSRId").focus();

var d = new Date();
var month = d.getMonth() + 1;
var day = d.getDate();
var dayd = d.getDate() + 2;
var currentDate = (day < 10 ? '0' : '') + day + '/' + (month < 10 ? '0' : '') + month + '/' + d.getFullYear();
var currentDated = (dayd < 10 ? '0' : '') + dayd + '/' + (month < 10 ? '0' : '') + month + '/' + d.getFullYear();
$("#txtOrderDate").val(currentDate);
$("#txtDeliveryDate").val(currentDate);
$j("#txtOrderDate").datepicker({ showAnim: "fold", dateFormat: "dd/mm/yy" });
$j("#txtDeliveryDate").datepicker({ showAnim: "fold", dateFormat: "dd/mm/yy" });

$.getScript("JS/loadCountry.js");

$.getScript("JS/loadDivision.js");
//loadZone();
$.getScript("JS/loadZone.js");


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

    var countryName = $("#ddlCountry").val();
    $.ajax({
        type: "POST",
        url: "operation.aspx/GetDMByCountry",
        data: "{countryName:'" + countryName + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {

            var outletInfo = response.d.split(';');
            var opt = "<option value='-1'>...Select...</option>";
            for (var i = 1; i < outletInfo.length; i = i + 2) {
                var dmId = outletInfo[i];
                var dmName = outletInfo[i + 1];
                opt = opt + "<option value='" + dmId + "'>" + dmId + "-" + dmName + "</option>";
            }
            $("#ddlDeliveryMan").html('');
            $("#ddlDeliveryMan").append(opt);

        },
        failure: function (response) {
            alert(response.d);
        }
    });

});

$("#ddlDeliveryMan").change(function (e) {
    var dmId = $("#ddlDeliveryMan").val();

    $.ajax({
        type: "POST",
        url: "operation.aspx/GetSRByDM",
        data: "{dmId:'" + dmId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {

            var outletInfo = response.d.split(';');
            var opt = "<option value='-1'>...Select...</option>";
            for (var i = 1; i < outletInfo.length; i = i + 2) {
                var itemId = outletInfo[i];
                var itemName = outletInfo[i + 1];
                opt = opt + "<option value='" + itemId + "'>" + itemId + "-" + itemName + "</option>";
            }
            $("#ddlSRName").html('');
            $("#ddlSRName").append(opt);

        },
        failure: function (response) {
            alert(response.d);
        }
    });
});

$("#ddlCompany").change(function (e) {
    var companyId = $("#ddlCompany").val();

    $.ajax({
        type: "POST",
        url: "operation.aspx/GetItemByCompany",
        data: "{companyId:'" + companyId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {

            var outletInfo = response.d.split(';');
            var opt = "<option value='-1'>...Select...</option>";
            for (var i = 1; i < outletInfo.length; i = i + 2) {
                var itemId = outletInfo[i];
                var itemName = outletInfo[i + 1];
                opt = opt + "<option value='" + itemId + "'>" + itemId + "-" + itemName + "</option>";
            }
            $("#ddlItem").html('');
            $("#ddlItem").append(opt);

        },
        failure: function (response) {
            alert(response.d);
        }
    });
});    

$("#ddlItem").change(function (e) {
    var itemId = $("#ddlItem").val();
    $.ajax({
        type: "POST",
        url: "operation.aspx/GetItemPrice",
        data: "{itemId:'" + itemId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {

            var price = response.d;
            $("#txtPrice").val(price);

        },
        failure: function (response) {
            alert(response.d);
        }
    });
});


$("#ddlZone").change(function () {
    var zoneId = $("#ddlZone").val();
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
});


$("#ddlRoute").change(function () {
    var routeId = $("#ddlRoute").val();
    $.ajax({
        type: "POST",
        url: "operation.aspx/GetRouteWiseForeignOutlet",
        data: "{routeId:'" + routeId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {

            var outletInfo = response.d.split(';');
            var opt = "<option value='-1'>...Select...</option>";
            for (var i = 1; i < outletInfo.length; i = i + 2) {
                var outletId = outletInfo[i];
                var outletName = outletInfo[i + 1];
                opt = opt + "<option value='" + outletId + "'>" + outletName + "</option>";
            }
            $("#ddlOutlet").html('');
            $("#ddlOutlet").append(opt);

        },
        failure: function (response) {
            alert(response.d);
        }
    });
});

$("#ddlOutlet").change(function () {
    //alert('ok boss');
    //$.getScript("JS/orderInfo.js");
    $("#btnSave").text('Add');

    var srid = $("#ddlSRName").val();
    var country = $("#ddlCountry").val();
    var division = $("#ddlDivision").val();
    var routeId = $("#ddlRoute").val();
    var outletId = $("#ddlOutlet").val();
    var itemId = $("#ddlItem").val();
    var itemCtn = $("#txtCarton").val();
    var itemPcs = $("#txtPiece").val();
    var orderType = $("#ddlOrderType").val();
    var orderDate = $("#txtOrderDate").val();

    if (srid == "-1") {
        alert('Select SR Name');
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
    else if (routeId == "-1") {
        alert('Select Route');
        return;
    }
    else if (outletId == "-1") {
        alert('Select Outelt');
        return;
    }

    //--load data---------------------------------------------------------------------
    $("#tblData").html("");
    $("#tblData").html("<div style='width:545px;'><span id='loader' style='float:left'><img src='images/ajax-loader.gif' alt='Loading...' style='width:300px;height:17px;margin-top:10px; margin-left:40%; display:block;'/></span><span id='spPercentage' style='float:right;color:#2eaccc;display:block;margin-top:10px;'></span></div>");

    var tbl = "<table id='example' class='display' style='color:#9C27B0' cellspacing='0' width='100%'style='padding-top:10px;'>";
    var thead = "<thead><tr><th>Transaction ID</th><th>Order Date</th><th>Item ID</th><th>Item Name</th><th>Carton</th><th>Piece</th><th>Factor</th><th>Out Price</th><th>Outlet Name</th><th>Operaton</th></tr></thead>";

    var row = "";

    $.ajax({
        type: "POST",
        url: "operation.aspx/GetOrderedItem",
        data: "{srid:'" + srid + "',routeId:'" + routeId + "',outletId:'" + outletId + "',orderDate:'" + orderDate + "',orderType:'" + orderType + "'}",
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

                row = row + "<tr style='text-align:left'><td>" + tranId + "</td><td>" + orderDate + "</td><td>" + itemId + "</td><td>" + itemName + "</td><td>" + cartonQty + "</td><td>" + pieceQty + "</td><td>" + factor + "</td><td>" + price + "</td><td>" + outletName + "</td><td><div style='float:right;'><button type='button' style='float: right; margin-top: 0px;' class='btn btn-xs btn-success edititemIdid' id='" + outletId + "'><span class ='glyphicon glyphicon-edit'></span>Edit</button></div></td></tr>";
            }

            tbl = tbl + thead + "<tbody>" + row + "</tbody></table>";

            //$("#loader").css("display", "none");
            $("#tblData").html("");
            $("#tblData").html(tbl);
            //$("#tblData").append("<div style='margin-top:10px;float:right;'><button type='button' id='btnActiveAll' style='background-color: #8dc060;border-color: #4cae4c;color: #fff;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;cursor: pointer;display: inline-block;font-size: 14px;font-weight: 400;line-height: 1.42857;margin-bottom: 0;padding: 6px 12px;text-align: center;vertical-align: middle;white-space: nowrap;'>Active All</button></div>");

            /*$('#btnActiveAll').on('click', function () {
                $.ajax({
                    type: "POST",
                    url: "operation.aspx/ActiveOrder",
                    data: "{srid:'" + srid + "',routeId:'" + routeId + "',outeltId:'" + outletId + "',orderType:'" + orderType + "',orderDate:'" + orderDate + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (response) {
                        alert(response.d);

                        $("#tblData").html('');

                    },
                    failure: function (response) {
                        alert(response.d);
                    }
                });
            });*/


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

    $('#tblData').on('click', '.edititemIdid', function () {

        $("#btnSave").text('Edit');

        var outletId = this.id;
        tranId = $(this).closest('tr').find('td:eq(0)').text();
        var orderDate = $(this).closest('tr').find('td:eq(1)').text();
        var itemId = $(this).closest('tr').find('td:eq(2)').text();
        var qtyCtn = $(this).closest('tr').find('td:eq(4)').text();
        var qty = $(this).closest('tr').find('td:eq(5)').text();
        var price = $(this).closest('tr').find('td:eq(7)').text();

        $("#ddlItem").val(itemId);
        $("#txtCarton").val(qtyCtn);
        $("#txtPiece").val(qty);
        $("#txtPrice").val(price);
        $("#ddlOrderType").val('OC');
        $("#txtOrderDate").val(orderDate);

        /*$.ajax({
            type: "POST",
            url: "operation.aspx/GetOrderItemInfo",
            data: "{itemId:'" + itemId + "',outletId:'" + outletId + "',orderDate:'" + orderDate + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                var itemInfo = response.d.split(';');

                var srId = itemInfo[1];
                var srName = itemInfo[2];
                var orderDate = itemInfo[3];
                var itemId = itemInfo[4];
                var itemName = itemInfo[5];
                var qtyCtn = itemInfo[6];
                var qty = itemInfo[7];
                var factor = itemInfo[8];
                var price = itemInfo[9];
                var outletName = itemInfo[10];
                var oltId = itemInfo[11];                                 

                $("#txtSRId").val(srId);
                $("#txtName").val(srName);                                 
                $("#ddlOutlet").val(oltId);
                $("#ddlItem").val(itemId);
                $("#txtCarton").val(qtyCtn);
                $("#txtPiece").val(qty);
                $("#txtPrice").val(price);
                $("#ddlOrderType").val('PO');
                $("#txtOrderDate").val(orderDate);

            },
            failure: function (response) {
                alert(response.d);
            }
        });*/

    });
});


$("#btnSave").click(function () {

    var srid = $("#ddlSRName").val();
    var country = $("#ddlCountry").val();
    var division = $("#ddlDivision").val();
    var routeId = $("#ddlRoute").val();
    var outeltId = $("#ddlOutlet").val();
    var itemId = $("#ddlItem").val();
    var itemCtn = $("#txtCarton").val();
    var itemPcs = $("#txtPiece").val();
    var orderType = $("#ddlOrderType").val();
    var orderDate = $("#txtOrderDate").val();

    if (srid == "") {
        alert('Enter SR Id');
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
    else if (routeId == "-1") {
        alert('Select Route');
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
    else if (orderType == "-1") {
        alert('Select Order Type');
        return;
    }
    else if (orderDate == "") {
        alert('Enter Order Date');
        return;
    }


    var opt = "Add";
    if ($("#btnSave").text() == "Add") {
        opt = opt;
    }
    else if ($("#btnSave").text() == "Edit") {
        opt = "Edit";
    }


    $.ajax({
        type: "POST",
        url: "operation.aspx/AddOrder",
        data: "{tranId:'" + tranId + "',opt:'" + opt + "',srid:'" + srid + "',routeId:'" + routeId + "',outeltId:'" + outeltId + "',itemId:'" + itemId + "',itemCtn:'" + itemCtn + "',itemPcs:'" + itemPcs + "',orderType:'" + orderType + "',orderDate:'" + orderDate + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var msg = response.d;

            alert(msg);

            $("#ddlOutlet").focus();

            //--load data---------------------------------------------------------------------
            $("#tblData").html("");
            $("#tblData").html("<div style='width:545px;'><span id='loader' style='float:left'><img src='images/ajax-loader.gif' alt='Loading...' style='width:300px;height:17px;margin-top:10px; margin-left:40%; display:block;'/></span><span id='spPercentage' style='float:right;color:#2eaccc;display:block;margin-top:10px;'></span></div>");

            var tbl = "<table id='example' class='display' style='color:#9C27B0' cellspacing='0' width='100%'style='padding-top:10px;'>";
            var thead = "<thead><tr><th>Transaction ID</th><th>Order Date</th><th>Item ID</th><th>Item Name</th><th>Carton</th><th>Piece</th><th>Factor</th><th>Out Price</th><th>Outlet Name</th><th>Operaton</th></tr></thead>";

            var row = "";
            var outletId = $("#ddlOutlet").val();
            var orderDate = $("#txtOrderDate").val();

            $.ajax({
                type: "POST",
                url: "operation.aspx/GetOrderedItem",
                data: "{srid:'" + srid + "',routeId:'" + routeId + "',outletId:'" + outletId + "',orderDate:'" + orderDate + "',orderType:'" + orderType + "'}",
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

                        row = row + "<tr style='text-align:left'><td>" + tranId + "</td><td>" + orderDate + "</td><td>" + itemId + "</td><td>" + itemName + "</td><td>" + cartonQty + "</td><td>" + pieceQty + "</td><td>" + factor + "</td><td>" + price + "</td><td>" + outletName + "</td><td><div style='float:right;'><button type='button' style='float: right; margin-top: 0px;' class='btn btn-xs btn-success edititemIdid' id='" + outletId + "'><span class ='glyphicon glyphicon-edit'></span>Edit</button></div></td></tr>";
                    }

                    tbl = tbl + thead + "<tbody>" + row + "</tbody></table>";

                    //$("#loader").css("display", "none");
                    $("#tblData").html("");
                    $("#tblData").html(tbl);
                    $("#tblData").append("<div style='margin-top:10px;float:right;'><button type='button' id='btnActiveAlls' style='background-color: #8dc060;border-color: #4cae4c;color: #fff;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;cursor: pointer;display: inline-block;font-size: 14px;font-weight: 400;line-height: 1.42857;margin-bottom: 0;padding: 6px 12px;text-align: center;vertical-align: middle;white-space: nowrap;'>Active All</button></div>");


                    $('#btnActiveAlls').on('click', function () {
                        $.ajax({
                            type: "POST",
                            url: "operation.aspx/ActiveOrder",
                            data: "{srid:'" + srid + "',routeId:'" + routeId + "',outeltId:'" + outeltId + "',orderType:'" + orderType + "',orderDate:'" + orderDate + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: function (response) {
                                alert(response.d);

                                $("#tblData").html('');

                            },
                            failure: function (response) {
                                alert(response.d);
                            }
                        });
                    });



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

        },
        failure: function (response) {
            alert(response.d);
        }
    });

});
});