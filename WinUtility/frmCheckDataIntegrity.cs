using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SubSonic;
using PowerPOS;

namespace WinUtility
{
    public partial class frmCheckDataIntegrity : Form
    {
        public frmCheckDataIntegrity()
        {
            InitializeComponent();
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            DateTime mStartDate = startDate.Value.Date;
            DateTime mEndDate = endDate.Value.Date;

            //Load all OrderHdr
            string SQL;
            QueryCommand cmd;

            SQL = "select orderhdrid from orderhdr where isvoided=0 and orderdate > '" + mStartDate.ToString("yyyy-MM-dd") + "' and orderdate < '" + mEndDate.ToString("yyyy-MM-dd") + "'";
            cmd = new QueryCommand(SQL, "PowerPOS");
            DataTable dt = DataService.GetDataSet(cmd).Tables[0];
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("OrderDetID");
            dtResult.Columns.Add("ItemNo");
            dtResult.Columns.Add("Quantity");
            dtResult.Columns.Add("UnitPrice");
            dtResult.Columns.Add("Discount");
            dtResult.Columns.Add("Amount");
            dtResult.Columns.Add("ExpectedAmount");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                OrderDetCollection cols = new OrderDetCollection();
                cols.Where(OrderDet.Columns.OrderHdrID, dt.Rows[i][0].ToString());
                cols.Where(OrderDet.Columns.IsVoided, false);
                cols.Load();
                for (int j = 0; j < cols.Count; j++)
                {
                    //decimal orderHdrDisc = new OrderHdr(dt.Rows[i][0].ToString()).Discount;
                    //decimal lineAmount = CalculateLineAmount(cols[j]);
                    decimal expectedResult = 0.0M;
                    if (!InventoryController.CalculateLineAmountDataIntegrity(cols[j], out expectedResult))                    
                    {
                        DataRow dr = dtResult.NewRow();
                        dr["OrderDetID"] = cols[j].OrderDetID;
                        dr["ItemNo"] = cols[j].ItemNo;
                        dr["Quantity"] = cols[j].Quantity;
                        dr["UnitPrice"] = cols[j].UnitPrice;
                        dr["Discount"] = cols[j].Discount;
                        dr["Amount"] = cols[j].Amount;
                        dr["ExpectedAmount"] = expectedResult;
                        dtResult.Rows.Add(dr);
                    }
                }
            }
            MessageBox.Show("Done");
            dgvResult.DataSource = dtResult;
            dgvResult.Refresh();

        }
        
        public decimal CalculateLineAmount(OrderDet myOrderDetItem)
        {
            decimal GST = 7.0M;
            decimal result = 0.0M;
            //if (DISCOUNT_BY_PERCENTAGE)
            //{
            //Discount by percentage
            int GSTRule = 0;
            if (myOrderDetItem.Item.GSTRule.HasValue) GSTRule = myOrderDetItem.Item.GSTRule.Value;
            
            if (myOrderDetItem.IsPromo)
            {
                if (myOrderDetItem.UsePromoPrice.HasValue && myOrderDetItem.UsePromoPrice.Value)
                {
                    if (GSTRule == 1)
                    {
                        //Exclusive GST
                        result = Math.Round(myOrderDetItem.Quantity.GetValueOrDefault(0) *
                         (decimal)myOrderDetItem.PromoUnitPrice, 2)
                         * (1 + ((decimal)GST) / 100); //The GST part                       

                        myOrderDetItem.GSTAmount = (myOrderDetItem.PromoAmount / (1 + ((decimal)GST) / 100)) * ((decimal)GST / 100);
                    }
                    else
                    {
                        //Inclusive GST
                        result =
                            Math.Round(myOrderDetItem.Quantity.GetValueOrDefault(0) *
                         (decimal)myOrderDetItem.PromoUnitPrice, 2);


                        if (GSTRule == 2) //Inclusive GST
                        {
                            myOrderDetItem.GSTAmount = (myOrderDetItem.PromoAmount / (1 + ((decimal)GST) / 100)) * ((decimal)GST / 100); ;
                        }
                        else
                        {
                            myOrderDetItem.GSTAmount = 0;
                        }
                    }
                }
                else
                {
                    if (GSTRule == 1)
                    {
                        //Exclusinge GST
                        result = Math.Round(myOrderDetItem.Quantity.GetValueOrDefault(0) *
                            (decimal)myOrderDetItem.UnitPrice *
                            (decimal)(1 - (myOrderDetItem.PromoDiscount / 100))
                            * (1 + ((decimal)GST) / 100), 2); //The GST part        

                        result =
                            (myOrderDetItem.PromoAmount / (1 + ((decimal)GST) / 100)) *
                            ((decimal)GST / 100);
                    }
                    else
                    {
                        //Inclusive GST & NO GST
                        result = Math.Round(myOrderDetItem.Quantity.GetValueOrDefault(0) *
                            (decimal)myOrderDetItem.UnitPrice *
                            (decimal)(1 - (myOrderDetItem.PromoDiscount / 100)), 2);

                        if (GSTRule == 2) //Inclusive GST
                        {
                            myOrderDetItem.GSTAmount = (myOrderDetItem.PromoAmount / (1 + ((decimal)GST) / 100)) * ((decimal)GST / 100); ;
                        }
                        else
                        {
                            myOrderDetItem.GSTAmount = 0;
                        }
                    }
                }
            }
            else
            {
                if (GSTRule == 1)
                {
                    //Exclusive GST
                    result = Math.Round(
                        myOrderDetItem.Quantity.GetValueOrDefault(0) *
                        myOrderDetItem.UnitPrice *
                        (1 - (myOrderDetItem.Discount / 100))
                         * (1 + ((decimal)GST) / 100), 2); //The GST part            
                    myOrderDetItem.GSTAmount = (myOrderDetItem.Amount / (1 + ((decimal)GST) / 100)) * ((decimal)GST / 100);
                }
                else
                {
                    //Inclusive GST
                    result = Math.Round(
                        myOrderDetItem.Quantity.GetValueOrDefault(0) *
                        myOrderDetItem.UnitPrice *
                        (1 - (myOrderDetItem.Discount / 100)), 2);

                    if (GSTRule == 2) //Inclusive GST
                    {
                        myOrderDetItem.GSTAmount = myOrderDetItem.Amount / (1 + ((decimal)GST) / 100) * ((decimal)GST / 100); ;
                    }
                    else
                    {
                        myOrderDetItem.GSTAmount = 0;
                    }
                }
            }

            return result;
        }
        
     
    }
}
