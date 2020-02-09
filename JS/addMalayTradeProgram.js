
var $ = jQuery.noConflict();
$(function () {
    $("#tblData").html("");
    $('#dvMiddleContent').html('');
    $('#dvMiddleContent').html("<div style='width:70%;height:36px;margin-top: 5px;background-color:#8dc060;text-align:center;color:#fff;font-size:25px;padding-top:10px;margin-top:10px;margin-left:15%;'><span>Add Trade Program</span></div>" +
                       "<div style='width:69.8%;height:auto;margin-top: 2px;border:1px solid #8dc060;padding-top:10px;margin-left:15%;'>" +
                       "<table style='padding-top:10px;padding-bottom: 10px;padding-left:25%'>" +
                       "<tr><td>Program ID :</td><td><input type='text' id='txtProgramID' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td></td></tr>"+
                       "<tr><td>Program Name :</td><td><input type='text' id='txtProgramName' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>Mother Company :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlMotherCompany' name='ddlMotherCompany'><option value='-1'>...Select...</option><option value='PRAN'>PRAN</option><option value='RFL'>RFL</option></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>Own Company :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlOwnCompany' name='ddlOwnCompany'></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>Country Name :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlCountry' name='ddlCountry'></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>Division/Region Name :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlDivision' name='ddlDivision'></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>Zone :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlZone' name='ddlZone'></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>Outlet Grade(A,B,C) :</td><td><input type='text' id='txtOutletGrade' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>*</span></td></tr>" +

                       "<tr><td>Group Name :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlItemGroup' name='ddlItemGroup'></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>Item Class :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlItemClass' name='ddlItemClass'></select></td><td></td></tr>"+
                       "<tr><td>Item Name :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlItemName' name='ddlItemName'></select></td><td></td></tr>" +
                       "<tr><td>Free/Gift Item ID :</td><td><input type='text' id='txtFreeItemID' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>(M32152,M35234)</span></td></tr>" +

                       "<tr><td>Min Qty(Ctn/Piece) :</td><td><input type='text' id='txtMinQty' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>Max Qty(Ctn/Piece) :</td><td><input type='text' id='txtMaxQty' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>Free Type :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlFreeType' name='ddlFreeType'><option value='-1'>...Select...</option><option value='Carton'>Carton/Box etc</option><option value='Piece'>Piece</option></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +

                       "<tr><td>Free Qty(Ctn/Box etc.) :</td><td><input type='text' id='txtFreeQty' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>Free Pieces :</td><td><input type='text' id='txtFreePcs' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>*</span></td></tr>" +

                       "<tr><td>Min Qty for Discnt :</td><td><input type='text' id='txtMinQtyForDiscount' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>Discount :</td><td><input type='text' id='txtDiscount' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>Discount Type :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlDiscountType' name='ddlDiscountType'><option value='-1'>...Select...</option><option value='Money'>Money</option><option value='Percent'>Percent-%</option></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +

                       "<tr><td>Program From Date :</td><td><input type='text' id='txtProgramFromDate' placeholder='DD/MM/YYYY' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>Program To Date :</td><td><input type='text' id='txtProgramToDate' placeholder='DD/MM/YYYY' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>*</span></td></tr>" +

                       "<tr><td>Free From Date :</td><td><input type='text' id='txtFreeFromDate' placeholder='DD/MM/YYYY' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td>Free To Date :</td><td><input type='text' id='txtFreeToDate' placeholder='DD/MM/YYYY' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td><span style='color:#ec407a;'>*</span></td></tr>" +


                       "<tr><td>Active :</td><td><select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlActive' name='ddlActive'><option value='-1'>...Select...</option><option value='Y'>Yes</option><option value='N'>No</option></select></td><td><span style='color:#ec407a;'>*</span></td></tr>" +
                       "<tr><td></td><td style='text-align:right;'> <button type='button' id='btnSave' style='background-color: #8dc060;border-color: #4cae4c;color: #fff;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;cursor: pointer;display: inline-block;font-size: 14px;font-weight: 400;line-height: 1.42857;margin-bottom: 0;padding: 6px 12px;text-align: center;vertical-align: middle;white-space: nowrap;'>Save</button> </td></tr></table></div>");

    var d = new Date();
    var month = d.getMonth() + 1;
    var day = d.getDate();    
    var currentDate = (day < 10 ? '0' : '') + day + '/' + (month < 10 ? '0' : '') + month + '/' + d.getFullYear();
    $j("#txtProgramFromDate").datepicker({ showAnim: "fold", dateFormat: "dd/mm/yy" });
    $j("#txtProgramToDate").datepicker({ showAnim: "fold", dateFormat: "dd/mm/yy" });
    $j("#txtFreeFromDate").datepicker({ showAnim: "fold", dateFormat: "dd/mm/yy" });
    $j("#txtFreeToDate").datepicker({ showAnim: "fold", dateFormat: "dd/mm/yy" });
     
    $("#txtProgramName").focus();
    $("#txtFreePcs").val('0');
    $("#txtDiscount").val('0');
    $("#txtProgramID").attr("disabled", "disabled");

    $.getScript("JS/loadCountry.js");
     
    $("#ddlCountry").change(function () {
        var countryName = $("#ddlCountry option:selected").text();
        $.ajax({
            type: "POST",
            url: "operation.aspx/GetDivision",
            data: "{countryName:'" + countryName + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {

                var companyInfo = response.d.split(';');
                var opt = "<option value='-1'>...Select...</option>";
                for (var i = 1; i < companyInfo.length; i = i + 2) {
                    var groupId = companyInfo[i];
                    var groupName = companyInfo[i + 1];
                    opt = opt + "<option value='" + groupId + "'>" + groupName + "</option>";
                }
                $("#ddlDivision").html('');
                $("#ddlDivision").append(opt);

            },
            failure: function (response) {
                alert(response.d);
            }
        });
    });

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

                var companyInfo = response.d.split(';');
                var opt = "<option value='-1'>...Select...</option>";
                for (var i = 1; i < companyInfo.length; i = i + 2) {
                    var groupId = companyInfo[i];
                    var groupName = companyInfo[i + 1];
                    opt = opt + "<option value='" + groupId + "'>" + groupName + "</option>";
                }
                $("#ddlZone").html('');
                $("#ddlZone").append(opt);

            },
            failure: function (response) {
                alert(response.d);
            }
        });
    });

    

    function loadAllTp() {
        var zoneId = $("#ddlZone").val();
        var divisionId = $("#ddlDivision").val();
        var country = $("#ddlCountry").val();

        //--Load grid view---------------------
        $("#tblData").html("");
        $("#tblData").html("<div style='width:545px;'><span id='loader' style='float:left'><img src='images/ajax-loader.gif' alt='Loading...' style='width:300px;height:17px;margin-top:10px; margin-left:40%; display:block;'/></span><span id='spPercentage' style='float:right;color:#2eaccc;display:block;margin-top:10px;'></span></div>");

        var tbl = "<table id='example' class='display' style='color:#9C27B0' cellspacing='0' width='100%'style='padding-top:10px;'>";
        var thead = "<thead><tr><th>Program ID</th><th>Program Name</th><th>Item Name</th><th>Min Qty</th><th>Max Qty</th><th>Free Carton/Box etc.</th><th>Free Qty</th><th>Free Type</th><th>Free Item</th><th>Discount</th><th>Min Qty For Discount</th><th>From Date</th><th>To Date</th><th>Division Name</th><th>Zone Name</th><th>Outlet Grade</th><th>Status</th><th>Operaton</th></tr></thead>";
        //var thead = "<thead><tr><th>Program Name</th><th>Item Name</th><th>Min Qty</th><th>Max Qty</th><th>Free Qty</th><th>Free Item</th><th>Discount</th><th>From Date</th><th>To Date</th><th>Division Name</th><th>Zone Name</th><th>Outlet Grade</th><th>Status</th></tr></thead>";
        var row = "";
        var itemId = "itemId";
        var fromDate = "f";
        var toDate = "t";
        var programid = "p";
        $.ajax({
            type: "POST",
            url: "operation.aspx/GetTradeProgram",
            data: "{itemId:'" + itemId + "',zoneId:'" + zoneId + "',divisionId:'" + divisionId + "',country:'" + country + "',fromDate:'" + fromDate + "',toDate:'" + toDate + "',programid:'" + programid + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                var itemInfo = response.d.split(';');

                for (var i = 1; i < itemInfo.length; i = i + 31) {
                    var PROGRAM_NAME = itemInfo[i];
                    var MOTHER_COMPANY = itemInfo[i + 1];
                    var OWN_COMPANY = itemInfo[i + 2];
                    var COUNTRY_NAME = itemInfo[i + 3];
                    var ITEM_GROUP_ID = itemInfo[i + 4];
                    var GROUP_NAME = itemInfo[i + 5];
                    var CLASS_ID = itemInfo[i + 6];
                    var CLASS_NAME = itemInfo[i + 7];
                    var ITEM_ID = itemInfo[i + 8];
                    var ITEM_NAME = itemInfo[i + 9];
                    var FREE_ITEM_ID = itemInfo[i + 10];
                    var FREE_ITEM_NAME = itemInfo[i + 11];
                    var MIN_QTY = itemInfo[i + 12];
                    var MAX_QTY = itemInfo[i + 13];
                    var FREE_QTY = itemInfo[i + 14];
                    var DISCOUNT = itemInfo[i + 15];
                    var DISCOUNT_TYPE = itemInfo[i + 16];
                    var P_FROM_DATE = itemInfo[i + 17];
                    var P_TO_DATE = itemInfo[i + 18];
                    var FROM_DATE = itemInfo[i + 19];
                    var TO_DATE = itemInfo[i + 20];
                    var DIVISION_ID = itemInfo[i + 21];
                    var DIVISION_NAME = itemInfo[i + 22];
                    var ZONE_ID = itemInfo[i + 23];
                    var ZONE_NAME = itemInfo[i + 24];
                    var OUTLET_GRADE = itemInfo[i + 25];
                    var STATUS = itemInfo[i + 26];
                    var FREE_PCS = itemInfo[i + 27];
                    var FREE_TYPE = itemInfo[i + 28];
                    var MIN_QTY_DISCOUNT = itemInfo[i + 29];
                    var PROGRAM_ID = itemInfo[i + 30];
                    //row = row + "<tr style='text-align:left'><td>" + PROGRAM_NAME + "</td><td>" + ITEM_NAME + "</td><td>" + MIN_QTY + "</td><td>" + MAX_QTY + "</td><td>" + FREE_QTY + "</td><td>" + FREE_ITEM_ID + "</td><td>" + DISCOUNT + "(" + DISCOUNT_TYPE + ")" + "</td><td>" + FROM_DATE + "</td><td>" + TO_DATE + "</td><td>" + DIVISION_NAME + "</td><td>" + ZONE_NAME + "</td><td>" + STATUS + "</td><td>" + STATUS + "</td></tr>";
                    row = row + "<tr style='text-align:left'><td>" + PROGRAM_ID + "</td><td>" + PROGRAM_NAME + "</td><td>" + ITEM_NAME + "</td><td>" + MIN_QTY + "</td><td>" + MAX_QTY + "</td><td>" + FREE_QTY + "</td><td>" + FREE_PCS + "</td><td>" + FREE_TYPE + "</td><td>" + FREE_ITEM_ID + "</td><td>" + DISCOUNT + "(" + DISCOUNT_TYPE + ")" + "</td><td>" + MIN_QTY_DISCOUNT + "</td><td>" + FROM_DATE + "</td><td>" + TO_DATE + "</td><td>" + DIVISION_NAME + "</td><td>" + ZONE_NAME + "</td><td>" + OUTLET_GRADE + "</td><td>" + STATUS + "</td><td><div style='float:right;'><button type='button' style='float: right; margin-top: 0px;' class='btn btn-xs btn-success editItemInfo' id='" + ITEM_ID + "," + ZONE_ID + "," + DIVISION_ID + "," + COUNTRY_NAME + "," + FROM_DATE + "," + TO_DATE + "," + PROGRAM_ID + "'><span class ='glyphicon glyphicon-edit'></span>Edit</button></div></td></tr>";
                }

                tbl = tbl + thead + "<tbody>" + row + "</tbody></table>";

                //$("#loader").css("display", "none");
                $("#tblData").html("");
                $("#tblData").html(tbl);

                var table = $('#example').DataTable({
                    lengthChange: true,
                    "scrollX": true,
                    buttons: true
                });

                table.buttons().container().insertBefore('#example_filter');


            },
            failure: function (response) {
                alert(response.d);
            }
        });


        $('#tblData').on('click', '.editItemInfo', function () {

            var itemInformations = this.id;
            var arrayOfItemInformations = itemInformations.split(",");
            var itemId = arrayOfItemInformations[0];
            var zoneId = arrayOfItemInformations[1];
            var divisionId = arrayOfItemInformations[2];
            var country = arrayOfItemInformations[3];
            var fromDate = arrayOfItemInformations[4];
            var toDate = arrayOfItemInformations[5];
            var programid = arrayOfItemInformations[6];

            $.ajax({
                type: "POST",
                url: "operation.aspx/GetTradeProgram",
                data: "{itemId:'" + itemId + "',zoneId:'" + zoneId + "',divisionId:'" + divisionId + "',country:'" + country + "',fromDate:'" + fromDate + "',toDate:'" + toDate + "',programid:'" + programid + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {
                    var itemInfo = response.d.split(';');

                    var PROGRAM_NAME = itemInfo[1];
                    var MOTHER_COMPANY = itemInfo[2];
                    var OWN_COMPANY = itemInfo[3];
                    var COUNTRY_NAME = itemInfo[4];
                    var ITEM_GROUP_ID = itemInfo[5];
                    var GROUP_NAME = itemInfo[6];
                    var CLASS_ID = itemInfo[7];
                    var CLASS_NAME = itemInfo[8];
                    var ITEM_ID = itemInfo[9];
                    var ITEM_NAME = itemInfo[10];
                    var FREE_ITEM_ID = itemInfo[11];
                    var FREE_ITEM_NAME = itemInfo[12];
                    var MIN_QTY = itemInfo[13];
                    var MAX_QTY = itemInfo[14];
                    var FREE_QTY = itemInfo[15];
                    var DISCOUNT = itemInfo[16];
                    var DISCOUNT_TYPE = itemInfo[17];
                    var P_FROM_DATE = itemInfo[18];
                    var P_TO_DATE = itemInfo[19];
                    var FROM_DATE = itemInfo[20];
                    var TO_DATE = itemInfo[21];
                    var DIVISION_ID = itemInfo[22];
                    var DIVISION_NAME = itemInfo[23];
                    var ZONE_ID = itemInfo[24];
                    var ZONE_NAME = itemInfo[25];
                    var OUTLET_GRADE = itemInfo[26];
                    var STATUS = itemInfo[27];
                    var FREE_PCS = itemInfo[28];
                    var FREE_TYPE = itemInfo[29];
                    var MIN_QTY_DISCOUNT = itemInfo[30];
                    var ProgramID = itemInfo[31];

                    $.ajax({
                        type: "POST",
                        url: "operation.aspx/GetCompany",
                        data: "{motherCompany:'" + MOTHER_COMPANY + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (response) {

                            //if (respond.d == "NotExist") {
                            //alert(response.d);
                            // }
                            //else {
                            var companyInfo = response.d.split(';');
                            var opt = "<option value='-1'>...Select...</option>";
                            for (var i = 1; i < companyInfo.length; i = i + 2) {
                                var comId = companyInfo[i];
                                var comName = companyInfo[i + 1];
                                opt = opt + "<option value='" + comId + "'>" + comName + "</option>";
                            }
                            $("#ddlOwnCompany").html('');
                            $("#ddlOwnCompany").append(opt);
                            //}                             
                        },
                        failure: function (response) {
                            alert(response.d);
                        }
                    });
                    $("#ddlCountry").val(COUNTRY_NAME);

                    $.ajax({
                        type: "POST",
                        url: "operation.aspx/GetDivision",
                        data: "{countryName:'" + COUNTRY_NAME + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (response) {

                            var companyInfo = response.d.split(';');
                            var opt = "<option value='-1'>...Select...</option>";
                            for (var i = 1; i < companyInfo.length; i = i + 2) {
                                var groupId = companyInfo[i];
                                var groupName = companyInfo[i + 1];
                                opt = opt + "<option value='" + groupId + "'>" + groupName + "</option>";
                            }
                            $("#ddlDivision").html('');
                            $("#ddlDivision").append(opt);

                        },
                        failure: function (response) {
                            alert(response.d);
                        }
                    });

                    $("#ddlDivision").val(DIVISION_ID);

                     
                    $.ajax({
                        type: "POST",
                        url: "operation.aspx/GetZone",
                        data: "{division:'" + DIVISION_ID + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (response) {

                            var companyInfo = response.d.split(';');
                            var opt = "<option value='-1'>...Select...</option>";
                            for (var i = 1; i < companyInfo.length; i = i + 2) {
                                var groupId = companyInfo[i];
                                var groupName = companyInfo[i + 1];
                                opt = opt + "<option value='" + groupId + "'>" + groupName + "</option>";
                            }
                            $("#ddlZone").html('');
                            $("#ddlZone").append(opt);

                        },
                        failure: function (response) {
                            alert(response.d);
                        }
                    });

                    $("#ddlZone").val(ZONE_ID);


                    $("#ddlOwnCompany").val(OWN_COMPANY);

                    
                    $.ajax({
                        type: "POST",
                        url: "operation.aspx/GetItemGroup",
                        data: "{ownCompany:'" + OWN_COMPANY + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (response) {

                            var companyInfo = response.d.split(';');
                            var opt = "<option value='-1'>...Select...</option>";
                            for (var i = 1; i < companyInfo.length; i = i + 2) {
                                var groupId = companyInfo[i];
                                var groupName = companyInfo[i + 1];
                                opt = opt + "<option value='" + groupId + "'>" + groupName + "</option>";
                            }
                            $("#ddlItemGroup").html('');
                            $("#ddlItemGroup").append(opt);

                        },
                        failure: function (response) {
                            alert(response.d);
                        }
                    });


                    $("#ddlItemGroup").val(ITEM_GROUP_ID);

                    var itemGroup = $("#ddlItemGroup").val();
                    $.ajax({
                        type: "POST",
                        url: "operation.aspx/GetItemClass",
                        data: "{itemGroup:'" + ITEM_GROUP_ID + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (response) {

                            var companyInfo = response.d.split(';');
                            var opt = "<option value='-1'>...Select...</option>";
                            for (var i = 1; i < companyInfo.length; i = i + 2) {
                                var comId = companyInfo[i];
                                var comName = companyInfo[i + 1];
                                opt = opt + "<option value='" + comId + "'>" + comName + "</option>";
                            }
                            $("#ddlItemClass").html('');
                            $("#ddlItemClass").append(opt);

                        },
                        failure: function (response) {
                            alert(response.d);
                        }
                    });

                    $("#ddlItemClass").val(CLASS_ID);

                     
                    $.ajax({
                        type: "POST",
                        url: "operation.aspx/GetItemByClass",
                        data: "{itemClass:'" + CLASS_ID + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (response) {

                            var companyInfo = response.d.split(';');
                            var opt = "<option value='-1'>...Select...</option>";
                            for (var i = 1; i < companyInfo.length; i = i + 2) {
                                var itemId = companyInfo[i];
                                var itemName = companyInfo[i + 1];
                                opt = opt + "<option value='" + itemId + "'>" + itemId + " - " + itemName + "</option>";
                            }
                            $("#ddlItemName").html('');
                            $("#ddlItemName").append(opt);

                        },
                        failure: function (response) {
                            alert(response.d);
                        }
                    });

                    $("#txtProgramID").val(ProgramID);

                    $("#ddlItemName").val(ITEM_ID);

                    $("#txtProgramName").val(PROGRAM_NAME);
                    $("#ddlMotherCompany").val(MOTHER_COMPANY);

                    $("#txtOutletGrade").val(OUTLET_GRADE);

                    $("#txtFreeItemID").val(FREE_ITEM_ID);
                    $("#txtMinQty").val(MIN_QTY);
                    $("#txtMaxQty").val(MAX_QTY);
                    $("#ddlFreeType").val(FREE_TYPE);
                    $("#txtFreeQty").val(FREE_QTY);
                    $("#txtFreePcs").val(FREE_PCS);

                    $("#txtDiscount").val(DISCOUNT);
                    $("#ddlDiscountType").val(DISCOUNT_TYPE);
                    $("#txtMinQtyForDiscount").val(MIN_QTY_DISCOUNT);

                    $("#txtProgramFromDate").val(P_FROM_DATE);
                    $("#txtProgramToDate").val(P_TO_DATE);
                    $("#txtFreeFromDate").val(FROM_DATE);
                    $("#txtFreeToDate").val(TO_DATE);

                    $("#ddlActive").val(STATUS);


                    //var table = $('#example').DataTable({
                    //    lengthChange: true,
                    //    "scrollX": true,
                    //    buttons: true
                    //});

                    //table.buttons().container().insertBefore('#example_filter');


                },
                failure: function (response) {
                    alert(response.d);
                }
            });

        });

        //--end grid view panel---------------------------------------------------
    }

    $("#ddlZone").change(function (e) {
        loadAllTp();
    });

   

    $("#txtProgramFromDate").val(currentDate);
    $("#txtProgramToDate").val(currentDate);

    $("#txtFreeFromDate").val(currentDate);
    $("#txtFreeToDate").val(currentDate);


    $("#ddlMotherCompany").change(function (e) {
        var motherCompany = $("#ddlMotherCompany option:selected").text();
        $.ajax({
            type: "POST",
            url: "operation.aspx/GetCompany",
            data: "{motherCompany:'" + motherCompany + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {

                //if (respond.d == "NotExist") {
                //alert(response.d);
                // }
                //else {
                var companyInfo = response.d.split(';');
                var opt = "<option value='-1'>...Select...</option>";
                for (var i = 1; i < companyInfo.length; i = i + 2) {
                    var comId = companyInfo[i];
                    var comName = companyInfo[i + 1];
                    opt = opt + "<option value='" + comId + "'>" + comName + "</option>";
                }
                $("#ddlOwnCompany").html('');
                $("#ddlOwnCompany").append(opt);
                //}                             
            },
            failure: function (response) {
                alert(response.d);
            }
        });
    });


    $("#ddlOwnCompany").change(function (e) {
        var ownCompany = $("#ddlOwnCompany").val();
        $.ajax({
            type: "POST",
            url: "operation.aspx/GetItemGroup",
            data: "{ownCompany:'" + ownCompany + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {

                var companyInfo = response.d.split(';');
                var opt = "<option value='-1'>...Select...</option>";
                for (var i = 1; i < companyInfo.length; i = i + 2) {
                    var groupId = companyInfo[i];
                    var groupName = companyInfo[i + 1];
                    opt = opt + "<option value='" + groupId + "'>" + groupName + "</option>";
                }
                $("#ddlItemGroup").html('');
                $("#ddlItemGroup").append(opt);

            },
            failure: function (response) {
                alert(response.d);
            }
        });
    });

    
    $("#ddlItemGroup").change(function (e) {
        var itemGroup = $("#ddlItemGroup").val();
        $.ajax({
            type: "POST",
            url: "operation.aspx/GetItemClass",
            data: "{itemGroup:'" + itemGroup + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {

                var companyInfo = response.d.split(';');
                var opt = "<option value='-1'>...Select...</option>";
                for (var i = 1; i < companyInfo.length; i = i + 2) {
                    var comId = companyInfo[i];
                    var comName = companyInfo[i + 1];
                    opt = opt + "<option value='" + comId + "'>" + comName + "</option>";
                }
                $("#ddlItemClass").html('');
                $("#ddlItemClass").append(opt);

            },
            failure: function (response) {
                alert(response.d);
            }
        });
    });

     
    $("#ddlItemClass").change(function (e) {
        var itemClass = $("#ddlItemClass").val();
        $.ajax({
            type: "POST",
            url: "operation.aspx/GetItemByClass",
            data: "{itemClass:'" + itemClass + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {

                var companyInfo = response.d.split(';');
                var opt = "<option value='-1'>...Select...</option>";
                for (var i = 1; i < companyInfo.length; i = i + 2) {
                    var itemId = companyInfo[i];
                    var itemName = companyInfo[i + 1];
                    opt = opt + "<option value='" + itemId + "'>" + itemId + " - " + itemName + "</option>";
                }
                $("#ddlItemName").html('');
                $("#ddlItemName").append(opt);

            },
            failure: function (response) {
                alert(response.d);
            }
        });
    });
     

    $("#btnSave").click(function () {

        var programID = $("#txtProgramID").val();
        var programName = $("#txtProgramName").val();
        var motherCompany = $("#ddlMotherCompany").val();
        var ownCompany = $("#ddlOwnCompany").val();
        var motherCompany = $("#ddlMotherCompany option:selected").text();         

        var country = $("#ddlCountry").val();
        var divisionId = $("#ddlDivision").val();
        var divisionName = $("#ddlDivision option:selected").text();
        var zoneId = $("#ddlZone").val();
        var zoneName = $("#ddlZone option:selected").text();

        var outletGrade = $("#txtOutletGrade").val();

        var itemGroupId = $("#ddlItemGroup").val();
        var itemGroupName = $("#ddlItemGroup option:selected").text();

        var classId = $("#ddlItemClass").val();
        var className = $("#ddlItemClass option:selected").text();

        var itemId = $("#ddlItemName").val();
        var itemName = $("#ddlItemName option:selected").text();

        var freeItems = $("#txtFreeItemID").val();
        var minQty = $("#txtMinQty").val();
        var maxQty = $("#txtMaxQty").val();
        var freeType = $("#ddlFreeType").val();
        var minQtyForDiscount = $("#txtMinQtyForDiscount").val();
        
        var freeCtn = $("#txtFreeQty").val();
        var freePcs = $("#txtFreePcs").val();

        var discount = $("#txtDiscount").val();
        var discountType = $("#ddlDiscountType").val();

        var programFromDate = $("#txtProgramFromDate").val();
        var programToDate = $("#txtProgramToDate").val();
        var freeFromDate = $("#txtFreeFromDate").val();
        var freeToDate = $("#txtFreeToDate").val();

        var status = $("#ddlActive").val();

        if (programName == "") {
            alert('Enter Program Name');
            return;
        }       
        else if (motherCompany == "-1") {
            alert('Select Mother Company');
            return;
        }
        else if (ownCompany == "-1") {
            alert('Select Own Company');
            return;
        }
        else if (country == "-1") {
            alert('Select Country');
            return;
        }
        else if (divisionId == "-1") {
            alert('Select Division');
            return;
        }
        else if (zoneId == "-1") {
            alert('Select Zone');
            return;
        }
        else if (outletGrade == "") {
            alert('Enter Outlet Grade');
            return;
        }
        else if (itemGroupId == "-1") {
            alert('Select Item Group');
            return;
        }
        else if (classId == "-1") {
            alert('Select Class');
            return;
        }
        else if (itemId == "-1") {
            alert('Select Item');
            return;
        }
        else if (freeItems == "") {
            alert('Enter Free Items ID');
            return;
        }
        else if (minQty == "") {
            alert('Enter Minimum Quantity');
            return;
        }
        else if (maxQty == "") {
            alert('Enter Maximum Quantity');
            return;
        }
        else if (freeType == "-1") {
            alert('Select Free Type');
            return;
        }
        else if (minQtyForDiscount == "") {
            alert('Entry Minumum Qty for Discount');
            return;
        }
        else if (freeCtn == "") {
            alert('Enter Free Carton Quantity');
            return;
        }
        else if (freePcs == "") {
            alert('Enter Free Pieces Quantity');
            return;
        }
        else if (discount == "") {
            alert('Enter Discount');
            return;
        }
        else if (discountType == "-1") {
            alert('Select Discount Type');
            return;
        }
        else if (programFromDate == "") {
            alert('Enter Program From Date');
            return;
        }
        else if (programToDate == "") {
            alert('Enter Program To Date');
            return;
        }
        else if (freeFromDate == "") {
            alert('Enter Free From Date');
            return;
        }
        else if (freeToDate == "") {
            alert('Enter Program To Date');
            return;
        }
        else if (status == "-1") {
            alert('Select Status');
            return;
        }

        $.ajax({
            type: "POST",
            url: "operation.aspx/AddTradeProgram",
            data: "{programID:'" + programID + "',programName:'" + programName + "',motherCompany:'" + motherCompany + "',ownCompany:'" + ownCompany + "',country:'" + country + "',itemGroupId:'" + itemGroupId + "',itemGroupName:'" + itemGroupName + "',classId:'" + classId + "',className:'" + className + "',itemId:'" + itemId + "',itemName:'" + itemName + "',freeItems:'" + freeItems + "',minQty:'" + minQty + "',maxQty:'" + maxQty + "',freeCtn:'" + freeCtn + "',freePcs:'" + freePcs + "',discount:'" + discount + "',discountType:'" + discountType + "',programFromDate:'" + programFromDate + "',programToDate:'" + programToDate + "',freeFromDate:'" + freeFromDate + "',freeToDate:'" + freeToDate + "',status:'" + status + "',divisionId:'" + divisionId + "',divisionName:'" + divisionName + "',zoneId:'" + zoneId + "',zoneName:'" + zoneName + "',outletGrade:'" + outletGrade + "',freeType:'" + freeType + "',minQtyForDiscount:'" + minQtyForDiscount + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                var msg = response.d;

                alert(msg);
                $("#txtProgramID").val('');
                $("#txtFreeItemID").val('');
                $("#txtMinQty").val('');
                $("#txtMaxQty").val('');
                $("#txtFreeQty").val('');
                $("#txtDiscount").val('0');
                $("#txtFreePcs").val('0');

                loadAllTp();

            },
            failure: function (response) {
                alert(response.d);
            }
        });


    });
});