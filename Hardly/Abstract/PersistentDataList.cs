namespace Hardly {
	public abstract class PersistentDataList : DataList {
		bool? pendingRead, pendingWrite;
		bool hasChangedDb, shouldSave;

		protected PersistentDataList(object[] values) : base(values) {
			pendingRead = null;
			pendingWrite = null;
			hasChangedDb = false;
			shouldSave = false;
		}

		~PersistentDataList() {
			if(shouldSave && pendingWrite.GetValueOrDefault(true)) {
				SaveDataList(true);
			}
		}

		public bool Save(bool lazySave = false) {
			bool hasChanged = false;

			if(!lazySave) {
				if(pendingWrite.GetValueOrDefault(true)) {
					pendingWrite = false;
					if(SaveDataList(false)) {
						hasChangedDb = true;
						pendingRead = false;
						hasChanged = true;
					}
				} else {
					shouldSave = true;
					hasChanged = pendingWrite.GetValueOrDefault(true);
				}
			}

			return hasChanged;
		}

		public bool Load() {
			bool hasChanged = false;
			
			if(pendingRead.GetValueOrDefault(true)) {
				pendingRead = false;
				pendingWrite = false;
				hasChanged = LoadDataList();
			}

			return hasChanged;
		}

		public bool HasChangedDb {
			get {
				Save(false);

				return hasChangedDb;
			}
		}

		protected abstract bool SaveDataList(bool lazySave);
		protected abstract bool LoadDataList();

		protected override T Get<T>(uint index) {
			T result = base.Get<T>(index);

			if(result.IsDefaultValue() && pendingRead.GetValueOrDefault(true)) {
				Load();
				result = base.Get<T>(index);
			}

			return result;
		}

		protected override bool Set(uint valueIndex, object value) {
			if(base.Set(valueIndex, value)) {
				pendingWrite = true;
				return true;
			} else {
				return false;
			}
		}
	}
}
