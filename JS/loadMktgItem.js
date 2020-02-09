

var $ = jQuery.noConflict();
$(function () {

    //--Load grid view---------------------
    $("#tblData").html("");
    $("#tblData").html("<div style='width:545px;'><span id='loader' style='float:left'><img src='images/ajax-loader.gif' alt='Loading...' style='width:300px;height:17px;margin-top:10px; margin-left:40%; display:block;'/></span><span id='spPercentage' style='float:right;color:#2eaccc;display:block;margin-top:10px;'></span></div>");

    var tbl = "<table id='example' class='display' style='color:#9C27B0' cellspacing='0' width='100%'style='padding-top:10px;'>";
    var thead = "<thead><tr><th>Item ID</th><th>Item Name</th><th>Item Price</th><th>Group Name</th><th>Company Name</th><th>Status</th><th>Operaton</th></tr></thead>";

    var row = "";
    var itemId = "itemId";
    $.ajax({
        type: "POST",
        url: "trademktg.aspx/GetItemInfo",
        data: "{itemId:'" + itemId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var itemInfo = response.d.split(';');

            for (var i = 1; i < itemInfo.length; i = i + 7) {
                var itemId = itemInfo[i];
                var itemName = itemInfo[i + 1];
                var price = itemInfo[i + 2];
                var groupName = itemInfo[i + 3];
                var company = itemInfo[i + 4];              
                var status = itemInfo[i + 5];
                var groupId = itemInfo[i + 6];

                row = row + "<tr style='text-align:left'><td>" + itemId + "</td><td>" + itemName + "</td><td>" + price + "</td><td>" + groupName + "</td><td>" + company + "</td><td>" + status + "</td><td><div style='float:right;'><button type='button' style='float: right; margin-top: 0px;' class='btn btn-xs btn-success edititemid' id='" + itemId + "'><span class ='glyphicon glyphicon-edit'></span>Edit</button></div></td></tr>";
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
        //alert(srID);

        $.ajax({
            type: "POST",
            url: "trademktg.aspx/GetItemInfo",
            data: "{itemId:'" + itemId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                var itemInfo = response.d.split(';');

                var itemId = itemInfo[1];
                var itemName = itemInfo[2];
                var price = itemInfo[3];
                var groupName = itemInfo[4];

                var company = itemInfo[5];
                var status = itemInfo[6];
                var groupId = itemInfo[7];
                  

                $("#txtItemCode").val(itemId);
                $("#txtItemName").val(itemName);
                $("#txtItemPrice").val(price);
                $("#ddlGroupName").val(groupId);
                $("#txtCompanyName").val(company);
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