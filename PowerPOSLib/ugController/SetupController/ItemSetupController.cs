using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SubSonic;

namespace PowerPOS.Setup
{
    public class ItemSetupController : SetupController
    {
        public struct SpecialCodes
        {
            public const int Show_Error_Barcode = 1;
        }

        private ItemDepartmentCollection _ItemDepartment;
        private CategoryCollection _Category;
        private ItemCollection _Item;

        public ItemSetupController()
        {
            _ItemDepartment = new ItemDepartmentCollection();
            _ItemDepartment.Where(ItemDepartment.Columns.DepartmentName, Comparison.NotEquals, "SYSTEM");
            _ItemDepartment.Where(ItemDepartment.Columns.Deleted, Comparison.Equals, false);
            _ItemDepartment.Load();

            _Category = new CategoryCollection();
            _Category.Where(Category.Columns.CategoryName, Comparison.NotEquals, "SYSTEM");
            _Category.Where(Category.Columns.Deleted, Comparison.Equals, false);
            _Category.Load();

            _Item = new ItemCollection();
            _Item.Where(Item.Columns.CategoryName, Comparison.NotEquals, "SYSTEM");
            _Item.Where(Item.Columns.Deleted, Comparison.Equals, false);
            _Item.Load();
        }

        public DataTable FetchAll()
        {
            DataTable Output = _Item.ToDataTable();

            return AddDepartmentColumn(Output);
        }
        public DataTable FetchAll(object SearchValue)
        {
            if (string.IsNullOrEmpty(SearchValue.ToString()))
                return FetchAll();

            ItemCollection myItems = new ItemCollection();

            myItems.AddRange(_Item.Where(Fnc
                => Fnc.ItemName.ToLower().Contains(SearchValue.ToString().ToLower())
                || Fnc.CategoryName.Contains(SearchValue.ToString().ToLower())
                || (Fnc.ItemDesc != null && Fnc.ItemDesc.ToLower().Contains(SearchValue.ToString().ToLower()))
                || (Fnc.Barcode != null && Fnc.Barcode.ToLower().Contains(SearchValue.ToString().ToLower()))
                || (Fnc.Attributes1 != null && Fnc.Attributes1.ToLower().Contains(SearchValue.ToString().ToLower()))
                || (Fnc.Attributes2 != null && Fnc.Attributes2.ToLower().Contains(SearchValue.ToString().ToLower()))
                || (Fnc.Attributes3 != null && Fnc.Attributes3.ToLower().Contains(SearchValue.ToString().ToLower()))
                || (Fnc.Attributes4 != null && Fnc.Attributes4.ToLower().Contains(SearchValue.ToString().ToLower()))
                || (Fnc.Attributes5 != null && Fnc.Attributes5.ToLower().Contains(SearchValue.ToString().ToLower()))
                ));

            return AddDepartmentColumn(myItems.ToDataTable());
        }

        public DataTable FetchSpecial(int SpecialCode, object SearchValue)
        {
            ItemCollection SpecialItems = new ItemCollection();
            if (SpecialCode == SpecialCodes.Show_Error_Barcode)
            {
                for (int Counter = 0; Counter < _Item.Count; Counter++)
                {
                    Item InvestigatedItem = _Item[Counter];

                    if (SpecialItems.Contains(InvestigatedItem)) continue;
                    if (string.IsNullOrEmpty(InvestigatedItem.Barcode)) { SpecialItems.Add(InvestigatedItem); continue; }
                    if (_Item.Count(Fnc => Fnc.Barcode == InvestigatedItem.Barcode) > 1) { SpecialItems.Add(InvestigatedItem); continue; }
                }
            }

            ItemCollection myItems = new ItemCollection();

            myItems.AddRange(SpecialItems.Where(Fnc
                => Fnc.ItemName.ToLower().Contains(SearchValue.ToString().ToLower())
                || Fnc.CategoryName.Contains(SearchValue.ToString().ToLower())
                || (Fnc.ItemDesc != null && Fnc.ItemDesc.ToLower().Contains(SearchValue.ToString().ToLower()))
                || (Fnc.Barcode != null && Fnc.Barcode.ToLower().Contains(SearchValue.ToString().ToLower()))
                || (Fnc.Attributes1 != null && Fnc.Attributes1.ToLower().Contains(SearchValue.ToString().ToLower()))
                || (Fnc.Attributes2 != null && Fnc.Attributes2.ToLower().Contains(SearchValue.ToString().ToLower()))
                || (Fnc.Attributes3 != null && Fnc.Attributes3.ToLower().Contains(SearchValue.ToString().ToLower()))
                || (Fnc.Attributes4 != null && Fnc.Attributes4.ToLower().Contains(SearchValue.ToString().ToLower()))
                || (Fnc.Attributes5 != null && Fnc.Attributes5.ToLower().Contains(SearchValue.ToString().ToLower()))
                ));

            return AddDepartmentColumn(myItems.ToDataTable());
        }

        private DataTable AddDepartmentColumn(DataTable Value)
        {
            Value.Columns.Add("DepartmentName", Type.GetType("System.String"));

            for (int Counter = 0; Counter < Value.Rows.Count; Counter++)
            {
                Category ActiveCategory = _Category.FirstOrDefault(Fnc => Fnc.CategoryName == Value.Rows[Counter].Field<string>(Item.Columns.CategoryName).ToString());
                if (ActiveCategory != null && !ActiveCategory.IsNew)
                {
                    Value.Rows[Counter].SetField<string>("DepartmentName", ActiveCategory.ItemDepartment.DepartmentName);
                }
            }

            return Value;
        }

