

var $ = jQuery.noConflict();
$(function () {

    //--Load grid view---------------------
    $("#tblData").html("");
    $("#tblData").html("<div style='width:545px;'><span id='loader' style='float:left'><img src='images/ajax-loader.gif' alt='Loading...' style='width:300px;height:17px;margin-top:10px; margin-left:40%; display:block;'/></span><span id='spPercentage' style='float:right;color:#2eaccc;display:block;margin-top:10px;'></span></div>");

    var tbl = "<table id='example' class='display' style='color:#9C27B0' cellspacing='0' width='100%'style='padding-top:10px;'>";
    var thead = "<thead><tr><th>HOS ID</th><th>Target Amount</th><th>Month</th><th>Year</th><th>Operaton</th></tr></thead>";

    var row = "";
    var hosId = "hosId";
    var month = "month";
    var year = "year";

    $.ajax({
        type: "POST",
        url: "operation.aspx/GetTSMTargetInfo",
        data: "{hosId:'" + hosId + "',month:'" + month + "',year:'" + year + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var hosTargetInfo = response.d.split(';');

            for (var i = 1; i < hosTargetInfo.length; i = i + 4) {
                var hosId = hosTargetInfo[i];
                var amount = hosTargetInfo[i + 1];
                var month = hosTargetInfo[i + 2];
                var year = hosTargetInfo[i + 3];


                row = row + "<tr style='text-align:left'><td>" + hosId + "</td><td>" + amount + "</td><td>" + month + "</td><td>" + year + "</td><td><div style='float:right;'><button type='button' style='float: right; margin-top: 0px;' class='btn btn-xs btn-success edittsmtid' id='" + hosId + "'><span class ='glyphicon glyphicon-edit'></span>Edit</button></div></td></tr>";
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


    $('#tblData').on('click', '.edittsmtid', function () {

        var hosId = this.id;
        var month = $(this).parents('tr').find('td:eq(2)').html();
        var year = $(this).parents('tr').find('td:eq(3)').html();
        //alert(month + "--" + year);

        $.ajax({
            type: "POST",
            url: "operation.aspx/GetTSMTargetInfo",
            data: "{hosId:'" + hosId + "',month:'" + month + "',year:'" + year + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                var hosTargetInfo = response.d.split(';');

                var hosId = hosTargetInfo[1];
                var amount = hosTargetInfo[2];
                var month = hosTargetInfo[3];
                var year = hosTargetInfo[4];


                $("#txtTSMId").val(hosId);
                $("#txtTargetAmount").val(amount);
                $("#ddlMonth").val(month);
                $("#ddlYear").val(year);


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