﻿<%@ Page Language="C#" MasterPageFile="~/UI/AMCLCommon.master" AutoEventWireup="true" CodeFile="DateWiseTransaction.aspx.cs" Inherits="DateWiseTransaction" Title="IAMCL Portfolio Market Price Update  (Design and Developed by Sakhawat)" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="javascript" type="text/javascript"> 
    
   
    
    function fnCheckInput()
    {   
       
        if(document.getElementById("<%=stockExchangeDropDownList.ClientID%>").value =="0")
        {
            document.getElementById("<%=stockExchangeDropDownList.ClientID%>").focus();
            alert("Please Select Stock Exchange.");
            return false; 
        }
        if(document.getElementById("<%=fundNameDropDownList.ClientID%>").value =="0")
        {
            document.getElementById("<%=fundNameDropDownList.ClientID%>").focus();
            alert("Please Select Fund Name.");
            return false; 
        }

      
        
    }
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
                Date Wise Transaction&nbsp; (DSE/CSE)&nbsp;  
            </td>
        </tr>
       
      <tr>
         <td colspan="4" align="left">&nbsp;&nbsp;</td>   
      </tr>
       <tr>
              <td align="right"><b>Stock Exchange :</b></td>  
              <td>
                  <asp:DropDownList ID="stockExchangeDropDownList" runat="server" 
                TabIndex="3">
            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
            <asp:ListItem Text="DSE" Value="D"></asp:ListItem>
            <asp:ListItem Text="CSE" Value="C"></asp:ListItem>
            </asp:DropDownList>
              </td>

       </tr>
         &nbsp;
         <tr>
              <td align="right" style="font-weight: 700"><b>Fund Name:</b></td>
              <td align="left" width="200px">
                   <asp:DropDownList ID="fundNameDropDownList" runat="server" TabIndex="6"  AutoPostBack="true"
                onselectedindexchanged="fundNameDropDownList_SelectedIndexChanged"></asp:DropDownList>
            <%--<ajaxToolkit:ListSearchExtender ID="ListSearchExtender" runat="server" TargetControlID="fundNameDropDownList"
                QueryPattern="Contains" QueryTimeout="2000">
            </ajaxToolkit:ListSearchExtender>--%>

              </td>
         </tr>
                     <tr>
                      <td align="right" style="font-weight: 700"><b>Howla Date From:</b></td>
                      <td align="left" width="300px">
                          <asp:TextBox ID="txtHowlaDateFrom" runat="server" style="width:195px;" 
                        CssClass="textInputStyle" TabIndex="7" AutoPostBack="True"  ReadOnly="true"
                        ontextchanged="txtHowlaDateFrom_TextChanged"></asp:TextBox>
                      <%--  <ajaxToolkit:CalendarExtender ID="txtHowlaDateFrom_CalendarExtender"
                            runat="server" TargetControlID="txtHowlaDateFrom"
                            PopupButtonID="ImageButton" Format="dd-MMM-yyyy" />
                        <asp:ImageButton ID="ImageButton" runat="server"
                            AlternateText="Click Here" ImageUrl="~/Image/Calendar_scheduleHS.png"
                            TabIndex="24" />--%>

                      </td>
                         <td align="right" style="font-weight: 700"><b>Last Howla Date</b></td>
                        <td align="left" width="300px">
                         <asp:TextBox ID="txtLastHowlaDate" ReadOnly="true" runat="server" style="width:195px;" 
                        CssClass="textInputStyle" TabIndex="7" AutoPostBack="True"></asp:TextBox>
                            <%-- <ajaxToolkit:CalendarExtender ID="txtLastHowlaDate_CalendarExtender1"
                            runat="server" TargetControlID="txtLastHowlaDate" 
                            PopupButtonID="ImageButton1" Format="dd-MMM-yyyy" />
                        <asp:ImageButton ID="ImageButton1" runat="server"
                            AlternateText="Click Here" ImageUrl="~/Image/Calendar_scheduleHS.png"
                            TabIndex="24" />--%>
                        </td>
                 </tr> 
            <tr>
                <td align="right" style="font-weight: 700"><b>Howla Date to:</b></td>
                <td align="left" width="200px">
                    <asp:TextBox ID="txtHowlaDateTo" runat="server" style="width:195px;" 
                CssClass="textInputStyle" TabIndex="7" AutoPostBack="True"   ReadOnly="true"
                ontextchanged="txtHowlaDateTo_TextChanged"></asp:TextBox>
                  <%--  <ajaxToolkit:CalendarExtender ID="txtHowlaDateTo_CalendarExtender1"
                        runat="server" TargetControlID="txtHowlaDateTo"
                        PopupButtonID="ImageButton2" Format="dd-MMM-yyyy" />
                    <asp:ImageButton ID="ImageButton2" runat="server"
                        AlternateText="Click Here" ImageUrl="~/Image/Calendar_scheduleHS.png"
                        TabIndex="24" />--%>
                </td>
            </tr>
             <tr>
                    <td align="right" style="font-weight: 700"><b>Voucher Number:</b></td>
                <td align="left" width="200px">
                    <asp:TextBox ID="txtVoucherNumber" runat="server" style="width:195px;"  MaxLength="10"
                CssClass="textInputStyle" TabIndex="7" AutoPostBack="False" 
                ></asp:TextBox>
                </td>
              </tr>   

              <tr>
                  <td align="right"><asp:Button ID="btnSave" runat="server" Text="Save"  CssClass="buttoncommon"  EnableViewState="false" OnClientClick=" return fnCheckInput();" OnClick="btnSave_Click"/></td>
                  <td align="left">
                      &nbsp;</td>
                 
              </tr>    
        </table>                
                  
 </asp:Content>

