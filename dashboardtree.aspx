<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dashboardtree.aspx.cs" Inherits="NestedRptGroupWiseSRDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sales-Mate</title>
    <link rel="shortcut icon" href="Nimages/logopran.gif" />
    <style type="text/css">
        body {
            font-family: Arial;
            font-size: 10pt;
        }

        .Grid td {
            background-color: #A1DCF2;
            color: black;
            font-size: 10pt;
            line-height: 200%;
        }

        .Grid th {
            background-color: #3AC0F2;
            color: White;
            font-size: 10pt;
            line-height: 200%;
        }

        .ChildGrid td {
            background-color: #eee !important;
            color: black;
            font-size: 10pt;
            line-height: 200%;
        }

        .ChildGrid th {
            background-color: #6C6C6C !important;
            color: White;
            font-size: 10pt;
            line-height: 200%;
        }

        .Nested_ChildGrid td {
            background-color: #fff !important;
            color: black;
            font-size: 10pt;
            line-height: 200%;
        }

        .Nested_ChildGrid th {
            background-color: #2B579A !important;
            color: White;
            font-size: 10pt;
            line-height: 200%;
        }
    </style>
    
    
    <link href="Ncss/jquery-ui.css" rel="stylesheet" />    
   <%-- <link href="Ncss/bootstrap-min2.css" rel="stylesheet" type="text/css"/>--%>
   

    <link rel="stylesheet" type="text/css" href="CSS/basic.css" />
    <link rel="stylesheet" type="text/css" href="CSS/lightbox.css" />
    <link rel="stylesheet" type="text/css" href="CSS/Format.css" />
    <link rel="stylesheet" type="text/css" href="CSS/styles.css" />
    <link rel="stylesheet" type="text/css" href="CSS/demo.css" />
    <link rel="stylesheet" type="text/css" href="CSS/sidemenu.css" />
    
    <link href="NgridCss/dataTables.jqueryui.min.css" rel="stylesheet" />    
    <link href="NgridCss/buttons.jqueryui.min.css" rel="stylesheet" />

       <style type="text/css">
        

        .datepicker {
           margin: 10px;
           padding: 2px;
           position: absolute;
           width: 261px;
           background-color: #fff;
           border: 1px solid #ccc;
           -webkit-border-radius: 4px;
           -moz-border-radius: 4px;
           border-radius: 4px;
        }
        div#month-wrap {
           height: 30px;
           background-color: #ddd;
           border: 1px solid black;
           -webkit-border-radius: 4px;
           -moz-border-radius: 4px;
           border-radius: 4px;
        }
        div#bn_prev {
           margin: 3px;
           float: left;
           width: 24px;
           height: 24px;
        }
        div#bn_next {
           margin: 3px;
           float: right;
           width: 24px;
           height: 24px;
        }

        div#bn_prev:hover,
        div#bn_prev:focus,
        div#bn_next:hover,
        div#bn_next:focus {
           margin: 2px;
           background-color: #fc3;
           border: 1px solid #800;
           -webkit-border-radius: 4px;
           -moz-border-radius: 4px;
           border-radius: 4px;
        }
        img.bn_img {
           margin: 0;
           padding: 2px;
        }
        div#month {
           float: left;
           padding-top: 6px;
           width: 199px;
           height: 24px;
           text-align: center;
           font-weight: bold;
           font-size: 1.2em;
        }
        table#cal {
           width: 261px;
           font-size: 1.2em;
           text-align: center;
        }
        table#cal th,
        table#cal td {
           width: 35px;
           height: 30px;
           padding: 0;
        }
        table#cal td {
           background-color: #ddd;
           border: 1px solid #999;
        }

        table#cal td.today {
           background-color: #FFF0C4;
           border: 1px solid #999;
        }

        table#cal td.empty {
           background-color: #f9f9f9;
           border: 1px solid #eee;
        }

        table#cal td:hover,
        table#cal td.focus {
           border-color: #800;
           background-color: #fc3;
        }

        table#cal td.empty:hover {
           background-color: #f9f9f9;
           border: 1px solid #eee;
        }

        .offscreen {
           position: absolute;
           left: -200em;
           top: -100em;
        }
        [aria-hidden="true"] {
           display: none;
        }


        .plc::-webkit-input-placeholder
        {
            color:#b2cde0;
        }
        
        .btn-success {
            background-color: #5cb85c;
            border-color: #4cae4c;
            color: #fff;
            
        }
        .boxbg5
        {
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
            border-right:1px solid #bbb;
        }

        ul li:last-child {
            border-right: none;
        }

        ul li a {
            display: block;
            color: white;
            text-align: center;
            padding: 14px 16px;
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
        
        .border-bottom
        {
            border-bottom: 1px solid #bbb;
        }
        
        .title{
            display: inline-block;
            height: 25px;
        }
        
        .txtColor
        {
            border:1px solid #5cb85c;       
            height:23px;      
        }
        
     </style>

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $("[id*=imgFirstGrid]").each(function () {
                if ($(this)[0].src.indexOf("minus") != -1) {
                    $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
                    $(this).next().remove();
                }
            });
            $("[id*=imgSecondGrid]").each(function () {
                if ($(this)[0].src.indexOf("minus") != -1) {
                    $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
                    $(this).next().remove();
                }
            });
            $("[id*=imgThirdGrid]").each(function () {
                if ($(this)[0].src.indexOf("minus") != -1) {
                    $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
                    $(this).next().remove();
                }
            });

            $("[id*=imgFourthGrid]").each(function () {
                if ($(this)[0].src.indexOf("minus") != -1) {
                    $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
                    $(this).next().remove();
                }
            });

        });
    </script>
