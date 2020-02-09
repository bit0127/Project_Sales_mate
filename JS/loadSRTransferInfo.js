var $ = jQuery.noConflict();
$(function () {

    $j('#dvMiddleContent').html('');
    $j('#dvMiddleContent').html("<div style='width:70%;height:36px;margin-top: 5px;background-color:#8dc060;text-align:center;color:#fff;font-size:25px;padding-top:10px;margin-top:10px;margin-left:15%;'><span>SR Route Outlet Transfer</span></div><div style='width:69.8%;height:auto;margin-top: 2px;border:1px solid #8dc060;padding-top:10px;margin-left:15%;'><table style='padding-top:10px;padding-bottom: 10px;padding-left:25%'><tr><td>Closed SR ID :</td><td><input type='text' id='txtClosedSRId' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td></td></tr><tr><td>New SR ID :</td><td><input type='text' id='txtNewSRId' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td></td></tr><tr><td></td><td style='text-align:right;'> <button type='button' id='btnSave' style='background-color: #8dc060;border-color: #4cae4c;color: #fff;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;cursor: pointer;display: inline-block;font-size: 14px;font-weight: 400;line-height: 1.42857;margin-bottom: 0;padding: 6px 12px;text-align: center;vertical-align: middle;white-space: nowrap;'>Transfer Outlet</button></td></tr></table></div>");

    $j("#txtClosedSRId").focus();

    $j("#btnSave").click(function () {
        var closedSRId = $j("#txtClosedSRId").val();
        var newSRId = $j("#txtNewSRId").val();

        if (closedSRId == "") {
            alert('Enter Closed SR ID');
            return;
        }
        else if (newSRId == "") {
            alert('Enter New SR ID');
            return;
        }

        $j.ajax({
            type: "POST",
            url: "operation.aspx/TransferSRRouteOutlet",
            data: "{closedSRId:'" + closedSRId + "',newSRId:'" + newSRId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                var msg = response.d;

                alert(msg);

                $j("#txtClosedSRId").val('');
                $j("#txtNewSRId").val('');

            },
            failure: function (response) {
                alert(response.d);
            }
        });

    });
});