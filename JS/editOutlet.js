
var $ = jQuery.noConflict();
$(function () {

    $("#lnkEditOutlet").click(function (e) {
        e.preventDefault();


        $.getScript("JS/searchOutletInfo.js");


        //$.getScript("JS/outletInfo.js");

        //$("html, body").animate({ scrollTop: $(document).height() - 20 }, "slow");
        
        

    });

});