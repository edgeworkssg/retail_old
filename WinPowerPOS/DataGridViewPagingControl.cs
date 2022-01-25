using System;
using System.Data;
using System.Windows.Forms;
using SubSonic;

namespace WinPowerPOS
{
	public partial class DataGridViewPagingControl : UserControl
	{
		private string _query;
		private string _sortingColumn;

		private int _currentPage;
		private int _pageCount;
		private int _queryCount;

		private DataGridView _dataGrid;
		private bool _autoGenerateColumns;
		private int _pageSize = 10;

		public string Query
		{
			get { return _query; }
		}

		public DataGridView DataGrid
		{
			get { return _dataGrid; }
			set
			{
				_dataGrid = value;
				SetPage(0);
			}
		}

		public string SortingColumn
		{
			get { return _sortingColumn; }
		}

		public int CurrentPage
		{
			get { return _currentPage; }
			set { SetPage(_currentPage); }
		}

		public int PageCount
		{
			get { return _pageCount; }
		}

		public int QueryCount
		{
			get { return _queryCount; }
		}

		public bool AutoGenerateColumns
		{
			get { return _autoGenerateColumns; }
			set { _autoGenerateColumns = value; }
		}

		public int PageSize
		{
			get { return _pageSize; }
			set
			{
				_pageSize = value;
				SetPage(0);
			}
		}

		public DataGridViewPagingControl()
		{
			InitializeComponent();
		}

		public void SetQuery(string query, string sortingField)
		{
			_query = query;
			_sortingColumn = sortingField;

			_queryCount = (int)DataService.ExecuteScalar(new QueryCommand(string.Format("SELECT COUNT(*) FROM ({0}) AS c", query), "PowerPOS"));
			_pageCount = (int) Math.Ceiling((double)_queryCount / _pageSize);
			SetPage(0);
		}

		public void SetPage(int page)
		{
			if (_dataGrid != null && !string.IsNullOrEmpty(_query) && !string.IsNullOrEmpty(_sortingColumn))
			{
				_currentPage = page;

				var pageSize = _pageSize;
				if (page == _pageCount - 1 && (_queryCount % _pageSize > 0))
					pageSize = _queryCount % _pageSize;

				var pageQuery = string.Format(
					"SELECT * FROM (SELECT TOP {2} * FROM (SELECT TOP {3} * FROM ({0}) as q ORDER BY {1}) AS r ORDER BY {1} DESC) AS g ORDER BY {1}",
					_query, _sortingColumn, pageSize, (_currentPage + 1) * _pageSize);

				var command = new QueryCommand(pageQuery, "PowerPOS");

				var reader = DataService.GetReader(command);
				var output = new DataTable();
				output.Load(reader);

				_dataGrid.AutoGenerateColumns = _autoGenerateColumns;
				_dataGrid.DataSource = null;
				_dataGrid.DataSource = output;
				

				btnPreviousPage.Enabled = _currentPage > 0;
				btnFirstPage.Enabled = _currentPage > 0;
				btnLastPage.Enabled = _currentPage != _pageCount - 1;
				btnNextPage.Enabled = _currentPage < _pageCount - 1;

				btnPage.Text = string.Format("Page {0} of {1}\nRecords {2} - {3} of {4}", _currentPage + 1, _pageCount,
				                             _currentPage * _pageSize + 1, _currentPage * _pageSize + pageSize, _queryCount);
			}
		}

		private void btnPreviousPage_Click(object sender, EventArgs e)
		{
			SetPage(_currentPage - 1);
		}

		private void btnFirstPage_Click(object sender, EventArgs e)
		{
			SetPage(0);
		}

		private void btnNextPage_Click(object sender, EventArgs e)
		{
			SetPage(_currentPage + 1);
		}

		private void btnLastPage_Click(object sender, EventArgs e)
		{
			SetPage(_pageCount - 1);
		}
	}
}
