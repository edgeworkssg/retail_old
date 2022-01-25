using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;

using PowerPOS.Container;
using System.Windows.Forms;

namespace PowerPOS
{
    public class TouchScreenHotKeysController
    {
        private const string XMLFILE = "\\TouchScreenQuickAccess.xml";
        private const string ButtonImageFolder = "\\ButtonImage\\";
        private DataSet ds;
        public Stack<string> Stck = new Stack<string>();
        private SortedList<string, int> MasterData = new SortedList<string, int>();

        private Stack<string> History = new Stack<string>();

        #region -= COLORS =-
        public const string BLUE = "blue";
        public const string GOLD = "gold";
        public const string GREEN = "green";
        public const string LIGHTBLUE = "lightblue";
        public const string PURPLE = "purple";
        public const string LIGHTGREEN = "lightgreen";
        public const string YELLOW = "yellow";
        public const string GREY = "grey";
        public const string ORANGE = "orange";
        public const string LIGHTORANGE = "lightorange";
        public const string RED = "red";
        public const string BROWN = "brown";
        #endregion

        #region -= Dead Function =-
        //public string GetHotKeyItem(string HotKey)
        //{
        //    try
        //    {                
        //        ds = new DataSet();
        //        ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + XMLFILE);
        //        DataRow[] dr;

        //        dr = ds.Tables[0].Select("keyname='" + HotKey + "'");
        //        if (dr != null)
        //        {
        //            for (int i = 0; i < dr.Length; i++)
        //            {
        //                return dr[i]["itemno"].ToString();
        //            }
        //        }
        //        return "";
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.writeLog(ex);
        //        throw ex;
        //    }              
        //    finally {
        //        ds.Dispose();
        //    }            
        //}

