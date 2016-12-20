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

public partial class UI_NonListedSecuritiesInvestmentEntryForm : System.Web.UI.Page
{
    CommonGateway commonGatewayObj = new CommonGateway();
    DropDownList dropDownListObj = new DropDownList();
    Pf1s1DAO pf1s1DAOObj = new Pf1s1DAO();
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
    protected void saveButton_Click(object sender, EventArgs e)
    {
        string LoginID = Session["UserID"].ToString();
        //string LoginName = Session["UserName"].ToString().ToUpper();
        Hashtable httable = new Hashtable();
        httable.Add("ID", Convert.ToInt32(pf1s1DAOObj.getMaxIDForNonListedSecurities() + 1));       
        if (!fundNameDropDownList.SelectedValue.Equals("0"))
        {
            httable.Add("F_CD", Convert.ToInt16(fundNameDropDownList.SelectedValue));
        }
        if (!amountTextBox.Text.Equals(""))
        {
            httable.Add("INV_AMOUNT", Convert.ToDouble(amountTextBox.Text));
        }
        httable.Add("INV_DATE", Convert.ToDateTime(investmentDateTextBox.Text).ToString("dd-MMM-yyyy"));
        httable.Add("ENTRY_BY", LoginID);
        httable.Add("ENTRY_DATE", DateTime.Now);
        if (pf1s1DAOObj.IsDuplicateNonListedSecurities(Convert.ToInt32(fundNameDropDownList.SelectedValue.ToString()), Convert.ToDecimal(amountTextBox.Text.Trim().ToString()), Convert.ToDateTime(investmentDateTextBox.Text.Trim().ToString()).ToString("dd-MMM-yyyy")))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Save Failed: You Are Trying to Duplicate entry.');", true);
        }
        else
        {
            commonGatewayObj.Insert(httable, "invest.NON_LISTED_SECURITIES");
            ClearFields();
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Saved Successfully');", true);
        }
        fundNameDropDownList.Focus();
    }
    public void ClearFields()
    {
        amountTextBox.Text = "";
        investmentDateTextBox.Text = "";
    }
}
