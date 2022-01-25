using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SubSonic;

namespace PowerPOS
{
    public partial class ChartController
    {
        public class OutletData
        {
            public string OutletName;
            public DateTime XCaption;
            public decimal Value;
        }

        public static string CreateLineChart(string OutletName, string[] Values, string[] XCaptions, string[] Colors)
        {
            /// calon Refactor Parameter
            string Title = "Collection Report";
            string _Values, _XCaptions, _Colors;

            Random random = new Random();

            //if (dt.Rows.Count > 0)
            //{
                _Values = string.Join(",", Values);
                _XCaptions = string.Join("|", XCaptions);
                _Colors = string.Join("|", Colors);

                //_Values = Uri.EscapeUriString(_Values.Substring(0, _Values.Length - 1));
                //_XCaptions = Uri.EscapeUriString(_XCaptions.Substring(0, _XCaptions.Length - 1));
                //_Colors = Uri.EscapeUriString(_Colors.Substring(0, _Colors.Length - 1));

                decimal MaxValue, MinValue;
                MaxValue = Values.Max(Fnc => decimal.Parse(Fnc));
                MinValue = Values.Min(Fnc => decimal.Parse(Fnc));

                MinValue = (decimal.Floor(MinValue / 1000) - 1) * 1000;
                MaxValue = (decimal.Ceiling(MaxValue / 1000) + 1) * 1000;

                List<string> YCaptions = new List<string>();
                for (int Counter = (int)MinValue / 1000; Counter <= (int)MaxValue / 1000; Counter++)
                {
                    YCaptions.Add("$" + Counter.ToString() + ((Counter == 0) ? "" : "000"));
                }


                return "http://chart.apis.google.com/chart" +
                    "?chxl=0:|" + _XCaptions +                                          //AxisLabel(<axis_index>:|<label_1>|...)
                        "|1:|" + string.Join("|", YCaptions.ToArray()) +
                        "|2:|" + string.Join("|", YCaptions.ToArray()) +
                    "&chxr=0,-5,100" +                                                  //AxisIndex(<axis_index>,<start_val>,<end_val>,<step>|...)
                    "&chxs=0,00AA00,14,0.5,l,676767" +                                  //AxisLabelStyles(<axis_index><optional_format_string>,<label_color>,<font_size>,<alignment>,<axis_or_tick>,<tick_color>|...)
                    "&chxt=x,r,y" +                                                     //VisibleAxis(<axis_1>,...)
                    "&chs=1000x300" +                                                   //ChartSize(<width>x<height>)
                    "&cht=lc" +                                                         //ChartType
                    "&chco=FF0000" +                                                    //SeriesColors(<series_1_element_1>|...|<series_1_element_n>,   <series_2>,...,<series_m>)
                    "&chds=" + MinValue + "," + MaxValue +                              //ScaleForTextFormatWithCustomRange(<series_1_min>,<series_1_max>,...
                    "&chd=t:" + _Values +                                               //Values
                    "&chdl=" +                                                          //ChartLegendTextAndStyle
                        OutletName.Replace(" ", "+") +
                    "&chdlp=b" +
                    "&chg=20,25" +                                                      //GridLines(<x_axis_step_size>,<y_axis_step_size>,<dash_length>,<space_length>,<x_offset>,<y_offset>)
                    "&chls=2";                                                          //LineStyles(<line_1_thickness>,<dash_length>,<space_length>|...)
            //}
            //else
            //{
            //    return "";
            //}
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="OutletName"></param>
        /// <param name="Values"></param>
        /// <param name="XCaptions"></param>
        /// <param name="Colors"></param>
        /// <returns></returns>
        /// <remarks>Not Finished</remarks>
        public static string CreateLineChart(string[] OutletName, string[] Values, string XCaptions, string[] Colors)
        {
            /// calon Refactor Parameter
            string Title = "Collection Report";

            List<string> _Values = new List<string>();
            //Random random = new Random();

            int MaxCounter = Math.Min(Math.Min(OutletName.Length, Values.Length), Math.Min(XCaptions.Length, Colors.Length));

            List<string> YCaptions = new List<string>();
            
            for (int Counter = 0; Counter < MaxCounter; Counter++)
            {
                _Values.Add("-1|" + Values[Counter]);
                //_Colors.Add(Colors[Counter]);
            }

            decimal MaxValue, MinValue;
            MaxValue = Values.Max(Fnc => Fnc.Split(',').Max(Fnc2 => decimal.Parse(Fnc2)));
            MinValue = Values.Min(Fnc => Fnc.Split(',').Min(Fnc2 => decimal.Parse(Fnc2))); 

            MinValue = (decimal.Floor(MinValue / 1000) - 1) * 1000;
            MaxValue = (decimal.Ceiling(MaxValue / 1000) + 1) * 1000;

            for (int Counter = (int)MinValue / 1000; Counter <= (int)MaxValue / 1000; Counter++)
            {
                YCaptions.Add("$" + Counter.ToString() + ((Counter == 0) ? "" : "000"));
            }


            return "http://chart.apis.google.com/chart" +
                "?chxl=0:|" + XCaptions +                                          //AxisLabel(<axis_index>:|<label_1>|...)
                    "|1:|" + string.Join("|", YCaptions.ToArray()) +
                    "|2:|" + string.Join("|", YCaptions.ToArray()) +
                "&chxr=0,-5,100" +                                                  //AxisIndex(<axis_index>,<start_val>,<end_val>,<step>|...)
                "&chxs=0,00AA00,14,0.5,l,676767" +                                  //AxisLabelStyles(<axis_index><optional_format_string>,<label_color>,<font_size>,<alignment>,<axis_or_tick>,<tick_color>|...)
                "&chxt=x,r,y" +                                                     //VisibleAxis(<axis_1>,...)
                "&chs=1000x300" +                                                   //ChartSize(<width>x<height>)
                "&cht=lc" +                                                         //ChartType
                "&chco=" + string.Join(",", Colors) +                               //SeriesColors(<series_1_element_1>|...|<series_1_element_n>,   <series_2>,...,<series_m>)
                "&chds=" + MinValue + "," + MaxValue +                              //ScaleForTextFormatWithCustomRange(<series_1_min>,<series_1_max>,...
                "&chd=t:" + string.Join("|", _Values.ToArray()) +                   //Values
                "&chdl=" +                                                          //ChartLegendTextAndStyle
                    OutletName[0].Replace(" ", "+") +
                "&chdlp=b" +
                "&chg=20,25" +                                                      //GridLines(<x_axis_step_size>,<y_axis_step_size>,<dash_length>,<space_length>,<x_offset>,<y_offset>)
                "&chls=2";                                                          //LineStyles(<line_1_thickness>,<dash_length>,<space_length>|...)
            //}
            //else
            //{
            //    return "";
            //}
        }
        public static string CreateLineChart(OutletData[] Values, string[] Colors)
        {
            return "";
            ///// calon Refactor Parameter
            //string Title = "Collection Report";





            ////List<string> _Values = new List<string>();
            //////Random random = new Random();

            //int MaxCounter = Math.Min(Math.Min(OutletName.Length, Values.Length), Math.Min(XCaptions.Length, Colors.Length));

            //List<string> YCaptions = new List<string>();

            //for (int Counter = 0; Counter < MaxCounter; Counter++)
            //{
            //    _Values.Add("-1|" + Values[Counter]);
            //    //_Colors.Add(Colors[Counter]);
            //}

            //decimal MaxValue, MinValue;
            //MaxValue = Values.Max(Fnc => Fnc.Split(',').Max(Fnc2 => decimal.Parse(Fnc2)));
            //MinValue = Values.Min(Fnc => Fnc.Split(',').Min(Fnc2 => decimal.Parse(Fnc2)));

            //MinValue = (decimal.Floor(MinValue / 1000) - 1) * 1000;
            //MaxValue = (decimal.Ceiling(MaxValue / 1000) + 1) * 1000;

            //for (int Counter = (int)MinValue / 1000; Counter <= (int)MaxValue / 1000; Counter++)
            //{
            //    YCaptions.Add("$" + Counter.ToString() + ((Counter == 0) ? "" : "000"));
            //}


            //return "http://chart.apis.google.com/chart" +
            //    "?chxl=0:|" + XCaptions +                                          //AxisLabel(<axis_index>:|<label_1>|...)
            //        "|1:|" + string.Join("|", YCaptions.ToArray()) +
            //        "|2:|" + string.Join("|", YCaptions.ToArray()) +
            //    "&chxr=0,-5,100" +                                                  //AxisIndex(<axis_index>,<start_val>,<end_val>,<step>|...)
            //    "&chxs=0,00AA00,14,0.5,l,676767" +                                  //AxisLabelStyles(<axis_index><optional_format_string>,<label_color>,<font_size>,<alignment>,<axis_or_tick>,<tick_color>|...)
            //    "&chxt=x,r,y" +                                                     //VisibleAxis(<axis_1>,...)
            //    "&chs=1000x300" +                                                   //ChartSize(<width>x<height>)
            //    "&cht=lc" +                                                         //ChartType
            //    "&chco=" + string.Join(",", Colors) +                               //SeriesColors(<series_1_element_1>|...|<series_1_element_n>,   <series_2>,...,<series_m>)
            //    "&chds=" + MinValue + "," + MaxValue +                              //ScaleForTextFormatWithCustomRange(<series_1_min>,<series_1_max>,...
            //    "&chd=t:" + string.Join("|", _Values.ToArray()) +                   //Values
            //    "&chdl=" +                                                          //ChartLegendTextAndStyle
            //        OutletName[0].Replace(" ", "+") +
            //    "&chdlp=b" +
            //    "&chg=20,25" +                                                      //GridLines(<x_axis_step_size>,<y_axis_step_size>,<dash_length>,<space_length>,<x_offset>,<y_offset>)
            //    "&chls=2";                                                          //LineStyles(<line_1_thickness>,<dash_length>,<space_length>|...)
            ////}
            ////else
            ////{
            ////    return "";
            ////}
        }

        public static string CreateLineChart(string OutletName, string[] Values, DateTime[] XCaptions, string[] Colors)
        {
            /// calon Refactor Parameter
            string Title = "Collection Report";
            string _Values, _XCaptions, _Colors;

            Random random = new Random();

                _Values = string.Join(",", Values);
                _XCaptions = "";// string.Join("|", XCaptions);
                _Colors = string.Join("|", Colors);

                decimal MaxValue, MinValue;
                MaxValue = Values.Max(Fnc => decimal.Parse(Fnc));
                MinValue = Values.Min(Fnc => decimal.Parse(Fnc));

                MinValue = (decimal.Floor(MinValue / 1000) - 1) * 1000;
                MaxValue = (decimal.Ceiling(MaxValue / 1000) + 1) * 1000;

            List<string> YCaptions = new List<string>();
            for (int Counter = (int)MinValue / 1000; Counter <= (int)MaxValue / 1000; Counter++)
            {
                YCaptions.Add("$" + Counter.ToString() + ((Counter == 0) ? "" : "000"));
            }


            return "http://chart.apis.google.com/chart" +
                "?chxl=0:|" + _XCaptions +                                          //AxisLabel(<axis_index>:|<label_1>|...)
                    "|1:|" + string.Join("|", YCaptions.ToArray()) +
                    "|2:|" + string.Join("|", YCaptions.ToArray()) +
                "&chxr=0,-5,100" +                                                  //AxisIndex(<axis_index>,<start_val>,<end_val>,<step>|...)
                "&chxs=0,00AA00,14,0.5,l,676767" +                                  //AxisLabelStyles(<axis_index><optional_format_string>,<label_color>,<font_size>,<alignment>,<axis_or_tick>,<tick_color>|...)
                "&chxt=x,r,y" +                                                     //VisibleAxis(<axis_1>,...)
                "&chs=1000x300" +                                                   //ChartSize(<width>x<height>)
                "&cht=lc" +                                                         //ChartType
                "&chco=FF0000" +                                                    //SeriesColors(<series_1_element_1>|...|<series_1_element_n>,   <series_2>,...,<series_m>)
                "&chds=" + MinValue + "," + MaxValue +                              //ScaleForTextFormatWithCustomRange(<series_1_min>,<series_1_max>,...
                "&chd=t:" + _Values +                                               //Values
                "&chdl=" +                                                          //ChartLegendTextAndStyle
                    OutletName.Replace(" ", "+") +
                "&chdlp=b" +
                "&chg=20,25" +                                                      //GridLines(<x_axis_step_size>,<y_axis_step_size>,<dash_length>,<space_length>,<x_offset>,<y_offset>)
                "&chls=2";                                                          //LineStyles(<line_1_thickness>,<dash_length>,<space_length>|...)
            //}
            //else
            //{
            //    return "";
            //}
        }

        #region For Multi Outlet Chart
        //public static string CreateLineChart(string OutletName, string[] Values, string[] XCaptions, string[] Colors)
        //{
        //    /// calon Refactor Parameter
        //    string Title = "Collection Report";
        //    string _Values, _XCaptions, _Colors;

        //    Random random = new Random();

        //    //if (dt.Rows.Count > 0)
        //    //{
        //    _Values = string.Join(",", Values);
        //    _XCaptions = string.Join("|", XCaptions);
        //    _Colors = string.Join("|", Colors);

        //    //_Values = Uri.EscapeUriString(_Values.Substring(0, _Values.Length - 1));
        //    //_XCaptions = Uri.EscapeUriString(_XCaptions.Substring(0, _XCaptions.Length - 1));
        //    //_Colors = Uri.EscapeUriString(_Colors.Substring(0, _Colors.Length - 1));

        //    return "http://chart.apis.google.com/chart" +
        //        "?chxl=0:|" + _XCaptions +                                          //AxisLabel(<axis_index>:|<label_1>|...)
        //            "|1:|0|25|50|75|100" +
        //            "|2:|0|25|50|75|100" +
        //        "&chxr=0,-5,100" +                                                  //AxisIndex(<axis_index>,<start_val>,<end_val>,<step>|...)
        //        "&chxs=0,00AA00,14,0.5,l,676767" +                                  //AxisLabelStyles(<axis_index><optional_format_string>,<label_color>,<font_size>,<alignment>,<axis_or_tick>,<tick_color>|...)
        //        "&chxt=x,r,y" +                                                     //VisibleAxis(<axis_1>,...)
        //        "&chs=1000x300" +                                                   //ChartSize(<width>x<height>)
        //        "&cht=lc" +                                                         //ChartType
        //        "&chco=FF0000,0000FF" +                                             //SeriesColors(<series_1_element_1>|...|<series_1_element_n>,   <series_2>,...,<series_m>)
        //        "&chds=0," + Values.Max(Fnc => Fnc.PadLeft(10, '0')) + ",0,100" +   //ScaleForTextFormatWithCustomRange(<series_1_min>,<series_1_max>,...
        //        "&chd=t:" + _Values +                                               //Values
        //            "|" + "44.322,48.923,55.781,74.307,75.224,77.318,78.624,76.854,70.191,63.309,67.995,75.112,65.172,59.024" +
        //        "&chdl=" +                                                           //ChartLegendTextAndStyle
        //            OutletName.Replace(" ", "+") +
        //            "|Object+2" +
        //        "&chdlp=b" +
        //        "&chg=20,25" +
        //        "&chls=2|2";
        //    //}
        //    //else
        //    //{
        //    //    return "";
        //    //}
        //}
        #endregion
    }
}
