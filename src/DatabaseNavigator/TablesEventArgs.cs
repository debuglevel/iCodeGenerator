using System;
using iCodeGenerator.DatabaseStructure;

namespace iCodeGenerator.DatabaseNavigator
{
	public class TablesEventArgs : EventArgs
	{

		private Table[] _tables;

		public Table[] Tables
		{
			get { return _tables; }
		}

		public TablesEventArgs(Table[] tables)
		{
			_tables = tables;
		}
	}
}
