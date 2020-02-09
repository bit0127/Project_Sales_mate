

var $ = jQuery.noConflict();
$(function () {

    //--Load grid view---------------------
    $("#tblData").html("");
    $("#tblData").html("<div style='width:545px;'><span id='loader' style='float:left'><img src='images/ajax-loader.gif' alt='Loading...' style='width:300px;height:17px;margin-top:10px; margin-left:40%; display:block;'/></span><span id='spPercentage' style='float:right;color:#2eaccc;display:block;margin-top:10px;'></span></div>");

    var tbl = "<table id='example' class='display' style='color:#9C27B0' cellspacing='0' width='100%'style='padding-top:10px;'>";
    var thead = "<thead><tr><th>Company ID</th><th>Company Name</th><th>Company Nick Name</th><th>Mother Company</th><th>Country Name</th><th>Active</th><th>Operaton</th></tr></thead>";

    var row = "";
    var comId = "comId";
    $.ajax({
        type: "POST",
        url: "operation.aspx/GetCompanyInfo",
        data: "{comId:'" + comId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var companyInfo = response.d.split(';');

            for (var i = 1; i < companyInfo.length; i = i + 6) {
                var comId = companyInfo[i];
                var comName = companyInfo[i + 1];
                var comNickName = companyInfo[i + 2];
                var motherCom = companyInfo[i + 3];
                var status = companyInfo[i + 4];
                var country = companyInfo[i + 5];

                row = row + "<tr style='text-align:left'><td>" + comId + "</td><td>" + comName + "</td><td>" + comNickName + "</td><td>" + motherCom + "</td><td>" + country + "</td><td>" + status + "</td><td><div style='float:right;'><button type='button' style='float: right; margin-top: 0px;' class='btn btn-xs btn-success editcomid' id='" + comId + "'><span class ='glyphicon glyphicon-edit'></span>Edit</button></div></td></tr>";
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


    $('#tblData').on('click', '.editcomid', function () {

        var comId = this.id;
        //alert(srID);

        $.ajax({
            type: "POST",
            url: "operation.aspx/GetCompanyInfo",
            data: "{comId:'" + comId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                var companyInfo = response.d.split(';');

                var comId = companyInfo[1];
                var comName = companyInfo[2];
                var comNickName = companyInfo[3];
                var motherCom = companyInfo[4];
                var status = companyInfo[5];
                var country = companyInfo[6];
                                 
                $("#txtComId").val(comId);
                $("#txtComName").val(comName);
                $("#txtComNickName").val(comNickName);
                $("#ddlMotherCompany").val(motherCom);
                $("#ddlComActive").val(status);
                $("#ddlCountry").val(country);

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