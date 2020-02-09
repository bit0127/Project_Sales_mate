

var $ = jQuery.noConflict();
$(function () {
    $("#ddlDivision").change(function () {
        var division = $("#ddlDivision").val();
        $.ajax({
            type: "POST",
            url: "trademktg.aspx/GetZoneInfoByDivision",
            data: "{divisionId:'" + division + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {

                var companyInfo = response.d.split(';');
                var opt = "<option value='-1'>...Select...</option>";
                //opt = opt + "<option value='AllZone'>All Zone</option>";
                for (var i = 1; i < companyInfo.length; i = i + 2) {
                    var groupId = companyInfo[i];
                    var groupName = companyInfo[i + 1];
                    opt = opt + "<option value='" + groupId + "'>" + groupName + "</option>";
                }
                $("#ddlZone").html('');
                $("#ddlZone").append(opt);

            },
            failure: function (response) {
                alert(response.d);
            }
        });
    });
});