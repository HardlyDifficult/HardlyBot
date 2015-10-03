using System;

namespace Hardly {
	public abstract class SqlRow : PersistentDataList {
		#region Public interface
		public abstract SqlTable table {
			get;
		}
		#endregion

		#region Protected interface
		protected SqlRow(object[] values) : base(values) {
			if(values == null) {
				values = new object[(table.primaryKeyHeaders?.Length ?? 0) + (table.additionalKeyHeaders?.Length ?? 0)];
			}
			else if(values.Length == (table.primaryKeyHeaders?.Length ?? 0) + (table.additionalKeyHeaders?.Length ?? 0)) {
				// Do nothing
			} else {
				throw new ArgumentNullException();
			}
		}
		
		protected override bool SaveDataList(bool lazySave) {
			if(CheckIfAllPrimaryKeysAreDefined()) {
				if(CheckIfAllNonNullableKeysAreDefined()) {
					return AddOrUpdateRow(lazySave);
				} 
			} else if(CheckIfMissingAutoIncrementingKey()) {
					return InsertAndGetAutoIncrementingKey(lazySave);
			}
			
			return false;
		}

		protected override bool LoadDataList() {
			if(CheckIfAllPrimaryKeysAreDefined()) {
				return RefreshColumnsByPrimaryKey();
			} else {
				uint? definedUniqueKey = null;
				for(uint i = 0; i < (table.additionalKeyHeaders?.Length ?? 0); i++) {
					if(table.additionalKeyHeaders[i].isUnique && !values[i + (table.primaryKeyHeaders?.Length ?? 0)].IsDefaultValue()) {
						definedUniqueKey = (uint)(i + (table.primaryKeyHeaders?.Length ?? 0));
					}
				}

				bool changed = false;
				if(definedUniqueKey.HasValue) {
					changed = RefreshColumnsByUniqueKey(definedUniqueKey.Value);
				} 

				if(!changed && CheckIfMissingAutoIncrementingKey()) {
					changed = InsertAndGetAutoIncrementingKey(false);
				}

				return changed;
			}
		}

		protected override bool Set(uint valueIndex, object value) {
			return base.Set(valueIndex, TypeHelpers.FromSql((
				valueIndex < (table.primaryKeyHeaders?.Length ?? 0) ? table.primaryKeyHeaders[valueIndex] : table.additionalKeyHeaders[valueIndex - (table.primaryKeyHeaders?.Length ?? 0)]
				).DataType, value));
		}
		#endregion

		#region Private helpers
		private bool CheckIfMissingAutoIncrementingKey() {
			if((table.primaryKeyHeaders?.Length ?? 0) == 1 && table.primaryKeyHeaders[0].isAutoIncrement && values[0].IsDefaultValue() && (table.additionalKeyHeaders?.Length ?? 0) > 0) {
				for(int i = 0; i < table.additionalKeyHeaders.Length; i++) {
					if(!table.additionalKeyHeaders[i].isNullable && values[i + 1].IsDefaultValue()) {
						return false;
					}
				}

				return true;
			}
			
			return false;
		}

		private bool CheckIfAllPrimaryKeysAreDefined() {
			bool allPrimaryKeysDefined = false;
			if((table.primaryKeyHeaders?.Length ?? 0) > 0) {
				allPrimaryKeysDefined = true;
				for(int i = 0; i < table.primaryKeyHeaders.Length; i++) {
					if(values[i].IsDefaultValue()) {
						allPrimaryKeysDefined = false;
					}
				}
			}

			return allPrimaryKeysDefined;
		}

		private bool CheckIfAllNonNullableKeysAreDefined() {
			if((table.additionalKeyHeaders?.Length ?? 0) > 0) {
				bool allDefined = true;
				for(int i = table.primaryKeyHeaders?.Length ?? 0; i < table.additionalKeyHeaders.Length + (table.primaryKeyHeaders?.Length ?? 0); i++) {
					if(!table.additionalKeyHeaders[i - (table.primaryKeyHeaders?.Length ?? 0)].isNullable && values[i] == null) {
						allDefined = false;
					}
				}

				return allDefined;
			}

			return false;
		}

		private bool InsertAndGetAutoIncrementingKey(bool lazySave) {
			return SqlController.AddOrUpdatedAndGetAutoIncrementingKey(table, ref values, lazySave);
		}

		private bool AddOrUpdateRow(bool lazySave) {
			return SqlController.AddOrUpdate(table, ref values, lazySave);
		}

		private bool RefreshColumnsByUniqueKey(uint value) {
			string whereClause = table.additionalKeyHeaders[value - (table.primaryKeyHeaders?.Length ?? 0)].name + "=?a";
			List<object[]> newValues = SqlController.Select(table.tableName, null, "*", whereClause, new[] { values[value] }, null, 1);
			if(newValues != null && newValues.Count > 0) {
				return Set(newValues[0]);
			}

			Debug.Fail();
			return false;
		}

		private bool RefreshColumnsByPrimaryKey() {
			object[] newValues = table.SelectByPrimaryKey(values.SubArray(0, (uint)(table.primaryKeyHeaders?.Length ?? 0)));
			if(newValues != null && newValues.Length > 0) {
				return Set(newValues);
			}
			
			return false;
		}
		#endregion
	}
}
