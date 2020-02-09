var $ = jQuery.noConflict();
$(function () {
    var designation = "";
    $.ajax({
        type: "POST",
        url: "operation.aspx/GetAllDivisionByCompany",
        //data: "{designation:'" + designation + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {


            var jsonData = JSON.parse(response.d);
            var opt = "<option value='-1'>...Select...</option>";

            for (var i = 0; i < jsonData.length; i++) {
                var id = jsonData[i].DIVISION_ID;
                var division_NAME = jsonData[i].DIVISION_NAME;

                opt = opt + "<option value='" + id + "'>" + division_NAME + "</option>";
            }


            $("#ddlDivision").html('');
            $("#ddlDivision").append(opt);

        },
        failure: function (response) {
            alert(response.d);
        }
    });
});