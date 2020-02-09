
var $ = jQuery.noConflict();
$(function () {
    var tranId = "tranId";

    $("#tblData").html("");
    $('#dvMiddleContent').html('');
    $('#dvMiddleContent').html("<div style='width:70%;height:36px;margin-top: 5px;background-color:#8dc060;text-align:center;color:#fff;font-size:25px;padding-top:10px;margin-top:10px;margin-left:15%;'><span>DM wise Gate pass Report</span></div>" +
                       "<div style='width:69.8%;height:auto;margin-top: 2px;border:1px solid #8dc060;padding-top:10px;margin-left:15%;'>" +
                          "<table style='padding-top:10px;padding-bottom: 10px;padding-left:25%'>" +

                            "<tr><td>Country Name :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlCountry' name='ddlCountry'></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                            "<tr><td>Delivery Man :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlDeliveryMan' name='ddlDeliveryMan'></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +

                            "<tr><td>Order Date :</td><td><input type='text' id='txtOrderDate' placeholder='DD/MM/YYYY' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>*</span></td></tr>" +

                            "<tr><td></td><td style='text-align:right;'><button type='button' id='btnShow' style='background-color: #8dc060;border-color: #4cae4c;color: #fff;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;cursor: pointer;display: inline-block;font-size: 14px;font-weight: 400;line-height: 1.42857;margin-bottom: 0;padding: 6px 12px;text-align: center;vertical-align: middle;white-space: nowrap;'>Show</button></td></tr>" +
                          "</table>" +
                        "</div>");

    $("#txtSRId").focus();

    var d = new Date();
    var month = d.getMonth() + 1;
    var day = d.getDate();
    var currentDate = (day < 10 ? '0' : '') + day + '/' + (month < 10 ? '0' : '') + month + '/' + d.getFullYear();
    $("#txtOrderDate").val(currentDate);
    $j("#txtOrderDate").datepicker({ showAnim: "fold", dateFormat: "dd/mm/yy" });


    //$.getScript("JS/loadDivision.js");
    //loadZone();
    //$.getScript("JS/loadZone.js");
    $.getScript("JS/loadCountry.js");

    $("#ddlCountry").change(function (e) {
        var countryName = $("#ddlCountry").val();
        $.ajax({
            type: "POST",
            url: "operation.aspx/GetDMByCountry",
            data: "{countryName:'" + countryName + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {

                var outletInfo = response.d.split(';');
                var opt = "<option value='-1'>...Select...</option>";
                for (var i = 1; i < outletInfo.length; i = i + 2) {
                    var dmId = outletInfo[i];
                    var dmName = outletInfo[i + 1];
                    opt = opt + "<option value='" + dmId + "'>" + dmId + "-" + dmName + "</option>";
                }
                $("#ddlDeliveryMan").html('');
                $("#ddlDeliveryMan").append(opt);

            },
            failure: function (response) {
                alert(response.d);
            }
        });
    });


    $("#btnShow").click(function () {

        var dmId = $("#ddlDeliveryMan").val();
        var orderDate = $("#txtOrderDate").val();

        if (dmId == "-1") {
            alert('Select DM');
            return;
        }
        else if (orderDate == "") {
            alert('Enter Order Date');
            return;
        }
        else {
            //window.location = "outletmemo.aspx?dm=" + dmId + "&sr=" + srId + "&orderdate=" + orderDate;
            var url = "gatepassreport.aspx?dm=" + dmId + "&orderdate=" + orderDate;
            window.open(url, '_blank');
            return false;
        }

    });

});