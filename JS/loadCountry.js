var $ = jQuery.noConflict();
$(function () {
    
    var countryName = "";//$("#ddlCountry option:selected").text();
        $.ajax({
            type: "POST",
            url: "operation.aspx/GetCountry",
            data: "{countryName:'" + countryName + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {

                var companyInfo = response.d.split(';');
                var opt = "<option value='-1'>...Select...</option>";
                for (var i = 1; i < companyInfo.length; i = i + 2) {
                    var countryId = companyInfo[i];
                    var countryName = companyInfo[i + 1];
                    opt = opt + "<option value='" + countryId + "'>" + countryName + "</option>";
                }
                $("#ddlCountry").html('');
                $("#ddlCountry").append(opt);

            },
            failure: function (response) {
                alert(response.d);
            }
        });
    
});