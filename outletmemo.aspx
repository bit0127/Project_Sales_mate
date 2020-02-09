<%@ Page Language="C#" AutoEventWireup="true" CodeFile="outletmemo.aspx.cs" Inherits="outletmemo" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sales-Mate Outlet Memo Report</title>
    <link rel="shortcut icon" href="Nimages/logopran.gif" />
    <link href="Ncss/jquery-ui.css" rel="stylesheet" />    
</head>
  <%--  <script type="text/javascript">
        function Print() {
            var dvReport = document.getElementById("dvReport");
            var frame1 = dvReport.getElementsByTagName("iframe")[0];
            if (navigator.appName.indexOf("Internet Explorer") != -1 || navigator.appVersion.indexOf("Trident") != -1) {
                frame1.name = frame1.id;
                window.frames[frame1.id].focus();
                window.frames[frame1.id].print();
            } else {
                var frameDoc = frame1.contentWindow ? frame1.contentWindow : frame1.contentDocument.document ? frame1.contentDocument.document : frame1.contentDocument;
                frameDoc.print();
            }
        }
</script> --%>
<body>
    <form id="form1" runat="server">
    <div id="dvReport">    
            
        <CR:CrystalReportViewer ID="CrystalReportViewer1" ToggleSidePanel="None" ShowToggleSidePanelButton="False" runat="server" AutoDataBind="true" HasToggleGroupTreeButton="False" />
        <CR:CrystalReportSource ID="CrystalReportSource1" runat="server"></CR:CrystalReportSource>

        <br />
      
    </div>

        
    <%--<input id="btnPrint" type="button" value="Print" onclick="Print()" />  --%>
    </form>
</body>
</html>
