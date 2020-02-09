<%@ Page Language="C#" AutoEventWireup="true" CodeFile="grpdashboard.aspx.cs" Inherits="grpdashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sales-Mate</title>
    <link rel="shortcut icon" href="Nimages/logopran.gif" />
    <link href="Ncss/jquery-ui.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="CSS/basic.css" />
    <link rel="stylesheet" type="text/css" href="CSS/styles.css" />
    <link rel="stylesheet" href="CSS/date-picker-css.css" />


    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.8.0/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="//www.google.com/jsapi"></script>


    <script src="JSfordate/jquery-1.10.2.js"></script>
    <script src="JSfordate/jquery-ui.js"></script>



    <script type="text/javascript">

        google.load('visualization', '1', { packages: ['corechart'] });

    </script>

    <style>
        .ui-datepicker {
            width: 17em;
            padding: .2em .2em 0;
            display: none;
            background: #846733;
        }

            .ui-datepicker .ui-datepicker-title {
                margin: 0 2.3em;
                line-height: 1.8em;
                text-align: center;
                color: #FFFFFF;
                background: #846733;
            }

            .ui-datepicker table {
                width: 100%;
                font-size: .7em;
                border-collapse: collapse;
                font-family: verdana;
                margin: 0 0 .4em;
                color: #000000;
                background: #FDF8E4;
            }

            .ui-datepicker td {
                border: 0;
                padding: 1px;
            }

                .ui-datepicker td span, .ui-datepicker td a {
                    display: block;
                    padding: .8em;
                    text-align: right;
                    text-decoration: none;
                }
    </style>

    <style type="text/css">
        .plc::-webkit-input-placeholder {
            color: #b2cde0;
        }

        .btn-success {
            background-color: #5cb85c;
            border-color: #4cae4c;
            color: #fff;
        }

        .boxbg5 {
            background-color: #FFF;
            padding: 1px;
            height: 50px;
            color: #ffffff;
            text-align: center;
            font-size: 14px;
            box-shadow: 0 1px 3px rgba(0,0,0,0.12), 0 1px 2px rgba(0,0,0,0.24);
            border-radius: 5px;
            margin-top: -30px;
        }

        .btn {
            -moz-user-select: none;
            background-image: none;
            border: 1px solid transparent;
            border-radius: 4px;
            cursor: pointer;
            display: inline-block;
            font-size: 14px;
            font-weight: 400;
            line-height: 1.42857;
            margin-bottom: 0;
            padding: 6px 12px;
            text-align: center;
            vertical-align: middle;
            white-space: nowrap;
        }

        ul {
            list-style-type: none;
            margin: 0;
            padding: 0;
            overflow: hidden;
            background-color: #607d8b;
        }

            ul li {
                float: left;
                border-right: 1px solid #bbb;
            }

                ul li:last-child {
                    border-right: none;
                }

                ul li a {
                    display: block;
                    color: white;
                    text-align: center;
                    padding: 14px 10px;
                    font-size: 14px;
                    text-decoration: none;
                }

                    ul li a:hover:not(.active) {
                        background-color: #8dc060;
                    }

        .active {
            background-color: #8dc060;
        }

        #content {
            float: left;
            padding-top: 10px;
            padding-left: 5px;
            width: 95.5%;
        }

        #middle {
            width: 580px; /* Account for margins + border values */
            float: left;
            padding: 5px;
        }

        #sidebar {
            width: 97%;
            padding-top: 10px;
            float: right;
            padding-right: 5px;
        }

        #pagewrap {
            padding-top: 5px;
            width: 1135px;
            margin: -14px auto;
        }

        .border-bottom {
            border-bottom: 1px solid #bbb;
        }

        .title {
            display: inline-block;
            height: 25px;
        }

        .txtColor {
            border: 1px solid #5cb85c;
            height: 23px;
        }
    </style>

    <script type="text/javascript" charset="utf-8">
        var $j = jQuery.noConflict();

        $j(document).ready(function () {

            $j('#dvMiddleContent').html('');
            $j('#dvMiddleContent').html("<div style='width:59.8%;height:auto;margin-top: 2px;border:1px solid #8dc060;padding-top:10px;margin-left:19%;'>" +
                                  "<table style='padding-top:10px;padding-bottom: 10px;padding-left:16%'>" +
                                    "<tr><td>From Date :</td><td><input type='text' id='txtFromDate' placeholder='DD/MM/YYYY' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td></td>" +
                                    "<td>To Date :</td><td><input type='text' id='txtToDate' placeholder='DD/MM/YYYY' style='border:1px solid #8dc060;width:200px;height:25px;'/></td><td></td><td></td><td><button type='button' id='btnLoad' style='background-color: #8dc060;border-color: #4cae4c;color: #fff;-moz-user-select: none;background-image: none;border: 1px solid transparent;border-radius: 4px;cursor: pointer;display: inline-block;font-size: 14px;font-weight: 400;line-height: 1.42857;margin-bottom: 0;padding: 6px 12px;text-align: center;vertical-align: middle;white-space: nowrap;'>Load</button></td></tr>" +
                                     
                                  "</table>" +
                                "</div>");


            var d = new Date();
            var month = d.getMonth() + 1;
            var day = d.getDate();
            var currentDate = (day < 10 ? '0' : '') + day + '/' + (month < 10 ? '0' : '') + month + '/' + d.getFullYear();
            $j("#txtFromDate").val(currentDate);
            $j("#txtFromDate").datepicker({ showAnim: "fold", dateFormat: "dd/mm/yy" });
            $j("#txtToDate").val(currentDate);
            $j("#txtToDate").datepicker({ showAnim: "fold", dateFormat: "dd/mm/yy" });

            
            
            $j("#btnLoad").click(function (e) {
                e.preventDefault();
                $j.ajax({

                    type: 'POST',

                    dataType: 'json',

                    contentType: 'application/json',

                    url: 'chartinfo.aspx/GetNationalSalesData',

                    data: "{fromDate:'" + $j("#txtFromDate").val() + "',toDate:'" + $j("#txtToDate").val() + "'}",

                    success:

                        function (response) {

                            drawVisualization(response.d);

                        }

                });

                $j.ajax({

                    type: 'POST',

                    dataType: 'json',

                    contentType: 'application/json',

                    url: 'chartinfo.aspx/GetDivisionalSalesData',

                    data: "{fromDate:'" + $j("#txtFromDate").val() + "',toDate:'" + $j("#txtToDate").val() + "'}",

                    success:

                        function (response) {

                            drawDivisionalSales(response.d);

                        }

                });

                $j.ajax({

                    type: 'POST',

                    dataType: 'json',

                    contentType: 'application/json',

                    url: 'chartinfo.aspx/GetNationalSalesvsTargetData',

                    data: "{fromDate:'" + $j("#txtFromDate").val() + "',toDate:'" + $j("#txtToDate").val() + "'}",

                    success:

                        function (response) {

                            drawNationalSalesVsTarget.val(response.d);

                        }

                });

                
            });

            function drawVisualization(dataValues) {

                var data = new google.visualization.DataTable();

                data.addColumn('string', 'Column Name');

                data.addColumn('number', 'Column Value');

                for (var i = 0; i < dataValues.length; i++) {

                    data.addRow([dataValues[i].ColumnName, dataValues[i].Value]);

                }

                new google.visualization.PieChart(document.getElementById('visualization')).

                    draw(data, { title: "Group wise National Sales Status" });

            }


            function drawDivisionalSales(dataValues) {

                var data = new google.visualization.DataTable();

                data.addColumn('string', 'Column Name');

                data.addColumn('number', 'Column Value');

                for (var i = 0; i < dataValues.length; i++) {
                    data.addRow([dataValues[i].ColumnName, dataValues[i].Value]);
                }

                new google.visualization.PieChart(document.getElementById('divisionalSales')).

                    draw(data, { title: "Division wise Sales Status" });

            }


            function drawNationalSalesVsTarget(dataValues) {
                var data = new google.visualization.DataTable();

                data.addColumn('string', 'Column Name');
                data.addColumn('number', 'Column Value');
                data.addColumn('number', 'Column Value');

                for (var i = 0; i < dataValues.length; i++) {
                    data.addRow([dataValues[i].ColumnName, dataValues[i].Value]);
                }

                new google.visualization.ColumnChart(document.getElementById('nationaltarget')).

                    draw(data, { title: "National Target vs Sales Status" });
            }

        });

 </script>
 
