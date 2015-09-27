using System;

namespace Hardly {
	public abstract class DataList {
		protected object[] values;

		protected DataList(object[] values) {
			this.values = values;
		}

		public override bool Equals(object obj) {
			bool same = false;

			if(obj != null && obj.GetType().Equals(GetType())) {
				DataList otherEntity = (DataList)obj;

				if(values.Length == otherEntity.values.Length) {
					same = true;
					for(int i = 0; i < values.Length; i++) {
						if((values[i].IsDefaultValue() && !otherEntity.values[i].IsDefaultValue())
								|| (!values[i].IsDefaultValue() && !values[i].Equals(otherEntity.values[i]))) {
							same = false;
						}
					}
				}
			}

			return same;
		}

		public override int GetHashCode() {
			int result = 0;
			foreach(var value in values) {
				if(value != null) {
					result = 37 * result + value.GetHashCode();
				}
			}
         return result;
		}
		
		protected virtual T Get<T>(uint index) {
			if(index < values.Length) {
				if(values[index] != null && !values[index].GetType().Equals(typeof(DBNull))) {
					return (T)values[index];
				}
			} 

			return (T)typeof(T).GetDefaultValue();
		}

		public bool Set(object[] newValues) {
			bool changed = false;

			if(newValues != null && newValues.Length == values.Length) {
				for(uint i = 0; i < values.Length; i++) {
					if(Set(i, newValues[i])) {
						changed = true;
					}
				}
			} else {
				Debug.Fail();
			}

			return changed;
		}

		protected virtual bool Set(uint valueIndex, object value) {
			if((values[valueIndex] != null && !values[valueIndex].Equals(value)) 
				|| (values[valueIndex] == null && value != null)) {
				values[valueIndex] = value;
				return true;
			}

			return false;
		}
	}
}
