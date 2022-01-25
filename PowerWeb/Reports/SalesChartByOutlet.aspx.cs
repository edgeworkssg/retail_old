using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PowerPOS.Container;

using PowerPOS;
using SubSonic;
using SubSonic.Utilities;

public partial class SalesChartByOutlet : PageBase
{
    private const int AMOUNT = 5;
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.AddDays(1).ToString("dd MMM yyyy");
        }
        BindGrid();
        ProductCategoryChart();
    }
    
    private void BindGrid()
    {
        string sql = "select outletname, sum(TotalActualCollected) as TotalCollected  " +
                        "from countercloselog a inner join " +
                        "pointofsale b  " +
                        "on a.pointofsaleid = b.pointofsaleid " +
                        "where endtime >= '" + txtStartDate.Text + "' " +
                        "and endtime < '" + txtEndDate.Text + "' " +
                        "group by outletname " +
                        "order by outletname ";
        Random random = new Random();
    
        QueryCommand cmd = new QueryCommand(sql, "PowerPOS");
        DataSet ds = DataService.GetDataSet(cmd);
        if (ds.Tables[0].Rows.Count > 0)
        {
            string Data = "", Names="", Colours="";
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Data += ds.Tables[0].Rows[i]["TotalCollected"].ToString() + ",";
                Names += ds.Tables[0].Rows[i]["outletname"].ToString() + "|";
                Colours += String.Format("{0:X6}", random.Next(0x1000000)) + "|"; 
            }


            Data = Uri.EscapeUriString(Data.Substring(0, Data.Length - 1));
            Names = Uri.EscapeUriString(Names.Substring(0, Names.Length - 1));
            Colours = Uri.EscapeUriString(Colours.Substring(0, Colours.Length - 1));
            iframe1.Attributes["src"] =
                "http://chart.apis.google.com/chart?chs=350x150&cht=p3&chco=" + Colours + "&chd=t:" + Data + "&chdl=" + Names + "&chma=5,5,5,5&chtt=Sales+By+Outlet";
        }
    }

    private void ProductCategoryChart()
    {
        string sql = "select categoryname, sum(amount) as amount  " +
                        "from orderhdr a inner join " +
                        "orderdet b  " +
                        "on a.orderhdrid = b.orderhdrid " +
                        "inner join item c on b.itemno=c.itemno " +
                        "where orderdate >= '" + txtStartDate.Text + "' " +
                        "and orderdate < '" + txtEndDate.Text + "' " +
                        "and a.isvoided=0 and b.isvoided=0 " +
                        "group by categoryname " +
                        "having sum(amount) > 0 ";                        
        Random random = new Random();

        QueryCommand cmd = new QueryCommand(sql, "PowerPOS");
        DataSet ds = DataService.GetDataSet(cmd);
        if (ds.Tables[0].Rows.Count > 0)
        {
            string Data = "", Names = "", Colours = "";
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Data += ds.Tables[0].Rows[i]["amount"].ToString() + ",";
                Names += ds.Tables[0].Rows[i]["categoryname"].ToString().Replace("&"," ") + "|";
                Colours += String.Format("{0:X6}", random.Next(0x1000000)) + "|";
            }
            Data = Uri.EscapeUriString(Data.Substring(0, Data.Length - 1));
            Names = Uri.EscapeUriString(Names.Substring(0, Names.Length - 1));
            Colours = Uri.EscapeUriString(Colours.Substring(0, Colours.Length - 1));
            iframe2.Attributes["src"] =
                "http://chart.apis.google.com/chart?chs=600x400&cht=p3&chco=" + Colours + "&chd=t:" + Data + "&chdl=" + Names + "&chma=5,5,5,5&chtt=Sales+By+Category";
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {

    }

    protected void lnkExport_Click(object sender, EventArgs e)
    {

    }

}
