using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Hardly {
	internal static class SqlController {
		static readonly string connectionString = File.ReadAllLines("UidAndPwd.txt") + ";useAffectedRows=true;";

		internal static bool AddOrUpdatedAndGetAutoIncrementingKey(SqlTable table, ref object[] allColumnValues, bool lazy) {
			if(table != null && allColumnValues != null && allColumnValues.Length > 1 && allColumnValues[0].IsDefaultValue()) {
				try {
					Debug.Assert(allColumnValues.Length == (table.primaryKeyHeaders?.Length ?? 0) + (table.additionalKeyHeaders?.Length ?? 0));
					Debug.Assert(table.primaryKeyHeaders[0].isAutoIncrement);

					NullNullableDefaultValues(table, ref allColumnValues);

					string[] vars = GetVars(allColumnValues.Length - 1);

					string sql = "insert into " + table.tableName;
					sql += " (" + table.additionalKeyHeaders.ToCsv() + ") values (";
					sql += vars.ToCsv(",", "?");
					sql += ")ON DUPLICATE KEY UPDATE " + table.primaryKeyHeaders[0] + "=last_insert_id(" + table.primaryKeyHeaders[0] + "),";
					sql += table.additionalKeyHeaders?.AppendStrings(vars, "=?").ToCsv();
					sql += ";select last_insert_id()";

					allColumnValues[0] = ExecuteScalar(sql, allColumnValues.SubArray(1));

					return allColumnValues[0] != null;
				} catch(Exception e) {
					Log.exception(e);
				}
			}

			Debug.Fail();
			return false;
		}

		internal static bool AddOrUpdate(SqlTable table, ref object[] allColumnValues, bool lazy) {
			if(table != null && allColumnValues != null && allColumnValues.Length > 0) {
				try {
					Debug.Assert(allColumnValues.Length == (table.primaryKeyHeaders?.Length ?? 0) + (table.additionalKeyHeaders?.Length ?? 0));

					NullNullableDefaultValues(table, ref allColumnValues);

					string[] vars = GetVars((table.primaryKeyHeaders?.Length ?? 0) + (table.additionalKeyHeaders?.Length ?? 0));
					string sql = "insert into " + table.tableName;
					sql += " (" + table.primaryKeyHeaders.Append(table.additionalKeyHeaders).ToCsv();
					sql += ") values (" + vars.ToCsv(",", "?") + ")";
					sql += " ON DUPLICATE KEY UPDATE ";
					string[] keyValues = table.additionalKeyHeaders?.AppendStrings(vars.SubArray((uint)(table.primaryKeyHeaders?.Length ?? 0)), "=?");
					sql += keyValues.ToCsv();

					return ExecuteNonQuery(sql, allColumnValues) == 1;
				} catch(Exception e) {
					Log.exception(e);
				}
			}

			Debug.Fail();
			return false;
		}

		internal static bool Delete(SqlTable table, string join, string where, object[] vars, bool lazy) {
			if(table != null && where != null && vars != null) {
				try {
					Debug.Assert(where.Trim().Length > 0);
					Debug.Assert(vars.Length > 0);

					string sql = "delete from " + table.tableName;
					if(join != null) {
						sql += " " + join;
					}
					sql += " where " + where;

					return ExecuteNonQuery(sql, vars) > 0;
				} catch(Exception e) {
					Log.exception(e);
				}
			}

			Debug.Fail();
			return false;
		}

		internal static SqlColumnHeaders[] GetColumns(string tableName) {
			if(tableName != null) {
				Debug.Assert(tableName.Length > 0 && tableName.IsLowercase() && tableName.IsTrimmed());

				try {
					object[][] columnObjects = Select("information_schema.columns",
						null,
						 "column_name, is_nullable='YES', character_maximum_length, column_type, column_key='PRI', column_key='UNI', extra='auto_increment'",
						 "table_name=?a",
						 new string[] { tableName },
						 "ordinal_position",
						 0
						 );

					if(columnObjects != null) {
						SqlColumnHeaders[] columns = new SqlColumnHeaders[columnObjects.Length];
						for(int i = 0; i < columnObjects.Length; i++) {
							columns[i] = SqlColumnHeaders.FromSql((string)columnObjects[i][0],
								 (long)columnObjects[i][1] > 0,
								 columnObjects[i][2].GetType().Equals(typeof(DBNull)) ? 0 : (ulong)columnObjects[i][2],
								 (string)columnObjects[i][3],
								 (long)columnObjects[i][4] > 0,
								 (long)columnObjects[i][5] > 0,
								 (long)columnObjects[i][6] > 0);
						}

						return columns;
					}
				} catch(Exception e) {
					Log.exception(e);
				}
			}

			Debug.Fail();
			return null;
		}

		internal static object[][] Select(string tableName, string join, string what, string whereClause, object[] vars, string orderBy, uint limit) {
			if(tableName != null) {
				try {
					List<object[]> resultsList = new List<object[]>();

					string sql = "select ";
					sql += what ?? "*";
					sql += " from " + tableName;
					if(join != null) {
						sql += " " + join;
					}
					if(whereClause != null) {
						sql += " where " + whereClause;
					}
					if(orderBy != null) {
						sql += " order by " + orderBy;
					}
					if(limit > 0) {
						sql += " limit " + limit;
					}
					ExecuteReader(sql, vars, resultsList);

					if(resultsList.Count > 0) {
						return resultsList.ToArray();
					} 
				} catch(Exception e) {
					Log.exception(e);
				}
			}
			
			return null;
		}
		
		internal static bool Update(SqlTable table, string join, string set, string where, object[] vars) {
			if(table != null && set != null && where != null && vars != null) {
				try {
					Debug.Assert(set.Trim().Length > 0);
					Debug.Assert(where.Trim().Length > 0);
					Debug.Assert(vars.Length > 0);

					string sql = "update " + table.tableName;
					if(join != null) {
						sql += " " + join;
					}
					sql += " set " + set + " where " + where;

					return ExecuteNonQuery(sql, vars) > 0;
				} catch(Exception e) {
					Log.exception(e);
				}
			}

			Debug.Fail();
			return false;
		}

		#region Private helpers
		static int ExecuteNonQuery(string sql, object[] values) {
			Log.debug("Sql non-query: " + sql);

			using(MySqlConnection connection = new MySqlConnection(connectionString)) {
				using(MySqlCommand command = new MySqlCommand(sql, connection)) {
					SetParameters(command, values);

					connection.Open();
					return command.ExecuteNonQuery();
				}
			}
		}

		static object ExecuteScalar(string sql, object[] values) {
			Log.debug("Sql scalar: " + sql);

			using(MySqlConnection connection = new MySqlConnection(connectionString)) {
				using(MySqlCommand command = new MySqlCommand(sql, connection)) {
					SetParameters(command, values);

					connection.Open();
					return command.ExecuteScalar();
				}
			}
		}

		static void ExecuteReader(string sql, object[] vars, List<object[]> resultsList) {
			Log.debug("Sql reader: " + sql);

			using(MySqlConnection connection = new MySqlConnection(connectionString)) {
				using(MySqlCommand command = new MySqlCommand(sql, connection)) {
					SetParameters(command, vars);
					connection.Open();
					using(MySqlDataReader reader = command.ExecuteReader()) {
						if(reader.HasRows) {
							while(reader.Read()) {
								object[] values = new object[reader.FieldCount];

								reader.GetValues(values);
								resultsList.Add(values);
							}
						}
					}
				}
			}
		}

		static void NullNullableDefaultValues(SqlTable table, ref object[] allValues) {
			for(int i = 0; i < (table.additionalKeyHeaders?.Length ?? 0); i++) {
				if(table.additionalKeyHeaders[i].isNullable && allValues[(table.primaryKeyHeaders?.Length ?? 0) + i].IsDefaultValue()) {
					allValues[(table.primaryKeyHeaders?.Length ?? 0) + i] = null;
				}
			}
		}

		static void SetParameters(MySqlCommand command, object[] values) {
			if(values != null) {
				for(uint i = 0; i < values.Length; i++) {
					command.Parameters.Add(new MySqlParameter("?" + i.ToStringAzViaMod(), values[i]));
				}
			}
		}

		static string[] GetVars(int count) {
			string[] variables = new string[count];
			for(uint i = 0; i < count; i++) {
				variables[i] = i.ToStringAzViaMod();
			}

			return variables;
		}
		#endregion
	}
}