<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OutletReport.aspx.cs" Inherits="OutletReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Outlet Picture</title>
   
   
    <link rel="shortcut icon" href="Nimages/logopran.gif" />
    <link href="Ncss/jquery-ui.css" rel="stylesheet" />    
   <%-- <link href="Ncss/bootstrap-min2.css" rel="stylesheet" type="text/css"/>--%>

 
    
    <script src="NScripts/jquery-2.1.4.min.js" type="text/javascript"></script>
    <script src="NScripts/bootstrap.min.js" type="text/javascript"></script>
    <script src="NJavaScript/jquery.min.js" type="text/javascript"></script>
    <script src="NJavaScript/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="NJavaScript/jquery-ui.js" type="text/javascript"></script>
    <script src="NScripts/script.js" type="text/javascript"></script>    
  
    <link href="CSS/jquery-ui.css" rel="stylesheet" type="text/css" />

    <script src="mktgJS/jquery.min.js" type="text/javascript"></script>

    <script src="mktgJS/jquery-1.10.2.js" type="text/javascript"></script>

    <script src="mktgJS/jquery-ui.js" type="text/javascript"></script>
 

    <script type="text/javascript">
        var chkOpt = "srwise";
        //$(function () {
        var $ = jQuery.noConflict();
        $(document).ready(function () {

            $("#dtpFromDate").datepicker();
            $("#dtpToDate").datepicker();

            $.getScript("JS/loadMKTGallGroup.js");

            $.getScript("JS/loadMktgDivision.js");

            $("#ddlDivision").change(function () {
                $.getScript("JS/loadMKTGregionByDivision.js");
            });
           
            $("#ddlRegion").change(function () {
                var regionId = $("#ddlRegion").val();
                $.ajax({
                    type: "POST",
                    url: "trademktg.aspx/GetZoneInfoByRegion",
                    data: "{regionId:'" + regionId + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (response) {

                        var companyInfo = response.d.split(';');
                        var opt = "<option value='-1'>...Select...</option>";
                        //opt = opt + "<option value='AllZone'>All Zone</option>";
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

            $("#ddlZone").change(function () {
                var zoneId = $("#ddlZone").val();
                var toDate = $("#dtpToDate").val();
                var fromDate = $("#dtpFromDate").val();;
                $.ajax({
                    type: "POST",
                    url: "trademktg.aspx/GetRouteInfoByZone",
                    data: "{zoneId:'" + zoneId + "',fromDate:'" + fromDate + "',toDate:'" + toDate + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (response) {

                        var companyInfo = response.d.split(';');
                        var opt = "<option value='-1'>...Select...</option>";
                        //opt = opt + "<option value='AllZone'>All Zone</option>";
                        for (var i = 1; i < companyInfo.length; i = i + 2) {
                            var groupId = companyInfo[i];
                            var groupName = companyInfo[i + 1];
                            opt = opt + "<option value='" + groupId + "'>" + groupName + "</option>";
                        }
                        $("#ddlRoute").html('');
                        $("#ddlRoute").append(opt);

                    },
                    failure: function (response) {
                        alert(response.d);
                    }
                });
            });

            /*$("#ddlRoute").change(function () {
                var routeId = $("#ddlRoute").val();
                var category = $("#ddlCategory").val();
                if (category == "-1") {
                    alert('Please Select Category');
                    return;
                }

                $.ajax({
                    type: "POST",
                    url: "OutletReport.aspx/GetZoneWiseSROutlet",
                    data: "{routeId:'" + routeId + "',category:'" + category + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (response) {

                        var companyInfo = response.d.split(';');
                        var opt = "<option value='-1'>...Select...</option>";
                        //opt = opt + "<option value='AllZone'>All Zone</option>";
                        for (var i = 1; i < companyInfo.length; i = i + 2) {
                            var groupId = companyInfo[i];
                            var groupName = companyInfo[i + 1];
                            opt = opt + "<option value='" + groupId + "'>" + groupId + "-" + groupName + "</option>";
                        }
                        $("#optOutlet").html('');
                        $("#optOutlet").append(opt);

                    },
                    failure: function (response) {
                        alert(response.d);
                    }
                });
            });*/

            $("#tblOutlet").hide();
            $("#spnMsg").hide();

            $("#btnLoad").click(function () {
                Validation();
                $("#spnMsg").hide();
                $("#tblOutlet").find("tr:gt(0)").remove();
                $("#tblOutlet").empty();
                $("#tblOutlet").show();
                GetAllOutletInfo();
            });

            $("#spinner").bind("ajaxSend", function () {
                $(this).show();
            }).bind("ajaxStop", function () {
                $(this).hide();
            }).bind("ajaxError", function () {
                $(this).hide();
            });

            



            $("#tblOutlet").on("click", "input[type='button']", function () {

                $.ajax({
                    type: "POST",
                    url: "OutletReport.aspx/ShowOutletPicture",
                    data: "{outletID: '" + this.id + "' }",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: OnSuc,
                    failure: function (response) {
                        alert(response.d);
                    }
                });

            });
            function OnSuc() { }

        });
         

 
        function GetAllOutletInfo() {
            $('#spinner').show();

            var toDate = $("#dtpToDate").val();
            var fromDate = $("#dtpFromDate").val();;
            var group = $("#ddlGroupName").val();
            var zone = $("#ddlZone").val();
            var route = $("#ddlRoute").val();
            var outlet = $("#optOutlet").val();
            if (outlet == "-1") {
                outlet = "";
            }

            $.ajax({
                type: "POST",
                url: "OutletReport.aspx/GetAllOutletInfo",
                data: "{toDate: '" + toDate + "',fromDate: '" + fromDate + "',group: '" + group + "',zone: '" + zone + "',outlet: '" + outlet + "',route: '" + route + "' }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: OnSuccessOutlet,
                failure: function (response) {
                    $('#spinner').hide();
                    alert(response.d);
                }
            });
        }
        function OnSuccessOutlet(response) {
            //alert(response.d);
            var len = response.d.length;
            if (len == 0) {
                $("#tblOutlet").hide();
                $("#spnMsg").show();
                $('#spinner').hide();
            }
            else {

                var newTR = "";
                var outlet = "";
                var outletID = [];
                var outID = "";
                var count = 0;
                var before = "";
                var after = "";
                var others = "";
                var totalOutlet = "";
                //var loader = new ajaxLoader(this);

                var arrayOutlet = [];

                for (var i = 0; i < len; i++) {
                    //var outletImage = response.d[i].base64;
                    //var imageCaptureDate = response.d[i].imageCaptureDate;

                    var srID = response.d[i].srID;
                    var outletID = response.d[i].outletID;
                    var outletName = response.d[i].outletName;
                    var outletAddress = response.d[i].outletAddress;
                    var marchandiseOption = response.d[i].marchandiseOption;

                    var beforeOutletImage = response.d[i].beforeOutletImage;
                    var beforeOutletImageCaptureTime = response.d[i].beforeOutletImageCaptureTime;

                    var afterOutletImage = response.d[i].afterOutletImage;
                    var afterOutletImageCaptureTime = response.d[i].afterOutletImageCaptureTime;

                    var othersOutletImage = response.d[i].othersOutletImage;
                    var othersOutletImageCaptureTime = response.d[i].othersOutletImageCaptureTime;

                    var srID = response.d[i].srID;
                    var outletID = response.d[i].outletID;
                    var outletName = response.d[i].outletName;
                    var outletAddress = response.d[i].outletAddress;
                    var marchandiseOption = response.d[i].marchandiseOption;

                    var beforeOutletImage = response.d[i].beforeOutletImage;
                    var beforeOutletImageCaptureTime = response.d[i].beforeOutletImageCaptureTime;

                    var afterOutletImage = response.d[i].afterOutletImage;
                    var afterOutletImageCaptureTime = response.d[i].afterOutletImageCaptureTime;

                    var othersOutletImage = response.d[i].othersOutletImage;
                    var othersOutletImageCaptureTime = response.d[i].othersOutletImageCaptureTime;
                    totalOutlet = response.d[i].totalOutlet;


                    outlet = "<td style='padding-top:7px;padding-bottom:7px;padding-right:7px;padding-left:7px;width:320px;border-right: 1px solid #000;'>" +
                          "<img style='height:280px;width:320px;box-shadow: 6px 6px 5px #888888;border-radius: 10px;background: #BADA55;' src='data:image/jpg;base64," + beforeOutletImage + "'/></td>" +
                          "<td style='padding-top:7px;padding-bottom:7px;padding-right:7px;padding-left:7px;width:320px;border-right: 1px solid #000;'>" +
                          "<img style='height:280px;width:320px;box-shadow: 6px 6px 5px #888888;border-radius: 10px;background: #BADA55;' src='data:image/jpg;base64," + afterOutletImage + "'/></td>" +
                          "<td style='padding-top:7px;padding-bottom:7px;padding-right:7px;padding-left:7px;width:290px;'>" +
                          "<img style='height:280px;width:290px;box-shadow: 6px 6px 5px #888888;border-radius: 10px;background: #BADA55;' src='data:image/jpg;base64," + othersOutletImage + "'/></td>";


                    newTR = newTR + "<tr width='1000px'><td style='background:#a9c6c9;border-right: 1px solid #000;'>" + srID + "</td><td style='background:#a9c6c9;border-right: 1px solid #000;'>" + outletName + "</td>" + outlet + "</tr>";
                    newTR = newTR + "<tr style='height:50px;background:#00ac45;color:#FFFFFF;'><td style='background:#a9c6c9;border-bottom:1px solid #00ac45;border-right: 1px solid #000;'></td><td style='background:#a9c6c9;border-bottom:1px solid #00ac45;'></td><td style='border: 1px solid #00ac45;'>Outlet Name:" + outletName + "(" + outletID + "),<br/>Address:" + outletAddress + "</td><td style='border: 1px solid #00ac45;'>Marchandise Time:" + beforeOutletImageCaptureTime + ",<br/> Daily Activities:working</td><td></td></tr>";
                }

                    $("#tblOutlet").empty();

                    newTR = "<tr style='width:1245px;height:30px;background: #00ac45; border: 1px solid #ffccaa;color:#FFFFFF;position:fixed; z-index:100;'><td style='width:45px;'>SR ID</td><td style='width:190px;padding-left:5px;text-align:center;'>Outlet name (" + totalOutlet + ")</td><td style='width:320px;text-align:center;'>Before</td><td style='width:320px;text-align:center;'>After</td><td style='width:290px;text-align:center;'>Others</td></tr>" + newTR;


                    $("#tblOutlet").append(newTR);


                    //          if(marchandiseOption=="Before"){
                    //          before=outletImage;          
                    //          }
                    //          else if(marchandiseOption=="After"){
                    //          after=outletImage;
                    //          }
                    //          else if(marchandiseOption=="Others"){
                    //          others=outletImage;
                    //          }
                    //          
                    //       count = count + 1;
                    //       if(count % 3 == 0){
                    //          outlet = "<td style='padding-top:7px;padding-bottom:7px;padding-right:7px;padding-left:7px;width:320px;border-right: 1px solid #000;'>"+          
                    //          "<img style='height:280px;width:320px;box-shadow: 6px 6px 5px #888888;border-radius: 10px;background: #BADA55;' src='data:image/jpg;base64," + before + "'/></td>"+          
                    //          "<td style='padding-top:7px;padding-bottom:7px;padding-right:7px;padding-left:7px;width:320px;border-right: 1px solid #000;'>"+
                    //          "<img style='height:280px;width:320px;box-shadow: 6px 6px 5px #888888;border-radius: 10px;background: #BADA55;' src='data:image/jpg;base64," + after + "'/></td>"+
                    //          "<td style='padding-top:7px;padding-bottom:7px;padding-right:7px;padding-left:7px;width:290px;'>"+
                    //          "<img style='height:280px;width:290px;box-shadow: 6px 6px 5px #888888;border-radius: 10px;background: #BADA55;' src='data:image/jpg;base64," + others + "'/></td>";
                    //           
                    //                    
                    //          newTR = newTR + "<tr width='1000px'><td style='background:#a9c6c9;border-right: 1px solid #000;'>"+srID+"</td><td style='background:#a9c6c9;border-right: 1px solid #000;'>"+outletName+"</td>"+outlet+"</tr>";
                    //          newTR = newTR + "<tr style='height:50px;background:#00ac45;color:#FFFFFF;'><td style='background:#a9c6c9;border-bottom:1px solid #00ac45;border-right: 1px solid #000;'></td><td style='background:#a9c6c9;border-bottom:1px solid #00ac45;'></td><td style='border: 1px solid #00ac45;'>Outlet Name:"+outletName+"("+outletID+"),<br/>Address:"+outletAddress+"</td><td style='border: 1px solid #00ac45;'>Marchandise Time:"+imageCaptureDate+",<br/> Daily Activities:working</td><td></td></tr>";
                    //          before="";
                    //          after="";           
                    //          others="";
                    //        }
                    //    outID = outletID;
                    //     }
                    //    $("#tblOutlet").empty();
                    //    //$("#tblOutlet").append("<tr style='position:fixed; z-index:100;'><td style='width:45px;'>SR ID</td><td style='width:100px;padding-left:5px;'>Outlet name</td><td style='width:350px;'>Before</td><td style='width:350px;'>After</td><td>Others</td></tr>");
                    //    
                    //    newTR = "<tr style='width:1130px;height:30px;background: #00ac45; border: 1px solid #ffccaa;color:#FFFFFF;position:fixed; z-index:100;'><td style='width:45px;'>SR ID</td><td style='width:100px;padding-left:5px;'>Outlet name</td><td style='width:350px;'>Before</td><td style='width:350px;'>After</td><td>Others</td></tr>" + newTR;
                    //    
                    //    
                    //    $("#tblOutlet").append(newTR);

                //}
                $('#spinner').hide();
            }
        }

        function Validation() {
            if ($("#dtpFromDate").val() == "") {
                alert("Please input From Date.");
                return;
            }
            if ($("#dtpToDate").val() == "") {
                alert("Please input To Date.");
                return;
            }
            if ($("#ddlCategory").val() == "-1") {
                alert("Please Select Category.");
                return;
            }
            if ($("#ddlGroupName").val() == "-1") {
                alert("Please Select Group.");
                return;
            }
            if ($("#ddlCountry").val() == "-1") {
                alert("Please Select Country.");
                return;
            }
            if ($("#ddlDivision").val() == "-1") {
                alert("Please Select Division.");
                return;
            }
            if ($("#ddlRegion").val() == "-1") {
                alert("Please Select Region.");
                return;
            }
            if ($("#ddlZone").val() == "-1") {
                alert("Please Select Zone.");
                return;
            }
        }
    </script>

    <style type="text/css">
        table.hovertable
        {
            font-family: verdana,arial,sans-serif;
            font-size: 11px;
            color: #333333;
            border-width: 1px;
            border-color: #999999;
            border-collapse: collapse;
        }
        table.hovertable th
        {
            padding: 8px;
        }
        table.hovertable tr
        {
        }
        table.hovertable td
        {
            padding: 8px;
        }
        .spinner
        {
            position: fixed;
            top: 50%;
            left: 50%;
            margin-left: -50px; /* half width of the spinner gif */
            margin-top: -50px; /* half height of the spinner gif */
            text-align: center;
            z-index: 1234;
            overflow: auto; /* width: 100px;  width of the spinner gif */ /* height: 102px; hight of the spinner gif +2px to fix IE8 issue */
        }
        table
        {
            border: none;
            border-collapse: collapse;
        }
         
        table td:last-child
        {
            border-right: none;
        }
    </style>
    &nbsp;</head><body><form id="form1" runat="server">
    <div align="center">
        <div style="top: 0px; left: 0; position: fixed; z-index: 100; width: 100%; height: 230px;background: #00bcd4;">
            <div align="left" style='margin-left:30px'>
                <img src="images/pran-logo.png" title="Upper Sales" width="80" height="25">
            </div>
            <div align="center">
               <div style="margin-top: 0px; background: #D0D0D0; border: 1px solid #ffccaa; width: 96%;height: 195px;padding-left:15px;">
                    <table align="left" style="border:1px solid #D0D0D0">
                        <%--<caption><span style="color:#00bcd4;">Outlet Information</span></caption>--%>
                        <tr>
                            <td style="width: 80px;">
                                From Date:
                            </td>
                            <td>
                                <input type="text" id="dtpFromDate" style="height: 25px;border:1px solid #8dc060;"/><span style="color: red;">*</span>
                            </td>
                            <td style="padding-left:15px;width:330px;">
                            <%--<input type="checkbox" id="chkBoxSR" name="outlet" value="SR" checked="checked"/>SR
                                wise &nbsp
                                <input type="checkbox" id="chkBoxOutlet" name="outlet" value="Outlet"/>Outlet wise
                                &nbsp
                                <input type="checkbox" id="chkBoxProduct" name="outlet" value="Outlet"/>Product wise<br>--%>
                                Select Category:&nbsp;<select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlCategory' name='ddlCategory'>
                                    <option value='-1'>...Select...</option>
                                    <option value='SRWise'>SR Wise</option>
                                    <option value='OutletWise'>Outlet Wise</option>
                                    <option value='ProductWise'>Product Wise</option>
                                    <option value='SalesAssetWise'>Sales Asset Wise</option>
                                    <option value='Rental'>Rental Wise</option>
                                    <option value='NonRental'>Non Rental Wise</option>

                                </select>
                            </td>
                            <td style="float:right;width:330px;">
                                Group:&nbsp;<select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlGroupName' name='ddlGroupName'>                                     
                                </select> 
                            </td>
                            <td style="padding-left:10px;">
                                <input type="button" id="btnLoad" value="LOAD" style="width: 100px; height: 28px;background-color: #8dc060;border-color: #4cae4c;color: #fff;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;cursor: pointer;display: inline-block;font-size: 14px;font-weight: 400;line-height: 0.42857;margin-bottom: 0;padding: 6px 12px;text-align: center;vertical-align: middle;white-space: nowrap;" />
                                 
                            </td>
                        </tr>
                        <tr>
                            <td>
                                To Date:
                            </td>
                            <td>
                                <input type="text" id="dtpToDate" style="height: 25px;border:1px solid #8dc060;"/><span style="color: red;">*</span>
                            </td>
                            <td style="padding-left:15px;">                                
                                <%--<input type="checkbox" id="chkBoxSalesAsset" name="outlet" value="asset"/>Sales Asset
                                wise &nbsp
                                <input type="checkbox" id="chkBoxRental" name="outlet" value="Outlet"/>Rental wise
                                &nbsp
                                <input type="checkbox" id="chkBoxNonRental" name="outlet" value="Outlet"/>Non Rental
                                wise<br>--%>
                            </td>
                            <td style="float:right;padding-top:1px;width:340px;padding-top:3px;">
                                Country:&nbsp;<select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlCountry' name='ddlCountry'>
                                    <option value='-1'>...Select...</option><option value='Bangladesh'>Bangladesh</option><option value='India'>India</option><option value='Nepal'>Nepal</option><option value='Malaysia'>Malaysia</option><option value='Oman'>Oman</option><option value='UAE'>UAE</option>
                                  </select>                                
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                
                            </td>
                            <td>
                            
                            </td>
                            <td style="float:right;padding-right:9px;padding-top:3px;width:331px;">
                                Division:&nbsp;<select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlDivision' name='ddlDivision'></select>
                            </td>
                            <td></td>
                        </tr>
                         <tr>
                            <td>
                            </td>
                            <td>
                                
                            </td>
                            <td>
                            
                            </td>
                            <td style="float:right;padding-right:9px;padding-top:3px;width:325px;">
                                Region:&nbsp;<select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlRegion' name='ddlRegion'></select>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                
                            </td>
                            <td>
                            
                            </td>
                            <td style="float:right;padding-right:9px;padding-top:3px;width:314px;">
                                Zone:&nbsp;<select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlZone' name='ddlZone'></select>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                
                            </td>
                            <td>
                            
                            </td>
                            <td style="float:right;padding-right:9px;padding-top:3px;width:319px;">
                                Route:&nbsp;<select style='height:28px;width:204px;border:1px solid #8dc060;' id='ddlRoute' name='ddlRoute'></select>
                            </td>
                            <td></td>
                        </tr>
                        <%--<tr>
                            <td>
                            </td>
                            <td>
                                
                            </td>
                            <td>
                            
                            </td>
                            <td style="float:right;padding-right:9px;padding-top:3px;width:336px;">
                                <span id="spnWise">SR(Opt):</span>&nbsp;<select style='height:28px;width:204px;border:1px solid #8dc060;' id='optOutlet' name='optOutlet'>   
                                    
                                </select>
                            </td>
                            <td></td>
                        </tr>
                        --%>
                         
                       
                        
                    </table>
                </div>
                
          
            </div>
           <%-- <div align="center">
                <div style="margin-top: 6px; background: #00ac45; border: 1px solid #ffccaa; width: 98.75%;
                    height: 30px;">
                    <table id="tblHeaderOutPic" style="height: 30px; color: #FFFFFF; width: 100%;">
                        <tr width='1000px'>
                            <td style="width:80px;">
                                SR ID 
                            </td>
                            <td style="width:250px;">
                                Outlet Name
                            </td>
                            <td style=';'>
                                Before
                            </td>
                            <td style=''>
                                After
                            </td>
                            <td style=''>
                                Others
                            </td>
                        </tr>
                    </table>
                </div>
            </div>--%>
        </div>
        <div align="center">
            <div id="divOutlet" style="border: 1px solid #ffccbb; width: 100%; margin-top: 200px;">
                <span id="spnMsg">There is no records! </span>
                <table id="tblOutlet" class="hovertable">
                
                </table>
            </div>
            <div align="center">
                <div id="spinner" class="spinner" style="display: none; margin-top: 20px;">
                    <img id="img-spinner" src="images/ajax-loader.gif" alt="Loading..." />
                </div>
            </div>
        </div>
       </div> 
    </form>
    </body>
</html>
