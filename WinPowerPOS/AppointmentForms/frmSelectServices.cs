using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using PowerPOS;

namespace WinPowerPOS.AppointmentForms
{
	public partial class frmSelectServices : Form
	{
		private const string _query = "SELECT * FROM ViewItem WHERE DELETED = 0 AND IsServiceItem = 1 AND IsInInventory <> 1 AND CategoryName <> 'SYSTEM'";
		private const string _sortingColumn = "ItemName";

		public Dictionary<string, Item> SelectedItems = new Dictionary<string, Item>();

		public frmSelectServices()
		{
			InitializeComponent();
			dgvItemList.AutoGenerateColumns = false;
			UpdateControls();

			pagingControl.DataGrid = dgvItemList;
			pagingControl.SetQuery(_query, _sortingColumn);
		}

		private void UpdateControls()
		{
			btnOk.Enabled = SelectedItems.Count > 0;
		}

		private void frmAddMember_Load(object sender, EventArgs e)
		{
			try
			{
				ActiveControl = tbSearch;
				tbSearch.Focus();
			}
			catch (Exception exception)
			{
				frmError.ShowError(this, exception);
			}
		}

		private void btSearch_TextChanged(object sender, EventArgs e)
		{
			try
			{
				tmrFilter.Stop();
				tmrFilter.Start();
			}
			catch (Exception exception)
			{
				frmError.ShowError(this, exception);
			}
		}

		private void tmrFilter_Tick(object sender, EventArgs e)
		{
			try
			{
				tmrFilter.Stop();

				var query = new StringBuilder(_query);

				query.Append(" AND (");

				var parts = tbSearch.Text.Split(new[] { ' ', '.', ',', ';', ':', '-', '\t', '/' });
				for (int i = 0; i < parts.Length; i++)
				{
					var part = parts[i];

					if (i > 0) query.Append(" AND ");
					query.Append("(ISNULL(CategoryName,'') + " +
					             "ISNULL(ItemNo,'') + " +
					             "ISNULL(ItemName,'') + " +
					             "ISNULL(Barcode,'') + " +
					             "ISNULL(DepartmentName,'') " +
					             "like '%" + part + "%')");
				}

				query.Append(")");

				pagingControl.SetQuery(query.ToString(), _sortingColumn);

				int j = 0;
				foreach (DataGridViewRow row in dgvItemList.Rows)
				{
					var itemNo = row.Cells[colItemNo.Index].Value.ToString();
					if (SelectedItems.ContainsKey(itemNo))
						dgvItemList.Rows[j].Cells[cbSelect.Index].Value = true;
					j++;
				}
			}
			catch (Exception exception)
			{
				frmError.ShowError(this, exception);
			}
			finally
			{
				UpdateControls();
			}
		}

		private void AddSelectedItem(int rowIndex)
		{
			var item = new Item(dgvItemList.Rows[rowIndex].Cells[colItemNo.Index].Value.ToString());
			if (!SelectedItems.ContainsKey(item.ItemNo))
			{
				SelectedItems.Add(item.ItemNo, item);
				var cbCell = dgvItemList.Rows[rowIndex].Cells[0] as DataGridViewCheckBoxCell;
				if (cbCell != null)
					cbCell.Value = true;
			}
		}

		private void RemoveSelectedItem(int rowIndex)
		{
			var item = new Item(dgvItemList.Rows[rowIndex].Cells[colItemNo.Index].Value.ToString());
			if (SelectedItems.ContainsKey(item.ItemNo))
			{
				SelectedItems.Remove(item.ItemNo);
				var cbCell = dgvItemList.Rows[rowIndex].Cells[0] as DataGridViewCheckBoxCell;
				if (cbCell != null)
					cbCell.Value = null;
			}
		}

		private void dgvItemList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (e.ColumnIndex > 0)
				{
					AddSelectedItem(e.RowIndex);
					UpdateControls();
					DialogResult = DialogResult.OK;
				}
			}
			catch (Exception exception)
			{
				frmError.ShowError(this, exception);
			}
		}

		private void dgvItemList_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			
		}

		public void Clear()
		{
			SelectedItems.Clear();
			UpdateControls();
		}

        private void dgvItemList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 0)
                {
                    var cbCell = dgvItemList.Rows[e.RowIndex].Cells[0] as DataGridViewCheckBoxCell;
                    if (cbCell != null)
                    {
                        if (cbCell.Value == null)
                        {
                            //Logger.writeLog("Add " + e.RowIndex.ToString());
                            AddSelectedItem(e.RowIndex);
                        }
                        else
                        {
                            //Logger.writeLog("Remove " + e.RowIndex.ToString());
                            RemoveSelectedItem(e.RowIndex);
                        }

                        UpdateControls();
                    }
                }
            }
            catch (Exception exception)
            {
                frmError.ShowError(this, exception);
            }
        }
	}
}
