var $ = jQuery.noConflict();
$(function () {

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
            var opt = "<option value='-1'>...Select...</option>";
            for (var i = 1; i < companyInfo.length; i = i + 3) {
                var groupId = companyInfo[i];
                var groupName = companyInfo[i + 1];
                var status = companyInfo[i + 2];
                opt = opt + "<option value='" + groupId + "'>" + groupName + "</option>";
            }
            $("#ddlGroupName").html('');
            $("#ddlGroupName").append(opt);
        },
        failure: function (response) {
            alert(response.d);
        }
    });
});