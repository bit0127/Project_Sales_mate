
var $ = jQuery.noConflict();
$(function () {
    $("#ddlCountry").change(function () {
        var countryName = $("#ddlCountry option:selected").text();
        var group = $("#ddlGroupName").val();
        $.ajax({
            type: "POST",
            url: "trademktg.aspx/GetDivision",
            data: "{countryName:'" + countryName + "',group:'" + group + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {

                var companyInfo = response.d.split(';');
                var opt = "<option value='-1'>...Select...</option>";
                //opt = opt + "<option value='AllDivision'>All Division</option>";
                for (var i = 1; i < companyInfo.length; i = i + 2) {
                    var groupId = companyInfo[i];
                    var groupName = companyInfo[i + 1];
                    opt = opt + "<option value='" + groupId + "'>" + groupName + "</option>";
                }
                $("#ddlDivision").html('');
                $("#ddlDivision").append(opt);

            },
            failure: function (response) {
                alert(response.d);
            }
        });
    });
});