<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NestedRptGroupWiseSRDetails.aspx.cs" Inherits="NestedRptGroupWiseSRDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sales Mate</title>

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

            $("[id*=imgFifthGrid]").each(function () {
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
            <div id="header" style="background-color: #8dc060;">
                <div>
                    <img src="images/pran-logo.png" width="80" height="25" title="Sales Mate" />
                    <span style='color: #d62d20; font-size: 20px; font-weight: bold;'>Sales Dashboard Tree</span>
                </div>
            </div>
            <asp:GridView ID="firstGrid" runat="server" AutoGenerateColumns="false" CssClass="Grid" PageSize="5" DataKeyNames="ITEM_GROUP_ID">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="imgFirstGrid" runat="server" OnClick="Show_Hide_FirstLevel_Grid"
                                ImageUrl="~/images/plus.png" CommandArgument="Show" />
                            <asp:Panel ID="pnlFirst" runat="server" Visible="false" Style="position: relative">
                                <asp:GridView ID="secondGrid" runat="server" AutoGenerateColumns="false" PageSize="20"
                                    AllowPaging="true" OnPageIndexChanging="OnSecondLevel_PageIndexChanging" CssClass="ChildGrid" DataKeyNames="ITEM_GROUP_ID,DIVISION_ID">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgSecondGrid" runat="server" OnClick="Show_Hide_SecondLevel_Grid"
                                                    ImageUrl="~/images/plus.png" CommandArgument="Show" />
                                                <asp:Panel ID="pnlSecond" runat="server" Visible="false" Style="position: relative">
                                                    <asp:GridView ID="thirdGrid" runat="server" AutoGenerateColumns="false" PageSize="15"
                                                        AllowPaging="true" OnPageIndexChanging="OnThirdLevel_PageIndexChanging" CssClass="Nested_ChildGrid" DataKeyNames="ITEM_GROUP_ID,DIVISION_ID,ZONE_ID">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgThirdGrid" runat="server" OnClick="Show_Hide_ThirdLevel_Grid"
                                                                        ImageUrl="~/images/plus.png" CommandArgument="Show" />
                                                                    <asp:Panel ID="pnlThird" runat="server" Visible="false" Style="position: relative">
                                                                        <asp:GridView ID="fourthGrid" runat="server" AutoGenerateColumns="false" PageSize="30"
                                                                            AllowPaging="true" OnPageIndexChanging="OnFourthLevel_PageIndexChanging" CssClass="ChildGrid" DataKeyNames="SR_ID">
                                                                            <Columns>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:ImageButton ID="imgFourthGrid" runat="server" OnClick="Show_Hide_FourthLevel_Grid"
                                                                                            ImageUrl="~/images/plus.png" CommandArgument="Show" />
                                                                                        <asp:Panel ID="pnlFourth" runat="server" Visible="false" Style="position: relative">
                                                                                            <asp:GridView ID="fifthGrid" runat="server" AutoGenerateColumns="false" PageSize="5"
                                                                                                AllowPaging="true" OnPageIndexChanging="OnFifthLevel_PageIndexChanging" CssClass="ChildGrid" DataKeyNames="SR_ID,ITEM_ID">
                                                                                                <Columns>
                                                                                                    <asp:TemplateField>
                                                                                                        <ItemTemplate>
                                                                                                            <asp:ImageButton ID="imgFifthGrid" runat="server" OnClick="Show_Hide_FifthLevel_Grid"
                                                                                                                ImageUrl="~/images/plus.png" CommandArgument="Show" />
                                                                                                            <asp:Panel ID="pnlFifth" runat="server" Visible="false" Style="position: relative">
                                                                                                                <asp:GridView ID="sixthGrid" runat="server" AutoGenerateColumns="false" PageSize="5"
                                                                                                                    AllowPaging="true" OnPageIndexChanging="OnSixthLevel_PageIndexChanging" CssClass="ChildGrid" DataKeyNames="ITEM_ID">
                                                                                                                    <Columns>


                                                                                                                        <asp:BoundField ItemStyle-Width="150px" DataField="OUTLET_NAME" HeaderText="Outlet" />
                                                                                                                        <asp:BoundField ItemStyle-Width="150px" DataField="SALESTIME" HeaderText="Sales Time" />
                                                                                                                        <asp:BoundField ItemStyle-Width="150px" DataField="CARTON" HeaderText="CTN" />
                                                                                                                        <asp:BoundField ItemStyle-Width="150px" DataField="PIECE" HeaderText="Pcs" />
                                                                                                                        <asp:BoundField ItemStyle-Width="150px" DataField="OUT_PRICE" HeaderText="Price" />
                                                                                                                        <asp:BoundField ItemStyle-Width="150px" DataField="TOTAL_AMT" HeaderText="Total Amount" />

                                                                                                                    </Columns>
                                                                                                                </asp:GridView>
                                                                                                            </asp:Panel>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>

                                                                                                    <asp:BoundField ItemStyle-Width="150px" DataField="ITEM_ID" HeaderText="Item ID" />
                                                                                                    <asp:BoundField ItemStyle-Width="150px" DataField="ITEM_NAME" HeaderText="Item Name" />
                                                                                                    <asp:BoundField ItemStyle-Width="150px" DataField="CARTON" HeaderText="Ctn" />
                                                                                                    <asp:BoundField ItemStyle-Width="150px" DataField="PIECE" HeaderText="Pcs" />
                                                                                                    <asp:BoundField ItemStyle-Width="150px" DataField="OUT_PRICE" HeaderText="Price" />
                                                                                                    <asp:BoundField ItemStyle-Width="150px" DataField="TOTAL_AMT" HeaderText="Total Amount" />

                                                                                                </Columns>
                                                                                            </asp:GridView>
                                                                                        </asp:Panel>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField ItemStyle-Width="150px" DataField="SR_ID" HeaderText="SR ID" />
                                                                                <asp:BoundField ItemStyle-Width="150px" DataField="SR_NAME" HeaderText="SR Name" />
                                                                                <asp:BoundField ItemStyle-Width="150px" DataField="ROUTE_NAME" HeaderText="ROUTE_NAME" />
                                                                                <asp:BoundField ItemStyle-Width="150px" DataField="TOTAL_OUTLET" HeaderText="Total Outlet" />
                                                                                <asp:BoundField ItemStyle-Width="150px" DataField="VISITED_OUTLET" HeaderText="Visited Outlet" />
                                                                                <asp:BoundField ItemStyle-Width="150px" DataField="TOTAL_MEMO" HeaderText="Total Memo" />
                                                                                <asp:BoundField ItemStyle-Width="150px" DataField="TOTAL_AMOUNT" HeaderText="Total Amount" />
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:BoundField ItemStyle-Width="150px" DataField="ZONE_NAME" HeaderText="ZONE" />
                                                            <asp:BoundField ItemStyle-Width="150px" DataField="SRCOUNT_ACTIVE" HeaderText="Total SR" />
                                                            <asp:BoundField ItemStyle-Width="150px" DataField="SRCOUNT_TODAY" HeaderText="On Field" />
                                                            <asp:BoundField ItemStyle-Width="150px" DataField="SRCOUNT_INACTIVE" HeaderText="In Active" />
                                                            <asp:BoundField ItemStyle-Width="120px" DataField="LEAVE_FROM_JOB" HeaderText="Resigned" />
                                                            <asp:BoundField ItemStyle-Width="150px" DataField="SRCOUNT_ONLEAVE" HeaderText="On Leave" />
                                                            <asp:BoundField ItemStyle-Width="120px" DataField="TOTAL_OUTLET" HeaderText="Total Outlet" />
                                                            <asp:BoundField ItemStyle-Width="120px" DataField="VISITED_OUTLET" HeaderText="Visited Outlet" />
                                                            <asp:BoundField ItemStyle-Width="120px" DataField="TOTAL_MEMO" HeaderText="Total Memo" />
                                                            <asp:BoundField ItemStyle-Width="120px" DataField="LPC" HeaderText="LPC" />
                                                            <asp:BoundField ItemStyle-Width="120px" DataField="STRIKE_RATE" HeaderText="STRIKE_RATE" />
                                                            <asp:BoundField ItemStyle-Width="120px" DataField="TOTAL" HeaderText="Total" />
                                                            <asp:BoundField ItemStyle-Width="120px" DataField="SR_AVARAGE_ORDER" HeaderText="SR Avg. Order" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-Width="150px" DataField="DIVISION_NAME" HeaderText="Division Name" />
                                        <asp:BoundField ItemStyle-Width="150px" DataField="SRCOUNT_ACTIVE" HeaderText="Total SR" />
                                        <asp:BoundField ItemStyle-Width="150px" DataField="SRCOUNT_TODAY" HeaderText="On Field" />
                                        <asp:BoundField ItemStyle-Width="150px" DataField="SRCOUNT_INACTIVE" HeaderText="In Active" />
                                        <asp:BoundField ItemStyle-Width="120px" DataField="LEAVE_FROM_JOB" HeaderText="Resigned" />
                                        <asp:BoundField ItemStyle-Width="150px" DataField="SRCOUNT_ONLEAVE" HeaderText="On Leave" />
                                        <asp:BoundField ItemStyle-Width="120px" DataField="TOTAL_OUTLET" HeaderText="Total Outlet" />
                                        <asp:BoundField ItemStyle-Width="120px" DataField="VISITED_OUTLET" HeaderText="Visited Outlet" />
                                        <asp:BoundField ItemStyle-Width="120px" DataField="TOTAL_MEMO" HeaderText="Total Memo" />
                                        <asp:BoundField ItemStyle-Width="120px" DataField="LPC" HeaderText="LPC" />
                                        <asp:BoundField ItemStyle-Width="120px" DataField="STRIKE_RATE" HeaderText="STRIKE_RATE" />
                                        <asp:BoundField ItemStyle-Width="120px" DataField="TOTAL" HeaderText="Total Amount" />
                                        <asp:BoundField ItemStyle-Width="120px" DataField="SR_AVARAGE_ORDER" HeaderText="SR Avg. Order" />
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-Width="120px" DataField="ITEM_GROUP_NAME" HeaderText="ITEM GROUP" />
                    <asp:BoundField ItemStyle-Width="120px" DataField="SRCOUNT_ACTIVE" HeaderText="Total SR" />
                    <asp:BoundField ItemStyle-Width="120px" DataField="SRCOUNT_TODAY" HeaderText="On Field" />
                    <asp:BoundField ItemStyle-Width="120px" DataField="SRCOUNT_INACTIVE" HeaderText="In Active" />
                    <asp:BoundField ItemStyle-Width="120px" DataField="LEAVE_FROM_JOB" HeaderText="Resigned" />
                    <asp:BoundField ItemStyle-Width="120px" DataField="SRCOUNT_ONLEAVE" HeaderText="On Leave" />
                    <asp:BoundField ItemStyle-Width="120px" DataField="TOTAL_OUTLET" HeaderText="Total Outlet" />
                    <asp:BoundField ItemStyle-Width="120px" DataField="VISITED_OUTLET" HeaderText="Visited Outlet" />
                    <asp:BoundField ItemStyle-Width="120px" DataField="TOTAL_MEMO" HeaderText="Total Memo" />
                    <asp:BoundField ItemStyle-Width="120px" DataField="LPC" HeaderText="LPC" />
                    <asp:BoundField ItemStyle-Width="120px" DataField="STRIKE_RATE" HeaderText="Strike Rate" />
                    <asp:BoundField ItemStyle-Width="120px" DataField="TOTAL" HeaderText="Total Amount" />
                    <asp:BoundField ItemStyle-Width="120px" DataField="SR_AVARAGE_ORDER" HeaderText="SR Avg. Order" />

                </Columns>
            </asp:GridView>

        </div>
    </form>
</body>
</html>
