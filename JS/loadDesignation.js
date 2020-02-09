var $ = jQuery.noConflict();
$(function () {
    var designation = "";
    $.ajax({
        type: "POST",
        url: "operation.aspx/GetAllDesignation",
        data: "{designation:'" + designation + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {


            var jsonData = JSON.parse(response.d);
            var opt = "<option value='-1'>...Select...</option>";

            for (var i = 0; i < jsonData.length; i++) {
                var id = jsonData[i].DESIGNATION_ID;
                var designation_NAME = jsonData[i].DESIGNATION_NAME;

                opt = opt + "<option value='" + id + "'>" + designation_NAME + "</option>";
            }


            $("#ddlDesignation").html('');
            $("#ddlDesignation").append(opt);

        },
        failure: function (response) {
            alert(response.d);
        }
    });
});