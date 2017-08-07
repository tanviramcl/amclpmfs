﻿<%@ Page Language="C#" MasterPageFile="~/UI/AMCLCommon.master" AutoEventWireup="true" CodeFile="AdvancedBalanceUpdateProcess.aspx.cs" Inherits="AdvancedBalanceUpdateProcess" Title="IAMCL Advanced Balance Update Process  " %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../CSS/vendor/bootstrap.min.css" rel="stylesheet" />
    <style type="text/css">
        label.error {
            color: red;
            display: inline-flex;
        }
    </style>
    <style type="text/css">
        .style6 {
            font-family: "Times New Roman";
            font-weight: bold;
        }

        .style8 {
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

        .style11 {
            height: 18px;
        }

        .processBtn {
            BORDER-TOP: #CCCCCC 1px solid;
            BORDER-BOTTOM: #000000 1px solid;
            BORDER-LEFT: #CCCCCC 1px solid;
            BORDER-RIGHT: #000000 1px solid;
            COLOR: #FFFFFF;
            FONT-WEIGHT: bold;
            FONT-SIZE: 11px;
            BACKGROUND-COLOR: #547AC6;
            WIDTH: 246px;
            HEIGHT: 65px;
            font-size: 21px;
            border-radius: 25px;

        }

    </style>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" ID="ScriptManager1" />

    <table width="1100" cellpadding="0" cellspacing="0" border="0">
    </table>
    <table>
        
        <tr>
            <td align="center" colspan="4" class="style8">Advanced Balance Update Process  
            </td>
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
                            <asp:Label ID="lblProcessing" runat="server" Text="" Style="font-size: 24px; color: green;"></asp:Label>
                            <br />


                              <asp:Button ID="btnProcess" runat="server"  Text="Process" 
                                CssClass="processBtn"  OnClick="btnProcess_Click" />

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </td>

        </tr>
    </table>
          <table class="table table-hover" id="bootstrap-table">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Fund Name</th>
                        <th>Balance Date</th>
                        <th>Last Upadate Date</th>
                      
                    </tr>
                </thead>
                <tbody>
                    <%

                        System.Data.DataTable dtGetAllFundTransHb = (System.Data.DataTable)Session["tblAllfundInfo"];
                        if (dtGetAllFundTransHb.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtGetAllFundTransHb.Rows.Count; i++)
                            {
                    %>
                    <tr>
                        <td><%   Response.Write(dtGetAllFundTransHb.Rows[i]["F_CD"].ToString());   %> </td>
                        <td><%   Response.Write(dtGetAllFundTransHb.Rows[i]["F_NAME"].ToString());   %> </td>
                         <td><%   Response.Write(dtGetAllFundTransHb.Rows[i]["BalanceDate"].ToString());   %> </td>
                         <td><%   Response.Write(dtGetAllFundTransHb.Rows[i]["LastUpadateDate"].ToString());   %> </td>
                     
                        
                    </tr>
                    <%

                            }
                        }

                    %>
                </tbody>
            </table>
            
    <script type="text/javascript">
        $(document).ready(function () {
            $('#bootstrap-table').bdt({
                "lengthMenu": [[5, -1], [5, "All"]]
            });
        });
    </script>
</asp:Content>

