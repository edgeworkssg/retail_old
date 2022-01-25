using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using SubSonic;
using System.Collections;
namespace PowerPOS
{
    public partial class frmAppointmentManager : Form
    {
        Dictionary<string, int> cashierListIndex = new Dictionary<string, int>();
        Dictionary<string, int> cashierAppointmentCount = new Dictionary<string, int>();
        Dictionary<string, int> timeRowIndex = new Dictionary<string, int>();

        DataTable dt;
        DateTime selectedDate = DateTime.Now;
        public frmAppointmentManager()
        {
            InitializeComponent();
           // this.dataGridView1.Paint += new PaintEventHandler(dataGridView1_Click);
            //this.dataGridView1.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            //this.dataGridView1.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
            this.dataGridView1.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            this.dataGridView1.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;

            timeRowIndex.Add("06:00", 0);
            timeRowIndex.Add("06:15", 1);
            timeRowIndex.Add("06:30", 2);
            timeRowIndex.Add("06:45", 3);

            timeRowIndex.Add("07:00", 4);
            timeRowIndex.Add("07:15", 5);
            timeRowIndex.Add("07:30", 6);
            timeRowIndex.Add("07:45", 7);

            timeRowIndex.Add("08:00", 8);
            timeRowIndex.Add("08:15", 9);
            timeRowIndex.Add("08:30", 10);
            timeRowIndex.Add("08:45", 11);

            timeRowIndex.Add("09:00", 12);
            timeRowIndex.Add("09:15", 13);
            timeRowIndex.Add("09:30", 14);
            timeRowIndex.Add("09:45", 15);

            timeRowIndex.Add("10:00", 16);
            timeRowIndex.Add("10:15", 17);
            timeRowIndex.Add("10:30", 18);
            timeRowIndex.Add("10:45", 19);

            timeRowIndex.Add("11:00", 20);
            timeRowIndex.Add("11:15", 21);
            timeRowIndex.Add("11:30", 22);
            timeRowIndex.Add("11:45", 23);

            timeRowIndex.Add("12:00", 24);
            timeRowIndex.Add("12:15", 25);
            timeRowIndex.Add("12:30", 26);
            timeRowIndex.Add("12:45", 27);

            timeRowIndex.Add("13:00", 24);
            timeRowIndex.Add("13:15", 25);
            timeRowIndex.Add("13:30", 26);
            timeRowIndex.Add("13:45", 27);

            timeRowIndex.Add("14:00", 24);
            timeRowIndex.Add("14:15", 25);
            timeRowIndex.Add("14:30", 26);
            timeRowIndex.Add("14:45", 27);

            timeRowIndex.Add("15:00", 24);
            timeRowIndex.Add("15:15", 25);
            timeRowIndex.Add("15:30", 26);
            timeRowIndex.Add("15:45", 27);

            timeRowIndex.Add("16:00", 24);
            timeRowIndex.Add("16:15", 25);
            timeRowIndex.Add("16:30", 26);
            timeRowIndex.Add("16:45", 27);

            timeRowIndex.Add("17:00", 24);
            timeRowIndex.Add("17:15", 25);
            timeRowIndex.Add("17:30", 26);
            timeRowIndex.Add("17:45", 27);

            timeRowIndex.Add("18:00", 24);
            timeRowIndex.Add("18:15", 25);
            timeRowIndex.Add("18:30", 26);
            timeRowIndex.Add("18:45", 27);

            timeRowIndex.Add("19:00", 24);
            timeRowIndex.Add("19:15", 25);
            timeRowIndex.Add("19:30", 26);
            timeRowIndex.Add("19:45", 27);

            timeRowIndex.Add("20:00", 24);
            timeRowIndex.Add("20:15", 25);
            timeRowIndex.Add("20:30", 26);
            timeRowIndex.Add("20:45", 27);

            timeRowIndex.Add("21:00", 24);
            timeRowIndex.Add("21:15", 25);
            timeRowIndex.Add("21:30", 26);
            timeRowIndex.Add("21:45", 27);

            timeRowIndex.Add("22:00", 24);
            timeRowIndex.Add("22:15", 25);
            timeRowIndex.Add("22:30", 26);
            timeRowIndex.Add("22:45", 27);

            timeRowIndex.Add("23:00", 24);
            timeRowIndex.Add("23:15", 25);
            timeRowIndex.Add("23:30", 26);
            timeRowIndex.Add("23:45", 27);

            CreateAppointmentTable();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            lblDate.Text = DateTime.Now.ToLongDateString();
            tlbl.Text = "Today is " + DateTime.Now.ToLongDateString();


            GetSalesPerson();
            GetAppointments(DateTime.Now);

            monthCalendar2.SelectionStart = DateTime.Now.AddMonths(1);
            monthCalendar1.SelectionStart = DateTime.Now;

            monthCalendar1.MinDate = DateTime.Now.AddMonths(-3);
            monthCalendar2.MinDate = DateTime.Now;
        }

