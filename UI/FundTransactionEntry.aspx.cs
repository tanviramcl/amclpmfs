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

public partial class UI_FundTransactionEntry : System.Web.UI.Page
{
    CommonGateway commonGatewayObj = new CommonGateway();
    DropDownList dropDownListObj = new DropDownList();
    Pf1s1DAO pf1s1DAOObj = new Pf1s1DAO();

    //double noOfShare = 0.00;
    //double amount = 0.00;
    double rate = 0.00;
    //double amountAfterComission = 0.00;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            Session.RemoveAll();
            Response.Redirect("../Default.aspx");
        }
        DataTable dtCompanyNameDropDownList = dropDownListObj.FillCompanyNameDropDownList();
        DataTable dtFundNameDropDownList = dropDownListObj.FundNameDropDownList();
        if (!IsPostBack)
        {
            companyNameDropDownList.DataSource = dtCompanyNameDropDownList;
            companyNameDropDownList.DataTextField = "COMP_NM";
            companyNameDropDownList.DataValueField = "COMP_CD";
            companyNameDropDownList.DataBind();

            fundNameDropDownList.DataSource = dtFundNameDropDownList;
            fundNameDropDownList.DataTextField = "F_NAME";
            fundNameDropDownList.DataValueField = "F_CD";
            fundNameDropDownList.DataBind();
        }
    }
    protected void noOfShareTextBox_TextChanged(object sender, EventArgs e)
    {        
        if (transTypeDropDownList.SelectedValue == "B")
        {
            if (noOfShareTextBox.Text == "")
            {
              ClientScript.RegisterStartupScript(this.GetType(),"SetFocus", "<script>document.getElementById('" + noOfShareTextBox.ClientID + "').focus();</script>");
            }
            else
            {
                amountTextBox.Text = "0.00";
                rateTextBox.Text = "0.00";
                amountAfterComissionTextBox.Text = "0.00";
                ClientScript.RegisterStartupScript(this.GetType(), "SetFocus", "<script>document.getElementById('" + voucherNoTextBox.ClientID + "').focus();</script>");
            }
        }
        if (transTypeDropDownList.SelectedValue != "B")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "SetFocus", "<script>document.getElementById('" + amountTextBox.ClientID + "').focus();</script>");
        }
        //else if (transTypeDropDownList.SelectedValue == "C")
        //{

        //}
        //noOfShareTextBox.AutoPostBack = false;
    }
    protected void amountTextBox_TextChanged(object sender, EventArgs e)
    {
        
        
        if (transTypeDropDownList.SelectedValue != "B")
        {
            if (amountTextBox.Text == "")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "SetFocus", "<script>document.getElementById('" + amountTextBox.ClientID + "').focus();</script>");
            }
            else
            {
                rate = Convert.ToDouble(amountTextBox.Text) / Convert.ToDouble(noOfShareTextBox.Text);
                rateTextBox.Text = rate.ToString();
                amountAfterComissionTextBox.Text = amountTextBox.Text;
                ClientScript.RegisterStartupScript(this.GetType(), "SetFocus", "<script>document.getElementById('" + voucherNoTextBox.ClientID + "').focus();</script>");
            }
        }
        //else if (transTypeDropDownList.SelectedValue == "S")
        //{

        //}
        //amountTextBox.AutoPostBack = false;
    }
    protected void transTypeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        //noOfShareTextBox.Text = "";
        //voucherNoTextBox.Text = "";
        //amountTextBox.Text = "";
        //rateTextBox.Text = "";
        //amountAfterComissionTextBox.Text = "";
    }
    protected void saveButton_Click(object sender, EventArgs e)
    {
        string LoginID = Session["UserID"].ToString();
        string LoginName = Session["UserName"].ToString().ToUpper();

        Hashtable httable = new Hashtable();
        httable.Add("VCH_DT", Convert.ToDateTime(howlaDateTextBox.Text.ToString()).ToString("dd-MMM-yyyy"));
        if (!stockExchangeDropDownList.SelectedValue.Equals("0"))
        {
            httable.Add("STOCK_EX", Convert.ToChar(stockExchangeDropDownList.SelectedValue));
        }
        if (!fundNameDropDownList.SelectedValue.Equals("0"))
        {
            httable.Add("F_CD", Convert.ToInt16(fundNameDropDownList.SelectedValue));
        }
        if (!companyNameDropDownList.SelectedValue.Equals("0"))
        {
            httable.Add("COMP_CD", Convert.ToInt16(companyNameDropDownList.SelectedValue));
        }
        if (!transTypeDropDownList.SelectedValue.Equals("0"))
        {
            httable.Add("TRAN_TP", Convert.ToChar(transTypeDropDownList.SelectedValue));
        }
        if (!noOfShareTextBox.Text.Equals(""))
        {
            httable.Add("NO_SHARE", Convert.ToDouble(noOfShareTextBox.Text));
        }
        if (!amountTextBox.Text.Equals(""))
        {
            httable.Add("AMOUNT", Convert.ToDouble(amountTextBox.Text));
        }
        if (!voucherNoTextBox.Text.Equals(""))
        {
            httable.Add("VCH_NO", (voucherNoTextBox.Text).ToString());
        }
        if (!rateTextBox.Text.Equals(""))
        {
            httable.Add("RATE", Convert.ToDouble(rateTextBox.Text));
        }
        if (!amountAfterComissionTextBox.Text.Equals(""))
        {
            httable.Add("AMT_AFT_COM", Convert.ToDouble(amountAfterComissionTextBox.Text));
        }
        
        

        //httable.Add("ENTRY_DATE", DateTime.Today.ToString("dd-MMM-yyyy"));
        httable.Add("OP_NAME", LoginID);

        if (pf1s1DAOObj.IsDuplicateBonusRightEntry(Convert.ToInt32(fundNameDropDownList.SelectedValue.ToString()), Convert.ToInt32(companyNameDropDownList.SelectedValue.ToString()), Convert.ToDateTime(howlaDateTextBox.Text.Trim().ToString()).ToString("dd-MMM-yyyy"), transTypeDropDownList.SelectedValue.ToString(), Convert.ToInt32(noOfShareTextBox.Text.Trim().ToString())))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Save Failed:You are not Smart User  Trying to Duplicate entry');", true);
        }
        else
        {
            commonGatewayObj.Insert(httable, "invest.fund_trans_hb");
            ClearFields();
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Saved Successfully');", true);
        }
        fundNameDropDownList.Focus();
    }

    public void ClearFields()
    {
        //fundNameDropDownList.SelectedValue = "0";
        noOfShareTextBox.Text = "";
        voucherNoTextBox.Text = "";
        amountTextBox.Text = "";
        rateTextBox.Text = "";
        amountAfterComissionTextBox.Text = "";

    }
    protected void fundNameDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
       
    }
}
