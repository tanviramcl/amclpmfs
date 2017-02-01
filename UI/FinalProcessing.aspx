<%@ Page Language="C#" MasterPageFile="~/UI/AMCLCommon.master" AutoEventWireup="true" CodeFile="FinalProcessing.aspx.cs" Inherits="BalanceUpdateProcess" Title="IAMCL Portfolio Market Price Update  (Design and Developed by Sakhawat)" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script language="javascript" type="text/javascript"> 
    
  
  </script>
   <style type="text/css">
        .style6
        {
            font-family: "Times New Roman";
            font-weight: bold;
        }
        .style8
        {
            font-family: Verdana, Arial, Helvetica, sans-serif;
            font-size: 17px;
            color: #08559D;
            text-decoration: none;
            FONT-WEIGHT: bold;
            text-decoration: underline;
            /*padding-right: 5;
            padding-bottom: 3;*/
            height: 21px;
        }  
       .style11
       {
           height: 18px;
       }
    </style>
 

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">  
   
     <ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" ID="ScriptManager1" />    
    
        <table width="1100" cellpadding="0" cellspacing="0" border="0" >
            </table>
    <table>
              <colgroup width="200"></colgroup>
              <colgroup width="220"></colgroup>
              <colgroup width="150"></colgroup>
        <tr>
            <td align="center" colspan="4" class="style8" >
              Final Processing
            </td>
        </tr>
       
      <tr>
         <td colspan="4" align="left">&nbsp;&nbsp;</td>   
      </tr>
            
            <tr>

            <td align="right" style="font-weight: 700"><b>Total Row Count:</b></td>
            <td align="left" width="200px">
                 <asp:TextBox ID="txttotalRowCount" runat="server" style="width:195px;" 
                CssClass="textInputStyle" ReadOnly="true" TabIndex="7" AutoPostBack="True" 
                ontextchanged="txttotalRowCount_TextChanged"></asp:TextBox>
            </td>
           </tr> 
            <tr>
                <td align="right" style="font-weight: 700">Balance Date</td>
                <td align="left" width="200px">
                    <asp:TextBox ID="txtbalanceDate1" runat="server" style="width:195px;" 
                CssClass="textInputStyle" ReadOnly="true" TabIndex="7" AutoPostBack="True" 
                ontextchanged="txtbalanceDate1_TextChanged"></asp:TextBox>
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
                <td align="right" style="font-weight: 700"></td>
                <td align="left" width="200px">
                     <asp:Button ID="btnProcessingforBackup" runat="server" style="width:195px;height: 20px;"  CssClass="buttoncommon" Text="Processing for Back Up" OnClick="btnProcessingforBackup_Click" />
                </td>
                <td>
                    <asp:Button ID="btnDelete" runat="server"  style="width:195px;height: 20px;" CssClass="buttoncommon" Text="Delete" OnClick="btnDelete_Click" />
                 </td>

            </tr>
        </table>                
                  
 </asp:Content>

