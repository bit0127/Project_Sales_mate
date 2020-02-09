var $ = jQuery.noConflict();
$(function () {

    var srid = $("#txtSRId").val();
    var country = $("#ddlCountry").val();
    var division = $("#ddlDivision").val();
    var routeId = $("#ddlRoute").val();
    var outeltId = $("#ddlOutlet").val();
    var itemId = $("#ddlItem").val();
    var itemCtn = $("#txtCarton").val();
    var itemPcs = $("#txtPiece").val();
    var orderType = "PO";
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
    
    //--load data---------------------------------------------------------------------
    $("#tblData").html("");
    $("#tblData").html("<div style='width:545px;'><span id='loader' style='float:left'><img src='images/ajax-loader.gif' alt='Loading...' style='width:300px;height:17px;margin-top:10px; margin-left:40%; display:block;'/></span><span id='spPercentage' style='float:right;color:#2eaccc;display:block;margin-top:10px;'></span></div>");

    var tbl = "<table id='example' class='display' style='color:#9C27B0' cellspacing='0' width='100%'style='padding-top:10px;'>";
    var thead = "<thead><tr><th>Order Date</th><th>Item ID</th><th>Item Name</th><th>Carton</th><th>Piece</th><th>Factor</th><th>Out Price</th><th>Outlet Name</th><th>Operaton</th></tr></thead>";

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

            for (var i = 1; i < orderInfo.length; i = i + 9) {
                var orderDate = orderInfo[i];
                var itemId = orderInfo[i + 1];
                var itemName = orderInfo[i + 2];
                var cartonQty = orderInfo[i + 3];
                var pieceQty = orderInfo[i + 4];
                var factor = orderInfo[i + 5];
                var price = orderInfo[i + 6];
                var outletName = orderInfo[i + 7];
                var outletId = orderInfo[i + 8];

                row = row + "<tr style='text-align:left'><td>" + orderDate + "</td><td>" + itemId + "</td><td>" + itemName + "</td><td>" + cartonQty + "</td><td>" + pieceQty + "</td><td>" + factor + "</td><td>" + price + "</td><td>" + outletName + "</td><td><div style='float:right;'><button type='button' style='float: right; margin-top: 0px;' class='btn btn-xs btn-success edititemIdid' id='" + outletId + "'><span class ='glyphicon glyphicon-edit'></span>Edit</button></div></td></tr>";
            }

            tbl = tbl + thead + "<tbody>" + row + "</tbody></table>";

            //$("#loader").css("display", "none");
            $("#tblData").html("");
            $("#tblData").html(tbl);
            $("#tblData").append("<div style='margin-top:10px;float:right;'><button type='button' id='btnActiveAll' style='background-color: #8dc060;border-color: #4cae4c;color: #fff;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;cursor: pointer;display: inline-block;font-size: 14px;font-weight: 400;line-height: 1.42857;margin-bottom: 0;padding: 6px 12px;text-align: center;vertical-align: middle;white-space: nowrap;'>Active All</button></div>");

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

    $('#btnActiveAll').on('click', function () {
        alert('Active all');
    });

    $('#tblData').on('click', '.edititemIdid', function () {

        var outletId = this.id;
        var itemId = $(this).closest('tr').find('td:eq(1)').text();
         
        $.ajax({
            type: "POST",
            url: "operation.aspx/GetCOOInfo",
            data: "{coo:'" + outletId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                var cooInfo = response.d.split(';');

                var cooId = cooInfo[1];
                var cooName = cooInfo[2];
                var designation = cooInfo[3];
                var motherCom = cooInfo[4];
                var ownCom = cooInfo[5];
                var pass = cooInfo[6];
                var status = cooInfo[7];


                $.ajax({
                    type: "POST",
                    url: "operation.aspx/GetCompany",
                    data: "{motherCompany:'" + motherCom + "'}",
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
                        $("#ddlOwnCompany").html('');
                        $("#ddlOwnCompany").append(opt);

                    },
                    failure: function (response) {
                        alert(response.d);
                    }
                });

                $("#txtCOOId").val(cooId);
                $("#txtName").val(cooName);
                $("#txtDesignation").val(designation);
                $("#txtPassword").val(pass);
                $("#ddlMotherCompany option:selected").text(motherCom);
                $("#ddlOwnCompany option:selected").text(ownCom);
                $("#ddlActive").val(status);

            },
            failure: function (response) {
                alert(response.d);
            }
        });

    });

});