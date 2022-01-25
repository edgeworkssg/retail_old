using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;
using System.Data;

namespace PowerPOS
{
    public partial class RoomController
    {
        public static string listboxDisplayedColumnName = "Displayed";
        public static string listboxValueColumnName = "Value";

        public DataTable FetchAll_forListView()
        {
            DataTable Output = new RoomCollection()
                .OrderByAsc(Room.Columns.OutletName)
                .OrderByAsc(Room.Columns.RoomName).Load().ToDataTable();

            for (int Counter = Output.Columns.Count - 1; Counter >= 0; Counter--)
                if (Output.Columns[Counter].ColumnName.ToLower().CompareTo(Room.Columns.RoomID.ToLower()) != 0 && Output.Columns[Counter].ColumnName.ToLower().CompareTo(Room.Columns.RoomName.ToLower()) != 0)
                    Output.Columns.RemoveAt(Counter);

            Output.Columns[Room.Columns.RoomID].ColumnName = listboxValueColumnName;
            Output.Columns[Room.Columns.RoomName].ColumnName = listboxDisplayedColumnName;

            DataRow Rw = Output.NewRow();
            Rw[listboxValueColumnName] = -1;
            Rw[listboxDisplayedColumnName] = "<Please Select>";

            Output.Rows.InsertAt(Rw, 0);

            return Output;
        }

        public DataTable FetchByOutlet_forListView(string OutletName)
        {
            DataTable Output = new RoomCollection()
                .Where(Room.Columns.OutletName, OutletName)
                .OrderByAsc(Room.Columns.OutletName)
                .OrderByAsc(Room.Columns.RoomName).Load().ToDataTable();

            for (int Counter = Output.Columns.Count - 1; Counter >= 0; Counter--)
                if (Output.Columns[Counter].ColumnName.ToLower().CompareTo(Room.Columns.RoomID.ToLower()) != 0 && Output.Columns[Counter].ColumnName.ToLower().CompareTo(Room.Columns.RoomName.ToLower()) != 0)
                    Output.Columns.RemoveAt(Counter);

            Output.Columns[Room.Columns.RoomID].ColumnName = listboxValueColumnName;
            Output.Columns[Room.Columns.RoomName].ColumnName = listboxDisplayedColumnName;

            DataRow Rw = Output.NewRow();
            Rw[listboxValueColumnName] = -1;
            Rw[listboxDisplayedColumnName] = "<Please Select>";

            Output.Rows.InsertAt(Rw, 0);

            return Output;
        }
    }
}
