<%@ Page Language="C#" MasterPageFile="~/UI/AMCLCommon.master" AutoEventWireup="true" CodeFile="CompanyWiseAllPortfoliosReportDSEonly.aspx.cs" Inherits="UI_CompanyWiseAllPortfoliosReportDSEonly" Title="Company Wise All Portfolios Report Form" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
        <script language="javascript" type="text/javascript"> 
                function fnReset()
                {
                    var confm=confirm("Are Sure To Reset?");
                    if(confm)
                    {   CheckAllDataGridFundName(this.checked=false)       
                        document.getElementById("<%=howlaDateDropDownList.ClientID%>").value ="0";
                        document.getElementById("<%=percentageTextBox.ClientID%>").value ="";
                        document.getElementById("<%=companyCodeTextBox.ClientID%>").value ="";
                        return false;
                    }
                    else
                    {
                        return true; 
                    }
                }
                function fncInputNumericValuesOnly()
	            {
		            if(!(event.keyCode==48||event.keyCode==49||event.keyCode==50||event.keyCode==51||event.keyCode==52||event.keyCode==53||event.keyCode==54||event.keyCode==55||event.keyCode==56||event.keyCode==57))
		            {
			            event.returnValue=false;
		            }
	            }
	            function fncInputCommaAndNumericValuesOnly()
	            {
		            if(!(event.keyCode==44||event.keyCode==48||event.keyCode==49||event.keyCode==50||event.keyCode==51||event.keyCode==52||event.keyCode==53||event.keyCode==54||event.keyCode==55||event.keyCode==56||event.keyCode==57))
		            {
			            event.returnValue=false;
		            }
	            }
                 function CheckAllDataGridFundName(checkVal)
                 {
                        if(document.getElementById("<%=grdShowFund.ClientID%>"))
                        {  
                            
                            var datagrid=document.getElementById("<%=grdShowFund.ClientID%>")
                               
                            var check = 0;                
                            
                            for( var rowCount = 0; rowCount < datagrid.rows.length; rowCount++)
                            {
                              var tr = datagrid.rows[rowCount];
                              var td= tr.childNodes[0]; 
                              var item = td.firstChild; 
                              var strType=item.type;
                              if(strType=="checkbox")
                              {
                                    item.checked = checkVal; 
                              }
                            }
                        }
                 }
              
                 function CalculateNoOfCheck(datagrid)
                 {
                    var noOfcheck = 0;                
                            
                        for( var rowCount = 0; rowCount < datagrid.rows.length; rowCount++)
                        {
                          var tr = datagrid.rows[rowCount];
                          var td= tr.childNodes[0]; 
                          var item = td.firstChild; 
                          var strType=item.type;
                          if(strType=="checkbox")
                          {
                            if(item.checked)
                            {
                             noOfcheck = noOfcheck + 1; 
                            }
                          }
                        }
                        return noOfcheck;
                 }
                 
              
                 function fnConfirm()
                 {
                        if(document.getElementById("<%=grdShowFund.ClientID%>"))
                        {
                            var datagridFund = document.getElementById("<%=grdShowFund.ClientID%>")
                            var noOfcheckFund =   CalculateNoOfCheck(datagridFund);
                        }
                        
                        if( noOfcheckFund > 0)
                        {
                            if(document.getElementById("<%=howlaDateDropDownList.ClientID%>").value =="0")
                            {
                                document.getElementById("<%=howlaDateDropDownList.ClientID%>").focus();
                                alert("Please Select Howla Date.");
                                return false; 
                            }
                            if(document.getElementById("<%=companyCodeTextBox.ClientID%>").value !="")
                            {  
                               var reg = /^\s*[\d]+(\,[\d]+)*\s*$/; 
                               if(!reg.test(document.getElementById("<%=companyCodeTextBox.ClientID%>").value))
                                {
                                    document.getElementById("<%=companyCodeTextBox.ClientID%>").focus();
                                    alert("Plese Insert Company Code(s) in Right Format.");
                                    return false;
                                }
                            }
                            var msg="Are You Sure to See The Selected Funds Report?";
                            var  conformMsg=confirm(msg);       
                            if(conformMsg)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            alert("Please Check Mark at least One Fund.");
                            return false;
                        }
                }       
    </script>
        <style type="text/css">
            .style4
            {
                font-family: Verdana, Arial, Helvetica, sans-serif;
                font-size: 17px;
                color: #08559D;
                FONT-WEIGHT: bold;
                text-align: center;
                background-image: url('../image/titlebg.gif');
                text-decoration: underline;
            }
            .style5
            {
                width: 123px;
                text-align: right;
            }
            .style6
            {
                color: #FF0066;
            }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" ID="ScriptManager1" />      
        
    <table width="600" align="center" cellpadding="0" cellspacing="0" >
        <tr>
        <td align="center" class="style4">Company Wise All Portfolios Report Form</td>  
        </tr>
    </table>
    
    <br />
    <br />
    <br />
    
        <table align="center" >
          <tr>
                
                <td>
                    <div style="height:310px; overflow:auto;" id="dvGridFund" runat="server" visible="false">
                        <asp:DataGrid id="grdShowFund" runat="server"  style="border: #666666 1px solid;"  AutoGenerateColumns="False" CellPadding="4">                               
                            <SelectedItemStyle HorizontalAlign="Center"></SelectedItemStyle>
                            <ItemStyle CssClass="TableText"></ItemStyle>
                            <HeaderStyle CssClass="DataGridHeader"></HeaderStyle>
                            <AlternatingItemStyle CssClass="AlternatColor"></AlternatingItemStyle>
                   
                            <Columns>    
                                 <asp:TemplateColumn>
                                    <HeaderTemplate>
                                        <input id="chkAllFund" type="checkbox" onclick="CheckAllDataGridFundName(this.checked)"> 
                                    </HeaderTemplate>
                                    <ItemTemplate> 
                                         <asp:CheckBox ID="chkFund" runat="server"></asp:CheckBox> 
                                    </ItemTemplate>
                                    <HeaderStyle Width="20px" />
                                </asp:TemplateColumn> 
                                     
                                <asp:BoundColumn HeaderText="ID" DataField="FUND_CODE" Visible="false"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="SI#" DataField="SI"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Fund Name" DataField="FUND_NAME"></asp:BoundColumn>
                            </Columns>          
                           </asp:DataGrid>
                       </div>
                </td>
            </tr>
        </table> 
        <br />
        <br />
        <table width="600" align="center" cellpadding="0" cellspacing="0" >
        <colgroup width="100"></colgroup>
            <tr>
                <td align="right" style="font-weight: 700" class="style5"><b>Howla Date:</b></td>
                <td align="left">
                    <asp:DropDownList ID="howlaDateDropDownList" runat="server" TabIndex="8"></asp:DropDownList>
                    <span class="style6">*</span>
                </td>
            </tr>
            <tr>
                <td align="right" style="font-weight: 700" class="style5"><b>Percentage Value:</b></td>
                <td align="left">
                    <asp:TextBox ID="percentageTextBox" runat="server" 
                        CssClass="textInputStyleDate" TabIndex="3" onkeypress= "fncInputNumericValuesOnly()"></asp:TextBox><span>%</span>
                </td>
            </tr>
            <tr>
                <td align="left" class="style5">Company Code(s):</td>
                <td align="left">
                    <asp:TextBox ID="companyCodeTextBox" runat ="server" TextMode="MultiLine" onkeypress= "fncInputCommaAndNumericValuesOnly()"
                        Width="450px" TabIndex="5"></asp:TextBox>
                         
                        <span>
                    <br />
                    Example: 101 or 101,105,210</span>
                </td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            
            <tr>
                <td align="center" class="style5">&nbsp;</td>
                <td align="left">
                    <asp:Button ID="showReportButton" runat="server" CssClass="buttoncommon" 
                        OnClientClick="return fnConfirm();" Text="Show Report" Width="78px" onclick="showReportButton_Click" 
                        />&nbsp;&nbsp;<asp:Button ID="resetButton" runat="server" CssClass="buttoncommon" 
                        OnClientClick="return fnReset();" Text="Reset" TabIndex="10" />&nbsp;
                    <asp:Button ID="CloseButton" runat="server" CssClass="buttoncommon" 
                        Text="Close" TabIndex="11" onclick="CloseButton_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
             <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
        </table>
       
    <br />
    <br /> 
</asp:Content>

