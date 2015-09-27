using System;

namespace Hardly {
	public abstract class SqlColumnHeaders {
		public readonly string name;
		public readonly bool isPrimaryKey, isUnique, isNullable, isAutoIncrement;
		public readonly ulong charMaxLength;

		protected SqlColumnHeaders(string name, bool isNullable, ulong charMaxLength, bool isPrimaryKey, bool isAutoIncrement, bool isUnique) {
			this.name = name;
			this.isNullable = isNullable;
			this.charMaxLength = charMaxLength;
			this.isPrimaryKey = isPrimaryKey;
			this.isAutoIncrement = isAutoIncrement;
			this.isUnique = isUnique;
		}

		public abstract Type DataType {
			get;
		}

      internal static SqlColumnHeaders FromSql(string name, bool isNullable, ulong charMaxLength, string dataType, bool isPrimaryKey, bool isUnique, bool isAutoIncrement) {
			string type = dataType.GetBefore("(");
			if(type == null) {
				type = dataType;
			}
			string precisionAndScale = dataType.GetBetween("(", ")");
			bool signed = !dataType.EndsWith("unsigned");

			switch(type) {
			case "bigint":
				if(signed) {
					return new SqlColumn<long>(name, isNullable, charMaxLength, isPrimaryKey, isAutoIncrement, isUnique);
				} else {
					return new SqlColumn<ulong>(name, isNullable, charMaxLength, isPrimaryKey, isAutoIncrement, isUnique);
				}
			case "int":
				if(signed) {
					return new SqlColumn<int>(name, isNullable, charMaxLength, isPrimaryKey, isAutoIncrement, isUnique);
				} else {
					return new SqlColumn<uint>(name, isNullable, charMaxLength, isPrimaryKey, isAutoIncrement, isUnique);
				}
			case "smallint":
				if(signed) {
					return new SqlColumn<short>(name, isNullable, charMaxLength, isPrimaryKey, isAutoIncrement, isUnique);
				} else {
					return new SqlColumn<ushort>(name, isNullable, charMaxLength, isPrimaryKey, isAutoIncrement, isUnique);
				}
			case "tinyint":
				if(signed) {
					return new SqlColumn<sbyte>(name, isNullable, charMaxLength, isPrimaryKey, isAutoIncrement, isUnique);
				} else {
					return new SqlColumn<byte>(name, isNullable, charMaxLength, isPrimaryKey, isAutoIncrement, isUnique);
				}
			case "bit":
				return new SqlColumn<bool>(name, isNullable, charMaxLength, isPrimaryKey, isAutoIncrement, isUnique);
			case "longtext":
			case "text":
			case "mediumblob":
			case "mediumtext":
			case "char":
			case "varchar":
			case "enum":
				return new SqlColumn<string>(name, isNullable, charMaxLength, isPrimaryKey, isAutoIncrement, isUnique);
			case "blob":
			case "longblob":
				return new SqlColumn<byte[]>(name, isNullable, charMaxLength, isPrimaryKey, isAutoIncrement, isUnique);
			case "datetime":
			case "time":
			case "timestamp":
				return new SqlColumn<DateTime>(name, isNullable, charMaxLength, isPrimaryKey, isAutoIncrement, isUnique);
			case "decimal":
			case "double":
			case "float":
			// TODO, currently not required..
			default:
				Log.error("Unknown Sql datatype " + dataType);

				return null;
			}
		}

		public override string ToString() {
			return name;
      }

		public override int GetHashCode() {
			return name.GetHashCode();
		}
	}

	internal sealed class SqlColumn<T> : SqlColumnHeaders {
		internal SqlColumn(string name, bool isNullable, ulong charMaxLength, bool isPrimaryKey, bool isAutoIncrement, bool isUnique) : base(name, isNullable, charMaxLength, isPrimaryKey, isAutoIncrement, isUnique) {
		}

		public override Type DataType {
			get {
				return typeof(T);
			}
		}
	}
}
