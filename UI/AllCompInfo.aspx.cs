using System;
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
        Session["CompInfo"] = GetAllCompInfo();

        DataTable dtCompInfo = (DataTable)Session["CompInfo"];



        //  companyNameTextBox.Text = "sss";
    }

    protected void saveButton_Click(object sender, EventArgs e)
    {

      

    }

    private DataTable GetAllCompInfo()
    {
        DataTable dtCompInfo = new DataTable();

        StringBuilder sbMst = new StringBuilder();
        StringBuilder sbOrderBy = new StringBuilder();
        sbOrderBy.Append("");

        sbMst.Append(" SELECT COMP_CD, COMP_NM, SECT_MAJ_CD,INSTR_CD, CAT_TP, ATHO_CAP, PAID_CAP, NO_SHRS, ");
        sbMst.Append(" FC_VAL, ISADD_BUYSLCHARGE_APPLDSE,ADD_BUYSLCHARGE_AMTDSE FROM COMP ");
        sbOrderBy.Append(" ORDER BY COMP_CD ");

        sbMst.Append(sbOrderBy.ToString());

        dtCompInfo = commonGatewayObj.Select(sbMst.ToString());

        Session["CompInfo"] = dtCompInfo;
        return dtCompInfo;
    }

    protected void AddButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("CompanyInformation.aspx");
    }

    protected void UpdateButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddFund.aspx");
    }
}