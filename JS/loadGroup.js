
var $ = jQuery.noConflict();
$(function () {
    var ownCompany = "";
    $.ajax({
        type: "POST",
        url: "operation.aspx/GetItemGroup",
        data: "{ownCompany:'" + ownCompany + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {


            var jsonData = JSON.parse(response.d);
            var opt = "<option value='-1'>...Select...</option>";

            for (var i = 0; i < jsonData.length; i++)
            {
                var item_GROUP_ID = jsonData[i].ITEM_GROUP_ID;
                var item_GROUP_NAME = jsonData[i].ITEM_GROUP_NAME;
               
                opt = opt + "<option value='" + item_GROUP_ID + "'>" + item_GROUP_NAME + "</option>";
            }

            
            $("#ddlItemGroup").html('');
            $("#ddlItemGroup").append(opt);

        },
        failure: function (response) {
            alert(response.d);
        }
    });
});