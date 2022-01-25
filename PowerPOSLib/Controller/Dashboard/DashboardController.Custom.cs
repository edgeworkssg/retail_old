using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Transactions;
using System.Collections;
using PowerPOS.Container;
using SubSonic;
using System.ComponentModel;
using PowerPOS;
using System.Linq;


namespace PowerPOS
{
    public partial class DashboardController
    {
        public List<ChartData> FetchAllChart()
        {
            List<ChartData> listChart = new List<ChartData>();
            var dashboardData = (from o in this.FetchAll()
                                 where o.Deleted == false && o.IsEnable == true
                                 orderby o.DisplayOrder ascending
                                 select o).ToList();

            foreach (var dashboardItem in dashboardData)
            {
                ChartData newChartData = new ChartData();
                newChartData.ID = "chart_" + dashboardItem.Id;
                newChartData.Title = dashboardItem.Title;
                newChartData.SubTitle = dashboardItem.SubTitle;
                newChartData.Description = dashboardItem.Description;
                newChartData.Height = dashboardItem.Height;
                newChartData.Width = dashboardItem.Width;
                newChartData.IsInline = dashboardItem.IsInline.GetValueOrDefault(false);
                newChartData.BreakAfter = dashboardItem.BreakAfter.GetValueOrDefault(false);
                newChartData.BreakBefore = dashboardItem.BreakBefore.GetValueOrDefault(false);
                newChartData.ChartScript = GetChartScript(dashboardItem,"");
                listChart.Add(newChartData);

            }

            return listChart;
        }

        public string FetchChartScript()
        {
            string chartScript = "";

            try
            {

                var allChartData = FetchAllChart();
                foreach (var chartItem in allChartData)
                {
                    if (chartItem.BreakBefore)
                        chartScript += ChartScript.Break + "\n";

                    chartScript += chartItem.ChartScript + "\n";

                    if (chartItem.BreakAfter)
                        chartScript += ChartScript.Break + "\n";
                }
            }
            catch (Exception ex)
            {
                chartScript = GetChartScript(new Dashboard { Id = 1, Title="FAILED TO LOAD DASHBOARD", Width="94%", Height="50px" }, ex.Message);
                Logger.writeLog(ex);
            }
            return chartScript;
        }

        private string GetChartScript(Dashboard data, string message)
        {
            string finalScript = "";

            DataTable dt = new DataTable();
            string divChartTitle = string.IsNullOrEmpty(data.Title) ? "" : string.Format(ChartScript.ChartTitleDiv, data.Title);
            string divChartSubTitle = string.IsNullOrEmpty(data.SubTitle) ? "" : string.Format(ChartScript.ChartSubTitleDiv, data.SubTitle);
            string divChartDesc = string.IsNullOrEmpty(data.Description) ? "" : string.Format(ChartScript.ChartDescriptionDiv, data.Description);
            try
            {
                string chartID = "chart_"+data.Id;
                string divChartScript = ""; //string.Format(ChartScript.ChartDiv, chartID, data.Width, data.Height);
                if (!string.IsNullOrEmpty(data.SQLString))
                {
                    divChartScript = string.Format(ChartScript.ChartDiv, chartID, data.Width, data.Height);
                    dt.Load(SubSonic.DataService.GetReader(new SubSonic.QueryCommand(data.SQLString, "PowerPOS")));

                    if (data.PlotType == ChartType.Lines.ToString()
                        || data.PlotType == ChartType.SteppedAreaChart.ToString()
                        || data.PlotType == ChartType.AreaChart.ToString())
                    {
                        dt = dt.Transpose();
                    }

                    string chartDataScript = "";
                    if(data.PlotType == ChartType.Table.ToString())
                        chartDataScript = string.Format(ChartScript.DataScript, dt.AsJSArrayForTable());
                    else
                        chartDataScript = string.Format(ChartScript.DataScript, dt.AsJSArray());


                    string chartOptionScript = ""; 
                    if(!string.IsNullOrEmpty(data.PlotOption))
                        chartOptionScript = string.Format(ChartScript.OptionsScript, data.PlotOption);
                    string visualizeChartScript = "";

                    if (data.PlotType == ChartType.ColumnChart.ToString())
                        visualizeChartScript = string.Format(ChartScript.VisualizeColumnChartScript, chartID);
                    else if (data.PlotType == ChartType.AreaChart.ToString())
                        visualizeChartScript = string.Format(ChartScript.VisualizeAreaChartScript, chartID);
                    else if (data.PlotType == ChartType.BarChart.ToString())
                        visualizeChartScript = string.Format(ChartScript.VisualizeBarChartScript, chartID);
                    else if (data.PlotType == ChartType.Lines.ToString())
                        visualizeChartScript = string.Format(ChartScript.VisualizeLineChartScript, chartID);
                    else if (data.PlotType == ChartType.PieChart.ToString())
                        visualizeChartScript = string.Format(ChartScript.VisualizePieChartScript, chartID);
                    else if (data.PlotType == ChartType.Table.ToString())
                        visualizeChartScript = string.Format(ChartScript.VisualizeTableScript, chartID);
                    else if (data.PlotType == ChartType.SteppedAreaChart.ToString())
                        visualizeChartScript = string.Format(ChartScript.VisualizeSteppedAreaChartScript, chartID);
                    else
                        throw new Exception("Plot Type Not Recognized");
                    string innerFinalScript = "";

                    if(data.PlotType == ChartType.Table.ToString())
                        innerFinalScript += string.Format("\n {0} \n", ChartScript.TablePackageScript);
                    else
                        innerFinalScript += string.Format("\n {0} \n", ChartScript.CoreChartPackageScript);


                    innerFinalScript += string.Format("\n {0} \n", ChartScript.LoadChartScript);
                    innerFinalScript += string.Format("\n {0} \n", string.Format(ChartScript.DrawChartFunctionScript,
                          string.Format("\n {0} \n", chartDataScript)
                        + string.Format("\n {0} \n", chartOptionScript)
                        + string.Format("\n {0} \n", visualizeChartScript)
                        + string.Format("\n {0} \n", ChartScript.DrawChartScript)));
                    string divTagScript = string.Format(ChartScript.NormalDiv, string.Format(ChartScript.Script, innerFinalScript));
                    finalScript = string.Format(ChartScript.ChartPanelDiv,
                        data.Width, data.IsInline.GetValueOrDefault(false) ? "inline-block" : "block",
                        divTagScript + "\n" + divChartTitle + "\n" + divChartSubTitle + "\n" + divChartScript + "\n" + divChartDesc);
                }
                else
                {
                    divChartScript = string.Format(ChartScript.ChartDivWithText, chartID, data.Width, data.Height, message);
                    finalScript = string.Format(ChartScript.ChartPanelDiv,
                        data.Width, data.IsInline.GetValueOrDefault(false) ? "inline-block" : "block", divChartTitle + "\n" + divChartSubTitle + "\n" + divChartScript + "\n" + divChartDesc);
                }
            }
            catch (Exception ex)
            {
                finalScript = string.Format(ChartScript.ChartPanelDiv,
                    data.Width, data.IsInline.GetValueOrDefault(false) ? "inline-block" : "block", 
                    divChartTitle + "\n" + divChartSubTitle + "\n" + string.Format(ChartScript.ErrorScriptDiv, "Error On SQL Query : " + ex.Message) + "\n" + divChartDesc);
                Logger.writeLog(ex);
            }

            return finalScript;
        }
    }