        private void GetSalesPerson()
        {
            cashierAppointmentCount = new Dictionary<string, int>();
            cashierListIndex = new Dictionary<string, int>();

            UserMstCollection salesPersons = new UserMstCollection();
            salesPersons.Where(UserMst.Columns.IsASalesPerson, true);
            salesPersons.OrderByAsc(UserMst.Columns.UserName).Load();

            if (salesPersons.Count > 10)
                dataGridView1.AutoSizeColumnsMode = dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            else
                dataGridView1.AutoSizeColumnsMode = dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            for (int i = 0; i < salesPersons.Count; i++)
            {
                DataGridViewColumn column = new DataGridViewColumn();
                column.Tag = salesPersons[i].UserName;
                column.HeaderText = salesPersons[i].DisplayName;
                column.CellTemplate = new DataGridViewTextBoxCell();
                column.HeaderCell.Style.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
                dataGridView1.Columns.Add(column);

                cashierListIndex.Add(salesPersons[i].UserName, i);
                cashierAppointmentCount.Add(salesPersons[i].UserName, 0);
            }

            dataGridView1.RowHeadersWidth = 80;
            for (int i = 0; i < 72; i++)
                dataGridView1.Rows.Add();
        }
        private void ClearCells()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for(int j=0; j< dataGridView1.Columns.Count; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = "";
                    dataGridView1.Rows[i].Cells[j].Tag = null;
                    dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.White;
                }
               
            }
        }
        private void GetAppointments(DateTime date)
        {
            try
            {
                // dataGridView1.Columns.Clear();
                //dataGridView1
                ClearCells();

                //b.Subtract(a).TotalMinutes

                List<string> keys = new List<string>();
                foreach (string key in cashierAppointmentCount.Keys)
                {
                    keys.Add(key);
                }
                foreach (string key in keys)
                {
                    cashierAppointmentCount[key] = 0;
                }

                AppointmentManagerCollection apps = new AppointmentManagerCollection();
                dt = apps.Where(AppointmentManager.Columns.AppointmentDate, date).Load().OrderByAsc(AppointmentManager.Columns.SalesPersonID).ToDataTable();

                dt.Columns.Add("Cells");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DateTime st = DateTime.Parse(dt.Rows[i]["StartTime"].ToString());
                    DateTime en = DateTime.Parse(dt.Rows[i]["EndTime"].ToString());
                    dt.Rows[i]["Cells"] = (en.Subtract(st).TotalMinutes / 15).ToString();
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                 
                    string salesPerson = dt.Rows[i]["SalesPersonID"].ToString();
                    int columnIndex = cashierListIndex[salesPerson];

                    cashierAppointmentCount[salesPerson] += 1;
                    try
                    {
                        int start = timeRowIndex[dt.Rows[i]["StartTime"].ToString()];
                        int end = timeRowIndex[dt.Rows[i]["EndTime"].ToString()];
               
                        int indexColor = cashierAppointmentCount[salesPerson];
                        PostAppointment(columnIndex, start, end, dt.Rows[i]["Description"].ToString(), int.Parse(dt.Rows[i]["AppointmentID"].ToString()), GetColor(indexColor));
                    }
                    catch
                    {
                    }

                }
                dataGridView1.ClearSelection();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private Color GetColor(int index)
        {
            switch (index)
            { 
                case 1:
                    return Color.LightSkyBlue;
                   // break;
                case 2:
                    return Color.LightGreen;
                    //break;
                case 3:
                    return Color.LightGray;
                    //break;
                case 4:
                    return Color.LightBlue;
                    //break;
                case 5:
                    return Color.LightCyan ;
                    //break;
                case 6:
                    return Color.LightSalmon;
                    //break;
                case 7:
                    return Color.LightCoral;
                    //break;
                case 8:
                    //break;
                case 9:
                    //break;
                case 10:
                    //break;
                case 11:
                    //break;
                case 12:
                    //break;
                default:
                    return Color.LightPink;
                    //break;
            }
        }
        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            selectedDate = monthCalendar1.SelectionStart;
            lblDate.Text = selectedDate.ToLongDateString();
            GetAppointments(selectedDate);
        }

        private void monthCalendar2_DateChanged(object sender, DateRangeEventArgs e)
        {
            selectedDate = monthCalendar2.SelectionStart;
            lblDate.Text  = selectedDate.ToLongDateString();
            GetAppointments(selectedDate);
        }
 
        //private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        //{
        //    if (e.RowIndex >= 0)
        //    {
               
        //    }
        //   //minIndex = 0;
        //    //Font fnt = new Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point);
        //    //Rectangle rct1 = new Rectangle((dataGridView1.GetColumnDisplayRectangle(0, true).X), (dataGridView1.GetColumnDisplayRectangle(0, true).Y + dataGridView1.Columns[0].HeaderCell.ContentBounds.Height + 8), dataGridView1.GetColumnDisplayRectangle(0, true).Width - 1, (dataGridView1.GetRowDisplayRectangle((dataGridView1.Rows.Count - 1), true).Top - dataGridView1.GetRowDisplayRectangle((dataGridView1.Rows.Count - 1), true).Height));

        //    //// Create string to draw.
        //    //String drawString = "Appointment 1";
        //    //// Create font and brush.
        //    //Font drawFont = new Font("Arial", 14);
        //    //SolidBrush drawBrush = new SolidBrush(Color.Black);
        //    //// Create point for upper-left corner of drawing.
        //    //float x = 100.0F;
        //    //float y = 50.0F;
        //    //// Set format of string.
        //    //StringFormat drawFormat = new StringFormat();
        //    //drawFormat.FormatFlags = StringFormatFlags.NoWrap;
        //    //// Draw string to screen.

        //    //e.Graphics.FillRectangle(Brushes.Gray, rct1);
        //    //e.PaintCells(rct1, DataGridViewPaintParts.Background);
        //    ////e.Graphics.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);

        //    //Pen pen = new Pen(drawBrush);
        //    ////Rectangle rect = new Rectangle(,);

        //    //e.Graphics.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);
        //    //e.Graphics.DrawRectangle(pen, rct1);
        //    //Rectangle rct =new Rectangle();
        //    //rct = dataGridView1.GetRowDisplayRectangle(3, true);
        //    //rct.Height -= 1;
        //    //SizeF s = new SizeF();
        //    //s = e.Graphics.MeasureString("HORINZONTAL TEXT", dataGridView1.Font);
        //    //float lefts = (rct.Width / 2) - (s.Width / 2);
        //    //float tops = rct.Top + ((rct.Height / 2) - (s.Height / 2));
        //    //e.Graphics.FillRectangle(Brushes.White, rct);
        //    //e.Graphics.DrawString("HORINZONTAL TEXT", fnt, Brushes.Black, 2, tops);

        //}

        private void button1_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedCellCollection cells = dataGridView1.SelectedCells;
            foreach (DataGridViewCell cell in cells)
            {
                cell.Style.BackColor = Color.Red;
            }
        }
        //private void Paint()
        //{
        //    //Graphics e =
        //    //e.draw
        //}
        //private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        //{
        //    if (e.RowIndex>= 0 && e.ColumnIndex >=0 )
        //    {
        //        using (Brush gridBrush = new SolidBrush(this.dataGridView1.GridColor))
        //        {
        //            using (Brush backColorBrush = new SolidBrush(e.CellStyle.BackColor))
        //            {
        //                using (Pen gridLinePen = new Pen(gridBrush))
        //                {
        //                    // Clear cell 
                           
        //                    e.Graphics.FillRectangle(backColorBrush, e.CellBounds);
        //                    //Bottom line drawing
        //                   // e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Bottom, e.CellBounds.Bottom - 1);
        //                    //x1, y1, x2,y2
        //                    //e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Top  - 1, e.CellBounds.Bottom, e.CellBounds.Top  - 1);

        //                    Brush brush = new SolidBrush(Color.Blue);
                            
        //                    Pen pen = new Pen(brush);

        //                    e.Graphics.DrawRectangle(pen, e.CellBounds);
        //                   // e.Graphics.DrawString("fsdf", this.Font, backColorBrush, new Point(e.CellBounds.Left, e.CellBounds.Top));
        //                    // here you force paint of content
        //                    e.PaintContent(e.ClipBounds);
        //                    e.Handled = true;
        //                }
        //            }
        //        }
        //    }
        //}
        frmAddAppointment add;
        private void button2_Click(object sender, EventArgs e)
        {
           
            if(add == null || add.IsDisposed)
            {
                add = new frmAddAppointment(selectedDate);
            }
            add.ShowDialog();

            GetAppointments(selectedDate);
        }

        private void dataGridView1_Paint(object sender, PaintEventArgs e)
        {

            
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            GetAppointments(selectedDate);
        }

        private void PostAppointment(int colIndex, int startIndex, int endIndex, string Description, int appId, Color backColor)
        {
            dataGridView1.Rows[startIndex].Cells[colIndex].Value = Description;
            //dataGridView1.Rows[startIndex].Cells[colIndex].Style.Alignment = DataGridViewContentAlignment.TopCenter;

            int cellCount = endIndex - startIndex;
            if (cellCount==0)
            {
                dataGridView1.Rows[startIndex].Cells[colIndex].Value = Description;
                dataGridView1.Rows[startIndex].Cells[colIndex].Tag = appId;
                dataGridView1.Rows[startIndex].Cells[colIndex].Style.BackColor = backColor;
                return;
            }

            string[] desc = Description.Split(' ');

            StringBuilder sb = new StringBuilder();
            List<string> parts = new List<string>();

            foreach (string str in desc)
            {
                sb.Append(str + " ");
                if (sb.Length > 20)
                {
                    parts.Add(sb.ToString().Trim());
                    sb = new StringBuilder();
                }
            }
            int currentCellIndex = 1;
            for (int i = startIndex; i < endIndex; i++)
            {
                dataGridView1.Rows[i].Cells[colIndex].Tag = appId;
                dataGridView1.Rows[i].Cells[colIndex].Style.BackColor = backColor;

                if (currentCellIndex <= cellCount && currentCellIndex <= parts.Count)
                {
                    dataGridView1.Rows[i].Cells[colIndex].Value = parts[currentCellIndex - 1];
                    currentCellIndex++;
                }
            }
        }

        string[] time = {   "6AM", 
                            "6:15", 
                            "6:30", 
                            "6:45", 
                            "7AM", 
                            "7:15", 
                            "7:30", 
                            "7:45", 
                            "8AM", 
                            "8:15", 
                            "8:30", 
                            "8:45",
                            "9AM", 
                            "9:15", 
                            "9:30", 
                            "9:45",
                            "10AM",
                            "10:15", 
                            "10:30", 
                            "10:45",
                            "11AM", 
                            "11:15", 
                            "11:30", 
                            "11:45",
                            "12 PM", 
                            "12:15", 
                            "12:30", 
                            "12:45",
                            "1 PM", 
                            "1:15", 
                            "1:30", 
                            "1:45",
                            "2 PM", 
                            "2:15", 
                            "2:30", 
                            "2:45",
                            "3 PM", 
                            "3:15", 
                            "3:30", 
                            "3:45",
                            "4 PM", 
                            "4:15", 
                            "4:30", 
                            "4:45",
                            "5 PM", 
                            "5:15", 
                            "5:30", 
                            "5:45",
                            "6 PM", 
                            "6:15", 
                            "6:30", 
                            "6:45",
                            "7 PM", 
                            "7:15", 
                            "7:30", 
                            "7:45",
                            "8 PM", 
                            "8:15", 
                            "8:30", 
                            "8:45",
                            "9 PM", 
                            "9:15", 
                            "9:30", 
                            "9:45",
                            "10 PM", 
                            "10:15", 
                            "10:30", 
                            "10:45",
                            "11 PM", 
                            "11:15", 
                            "11:30", 
                            "11:45",
                            //"", 
                            //"", 
                            //"", 
                            //"", 
                            //"", 
                            //"", 
                            //"" 
                            };


        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {
                //timeRowIndex = new Dictionary<string, int>();

                dataGridView1.Rows[e.RowIndex].Height = 30;

                var grid = sender as DataGridView;
                Font font = new Font(this.Font.FontFamily, 13, FontStyle.Bold);
                var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);

                var centerFormat = new StringFormat()
                {
                    // right alignment might actually make more sense for numbers asdf asdf
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center

                };

                if (time[e.RowIndex].Contains("AM") || time[e.RowIndex].Contains("PM"))
                {
                    e.Graphics.DrawString(time[e.RowIndex], font, Brushes.Red, headerBounds, centerFormat);
                    headerBounds = new Rectangle(e.RowBounds.Left + 10, e.RowBounds.Top+2, grid.RowHeadersWidth, e.RowBounds.Height + 10);
                    e.Graphics.DrawString("__", font, SystemBrushes.ControlText, headerBounds, centerFormat);

                   // timeRowIndex.Add(time[e.RowIndex], e.RowIndex);
                }
                else
                {

                    headerBounds = new Rectangle(e.RowBounds.Left + 10, e.RowBounds.Top+2, grid.RowHeadersWidth, e.RowBounds.Height + 10);
                    e.Graphics.DrawString("__", font, SystemBrushes.ControlText, headerBounds, centerFormat);

                    //timeRowIndex.Add(time[e.RowIndex], e.RowIndex);
                    //e.Graphics.DrawString("__", font, SystemBrushes.ControlText, headerBounds, centerFormat);
                }



            }
            catch
            { }



            

            

            //string[] mins = { "15", "30", "45" };

            //switch (rowIdx)
            //{
            //    case 1:
            //        e.Graphics.DrawString("6 AM", font, Brushes.Red, headerBounds, centerFormat);
            //        headerBounds = new Rectangle(e.RowBounds.Left + 10, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height + 10);
            //        e.Graphics.DrawString("__", font, SystemBrushes.ControlText, headerBounds, centerFormat);
            //        minIndex = 0;
            //        break;
            //    case 5:
            //        e.Graphics.DrawString("7 AM", font, Brushes.Red, headerBounds, centerFormat);
            //        headerBounds = new Rectangle(e.RowBounds.Left + 10, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height + 10);
            //        e.Graphics.DrawString("__", font, SystemBrushes.ControlText, headerBounds, centerFormat);
            //        minIndex = 0;
            //        break;
            //    case 9:
            //        e.Graphics.DrawString("8 AM", font, Brushes.Red, headerBounds, centerFormat);
            //        headerBounds = new Rectangle(e.RowBounds.Left + 10, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height + 10);
            //        e.Graphics.DrawString("__", font, SystemBrushes.ControlText, headerBounds, centerFormat);
            //        minIndex = 0;
            //        break;
            //    case 13:
            //        e.Graphics.DrawString("9 AM", font, Brushes.Red, headerBounds, centerFormat);
            //        headerBounds = new Rectangle(e.RowBounds.Left + 10, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height + 10);
            //        e.Graphics.DrawString("__", font, SystemBrushes.ControlText, headerBounds, centerFormat);
            //        minIndex = 0;
            //        break;
            //    case 17:
            //        e.Graphics.DrawString("10 AM", font, Brushes.Red, headerBounds, centerFormat);
            //        headerBounds = new Rectangle(e.RowBounds.Left + 10, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height + 10);
            //        e.Graphics.DrawString("__", font, SystemBrushes.ControlText, headerBounds, centerFormat);
            //        minIndex = 0;
            //        break;
            //    case 21:
            //        e.Graphics.DrawString("11 AM", font, Brushes.Red, headerBounds, centerFormat);
            //        headerBounds = new Rectangle(e.RowBounds.Left + 10, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height + 10);
            //        e.Graphics.DrawString("__", font, SystemBrushes.ControlText, headerBounds, centerFormat);
            //        minIndex = 0;
            //        break;
            //    case 25:
            //        e.Graphics.DrawString("12 PM", font, Brushes.Red, headerBounds, centerFormat);
            //        headerBounds = new Rectangle(e.RowBounds.Left + 10, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height + 10);
            //        e.Graphics.DrawString("__", font, SystemBrushes.ControlText, headerBounds, centerFormat);
            //        minIndex = 0;
            //        break;
            //    default:
            //        try
            //        {
            //            e.Graphics.DrawString("    " + mins[minIndex], font, Brushes.Black, headerBounds, centerFormat);
            //            headerBounds = new Rectangle(e.RowBounds.Left + 2, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height + 10);
            //            //e.Graphics.DrawString("__", font, SystemBrushes.ControlText, headerBounds, centerFormat);
            //            minIndex++;
            //            if (rowIdx >= 25 && minIndex==2)
            //            {
            //                done = true;
            //                return;
            //            }
            //        }
            //        catch { }
            //        break;
            //}
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                object appID = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag;
                if (appID != null)
                {
                    AppointmentManager app = new AppointmentManager(appID);
                    if (app.AppointmentID != 0)
                    {
                        int columnIndex = cashierListIndex[app.SalesPersonID];
                        int start = timeRowIndex[app.StartTime];
                        int end = timeRowIndex[app.EndTime];

                        for (int i = start; i < end; i++)
                        {
                            dataGridView1.Rows[i].Cells[columnIndex].Selected = true;
                            //dataGridView1.Rows[end].Cells[columnIndex].Selected = true;
                        }
                    }
                    //dataGridView1

                

                //object appID = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag;
                frmViewAppointment view = new frmViewAppointment(appID);
                view.StartPosition = FormStartPosition.CenterParent;
                view.ShowDialog();
                view.Dispose();
                }
            }
            catch 
            {
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                object appID = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag;
                if (appID != null)
                {
                    AppointmentManager app = new AppointmentManager(appID);
                    if (app.AppointmentID != 0)
                    {
                        int columnIndex = cashierListIndex[app.SalesPersonID];
                        int start = timeRowIndex[app.StartTime];
                        int end = timeRowIndex[app.EndTime];

                        for (int i = start; i < end; i++)
                        {
                            dataGridView1.Rows[i].Cells[columnIndex].Selected = true;
                            //dataGridView1.Rows[end].Cells[columnIndex].Selected = true;
                        }
                    }
                    //dataGridView1

                }
            }
            catch { }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GetAppointments(selectedDate);
        }

        private void CreateAppointmentTable()
        {
            try
            {
                string SQL = @" 
                IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AppointmentManager]') AND type in (N'U'))
                BEGIN
                    CREATE TABLE [dbo].[AppointmentManager](
	                    [AppointmentID] [int] IDENTITY(1,1) NOT NULL,
	                    [AppointmentDate] [date] NULL,
	                    [StartTime] [varchar](20) NULL,
	                    [EndTime] [varchar](20) NULL,
	                    [Description] [varchar](500) NULL,
	                    [SalesPersonID] [varchar](100) NULL,
                     CONSTRAINT [PK_AppointmentManager] PRIMARY KEY CLUSTERED 
                    (
	                    [AppointmentID] ASC
                    )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
                    ) ON [PRIMARY]
                END
                    ";

                SubSonic.DataService.ExecuteQuery(new QueryCommand(SQL));
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.ToString());
            }

        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }







    }
}
