﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UI_CompanyInformation : System.Web.UI.Page
{
    CommonGateway commonGatewayObj = new CommonGateway();
  

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            Session.RemoveAll();
            Response.Redirect("../Default.aspx");
        }
        Session["user_list"] = GetUserInfo();

        DataTable dtuserlist = (DataTable)Session["user_list"];



        //  companyNameTextBox.Text = "sss";
    }

    protected void saveButton_Click(object sender, EventArgs e)
    {

      

    }

    private DataTable GetUserInfo()
    {
        DataTable dtUserList = new DataTable();

        StringBuilder sbMst = new StringBuilder();
        StringBuilder sbOrderBy = new StringBuilder();
        sbOrderBy.Append("");

        sbMst.Append(" Select user_table.ID,user_table.USER_ID,user_table.NAME,user_table.DESIGNATION,USER_ROLE.ROLE_NAME from user_table  INNER JOIN user_role ");
        sbMst.Append("ON user_table.ROLE_ID = user_role.ROLE_ID   ");
        sbOrderBy.Append(" order by user_table.ID  ");

        sbMst.Append(sbOrderBy.ToString());
        dtUserList = commonGatewayObj.Select(sbMst.ToString());

        Session["dtUserInfo"] = dtUserList;
        return dtUserList;
    }

    protected void AddButton_Click(object sender, EventArgs e)
    {
      //  Response.Redirect("AddFund.aspx");
    }

    protected void UpdateButton_Click(object sender, EventArgs e)
    {
        //Response.Redirect("AddFund.aspx");
    }
}