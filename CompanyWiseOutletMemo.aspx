<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompanyWiseOutletMemo.aspx.cs" Inherits="CompanyWiseOutletMemo" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sales-Mate Outlet Memo Report</title>
    <link rel="shortcut icon" href="Nimages/logopran.gif" />
    <link href="Ncss/jquery-ui.css" rel="stylesheet" />    
</head>
<body>
    <form id="form1" runat="server">
    <div id="dvReport">
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" ToggleSidePanel="None" ShowToggleSidePanelButton="False" HasToggleGroupTreeButton="False" />
        <CR:CrystalReportSource ID="CrystalReportSource1" runat="server"></CR:CrystalReportSource>
    
        <br />
      
    </div>
    </form>
</body>
</html>