</head>
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
                    <li><a href="#">About Us</a></li>
                    <li><a href="#">Policies</a></li>
                    <li><a href="dashboard.aspx">Dashboard</a></li>
                    <li style="float: right"><a href="#" id="lnkLogout">Log Out</a></li>
                </ul>
            </div>
            <div id="contentwrapper">
                <div style="background: rgba(0,0,0,0.12); width: 100%; height: auto;">

                    <div style="width: 100%; height: auto; float: right;">
                        <div id="dvMiddleContent" style="border: 1px solid #8dc060; width: 99.2%; height: auto; float: left; padding-bottom: 15px; margin-bottom: 10px;">
                          <asp:GridView ID="firstGrid" runat="server" AutoGenerateColumns="false" CssClass="Grid" PageSize="5" DataKeyNames="ITEM_GROUP_ID">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgFirstGrid" runat="server" OnClick="Show_Hide_FirstLevel_Grid"
                                            ImageUrl="~/images/plus.png" CommandArgument="Show" />
                                        <asp:Panel ID="pnlFirst" runat="server" Visible="false" Style="position: relative">
                                            <asp:GridView ID="secondGrid" runat="server" AutoGenerateColumns="false" PageSize="5"
                                                AllowPaging="true" OnPageIndexChanging="OnSecondLevel_PageIndexChanging" CssClass="ChildGrid" DataKeyNames="ITEM_GROUP_ID,DIVISION_ID">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgSecondGrid" runat="server" OnClick="Show_Hide_SecondLevel_Grid"
                                                                ImageUrl="~/images/plus.png" CommandArgument="Show" />
                                                            <asp:Panel ID="pnlSecond" runat="server" Visible="false" Style="position: relative">
                                                                <asp:GridView ID="thirdGrid" runat="server" AutoGenerateColumns="false" PageSize="4"
                                                                    AllowPaging="true" OnPageIndexChanging="OnThirdLevel_PageIndexChanging" CssClass="Nested_ChildGrid" DataKeyNames="ITEM_GROUP_ID,DIVISION_ID,ZONE_ID">
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="imgThirdGrid" runat="server" OnClick="Show_Hide_ThirdLevel_Grid"
                                                                                    ImageUrl="~/images/plus.png" CommandArgument="Show" />
                                                                                <asp:Panel ID="pnlThird" runat="server" Visible="false" Style="position: relative">
                                                                                    <asp:GridView ID="fourthGrid" runat="server" AutoGenerateColumns="false" PageSize="20"
                                                                                        AllowPaging="true" OnPageIndexChanging="OnFourthLevel_PageIndexChanging" CssClass="ChildGrid" DataKeyNames="SR_ID">
                                                                                        <Columns>
                                                                                            <asp:TemplateField>
                                                                                                <ItemTemplate>
                                                                                                    <asp:ImageButton ID="imgFourthGrid" runat="server" OnClick="Show_Hide_FourthLevel_Grid"
                                                                                                        ImageUrl="~/images/plus.png" CommandArgument="Show" />
                                                                                                    <asp:Panel ID="pnlFourth" runat="server" Visible="false" Style="position: relative">
                                                                                                        <asp:GridView ID="fifthGrid" runat="server" AutoGenerateColumns="false" PageSize="20"
                                                                                                            AllowPaging="true" OnPageIndexChanging="OnFifthLevel_PageIndexChanging" CssClass="ChildGrid" DataKeyNames="ITEM_ID">
                                                                                                            <Columns>

                                                                                                                <asp:BoundField ItemStyle-Width="150px" DataField="ITEM_ID" HeaderText="Item ID" />
                                                                                                                <asp:BoundField ItemStyle-Width="150px" DataField="ITEM_NAME" HeaderText="Item Name" />
                                                                                                                <asp:BoundField ItemStyle-Width="150px" DataField="TOTAL_AMT" HeaderText="Total Amount" />


                                                                                                            </Columns>
                                                                                                        </asp:GridView>
                                                                                                    </asp:Panel>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:BoundField ItemStyle-Width="150px" DataField="SR_ID" HeaderText="SR ID" />
                                                                                            <asp:BoundField ItemStyle-Width="150px" DataField="SR_NAME" HeaderText="SR Name" />

                                                                                        </Columns>
                                                                                    </asp:GridView>
                                                                                </asp:Panel>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:BoundField ItemStyle-Width="150px" DataField="ZONE_NAME" HeaderText="ZONE" />
                                                                        <asp:BoundField ItemStyle-Width="150px" DataField="SRCOUNT_ACTIVE" HeaderText="Total Active SR" />
                                                                        <asp:BoundField ItemStyle-Width="150px" DataField="SRCOUNT_ONLEAVE" HeaderText="On Leave" />
                                                                        <asp:BoundField ItemStyle-Width="150px" DataField="SRCOUNT_TODAY" HeaderText="On Field" />
                                                                        <asp:BoundField ItemStyle-Width="120px" DataField="TOTAL" HeaderText="Total" />
                                                                        <asp:BoundField ItemStyle-Width="120px" DataField="SR_AVARAGE_ORDER" HeaderText="SR Avg. Order" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField ItemStyle-Width="150px" DataField="DIVISION_NAME" HeaderText="Division Name" />
                                                    <asp:BoundField ItemStyle-Width="150px" DataField="SRCOUNT_ACTIVE" HeaderText="Total Active SR" />
                                                    <asp:BoundField ItemStyle-Width="150px" DataField="SRCOUNT_ONLEAVE" HeaderText="On Leave" />
                                                    <asp:BoundField ItemStyle-Width="150px" DataField="SRCOUNT_TODAY" HeaderText="On Field" />
                                                    <asp:BoundField ItemStyle-Width="120px" DataField="TOTAL" HeaderText="Total" />
                                                    <asp:BoundField ItemStyle-Width="120px" DataField="SR_AVARAGE_ORDER" HeaderText="SR Avg. Order" />
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField ItemStyle-Width="120px" DataField="ITEM_GROUP_NAME" HeaderText="ITEM GROUP" />
                                <asp:BoundField ItemStyle-Width="120px" DataField="SRCOUNT_ACTIVE" HeaderText="Total Active SR" />
                                <asp:BoundField ItemStyle-Width="120px" DataField="SRCOUNT_ONLEAVE" HeaderText="On Leave" />
                                <asp:BoundField ItemStyle-Width="120px" DataField="SRCOUNT_TODAY" HeaderText="On Field" />
                                <asp:BoundField ItemStyle-Width="120px" DataField="TOTAL" HeaderText="Total" />
                                <asp:BoundField ItemStyle-Width="120px" DataField="SR_AVARAGE_ORDER" HeaderText="SR Avg. Order" />

                            </Columns>
                        </asp:GridView>
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
                    Telephone: (0088) 01924602673 &nbsp;&nbsp;&nbsp;&nbsp;Any time: 01710649448<br />
                    Copyright &copy; 2016 Website: www.pranrflgroup.com Email: info@pranrflgroup.com   
        <div style="text-align: right; margin-top: -14px;"><a href="http://www.pranrflgroup.com" target="_new" style="color: white; text-decoration: none;">Design & Developed by Rathindra Nath</a></div>
            </div>
        </div>
    </form>
</body>
</html>
