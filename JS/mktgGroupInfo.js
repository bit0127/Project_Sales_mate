

var $ = jQuery.noConflict();
$(function () {

    //--Load grid view---------------------
    $("#tblData").html("");
    $("#tblData").html("<div style='width:545px;'><span id='loader' style='float:left'><img src='images/ajax-loader.gif' alt='Loading...' style='width:300px;height:17px;margin-top:10px; margin-left:40%; display:block;'/></span><span id='spPercentage' style='float:right;color:#2eaccc;display:block;margin-top:10px;'></span></div>");

    var tbl = "<table id='example' class='display' style='color:#9C27B0' cellspacing='0' width='100%'style='padding-top:10px;'>";
    var thead = "<thead><tr><th>Group ID</th><th>Group Name</th><th>Active</th><th>Operaton</th></tr></thead>";

    var row = "";
    var groupId = "groupId";
    $.ajax({
        type: "POST",
        url: "trademktg.aspx/GetGroupInfo",
        data: "{groupId:'" + groupId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var companyInfo = response.d.split(';');

            for (var i = 1; i < companyInfo.length; i = i + 3) {
                var groupId = companyInfo[i];
                var groupName = companyInfo[i + 1];                 
                var status = companyInfo[i + 2];
               

                row = row + "<tr style='text-align:left'><td>" + groupId + "</td><td>" + groupName + "</td><td>" + status + "</td><td><div style='float:right;'><button type='button' style='float: right; margin-top: 0px;' class='btn btn-xs btn-success editgroupId' id='" + groupId + "'><span class ='glyphicon glyphicon-edit'></span>Edit</button></div></td></tr>";
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


    $('#tblData').on('click', '.editgroupId', function () {

        var groupId = this.id;
        //alert(srID);

        $.ajax({
            type: "POST",
            url: "trademktg.aspx/GetGroupInfo",
            data: "{groupId:'" + groupId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                var companyInfo = response.d.split(';');

                var groupId = companyInfo[1];
                var groupName = companyInfo[2];
                var status = companyInfo[3];

                $("#txtGroupId").val(groupId);
                $("#txtGroupName").val(groupName);
                $("#ddlComActive").val(status);
                

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