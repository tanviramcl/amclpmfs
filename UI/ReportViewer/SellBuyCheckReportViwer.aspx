﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SellBuyCheckReportViwer.aspx.cs" Inherits="UI_ReportViewer_StockDeclarationBeforePostedReportViewer" Title="Sell Buy Check" %>
 <%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
   <div>
        <CR:CrystalReportViewer  ID="CR_SellBuyCheckReport"  Height="50px" runat="server" AutoDataBind="true" />
    </div>
     <div>
        <CR:CrystalReportViewer  ID="CrystalReportViewerfundwise"  Height="50px" runat="server" AutoDataBind="true" />
    </div>
    </form>
</body>
</html>
