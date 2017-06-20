<%@ Page Language="C#" MasterPageFile="~/UI/AMCLCommon.master" AutoEventWireup="true" CodeFile="BalanceUpdateProcess.aspx.cs" Inherits="BalanceUpdateProcess" Title="IAMCL Portfolio Market Price Update  (Design and Developed by Sakhawat)" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="javascript" type="text/javascript"> 
    
     function fnCheckInput()
    {   
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
               Balance Update Process  
            </td>
        </tr>
       
      <tr>
         <td colspan="4" align="left">&nbsp;&nbsp;</td>   
      </tr>
    
         <tr>
              <td align="right" style="font-weight: 700"><b>Fund Name:</b></td>
              <td align="left" width="200px">
                  <asp:DropDownList ID="fundNameDropDownList" runat="server" TabIndex="6" 
                onselectedindexchanged="fundNameDropDownList_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
              </td>
         </tr>
                     <tr>
                      <td align="right" style="font-weight: 700"><b>Balance Date:</b></td>
                      <td align="left" width="300px">
                          <asp:TextBox ID="txtBalanceDate" runat="server" style="width:195px;" 
                        CssClass="textInputStyle" TabIndex="7" AutoPostBack="True"
                        ></asp:TextBox>
                          <ajaxToolkit:CalendarExtender ID="txtBalanceDate_CalendarExtender"
                              runat="server" TargetControlID="txtBalanceDate"
                              PopupButtonID="ImageButton" Format="dd-MMM-yyyy" />
                          <asp:ImageButton ID="ImageButton" runat="server"
                              AlternateText="Click Here" ImageUrl="~/Image/Calendar_scheduleHS.png"
                              TabIndex="24" />
                           
                       </td>
                         <td align="right" style="font-weight: 700"><b>Last Update Date</b></td>
                        <td align="left" width="300px">
                         <asp:TextBox ID="txtLastUpadateDate" runat="server" style="width:195px;" 
                        CssClass="textInputStyle" TabIndex="7" AutoPostBack="True" 
                       ></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="txtlastUpadateDate_CalendarExtender1"
                                runat="server" TargetControlID="txtlastUpadateDate"
                                PopupButtonID="ImageButton1" Format="dd-MMM-yyyy" />
                            <asp:ImageButton ID="ImageButton1" runat="server"
                                AlternateText="Click Here" ImageUrl="~/Image/Calendar_scheduleHS.png"
                                TabIndex="24" />
                        </td>
                 </tr> 
            <tr>
                <td align="right" style="font-weight: 700"><b>No of Purchase Record:</b></td>
                <td align="left" width="200px">
                    <asp:TextBox ID="txtNoPurchaseRecord" runat="server" style="width:195px;" 
                CssClass="textInputStyle" TabIndex="7" AutoPostBack="True" BackColor="#DDDDDD" ReadOnly="True" 
                ></asp:TextBox>
                </td>
            </tr>
             <tr>
                    <td align="right" style="font-weight: 700"><b>No of Purchase Shares:</b></td>
                <td align="left" width="200px">
                    <asp:TextBox ID="txtNoPurchaseShares" runat="server" style="width:195px;" 
                CssClass="textInputStyle" TabIndex="7" AutoPostBack="True" BackColor="#DDDDDD" ReadOnly="True" 
               ></asp:TextBox>
                </td>
              </tr>   
               <tr>
                    
                   <td align="right" style="font-weight: 700"><b>No sale Record:</b></td>
                    <td align="left" width="200px">
                        <asp:TextBox ID="txtNoSaleRecord" runat="server" style="width:195px;" 
                    CssClass="textInputStyle" TabIndex="7" AutoPostBack="True" BackColor="#DDDDDD" ReadOnly="True" 
                    ></asp:TextBox>
                    </td>
                   <td>
                       
                    </td>
                   <td>
                       <asp:TextBox ID="txtLastBalDate" runat="server"  style="width:195px;" 
                    CssClass="textInputStyle" TabIndex="7" AutoPostBack="True" BackColor="#DDDDDD" 
                   ></asp:TextBox>
                    </td>
                    
                    
                </tr> 
        <tr>

            <td align="right" style="font-weight: 700"><b>No sale Shares:</b></td>
            <td align="left" width="200px">
                <asp:TextBox ID="txtNoOfSaleShare" runat="server" Style="width: 195px;"
                    CssClass="textInputStyle" TabIndex="7" AutoPostBack="True" BackColor="#DDDDDD" ReadOnly="True"
                   ></asp:TextBox>
            </td>
             <td>
                       
            </td>
             <td>
                <asp:TextBox ID="txtMarketPriceDate" runat="server"  style="width:195px;" 
            CssClass="textInputStyle" TabIndex="7" AutoPostBack="True" BackColor="#DDDDDD" ReadOnly="True" 
           ></asp:TextBox>
            </td>

        </tr> 


              <tr>
                  <td align="right">&nbsp;</td>
                  <td align="left">
                      <%--<asp:Button ID="btnSave" runat="server" Text="Save" 
                       CssClass="buttoncommon" OnClick="btnSave_Click" OnClientClick=" return fnCheckInput();"/>--%></td>
                 
              </tr>    


              <tr>
                  <td align="right">&nbsp;</td>
                  <td align="left">
                      <div>
       
        <asp:UpdateProgress ID="updProgress"
        AssociatedUpdatePanelID="UpdatePanel1"
        runat="server">
            <ProgressTemplate>   
                <img src="../Image/Processing.gif" alt="processing" style="width: 186px; height: 128px; margin-left: 36px" />      
               
            </ProgressTemplate>
        </asp:UpdateProgress>
       
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblProcessing" runat="server" Text="" style="font-size:24px; color:green;"></asp:Label>
                <br />
               
                <asp:Button ID="btnSave" runat="server" Text="Save" 
                       CssClass="buttoncommon" OnClick="btnSave_Click" OnClientClick=" return fnCheckInput();"/>
            </ContentTemplate>
        </asp:UpdatePanel>        
    </div></td>
                 
              </tr>    
        </table>                
                  
 </asp:Content>

