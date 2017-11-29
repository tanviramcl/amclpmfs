<%@ Page Language="C#" MasterPageFile="~/UI/AMCLCommon.master" AutoEventWireup="true" CodeFile="NonListedSecuritiesInvestmentEntryForm.aspx.cs" Inherits="UI_NonListedSecuritiesInvestmentEntryForm" Title="Non Listed Securites (Investment) Entry Page" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="Stylesheet" type="text/css" href="../Scripts/jquery-ui.css"  />
    <link href="../CSS/vendor/bootstrap.min.css" rel="stylesheet" />
    <script  type="text/javascript" src="../Scripts/jquery-ui.js"></script>
    <style type="text/css">
        label.error {
            color: red;
            display: inline-flex;
        }

        .style5 {
            height: 24px;
        }

        .style6 {
            height: 14px;
        }
    </style>
     <style type="text/css">
         .Gridview {
             font-family: Verdana;
             font-size: 10pt;
             font-weight: normal;
             color: black;
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
        <td align="right" style="font-weight: 700" class="style5"><b>Company Name:&nbsp; </b></td>
        <td align="left" class="style5" >
            <asp:DropDownList ID="nonlistedCompanyDropDownList" runat="server" TabIndex="1" ></asp:DropDownList>
            </td>
    </tr>
    <tr>
       
        <td align="right" style="font-weight: 700" class="style5"><b>Investment Date:</b></td>
        <td align="left">
            <asp:TextBox ID="InvestMentDateTextBox" runat="server" Width="100px"></asp:TextBox>
            <span class="style6">*</span>
        </td>
    </tr>
    <tr>

       
       <%-- <td align="right" style="font-weight: 700" class="style5"><b>Nonlisted Category:</b></td>--%>
         <td align="right" style="font-weight: 700" class="style5"><asp:Label ID="Label2" runat="server" Text="Nonlisted Category:"></asp:Label></td>
        <td align="left">
            <asp:DropDownList ID="nonlistedCategoryDropDownList" runat="server" TabIndex="8"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td align="right" style="font-weight: 700" class="style5"><b>Amount(BDT):&nbsp;</b></td>
        <td align="left" class="style5" >
            <asp:TextBox ID="amountTextBox" runat="server"  style="width:100px;" 
                CssClass="textInputStyleammount" TabIndex="2" 
                ></asp:TextBox></td>   
        
    </tr>
     <tr>

        
            
        <td align="right" style="font-weight: 700" class="style5"><b>Rate(BDT):&nbsp;</b></td>
        <td align="left" class="style5" >
           <div> 
                <asp:UpdatePanel ID ="updt1" runat ="server" >
              <ContentTemplate >
            <asp:TextBox ID="rateTextBox" runat="server"  AutoPostBack="true"  style="width:100px" OnTextChanged=" rateChange_TextChanged"
                CssClass="textInputStyle" TabIndex="2" 
                ></asp:TextBox>
                   <asp:TextBox ID="TextBox1" runat="server"  style="width:100px;" AutoPostBack="true"
                CssClass="textInputStyle" TabIndex="2" 
                ></asp:TextBox>
                   </ContentTemplate >
                  
                  
                  </asp:UpdatePanel>
            </div>
                  </td>   
        
    </tr>
    
    <tr>
           <td align="left" class="style6" ></td>
    </tr>
    <tr>
        <td align="center" colspan="2" class="style5" ></td>
    </tr>
    <tr>
            <td align="center" colspan="2" >
            <asp:Button ID="saveButton" runat="server" Text="Add" 
                CssClass="buttoncommon" TabIndex="5" 
                     AccessKey="s" onclick="saveButton_Click"  OnClientClick="return Confirm();"
                    />
          <%-- <asp:Button ID="resetButton" runat="server" Text="Reset" 
                CssClass="buttoncommon" TabIndex="6" OnClientClick=" return fnReset();"
                />--%>
            </td>
            
    </tr>
    <tr>
           <td align="center" colspan="2" >
                &nbsp;
           </td>
    </tr>
</table>



    <script type="text/javascript">


       
   
       $.validator.addMethod("fundDropDownList", function (value, element, param) {  
           if (value == '0')  
               return false;  
           else  
               return true;  
       },"* Please select a Fund");


       $.validator.addMethod("pfoliodatecheck", function (value, element, param) {  
           if (value == '0')  
               return false;  
           else  
               return true;  
       },"* Please select a Date");
 
       $.validator.addMethod("companycheck", function (value, element, param) {  
           if (value == '0')  
               return false;  
           else  
               return true;  
       },"* Please select a Company");

       $.validator.addMethod("CategoryCheck", function (value, element, param) {  
           if (value == '0')  
               return false;  
           else  
               return true;  
       },"* Please select a Category");

       $(function () {

           $('#<%=InvestMentDateTextBox.ClientID%>').datepicker({
                  changeMonth: true,
                  changeYear: true,
                  dateFormat: "dd/mm/yy",
                  maxdate: 'today',
                  onSelect: function(selected) {
                   
                  }
              });
      
          });
    
       $("#aspnetForm").validate({
           rules: {
               <%=fundNameDropDownList.UniqueID %>: {
                        
                   //required:true 
                   fundDropDownList:true
                        
               }, <%=amountTextBox.UniqueID %>: {
                        
                   required:true ,
                   number:true              
                        
               }
               ,<%=nonlistedCategoryDropDownList.UniqueID %>: {
                        
                   CategoryCheck:true 
                        
               }, <%=rateTextBox.UniqueID %>: {
                        
                     required:true ,
                     number:true              
                        
                 }, <%=noOfShareTextBox.UniqueID %>: {
                        
                   required:true ,
                   number:true              
                        
               }
              
           }
       });

    </script>
    
               
</asp:Content>

