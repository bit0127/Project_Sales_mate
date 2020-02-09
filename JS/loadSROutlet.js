var $ = jQuery.noConflict();
$(function () {

    $j('#dvMiddleContent').html('');
    $j('#dvMiddleContent').html("<div style='width:70%;height:36px;margin-top: 5px;background-color:#8dc060;text-align:center;color:#fff;font-size:25px;padding-top:10px;margin-top:10px;margin-left:15%;'><span>SR Outlet Transfer</span></div>" +
        "<div style='width:69.8%;height:auto;margin-top: 2px;border:1px solid #8dc060;padding-top:10px;margin-left:15%;'>" +
        "<table style='padding-top:10px;padding-bottom: 10px;padding-left:25%'>" +

        "<tr><td>Country Name :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlCountry' name='ddlCountry'></select></td><td><span style='color:#ec407a;'>*</span></td></tr>"+
        "<tr><td>Division Name :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlDivision' name='ddlDivision'></select></td><td><span style='color:#ec407a;'>*</span></td></tr>"+
        "<tr><td>Zone Name :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlZone' name='ddlZone'></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
        "<tr><td>Route Name :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlRoute' name='ddlRoute'></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +

        "<tr><td>Previous SR ID :</td><td><input type='text' id='txtClosedSRId' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td></td></tr>" +
        "<tr><td>Current SR ID :</td><td><input type='text' id='txtNewSRId' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td></td></tr>" +
        
        "<tr><td></td><td style='text-align:right;'> <button type='button' id='btnSave' style='background-color: #8dc060;border-color: #4cae4c;color: #fff;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;cursor: pointer;display: inline-block;font-size: 14px;font-weight: 400;line-height: 1.42857;margin-bottom: 0;padding: 6px 12px;text-align: center;vertical-align: middle;white-space: nowrap;'>Transfer Outlet</button></td></tr></table></div>");

    $j("#txtClosedSRId").focus();

    $j.getScript("JS/loadCountry.js");
    //loadDivision();
    $j.getScript("JS/loadDivision.js");
    //loadZone();
    $j.getScript("JS/loadZone.js");

    $j("#ddlZone").change(function () {
        var zoneId = $j("#ddlZone").val();
        $j.ajax({
            type: "POST",
            url: "operation.aspx/GetRoute",
            data: "{zoneId:'" + zoneId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {

                var routeInfo = response.d.split(';');
                $j("#ddlRoute").html('');
                var opt = "<option value='-1'>...Select...</option>";
                for (var i = 1; i < routeInfo.length; i = i + 2) {
                    var routeId = routeInfo[i];
                    var routeName = routeInfo[i + 1];
                    opt = opt + "<option value='" + routeId + "'>" + routeName + "</option>";
                }
                
                $j("#ddlRoute").append(opt);

            },
            failure: function (response) {
                alert(response.d);
            }
        });
    });

    $j("#btnSave").click(function () {
         
        var ddlCountry = $j("#ddlCountry").val();
        var ddlDivision = $j("#ddlDivision").val();
        var ddlZone = $j("#ddlZone").val();
        var routeID = $j("#ddlRoute").val();
        var closedSRId = $j("#txtClosedSRId").val();
        var newSRId = $j("#txtNewSRId").val();

        if (ddlCountry == "-1") {
            alert('Select Country Name');
            return;
        }
        else if (ddlDivision == "-1") {
            alert('Select Division Name');
            return;
        }
        else if (ddlZone == "-1") {
            alert('Select Zone Name');
            return;
        }
        else if (routeID == "-1") {
            alert('Select Route Name');
            return;
        }
        else if (closedSRId == "") {
            alert('Enter Previous SR ID');
            return;
        }
        else if (newSRId == "") {
            alert('Enter Current SR ID');
            return;
        }

        $j.ajax({
            type: "POST",
            url: "operation.aspx/TransferSROutlet",
            data: "{routeID:'" + routeID + "',closedSRId:'" + closedSRId + "',newSRId:'" + newSRId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                var msg = response.d;
                alert(msg); 
            },
            failure: function (response) {
                alert(response.d);
            }
        });

    });
});