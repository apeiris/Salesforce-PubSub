using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace mySalesforce {
	#region SqlServerLib.ctor
	public class SqlServerLib {
		private readonly string? _connectionString;
		private readonly ILogger<SqlServerLib> _l;

		public SqlServerLib(IConfiguration configuration, ILogger<SqlServerLib> logger) {
			if (configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			_connectionString = configuration.GetConnectionString("mssql");
			_l = logger ?? throw new ArgumentNullException(nameof(logger));
			if (string.IsNullOrWhiteSpace(_connectionString))
				throw new InvalidOperationException("Connection string 'mssql' is missing or empty in configuration.");
		}
		#endregion SqlServerLib.ctor
		#region Public Methods
		public DataTable GetAll_sfoTables() {
			DataTable dataTable = new DataTable();
			try {
				using (SqlConnection connection = new SqlConnection(_connectionString)) {
					string query = @"
                        SELECT name = o.name  
						FROM sys.objects o
                        JOIN sys.schemas s ON o.schema_id = s.schema_id
                        WHERE type = 'U' and s.Name= 'sfo'";
					using (SqlCommand command = new SqlCommand(query, connection)) {
						connection.Open();
						using (SqlDataAdapter adapter = new SqlDataAdapter(command)) {
							adapter.Fill(dataTable);
						}
					}
				}
			} catch (SqlException ex) {
				Console.WriteLine($"SQL Error: {ex.Message}");
				throw;
			} catch (Exception ex) {
				Console.WriteLine($"Error: {ex.Message}");
				throw;
			}
			return dataTable;
		}

		public string CreateTable(string tableName, string schema = "sfo") {
			if (string.IsNullOrWhiteSpace(tableName))
				throw new ArgumentException("Table name cannot be null or empty.", nameof(tableName));

			string sql = $@"IF NOT EXISTS(
							 SELECT 1
							 FROM sys.tables t
							 JOIN sys.schemas s ON t.schema_id = s.schema_id
							WHERE t.name = @TableName AND s.name = @SchemaName
							)
					BEGIN
						EXEC('CREATE TABLE [{schema}].[{tableName}]
							(Id INT IDENTITY(1, 1) PRIMARY KEY,
							CreatedAt DATETIME DEFAULT GETDATE()
							)')
						END";
			_l.LogDebug($"Sql = {sql}");
			try {
				using (SqlConnection conn = new SqlConnection(_connectionString)) {
					conn.Open();
					using (SqlCommand cmd = new SqlCommand(sql, conn)) {
						cmd.Parameters.AddWithValue("@SchemaName", schema);
						cmd.Parameters.AddWithValue("@TableName", tableName);
						cmd.ExecuteNonQuery();
					}
				}
			} catch (SqlException ex) {
				_l.LogError(ex, $"SQL Error: {ex.Message}");
				throw;
			} catch (Exception ex) {
				_l.LogError(ex, $"Error: {ex.Message}");
				throw;
			}

			_l.LogDebug($"CreateTable SQL:\n {sql}");



			//using (SqlConnection connection = new SqlConnection(_connectionString)) {
			//	connection.Open();
			//	using (SqlCommand command = new SqlCommand()) {
			//		command.Connection = connection;
			//		command.CommandText = $"CREATE TABLE {tableName} (";
			//		foreach (DataColumn column in schema.Columns) {
			//			command.CommandText += $"{column.ColumnName} {GetSqlType(column.DataType)}, ";
			//		}
			//		command.CommandText = command.CommandText.TrimEnd(',', ' ') + ")";
			//		command.ExecuteNonQuery();
			//	}
			//}
			return sql;
		}
		#endregion	Public Methods
	}
	#region Extensions
	
	public static class SqlServerLibExtensions {
		public static void AddIdentityColumn(this DataTable table,
												string columnName = "Id",
												int seed = 1,
												int step = 1,
												bool setAsPrimary = true) {
			if (table.Columns.Contains(columnName))
				throw new ArgumentException($"Column '{columnName}' already exists in the DataTable.");
			DataColumn identityColumn = new DataColumn(columnName, typeof(int)) {
				AutoIncrement = true,
				AutoIncrementSeed = seed,
				AutoIncrementStep = step
			};
			table.Columns.Add(identityColumn);
			if (setAsPrimary) {
				table.PrimaryKey = new DataColumn[] { identityColumn };
			}
		}
		public static DataTable ExcludeRegistered(this DataTable sfObjects,
													   DataTable dtRegistered,
													   string columnName = "Name") {
			if (sfObjects == null) throw new ArgumentNullException(nameof(sfObjects));
			if (dtRegistered == null) throw new ArgumentNullException(nameof(dtRegistered));
			if (!sfObjects.Columns.Contains(columnName) || !dtRegistered.Columns.Contains(columnName))
				throw new ArgumentException($"Column '{columnName}' must exist in both DataTables.");
			var result = sfObjects.Clone();
			var sfNames = sfObjects.AsEnumerable()
									.Select(row => row.Field<string>(columnName))
									.ToHashSet();
			var registeredNames = dtRegistered.AsEnumerable()
								  .Select(row => row.Field<string>(columnName))
								  .ToHashSet();
			var onlyInSource = sfObjects.AsEnumerable()
									 .Where(row => !registeredNames.Contains(row.Field<string>(columnName)));
			var onlyInOther = dtRegistered.AsEnumerable()
								   .Where(row => !sfNames.Contains(row.Field<string>(columnName)));

			foreach (var row in onlyInSource.Concat(onlyInOther)) {
				result.ImportRow(row);
			}
			return result;
		}
		public static void ImportAllRowsFrom(this DataTable target, DataTable source) {
			foreach (DataRow row in source.Rows) target.ImportRow(row);
		}
		public static string GenerateDDL(this DataSet dataSet) {
			StringBuilder ddl = new StringBuilder();

			// Drop tables if they exist (in reverse order to avoid FK conflicts)
			for (int i = dataSet.Tables.Count - 1; i >= 0; i--) {
				var table = dataSet.Tables[i];
				ddl.AppendLine($"IF OBJECT_ID('{table.TableName}', 'U') IS NOT NULL DROP TABLE {table.TableName};");
			}

			// Create tables
			foreach (DataTable table in dataSet.Tables) {
				ddl.AppendLine($"CREATE TABLE {table.TableName} (");

				// Columns
				for (int i = 0; i < table.Columns.Count; i++) {
					var column = table.Columns[i];
					string columnDef = $"    {column.ColumnName} {GetSqlType(column)}";

					// Handle defaults
					if (column.DefaultValue != DBNull.Value && column.DefaultValue != null) {
						string defaultValue = FormatDefaultValue(column);
						columnDef += $" DEFAULT {defaultValue}";
					}

					// Handle nullability
					if (!column.AllowDBNull && !IsPrimaryKey(column, table)) {
						columnDef += " NOT NULL";
					}

					if (i < table.Columns.Count - 1)
						columnDef += ",";

					ddl.AppendLine(columnDef);
				}

				// Primary Key
				if (table.PrimaryKey.Length > 0) {
					var pkColumns = string.Join(", ", Array.ConvertAll(table.PrimaryKey, c => c.ColumnName));
					ddl.AppendLine($"    CONSTRAINT PK_{table.TableName} PRIMARY KEY ({pkColumns})");
				}

				ddl.AppendLine(");");
			}

			// Foreign Keys
			foreach (DataRelation relation in dataSet.Relations) {
				var parentTable = relation.ParentTable.TableName;
				var childTable = relation.ChildTable.TableName;
				var parentColumn = relation.ParentColumns[0].ColumnName;
				var childColumn = relation.ChildColumns[0].ColumnName;

				ddl.AppendLine($"ALTER TABLE {childTable}");
				ddl.AppendLine($"ADD CONSTRAINT FK_{childTable}_{parentTable} FOREIGN KEY ({childColumn})");
				ddl.AppendLine($"REFERENCES {parentTable} ({parentColumn});");
			}

			return ddl.ToString();
		}

		#region helpers
		static string GetSqlType(DataColumn column) {
			Type dataType = column.DataType;
			if (dataType == typeof(int))
				return "INT";
			if (dataType == typeof(string))
				return $"NVARCHAR({(column.MaxLength > 0 ? column.MaxLength : 255)})";
			if (dataType == typeof(DateTime))
				return "DATETIME";
			if (dataType == typeof(decimal))
				return "DECIMAL(18,2)";
			if (dataType == typeof(bool))
				return "BIT";
			return "NVARCHAR(255)"; // Fallback
		}

		static string FormatDefaultValue(DataColumn column) {
			if (column.DataType == typeof(string))
				return $"'{column.DefaultValue}'";
			if (column.DataType == typeof(DateTime))
				return "'2023-01-01'"; // Simplified for example
			if (column.DataType == typeof(decimal) || column.DataType == typeof(int))
				return column.DefaultValue.ToString();
			if (column.DataType == typeof(bool))
				return (bool)column.DefaultValue ? "1" : "0";
			return "''";
		}

		static bool IsPrimaryKey(DataColumn column, DataTable table) {
			return Array.Exists(table.PrimaryKey, pk => pk.ColumnName == column.ColumnName);
		}

		static void dummyTestforGitHub() {
			// This is a dummy test for GitHub
			Console.WriteLine("This is a dummy test for GitHub test1 .");
			Console.WriteLine("This is a dummy test for GitHub test 2.");
			Console.WriteLine("This is a dummy test for GitHub test 3.");
		Console.WriteLine("This is a dummy test for GitHub test 4.");
		}
		#endregion	helpers

	}





	#endregion Extensions
}