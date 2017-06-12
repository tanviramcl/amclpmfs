﻿<%@ Page Language="C#" MasterPageFile="~/UI/AMCLCommon.master" AutoEventWireup="true" CodeFile="FinalProcessing.aspx.cs" Inherits="BalanceUpdateProcess" Title="IAMCL Portfolio Market Price Update  (Design and Developed by Sakhawat)" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style6 {
            font-family: "Times New Roman";
            font-weight: bold;
        }

        .style8 {
            font-family: Verdana, Arial, Helvetica, sans-serif;
            font-size: 17px;
            color: #08559D;
            text-decoration: none;
            FONT-WEIGHT: bold;
            text-decoration: underline;
            height: 21px;
        }

        .style11 {
            height: 18px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" ID="ScriptManager1" />
    <table width="1100" cellpadding="0" cellspacing="0" border="0">
    </table>
    <table>
        <colgroup width="200"></colgroup>
        <colgroup width="220"></colgroup>
        <colgroup width="150"></colgroup>
        <tr>
            <td align="center" colspan="3" class="style8">Final Processing
            </td>
        </tr>
        <tr>
            <td colspan="3" align="left">&nbsp;&nbsp;</td>
        </tr>

        <tr>
            <td align="right" style="font-weight: 700">Balance Date</td>
            <td align="left" width="300px">
                <asp:TextBox ID="txtbalanceDate1" runat="server" Style="width: 195px;"
                    CssClass="textInputStyle"  TabIndex="7" AutoPostBack="True"
                    OnTextChanged="txtbalanceDate1_TextChanged"></asp:TextBox>
                 <ajaxToolkit:CalendarExtender ID="txtbalanceDate1_CalendarExtender1"
                    runat="server" TargetControlID="txtbalanceDate1"
                    PopupButtonID="ImageButton2" Format="dd-MMM-yyyy" />
                <asp:ImageButton ID="ImageButton2" runat="server"
                    AlternateText="Click Here" ImageUrl="~/Image/Calendar_scheduleHS.png"
                    TabIndex="24" />
            </td>
            <td align="left" width="300px">
                <asp:TextBox ID="txtbalanceDate2" runat="server" Style="width: 195px;"
                    CssClass="textInputStyle" TabIndex="7" AutoPostBack="True"
                    OnTextChanged="txtbalanceDate2_TextChanged"></asp:TextBox>

                <ajaxToolkit:CalendarExtender ID="txtbalanceDate2_CalendarExtender1"
                    runat="server" TargetControlID="txtbalanceDate2"
                    PopupButtonID="ImageButton1" Format="dd-MMM-yyyy" />
                <asp:ImageButton ID="ImageButton1" runat="server"
                    AlternateText="Click Here" ImageUrl="~/Image/Calendar_scheduleHS.png"
                    TabIndex="24" />
            </td>
        </tr>
        <tr>

            <td align="right" style="font-weight: 700"><b>Total Row Count:</b></td>
            <td align="left" width="200px">
                <asp:TextBox ID="txttotalRowCount" runat="server" Style="width: 195px;"
                    CssClass="textInputStyle" ReadOnly="true" TabIndex="7" AutoPostBack="True"
                    OnTextChanged="txttotalRowCount_TextChanged"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right" style="font-weight: 700">&nbsp;</td>
            <td align="left" width="200px">
                <asp:UpdateProgress ID="updProgress"
                    AssociatedUpdatePanelID="UpdatePanel1" runat="server">
                    <ProgressTemplate>
                        <img src="../Image/Processing.gif" alt="processing" style="width: 186px; height: 128px; margin-left: 36px" />

                    </ProgressTemplate>
                </asp:UpdateProgress>
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td align="right" style="font-weight: 700">&nbsp;</td>
            <td align="left" width="200px">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblProcessing" runat="server" Text="" Style="font-size: 24px; color: green;"></asp:Label>
                        <br />

                        <asp:Button ID="btnProcessingforBackup" runat="server" Style="width: 195px; height: 30px; font-size: 16px; margin-bottom: 20px;" Text="Processing for Backup" OnClick="btnProcessingforBackup_Click" />
                        <br />
                        <asp:Button ID="btnDelete" runat="server" Style="width: 195px; height: 30px; font-size: 16px;" Text="Delete" OnClick="btnDelete_Click" /></td>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="right" style="font-weight: 700"></td>
            <td align="left" width="200px"></td>
            <td></td>
        </tr>
    </table>
</asp:Content>

