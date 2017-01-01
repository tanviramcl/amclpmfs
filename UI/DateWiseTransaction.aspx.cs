using System;
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
using System.Text;
using System.Data.OracleClient;
using System.IO;
//using AMCL.DL;
//using AMCL.BL;
//using AMCL.UTILITY;
//using AMCL.GATEWAY;
//using AMCL.COMMON;


public partial class DateWiseTransaction : System.Web.UI.Page
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
        if (!IsPostBack)
        {
            fundNameDropDownList.DataSource = dtFundNameDropDownList;
            fundNameDropDownList.DataTextField = "F_NAME";
            fundNameDropDownList.DataValueField = "F_CD";
            fundNameDropDownList.DataBind();
        }
    }
    
    protected void txtHowlaDateFrom_TextChanged(object sender, EventArgs e)
    {

    }

    protected void txtLastHowlaDate_TextChanged(object sender, EventArgs e)
    {

    }

    protected void txtHowlaDateTo_TextChanged(object sender, EventArgs e)
    {

    }

   

    protected void fundNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string LoginID = Session["UserID"].ToString();
        string LoginName = Session["UserName"].ToString().ToUpper();

        Hashtable httable = new Hashtable();
        httable.Add("VCH_DT", Convert.ToDateTime(txtHowlaDateFrom.Text.ToString()).ToString("dd-MMM-yyyy"));
        if (!stockExchangeDropDownList.SelectedValue.Equals("0"))
        {
            httable.Add("STOCK_EX", Convert.ToChar(stockExchangeDropDownList.SelectedValue));
        }
        if (!fundNameDropDownList.SelectedValue.Equals("0"))
        {
            httable.Add("F_CD", Convert.ToInt16(fundNameDropDownList.SelectedValue));
        }
        //if (!companyNameDropDownList.SelectedValue.Equals("0"))
        //{
        //    httable.Add("COMP_CD", Convert.ToInt16(companyNameDropDownList.SelectedValue));
        //}
        //if (!transTypeDropDownList.SelectedValue.Equals("0"))
        //{
        //    httable.Add("TRAN_TP", Convert.ToChar(transTypeDropDownList.SelectedValue));
        //}
        //if (!noOfShareTextBox.Text.Equals(""))
        //{
        //    httable.Add("NO_SHARE", Convert.ToDouble(noOfShareTextBox.Text));
        //}
        //if (!amountTextBox.Text.Equals(""))
        //{
        //    httable.Add("AMOUNT", Convert.ToDouble(amountTextBox.Text));
        //}
        //if (!voucherNoTextBox.Text.Equals(""))
        //{
        //    httable.Add("VCH_NO", (voucherNoTextBox.Text).ToString());
        //}
        //if (!rateTextBox.Text.Equals(""))
        //{
        //    httable.Add("RATE", Convert.ToDouble(rateTextBox.Text));
        //}
        //if (!amountAfterComissionTextBox.Text.Equals(""))
        //{
        //    httable.Add("AMT_AFT_COM", Convert.ToDouble(amountAfterComissionTextBox.Text));
        //}



        //httable.Add("ENTRY_DATE", DateTime.Today.ToString("dd-MMM-yyyy"));
        httable.Add("OP_NAME", LoginID);

        //if (pf1s1DAOObj.IsDuplicateBonusRightEntry(Convert.ToInt32(fundNameDropDownList.SelectedValue.ToString()), Convert.ToInt32(companyNameDropDownList.SelectedValue.ToString()), Convert.ToDateTime(howlaDateTextBox.Text.Trim().ToString()).ToString("dd-MMM-yyyy"), transTypeDropDownList.SelectedValue.ToString(), Convert.ToInt32(noOfShareTextBox.Text.Trim().ToString())))
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Save Failed:You are not Smart User  Trying to Duplicate entry');", true);
        //}
        //else
        //{
        //    commonGatewayObj.Insert(httable, "invest.fund_trans_hb");
        //    ClearFields();
        //    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Saved Successfully');", true);
        //}
        //fundNameDropDownList.Focus();
    }
}
