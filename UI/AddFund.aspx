<%@ Page Language="C#" MasterPageFile="~/UI/AMCLCommon.master" AutoEventWireup="true" CodeFile="AddFund.aspx.cs" Inherits="UI_CompanyInformation" Title="Company Information" %>
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
        <table id="Table1" width="600" align="center" cellpadding="0" cellspacing="0" runat="server">
            <tr>
                <td align="center" class="style3">
                    <b><u>Fund Entry</u></b>
                </td>
            </tr>
        </table>

        <table id="Table2" width="600" align="center" cellpadding="2" cellspacing="2" runat="server">
          
            <tr>
                <td align="left">
                    <b>Fund code </b>
                </td>
                <td align="left">
                    <asp:TextBox ID="fundcodeTextBox" runat="server" Width="100px" OnTextChanged="fundCodeTextBox_TextChanged" AutoPostBack="true"></asp:TextBox>
                </td>

            </tr>

            <tr>
                <td align="left">
                    <b>Fund Name </b>
                </td>
                <td align="left">
                    <textarea id="txtfundName" cols="20" rows="2" runat="server"></textarea>
                </td>

            </tr>

            <tr>

                <td align="left">
                    <b>Fund type</b>
                </td>
                <td>
                    <asp:DropDownList ID="FundTypeDropDownList" Width="100px" runat="server"
                        TabIndex="3">
                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                        <asp:ListItem Text="OPEN END" Value="N"></asp:ListItem>
                        <asp:ListItem Text="CLOSE END" Value="R"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td align="left">
                    <b>Customer Code</b>
                </td>
                <td align="left">
                    <asp:TextBox ID="customerCode" runat="server" Width="100px"></asp:TextBox>
                </td>


            </tr>

            <tr>
                <td align="left">
                    <b>BO ID</b>
                </td>
                <td align="left">
                    <asp:TextBox ID="boIdTextBox" runat="server" Width="100px"></asp:TextBox>
                </td>

                <td align="left">
                    <b>Sell buy Commission</b>
                </td>
                <td align="left">

                    <asp:TextBox ID="txtsellbuycommision" runat="server" Width="100px"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <td align="left">
                    <b>Company Code</b>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtCompanyCode" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td align="left">
                    <b>IS Fund Close ?</b>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtfundClose" runat="server" Width="100px"></asp:TextBox>
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
        

     

    <script type="text/javascript">

        
        $.validator.addMethod("CheckDropDownList", function (value, element, param) {  
            if (value == '0')  
                return false;  
            else  
                return true;  
        },"Please select a Fund Type."); 


        $("#aspnetForm").validate({
            rules: {
                <%=fundcodeTextBox.UniqueID %>: {
                        
                      required: true,
                      number:true,
                      minlength: 1,
                      maxlength: 3
                  },<%=txtfundName.UniqueID %>: {
                        required: true 
                    },<%=FundTypeDropDownList.UniqueID %>: {
                        CheckDropDownList: true 
                    },<%=customerCode.UniqueID %>: {
                        required: true 
                    },<%=boIdTextBox.UniqueID %>: {
                        required: true 
                    },<%=txtsellbuycommision.UniqueID %>: {
                        required: true,
                        number:true
                    },<%=txtCompanyCode.UniqueID %>: {
                        
                        number:true
                    }
                    
          }, messages: {
              <%=fundcodeTextBox.UniqueID %>:{  
                        required: "*Fund Code is required*",
                        maxlength: "* Please enter maximum 3 characters *"
                    }, <%=txtfundName.UniqueID %>:{  
                        required: "*Fund name is required*",
                    }, <%=txtfundName.UniqueID %>:{  
                        required: "*Fund name is required*",
                    }, <%=customerCode.UniqueID %>:{  
                        required: "*Customer Code is required*",
                    }, <%=boIdTextBox.UniqueID %>:{  
                        required: "*BO ID is required*",
                    }, <%=txtsellbuycommision.UniqueID %>:{  
                        required: "*Sell buy commision is required*",
                    }
                    
                }
      });
     
    </script>
      </div>

</asp:Content>

