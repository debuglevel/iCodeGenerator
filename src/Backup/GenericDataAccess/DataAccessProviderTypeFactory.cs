using System;
using System.Data.SqlClient;
using System.Data.OleDb;
using MySQLDriverCS;
using System.Data.OracleClient;
using Npgsql;

namespace iCodeGenerator.GenericDataAccess
{
	public enum DataProviderType
	{
		SqlClient,
		MySql,
		Access,
		PostgresSql,
		Oracle,
		ODBC
	}

	public class DataAccessProviderTypeFactory
	{

		#region Attributes
		private DataProviderType _providerType;
		private Type[] _connectionType = new Type[] { 
								typeof(SqlConnection),
								typeof(MySQLConnection), 
								typeof(OleDbConnection), 
								typeof(NpgsqlConnection),
								typeof(OracleConnection)
								};
		private Type[] _commandType = new Type[] { 
								typeof(SqlCommand),
								typeof(MySQLCommand),
								typeof(OleDbCommand), 
								typeof(NpgsqlCommand),
								typeof(OracleCommand),
								 };
		private Type[] _dataAdapterType = new Type[] { 
														 typeof(SqlDataAdapter),
														 typeof(MySQLDataAdapter),
														 typeof(OleDbDataAdapter),
														 typeof(NpgsqlDataAdapter), 
														 typeof(OracleDataAdapter) 
		                                             };
		private Type[] _paramenterType = new Type[] { 
														typeof(SqlParameter),
														typeof(MySQLParameter), 
														typeof(OleDbParameter), 
														typeof(NpgsqlParameter),
														typeof(OracleParameter)
		                                            } ;						
		#endregion

		#region Constructor

		private DataAccessProviderTypeFactory()
		{
		}

		public DataAccessProviderTypeFactory(DataProviderType dataType)
		{
			_providerType = dataType;
		}

		public DataAccessProviderTypeFactory(string dataTypeName)
		{
			switch(dataTypeName.ToLower().Trim())
			{
				case "sqlclient":
					_providerType = DataProviderType.SqlClient;
					break;
				case "mysql":
					_providerType = DataProviderType.MySql;
					break;
				case "access":
					_providerType = DataProviderType.Access;
					break;
				case "postgressql":
					_providerType = DataProviderType.PostgresSql;
					break;
				case "oracle":
					_providerType = DataProviderType.Oracle;
					break;
			}	
		}
		#endregion

		#region Properties
		public Type ConnectionType
		{
			get { return _connectionType[(int)_providerType]; }
		}

		public Type CommandType
		{
			get{ return _commandType[(int)_providerType]; }
		}

		public Type DataAdapterType
		{
			get{ return _dataAdapterType[(int)_providerType]; }
		}

		public Type ParameterType
		{
			get{ return _paramenterType[(int)_providerType]; }
		}
		#endregion
	}
}
