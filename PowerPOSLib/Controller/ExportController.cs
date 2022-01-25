using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using Microsoft.Office.Tools.Excel;
using System.Windows.Forms;
using System.Collections;
using System.Data.OleDb;
using SubSonic;
using PowerPOS.Container;
using SpreadsheetLight;
using SpreadsheetLight.Drawing;
using DocumentFormat.OpenXml.Packaging;
using System.Drawing;


namespace PowerPOS
{
    public class ExportController
    {
        public static string INVENTORY_IMPORT_CONFIG = "ImportInventoryConfig.txt";
        public static bool ConvertDataGridViewToDataTable(DataGridView dgv, out DataTable dt, out ArrayList headerName)
        {
            dt = null;
            headerName = null;
            if (dgv == null || dgv.Rows.Count == 0)
            {
                return false;
            }
            dt = new DataTable();
            headerName = new ArrayList();
            for (int i = 0; i < dgv.ColumnCount; i++)
            {
                if (dgv.Columns[i] is DataGridViewTextBoxColumn && dgv.Columns[i].Visible)
                {
                    dt.Columns.Add(dgv.Columns[i].Name.Replace("\"", "''"));
                    headerName.Add(dgv.Columns[i].HeaderText);
                }
            }
            DataRow dr;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                dr = dt.NewRow();
                int j = 0, k = 0;
                while (j < dgv.Columns.Count && k < dt.Columns.Count)
                {
                    if (dgv.Columns[j] is DataGridViewTextBoxColumn && dgv.Columns[j].Visible)
                    {
                        if (dgv.Rows[i].Cells[j].Value != null)
                        {
                            dr[k] = dgv.Rows[i].Cells[j].Value.ToString().Replace("\"", "''");
                        }
                        else
                        {
                            dr[k] = "";
                        }
                        k++;
                    }
                    j++;
                }
                dt.Rows.Add(dr);
            }
            return true;
        }
        public static bool ExportToExcel(DataGridView dgv, string filepath)
        {
            if (dgv != null && dgv.Rows.Count > 0)
            {
                DataTable dt;
                ArrayList headerName;
                if (ConvertDataGridViewToDataTable(dgv, out dt, out headerName))
                {
                    ArrayList ColumnList = new ArrayList();
                    for (int m = 0; m < dt.Columns.Count; m++)
                    {
                        ColumnList.Add(m);
                    }
                    /*RKLib.ExportData.Export a = new RKLib.ExportData.Export("Win");
                    a.ExportDetails(dt, (int[])ColumnList.ToArray(typeof(int)),
                        (string[])headerName.ToArray(typeof(string)),
                        RKLib.ExportData.Export.ExportFormat.CSV, filepath);*/
                    ExportToCSV(dt, filepath);
                    return true;
                }
                return false;
            }
            return false;
        }

