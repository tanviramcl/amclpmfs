<%@ Page Language="C#" MasterPageFile="~/UI/AMCLCommon.master" AutoEventWireup="true" CodeFile="NonListedSecuritiesInvestmentEntryForm.aspx.cs" Inherits="UI_NonListedSecuritiesInvestmentEntryForm" Title="Non Listed Securites (Investment) Entry Page" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script language="javascript" type="text/javascript"> 
    function fnReset()
    {
        var confrm=confirm("Are You Sure To Reset?");
        if(confrm)
        {
            document.getElementById("<%=fundNameDropDownList.ClientID%>").value ="0"; 
            document.getElementById("<%=amountTextBox.ClientID%>").value =""; 
            document.getElementById("<%=investmentDateTextBox.ClientID%>").value =""; 
            return false;
        }
        else
        {   
            return true;
        }
            
    }
    
    function fnCheckInput()
    {   
        if(document.getElementById("<%=fundNameDropDownList.ClientID%>").value =="0")
        {
            document.getElementById("<%=fundNameDropDownList.ClientID%>").focus();
            alert("Please Select Fund Name.");
            return false; 
        }
        if(document.getElementById("<%=amountTextBox.ClientID%>").value =="")
        {
            document.getElementById("<%=amountTextBox.ClientID%>").focus();
            alert("Please Enter Investment Amount.");
            return false; 
        }
        if(document.getElementById("<%=investmentDateTextBox.ClientID%>").value =="")
        {
            document.getElementById("<%=investmentDateTextBox.ClientID%>").focus();
            alert("Please Insert Investment Date.");
            return false; 
        }
        if(document.getElementById("<%=investmentDateTextBox.ClientID%>").value!="")
        {
            var checkDate=/^([012]?\d|3[01])-([Jj][Aa][Nn]|[Ff][Ee][bB]|[Mm][Aa][Rr]|[Aa][Pp][Rr]|[Mm][Aa][Yy]|[Jj][Uu][Nn]|[Jj][Uu][Ll]|[aA][Uu][gG]|[Ss][eE][pP]|[Oo][Cc][Tt]|[Nn][Oo][Vv]|[Dd][Ee][Cc])-(19|20)\d\d$/;
            if(!checkDate.test(document.getElementById("<%=investmentDateTextBox.ClientID%>").value))
            {
                document.getElementById("<%=investmentDateTextBox.ClientID%>").focus();
                alert("Invalid Date Format! Select Investment Date From The Calender.");
                return false;
            }
        }
    }
    function fncInputNumericValuesOnly()
	{
		if(!(event.keyCode==46||event.keyCode==48||event.keyCode==49||event.keyCode==50||event.keyCode==51||event.keyCode==52||event.keyCode==53||event.keyCode==54||event.keyCode==55||event.keyCode==56||event.keyCode==57))
		{
			event.returnValue=false;
		}
	}
</script>
    <style type="text/css">
        .style5
        {
            height: 24px;
        }
        .style6
        {
            height: 14px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true" EnableScriptLocalization="true" ID="ScriptManager1" />

&nbsp;&nbsp;&nbsp;
<table "text-align"="center">
    <tr>
        <td class="FormTitle" align="center">
            NON LISTED SECURITIES (Investment) ENTRY FORM
        </td>           
        <td>
            <br />
        </td>
    </tr> 
</table>
<br />
<table width="750" "text-align"="center" cellpadding="0" cellspacing="0" border="0">
    
    <tr>
        <td align="right" style="font-weight: 700" class="style5"><b>Fund Name:&nbsp; </b></td>
        <td align="left" class="style5" >
            <asp:DropDownList ID="fundNameDropDownList" runat="server" TabIndex="1" ></asp:DropDownList>
            </td>
    </tr>
    <tr>
        <td align="right" style="font-weight: 700" class="style5"><b>Amount(BDT):&nbsp;</b></td>
        <td align="left" class="style5" >
            <asp:TextBox ID="amountTextBox" runat="server" style="width:100px;" 
                CssClass="textInputStyle" TabIndex="2" onkeypress= "fncInputNumericValuesOnly()"
                ></asp:TextBox></td>   
        
    </tr>
    <tr>
        <td align="right" style="font-weight: 700" class="style5"><b>Investment Date:&nbsp;</b></td>
        <td align="left" class="style5">
            <asp:TextBox ID="investmentDateTextBox" runat="server" style="width:100px;" 
                CssClass="textInputStyle" TabIndex="3"></asp:TextBox>
            <ajaxToolkit:CalendarExtender ID="CalendarExtender" runat="server" TargetControlID="investmentDateTextBox"
                PopupButtonID="ImageButton" Format="dd-MMM-yyyy"/>
            <asp:ImageButton ID="ImageButton" runat="server" AlternateText="Click Here" 
                ImageUrl="~/Image/Calendar_scheduleHS.png" TabIndex="4" /></td>
    </tr>
    <tr>
           <td align="left" class="style6" ></td>
           <td align="left" class="style6" >(dd-MMM-yyyy)</td>
    </tr>
    <tr>
        <td align="center" colspan="2" class="style5" >
    </tr>
    <tr>
            <td align="center" colspan="2" >
            <asp:Button ID="saveButton" runat="server" Text="Save" 
                CssClass="buttoncommon" TabIndex="5" OnClientClick=" return fnCheckInput();" 
                     AccessKey="s" onclick="saveButton_Click" 
                    />
            <asp:Button ID="resetButton" runat="server" Text="Reset" 
                CssClass="buttoncommon" TabIndex="6" OnClientClick=" return fnReset();"
                />
            </td>
            
    </tr>
    <tr>
           <td align="center" colspan="4" >
                &nbsp;
           </td>
    </tr>
</table>            
</asp:Content>