</head>
<body>
    <body>
    <form id="form1" runat="server">
        <div id="bodywrapper">
            <div id="header">
                <div>
                    <img src="images/pran-logo.png" width="80" height="25" title="Upper Sales" /></div>

            </div>
            <div style="background: #8dc060; height: 49px;">

                <ul>
                    <li><a class="active" href="login.aspx" id="lnkHome">Home</a></li>

                    <li style="float: right;"><a href="#"><span id="logeduser" style="color: #ff9800;"></span></a></li>
                    <li style="float: right"><a href="#" id="lnkLogout">Log Out</a></li>
                </ul>
            </div>
            <div id="contentwrapper">
                <div style="background: rgba(0,0,0,0.12); width: 100%; height: auto;">

                    <div style="width: 100%; height: auto; float: right;">
                        <div id="dvMiddleContent" style="border: 1px solid #8dc060; width: 100%; height: auto; float: left; padding-bottom: 5px;">
                        </div>
                
                        <div style="width:100%; height: auto;">
                        <div id="visualization" style="width:49%; height: 450px;margin-top:5px;float:left;border:1px solid #ff9800;"></div>
                        <div id="divisionalSales" style="width:49%; height: 450px;margin-top:5px;float:right;border:1px solid #ff9800;"></div>
                        </div>
                    </div>

                    <div style="width: 100%; height: auto; float: right;margin-top:20px;">
                        <div id="dvMiddleContentS" style="border: 1px solid #8dc060; width: 100%; height: auto; float: left; padding-bottom: 5px;">
                        </div>
                
                        <div style="width:100%; height: auto;">
                        <div id="nationaltarget" style="width:49%; height: 450px;margin-top:5px;float:left;border:1px solid #ff9800;">
                          <asp:Literal ID="lt" runat="server"></asp:Literal>
                            <div id="chart_div"></div> 
                        </div>
                        <div id="divisionalSalesS" style="width:49%; height: 450px;margin-top:5px;float:right;border:1px solid #ff9800;"></div>
                        </div>
                    </div>

                </div>

                <div style="clear: both"></div>
                <div style="line-height: 14px; margin-top: 15px;">
                </div>
            </div>
        </div>

        <div id="footerwrapper">
            <div id="footer">
                <a href="login.aspx" style="color: white; text-decoration: none;">Home | </a><a href="" style="color: white; text-decoration: none;">About us | </a><a href="" style="color: white; text-decoration: none;">Contact us </a>
                <p>
                    Telephone: (0088) 01924602673&nbsp;&nbsp;&nbsp;&nbsp;Any time: 01710649448<br />
                    Copyright &copy; 2016 Website: www.pranrflgroup.com Email: info@pranrflgroup.com   
        <div style="text-align: right; margin-top: -14px;"><a href="http://www.pranrflgroup.com" target="_new" style="color: white; text-decoration: none;">Design & Developed by Rathindra Nath</a></div>
            </div>
        </div>
    </form>
</body>
</body>
</html>