        public static bool ExportToExcelWithItemImageInventory(DataGridView dgv, string filepath)
        {
            SLDocument sl = new SLDocument();
            if (dgv != null && dgv.Rows.Count > 0)
            {
                try
                {
                    DataTable dt;
                    ArrayList headerName;
                    if (ConvertDataGridViewToDataTable(dgv, out dt, out headerName))
                    {
                        ArrayList ColumnList = new ArrayList();
                        for (int m = 0; m < dt.Columns.Count; m++)
                        {
                            ColumnList.Add(m);
                        }

                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.AddItemPicture), false))
                        {
                            dt.Columns.Add("Image", typeof(string));
                        }

                        int imageColumn = dt.Columns.Count;
                        int iStartRowIndex = 1;
                        int iStartColumnIndex = 1;
                        sl.ImportDataTable(iStartRowIndex, iStartColumnIndex, dt, true);
                        int iEndRowIndex = iStartRowIndex + dt.Rows.Count + 1 - 1;
                        int iEndColumnIndex = iStartColumnIndex + dt.Columns.Count - 1;
                        SLTable table = sl.CreateTable(iStartRowIndex, iStartColumnIndex, iEndRowIndex, iEndColumnIndex);
                        table.SetTableStyle(SLTableStyleTypeValues.Medium2);
                        table.HasTotalRow = false;
                        sl.InsertTable(table);

                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.AddItemPicture), false))
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                SLPicture slp = null;

                                string itemNo = dt.Rows[i]["ItemNo"].ToString();
                                var myItem = new Item(Item.Columns.ItemNo, itemNo);
                                Image img = null;
                                int height = 10;

                                string[] extensions = new string[] { "jpg", "gif", "png", "bmp", "jpeg" };

                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.UseImageLocal), false))
                                {
                                    string ImagePhotosFolder = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ImageLocalPath);
                                    if (!string.IsNullOrEmpty(ImagePhotosFolder) && Directory.Exists(ImagePhotosFolder))
                                    {

                                        foreach (string ext in extensions)
                                        {
                                            string ImagePath = ImagePhotosFolder + itemNo.Trim() + "." + ext;
                                            if (System.IO.File.Exists(ImagePath))
                                            {
                                                slp = new SLPicture(ImagePath);
                                                img = Image.FromFile(ImagePath);
                                                height = img.Height;
                                                img.Dispose();
                                                break;
                                            }

                                        }
                                    }
                                }
                                else
                                {
                                    if (myItem.ItemImage != null)
                                    {
                                        slp = new SLPicture(myItem.ItemImage, ImagePartType.Jpeg);
                                    }
                                }

                                if (slp != null)
                                {

                                    sl.SetRowHeight(iStartRowIndex + i, imageColumn - 1, height);
                                    slp.SetPosition(iStartRowIndex + i, imageColumn - 1);
                                    sl.InsertPicture(slp);
                                }
                            }
                        }
                        sl.AutoFitColumn(1, iEndColumnIndex);
                        sl.SaveAs(filepath);

                        return true;
                    }                    
                }
                catch (Exception ex)
                {
                    Logger.writeLog("Error when Export File: " + ex.Message);
                    return false;
                }
                return false;
            }
            return false;
        }

        public static bool ExportToExcel(DataTable dt, string filepath)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                ArrayList headerName = new ArrayList();

                foreach (DataColumn Col in dt.Columns)
                    headerName.Add(Col.ColumnName);

                ArrayList ColumnList = new ArrayList();
                for (int m = 0; m < dt.Columns.Count; m++)
                {
                    ColumnList.Add(m);
                }
                /*RKLib.ExportData.Export a = new RKLib.ExportData.Export("Win");
                a.ExportDetails(dt, (int[])ColumnList.ToArray(typeof(int)),
                    (string[])headerName.ToArray(typeof(string)),
                    RKLib.ExportData.Export.ExportFormat.CSV, filepath);*/
                ExportToCSV(dt, filepath);
                return true;
            }
            return false;
        }

        public static bool ExportToCSV(DataTable dt, string filepath)
        {
            if (dt == null)
            {
                return false;
            }
            //File.CreateText(filepath)
            //File.Open(myfilename, FileMode.Create
            using (StreamWriter f = new StreamWriter(File.Open(filepath, FileMode.Create), Encoding.UTF8))
            {
                //f.
                int i;
                for (i = 0; i < dt.Columns.Count - 1; i++)
                {
                    //if (dt.Columns[i].ColumnName != "UniqueID") 
                    f.Write("\"" + dt.Columns[i].ColumnName + "\",");
                }
                f.WriteLine("\"" + dt.Columns[i].ColumnName + "\"");

                for (int j = 0; j < dt.Rows.Count; ++j)
                {

                    for (i = 0; i < dt.Columns.Count - 1; i++)
                    {
                        //if (dt.Columns[i].ColumnName != "UniqueID") 
                        f.Write("\"" + dt.Rows[j][i].ToString() + "\",");
                    }
                    f.WriteLine("\"" + dt.Rows[j][i].ToString() + "\"");
                }
                f.Close();
            }
            return false;
        }

        public static bool ExportToCSV(DataGridView dgv, string filepath)
        {
            DataTable dt = (DataTable)dgv.DataSource;
            if (dt == null)
            {
                return false;
            }

            bool Flag;
            for (int Counter = dt.Columns.Count - 1; Counter >= 0; Counter--)
            {
                DataColumn Col = dt.Columns[Counter];

                Flag = false;
                foreach (DataGridViewColumn dgvCol in dgv.Columns)
                {
                    if (dgvCol.DataPropertyName == Col.ColumnName)
                    {
                        Flag = dgvCol.Visible;
                        break;
                    }
                }
                if (!Flag)
                {
                    dt.Columns.RemoveAt(Counter);
                }
            }

            StreamWriter f = File.CreateText(filepath);
            int i;
            for (i = 0; i < dt.Columns.Count - 1; i++)
            {
                //if (dt.Columns[i].ColumnName != "UniqueID") 
                f.Write("\"" + dt.Columns[i].ColumnName + "\",");
            }
            f.WriteLine("\"" + dt.Columns[i].ColumnName + "\"");

            for (int j = 0; j < dt.Rows.Count; ++j)
            {

                for (i = 0; i < dt.Columns.Count - 1; i++)
                {
                    //if (dt.Columns[i].ColumnName != "UniqueID") 
                    f.Write("\"" + dt.Rows[j][i].ToString() + "\",");
                }
                f.WriteLine("\"" + dt.Rows[j][i].ToString() + "\"");
            }
            f.Close();
            return false;
        }

        public static Hashtable ReadInventoryImportConfig()
        {
            Hashtable result = new Hashtable(); //Column name and its position
            char[] delim = { '=' };
            string[] splitresult;
            //Open text file            
            string fileName = INVENTORY_IMPORT_CONFIG;
            int lineNumber = 0;
            //Read text file content
            //split <field name = position> using sub string
            //- Open the text file
            using (StreamReader sr = new StreamReader(fileName))
            {
                string line;    //- Holds the entire line

                //- Cycle thru the text file 1 line at a time pulling
                //- substrings into the variables initialized above
                while ((line = sr.ReadLine()) != null)
                {
                    lineNumber++;
                    try
                    {
                        splitresult = line.Split(delim);
                        if (splitresult.Length == 2)
                        {
                            result.Add(splitresult[0].ToString().Trim().ToUpper(), splitresult[1].ToString().Trim());
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                    }
                }
                sr.Close();
            }
            return result;
        }

        public static bool ImportInventoryFromExcel(string strFilePath)
        {
            Hashtable ht = ReadInventoryImportConfig();

            DataTable dt = getDataTableFromXLS(strFilePath, ht);
            QueryCommandCollection cmd = new QueryCommandCollection();
            //read column configuration from text file

            if (ht == null || ht.Count == 0) return false;

            if (ht["CATEGORYNAME"] != null)//Check Category Column exist
            {
                //Scan through the category and Create Create
                ArrayList catName = new ArrayList();
                int catCol = int.Parse(ht["CATEGORYNAME"].ToString());
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    catName.Add(dt.Rows[i][catCol].ToString());
                }
                catName.Sort();


                //Delete if same name exist
                int k = 1;
                while (k < catName.Count)
                {

                    if (catName[k].ToString() == catName[k - 1].ToString())
                    {
                        catName.RemoveAt(k);
                    }
                    else
                    {
                        k++;

                    }
                }

                //insert a new one
                int j = 0, count;
                count = Category.CreateQuery().
                    WHERE(Category.Columns.CategoryName,
                    catName[j].ToString()).
                    GetCount(Category.Columns.CategoryName);
                if (count == 0)
                {
                    //do insert
                    Category c = new Category();
                    c.AccountCategory = "";
                    c.CategoryId = "";
                    c.CategoryName = catName[j].ToString();
                    c.IsForSale = true;
                    c.IsDiscountable = true;
                    c.IsNew = true;
                    cmd.Add(c.GetInsertCommand("IMPORT"));
                }
            }
            else
            {
                //Use system category.
                //insert system category if not exist.....
                dt.Columns.Add("CATEGORYNAME");
                for (int m = 0; m < dt.Rows.Count; m++)
                {
                    dt.Rows[m][dt.Columns.Count - 1] = "SYSTEM";
                }
                ht.Add("CATEGORYNAME", dt.Columns.Count - 1);
            }
            int ITEMNO_COL, QTY_COL;

            int.TryParse(ht["ITEMNO"].ToString(), out ITEMNO_COL);
            int.TryParse(ht["QUANTITY"].ToString(), out QTY_COL);

            Query qry = new Query("Item");
            Where whr = new Where();
            whr.ColumnName = "ItemNo";
            whr.Comparison = Comparison.Equals;
            whr.ParameterName = "@ItemId";

            TableSchema.TableColumnCollection tCol = DataService.GetTableSchema("Item", "PowerPOS").Columns;
            //check product exist
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //Get count....
                whr.ParameterValue = dt.Rows[i][ITEMNO_COL];
                if (qry.GetCount(Item.Columns.ItemNo, whr) > 0)
                {
                    //update the fields
                    Query updateQry = Item.CreateQuery();
                    updateQry.QueryType = QueryType.Update;
                    foreach (DictionaryEntry de in ht)
                    {
                        if (de.Key.ToString().ToUpper() != "QUANTITY")
                        {
                            updateQry.AddWhere(Item.Columns.ItemNo, dt.Rows[i][ITEMNO_COL]);
                            updateQry.AddUpdateSetting(de.Key.ToString(), dt.Rows[i][int.Parse(de.Value.ToString())]);
                        }
                    }
                    cmd.Add(updateQry.BuildUpdateCommand());
                }
                else
                {
                    //insert
                    Item item = new Item();
                    item.IsNew = true;
                    item.IsInInventory = true;
                    item.Deleted = false;
                    foreach (DictionaryEntry de in ht)
                    {
                        item.SetColumnValue(de.Key.ToString(), dt.Rows[i][int.Parse(de.Value.ToString())]); ;
                    }
                    cmd.Add(item.GetInsertCommand("IMPORT"));
                }
            }


            //Perform transactions
            DataService.ExecuteTransaction(cmd);
            //do stock in
            //find quantity column
            //build stock in command
            InventoryController invCtrl = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
            string status;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                invCtrl.AddItemIntoInventory(dt.Rows[i][ITEMNO_COL].ToString(),
                    int.Parse(dt.Rows[i][QTY_COL].ToString()), out status);
            }
            invCtrl.StockIn("IMPORT", PointOfSaleInfo.InventoryLocationID, false, false, out status);

            return true;
        }

        public static DataTable getDataTableFromXLS(string strFilePath, Hashtable columns)
        {
            try
            {
                string strConnectionString = string.Empty;
                strConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFilePath + @";Extended Properties=""Excel 8.0;HDR=No;IMEX=1""";
                OleDbConnection cnCSV = new OleDbConnection(strConnectionString);
                cnCSV.Open();
                OleDbCommand cmdSelect = new OleDbCommand(@"SELECT * FROM [Sheet1$]", cnCSV);
                OleDbDataReader reader = cmdSelect.ExecuteReader();
                DataTable dtCSV = new DataTable();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    dtCSV.Columns.Add(i.ToString());
                }
                while (reader.Read())
                {
                    DataRow dr = dtCSV.NewRow();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dr[i] = reader.GetValue(i).ToString();
                    }
                    dtCSV.Rows.Add(dr);

                }
                cnCSV.Close();
                dtCSV.Rows.RemoveAt(0);
                return dtCSV;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
            finally
            {
            }
        }
    }
}
