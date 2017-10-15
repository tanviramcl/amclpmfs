<%@ Page Language="C#" MasterPageFile="~/UI/AMCLCommon.master" AutoEventWireup="true" CodeFile="UserInfo.aspx.cs" Inherits="UI_CompanyInformation" Title="User Information" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../CSS/vendor/bootstrap.min.css" rel="stylesheet" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true" EnableScriptLocalization="true" ID="ScriptManager1" />

    <div align="center">
        <h2>User Information</h2>
        <%--<div style="float: left; margin-top: -34px; margin-left: -12px;">
            <asp:Button ID="AddButton" runat="server" Text="Add Fund"
                CssClass="buttoncommon" TabIndex="48"
                OnClick="AddButton_Click" />
        </div>--%>
        <div class="row">

            <table class="table table-hover" id="bootstrap-table">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>USER ID</th>
                        <th>NAME</th>
                        <th>DESIGNATION</th>
                        <th>ROLE NAME</th>
                     <%--   
                        <th>Action</th>--%>
                    </tr>
                </thead>
                <tbody>
                    <%

                        System.Data.DataTable dtuserlist = (System.Data.DataTable)Session["user_list"];
                        if (dtuserlist.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtuserlist.Rows.Count; i++)
                            {
                    %>
                    <tr>
                        <td><%   Response.Write(dtuserlist.Rows[i]["ID"].ToString());   %> </td>
                        <td><%   Response.Write(dtuserlist.Rows[i]["USER_ID"].ToString());   %> </td>
                        <td><%   Response.Write(dtuserlist.Rows[i]["NAME"].ToString());   %> </td>
                        <td><%   Response.Write(dtuserlist.Rows[i]["DESIGNATION"].ToString());   %> </td>
                        <td><%   Response.Write(dtuserlist.Rows[i]["ROLE_NAME"].ToString());   %> </td>
                       
                 <%--       <td><a href="AddFund.aspx?ID=<%   Response.Write(dtNoOfFunds.Rows[i]["F_CD"].ToString());   %>" 
                            class="custUpdBtn">Update</a></td>--%>
                    </tr>
                    <%

                            }
                        }

                    %>
                </tbody>
            </table>
        </div>


    </div>


    <script type="text/javascript">
        $(document).ready(function () {
            $('#bootstrap-table').bdt({

            });
        });
    </script>

</asp:Content>

