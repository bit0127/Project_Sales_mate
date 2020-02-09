

var $ = jQuery.noConflict();
$(function () {

    //--Load grid view---------------------
    $("#tblData").html("");
    $("#tblData").html("<div style='width:545px;'><span id='loader' style='float:left'><img src='images/ajax-loader.gif' alt='Loading...' style='width:300px;height:17px;margin-top:10px; margin-left:40%; display:block;'/></span><span id='spPercentage' style='float:right;color:#2eaccc;display:block;margin-top:10px;'></span></div>");

    var tbl = "<table id='example' class='display' style='color:#9C27B0' cellspacing='0' width='100%'style='padding-top:10px;'>";
    var thead = "<thead><tr><th>Group ID</th><th>Group Name</th><th>Mother Company</th><th>Item Company</th><th>Active</th><th>Operaton</th></tr></thead>";

    var row = "";
    var groupId = "groupId";
    $.ajax({
        type: "POST",
        url: "operation.aspx/GetGroupInfo",
        data: "{groupId:'" + groupId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var groupInfo = response.d.split(';');

            for (var i = 1; i < groupInfo.length; i = i + 6) {
                var groupId = groupInfo[i];
                var groupName = groupInfo[i + 1];
                var companyId = groupInfo[i + 2];
                var companyName = groupInfo[i + 3];
                var motherCom = groupInfo[i + 4];
                var status = groupInfo[i + 5];

                row = row + "<tr style='text-align:left'><td>" + groupId + "</td><td>" + groupName + "</td><td>" + motherCom + "</td><td>" + companyName + "</td><td>" + status + "</td><td><div style='float:right;'><button type='button' style='float: right; margin-top: 0px;' class='btn btn-xs btn-success editgroupid' id='" + groupId + "'><span class ='glyphicon glyphicon-edit'></span>Edit</button></div></td></tr>";
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


    $('#tblData').on('click', '.editgroupid', function () {

        var groupId = this.id;
        //alert(srID);

        $.ajax({
            type: "POST",
            url: "operation.aspx/GetGroupInfo",
            data: "{groupId:'" + groupId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                var groupInfo = response.d.split(';');

                var groupId = groupInfo[1];
                var groupName = groupInfo[2];
                var companyId = groupInfo[3];
                var companyName = groupInfo[4];
                var motherCom = groupInfo[5];
                var status = groupInfo[6];

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

                $("#txtGroupId").val(groupId);
                $("#txtGroupName").val(groupName);
                $("#ddlMotherCompany").val(motherCom);
                $("#ddlOwnCompany").val(companyId);
                $("#ddlGroupActive").val(status);

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