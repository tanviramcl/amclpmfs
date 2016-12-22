<%@ Page Language="C#" MasterPageFile="~/UI/AMCLCommon.master" AutoEventWireup="true" CodeFile="DateWiseTranjectionDSE.aspx.cs" Inherits="UI_PORTFOLIO_PortfolioMPUpdate" Title="IAMCL Portfolio Market Price Update  (Design and Developed by Sakhawat)" %>
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
    <ajaxToolkit:ToolkitScriptManager runat="Server" 
        EnableScriptGlobalization="True" ID="ScriptManager1" CombineScripts="True" />  
   

    
        <table width="1100" cellpadding="0" cellspacing="0" border="0" >
            </table>
    <table>
              <colgroup width="200"></colgroup>
              <colgroup width="220"></colgroup>
              <colgroup width="150"></colgroup>
        <tr>
            <td align="center" colspan="4" class="style8" >
                Date Wise Tranjection&nbsp; (DSE/CSE)&nbsp;  
            </td>
        </tr>
       
      <tr>
         <td colspan="4" align="left">&nbsp;&nbsp;</td>   
      </tr>
       <tr>
              <td align="right"><b>Stock Exchange :</b></td>  
              <td>
                  <asp:DropDownList ID="stockDropDownList" runat="server" Width="200px">
                    <asp:ListItem Text="Select Stock" Value="0"></asp:ListItem>
                    <asp:ListItem Text="DSE" Value="1"></asp:ListItem>
                    <asp:ListItem Text="CSE" Value="2"></asp:ListItem>
                  </asp:DropDownList>
              </td>

       </tr>
         &nbsp;
         <tr>
              <td align="right" style="font-weight: 700"><b>Fund code:</b></td>
              <td align="left" width="200px">
                  <asp:TextBox ID="txtFundcode" runat="server" style="width:195px;" 
                CssClass="textInputStyle" TabIndex="7" AutoPostBack="True" 
                ontextchanged="txtFundcode_TextChanged"></asp:TextBox>
              </td>
         </tr>
                     <tr>
                      <td align="right" style="font-weight: 700"><b>Howla Date From:</b></td>
                      <td align="left" width="200px">
                          <asp:TextBox ID="txtHowlaFrom" runat="server" style="width:195px;" 
                        CssClass="textInputStyle" TabIndex="7" AutoPostBack="True" 
                        ontextchanged="txtHowlaFrom_TextChanged"></asp:TextBox>
                      </td>
                         <td align="right" style="font-weight: 700"><b>Last Howla Date</b></td>
                        <td align="left" width="200px">
                         <asp:TextBox ID="txtHowlato" runat="server" style="width:195px;" 
                        CssClass="textInputStyle" TabIndex="7" AutoPostBack="True" 
                        ontextchanged="txtHowlato_TextChanged"></asp:TextBox>
                        </td>
                 </tr> 
            <tr>
                <td align="right" style="font-weight: 700"><b>Howla Date to:</b></td>
                <td align="left" width="200px">
                    <asp:TextBox ID="txtHowlaDate" runat="server" style="width:195px;" 
                CssClass="textInputStyle" TabIndex="7" AutoPostBack="True" 
                ontextchanged="txtHowlaDate_TextChanged"></asp:TextBox>
                </td>
            </tr>
              <tr>
                  <td align="right"><asp:Button ID="btnSave" runat="server" Text="Save"  CssClass="buttoncommon"/></td>
                  <td align="left">
                      <asp:Button ID="btnExit" runat="server" CssClass="buttoncommon" Text="Exit" /></td>
                 
              </tr>    
        </table>                
                  
 </asp:Content>