        public void Update(Dictionary<string,object> Value)
        {
            if (!Value.ContainsKey(Item.Columns.ItemNo))
                throw new Exception("Bug Found: No Item No column is found from the input value when doing update.");

            Item UpdatedItem = _Item.FirstOrDefault(Fnc => Fnc.ItemNo.ToLower().Trim() == Value[Item.Columns.ItemNo].ToString().ToLower().Trim());

            if (UpdatedItem == null)
                throw new Exception("Bug Found: This class doesn't support Adding-New-Item Feature.");

            decimal tmpDec = 0;
            bool tmpBool = false;

            if (Value.ContainsKey(Item.Columns.ItemName))
                UpdatedItem.ItemName = Value[Item.Columns.ItemName].ToString().Trim();
            if (Value.ContainsKey(Item.Columns.Barcode))
                UpdatedItem.Barcode = Value[Item.Columns.Barcode].ToString().Trim();
            //if (Value.ContainsKey(Item.Columns.CategoryName))
            //    UpdatedItem.CategoryName = Value[Item.Columns.CategoryName].ToString().Trim();
            if (Value.ContainsKey(Item.Columns.RetailPrice) && decimal.TryParse(Value[Item.Columns.RetailPrice].ToString(), out tmpDec))
                UpdatedItem.RetailPrice = tmpDec;
            if (Value.ContainsKey(Item.Columns.FactoryPrice) && decimal.TryParse(Value[Item.Columns.FactoryPrice].ToString(), out tmpDec))
                UpdatedItem.FactoryPrice = tmpDec;
            if (Value.ContainsKey(Item.Columns.MinimumPrice) && decimal.TryParse(Value[Item.Columns.MinimumPrice].ToString(), out tmpDec))
                UpdatedItem.MinimumPrice = tmpDec;
            if (Value.ContainsKey(Item.Columns.ItemDesc))
                UpdatedItem.ItemDesc = Value[Item.Columns.ItemDesc].ToString().Trim();
            if (Value.ContainsKey(Item.Columns.IsServiceItem))
            {
                string SelectedVal = Value[Item.Columns.IsServiceItem].ToString().Trim().ToLower();
                if (SelectedVal == "yes" || SelectedVal == "true")
                    UpdatedItem.IsServiceItem = true;
                else if (SelectedVal == "no" || SelectedVal == "false")
                    UpdatedItem.IsServiceItem = false;
            }
            if (Value.ContainsKey(Item.Columns.IsInInventory))
            {
                string SelectedVal = Value[Item.Columns.IsInInventory].ToString().Trim().ToLower();
                if (SelectedVal == "yes" || SelectedVal == "true")
                    UpdatedItem.IsInInventory = true;
                else if (SelectedVal == "no" || SelectedVal == "false")
                    UpdatedItem.IsInInventory = false;
            }
            if (Value.ContainsKey(Item.Columns.IsNonDiscountable))
            {
                string SelectedVal = Value[Item.Columns.IsNonDiscountable].ToString().Trim().ToLower();
                if (SelectedVal == "yes" || SelectedVal == "true")
                    UpdatedItem.IsNonDiscountable = true;
                else if (SelectedVal == "no" || SelectedVal == "false")
                    UpdatedItem.IsNonDiscountable = false;
            }
            if (Value.ContainsKey(Item.Columns.Attributes1))
                UpdatedItem.Attributes1 = Value[Item.Columns.Attributes1].ToString().Trim();
            if (Value.ContainsKey(Item.Columns.Attributes2))
                UpdatedItem.Attributes2 = Value[Item.Columns.Attributes2].ToString().Trim();
            if (Value.ContainsKey(Item.Columns.Attributes3))
                UpdatedItem.Attributes3 = Value[Item.Columns.Attributes3].ToString().Trim();
            if (Value.ContainsKey(Item.Columns.Attributes4))
                UpdatedItem.Attributes4 = Value[Item.Columns.Attributes4].ToString().Trim();
            if (Value.ContainsKey(Item.Columns.Attributes5))
                UpdatedItem.Attributes5 = Value[Item.Columns.Attributes5].ToString().Trim();
            if (Value.ContainsKey(Item.Columns.Attributes6))
                UpdatedItem.Attributes6 = Value[Item.Columns.Attributes6].ToString().Trim();
            if (Value.ContainsKey(Item.Columns.Attributes7))
                UpdatedItem.Attributes7 = Value[Item.Columns.Attributes7].ToString().Trim();
            if (Value.ContainsKey(Item.Columns.Attributes8))
                UpdatedItem.Attributes8 = Value[Item.Columns.Attributes8].ToString().Trim();
            if (Value.ContainsKey(Item.Columns.Remark))
                UpdatedItem.Remark = Value[Item.Columns.Remark].ToString().Trim();


            //myItem.Load(Value);

        }

        public void SaveChanges()
        {
            _ItemDepartment.SaveAll(PowerPOS.Container.UserInfo.displayName);
            _Category.SaveAll(PowerPOS.Container.UserInfo.displayName);
            _Item.SaveAll(PowerPOS.Container.UserInfo.displayName);
        }
    }
}
