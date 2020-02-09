

var $ = jQuery.noConflict();
$(function () {
    $("#ddlDivision").change(function () {
        var division = $("#ddlDivision").val();
        $.ajax({
            type: "POST",
            url: "operation.aspx/GetZone",
            data: "{division:'" + division + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {

                var jsonData = JSON.parse(response.d);
                var opt = "<option value='-1'>...Select...</option>";

                for (var i = 0; i < jsonData.length; i++) {
                    var id = jsonData[i].ZONE_ID;
                    var zone_NAME = jsonData[i].ZONE_NAME;

                    opt = opt + "<option value='" + id + "'>" + zone_NAME + "</option>";
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