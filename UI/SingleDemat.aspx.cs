﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UI_CompanyInformation : System.Web.UI.Page
{
    CommonGateway commonGatewayObj = new CommonGateway();
    List<PSDR_FI> psdrfilist = new List<PSDR_FI>();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            Session.RemoveAll();
            Response.Redirect("../Default.aspx");
        }

    }

    protected void saveButton_Click(object sender, EventArgs e)
    {
        DataTable dtsourcepsdfi = new DataTable();
        DataTable dtsource = new DataTable();
        string Query = "";
        string Query1 = "";
        string strInsQuery = "";
        string strUPQuery = "", strUPQuery2="", strUPQuery3 = "";

        DateTime currentdate = DateTime.Today;
     


        string p1date = currentdate.ToString("dd-MMM-yyyy");
        DateTime date1 = DateTime.ParseExact(DematsendingDateTextBox.Text, "dd/MM/yyyy", null);
        DateTime date2 = DateTime.ParseExact(HowladateTextBox.Text, "dd/MM/yyyy", null);

        //DateTime currentdate = DateTime.Today;
        string LoginName = Session["UserName"].ToString();

        string companycode = companyCodeTextBox.Text.ToString();
        string dematsendingNo = DematsendingNo.Text.ToString();
        string DemasendingDate = Convert.ToDateTime(date1).ToString("dd-MMM-yyyy");
        string fundcode = fundcodeTextBox.Text.ToString();
        string noofShare = SecuritiesTextBox.Text.ToString();
        string certificateNo = certificateNoTextBox.Text.ToString();
        string AllotmentNo = AllotmentNoTextBox.Text.ToString();
        string folio_no = folioNoTextBox.Text.ToString();
        string Dictincttivefrom = DictincttivefromTextBox.Text.ToString();
        string Dictincttiveto = DictincttivetoTextBox.Text.ToString();
        string ShareType = shareTypeTextBox.Text.ToString();
        string Howladate = Convert.ToDateTime(date2).ToString("dd-MMM-yyyy");
        string purchaseRate = PurchaseRateTextBox.Text.ToString();
        DataTable dtsource2 = new DataTable();
      
        string Query2 = "";

        Query2 = "select f_cd,no_shares,dis_no_fm,dis_no_to,sh_type from shr_dmat_fi where comp_cd = " + companyCodeTextBox.Text.ToString() + "  and cert_no = '" + certificateNoTextBox.Text.ToString() + "'";
        dtsource2 = commonGatewayObj.Select(Query2.ToString());

        if (dtsource2.Rows.Count > 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Certificate No. is Already Demetarialized!", true);
            
        }
        else
        {
            Query1 = "select folio_no from psdr_fi where comp_cd = '" + companyCodeTextBox.Text.ToString() + "' and f_cd = " + fundcodeTextBox.Text.ToString() + "  and sh_type <> 'T' and folio_no is not null";
            dtsourcepsdfi = commonGatewayObj.Select(Query1.ToString());
            if (dtsourcepsdfi.Rows.Count > 0)
            {
                psdrfilist = (from DataRow dr in dtsourcepsdfi.Rows
                              select new PSDR_FI()
                              {

                                  folio_no = dr["folio_no"].ToString()

                              }).ToList();
                foreach (PSDR_FI psdrfi in psdrfilist)
                {
                    if (folioNoTextBox.Text.ToString() == "")
                    {

      //                  insert into invest.shr_dmat_fi(comp_cd, dmat_no, dmat_dt, f_cd,
      //              no_shares, cert_no, allot_no, folio_no, op_name,
      //         dis_no_fm, dis_no_to, sh_type, sp_date, sp_rate, c_dt)
      //values(:nshdmt.comp_cd,:nshdmt.dmat_no,:nshdmt.dmat_dt,
      //       :nshdmt.f_cd, :nshdmt.no_shares,
      //       :nshdmt.cert_no,:nshdmt.allot_no, nvl(:nshdmt.folio_no, folio),:nshdmt.op_name,
      //       :nshdmt.dis_no_fm, :nshdmt.dis_no_to, :nshdmt.sh_type,
      //          :nshdmt.sp_date,:nshdmt.sp_rate, :nshdmt.c_dt);
                        strInsQuery = " insert into shr_dmat_fi(comp_cd,dmat_no,dmat_dt,f_cd,no_shares,cert_no,allot_no,folio_no,op_name,"+
                            "dis_no_fm,dis_no_to,sh_type,sp_date,sp_rate,c_dt )"+
                            " values(" + companycode + ","+ dematsendingNo + ",'"+ DemasendingDate + "',"+fundcode+","+ noofShare + ",'"+certificateNo+"',"+AllotmentNo+",'"+ psdrfi.folio_no+ "','"+LoginName+"','"+Dictincttivefrom+"','"+Dictincttiveto+"','"+ ShareType + "','"+Howladate+"','"+purchaseRate+"','"+p1date+"') ";
                    }

                    int NumOfRows = commonGatewayObj.ExecuteNonQuery(strInsQuery);
                    frmclear();

                    if (certificateNo != "")
                    {
                        strUPQuery = "update psdr_fi set posted = 'D', op_name = '" + LoginName + "' where comp_cd = " +
                            companyCodeTextBox.Text.ToString() + " and cert_no = '" + certificateNoTextBox.Text.ToString() + "'";

                        int NumOfRows1 = commonGatewayObj.ExecuteNonQuery(strUPQuery);
                    }
                    if (certificateNo == "" && AllotmentNo != "")
                    {
                        strUPQuery2 = "update psdr_fi set posted = 'D', op_name = '" + LoginName + "' where comp_cd = '" + companyCodeTextBox.Text.ToString() + "' and allot_no = '" + AllotmentNoTextBox.Text.ToString() + "' ";

                        int NumOfRows2 = commonGatewayObj.ExecuteNonQuery(strUPQuery2);
                    }
                    if (certificateNo != "" && AllotmentNo != "")
                    {
                        strUPQuery3 = "update psdr_fi set posted = 'D', op_name = '" + LoginName + "' where comp_cd = '" + companyCodeTextBox.Text.ToString() + "' and allot_no = '" + AllotmentNoTextBox.Text.ToString() + "' and cert_no = '" + certificateNoTextBox.Text.ToString() + "' ";

                        int NumOfRows3 = commonGatewayObj.ExecuteNonQuery(strUPQuery3);

                    }

                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Certificate No.  Demetarialized');", true);
                }

            }
        }
    
    }
    protected void deleteButton_Click(object sender, EventArgs e)
    {

      string  strDelQuery = "delete from shr_dmat_fi where comp_cd="+companyCodeTextBox.Text.ToString()+" and cert_no= '"+certificateNoTextBox.Text.ToString()+"'   and sh_type = '"+shareTypeTextBox.Text.ToString()+"'  and   no_shares='"+SecuritiesTextBox.Text.ToString()+"' and f_cd="+fundcodeTextBox.Text.ToString()+" and posted is null ";
      int NumOfRows = commonGatewayObj.ExecuteNonQuery(strDelQuery);
// Must be added below code
      //  update invest.psdr_fi
      // set posted = 'A'
      //where comp_cd =:shdmt1.comp_cd
      //    and cert_no = :shdmt1.cert_no
      //  and sh_type = :nshdmt.sh_type
      //   and   no_shares =:nshdmt.no_shares
      //      and f_cd =:nshdmt.f_cd
      //     and posted = 'D';


        frmclear();
      ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Delete Data Successfully');", true);

    }

    protected void certificateNoTextBox_TextChanged(object sender, EventArgs e)
    {
        //get_tosh



        DataTable dtsource = new DataTable();
        DataTable dtsource2 = new DataTable();
        DataTable dtsource3 = new DataTable();
      
        string  Query3 = "";

        //string  Query2 = "", Query3 = "";

        //Query2 = "select f_cd,no_shares,dis_no_fm,dis_no_to,sh_type from shr_dmat_fi where comp_cd = " + companyCodeTextBox.Text.ToString() + "  and cert_no = '" + certificateNoTextBox.Text.ToString() + "'";
        //dtsource2 = commonGatewayObj.Select(Query2.ToString());

        //if (dtsource2.Rows.Count > 0)
        //{
        //    shr_dmat_filist = (from DataRow dr in dtsource2.Rows
        //                       select new Shr_dmat_fi()
        //                       {

        //                           f_cd = dr["f_cd"].ToString(),
        //                           no_shares = dr["no_shares"].ToString(), 
        //                           dis_no_fm = dr["dis_no_fm"].ToString(),
        //                           dis_no_to = dr["dis_no_to"].ToString(),
        //                           sh_type = dr["sh_type"].ToString(),

        //                       }).ToList();
        //    foreach (Shr_dmat_fi shr_dmat_fia in shr_dmat_filist)
        //    {

        //        fundcodeTextBox.Text = shr_dmat_fia.f_cd;
        //        SecuritiesTextBox.Text = shr_dmat_fia.no_shares;           
        //        DictincttivefromTextBox.Text = shr_dmat_fia.dis_no_fm;
        //        DictincttivetoTextBox.Text = shr_dmat_fia.dis_no_to;
        //        shareTypeTextBox.Text = shr_dmat_fia.sh_type;


        //    }
        //}

     

        Query3 = "select f_cd,no_shares,dis_no_fm,dis_no_to,sh_type,sp_date, sp_rate , folio_no from psdr_fi where comp_cd = " + companyCodeTextBox.Text + " and cert_no = '" + certificateNoTextBox.Text + "' and posted = 'A'";
        dtsource3 = commonGatewayObj.Select(Query3.ToString());

        if (dtsource3.Rows.Count > 0)
        {
            psdrfilist = (from DataRow dr in dtsource3.Rows
                          select new PSDR_FI()
                          {
                              f_cd = dr["f_cd"].ToString(),
                              no_shares = dr["no_shares"].ToString(),
                              folio_no = dr["folio_no"].ToString(),
                              dis_no_fm = dr["dis_no_fm"].ToString(),
                              dis_no_to = dr["dis_no_to"].ToString(),
                              sh_type = dr["sh_type"].ToString(),
                              sp_date = dr["sp_date"].ToString(),
                              sp_rate = dr["sp_rate"].ToString(),
                          }).ToList();
            foreach (PSDR_FI psdrfi in psdrfilist)
            {
                fundcodeTextBox.Text = psdrfi.f_cd;
                SecuritiesTextBox.Text = psdrfi.no_shares;
                folioNoTextBox.Text = psdrfi.folio_no;
                DictincttivefromTextBox.Text = psdrfi.dis_no_fm;
                DictincttivetoTextBox.Text = psdrfi.dis_no_to;
                shareTypeTextBox.Text = psdrfi.sh_type;
                PurchaseRateTextBox.Text = psdrfi.sp_rate;
                HowladateTextBox.Text = Convert.ToDateTime(psdrfi.sp_date).ToString("dd-MMM-yyyy");
                //strLastBalDate = dtimeLastBalDate.Value.ToString("dd-MMM-yyyy");



            }
            if (shareTypeTextBox.Text.ToString() == "S")
            {
                if (PurchaseRateTextBox.Text.ToString() == "")
                {
                    PurchaseRateTextBox.Text = "100";
                }
            }
            else if (shareTypeTextBox.Text.ToString() == "B")
            {
                PurchaseRateTextBox.Text = "0";
            }
            else if (shareTypeTextBox.Text.ToString() == "T")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Third Party Certificate No. cannot be demated.');", true);
            }

            //get_fund, company name must be added here

        }
        else {
            PurchaseRateTextBox.Text = "0";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Invalid Certificate No!');", true);

        }

    }

    protected void AllotmentNoTextBox_TextChanged(object sender, EventArgs e)
    {

        DataTable dtsource = new DataTable();
        DataTable dtsource2 = new DataTable();
        DataTable dtsource3 = new DataTable();
        DataTable dtsource4 = new DataTable();
        List<Shr_dmat_fi> shr_dmat_filist = new List<Shr_dmat_fi>();

        string  Query2 = "",Query4 ="" ;



        if (certificateNoTextBox.Text.ToString() != "" && AllotmentNoTextBox.Text.ToString() != "")
        {

            Query4 = "select count(cert_no) from psdr_fi where comp_cd = " + companyCodeTextBox.Text.ToString() + "  and cert_no = '" + certificateNoTextBox.Text.ToString() + "'";
            dtsource4 = commonGatewayObj.Select(Query4.ToString());
            if (dtsource4.Rows.Count > 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('Duplicate Certificate No.')", true);
                deleteButton.Visible = true;
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "alert('No Certificate No. Found')", true);
            }
        }

        Query2 = "select comp_cd,dmat_no,dmat_dt,f_cd, no_shares,cert_no,allot_no,folio_no,op_name,dis_no_fm,dis_no_to,sh_type,sp_date,sp_rate from shr_dmat_fi where comp_cd =" + companyCodeTextBox.Text + " and cert_no ='" + certificateNoTextBox.Text + "'  and sh_type != 'T' and posted is null";
        dtsource2 = commonGatewayObj.Select(Query2.ToString());

        if (dtsource2.Rows.Count > 0)
        {
            shr_dmat_filist = (from DataRow dr in dtsource2.Rows
                               select new Shr_dmat_fi()
                               {
                                   comp_cd = dr["comp_cd"].ToString(),
                                   dmat_no = dr["dmat_no"].ToString(),
                                   dmat_dt = dr["dmat_dt"].ToString(),
                                   f_cd = dr["f_cd"].ToString(),
                                   no_shares = dr["no_shares"].ToString(),
                                   cert_no = dr["cert_no"].ToString(),
                                   allot_no = dr["allot_no"].ToString(),
                                   folio_no = dr["folio_no"].ToString(),
                                   op_name = dr["op_name"].ToString(),
                                   dis_no_fm = dr["dis_no_fm"].ToString(),
                                   dis_no_to = dr["dis_no_to"].ToString(),
                                   sh_type = dr["sh_type"].ToString(),
                                   sp_date = dr["sp_date"].ToString(),
                                   sp_rate = dr["sp_rate"].ToString(),
                               }).ToList();
            foreach (Shr_dmat_fi shr_dmat_fi in shr_dmat_filist)
            {
                AllotmentNoTextBox.Text = shr_dmat_fi.allot_no;
                DematsendingNo.Text = shr_dmat_fi.dmat_no;
                fundcodeTextBox.Text = shr_dmat_fi.f_cd;
                SecuritiesTextBox.Text = shr_dmat_fi.no_shares;
                folioNoTextBox.Text = shr_dmat_fi.folio_no;
                DictincttivefromTextBox.Text = shr_dmat_fi.dis_no_fm;
                DictincttivetoTextBox.Text = shr_dmat_fi.dis_no_to;
                shareTypeTextBox.Text = shr_dmat_fi.sh_type;
                PurchaseRateTextBox.Text = shr_dmat_fi.sp_rate;
                DematsendingDateTextBox.Text = Convert.ToDateTime(shr_dmat_fi.dmat_dt).ToString("dd/MM/yyyy");
            }

        }
      
        
    }
    public void frmclear()
    {
        companyCodeTextBox.Text = "";
        fundcodeTextBox.Text = "";
        certificateNoTextBox.Text = "";
        AllotmentNoTextBox.Text = "";
        HowladateTextBox.Text = "";
        SecuritiesTextBox.Text = "";
        DictincttivefromTextBox.Text = "";
        DictincttivetoTextBox.Text = "";
        folioNoTextBox.Text = "";
        shareTypeTextBox.Text = "";
        PurchaseRateTextBox.Text = "";
        DematsendingDateTextBox.Text = "";
        DematsendingNo.Text = "";
    }

    public class Shr_dmat_fi
    {
        public string comp_cd { get; set; }
        public string dmat_no { get; set; }
        public string dmat_dt { get; set; }
        public string f_cd { get; set; }
        public string no_shares { get; set; }
        public string cert_no { get; set; }
        public string allot_no { get; set; }
        public string folio_no { get; set; }
        public string op_name { get; set; }
        public string dis_no_fm { get; set; }
        public string dis_no_to { get; set; }
        public string sh_type { get; set; }
        public string sp_date { get; set; }
        public string sp_rate { get; set; }


    }
    public class PSDR_FI {

        public string f_cd { get; set; }
        public string no_shares { get; set; }
        public string dis_no_fm { get; set; }
        public string dis_no_to { get; set; }
        public string sh_type { get; set; }
        public string sp_date { get; set; }
        public string sp_rate { get; set; }
        public string folio_no { get; set; }
    }
}





