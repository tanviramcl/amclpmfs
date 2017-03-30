<%@ Page Language="C#"  MasterPageFile="~/UI/AMCLCommon.master" AutoEventWireup="true" CodeFile="CapitalGainSummeryDateWise.aspx.cs" Inherits="UI_BalancechekReport" Title="Capital Gain Summery Date Wise" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <%--<ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true" EnableScriptLocalization="true" ID="ScriptManager1" />--%>
    <asp:ScriptManager ID="ScriptManager1" EnableScriptGlobalization="true" EnableScriptLocalization="true" runat="server"></asp:ScriptManager>
    <table style="text-align: center">
        <tr>
            <td class="FormTitle" align="center">Capital Gain Summery Date Wise</td>
            <td>
                <br />
            </td>
        </tr>

    </table>
      <table style="text-align: center">
        <tr>
            <td align="right" style="font-weight: 700"><b>P Date 1:</b></td>
            <td align="left">
                <asp:DropDownList ID="p1dateDropDownList" runat="server" TabIndex="6">
                </asp:DropDownList>
            </td>
        </tr>
          <tr>
            <td align="right" style="font-weight: 700"><b>P Date 2:</b></td>
            <td align="left">
                <asp:DropDownList ID="p2dateDropDownList" runat="server" TabIndex="6">
                </asp:DropDownList>
            </td>
        </tr>

    </table>
    <table width="750" style="text-align: center" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td align="center" colspan="2">
                <asp:Button ID="showButton" runat="server" Text="Show Report"
                    CssClass="buttoncommon" TabIndex="5" OnClick="showButton_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
