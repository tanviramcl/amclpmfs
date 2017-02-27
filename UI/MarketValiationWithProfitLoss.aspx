<%@ Page Language="C#" MasterPageFile="~/UI/AMCLCommon.master" AutoEventWireup="true" CodeFile="MarketValiationWithProfitLoss.aspx.cs" Inherits="UI_MarketValiationWithProfitLoss" Title="Market Valiation With Profit Loss" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
        <script language="javascript" type="text/javascript"> 
                function fnReset()
                {
                    var confm=confirm("Are Sure To Reset?");
                    if(confm)
                    {   CheckAllDataGridFundName(this.checked=false)       
                        document.getElementById("<%=PortfolioAsOnDropDownList.ClientID%>").value ="0";
                        
                        return false;
                    }
                    else
                    {
                        return true; 
                    }
                }
               
                 function CheckAllDataGridFundName(checkVal)
                 {
                        if(document.getElementById("<%=grdShowFund.ClientID%>"))
                        {  
                            
                            var datagrid = document.getElementById("<%=grdShowFund.ClientID%>");
                               
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
                          if(strType=='checkbox')
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

                            
                            var datagridFund = document.getElementById("<%=grdShowFund.ClientID%>");
                           
                            var noOfcheckFund =   CalculateNoOfCheck(datagridFund);
                        }
                        
                        if( noOfcheckFund > 0)
                        {
                            if(document.getElementById("<%=PortfolioAsOnDropDownList.ClientID%>").value =="0")
                            {
                                document.getElementById("<%=PortfolioAsOnDropDownList.ClientID%>").focus();
                                alert("Please Select Howla Date.");
                                return false; 
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
        <td align="center" class="style4">Market Valuation with Profit/Loss</td>  
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
                <td align="right" style="font-weight: 700" class="style5"><b>Portfolio As On:</b></td>
                <td align="left">
                    <asp:DropDownList ID="PortfolioAsOnDropDownList" runat="server" TabIndex="8"></asp:DropDownList>
                    <span class="style6">*</span>
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