    public enum ChartType
    {
        ColumnChart,
        BarChart,
        AreaChart,
        Lines,
        PieChart,
        Table,
        SteppedAreaChart
    }

    public class ChartData
    {
        public string ID { set; get; }
        public string Title { set; get; }
        public string SubTitle { set; get; }
        public string Description { set; get; }
        public bool IsInline { set; get; }
        public string ChartScript { set; get; }
        public string Height { set; get; }
        public string Width { set; get; }
        public bool BreakAfter { set; get; }
        public bool BreakBefore { set; get; }
    }

    public struct ChartScript
    {
        public const string LoadChartScript = "google.setOnLoadCallback(drawChart);";
        public const string DrawChartFunctionScript = "function drawChart() {{ {0} }}";
        public const string DataScript = "var data = google.visualization.arrayToDataTable([ {0} ]);";
        public const string OptionsScript = "var options = {{ {0} }};";
        public const string DrawChartScript = "chart.draw(data, options);";

        public const string CoreChartPackageScript = "google.load(\"visualization\", \"1\", {packages:[\"corechart\"]});";
        public const string TablePackageScript = "google.load(\"visualization\", \"1\", {packages:[\"table\"]});";

        public const string VisualizeColumnChartScript = "var chart = new google.visualization.ColumnChart(document.getElementById('{0}'));";
        public const string VisualizeAreaChartScript = "var chart = new google.visualization.AreaChart(document.getElementById('{0}'));";
        public const string VisualizeSteppedAreaChartScript = "var chart = new google.visualization.SteppedAreaChart(document.getElementById('{0}'));";
        public const string VisualizeBarChartScript = "var chart = new google.visualization.BarChart(document.getElementById('{0}'));";
        public const string VisualizeLineChartScript = "var chart = new google.visualization.LineChart(document.getElementById('{0}'));";
        public const string VisualizePieChartScript = "var chart = new google.visualization.PieChart(document.getElementById('{0}'));";
        public const string VisualizeTableScript = "var chart = new google.visualization.Table(document.getElementById('{0}'));";


        public const string ErrorScriptDiv = "<div style=\"background-color: #FFFFCC; border: medium solid #FF6600; padding: 5px\">{0}</div>";
        public const string ChartDiv = "<div id=\"{0}\" style=\"width: {1}; height: {2};\"></div>";
        public const string ChartDivWithText = "<div id=\"{0}\" style=\"padding: 5px; width: {1}; height: {2};\">{3}</div>";
        public const string NormalDiv = "<div>{0}</div>";
        public const string Script = "<script type=\"text/javascript\">{0}</script>";

        public const string ChartPanelDiv = "<div style=\"width: {0}; display:{1}\" class=\"chartPanel\">{2}</div>";
        public const string ChartTitleDiv = "<div align=\"center\" class=\"chartTitle\">{0}</div>";
        public const string ChartSubTitleDiv = "<div align=\"center\" class=\"chartSubTitle\">{0}</div>";
        public const string ChartDescriptionDiv = "<div align=\"center\" class=\"chartDesc\">{0}</div>";
        public const string Break = "<br/>";
    }
}
