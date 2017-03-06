<%@ Page Language="C#" MasterPageFile="~/UI/AMCLCommon.master" AutoEventWireup="true" CodeFile="CompanyInformation.aspx.cs" Inherits="UI_CompanyInformation" Title="Company Information" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
  <style type ="text/css" >  
        label.error {             
            color: red;   
            display:inline-flex ;                 
        }  
    </style> 
  

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
 
       <ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true" EnableScriptLocalization="true" ID="ScriptManager1" />
   
        <div align="center">
            <table id="Table1" width = "600" align = "center" cellpadding ="0" cellspacing ="0" runat="server">
            <tr>
                <td align="center" class="style3">
                    <b><u>Company Information</u></b>
                </td>
            </tr>
            </table>
    
             <table id="Table2" width="600" align="center" cellpadding="2" cellspacing="2" runat="server">
            <tr>
                <td align="left">
                    <b>Company code </b>
                </td>
                <td align="left">
                    <asp:TextBox ID="companyCodeTextBox" runat="server" Width="100px" OnTextChanged="companyCodeTextBox_TextChanged"  AutoPostBack="true"></asp:TextBox>
                </td>

                <td align="left">
                    <b>Company Name </b>
                </td>
                <td align="left">
                    <asp:TextBox ID="companyNameTextBox" runat="server" Width="100px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <b>Sector </b>
                </td>
                <td align="left">
                    <asp:TextBox ID="sectorTextBox" runat="server" Width="100px"></asp:TextBox>
                </td>

                <td align="left">
                    <b>Category </b>
                </td>
                <td align="left">
                     <asp:DropDownList ID="categoryDropDownList" Width="100px" runat="server"
                        TabIndex="3">
                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Share" Value="SH"></asp:ListItem>
                        <asp:ListItem Text="Mutual Fund" Value="DB"></asp:ListItem>
                        <asp:ListItem Text="S" Value="S"></asp:ListItem>
                        <asp:ListItem Text="L" Value="L"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <b>Market Lot </b>
                </td>
                <td align="left">
                    <asp:TextBox ID="MarketLotTextBox" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td align="left">
                    <b>Avarage Market Rate</b>
                </td>
                <td align="left">
                    <asp:TextBox ID="avarageMarketRateTextBox" runat="server" Width="100px" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <b>Face value</b>
                </td>
                <td align="left">
                    <asp:TextBox ID="faceValueTextBox" runat="server" Width="100px" ></asp:TextBox>
                </td>

                <td align="left">
                    <b>Last Trading date</b>
                </td>
                <td align="left">
                    <asp:TextBox ID="lasttradingdateTextBox" runat="server" Width="100px" ReadOnly="True"></asp:TextBox>
                    <asp:ImageButton ID="lasttradingdateTextBoxImageButton" runat="server"
                        ImageUrl="~/Image/Calendar_scheduleHS.png" TabIndex="7" />
                    <ajaxToolkit:CalendarExtender ID="lasttradingdateTextBoxCalendarExtender" runat="server" Format="dd-MMM-yyyy" PopupButtonID="lasttradingdateTextBoxImageButton" TargetControlID="lasttradingdateTextBox"></ajaxToolkit:CalendarExtender>
                </td>
            </tr>
            <tr>


                <td align="left">
                    <b>Dse code</b>
                </td>
                <td align="left">
                    <asp:TextBox ID="dsecodeTextBox" runat="server" Width="100px"></asp:TextBox>
                </td>

                <td align="left">
                    <b>CSE code</b>
                </td>
                <td align="left">
                    <asp:TextBox ID="csecodeTextBox" runat="server" Width="100px" ReadOnly="True"></asp:TextBox>
                </td>


            </tr>
            <tr>
                <td align="left">
                    <b>Group type</b>
                </td>
                <td>
                    <asp:DropDownList ID="groupDropDownList" Width="100px" runat="server"
                        TabIndex="3">
                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                        <asp:ListItem Text="A Group" Value="A"></asp:ListItem>
                        <asp:ListItem Text="B Group" Value="B"></asp:ListItem>
                        <asp:ListItem Text="Z Group" Value="Z"></asp:ListItem>


                    </asp:DropDownList>
                </td>
                 <td align="left">
                    <b>Flag</b>
                </td>
                <td align="left">
                    <asp:TextBox ID="flugTextBox" runat="server" Width="100px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <b>Address1</b>
                </td>
                <td align="left">