        public DataTable GetDataTables()
        {
            ds = new DataSet();
            ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + XMLFILE);
            if (ds != null)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }
        public void AssignHotkey(string HotKey, string ItemNo, string itemname)
        {
            try
            {
                ds = new DataSet();
                ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + XMLFILE);
                DataRow[] dr;

                dr = ds.Tables[0].Select("keyname='" + HotKey + "'");
                if (dr != null)
                {
                    for (int i = 0; i < dr.Length; i++)
                    {
                        dr[i]["itemno"] = ItemNo;
                        dr[i]["itemname"] = itemname;
                    }
                }
                ds.WriteXml(AppDomain.CurrentDomain.BaseDirectory + "\\POSDevices\\HotKeys.xml");
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                throw ex;
            }
            finally
            {
                ds.Dispose();
            }
        }
        public ArrayList GetHotKeyList()
        {
            try
            {
                ds = new DataSet();
                ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + XMLFILE);
                ArrayList ar = new ArrayList();
                ar.Add("");
                if (ds != null && ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ar.Add(ds.Tables[0].Rows[i]["keyname"].ToString());
                    }
                    return ar;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                throw ex;
            }
            finally
            {
                ds.Dispose();
            }
        }
        #endregion

        public TouchScreenHotKeysController()
        {
            ds = new DataSet();
            ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + XMLFILE);
        }

        public void ClearControls(System.Windows.Forms.TableLayoutPanel myPanel)
        {
            for (int i = 0; i < myPanel.ColumnCount; i++)
            {
                for (int j = 0; j < myPanel.RowCount; j++)
                {
                    Control curr = myPanel.GetControlFromPosition(i, j);
                    curr.Text = "";
                    curr.Tag = "";
                    curr.BackgroundImage = null;
                    curr.ForeColor = System.Drawing.Color.Black;
                }
            }
        }

        public void SetButton(Button myButton, string ID, string ShownName, string ForeColor, string BackColor)
        {
            myButton.Text = ShownName;
            myButton.Tag = ID;
            #region *) Set: Fore Color
            if (ForeColor != "")
            {
                try
                {
                    myButton.ForeColor =
                    System.Drawing.Color.FromArgb(Convert.ToInt32(ForeColor, 16));
                }
                catch (Exception ex)
                {
                    myButton.ForeColor =
                        System.Drawing.Color.FromName(ForeColor);
                }
                
            }
            #endregion
            #region *) Set: Back Color
            if (BackColor != "")
            {
                if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory +
                    TouchScreenHotKeysController.ButtonImageFolder + BackColor + ".png"))
                {
                    myButton.BackgroundImage =
                        System.Drawing.Image.FromFile(AppDomain.CurrentDomain.BaseDirectory +
                        TouchScreenHotKeysController.ButtonImageFolder + BackColor + ".png");
                }
                else
                {
                    //myButton.BackgroundImage = null;
                    try
                    {
                        myButton.UseVisualStyleBackColor = true;
                        myButton.BackColor =
                        System.Drawing.Color.FromArgb(Convert.ToInt32(BackColor, 16));
                        
                    }
                    catch (Exception ex)
                    {
                        myButton.BackColor =
                            System.Drawing.Color.FromName(BackColor);                                                
                    }
                }
            }
            myButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8);
            #endregion
        }

        public void populateItemDepartmentDisplayPanel
            (System.Windows.Forms.TableLayoutPanel myPanel)
        {
            int X, Y;
            //ItemDepartment id;

            //DataSet ds = new DataSet();
            //ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + XMLFILE);

            ClearControls(myPanel);

            for (int Counter = 0; Counter < ds.Tables[0].Rows.Count; Counter++)
            {
                X = 0;
                Y = 0;
                DataRow dr = ds.Tables[0].Rows[Counter];
                //id = new ItemDepartment(dr["ItemDepartmentID"].ToString());

                #region *) Fetch: Try to get X & Y
                if (!(int.TryParse(dr["X"].ToString(), out X) && int.TryParse(dr["Y"].ToString(), out Y)))
                    continue;
                #endregion
                #region *) Validation: X & Y not greater than panel
                if (X >= myPanel.ColumnCount || Y >= myPanel.RowCount)
                    continue;
                #endregion

                Control selectedControl = myPanel.GetControlFromPosition(X, Y);

                #region *) Validation: Selected control is Button & not null
                if (selectedControl == null && !(selectedControl is Button))
                    continue;
                #endregion
                #region *) Validation: Selected control has never been updated for this turn
                if (selectedControl.Tag != null && selectedControl.Tag != "")
                    continue;
                #endregion

                SetButton((Button)selectedControl, dr["ItemDepartment_ID"].ToString(), dr["ItemDepartmentID"].ToString(), dr["ForeColor"].ToString(), dr["BackColor"].ToString());
            }
        }

        public void populateCategoryDisplayPanel
            (System.Windows.Forms.TableLayoutPanel myPanel, string ItemDepartmentID)
        {
            int X, Y;
            //Category id;

            DataRow[] drs = ds.Tables[1].Select("ItemDepartment_ID = '" + ItemDepartmentID + "'");
            
            //DataSet ds = new DataSet();
            //ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + XMLFILE);

            ClearControls(myPanel);

            for (int Counter = 0; Counter < drs.Length; Counter++)
            {
                X = 0;
                Y = 0;
                DataRow dr = drs[Counter];
                //id = new Category(dr["CategoryName"].ToString());

                #region *) Fetch: Try to get X & Y
                if (!(int.TryParse(dr["X"].ToString(), out X) && int.TryParse(dr["Y"].ToString(), out Y)))
                    continue;
                #endregion
                #region *) Validation: X & Y not greater than panel
                if (X >= myPanel.ColumnCount || Y >= myPanel.RowCount)
                    continue;
                #endregion

                Control selectedControl = myPanel.GetControlFromPosition(X, Y);

                #region *) Validation: Selected control is Button & not null
                if (selectedControl == null && !(selectedControl is Button))
                    continue;
                #endregion
                #region *) Validation: Selected control has never been updated for this turn
                if (selectedControl.Tag != null && selectedControl.Tag != "")
                    continue;
                #endregion

                SetButton((Button)selectedControl, dr["Category_ID"].ToString(), dr["CategoryName"].ToString(), dr["ForeColor"].ToString(), dr["BackColor"].ToString());
            }
        }

        public void populateItemDisplayPanel
            (System.Windows.Forms.TableLayoutPanel myPanel, string CategoryName)
        {
            int X, Y;
            Item id;

            DataRow[] drs = ds.Tables[2].Select("Category_ID = '" + CategoryName + "'");

            //DataSet ds = new DataSet();
            //ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + XMLFILE);

            ClearControls(myPanel);

            for (int Counter = 0; Counter < drs.Length; Counter++)
            {
                X = 0;
                Y = 0;
                DataRow dr = drs[Counter];
                id = new Item(dr["ItemNo"].ToString());

                #region *) Fetch: Try to get X & Y
                if (!(int.TryParse(dr["X"].ToString(), out X) && int.TryParse(dr["Y"].ToString(), out Y)))
                    continue;
                #endregion
                #region *) Validation: X & Y not greater than panel
                if (X >= myPanel.ColumnCount || Y >= myPanel.RowCount)
                    continue;
                #endregion

                Control selectedControl = myPanel.GetControlFromPosition(X, Y);

                #region *) Validation: Selected control is Button & not null
                if (selectedControl == null && !(selectedControl is Button))
                    continue;
                #endregion
                #region *) Validation: Selected control has never been updated for this turn
                if (selectedControl.Tag != null && selectedControl.Tag != "")
                    continue;
                #endregion
                string shownname = "";
                if (id.IsServiceItem.HasValue && id.IsServiceItem.Value)
                {
                    shownname = "[O]" + " " + id.ItemName;
                }
                else
                {
                    shownname = "[" + id.RetailPrice.ToString("N2") +"]" + " " + id.ItemName;
                }
                SetButton((Button)selectedControl, id.ItemNo, shownname, dr["ForeColor"].ToString(), dr["BackColor"].ToString());
            }
        }
        public static void populateItemDisplayPanel
            (System.Windows.Forms.FlowLayoutPanel myPanel, string CategoryName)
        {

            DataSet ds = new DataSet();
            ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + XMLFILE);

            //get category id
            DataRow[] dr = ds.Tables[0].Select("CategoryName = '" + CategoryName + "'");
            if (dr.Length <= 0)
                return;
            string CategoryID = dr[0]["Category_ID"].ToString(); //XML ID
            Item it;
            dr = ds.Tables[1].Select("Category_ID = '" + CategoryID + "'");
            int j = 0;
            for (int i = 0; i < myPanel.Controls.Count; i++)
            {
                if (j < dr.Length)
                {
                    it = new Item(dr[j]["ItemNo"].ToString());
                    if (it != null && it.IsLoaded)
                    {
                        myPanel.Controls[i].Text = it.ItemName;
                        ((Button)myPanel.Controls[i]).Tag = it.ItemNo;
                        ((Button)myPanel.Controls[i]).Font = new System.Drawing.Font("Verdana", 4);
                        ((Button)myPanel.Controls[i]).Refresh();
                    }
                    else
                    {
                        myPanel.Controls[i].Text = "";
                    }
                }
                j++;
            }
        }

        public void AddNewItem(string ItemNo)
        {
            Item Curr = new Item(ItemNo );
            if (Curr == null || !Curr.IsLoaded) return;

            if (MasterData.ContainsKey(ItemNo))
            {
                MasterData[ItemNo] = MasterData[ItemNo] + 1;
            }
            else
            {
                MasterData.Add(ItemNo, 1);
            }

        }

        public void SelectButton(System.Windows.Forms.TableLayoutPanel myPanel, string Value)
        {
            if (Value == null || Value == "") return;

            if (Stck.Count == 0)
            {
                populateCategoryDisplayPanel(myPanel, Value);
                Stck.Push(Value);
            }
            else if (Stck.Count == 1)
            {
                populateItemDisplayPanel(myPanel, Value);
                Stck.Push(Value);
            }
            else
            {
                AddNewItem(Value);
            }
        }

        public bool GoBack(System.Windows.Forms.TableLayoutPanel myPanel)
        {
            if (Stck.Count == 0)
            {
                return false;
            }
            else
            {
                if (Stck.Count == 1)
                {
                    Stck.Pop();
                    populateItemDepartmentDisplayPanel(myPanel);
                }
                else if (Stck.Count == 2)
                {
                    Stck.Pop();
                    string ParentValue = Stck.Peek();
                    populateCategoryDisplayPanel(myPanel, ParentValue);
                }

                return true;
            }
        }

        public DataTable GetAllData()
        {
            ItemCollection Result = new ItemCollection();
            for (int Counter = 0; Counter < MasterData.Count; Counter++)
            {
                Item Curr = new Item(MasterData.Keys[0]);
                if (Curr == null || !Curr.IsLoaded) continue;

                Result.Add(Curr);
            }

            DataTable dt = Result.ToDataTable();

            dt.Columns.Add("Quantity", Type.GetType("System.Int32"));
            dt.Columns.Add("TotalAmount", Type.GetType("System.Decimal"), "Quantity * RetailPrice");

            for (int Counter = 0; Counter < dt.Rows.Count; Counter++)
            {
                DataRow Rw = dt.Rows[Counter];

                if (MasterData.ContainsKey(Rw["ItemNo"].ToString()))
                {
                    Rw["Quantity"] = MasterData[Rw["ItemNo"].ToString()];
                }
                else
                {
                    Rw["Quantity"] = 0;
                }
            }

            return dt;
        }

        public void ChangeQuantity(string ItemNo, int Quantity)
        {
            if (MasterData.ContainsKey(ItemNo))
            {
                MasterData[ItemNo] = Quantity;
                if (MasterData[ItemNo] == 0)
                    MasterData.Remove(ItemNo);
            }
        }

        public string GetNavigation()
        {
            string[] Lists = Stck.ToArray();
            string Result = "";

            if (Lists.Length == 1)
            {
                DataRow[] drs = ds.Tables[0].Select("ItemDepartment_ID = '" + Lists[0] + "'");
                if (drs.Length > 0)
                    Result = drs[0]["ItemDepartmentID"].ToString();
            }
            else if (Lists.Length == 2)
            {
                DataRow[] drs = ds.Tables[0].Select("ItemDepartment_ID = '" + Lists[1] + "'");
                if (drs.Length > 0)
                    Result = drs[0]["ItemDepartmentID"].ToString();
                drs = ds.Tables[1].Select("Category_ID = '" + Lists[0] + "'");
                if (drs.Length > 0)
                    Result += "->" + drs[0]["CategoryName"].ToString();
            }

            return Result;
        }
    }
}
