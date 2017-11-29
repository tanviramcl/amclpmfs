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


        if (string.IsNullOrEmpty(Request.QueryString["ID"]))
        {
            // not there!
            Session["NonlistedDetails"] = NonlistedSecuritiesDetails();

            DataTable dtNonlistedSecurities = (DataTable)Session["dtNonlistedSecurities"];

           
            saveButton.Text = "Add";
        }
        else
        {
            string CompanyCode = Convert.ToString(Request.QueryString["ID"]).Trim();
            Session["dtNonlistedSecuritiesByCOMPCODE"] = NonlistedSecuritiesDetailsByCOMPID(CompanyCode);
            DataTable dtNonlistedSecuritiesByCOMPCODE = (DataTable)Session["dtNonlistedSecuritiesByCOMPCODE"];

            if (dtNonlistedSecuritiesByCOMPCODE.Rows.Count > 0)
            {
                fundNameDropDownList.SelectedValue = dtNonlistedSecuritiesByCOMPCODE.Rows[0]["F_CD"].ToString();
                nonlistedCompanyDropDownList.SelectedValue = dtNonlistedSecuritiesByCOMPCODE.Rows[0]["COMP_CD"].ToString();
                InvestMentDateTextBox.Text = dtNonlistedSecuritiesByCOMPCODE.Rows[0]["INV_DATE"].ToString();
                Label2.Visible = false;
                nonlistedCategoryDropDownList.Visible = false;
                amountTextBox.Text = dtNonlistedSecuritiesByCOMPCODE.Rows[0]["AMOUNT"].ToString();
                rateTextBox.Text = dtNonlistedSecuritiesByCOMPCODE.Rows[0]["RATE"].ToString();
                TextBox1.Text= dtNonlistedSecuritiesByCOMPCODE.Rows[0]["NO_SHARES"].ToString();

                saveButton.Text = "Update";
                //saveButton.Text = "Update";
            }
        }




        DataTable dtFundNameDropDownList = dropDownListObj.FundNameDropDownList();
        DataTable dtCompanyDropdownlist = dropDownListObj.NolistedCompanyCodeNameDropDownList();
        DataTable dtnonlistedCategoryDropdownlist = dropDownListObj.NolistedCategoryTypeDropDownList();
        DataTable dtPortfolioAsOnDropDownList = BalanceDateDropDownList();
    //    DataTable dtNonlistedSecurities = NonlistedSecuritiesDetails();


     

        //grdShowDSEMP.DataSource = dtNonlistedSecurities;
        //grdShowDSEMP.DataBind();

        if (!IsPostBack)
        {
            fundNameDropDownList.DataSource = dtFundNameDropDownList;
            fundNameDropDownList.DataTextField = "F_NAME";
            fundNameDropDownList.DataValueField = "F_CD";
            fundNameDropDownList.DataBind();


            //PortfolioAsOnDropDownList.DataSource = dtPortfolioAsOnDropDownList;
            //PortfolioAsOnDropDownList.DataTextField = "Balance_Date";
            //PortfolioAsOnDropDownList.DataValueField = "bal_dt_ctrl";
            //PortfolioAsOnDropDownList.DataBind();

            nonlistedCompanyDropDownList.DataSource = dtCompanyDropdownlist;
            nonlistedCompanyDropDownList.DataTextField = "COMP_NM";
            nonlistedCompanyDropDownList.DataValueField = "COMP_CD";
            nonlistedCompanyDropDownList.DataBind();

            nonlistedCategoryDropDownList.DataSource = dtnonlistedCategoryDropdownlist;
            nonlistedCategoryDropDownList.DataTextField = "CAT_NM";
            nonlistedCategoryDropDownList.DataValueField = "CAT_ID";
            nonlistedCategoryDropDownList.DataBind();




        }
    }
    protected void saveButton_Click(object sender, EventArgs e)
    {

        string pfolioMaxDate = "";
        string loginId = Session["UserID"].ToString();
        string invDate = InvestMentDateTextBox.Text.ToString();
        DataTable dtNonlistedMaxDate = commonGatewayObj.Select("Select TO_CHAR(MAX(INV_DATE), 'DD-MON-YYYY ') as INV_DATE   from NON_LISTED_SECURITIES where f_cd=" + fundNameDropDownList.SelectedValue.ToString()+"");
        if (dtNonlistedMaxDate != null && dtNonlistedMaxDate.Rows.Count > 0)
        {
            pfolioMaxDate = dtNonlistedMaxDate.Rows[0]["INV_DATE"].ToString();
        }
        DateTime dtNonlistedDate = DateTime.Parse(pfolioMaxDate);
        DateTime dtINVDATE = DateTime.ParseExact(invDate, "dd/MM/yyyy", null);

        if (dtNonlistedDate.Date > dtINVDATE.Date)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Investment date must be greater than or equal to : " + dtNonlistedDate.ToString("dd-MMM-yyyy") + "');", true);
        }
        else
        {
            //It's an earlier or equal date
            DataTable dtnonListedDetails = new DataTable();
            string strQuery = "Select F_CD,COMP_CD,AMOUNT,RATE,NO_SHARES,ENTRY_BY,ENTRY_DATE from NON_LISTED_SECURITIES_DETAILS where f_cd=" + fundNameDropDownList.SelectedValue.ToString() + " and COMP_CD="+ nonlistedCompanyDropDownList.SelectedValue.ToString() + " and INV_DATE='" + dtINVDATE.ToString("dd-MMM-yyyy") + "'";
            dtnonListedDetails = commonGatewayObj.Select(strQuery);
            if (dtnonListedDetails != null && dtnonListedDetails.Rows.Count > 0)
            {
                string strUPQuery = "update NON_LISTED_SECURITIES_DETAILS SET COMP_CD ='" + nonlistedCompanyDropDownList.SelectedValue.ToString() + "',AMOUNT =" + amountTextBox.Text.ToString() + ",RATE=" + rateTextBox.Text.ToString() + ",NO_SHARES =" + TextBox1.Text.ToString() + ",ENTRY_BY ='" + loginId + "',ENTRY_DATE ='" + DateTime.Now.ToString("dd-MMM-yyyy") + "' where F_CD='" + fundNameDropDownList.SelectedValue.ToString() + "' and  INV_DATE='" + dtINVDATE.ToString("dd-MMM-yyyy") + "'";

                int NumOfRows = commonGatewayObj.ExecuteNonQuery(strUPQuery);

                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Update Sucessfully')", true);
            }
            else
            {
                string strInsQuery;

                strInsQuery = "insert into NON_LISTED_SECURITIES_DETAILS(F_CD,COMP_CD,AMOUNT,RATE,NO_SHARES,INV_DATE,ENTRY_BY,ENTRY_DATE)values('" + fundNameDropDownList.SelectedValue.ToString() + "','" + nonlistedCompanyDropDownList.SelectedValue.ToString() + "','" + amountTextBox.Text.ToString() + "','" + rateTextBox.Text.ToString() + "','" + TextBox1.Text.ToString() + "','" + dtINVDATE.ToString("dd-MMM-yyyy") + "','" + loginId + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "')";

                int NumOfRows = commonGatewayObj.ExecuteNonQuery(strInsQuery);
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Insert Sucessfully')", true);

            }
            Response.Redirect("NonListedSecurities.aspx");
        }

       
    }

    public DataTable BalanceDateDropDownList()//Get Howla Date from invest.fund_trans_hb Table
    {
        DataTable dtHowlaDate = commonGatewayObj.Select("select distinct bal_dt_ctrl from pfolio_bk order by bal_dt_ctrl desc");
        DataTable dtHowlaDateDropDownList = new DataTable();
        dtHowlaDateDropDownList.Columns.Add("Balance_Date", typeof(string));
        dtHowlaDateDropDownList.Columns.Add("bal_dt_ctrl", typeof(string));
        DataRow dr = dtHowlaDateDropDownList.NewRow();
        dr["Balance_Date"] = "--Select--";
        dr["bal_dt_ctrl"] = "0";
        dtHowlaDateDropDownList.Rows.Add(dr);
        for (int loop = 0; loop < dtHowlaDate.Rows.Count; loop++)
        {
            dr = dtHowlaDateDropDownList.NewRow();
            dr["Balance_Date"] = Convert.ToDateTime(dtHowlaDate.Rows[loop]["bal_dt_ctrl"]).ToString("dd-MMM-yyyy");
            dr["bal_dt_ctrl"] = Convert.ToDateTime(dtHowlaDate.Rows[loop]["bal_dt_ctrl"]).ToString("dd-MMM-yyyy");
            dtHowlaDateDropDownList.Rows.Add(dr);
        }
        return dtHowlaDateDropDownList;
    }


    protected void rateChange_TextChanged(object sender, EventArgs e)
    {

        if (amountTextBox.Text != "" && rateTextBox.Text != "")
        {
            double ammount = Convert.ToDouble(amountTextBox.Text);
            double rate = Convert.ToDouble(rateTextBox.Text);
            double noOfShare = (ammount / rate);
            int share = Convert.ToInt32(noOfShare);
            TextBox1.Text = share.ToString();
          //  ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Ammount" + noOfShareTextBox.Text.ToString() + "');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Please Enter Ammount !');", true);
        }
        
       
    }

    public string UpdateAmmountInNonlisted(string FundId, string Date,string Ammount)
    {

        CommonGateway commonGatewayObj = new CommonGateway();
       
        string strUpdateNonlisted = "";
        string message = "";
        if (FundId != "")
        {

            strUpdateNonlisted = "UPDATE NON_LISTED_SECURITIES SET INV_AMOUNT = '"+ Convert.ToDouble(Ammount) + "' WHERE F_CD = " + FundId+" and INV_DATE ='"+Date+ "'";

            int NumOfRows = commonGatewayObj.ExecuteNonQuery(strUpdateNonlisted);
            if (NumOfRows > 0)
            {
                message = "Data Updated Successfully";
            }


        }

        return message;

    }

    [System.Web.Services.WebMethod]

    public static string RATEONCHANGE(string Ammount, string Rate)
    {
        string noofSare = "";
      if (Ammount != "" && Rate != "")
        {
            double ammount = Convert.ToDouble(Rate);
            double rate = Convert.ToDouble(Rate);
            double noOfShare = (ammount / rate);
            int share = Convert.ToInt32(noOfShare);
            noofSare = share.ToString();
        }

        return noofSare;

    }


    public DataTable NonlistedSecuritiesDetails()
    {
        DataTable dtNonlistedDetails = commonGatewayObj.Select("  SELECT  F_CD,COMP_CD,AMOUNT,RATE,NO_SHARES,TO_CHAR(INV_DATE, 'DD-MON-YYYY') as  INV_DATE,ENTRY_BY,  TO_CHAR (ENTRY_DATE, 'DD-MON-YYYY') as  ENTRY_DATE  FROM NON_LISTED_SECURITIES_DETAILS   ");
        return dtNonlistedDetails;
    }
    public DataTable NonlistedSecuritiesDetailsByCOMPID(string COMPCODE)
    {
        DataTable dtNonlistedDetails = commonGatewayObj.Select("  SELECT  F_CD,COMP_CD,AMOUNT,RATE,NO_SHARES,TO_CHAR(INV_DATE, 'DD-MON-YYYY') as  INV_DATE,ENTRY_BY,  TO_CHAR (ENTRY_DATE, 'DD-MON-YYYY') as  ENTRY_DATE  FROM NON_LISTED_SECURITIES_DETAILS  where comp_cd="+COMPCODE+" ");
        return dtNonlistedDetails;
    }

    public void ClearFields()
    {
        amountTextBox.Text = "";
      
    }
}
