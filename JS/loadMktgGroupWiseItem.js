
var $ = jQuery.noConflict();
$(function () {
    var group = $("#ddlGroup").val();
    
    $.ajax({
        type: "POST",
        url: "trademktg.aspx/GetGroupWiseItemInfo",
        data: "{group:'" + group + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var itemInfo = response.d.split(';');
            var opt = "<option value='-1'>...Select...</option>";
            opt = opt + "<option value='AllProduct'>All Product</option>";
            for (var i = 1; i < itemInfo.length; i = i + 2) {
                var itemId = itemInfo[i];
                var itemName = itemInfo[i + 1];
                opt = opt + "<option value='" + itemId + "'>" + itemName + "</option>";
            }
            $("#ddlProductName").html('');
            $("#ddlProductName").append(opt);
        },
        failure: function (response) {
            alert(response.d);
        }
    });

 
});