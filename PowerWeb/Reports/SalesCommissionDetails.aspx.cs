using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SubSonic;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using System.Globalization;
using PowerPOS;
using System.Xml.Linq;

namespace PowerWeb.Reports
{
    public partial class SalesCommissionDetails : System.Web.UI.Page
    {
        private const string DS1Key = "SalesCommissionDetails.Headers";
        private const string DS2Key = "SalesCommissionDetails.Details";

        protected void Page_Load(object sender, EventArgs e)
        {
            ReportDocument rptDoc = new ReportDocument();
            rptDoc.Load(Server.MapPath(@"~\bin\Reports\ReportFiles\Sales\SalesCommissionDetails.rpt"));

            List<Rpt_SalesCommissionDetail_Header> headers = new List<Rpt_SalesCommissionDetail_Header>();
            List<Rpt_SalesCommissionDetail_Detail> details = new List<Rpt_SalesCommissionDetail_Detail>();

            if (!IsPostBack)
            {
                String month = Request.QueryString["Month"];
                String staff = Request.QueryString["Staff"];    

                #region *) Fill Header

                {
                    QueryCommand cmd = new QueryCommand(@"
                        SELECT
                            Staff
                            ,Salary
                            ,OtherAllowance
                            ,Total
                        FROM SalesCommissionSummary
                        WHERE
                            [Month] = @Month
                            AND Staff = @Staff
                    ");

                    cmd.AddParameter("@Month", month);
                    cmd.AddParameter("@Staff", staff);

                    var rdr = DataService.GetReader(cmd);
                    if (rdr.Read())
                    {
                        string _staff = rdr["Staff"].ToString();

                        double salary = 0d;
                        double otherAllowance = 0d;
                        double grandTotal = 0d;
                        double totalDeduction = 0d;

                        double.TryParse(rdr["Salary"].ToString(), out salary);
                        double.TryParse(rdr["OtherAllowance"].ToString(), out otherAllowance);
                        double.TryParse(rdr["Total"].ToString(), out grandTotal);

                        QueryCommand cmd2 = new QueryCommand(@"
                            SELECT SUM(Deduction) as TotalDeduction FROM SalesCommissionDetails_Deduction WHERE [Month] = @Month AND Staff = @Staff
                        ");

                        cmd2.AddParameter("@Month", month);
                        cmd2.AddParameter("@Staff", staff);

                        var rdr2 = DataService.GetReader(cmd2);
                        if (rdr2.Read())
                            double.TryParse(rdr2["TotalDeduction"].ToString(), out totalDeduction);

                        DateTime dtStart = new DateTime();
                        DateTime dtEnd = new DateTime();
                        try
                        {
                            dtStart = DateTime.ParseExact(month, "MMM yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            dtEnd = dtStart.AddMonths(1).AddDays(-1);
                        }
                        catch(Exception ex){}

                        var h = new Rpt_SalesCommissionDetail_Header
                        {
                            Salesman = _staff,
                            StartDate = dtStart.ToString("dd MMM yyyy"),
                            EndDate = dtEnd.ToString("dd MMM yyyy"),
                            BasicSalary = new decimal(salary),
                            OtherAllowance = new decimal(otherAllowance),
                            TotalDeduction = new decimal(totalDeduction),
                            GrantTotal = new decimal(grandTotal)
                        };

                        headers.Add(h);
                    }
                }

                Session[DS1Key] = headers;

                #endregion

                #region *) Fill Details

                {
                    List<Scheme> schemes = new List<Scheme>();

                    QueryCommand cmd = new QueryCommand(@"
                        SELECT CommissionType, TotalQty, TotalSales FROM SalesCommissionDetails WHERE [Month] = @Month AND Staff = @Staff
                    ");

                    cmd.AddParameter("@Month", month);
                    cmd.AddParameter("@Staff", staff);

                    var rdr = DataService.GetReader(cmd);
                    while (rdr.Read())
                    {
                        double totalQty = 0d;
                        double totalSales = 0d;

                        string commissionType = rdr["CommissionType"].ToString();
                        double.TryParse(rdr["TotalQty"].ToString(), out totalQty);
                        double.TryParse(rdr["TotalSales"].ToString(), out totalSales);

                        var tmps = commissionType.Split('-');
                        var scheme = tmps[0].Trim();

                        SalesType st = new SalesType();
                        st.Type = tmps[1].Trim();
                        st.SalesAmount = totalSales;
                        st.SalesQty = totalQty;

                        var obj = (from p in schemes where p.SchemeName == scheme select p).FirstOrDefault();
                        if (obj == null)
                        {
                            var sts = new List<SalesType>();
                            sts.Add(st);

                            schemes.Add(new Scheme { SchemeName = scheme, SalesTypes = sts });
                        }
                        else
                        {
                            obj.SalesTypes.Add(st);
                        }
                    }

                    foreach (var sch in schemes)
                    {
                        var product = (from p in sch.SalesTypes where p.Type.ToUpper().StartsWith("PRODUCT") select p).FirstOrDefault();
                        var service = (from p in sch.SalesTypes where p.Type.ToUpper().StartsWith("SERVICE") select p).FirstOrDefault();
                        var pointSold = (from p in sch.SalesTypes where p.Type.ToUpper().StartsWith("POINT_SOLD") select p).FirstOrDefault();
                        var pointRedeem = (from p in sch.SalesTypes where p.Type.ToUpper().StartsWith("POINT_REDEEM") select p).FirstOrDefault();
                        var packageSold = (from p in sch.SalesTypes where p.Type.ToUpper().StartsWith("PACKAGE_SOLD") select p).FirstOrDefault();
                        var packageRedeem = (from p in sch.SalesTypes where p.Type.ToUpper().StartsWith("PACKAGE_REDEEM") select p).FirstOrDefault();


                        List<CommissionLine> commissionLines = new List<CommissionLine>();

                        QueryCommand cmd2 = new QueryCommand(@"
                            SELECT CommissionText, CommissionValue FROM SalesCommissionDetails_Commission WHERE [Month] = @Month AND Staff = @Staff AND Scheme = @Scheme
                        ");
                        cmd2.AddParameter("@Month", month);
                        cmd2.AddParameter("@Staff", staff);
                        cmd2.AddParameter("@Scheme", sch.SchemeName);

                        var rdr2 = DataService.GetReader(cmd2);
                        while (rdr2.Read())
                        {
                            double commissionValue = 0d;
                            double.TryParse(rdr2["CommissionValue"].ToString(), out commissionValue);

                            commissionLines.Add(new CommissionLine
                            {
                                CommissionText = rdr2["CommissionText"].ToString(),
                                CommissionValue = commissionValue
                            });
                        }

                        var d = new Rpt_SalesCommissionDetail_Detail
                        {
                            SchemeName = sch.SchemeName,
                            Attribute1 = product == null ? "PRODUCT" : product.Type,
                            Attribute2 = service == null ? "SERVICE" : service.Type,
                            Attribute3 = pointSold == null ? "POINT_SOLD" : pointSold.Type,
                            Attribute4 = pointRedeem == null ? "POINT_REDEEM" : pointRedeem.Type,
                            Attribute5 = packageSold == null ? "PACKAGE_SOLD" : packageSold.Type,
                            Attribute6 = packageRedeem == null ? "PACKAGE_REDEEM" : packageRedeem.Type,
                            Sales1 = product == null ? 0m : new Decimal(product.SalesAmount),
                            Sales2 = service == null ? 0m : new Decimal(service.SalesAmount),
                            Sales3 = pointSold == null ? 0m : new Decimal(pointSold.SalesAmount),
                            Sales4 = pointRedeem == null ? 0m : new Decimal(pointRedeem.SalesAmount),
                            Sales5 = packageSold == null ? 0m : new Decimal(packageSold.SalesAmount),
                            Sales6 = packageRedeem == null ? 0m : new Decimal(packageRedeem.SalesAmount),
                            CommissionRange1 = 0 < commissionLines.Count ? commissionLines[0].CommissionText : "",
                            CommissionRange2 = 1 < commissionLines.Count ? commissionLines[1].CommissionText : "",
                            CommissionRange3 = 2 < commissionLines.Count ? commissionLines[2].CommissionText : "",
                            CommissionRange4 = 3 < commissionLines.Count ? commissionLines[3].CommissionText : "",
                            CommissionRange5 = 4 < commissionLines.Count ? commissionLines[4].CommissionText : "",
                            CommissionAmount1 = 0 < commissionLines.Count ? new decimal(commissionLines[0].CommissionValue) : 0m,
                            CommissionAmount2 = 1 < commissionLines.Count ? new decimal(commissionLines[1].CommissionValue) : 0m,
                            CommissionAmount3 = 2 < commissionLines.Count ? new decimal(commissionLines[2].CommissionValue) : 0m,
                            CommissionAmount4 = 3 < commissionLines.Count ? new decimal(commissionLines[3].CommissionValue) : 0m,
                            CommissionAmount5 = 4 < commissionLines.Count ? new decimal(commissionLines[4].CommissionValue) : 0m,
                        };

                        d.TotalSales = d.Sales1 + d.Sales2 + d.Sales3 + d.Sales4 + d.Sales5 + d.Sales6;
                        d.TotalCommission = d.CommissionAmount1 + d.CommissionAmount2 + d.CommissionAmount3 + d.CommissionAmount4 + d.CommissionAmount5;

                        if (d.TotalSales > 0 && d.TotalCommission > 0)
                            details.Add(d);
                    }
                }

                if (details.Count == 0)
                {
                    details.Add(new Rpt_SalesCommissionDetail_Detail
                    {
                        SchemeName = "",
                        Attribute1 = "PRODUCT",
                        Attribute2 = "SERVICE",
                        Attribute3 = "POINT_SOLD",
                        Attribute4 = "POINT_REDEEM",
                        Attribute5 = "PACKAGE_SOLD",
                        Attribute6 = "PACKAGE_REDEEM",
                        Sales1 = 0m,
                        Sales2 = 0m,
                        Sales3 = 0m,
                        Sales4 = 0m,
                        Sales5 = 0m,
                        Sales6 = 0m,
                        CommissionRange1 = "",
                        CommissionRange2 = "",
                        CommissionRange3 = "",
                        CommissionRange4 = "",
                        CommissionRange5 = "",
                        CommissionAmount1 = 0m,
                        CommissionAmount2 = 0m,
                        CommissionAmount3 = 0m,
                        CommissionAmount4 = 0m,
                        CommissionAmount5 = 0m,
                    });
                }

                Session[DS2Key] = details;

                #endregion
            }
            else
            {
                headers = (List<Rpt_SalesCommissionDetail_Header>) Session[DS1Key];
                details = (List<Rpt_SalesCommissionDetail_Detail>) Session[DS2Key];
            }

            rptDoc.Database.Tables[0].SetDataSource(headers);
            rptDoc.Database.Tables[1].SetDataSource(details);
            crViewer.ReportSource = rptDoc;
        }
    }

    public class Scheme
    {
        public string SchemeName { get; set; }
        public List<SalesType> SalesTypes { get; set; }
    }

    public class SalesType
    {
        public string Type { get; set; }
        public double SalesQty { get; set; }
        public double SalesAmount { get; set; }
    }

    public class CommissionLine
    {
        public string CommissionText { get; set; }
        public double CommissionValue { get; set; }
    }

    public class Rpt_SalesCommissionDetail_Header
    {
        public string Salesman { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public decimal BasicSalary { get; set; }

        public decimal OtherAllowance { get; set; }

        public decimal TotalSales { get; set; }

        public decimal TotalCommission { get; set; }

        public decimal TotalDeduction { get; set; }

        public decimal GrantTotal { get; set; }
    }

    public class Rpt_SalesCommissionDetail_Detail
    {
        public string SchemeName { get; set; }

        public String Attribute1 { get; set; }

        public String Attribute2 { get; set; }

        public String Attribute3 { get; set; }

        public String Attribute4 { get; set; }

        public String Attribute5 { get; set; }

        public String Attribute6 { get; set; }

        public decimal Sales1 { get; set; }

        public decimal Sales2 { get; set; }

        public decimal Sales3 { get; set; }

        public decimal Sales4 { get; set; }

        public decimal Sales5 { get; set; }

        public decimal Sales6 { get; set; }

        public decimal TotalSales { get; set; }

        public string CommissionRange1 { get; set; }

        public string CommissionRange2 { get; set; }

        public string CommissionRange3 { get; set; }

        public string CommissionRange4 { get; set; }

        public string CommissionRange5 { get; set; }


        public decimal CommissionAmount1 { get; set; }

        public decimal CommissionAmount2 { get; set; }

        public decimal CommissionAmount3 { get; set; }

        public decimal CommissionAmount4 { get; set; }

        public decimal CommissionAmount5 { get; set; }


        public decimal TotalCommission { get; set; }
    }

}
