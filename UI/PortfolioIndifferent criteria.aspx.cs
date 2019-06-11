﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class UI_PortfolioWithNonListedSecurities : System.Web.UI.Page
{
    CommonGateway commonGatewayObj = new CommonGateway();
    DropDownList dropDownListObj = new DropDownList();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            Session.RemoveAll();
            Response.Redirect("../Default.aspx");
        }
        DataTable dtHowlaDateDropDownList = dropDownListObj.HowlaDateDropDownList();
        DataTable dtFundNameDropDownList = dropDownListObj.FundNameDropDownList();
        if (!IsPostBack)
        {
            fundNameDropDownList.DataSource = dtFundNameDropDownList;
            fundNameDropDownList.DataTextField = "F_NAME";
            fundNameDropDownList.DataValueField = "F_CD";
            fundNameDropDownList.DataBind();

            portfolioAsOnDropDownList.DataSource = dtHowlaDateDropDownList;
            portfolioAsOnDropDownList.DataTextField = "Howla_Date";
            portfolioAsOnDropDownList.DataValueField = "VCH_DT";
            portfolioAsOnDropDownList.DataBind();
        }
    }

    protected void showButton_Click(object sender, EventArgs e)
    {
        string fundCode = fundNameDropDownList.SelectedValue.ToString();
        string balDate = portfolioAsOnDropDownList.SelectedValue.ToString();
        string sector = sectorDropDownList.SelectedValue.ToString();
        string category = categoryDropDownList.SelectedValue.ToString();
        string group = groupDropDownList.SelectedValue.ToString();
        string ipo = IPODropDownList.SelectedValue.ToString();
        string marketype = marketDropDownList.SelectedValue.ToString();

        //   ClientScript.RegisterStartupScript(this.GetType(), "PortfolioSummaryReportViewer", "window.open('ReportViewer/PortfolioWithNonListedReportViewer.aspx')", true);
        Response.Redirect("ReportViewer/PortfolioIndifferentcriteriaReportViewer.aspx?fundCode=" + fundCode+ "&balDate="+ balDate + "&sector=" + sector + "&category= " + category + "&group= " + group + " &ipo= " + ipo + "&marketype= " + marketype + "");
    }
}
