using System;

namespace Hardly {
	public class SqlTable {
		public readonly SqlColumnHeaders[] primaryKeyHeaders, additionalKeyHeaders;
		public readonly string tableName;

		public SqlTable(string tableName) {
			if(tableName != null && tableName.Trim().Length > 0) {
				this.tableName = tableName;

				SqlColumnHeaders[] columns = SqlController.GetColumns(tableName);
				if(columns != null && columns.Length > 0) {
					uint primaryKeyCount = 0;
					foreach(SqlColumnHeaders column in columns) {
						if(column.isPrimaryKey) {
							primaryKeyCount++;
						} else {
							break;
						}
					}

					if(primaryKeyCount > 0) {
						primaryKeyHeaders = new SqlColumnHeaders[primaryKeyCount];
					} else {
						primaryKeyHeaders = null;
					}

					long additionalKeyCount = columns.Length - primaryKeyCount;
					if(additionalKeyCount > 0) {
						additionalKeyHeaders = new SqlColumnHeaders[additionalKeyCount];
					} else {
						additionalKeyHeaders = null;
					}

					for(int i = 0; i < columns.Length; i++) {
						if(i < primaryKeyCount) {
							Debug.Assert(columns[i].isPrimaryKey);

							primaryKeyHeaders[i] = columns[i];
						} else {
							Debug.Assert(!columns[i].isPrimaryKey);

							additionalKeyHeaders[i - primaryKeyCount] = columns[i];
						}
					}
				} else {
					throw new ArgumentNullException();
				}
			} else {
				throw new ArgumentNullException();
			}
		}

		public object[] SelectByPrimaryKey(object[] primaryKeyValues) {
			if(primaryKeyValues != null && primaryKeyValues.Length == (primaryKeyHeaders?.Length ?? 0)) {
				string whereClause = "";
				for(uint i = 0; i < (primaryKeyHeaders?.Length ?? 0); i++) {
					if(i > 0) {
						whereClause += " and ";
					}
					whereClause += primaryKeyHeaders[i].name + "=?" + i.ToStringAzViaMod();
				}
				if(whereClause.Length > 0) {
					return Select(null, null, whereClause, primaryKeyValues, null);
				}
			}

			Debug.Fail();
			return null;
		}

		public object[] Select(string join, string what, string whereClause, object[] vars, string orderBy) {
			return Select(join, what, whereClause, vars, orderBy, 1)?[0];
		}

		public List<object[]> Select(string join, string what, string whereClause, object[] vars, string orderBy, uint limit) {
			return SqlController.Select(tableName,
				join,
				what ?? (join == null ? "*" : tableName + ".*"),
				whereClause,
				vars,
				orderBy,
				limit);
		}

		public bool Update(string join, string set, string where, object[] vars) {
			return SqlController.Update(this, join, set, where, vars);
		}
   }
}
