<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="Default2" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table><tr><td>
            <asp:Button ID="Button1" runat="server" Text="Show" OnClick="Button1_Click" /></td><td></td><td>
                <asp:Button ID="Button2" runat="server" Text="Print" /></td></tr></table>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewer1"  Width="700" Height="500" ShowPrintButton="true" SizeToReportContent="true" runat="server"></rsweb:ReportViewer>
    </div>
    </form>
</body>
</html>
