using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UI_ReportViewer_StockDeclarationBeforePostedReportViewer : System.Web.UI.Page
{
    CommonGateway commonGatewayObj = new CommonGateway();
    private ReportDocument rdoc = new ReportDocument();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            Session.RemoveAll();
            Response.Redirect("../../Default.aspx");
        }

        
        DataTable dtReprtSource = new DataTable();
        StringBuilder sbMst = new StringBuilder();
        StringBuilder sbfilter = new StringBuilder();
        sbfilter.Append(" ");
        sbMst.Append("SELECT  COMP_CD, COMP_NM, A.SECT_MAJ_CD,b.SECT_MAJ_NM, SECT_MIN_CD, INSTR_CD, CAT_TP, ADD1, ADD2, REG_OFF, PRN_STH, OPN_DT, TAX_HDAY, TEL, TLX, E_MAIL" +
                    " PROD, PRO_VOL, SPNR, ATHO_CAP, PAID_CAP, NO_SHRS, FC_VAL, MLOT, SBASE_RT, FLOT_DT_FM, FLOT_DT_TO, BOK_CL_FDT, BOK_CL_TDT, MARGIN, AVG_RT,  " +
                    " RT_UPD_DT, FLAG, AUDITOR, NS_ICB, NS_UNIT, NS_MUTUAL, PMARGIN, RISSU_DT_FM, RISSU_DT_TO, PREMIUM, CFLAG, MAR_FLOAT, MON_TO, TRADE_METH, CSEINSTR_CD,  "+
                    " INDX_LST, BASE_UPD_DT, CDS, CSE_SID, CSE_RT, CSE_DT, ADC_RT, VALID, DSE_HIGH,  DSE_OPEN, DSE_LOW, ISADD_HOWLACHARGE_DSE,  ADD_HOWLACHARGE_AMTDSE, EXCEP_BUYSL_COMPCT_DSE  FROM COMP a INNER JOIN SECT_MAJ  b ON  A.SECT_MAJ_CD=B.SECT_MAJ_CD order by COMP_NM   ");
        sbMst.Append(sbfilter.ToString());
        
        dtReprtSource = commonGatewayObj.Select(sbMst.ToString());
        dtReprtSource.TableName = "CompanyInfomationReport";
        dtReprtSource.WriteXmlSchema(@"D:\IAMCL_10-7-17\amclpmfs\amclpmfs\UI\ReportViewer\Report\CompanyInformationReport.xsd");
        if (dtReprtSource.Rows.Count > 0)
        {
            //string Path = Server.MapPath("~/Report/crtNegativeBalanceCheckReport.rpt");
            string Path = Server.MapPath("Report/CR_CompayInformation.rpt");
            rdoc.Load(Path);
            rdoc.SetDataSource(dtReprtSource);
            CR_CompanyInformationReport.ReportSource = rdoc;
            CR_CompanyInformationReport.DisplayToolbar = true;
            CR_CompanyInformationReport.HasExportButton = true;
            CR_CompanyInformationReport.HasPrintButton = true;
            //rdoc.SetParameterValue("prmtransTypeDetais", transTypeDetais);
            rdoc = ReportFactory.GetReport(rdoc.GetType());

        }
        else
        {
            Response.Write("No Data Found");
        }


    }
    protected void Page_Unload(object sender, EventArgs e)
    {
        CR_CompanyInformationReport.Dispose();
        CR_CompanyInformationReport = null;
        rdoc.Close();
        rdoc.Dispose();
        rdoc = null;
        GC.Collect();
    }
}