<%--                    <asp:TextBox ID="addressTextBox1" runat="server" Width="170px" Height="80px"></asp:TextBox>--%>
                    <textarea id="addressTextBox1" cols="20" rows="2" runat="server"></textarea>
                </td>
                   <td align="left">
                    <b>Address2</b>
                </td>
                <td align="left">
                  <%--  <asp:TextBox ID="addressTextBox2" runat="server" Width="170px" Height="80px"></asp:TextBox>--%>
                       <textarea id="addressTextBox2" cols="20" rows="2" runat="server"></textarea>
                </td>
            </tr>
            <tr>
             
                  <td align="left">
                    <b>Opening Date</b>
                </td>
                <td align="left">
                    <asp:TextBox ID="openingdateTextBox" runat="server" Width="100px"></asp:TextBox>
                    <asp:ImageButton ID="openingdateTextBoxImageButton" runat="server"
                        ImageUrl="~/Image/Calendar_scheduleHS.png" TabIndex="7" />
                    <ajaxToolkit:CalendarExtender ID="openingdateTextBoxCalendarExtender" runat="server" Format="dd-MMM-yyyy" PopupButtonID="openingdateTextBoxImageButton" TargetControlID="openingdateTextBox"></ajaxToolkit:CalendarExtender>
                </td>
                <td align="left">
                    <b>Phone No</b>
                </td>
                <td align="left">
                    <asp:TextBox ID="phnNoTextBox" runat="server" Width="100px"></asp:TextBox>
                </td>

               
            </tr>
            <tr>
                
                <td align="left">
                    <b>Total Share</b>
                </td>
                <td align="left">
                    <asp:TextBox ID="totalshareTextBox" runat="server" Width="100px"></asp:TextBox>
                </td>
                 <td align="left">
                    <b>Reg. Office</b>
                </td>
                <td align="left">
                    <asp:TextBox ID="regofficeTextBox2" runat="server" Width="100px"></asp:TextBox>
                </td>
                

            </tr>
            <tr>
                <td align="left">
                    <b>Base Rate</b>
                </td>
                <td align="left">
                    <asp:TextBox ID="baserateTextBox" runat="server" Width="100px"></asp:TextBox>
                </td>

                <td align="left">
                    <b>Product</b>
                </td>
                <td align="left">
                    <asp:TextBox ID="productTextBox" runat="server" Width="100px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <b>Base Update Date</b>
                </td>
                <td align="left">
                    <asp:TextBox ID="baseupdateDateTextBox" runat="server" Width="100px"></asp:TextBox>
                       <asp:ImageButton ID="baseupdateDateImageButton" runat="server"
                        ImageUrl="~/Image/Calendar_scheduleHS.png" TabIndex="7" />
                     <ajaxToolkit:CalendarExtender ID="baseupdateDateTextBoxCalendarExtender" runat="server" Format="dd-MMM-yyyy" PopupButtonID="baseupdateDateImageButton" TargetControlID="baseupdateDateTextBox"></ajaxToolkit:CalendarExtender>

                </td>
                  <td align="left">
                    <b>Margin</b>
                </td>
                <td align="left">
                    <asp:TextBox ID="merginTextBox" runat="server" Width="100px"></asp:TextBox>
                </td>
               
            </tr>
            <tr>
                 <td align="left">
                    <b>Float Date from</b>
                </td>
                <td align="left">
                    <asp:TextBox ID="floatdatefromTextBox" runat="server" Width="100px"></asp:TextBox>
                      <asp:ImageButton ID="floatdatefromTextBoxImageButton" runat="server"
                        ImageUrl="~/Image/Calendar_scheduleHS.png" TabIndex="7" />
                     <ajaxToolkit:CalendarExtender ID="floatdatefromTextBoxCalendarExtender" runat="server" Format="dd-MMM-yyyy" PopupButtonID="floatdatefromTextBoxImageButton" TargetControlID="floatdatefromTextBox"></ajaxToolkit:CalendarExtender>
                </td>
                <td align="left">
                    <b>Float Date To</b>
                </td>
                <td align="left">
                    <asp:TextBox ID="floatdatetoTextBox" runat="server" Width="100px"></asp:TextBox>
                    <asp:ImageButton ID="floatdatetoTextBoxImageButton" runat="server"
                        ImageUrl="~/Image/Calendar_scheduleHS.png" TabIndex="7" />
                     <ajaxToolkit:CalendarExtender ID="floatdatetoTextBoxCalendarExtender" runat="server" Format="dd-MMM-yyyy" PopupButtonID="floatdatetoTextBoxImageButton" TargetControlID="floatdatetoTextBox"></ajaxToolkit:CalendarExtender>
                </td>

              
            </tr>
              <tr>
                 <td align="left">
                    <b>R Issue Date from</b>
                </td>
                <td align="left">
                    <asp:TextBox ID="RIssuefromTextBox" runat="server" Width="100px"></asp:TextBox>
                      <asp:ImageButton ID="RIssuefromTextBoxImageButton" runat="server"
                        ImageUrl="~/Image/Calendar_scheduleHS.png" TabIndex="7" />
                     <ajaxToolkit:CalendarExtender ID="RIssuefromCalendarExtender" runat="server" Format="dd-MMM-yyyy" PopupButtonID="RIssuefromTextBoxImageButton" TargetControlID="RIssuefromTextBox"></ajaxToolkit:CalendarExtender>
                </td>
                <td align="left">
                    <b>R Issue Date from To</b>
                </td>
                <td align="left">
                    <asp:TextBox ID="RIssuetoTextBox" runat="server" Width="100px"></asp:TextBox>
                    <asp:ImageButton ID="RIssuetoTextBoxImageButton" runat="server"
                        ImageUrl="~/Image/Calendar_scheduleHS.png" TabIndex="7" />
                     <ajaxToolkit:CalendarExtender ID="RIssuetoTextBoxImageButtonCalendarExtender" runat="server" Format="dd-MMM-yyyy" PopupButtonID="RIssuetoTextBoxImageButton" TargetControlID="RIssuetoTextBox"></ajaxToolkit:CalendarExtender>
                </td>

              
            </tr>
            <tr>
                <td align="left">
                    <b>Premium</b>
                </td>
                <td align="left">
                    <asp:TextBox ID="premiumTextBox" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td align="left">
                    <b>Paid Up capital</b>
                </td>
                <td align="left">
                    <asp:TextBox ID="TextBoxPaidUpCapital" runat="server" Width="100px"></asp:TextBox>
                </td>
                
                
            </tr>
             <tr>
                <td align="left">
                    <b>Authorized capital</b>
                </td>
                <td align="left">
                    <asp:TextBox ID="TextBoxAuthorizedcapital" runat="server" Width="100px"></asp:TextBox>
                </td>
            
            </tr>

            <tr>
               
                <td align="center" colspan="6">
                    <asp:Button ID="saveButton" runat="server" Text="Save"
                        CssClass="buttoncommon" TabIndex="48" 
                        OnClick="saveButton_Click" />
                  

                </td>

            </tr>
        </table>
        </div>
  
         
    <script type="text/javascript">
    
        $.validator.addMethod("CheckDropDownList", function (value, element, param) {  
            if (value == '0')  
                return false;  
            else  
                return true;  
        },"Please select a Category.");   

      $("#aspnetForm").validate({
          rules: {
                    <%=companyCodeTextBox.UniqueID %>: {
                        
                        required: true,
                        number:true,
                        minlength: 3,
                        maxlength: 3
                    }, <%=companyNameTextBox.UniqueID %>: {
                        
                        required: true,
                        maxlength: 40
                    }, <%=sectorTextBox.UniqueID %>: {
                        
                        required: true,
                        maxlength: 4
                    },<%=categoryDropDownList.UniqueID %>: {
                        
                        //required:true 
                        CheckDropDownList:true
                        
                    },<%=MarketLotTextBox.UniqueID %>: {
                        
                        required: true,
                        number:true,
                        maxlength: 5
                    },<%=faceValueTextBox.UniqueID %>: {
                        
                        required: true,
                        number:true
                      
                    },<%=dsecodeTextBox.UniqueID %>: {
                        
                        required: true,
                        maxlength:20
                      
                    },<%=TextBoxPaidUpCapital.UniqueID %>: {
                        
                        required: true,
                        number:true
                       
                      
                    },<%=totalshareTextBox.UniqueID %>: {
                        
                        required: true,
                        number:true
                      
                      
                    },<%=baserateTextBox.UniqueID %>: {
                        
                        required: true,
                        number:true
                        
                      
                    }
              
                }, messages: {
                     <%=companyCodeTextBox.UniqueID %>:{  
                         required: "*Company Code is required*",
                         maxlength: "* Please enter maximum 3 characters *"
                      }, <%=companyNameTextBox.UniqueID %>: {
                        
                        required: "*Company name is required*",
                        maxlength:"* Please enter maximum 40 characters *"
                    },<%=sectorTextBox.UniqueID %>: {
                        
                        required: "*Sector is required*",
                        maxlength:"* Please enter maximum 4 characters *"
                    },<%=MarketLotTextBox.UniqueID %>: {
                        
                        required: "*Market Lot is required*",
                        number: "Please Enter an Numeric Value",
                        maxlength:"* Please enter maximum 5 characters *"
                    },<%=faceValueTextBox.UniqueID %>: {
                        
                        required: "*Face value is required*",
                        number: "Please Enter an Numeric Value",
                     
                    },<%=dsecodeTextBox.UniqueID %>: {
                        required: "*Dse Code is required*",
                        maxlength:"* Please enter maximum 20 characters *"
                    },<%=TextBoxPaidUpCapital.UniqueID %>: {
                        required: "*Paid Up Capital is required*",
                        number: "Please Enter an Numeric Value",
                       
                    },<%=totalshareTextBox.UniqueID %>: {
                        required: "*Total Share is required*",
                        number: "Please Enter an Numeric Value"
                      
                    },<%=baserateTextBox.UniqueID %>: {
                        required: "*Base rate is required*",
                        number: "Please Enter an Numeric Value"
                       
                    }
              
                
                }
      });
     
    </script>
</asp:Content>

