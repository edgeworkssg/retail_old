using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;

namespace PowerPOS
{
    public partial class ExcelController
    {
        struct ColumnType
        {
            public Type type;
            private string name;
            public ColumnType(Type type) { this.type = type; this.name = type.ToString().ToLower(); }
            public object ParseString(string input)
            {
                if (String.IsNullOrEmpty(input))
                    return DBNull.Value;
                switch (type.ToString())
                {
                    case "system.datetime":
                        return DateTime.Parse(input);
                    case "system.decimal":
                        return decimal.Parse(input);
                    case "system.boolean":
                        return bool.Parse(input);
                    default:
                        return input;
                }
            }
        }
        public static DataSet ImportExcelXML(string filepath, bool hasHeaders, bool autoDetectColumnType)
        {
            if (!System.IO.File.Exists(filepath)) return null;

            XmlDocument doc = new XmlDocument();
            doc.Load(filepath);

            return ImportExcelXML(doc, hasHeaders, autoDetectColumnType);
        }
        public static DataSet ImportExcelXML(Stream inputFileStream, bool hasHeaders, bool autoDetectColumnType)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(new XmlTextReader(inputFileStream));

            return ImportExcelXML(doc, hasHeaders, autoDetectColumnType);
        }
        public static DataSet ImportExcelXML(XmlDocument doc, bool hasHeaders, bool autoDetectColumnType)
        {
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);

            nsmgr.AddNamespace("o", "urn:schemas-microsoft-com:office:office");
            nsmgr.AddNamespace("x", "urn:schemas-microsoft-com:office:excel");
            nsmgr.AddNamespace("ss", "urn:schemas-microsoft-com:office:spreadsheet");

            DataSet ds = new DataSet();

            foreach (XmlNode node in doc.DocumentElement.SelectNodes("//ss:Worksheet", nsmgr))
            {
                DataTable dt = new DataTable(node.Attributes["ss:Name"].Value);
                ds.Tables.Add(dt);
                XmlNodeList rows = node.SelectNodes("ss:Table/ss:Row", nsmgr);
                if (rows.Count > 0)
                {
                    List<ColumnType> columns = new List<ColumnType>();
                    int startIndex = 0;
                    if (hasHeaders)
                    {
                        foreach (XmlNode data in rows[0].SelectNodes("ss:Cell/ss:Data", nsmgr))
                        {
                            columns.Add(new ColumnType(typeof(string)));//default to text
                            dt.Columns.Add(data.InnerText, typeof(string));
                        }
                        startIndex++;
                    }
                    if (autoDetectColumnType && rows.Count > 0)
                    {
                        XmlNodeList cells = rows[startIndex].SelectNodes("ss:Cell", nsmgr);
                        int actualCellIndex = 0;
                        for (int cellIndex = 0; cellIndex < cells.Count; cellIndex++)
                        {
                            XmlNode cell = cells[cellIndex];
                            if (cell.Attributes["ss:Index"] != null)
                                actualCellIndex = int.Parse(cell.Attributes["ss:Index"].Value) - 1;

                            ColumnType autoDetectType = getType(cell.SelectSingleNode("ss:Data", nsmgr));

                            if (actualCellIndex >= dt.Columns.Count)
                            {
                                dt.Columns.Add("Column" + actualCellIndex.ToString(), autoDetectType.type);
                                columns.Add(autoDetectType);
                            }
                            else
                            {
                                dt.Columns[actualCellIndex].DataType = autoDetectType.type;
                                columns[actualCellIndex] = autoDetectType;
                            }

                            actualCellIndex++;
                        }
                    }
                    for (int i = startIndex; i < rows.Count; i++)
                    {
                        DataRow row = dt.NewRow();
                        XmlNodeList cells = rows[i].SelectNodes("ss:Cell", nsmgr);
                        int actualCellIndex = 0;
                        for (int cellIndex = 0; cellIndex < cells.Count; cellIndex++)
                        {
                            XmlNode cell = cells[cellIndex];
                            if (cell.Attributes["ss:Index"] != null)
                                actualCellIndex = int.Parse(cell.Attributes["ss:Index"].Value) - 1;

                            XmlNode data = cell.SelectSingleNode("ss:Data", nsmgr);

                            if (actualCellIndex >= dt.Columns.Count)
                            {
                                for (int i2 = dt.Columns.Count; i2 < actualCellIndex; i++)
                                {
                                    dt.Columns.Add("Column" + actualCellIndex.ToString(), typeof(string));
                                    columns.Add(getDefaultType());
                                }
                                ColumnType autoDetectType = getType(cell.SelectSingleNode("ss:Data", nsmgr));
                                dt.Columns.Add("Column" + actualCellIndex.ToString(), typeof(string));
                                columns.Add(autoDetectType);
                            }
                            if (data != null)
                                row[actualCellIndex] = data.InnerText;

                            actualCellIndex++;
                        }

                        dt.Rows.Add(row);
                    }
                }
            }
            return ds;

            //<?xml version="1.0"?>
            //<?mso-application progid="Excel.Sheet"?>
            //<Workbook>
            // <Worksheet ss:Name="Sheet1">
            //  <Table>
            //   <Row>
            //    <Cell><Data ss:Type="String">Item Number</Data></Cell>
            //    <Cell><Data ss:Type="String">Description</Data></Cell>
            //    <Cell ss:StyleID="s21"><Data ss:Type="String">Item Barcode</Data></Cell>
            //   </Row>
            // </Worksheet>
            //</Workbook>
        }

        private static ColumnType getDefaultType()
        {
            return new ColumnType(typeof(String));
        }

        private static ColumnType getType(XmlNode data)
        {
            string type = null;
            if (data.Attributes["ss:Type"] == null || data.Attributes["ss:Type"].Value == null)
                type = "";
            else
                type = data.Attributes["ss:Type"].Value;

            switch (type)
            {
                case "DateTime":
                    return new ColumnType(typeof(DateTime));
                case "Boolean":
                    return new ColumnType(typeof(Boolean));
                case "Number":
                    return new ColumnType(typeof(Decimal));
                case "":
                    decimal test2;
                    if (data == null || String.IsNullOrEmpty(data.InnerText) || decimal.TryParse(data.InnerText, out test2))
                    {
                        return new ColumnType(typeof(Decimal));
                    }
                    else
                    {
                        return new ColumnType(typeof(String));
                    }
                default://"String"
                    return new ColumnType(typeof(String));
            }
        }
    }
}
