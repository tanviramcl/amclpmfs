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
        DataTable dtpdateDropDownList = pdateDropDownList();
        if (!IsPostBack)
        {
            p1dateDropDownList.DataSource = dtpdateDropDownList;
            p1dateDropDownList.DataTextField = "p1date";
            p1dateDropDownList.DataValueField = "p1date";
            p1dateDropDownList.DataBind();

            p2dateDropDownList.DataSource = dtpdateDropDownList;
            p2dateDropDownList.DataTextField = "p2date";
            p2dateDropDownList.DataValueField = "p2date";
            p2dateDropDownList.DataBind();
        }

    }

    protected void showButton_Click(object sender, EventArgs e)
    {

        string p1date = p1dateDropDownList.Text.ToString();
        string p2date = p2dateDropDownList.Text.ToString();


        StringBuilder sb = new StringBuilder();
        //sb.Append("window.open('ReportViewer/NonDemateSharesCheckReportViwer.aspx?p1date=" + p1date + " &p2date= " + p2date + ");");
        //ClientScript.RegisterStartupScript(this.GetType(), "ReportViwer", sb.ToString(), true);
        Response.Redirect("ReportViewer/CapitalGainSummeryDateWiseReportViwer.aspx?p1date=" + p1date + "&p2date=" + p2date);
    }

    protected void p1dateDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    public DataTable pdateDropDownList()//For Authorized Signatory
    {
       
        CommonGateway commonGatewayObj = new CommonGateway();
        DataTable p1date = commonGatewayObj.Select("SELECT MAX( VCH_DT)-1 as p1date FROM FUND_TRANS_HB");
        DataTable p2date = commonGatewayObj.Select("SELECT MAX(VCH_DT) as p2date FROM FUND_TRANS_HB");
        DataTable pdateDropDownList = new DataTable();
        pdateDropDownList.Columns.Add("p1date", typeof(string));
        pdateDropDownList.Columns.Add("p2date", typeof(string));
        DataRow dr = pdateDropDownList.NewRow();
        DataRow dr1 = pdateDropDownList.NewRow();

        for (int loop = 0; loop < p1date.Rows.Count; loop++)
        {
            //dr = pdateDropDownList.NewRow();
            dr["p1date"] = Convert.ToDateTime(p1date.Rows[loop]["p1date"]).ToString("dd-MMM-yyyy");
           // dr["p2date"] = Convert.ToDateTime(pdate.Rows[loop]["vch_dt"]).ToString("dd-MMM-yyyy");
            pdateDropDownList.Rows.Add(dr);
        }
        for (int loop = 0; loop < p2date.Rows.Count; loop++)
        {
           // dr = pdateDropDownList.NewRow();
           
            dr["p2date"] = Convert.ToDateTime(p2date.Rows[loop]["p2date"]).ToString("dd-MMM-yyyy");
            pdateDropDownList.Rows.Add(dr1);
        }
        return pdateDropDownList;
    }

    protected void p2dateDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}