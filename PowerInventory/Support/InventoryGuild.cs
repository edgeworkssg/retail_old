using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerInventory.Support
{
    public partial class InventoryGuild
    {
        List<InventoryMage> Registrar;

        public InventoryGuild()
        {
            Registrar = new List<InventoryMage>();
        }

        public void RegisterMage(string ItemNo, int InventoryLocationID)
        {
            InventoryMage WantedMage = Registrar.FirstOrDefault(Fnc => Fnc.ItemNo == ItemNo && Fnc.InventoryLocationID == InventoryLocationID);

            if (WantedMage != null)
                throw new Exception("(error)Trying to re-register existing mage");

            InventoryMage theMage;
            theMage = new InventoryMage(ItemNo, InventoryLocationID);
            theMage.GenerateAllMovement();

            Registrar.Add(theMage);
        }

        public DataTable CastAllOutMagix()
        {
            return CastMagix("", true, true, true);
        }

        public DataTable CastMagix(string MagixWord, bool ShowNormalTransaction, bool ShowUndeductedSales, bool ShowUnassignedError)
        {
            DataTable Dt = null;

            for (int Counter = 0; Counter < Registrar.Count; Counter++)
            {
                PowerPOS.Item myItem = new PowerPOS.Item(Registrar[Counter].ItemNo);
                bool IsThisTheOne = false;
                IsThisTheOne = IsThisTheOne || myItem.ItemName.Contains(MagixWord);
                IsThisTheOne = IsThisTheOne || (myItem.Attributes1 != null && myItem.Attributes1.Contains(MagixWord));
                IsThisTheOne = IsThisTheOne || (myItem.Attributes2 != null && myItem.Attributes2.Contains(MagixWord));
                IsThisTheOne = IsThisTheOne || (myItem.Attributes3 != null && myItem.Attributes3.Contains(MagixWord));
                IsThisTheOne = IsThisTheOne || (myItem.Attributes4 != null && myItem.Attributes4.Contains(MagixWord));
                IsThisTheOne = IsThisTheOne || (myItem.Attributes5 != null && myItem.Attributes5.Contains(MagixWord));
                IsThisTheOne = IsThisTheOne || (myItem.Attributes6 != null && myItem.Attributes6.Contains(MagixWord));
                IsThisTheOne = IsThisTheOne || (myItem.Attributes7 != null && myItem.Attributes7.Contains(MagixWord));
                IsThisTheOne = IsThisTheOne || (myItem.Attributes8 != null && myItem.Attributes8.Contains(MagixWord));
                IsThisTheOne = IsThisTheOne || myItem.CategoryName.Contains(MagixWord);
                IsThisTheOne = IsThisTheOne || myItem.Category.ItemDepartmentId.Contains(MagixWord);

                if (!IsThisTheOne) continue;

                DataTable tmpDT = Registrar[Counter].CastMagix(ShowNormalTransaction, ShowUndeductedSales, ShowUnassignedError);
                if (Dt == null)
                {
                    Dt = tmpDT;
                }
                else
                {
                    for (int inCounter = 0; inCounter < tmpDT.Rows.Count; inCounter++)
                    {
                        Dt.Rows.Add(tmpDT.Rows[inCounter].ItemArray);
                    }
                }
            }

            return Dt;
        }
    }
}
