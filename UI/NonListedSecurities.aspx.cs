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
       


        Session["NonlistedDetails"] = NonlistedSecuritiesDetails();

        DataTable dtNonlistedSecurities = (DataTable)Session["dtNonlistedSecurities"];

        //grdShowDSEMP.DataSource = dtNonlistedSecurities;
        //grdShowDSEMP.DataBind();

        if (!IsPostBack)
        {
           



        }
    }
    
    public DataTable NonlistedSecuritiesDetails()
    {
        DataTable dtNonlistedDetails = commonGatewayObj.Select("  SELECT  F_CD,COMP_CD,AMOUNT,RATE,NO_SHARES,TO_CHAR(INV_DATE, 'DD-MON-YYYY') as  INV_DATE,ENTRY_BY,  TO_CHAR (ENTRY_DATE, 'DD-MON-YYYY') as  ENTRY_DATE  FROM NON_LISTED_SECURITIES_DETAILS   ");
        return dtNonlistedDetails;
    }

   
    public void ClearFields()
    {
        
      
    }
}
