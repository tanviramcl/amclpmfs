<%@ Page Language="C#"  MasterPageFile="~/UI/AMCLCommon.master" AutoEventWireup="true" CodeFile="CapitalGainSummeryDateWise.aspx.cs" Inherits="UI_BalancechekReport" Title="Capital Gain Summery Date Wise" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
       <style type ="text/css" >  
        label.error {             
            color: red;   
            display:inline-flex ;                 
        } 
        .ui-datepicker {
        position: relative !important;
        top: -250px !important;
        left: 100px !important;
        margin-left: 390px;
        margin-top: -15px;
        } 
    </style> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <%--<ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true" EnableScriptLocalization="true" ID="ScriptManager1" />--%>
    <asp:ScriptManager ID="ScriptManager1" EnableScriptGlobalization="true" EnableScriptLocalization="true" runat="server"></asp:ScriptManager>
    <table style="text-align: center">
        <tr>
            <td class="FormTitle" align="center">Capital Gain Summary Date Wise</td>
            <td>
                <br />
            </td>
        </tr>

    </table>
      <table style="text-align: center">
          <tr>
           <td align="right">
                <b>From Date:</b>
            </td>
            <td align="left">
  
                <asp:TextBox ID="RIssuefromTextBox" runat="server" Width="100px"></asp:TextBox>
            </td>
        </tr>
        
        <tr>
            <td align="right">
                <b>To Date:</b>
            </td>
            <td align="left">
             <%--  <input type="text" id="txtTo" /><img alt="d" id="txtFrom2" src="../Image/Calendar_scheduleHS.png" />--%>
                <asp:TextBox ID="RIssueToTextBox" runat="server" Width="100px"></asp:TextBox>
            </td>
        </tr>


    </table>
    <table width="750" style="text-align: center" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td align="center" colspan="2">
                <asp:Button ID="showButton" runat="server" Text="Show Report"
                    CssClass="buttoncommon" TabIndex="5" OnClick="showButton_Click" />
            </td>
        </tr>
    </table>


     <script type="text/javascript">
         $(function () {

        

    <%--$('#<%=RIssuefromTextBox.ClientID%>').datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: "dd/mm/yy",
                onSelect: function (selected) {
                    var dt = new Date(selected);
                  //  alert(dt);
                  //  dt.setDate(dt.getDate()+1);
                    //alert(dt);
                    $('#<%=RIssueToTextBox.ClientID%>').datepicker("option", "minDate", dt.format("dd/mm/yy"));
                }
        });

          $('#<%=RIssueToTextBox.ClientID%>').datepicker({
                changeMonth: true,
                changeYear: true,
               // yearRange: "-100:-17",
               // defaultDate: new Date(1980, 01, 01),
                dateFormat: "dd/mm/yy",
                onSelect: function (selected) {
                    var dt = new Date(selected);
                    //dt.setDate(dt.getDate()-1);
                   // dt.format('dd-M-yy');
                   // alert(dt);
                    $('#<%=RIssuefromTextBox.ClientID%>').datepicker("option", "maxDate", dt.format("dd/mm/yy"));
                }
          });--%>

              $('#<%=RIssuefromTextBox.ClientID%>').datepicker({
                 changeMonth: true,
                 changeYear: true,
                 dateFormat: "dd/mm/yy",
                 onSelect: function(selected) {
                     $('#<%=RIssueToTextBox.ClientID%>').datepicker("option","minDate", selected)
                 }
             });
             $('#<%=RIssueToTextBox.ClientID%>').datepicker({ 
                 changeMonth: true,
                 changeYear: true,
                 dateFormat: "dd/mm/yy",
                 maxDate:"today",
                 onSelect: function(selected) {

                     <%--alert(selected);
                     $('#<%=RIssuefromTextBox.ClientID%>').datepicker("option","maxDate", selected)--%>


                 }
             });    

    });


    
    $("#aspnetForm").validate({
        rules: {
             <%=RIssuefromTextBox.UniqueID %>: {
                        
                        required: true
                       // date: true,
                        //assetDate:true
                    },<%=RIssueToTextBox.UniqueID %>: {
                        
                        required: true,
                        greaterThan: '#<%=RIssueToTextBox.ClientID%>'                      
                    }
              
                }, messages: {
                   <%=RIssuefromTextBox.UniqueID %>:{  
                       required: "*From Date  is required*"
                      // date: "* Please enter a date *"
                   },<%=RIssueToTextBox.UniqueID %>:{  
                       required: "*To Date  is required*",
                      
                   }
                    
                    
                        
                }
      });

    </script>
</asp:Content>
