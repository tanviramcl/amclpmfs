using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for DropDownList
/// </summary>
public class DropDownList
{
    CommonGateway commonGatewayObj = new CommonGateway();
    
	public DropDownList()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public DataTable FillSectorDropDownList()//Sector Name
    {

        DataTable dtSectorName = commonGatewayObj.Select("SELECT * FROM INVEST.SECT_MAJ ORDER BY SECT_MAJ_NM");
        DataTable dtSectorNameDropDownList = new DataTable();
        dtSectorNameDropDownList.Columns.Add("SECT_MAJ_NM", typeof(string));
        dtSectorNameDropDownList.Columns.Add("SECT_MAJ_CD", typeof(string));
        DataRow dr = dtSectorNameDropDownList.NewRow();
        dr["SECT_MAJ_NM"] = "--Click Here to Select--";
        dr["SECT_MAJ_CD"] = "0";
        dtSectorNameDropDownList.Rows.Add(dr);
        for (int loop = 0; loop < dtSectorName.Rows.Count; loop++)
        {
            dr = dtSectorNameDropDownList.NewRow();
            dr["SECT_MAJ_NM"] = dtSectorName.Rows[loop]["SECT_MAJ_NM"].ToString();
            dr["SECT_MAJ_CD"] = Convert.ToInt32(dtSectorName.Rows[loop]["SECT_MAJ_CD"]);
            dtSectorNameDropDownList.Rows.Add(dr);
        }
        return dtSectorNameDropDownList;
    }
    public DataTable FillCompanyNameDropDownList()//For All Company Name
    {
        DataTable dtCompName = commonGatewayObj.Select("SELECT COMP_NM, COMP_CD FROM INVEST.COMP ORDER BY COMP_NM");
        DataTable dtCompNameDropDownList = new DataTable();
        dtCompNameDropDownList.Columns.Add("COMP_NM", typeof(string));
        dtCompNameDropDownList.Columns.Add("COMP_CD", typeof(string));
        DataRow dr = dtCompNameDropDownList.NewRow();
        dr["COMP_NM"] = "--Click Here to Select--";
        dr["COMP_CD"] = "0";
        dtCompNameDropDownList.Rows.Add(dr);
        for (int loop = 0; loop < dtCompName.Rows.Count; loop++)
        {
            dr = dtCompNameDropDownList.NewRow();
            dr["COMP_NM"] = dtCompName.Rows[loop]["COMP_NM"].ToString();
            dr["COMP_CD"] = Convert.ToInt32(dtCompName.Rows[loop]["COMP_CD"]);
            dtCompNameDropDownList.Rows.Add(dr);
        }
        return dtCompNameDropDownList;
    }
    public DataTable FundNameDropDownList()//For All Funds
    {
        DataTable dtFundName = commonGatewayObj.Select("SELECT F_NAME, F_CD FROM INVEST.FUND WHERE F_CD < 27 AND IS_F_CLOSE IS NULL AND BOID IS NOT NULL ORDER BY F_CD");
        DataTable dtFundNameDropDownList = new DataTable();
        dtFundNameDropDownList.Columns.Add("F_NAME", typeof(string));
        dtFundNameDropDownList.Columns.Add("F_CD", typeof(string));
        DataRow dr = dtFundNameDropDownList.NewRow();
        dr["F_NAME"] = "--Click Here to Select--";
        dr["F_CD"] = "0";
        dtFundNameDropDownList.Rows.Add(dr);
        for (int loop = 0; loop < dtFundName.Rows.Count; loop++)
        {
            dr = dtFundNameDropDownList.NewRow();
            dr["F_NAME"] = dtFundName.Rows[loop]["F_NAME"].ToString();
            dr["F_CD"] = Convert.ToInt32(dtFundName.Rows[loop]["F_CD"]);
            dtFundNameDropDownList.Rows.Add(dr);
        }
        return dtFundNameDropDownList;
    }
    public DataTable CloseEndFundNameDropDownList()//For Close End MF
    {
        DataTable dtCloseEndFundName = commonGatewayObj.Select("SELECT F_NAME, F_CD FROM INVEST.FUND WHERE (F_CD between 3 and 26) AND F_CD NOT IN(4) ORDER BY F_CD");
        DataTable dtCloseEndFundNameDropDownList = new DataTable();
        dtCloseEndFundNameDropDownList.Columns.Add("F_NAME", typeof(string));
        dtCloseEndFundNameDropDownList.Columns.Add("F_CD", typeof(string));
        DataRow dr = dtCloseEndFundNameDropDownList.NewRow();
        dr["F_NAME"] = "--All Close End Mutual Funds--";
        dr["F_CD"] = "0";
        dtCloseEndFundNameDropDownList.Rows.Add(dr);
        for (int loop = 0; loop < dtCloseEndFundName.Rows.Count; loop++)
        {
            dr = dtCloseEndFundNameDropDownList.NewRow();
            dr["F_NAME"] = dtCloseEndFundName.Rows[loop]["F_NAME"].ToString();
            dr["F_CD"] = Convert.ToInt32(dtCloseEndFundName.Rows[loop]["F_CD"]);
            dtCloseEndFundNameDropDownList.Rows.Add(dr);
        }
        return dtCloseEndFundNameDropDownList;
    }
    public DataTable OpenEndFundNameDropDownList()//For Open End MF
    {
        DataTable dtOpenEndFundName = commonGatewayObj.Select("SELECT F_NAME, F_CD FROM INVEST.FUND WHERE F_CD IN(2,4) ORDER BY F_CD");
        DataTable dtOpenEndFundNameDropDownList = new DataTable();
        dtOpenEndFundNameDropDownList.Columns.Add("F_NAME", typeof(string));
        dtOpenEndFundNameDropDownList.Columns.Add("F_CD", typeof(string));
        DataRow dr = dtOpenEndFundNameDropDownList.NewRow();
        dr["F_NAME"] = "--Click Here to Select--";
        dr["F_CD"] = "0";
        dtOpenEndFundNameDropDownList.Rows.Add(dr);
        for (int loop = 0; loop < dtOpenEndFundName.Rows.Count; loop++)
        {
            dr = dtOpenEndFundNameDropDownList.NewRow();
            dr["F_NAME"] = dtOpenEndFundName.Rows[loop]["F_NAME"].ToString();
            dr["F_CD"] = Convert.ToInt32(dtOpenEndFundName.Rows[loop]["F_CD"]);
            dtOpenEndFundNameDropDownList.Rows.Add(dr);
        }
        return dtOpenEndFundNameDropDownList;
    }
    public DataTable SignatoryDropDownList()//For Authorized Signatory
    {
        DataTable dtSignatoryName = commonGatewayObj.Select("SELECT ID, NAME FROM NAV_LETTER.SIGNATORY");
        DataTable dtSignatoryNameDropDownList = new DataTable();
        dtSignatoryNameDropDownList.Columns.Add("NAME", typeof(string));
        dtSignatoryNameDropDownList.Columns.Add("ID", typeof(string));
        DataRow dr = dtSignatoryNameDropDownList.NewRow();
        //dr["NAME"] = "Tanzina Ahsan";
        //dr["ID"] = "IAMCL411";
        //dtSignatoryNameDropDownList.Rows.Add(dr);
        for (int loop = 0; loop < dtSignatoryName.Rows.Count; loop++)
        {
            dr = dtSignatoryNameDropDownList.NewRow();
            dr["NAME"] = dtSignatoryName.Rows[loop]["NAME"].ToString();
            dr["ID"] = Convert.ToString(dtSignatoryName.Rows[loop]["ID"]);
            dtSignatoryNameDropDownList.Rows.Add(dr);
        }
        return dtSignatoryNameDropDownList;
    }
    public DataTable PaperNameDropDownList()//For Authorized Signatory
    {
        DataTable dtPaperName = commonGatewayObj.Select("SELECT ID, NEWS_PAPER_NAME FROM NAV_LETTER.NEWS_PAPER");
        DataTable dtPaperNameDropDownList = new DataTable();
        dtPaperNameDropDownList.Columns.Add("NEWS_PAPER_NAME", typeof(string));
        dtPaperNameDropDownList.Columns.Add("ID", typeof(string));
        DataRow dr = dtPaperNameDropDownList.NewRow();
        for (int loop = 0; loop < dtPaperName.Rows.Count; loop++)
        {
            dr = dtPaperNameDropDownList.NewRow();
            dr["NEWS_PAPER_NAME"] = dtPaperName.Rows[loop]["NEWS_PAPER_NAME"].ToString();
            dr["ID"] = Convert.ToString(dtPaperName.Rows[loop]["ID"]);
            dtPaperNameDropDownList.Rows.Add(dr);
        }
        return dtPaperNameDropDownList;
    }
    public DataTable MonthlyBankAdviceDropDownList()//For Month of Bank Advice
    {
        DataTable dtMonthOfBankAdvice = commonGatewayObj.Select("SELECT  CAL_DATE, TO_CHAR(CAL_DATE, 'MONTH,YYYY') AS MONTH_OF_BANK_ADVICE FROM INVEST.AMCL_EMP_SALARY GROUP BY CAL_DATE ORDER BY CAL_DATE DESC");
        DataTable dtMonthOfBankAdviceDropDownList = new DataTable();
        dtMonthOfBankAdviceDropDownList.Columns.Add("MONTH_OF_BANK_ADVICE", typeof(string));
        dtMonthOfBankAdviceDropDownList.Columns.Add("CAL_DATE", typeof(string));
        DataRow dr = dtMonthOfBankAdviceDropDownList.NewRow();
        dr["MONTH_OF_BANK_ADVICE"] = "--Select--";
        dr["CAL_DATE"] = "0";
        dtMonthOfBankAdviceDropDownList.Rows.Add(dr);
        for (int loop = 0; loop < dtMonthOfBankAdvice.Rows.Count; loop++)
        {
            dr = dtMonthOfBankAdviceDropDownList.NewRow();
            dr["MONTH_OF_BANK_ADVICE"] = dtMonthOfBankAdvice.Rows[loop]["MONTH_OF_BANK_ADVICE"].ToString();
            dr["CAL_DATE"] = Convert.ToString(dtMonthOfBankAdvice.Rows[loop]["CAL_DATE"]);
            dtMonthOfBankAdviceDropDownList.Rows.Add(dr);
        }
        return dtMonthOfBankAdviceDropDownList;
    }
    public DataTable HowlaDateDropDownList()//Get Howla Date from invest.fund_trans_hb Table
    {
        DataTable dtHowlaDate = commonGatewayObj.Select("SELECT DISTINCT VCH_DT    FROM   INVEST.FUND_TRANS_HB  ORDER BY VCH_DT DESC");
        DataTable dtHowlaDateDropDownList = new DataTable();
        dtHowlaDateDropDownList.Columns.Add("Howla_Date", typeof(string));
        dtHowlaDateDropDownList.Columns.Add("VCH_DT", typeof(string));
        DataRow dr = dtHowlaDateDropDownList.NewRow();
        dr["Howla_Date"] = "--Select--";
        dr["VCH_DT"] = "0";
        dtHowlaDateDropDownList.Rows.Add(dr);
        for (int loop = 0; loop < dtHowlaDate.Rows.Count; loop++)
        {
            dr = dtHowlaDateDropDownList.NewRow();
            dr["Howla_Date"] = Convert.ToDateTime(dtHowlaDate.Rows[loop]["VCH_DT"]).ToString("dd-MMM-yyyy");
            dr["VCH_DT"] = Convert.ToDateTime(dtHowlaDate.Rows[loop]["VCH_DT"]).ToString("dd-MMM-yyyy");
            dtHowlaDateDropDownList.Rows.Add(dr);
        }
        return dtHowlaDateDropDownList;
    }
}
