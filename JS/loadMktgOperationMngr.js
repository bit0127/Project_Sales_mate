
var $ = jQuery.noConflict();
$(function () {
     
    var mngrInfo = "country";
        $.ajax({
            type: "POST",
            url: "trademktg.aspx/GetOperationManager",
            data: "{mngrInfo:'" + mngrInfo + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {

                var mngrInfo = response.d.split(';');
                var opt = "<option value='-1'>...Select...</option>";                 
                for (var i = 1; i < mngrInfo.length; i = i + 2) {
                    var mngrId = mngrInfo[i];
                    var mngrName = mngrInfo[i + 1];
                    opt = opt + "<option value='" + mngrId + "'>" + mngrName + "</option>";
                }
                $("#ddlOperationMngr").html('');
                $("#ddlOperationMngr").append(opt);

            },
            failure: function (response) {
                alert(response.d);
            }
        });
 
});