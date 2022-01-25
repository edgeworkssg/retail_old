using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace GenericReport
{
    public class Table
    {
        public DataTable dt;
        public DataTable dtHeader;
        public string tblName;
        public List<double> listColumnWidth;
        public TableFormat tblFormat;
        public ElementPosition tablePosition;

        public Table(DataTable dt, DataTable header, string strName, List<double> ColumnWidth, TableFormat tblFormat, ElementPosition pos, CellFormat cellFormat)
        {
            this.dt = dt;
            dtHeader = header;
            tblName = strName;
            listColumnWidth = ColumnWidth;
            tablePosition = new ElementPosition();
            this.tblFormat = tblFormat;
            this.tablePosition.x = pos.x;
            this.tablePosition.y = pos.y;
        }

        public Table(DataTable dt, string strName, List<double> ColumnWidth, TableFormat tblFormat, ElementPosition pos, CellFormat cellFormat)
        {
            this.dt = dt;
            dtHeader = null;
            tblName = strName;
            listColumnWidth = ColumnWidth;
            tablePosition = new ElementPosition();
            this.tblFormat = tblFormat;
            this.tablePosition.x = pos.x;
            this.tablePosition.y = pos.y;
        }

        public Table()
        {
            dt = null;
            dtHeader = null;
            tblName = "";
            listColumnWidth = new List<double>();
            tblFormat = new TableFormat();
            tablePosition = new ElementPosition();
        }

        public DataTable DataTable
        {
            set
            {
                dt = value;
            }
            get
            {
                return dt;
            }
        }

        public void SetColumnFormat(List<ColumnFormat> columnFormat)
        {
            for (int i = 0; i < columnFormat.Count; i++)
                tblFormat.listColumnFormat.Add(columnFormat[i]);
        }
    }
}
