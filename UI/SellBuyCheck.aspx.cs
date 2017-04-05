using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;

public partial class UI_BalancechekReport : System.Web.UI.Page
{
    DropDownList dropDownListObj = new DropDownList();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            Session.RemoveAll();
            Response.Redirect("../Default.aspx");
        }

        DataTable dtFundNameDropDownList = dropDownListObj.FundNameDropDownList();
        DataTable dtCompanyNameDropDownList = dropDownListObj.FillCompanyNameDropDownList();

        if (!IsPostBack)
        {

            fundNameDropDownList.DataSource = dtFundNameDropDownList;
            fundNameDropDownList.DataTextField = "F_NAME";
            fundNameDropDownList.DataValueField = "F_CD";
            fundNameDropDownList.DataBind();


            companyNameDropDownList.DataSource = dtCompanyNameDropDownList;
            companyNameDropDownList.DataTextField = "COMP_NM";
            companyNameDropDownList.DataValueField = "COMP_CD";
            companyNameDropDownList.DataBind();


        }

    }


    protected void radio_CheckedChanged(object sender, EventArgs e)
    {
        if (fundwiseRadioButton.Checked)
        {
            //RIssuefromTextBox.Text = string.Empty;
            //RIssueToTextBox.Text = string.Empty;
            fundNameDropDownList.SelectedValue = "0";
            fundNameDropDownListlabel.Visible = true;
            fundNameDropDownList.Visible = true;
            companyNameDropDownListlabel.Visible = false;
            companyNameDropDownList.Visible = false;
            transTypeDropDownListLabel.Visible = false;
            transTypeDropDownList.Visible = false;
        }
        else if (CompanyWiseRadioButton.Checked)
        {
            //RIssuefromTextBox.Text = string.Empty;
            //RIssueToTextBox.Text = string.Empty;
            companyNameDropDownListlabel.Visible = true;
            companyNameDropDownList.Visible = true;
            fundNameDropDownListlabel.Visible = false;
            fundNameDropDownList.Visible = false;
            transTypeDropDownListLabel.Visible = false;
            transTypeDropDownList.Visible = false;
        }
        else if (allRadioButton.Checked)
        {
            //RIssuefromTextBox.Text = string.Empty;
            //RIssueToTextBox.Text = string.Empty;
            fundNameDropDownListlabel.Visible = false;
            fundNameDropDownList.Visible = false;
            companyNameDropDownListlabel.Visible = false;
            companyNameDropDownList.Visible = false;
            transTypeDropDownListLabel.Visible = false;
            transTypeDropDownList.Visible = false;
        }
        else if (CompanywiseallRadioButton.Checked)
        {
            //RIssuefromTextBox.Text = string.Empty;
            //RIssueToTextBox.Text = string.Empty;
            fundNameDropDownList.SelectedValue = "0";
            fundNameDropDownListlabel.Visible = true;
            fundNameDropDownList.Visible = true;
            companyNameDropDownListlabel.Visible = false;
            companyNameDropDownList.Visible = false;
            transTypeDropDownListLabel.Visible = true;
            transTypeDropDownList.Visible = true;
        }

    }


    protected void showButton_Click(object sender, EventArgs e)
    {



        //string Fromdate = Convert.ToDateTime(RIssuefromTextBox.Text).ToString("dd-MMM-yyyy");
        //string Todate = Convert.ToDateTime(RIssueToTextBox.Text).ToString("dd-MMM-yyyy");


        //string fundcode = fundNameDropDownList.SelectedValue.ToString();


        //string companycode = companyNameDropDownList.SelectedValue.ToString();
        //string transtype = transTypeDropDownList.SelectedValue.ToString();
        Session["Fromdate"] = Convert.ToDateTime(RIssuefromTextBox.Text).ToString("dd-MMM-yyyy");
        Session["Todate"] = Convert.ToDateTime(RIssueToTextBox.Text).ToString("dd-MMM-yyyy");
        Session["fundCodes"] = fundNameDropDownList.SelectedValue.ToString();
        Session["companycode"] = companyNameDropDownList.SelectedValue.ToString();
        Session["transtype"] = transTypeDropDownList.SelectedValue.ToString();



        //if (fundwiseRadioButton.Checked && Fromdate != "" && Todate != "" && fundcode!="")
        //{
        //    Response.Redirect("ReportViewer/SellBuyCheckReportViwer.aspx?fundcode=" + fundcode + "&Fromdate=" + Fromdate + "&Todate=" + Todate + "");
        //}
        //else if (CompanyWiseRadioButton.Checked )
        //{
        //    Response.Redirect("ReportViewer/SellBuyCheckReportViwer.aspx?companycode=" + companycode + "&Fromdate=" + Fromdate + "&Todate=" + Todate + "&transtype=" + transtype + "");
        //}
        //else if (allRadioButton.Checked)
        //{
        //    Response.Redirect("ReportViewer/SellBuyCheckReportViwer.aspx?Fromdate=" + Fromdate + "&Todate=" + Todate + "");
        //}
        //else if (CompanywiseallRadioButton.Checked)
        //{
        //    Response.Redirect("ReportViewer/SellBuyCheckReportViwer.aspx?companycode=" + companycode + "&Fromdate=" + Fromdate + "&Todate=" + Todate + "&transtype=" + transtype + "");
        //}
        //else
        //{
        //    // ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Not valid')", true);
        //    Labelerror.Visible = true;
        //    Labelerror.Text = "Not Valid";
        //}

        ClientScript.RegisterStartupScript(this.GetType(), "SellBuyCheckReportViwer", "window.open('ReportViewer/SellBuyCheckReportViwer.aspx')", true);


    }




}

   