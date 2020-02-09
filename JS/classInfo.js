

var $ = jQuery.noConflict();
$(function () {

    //--Load grid view---------------------
    $("#tblData").html("");
    $("#tblData").html("<div style='width:545px;'><span id='loader' style='float:left'><img src='images/ajax-loader.gif' alt='Loading...' style='width:300px;height:17px;margin-top:10px; margin-left:40%; display:block;'/></span><span id='spPercentage' style='float:right;color:#2eaccc;display:block;margin-top:10px;'></span></div>");

    var tbl = "<table id='example' class='display' style='color:#9C27B0' cellspacing='0' width='100%'style='padding-top:10px;'>";
    var thead = "<thead><tr><th>Class ID</th><th>Class Name</th><th>Item Group</th><th>Mother Company</th><th>Item Company</th><th>Active</th><th>Operaton</th></tr></thead>";

    var row = "";
    var classId = "classId";
    $.ajax({
        type: "POST",
        url: "operation.aspx/GetClassInfo",
        data: "{classId:'" + classId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var classInfo = response.d.split(';');

            for (var i = 1; i < classInfo.length; i = i + 8) {
                var classId = classInfo[i];
                var className = classInfo[i + 1];
                var groupId = classInfo[i + 2];
                var groupName = classInfo[i + 3];
                var companyId = classInfo[i + 4];
                var companyName = classInfo[i + 5];
                var motherCom = classInfo[i + 6];
                var status = classInfo[i + 7];

                row = row + "<tr style='text-align:left'><td>" + classId + "</td><td>" + className + "</td><td>" + groupName + "</td><td>" + motherCom + "</td><td>" + companyName + "</td><td>" + status + "</td><td><div style='float:right;'><button type='button' style='float: right; margin-top: 0px;' class='btn btn-xs btn-success editclassid' id='" + classId + "'><span class ='glyphicon glyphicon-edit'></span>Edit</button></div></td></tr>";
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


    $('#tblData').on('click', '.editclassid', function () {

        var classId = this.id;
        //alert(srID);

        $.ajax({
            type: "POST",
            url: "operation.aspx/GetClassInfo",
            data: "{classId:'" + classId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                var classInfo = response.d.split(';');

                var classId = classInfo[1];
                var className = classInfo[2];
                var groupId = classInfo[3];
                var groupName = classInfo[4];
                var companyId = classInfo[5];
                var companyName = classInfo[6];
                var motherCom = classInfo[7];
                var status = classInfo[8];

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

                $("#txtClassId").val(classId);
                $("#txtClassName").val(className);
                $("#ddlMotherCompany").val(motherCom);
                $("#ddlOwnCompany").val(companyId);
                $("#ddlItemGroup").val(groupId);
                $("#ddlClassActive").val(status);


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